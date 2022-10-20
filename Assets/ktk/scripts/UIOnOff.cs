using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOnOff : MonoBehaviour
{
    public GameObject catlist;

    public bool Active = false;
    public void SetAxtive()
    {
        transform.localPosition = new Vector3(816, 557, 0);
        if (!Active)
        {
            iTween.MoveTo(catlist, iTween.Hash("x",  500, "islocal", true));

            Active = true;
        }
        else {
            
            iTween.MoveTo(catlist, iTween.Hash("x", 816, "islocal", true));
            Active = false;
        }
    }

     
}
