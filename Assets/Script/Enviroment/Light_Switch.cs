using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light_Switch : MonoBehaviour
{
    [SerializeField] float max,min;
    [SerializeField] Light roomLight;
    [SerializeField] Material mat;
    [SerializeField] Color baseColor;
    private float timmer=0;
    private bool mode=true;
    
    void Start()
    {
        timmer=Random.Range(min,max);
    }


    void Update()
    {
        timmer-=Time.deltaTime;
        FLickerLight();
    }

    void FLickerLight()
    {
       
        
        if(timmer<=0)
        {
            if(mat!=null)
            {
                if(mode)
                {
                    mat.SetColor("_EmissionColor",Color.black);
                }else{
                    mat.SetColor("_EmissionColor",baseColor);
                }
                mode=!mode;
                roomLight.enabled=!roomLight.enabled;
                
            }else{
            roomLight.enabled=!roomLight.enabled;
            }
            timmer=Random.Range(min,max);
            //play sound
        }
    }
}
