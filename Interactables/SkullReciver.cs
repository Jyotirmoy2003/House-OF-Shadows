using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullReciver : MonoBehaviour
{
   [SerializeField] Transform skullObject;
   [SerializeField] GrabObjectReceiver receiver;
   [SerializeField] float rotationSpeed;
   [SerializeField] GameEvent OnGameWin;



    void FixedUpdate()
    {
        if(receiver.Recived)
        {
            skullObject.Rotate(0f,0f,rotationSpeed*Time.deltaTime, Space.Self);
            //Game event Raise
            OnGameWin.Raise(this,true);
        }
        
    }

}
