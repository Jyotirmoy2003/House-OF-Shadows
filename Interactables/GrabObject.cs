using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObject : MonoBehaviour,IGrabable
{
   public enum ItemId{
        Fuel,
        Wood,
        Lighter,
        Bulbe,
        skull,

    }
    public ItemId myId;
    private bool inhand;
    private bool isLooking;
    [SerializeField] Outline outline;
    [SerializeField] Rigidbody rb;
    
    void Start()
    {
        outline.enabled=false;
    }



    public void GrabInHand()
    {
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
   
}
