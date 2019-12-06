using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TTS : MonoBehaviour
{
    Button b;
    Text t;
  
    private AndroidJavaObject tts = null;
    private AndroidJavaObject activityContext = null;
     
    void Start()
    {

        b = GameObject.Find("Button").GetComponent<Button>();

        t = GameObject.Find("Text").GetComponent<Text>();
 
        b.onClick.AddListener(ac);
        t.text = "started";




    }

    public void ac()
    {
     
        try
        {
            t.text = "actions";

            // if (tts == null)
            // {
            t.text = "IFIN1";
            using (AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                activityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
                t.text = "USEACT";
            }

            using (AndroidJavaClass pluginClass = new AndroidJavaClass("com.example.ttsunity.MainClass"))
            {
                t.text = "USEPLGU";
                // if (pluginClass != null)
                // {
                t.text = "INSIDEPLUFID";
                tts = pluginClass.CallStatic<AndroidJavaObject>("instance");

                tts.Call("setContext", activityContext);
                activityContext.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    t.text = "INSIDEPLUFIDSSS";

                    String speak = "Hi How are you";

                    String aa = tts.Call<String>("getText", speak);
                    t.text = aa;



                }));
                t.text = "INSIDEPLUFIDENDS";
                //  }
                // else
                // {
                //    t.text = "Error2";
                //}
            }
            //}
            //else
            //{
            //    t.text = "Error1";
            //}

        }
        catch (Exception e)
        {
            t.text = e.Message;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
