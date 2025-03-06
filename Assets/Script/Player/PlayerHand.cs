using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    public static PlayerHand instance;
    private static  Transform objectTransform;
    private static IGrabable Caller;
    [SerializeField] float smoothTime;
    [SerializeField] GameEvent OnPlayerInHand;
   
    void Awake()
    {
        instance=this;
    }

    void Update()
    {
        if(objectTransform!=null)
        {
            Grab();
        }else{
            OnPlayerInHand.Raise(null,(object)null);
        }
    }

    #region Main Hand Fun
    public static void PutInHand(GameObject obje,IGrabable  caller)  //taking who is In Hand By Caller
    {
        objectTransform=obje.transform;
        Caller=caller; 
        //If Its RigidBody Is Already Off
        caller.TwickRigidbody(true);
    }
    public static void RemoveFromHand()
    {
        if(Caller!=null)Caller.TwickRigidbody(false);
        objectTransform=null;
        Caller=null;
    }
    #endregion
    
    private void Grab()
    {
        objectTransform.position=Vector3.Lerp(objectTransform.position,transform.position,smoothTime);
        //Event
        OnPlayerInHand.Raise(objectTransform,(object)Caller);  //Sending The IGrabable to GetInfo
    }
    

    //Fun To Call From GameEventListener
    public void EventObjectIsUsed(Component sender,object data)  //If Object Is Used
    {
        if(data is IGrabable && (IGrabable)data==Caller)
        {
            Caller=null;
            RemoveFromHand();
        }
    }

    

    
}
