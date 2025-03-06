using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour,IInteractable
{   
    [SerializeField] KeyType keyType;
   public enum KeyType
   {
        Dyning,
        ElectricBox,
        DollRoom,
        Kitchen,
        BedRoom,

   }
   private Outline outline;
   private bool isLooking=false;



    void Start()
    {
        outline=GetComponent<Outline>();
        outline.enabled=false;
        
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
    public void NotLooking()
    {
        outline.enabled=false;
    }
    public void Interact()
    {
        FindObjectOfType<AudioManager>().PlaySound("KeyPick");
        FindObjectOfType<KeyHolder>().AddKey(keyType);
        Destroy(this.gameObject);
    }

}
