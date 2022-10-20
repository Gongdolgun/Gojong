using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class log : MonoBehaviour
{
    //음 높이에따라 반응하는거
    public float v;
    public float d;
    public float r;
    public Transform t;
    public Transform cube;
    public Material _light;
    public Image gage1;
    public Image gage2;

    float startValue = 0;
    float endValue = 0;
    public float damping = 10;
  
    void Update()
    {
        v = MicInput.MicLoudness  ;
        d =  Mathf.Clamp( MicInput.MicLoudnessinDecibels  + 100, 0, 1000) * 0.01f;
         
        t.localPosition = new Vector3(0,   d    , 0);
        //cube.localScale = Vector3.one * 7 * (d+1);
        _light.SetFloat("_EmissiveExposureWeight",   (1 - (d* d))   );
        startValue = d / 2;
        endValue = Mathf.Lerp(endValue, startValue, damping * Time.deltaTime);
 
            gage1.fillAmount = endValue;
            gage2.fillAmount = endValue;
      
        
    }
}
