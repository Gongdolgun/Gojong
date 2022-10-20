using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class go_Sync : MonoBehaviour
{
    public Animator animator;
    public float delay;
    public AudioSource audioSource;
    public int frame;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            animator.SetTrigger("shot");
            audioSource.time = frame / 24f;
            audioSource.Play();
        }
    }

     
}
