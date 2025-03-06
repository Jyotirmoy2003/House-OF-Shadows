using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuel : MonoBehaviour,IGrabable
{
    private Outline outline;
    private Rigidbody rb;
    private bool inhand;
    private bool isLooking=false;






    void Start()
    {
        outline=GetComponent<Outline>();
        outline.enabled=false;
        rb=GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(inhand)
        {
            rb.freezeRotation=true;
        }

    }
    
    
   
    #region  IGrabable
    public void GrabInHand()
    {
    //    if(inhand){
    //         PlayerHand.RemoveFromHand();
    //     }

    //    else {
    //     PlayerHand.PutInHand(this.gameObject,this); //sending IGrabale script by "this" to send In Event
    //     }  

        PlayerHand.PutInHand(this.gameObject,this);
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
    #endregion
    public void EventcheckInHand(Component sender,object data)
    {
       if(data is null) inhand=false;
       else inhand=true;
    }

    

}
