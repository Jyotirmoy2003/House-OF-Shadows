using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightHolder : MonoBehaviour
{
    [SerializeField] Outline outline;
    [SerializeField] DialugeTrigger dt;
    [SerializeField] Light lightComponent;
    [SerializeField] GameObject lightOnObject;
    [SerializeField] GrabObjectReceiver reciver;

                    
   

   public void ListenWenObjectIsUded(Component sender,object data)
   {
        if(data is GrabObject && reciver.Recived)
        {
            SetLight(true);
        }
   }



    
   



   

    //fun to toggle lights outside of this class
    public void SetLight(bool toggle)
    {
        lightComponent.enabled = toggle;
        lightOnObject.SetActive(toggle);
    }
}

