using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using KKSpeech;

namespace UnityStandardAssets.Characters.ThirdPerson{
public class RecordingCanvas : MonoBehaviour {

	public Button startRecordingButton,btn;
	public InputField resultText;

	Dictionary<string,string> modelKeys;
	Dictionary<string,string> voice;

	private string modelkey;
	private string voiceType;

	string question;
		GameObject target;

	void Start() {

		modelKeys = new Dictionary<string,string> ();
		voice = new Dictionary<string,string> ();

		//set keys and voices for chaacters
		modelKeys.Add("salesgirl","e89bde30c4df474184629e61f9e83754");
		modelKeys.Add ("security", "c185fcc801ca49069c23e2271fed6fbf");
		modelKeys.Add("rsecurity", "ef5c8d622cc24b81866bf2f2d6b11f2b");

		voice.Add ("salesgirl","en-gb-x-rjs#female_3-local");
		voice.Add ("security", "en-us-x-sfg#male_3-local");
		voice.Add ("rsecurity", "en-us-x-sfg#male_3-local");

		if (SpeechRecognizer.ExistsOnDevice()) {
			SpeechRecognizerListener listener = GameObject.FindObjectOfType<SpeechRecognizerListener>();
			listener.onAuthorizationStatusFetched.AddListener(OnAuthorizationStatusFetched);
			listener.onAvailabilityChanged.AddListener(OnAvailabilityChange);
			listener.onErrorDuringRecording.AddListener(OnError);
			listener.onErrorOnStartRecording.AddListener(OnError);
			listener.onFinalResults.AddListener(OnFinalResult);
			listener.onPartialResults.AddListener(OnPartialResult);
			listener.onEndOfSpeech.AddListener(OnEndOfSpeech);
			startRecordingButton.enabled = false;
			SpeechRecognizer.RequestAccess();
		} else {
			resultText.text = "Sorry, but this device doesn't support speech recognition";
			startRecordingButton.enabled = false;
		}

	}

	public void OnFinalResult(string result) {
			resultText.text = result;
			// clickBttn();

			if (this.modelkey.Equals ("q")) {
				MyInteractionManager im = GameObject.Find ("GameManager").GetComponent<MyInteractionManager> ();
				bool correct = im.questionManager (this.question,result);
				if (correct) {
					im.globalScore += 5;
					Destroy (this.target);
				}
				return;
			}

        ReceiveResult r = new ReceiveResult();
		r.setModel (this.modelkey);
		r.setVoiceType (this.voiceType);
        r.Start();
        r.setAnswer(result);
        //Button btn = GameObject.Find("apibtn").GetComponent<Button>();
        //btn.onClick.Invoke();
		r.active();

    }

	public void OnPartialResult(string result) {
		resultText.text = result;
	}

	public void OnAvailabilityChange(bool available) {
		startRecordingButton.enabled = available;
		if (!available) {
			resultText.text = "Speech Recognition not available";
		} else {
			resultText.text = "Say something :-)";
		}
	}

	public void OnAuthorizationStatusFetched(AuthorizationStatus status) {
		switch (status) {
		case AuthorizationStatus.Authorized:
			startRecordingButton.enabled = true;
			break;
		default:
			startRecordingButton.enabled = false;
			resultText.text = "Cannot use Speech Recognition, authorization status is " + status;
			break;
		}
	}

	public void OnEndOfSpeech() {
		startRecordingButton.GetComponentInChildren<Text>().text = "Start Recording";
	}

	public void OnError(string error) {
		Debug.LogError(error);
		resultText.text = "Something went wrong... Try again! \n [" + error + "]";
		startRecordingButton.GetComponentInChildren<Text>().text = "Start Recording";
	}

	public void OnStartRecordingPressed(string name) {

		if (name.Equals ("cashier")) {
			this.modelkey = modelKeys ["salesgirl"];
			this.voiceType = voice ["security"];
		} else {
			this.modelkey = modelKeys [name];
			this.voiceType = voice [name];
		}



		if (SpeechRecognizer.IsRecording()) {
			SpeechRecognizer.StopIfRecording();
			startRecordingButton.GetComponentInChildren<Text>().text = "Start Recording";
		} else {
			SpeechRecognizer.StartRecording(true);
			startRecordingButton.GetComponentInChildren<Text>().text = "Stop Recording";
			resultText.text = "Say something :-)";
		}
	}

		public void OnQRecordingPressed(string question,GameObject target) {

			this.modelkey = "q";
			//this.voiceType = voice ["q"];
			this.question=question;
			this.target = target;

			if (SpeechRecognizer.IsRecording()) {
				SpeechRecognizer.StopIfRecording();
				startRecordingButton.GetComponentInChildren<Text>().text = "Start Recording";
			} else {
				SpeechRecognizer.StartRecording(true);
				startRecordingButton.GetComponentInChildren<Text>().text = "Stop Recording";
				resultText.text = "Say something :-)";
			}
		}

    public void clickBttn()
    {
        btn = GameObject.Find("Button").GetComponent<Button>();
        btn.onClick.Invoke();
    }
}
}