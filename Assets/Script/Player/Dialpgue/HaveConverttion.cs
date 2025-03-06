using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;


public class HaveConverttion : MonoBehaviour
{
    private DialugeTrigger Dt;



    void Start()
    {
        Dt=GetComponent<DialugeTrigger>();
    }
   
    public void SetName()
    {
        
        if(Dt.dialouge.name==null){return;}
    }
    
}
    
