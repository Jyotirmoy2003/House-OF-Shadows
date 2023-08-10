using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour,IInteractable
{
    public bool locked;
    [SerializeField] Key.KeyType keyType;
    public Key.KeyType GetKeyType()
    {
        return keyType;
    }

    [HideInInspector]
    public bool doorOpen=false; 
    public bool isItJSTrigger=false; //JSTrigger => jump scare trigger
    public GameEvent JSTriggerEvent; //event to call if it is a JSTrigger
    private Animator animator;
    private Outline outline;
    private bool isLooking=false;
    private bool isElectricityOn=false; //if electricty supply in on




    void Start()
    {
        outline=GetComponent<Outline>();
        animator=GetComponent<Animator>();
        outline.enabled=false;
    }

    void Update()
    {

    }
    //Iterface fun
    public void Interact()
    {
        if(locked) //if door is locked
        {
            if(FindObjectOfType<KeyHolder>().ContainsKey(keyType) && isElectricityOn)
            {
                IntDoor();
                FindObjectOfType<KeyHolder>().RemoveKey(keyType);
                locked=false;
            }else //if player dont meet the condition then play loocked Sound
            {
                FindObjectOfType<AudioManager>().PlaySound("LockedDoor",this.gameObject);
            }
        }else{
            IntDoor();
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




    // public void Looking()
    // {
    //     outline.enabled=true;
    // }
    public void NotLooking()
    {
        outline.enabled=false;
    }




    void IntDoor()  //Mian Fun To Open And Close
    {
        
        if(!doorOpen)
        {
            animator.Play("Opening");
            if(isItJSTrigger)
            {
                JSTriggerEvent.Raise(this,true); //Calling event to trigger Jump Scare
            }
                
        }else
        {
            animator.Play("Closing");
        }
        FindObjectOfType<AudioManager>().PlaySound("DoorOpen",this.gameObject);
        doorOpen=!doorOpen;

    }

    //listen to event when Electricity is on
    public void ListenToElectricityEvent(Component sender,object data)
    {
        if(data is bool )
        {
            isElectricityOn=(bool)data;
        }
    }
}
