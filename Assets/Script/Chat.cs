using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chat : MonoBehaviour
{
    public static Chat instance;
    void Awake() => instance = this;

    public InputField SendInput;
    public RectTransform ChatContent;
    public Text ChatText;
    public ScrollRect ChatScrollRect;
    public int dataNum = 100;
    public DF2ClientAudioTester df2;
    public int state = 1000;
    public GameObject come;
    public GameObject canQustion;
    public GameObject listen;
    public go_data gd;

    private void Start()
    {
        gd.QuestionRef();
    }

    private void Update()
    {
        if(state == 2000) {
            come.SetActive(false);

            if (!df2.isAnimation)
            {
                listen.SetActive(false);
                canQustion.SetActive(true);
            }

            else
            {
                listen.SetActive(true);
                canQustion.SetActive(false);
            }
        }

        else
        {
            come.SetActive(true);
            listen.SetActive(false);
            canQustion.SetActive(false);
        }
    }

    public void ShowMessage(string data)
    {
        Debug.Log(data);
        if (data.Contains(":"))
        {
            string[] dataArr = data.Split(':');

            dataNum = int.Parse(dataArr[1].Trim());
            Debug.Log(dataNum);
            ChatText.text += ChatText.text == "" ? data : "\n" + data;
        }

        if(dataNum == 1000) // 사람 없을 때
        {
            state = 1000;
        }

        else if(dataNum == 2000) // 사람 있을 때
        {
            state = 2000;
            
        }

        if(state == 1000)
        {
            dataNum = 0;
            
        }

        else if(state == 2000) 
        {
            if (!df2.isAnimation) 
            { 
                df2.playAnimation(dataNum);                
            }
        }
        Fit(ChatText.GetComponent<RectTransform>());
        Fit(ChatContent);
        Invoke("ScrollDelay", 0.03f);
    }

    void Fit(RectTransform Rect) => LayoutRebuilder.ForceRebuildLayoutImmediate(Rect);

    void ScrollDelay() => ChatScrollRect.verticalScrollbar.value = 0;

}
