using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;


public class go_controller : MonoBehaviour
{
    public AudioSource myAudio;
    public float a;
    public float offset;
    public AnimationClipOverrides aco;
    public InputField answer;
    public go_data gd;

    public void playAnswer()
    {
        PlayAnswer( int.Parse(answer.text));
    }
    public void PlayAnswer(int _i)
    {
        string s = "s" + _i.ToString("0000");
        
        //a = gd.getFrame(_i.ToString()) + offset;
        //Debug.Log(a);
        AudioClip ac = Resources.Load("mp3/" + s) as AudioClip;
         
        //aco.playAnimation(_i.ToString());
        myAudio.clip = ac;
        myAudio.time = a/24f;
        myAudio.Play();
    } 
}
