using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    public GameObject cubeHorizontal;
    public float HSpeed;
    public GameObject cubeVertical;
    public float VSpeed;
    public GameObject moveR;
    public GameObject moveL;

    public GameObject rotateRoot;
    public float rotateRootSpeed;
    public GameObject rotateRTR;
    public GameObject rotateLTR;
    public GameObject moveTargetR;
    public GameObject moveTargetL;
    void Start()
    {
        moveTargetR = rotateRTR;
        moveTargetL = rotateLTR;
        //iTween.MoveTo(rotateRTR, iTween.Hash("x", 0.4f, "easetype","linear","time", 5, "looptype", "pingpong","islocal", true));
        //iTween.MoveTo(rotateLTR, iTween.Hash("x", -0.4f, "easetype", "linear", "looptype", "pingpong","islocal", true));
    }

    // Update is called once per frame
    void Update()
    {
        //rotateRoot.transform.Rotate(new Vector3(0, 0,  rotateRootSpeed * Time.deltaTime ));
        cubeHorizontal.transform.Rotate(new Vector3(0, HSpeed*Time.deltaTime, 0));
        cubeVertical.transform.Rotate(new Vector3( VSpeed * Time.deltaTime, 0, 0));
    }
}
