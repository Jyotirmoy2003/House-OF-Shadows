using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricHandel : MonoBehaviour,IInteractable
{
   private Outline outline;
   private bool doorOpen=false;
   private Animator animator;
   [SerializeField] Generator generator;
   private bool isLooking=false;

    void Start()
    {
        outline=GetComponent<Outline>();
        animator=GetComponent<Animator>();
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


    public void Interact()
    {
        if(generator.fuelReloder.Recived)
        {
            Handel();
            FindObjectOfType<AudioManager>().PlaySound("Lever",this.gameObject);
        }else
        {
            FindObjectOfType<AudioManager>().PlaySound("LeverStuck",this.gameObject);
        }
       
    }


    void Handel()  //Mian Fun To Open And Close
    {
            if(!doorOpen)
            {
                animator.SetBool("EboXDoorOpen",true);
                generator.TwickSwitch(true);
                
                
            }else
            {   
                generator.TwickSwitch(false);
                animator.SetBool("EboXDoorOpen",false);
                
            }
            doorOpen=!doorOpen;

    }
}
