using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
 
public class MicInput : MonoBehaviour
{

    #region SingleTon

    public static MicInput Inctance { set; get; }

    #endregion

    public static float MicLoudness;
    public static float MicLoudnessinDecibels;
    public int device;
    public DF2ClientAudioTester clientAudioTester;
    public string _device;
    public Text logtext;
    

    //mic initialization
    public void InitMic()
    {
        if (!micBlock)
        {
            _device = Microphone.devices[0];

            _clipRecord = Microphone.Start(_device, false, 300, 44100);
            startRecordingTime = Time.time;
            Debug.Log("InitMic");

            if (logtext) logtext.text = _device;
            _isInitialized = true;
            intervalTime = Time.time;
            micObj.SetColor("_BaseColor", Color.white);
            StartCoroutine(SetMic(1));
        }
        doQuestion.SetActive(true);
        doListen.SetActive(false);
    }

    public void StopMicrophone()
    {
        Debug.Log("StopMicrophone");
        Microphone.End(_device);
        _isInitialized = false;
         
    }

    AudioClip _clipRecord;
    AudioClip recordedAudioClip;
    AudioClip _recordedClip  ;
    public AudioSource audioPlayer;
    int _sampleWindow = 128;


    public int devices;
    public float startRecordingTime;
    public float startTalkingTime;
    public int offset;
    public float talkingTime;
    public int duration;
    public Material micObj;
    public MeshRenderer cuberendrer;

    public GameObject mic;
    public GameObject cube;
    public GameObject speaker;
    public GameObject haf;
    public Indicator indicator;

    public bool _isInitialized;

    public float intervalRecord = 240;
    public float intervalTime = 0;
    public lerp _lerp;
    public bool micBlock = false;
    public GameObject doQuestion;
    public GameObject doListen;
    public GameObject doWait;
     
    public void Start()
    {
        Application.runInBackground = true;
        RotateHaf();
        doListen.SetActive(false);
        doWait.SetActive(false);
    }
    public GameObject people;
    string stateText = "";
    float lastTime = 0;
    void SetMicBlock()
    {
        if (micBlock)
        {
            micBlock = false;
            //InitMic();
        }
        else
        {
            micBlock = true;
            StopMicrophone();
        }
    }
    private void Update()
    {
        if (Time.time > lastTime + 1f) //1초마다 파일 체크
        {
            string localpath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            string path = localpath + "/state.txt";

            StreamReader reader = new StreamReader(path);
            string aa = reader.ReadLine();
            reader.Close();
            if(stateText != aa)
            {
                if (aa == "1")
                {
                    //Debug.Log("kkkkkkkkkkkkkkkkkkkkkkkkkkk");
                    //micBlock = false;
                    //InitMic();
                    people.SetActive(false);
                }
                else {
                    //Debug.Log("oooooooooooooooooooooooo");
                    //micBlock = true;
                    //StopMicrophone();
                    people.SetActive(true);
                }
                stateText = aa;
            }
            
            lastTime = Time.time;
        }
    
        if (Input.GetKeyDown(KeyCode.W))
        {
            SetMicBlock();
        }
        if (!(_lerp.isLoud) || !(_lerp.isTalking))  //오디오 데이타 리셋위해 마이크 다시켬
        {
            if (Time.time > intervalTime + intervalRecord)
            {
                StopMicrophone();
                //InitMic();
            }
        }
        if (_isInitialized)
        {
            if (micObj) micObj.SetColor("_BaseColor", Color.white);

            cuberendrer.enabled = false;
        }
        else
        {
            if (micObj) micObj.SetColor("_BaseColor", Color.red);
            cuberendrer.enabled = true;
        }
        // levelMax equals to the highest normalized value power 2, a small number because < 1
        // pass the value to a static var so we can access it from anywhere
        MicLoudness = MicrophoneLevelMax();
        MicLoudnessinDecibels = MicrophoneLevelMaxDecibels();
    }
    public void StartRecord()
    {
        Debug.Log("StartRecord startTalkingTime");
        startTalkingTime = Time.time;
        
    }
    public void StopRecord()
    {
        //WaitingRecord.SetActive(false);
        //End the recording when the mouse comes back up, then play it
        StopMicrophone();
        _isInitialized = false;
        Debug.Log("StopRecord");
        offset = (int)((startTalkingTime - startRecordingTime - 1) * _clipRecord.frequency);
        if (offset < 0) offset = 0;
        duration = (int)((Time.time - startTalkingTime + 1) * _clipRecord.frequency);
        talkingTime = Time.time - startTalkingTime;
        //Trim the audioclip by the length of the recording
        AudioClip recordingNew = AudioClip.Create(_clipRecord.name + "_new", duration, _clipRecord.channels, _clipRecord.frequency, false);
        float[] data = new float[duration]; //버퍼
        _clipRecord.GetData(data, offset); //가져오기

        recordingNew.SetData(data, 0);   //저장
        StartCoroutine(SetMic(2));
        clientAudioTester.SendAudio(recordingNew);
        
    }
    public IEnumerator SetMic(int _i)
    {
        iTween.Stop(gameObject);
        RotateHaf();
        yield return new WaitForSeconds(1f);
        mic.SetActive(false);
        cube.SetActive(false);
        speaker.SetActive(false);
        if (_i == 1)
        {
            clientAudioTester.spoken.text = "";
            doQuestion.SetActive(true);
            doListen.SetActive(false);
            doWait.SetActive(false);
            mic.SetActive(true);
            indicator.rotateRootSpeed = 100;
        }
        else if (_i == 2)
        {
            doQuestion.SetActive(false);
            doListen.SetActive(false);
            doWait.SetActive(true);
            cube.SetActive(true);
            indicator.rotateRootSpeed = 150;
        }
        else if (_i == 3)
        {
            doQuestion.SetActive(false);
            doListen.SetActive(true);
            doWait.SetActive(false);
            speaker.SetActive(true);
            indicator.rotateRootSpeed = 50;
        }
    }
    public void RotateHaf()
    {
        haf.transform.rotation = Quaternion.identity;
        iTween.RotateAdd(haf, iTween.Hash("x", 360, "time", 1f, "easetype", "linear"   ));
    }
    int micPosition;
    float[] waveData = new float[128];

    //get data from microphone into audioclip
    float MicrophoneLevelMax()
    {

        float levelMax = 0;
        
        micPosition = Microphone.GetPosition(_device) - (_sampleWindow + 1); // null means the first microphone
        if (micPosition < 0) return 0;
        if (waveData == null) return 0;
        try {  
            _clipRecord.GetData(waveData, micPosition);
        }
        catch(InvalidCastException e)
        {
            Debug.Log(waveData.Length );
            Debug.Log(micPosition);
        }
        
        // Getting a peak on the last 128 samples
        for (int i = 0; i < _sampleWindow; i++)
        {
            float wavePeak = waveData[i] * waveData[i];
            if (levelMax < wavePeak)
            {
                levelMax = wavePeak;
            }
        }
        return levelMax;
         
        
    }
    
    //get data from microphone into audioclip
    float MicrophoneLevelMaxDecibels()
    {

        float db = 20 * Mathf.Log10(Mathf.Abs(MicLoudness)) ;

        return db;
    }

    public float FloatLinearOfClip(AudioClip clip)
    {
        StopMicrophone();

        _recordedClip = clip;

        float levelMax = 0;
        float[] waveData = new float[_recordedClip.samples];

        _recordedClip.GetData(waveData, 0);
        // Getting a peak on the last 128 samples
        for (int i = 0; i < _recordedClip.samples; i++)
        {
            float wavePeak = waveData[i] * waveData[i];
            if (levelMax < wavePeak)
            {
                levelMax = wavePeak;
            }
        }
        return levelMax;
    }

    public float DecibelsOfClip(AudioClip clip)
    {
        StopMicrophone();

        _recordedClip = clip;

        float levelMax = 0;
        float[] waveData = new float[_recordedClip.samples];

        _recordedClip.GetData(waveData, 0);
        // Getting a peak on the last 128 samples
        for (int i = 0; i < _recordedClip.samples; i++)
        {
            float wavePeak = waveData[i] * waveData[i];
            if (levelMax < wavePeak)
            {
                levelMax = wavePeak;
            }
        }

        float db = 20 * Mathf.Log10(Mathf.Abs(levelMax));

        return db;
    }



    

    
    // start mic when scene starts
    void OnEnable()
    {
        //InitMic();
        _isInitialized = true;
        Inctance = this;
    }

    //stop mic when loading a new level or quit application
    void OnDisable()
    {
        StopMicrophone();
    }

    void OnDestroy()
    {
        StopMicrophone();
    }


    // make sure the mic gets started & stopped when application gets focused
    void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            //Debug.Log("Focus");

            if (!_isInitialized)
            {
                //Debug.Log("Init Mic");
                //InitMic();
            }
        }
        if (!focus)
        {
            //Debug.Log("Pause");
            StopMicrophone();
            //Debug.Log("Stop Mic");

        }
    }
}
