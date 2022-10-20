using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectController : MonoBehaviour
{
    public int chatNum;
    public Chat chat;
    public DF2ClientAudioTester df2Audiotester;

    // Start is called before the first frame update
    void Start()
    {
        chatNum = chat.dataNum;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
