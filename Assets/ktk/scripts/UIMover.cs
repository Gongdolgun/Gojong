using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMover : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnEnable()
    {
        transform.localPosition = new Vector3(0, 0, 0);
        iTween.MoveFrom(gameObject, iTween.Hash("x", 100,"islocal", true));
    }
    private void OnDisable()
    {
        iTween.Stop();
    }
}
