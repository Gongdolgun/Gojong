using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationClipOverrides : MonoBehaviour
{
    public Animator animator;
    public AnimatorOverrideController animatorOverrideController;
    public float delay;
    public AudioSource audioSource;
    public int frame;
    public float a;
    public go_data gd;
    public float offset;
    public void Start()
    {
        animator.runtimeAnimatorController = animatorOverrideController;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            playAnimation(185);
        }
    }
    public void playAnimation(int _i)
    {
        string s = "s" + _i.ToString("0000");
        a = gd.getFrame(_i.ToString()) + offset;
        ResourceRequest request = Resources.LoadAsync("ani/" + _i);
        AnimationClip animClip = request.asset as AnimationClip;
        AudioClip ac = Resources.Load("mp3/" + s) as AudioClip;
        animatorOverrideController["A"] = animClip;
        animator.SetTrigger("shot");
        audioSource.clip = ac;
        audioSource.time = frame / 24f;
        audioSource.Play();
    }
}
