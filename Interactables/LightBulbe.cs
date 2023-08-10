using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBulbe : MonoBehaviour,IGrabable
{
    private bool isLooking=false;
    [SerializeField] Outline outline;
    [SerializeField] Rigidbody rb;
    private bool inhand;


     public void GrabInHand()
     {
        if(inhand)
        {
            PlayerHand.RemoveFromHand();
        }
       else 
       {
            PlayerHand.PutInHand(this.gameObject,this); //sending IGrabale script by "this" to send In Event
        }  
     }

    public bool Looking
    {
        // when accessing the property simply return the value
        get => isLooking;

        // when assigning the property apply visuals
        set
        {
            // same value ignore to save some work
            if(isLooking == value) return;

            // store the new value in the backing field
            isLooking = value;

            outline.enabled=isLooking;
            if(TryGetComponent<DialugeTrigger>(out DialugeTrigger dt)) dt.Trigger();
        }
    }
     public void OnDestroy()
    {
        Destroy(this.gameObject);
    }
   
    public void TwickRigidbody(bool mode)
    {
        if(mode) rb.isKinematic=true;
        else rb.isKinematic=false;
    }

    public void EventcheckInHand(Component sender,object data)
    {
       if(data is null) inhand=false;
       else inhand=true;
    }
}
