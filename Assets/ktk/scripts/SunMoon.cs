using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunMoon : MonoBehaviour
{
     
    public Transform rotateObj;
    public float rotatespeed;
    public float movespeed;
    public GameObject move1;
    public GameObject move2;
    public GameObject move3;

    public GameObject move4;
    public GameObject move5;
    public GameObject move6;
    public bool Active = true;
    public SpriteRenderer[] moon;
    public SpriteRenderer[] sun;

    public void Update()
    {
        float k = Mathf.Sin(Time.time * movespeed);
        foreach (SpriteRenderer s in moon)
        {
            s.color = new Color(1, 1, 1, k );
        }
        foreach (SpriteRenderer s in sun)
        {
            s.color = new Color(1, 1, 1, -k);
        }
        if (Active)
        {
            rotateObj.Rotate(new Vector3(0, 0, rotatespeed * Time.deltaTime));
            

            float x1 = Mathf.Sin(Time.time * movespeed + 1f) / 2;
            float x2 = Mathf.Sin(Time.time * movespeed) / 2;
            float x3 = Mathf.Sin(Time.time * movespeed + 2f) / 2;
            float x4 = Mathf.Sin(Time.time * movespeed + 1.1f) / 2;
            float x5 = Mathf.Sin(Time.time * movespeed + 0.1f) / 2;
            float x6 = Mathf.Sin(Time.time * movespeed + 2.1f) / 2;
            if(move1) move1.transform.localPosition = new Vector3(x1, move1.transform.localPosition.y, move1.transform.localPosition.z);
            if (move2) move2.transform.localPosition = new Vector3(x2, move2.transform.localPosition.y, move2.transform.localPosition.z);
            if (move3) move3.transform.localPosition = new Vector3(x3, move3.transform.localPosition.y, move3.transform.localPosition.z);
            if (move4) move4.transform.localPosition = new Vector3(x4, move4.transform.localPosition.y, move4.transform.localPosition.z);
            if (move5) move5.transform.localPosition = new Vector3(x5, move5.transform.localPosition.y, move5.transform.localPosition.z);
            if (move6) move6.transform.localPosition = new Vector3(x6, move6.transform.localPosition.y, move6.transform.localPosition.z);
        }
    }
    
}
