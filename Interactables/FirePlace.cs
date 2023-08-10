using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePlace : MonoBehaviour
{
    [SerializeField] bool isFire=false;
    [SerializeField] Outline outline;
    [SerializeField] GrabObjectReceiver receiver;
    [SerializeField] ParticleSystem fireParticel;
    [SerializeField] Light pointLight;
    [SerializeField] GameObject skullObjectRecevir;

    void Start()
    {
        fireParticel.Pause();
        fireParticel.Stop();
        pointLight.enabled=false;
        skullObjectRecevir.SetActive(false);
    }
    

   

    void Update()
    {
        if(receiver.Recived && !isFire )
        {
            fireParticel.Play();
            pointLight.enabled=true;
            isFire=true;
            skullObjectRecevir.SetActive(true);
        }
    }





}

