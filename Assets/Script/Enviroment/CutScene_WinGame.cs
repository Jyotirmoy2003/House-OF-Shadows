using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutScene_WinGame : MonoBehaviour
{
    [SerializeField] Transform skullTransform;
    [SerializeField] float rotationSpeed=0.1f;
    [SerializeField] float increaseLight=0.2f;
    [SerializeField] Light skullLight;
    [SerializeField] float maxIntensity=5;
  



   


    void Update()
    {
        skullTransform.Rotate(0f,0f,rotationSpeed*Time.deltaTime, Space.Self);
        if(skullLight.intensity<maxIntensity)
        {
            skullLight.intensity+=increaseLight;
        }
    }
}
