using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class lerp : MonoBehaviour
{
    public Transform t;
    public bool isTalking;
    public bool isLoud;
    public float zerotime = 0;
    public float uptime = 0;
    public float zeroTolerance = 1f;
    public float upTolerance = 1f;
    public MicInput micInput;
    public GameObject micObj;
    
    public Text questionText;
    public float interval;
    public float last_change_time;
    public go_data gd;
    public Text logtext;
    public float level = .75f;
    float logtime;
    float logtime_delay = 3;
    public GameObject ui;
    public int uiState;
    public GameObject mountin;
    private void Start()
    {
        level = PlayerPrefs.GetFloat("level");
        uiState = PlayerPrefs.GetInt("uiState");
        Debug.Log("level  " + level);
        Debug.Log("uiState  " + uiState);
        if (uiState == 0)
        {
            ui.SetActive(false);
            Cursor.visible = false;
        }
        else
        {
            ui.SetActive(true);
            Cursor.visible = true; 
        }
        gd.QuestionRef(); 
    }
    

    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (mountin.activeSelf)
            {
                mountin.SetActive(false);
                 
            }
            else
            {
                mountin.SetActive(true);
                 
            }
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (ui.activeSelf)
            {
                ui.SetActive(false);
                Cursor.visible = false;
                PlayerPrefs.SetInt("uiState", 0);
            }
            else { 
                ui.SetActive(true);
                Cursor.visible = true;
                PlayerPrefs.SetInt("uiState", 1);
            }
            
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            level = level + .01f;
            logtext.text = "level = " + level.ToString();
            PlayerPrefs.SetFloat("level", level);
            logtime = Time.time;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            level = level - .01f;
            logtext.text = "level = " + level.ToString();
            PlayerPrefs.SetFloat("level", level);
            logtime = Time.time;
        }
        if (logtime_delay + logtime < Time.time)
        {
            logtext.enabled = false;
        }
        else
        {
            logtext.enabled = true;
        }
        if (interval + last_change_time < Time.time && micInput._isInitialized)
        {
            if (!isLoud) gd.QuestionRef();
            last_change_time = Time.time;
        }
        transform.position = Vector3.Lerp(transform.position, t.position, .1f);

        
         

        if (transform.position.y > level)
        {
            //micObj.transform.localScale = Vector3.one * 1.2f;
            if (!isLoud) //소리 들어올때
            {
                isLoud = true;
                
                if (!isTalking) //말안할때
                {
                    isTalking = true;
                    
                    if (micInput)
                    {
                         
                            uptime = Time.time;
                            micInput.StartRecord();
                         
                        
                        //Debug.Log("start talking      StartRecord");
                    }
                }
                else
                {
                    //Debug.Log("continue talking");
                }   
            }
            
        }
        else
        {
            //if (micObj) micObj.transform.localScale = Vector3.one  ;
            if (isLoud)
            {
                isLoud = false;
                zerotime = Time.time;
            }
            else
            {
                if (Time.time - zerotime > zeroTolerance && isTalking)
                {
                    //Debug.Log("stop talking");
                    isTalking = false;
                    if (Time.time - uptime > upTolerance)
                    {
                        if (micInput) micInput.StopRecord();
                        //Debug.Log("recorded    lerp    " + (Time.time - uptime));
                    }
                    else
                    {

                    }
                }
            }          
            
        }
    }
}
