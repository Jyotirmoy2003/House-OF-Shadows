using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EDoor : MonoBehaviour,IInteractable
{
    private Outline outline;
   private float timmerForOutline;
   private bool doorOpen=false;
   private Animator animator;
   private bool isLooking=false;




    void Start()
    {
        outline=GetComponent<Outline>();
        outline.enabled=false;
        animator=GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
   
        
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
        IntDoor();
    }


    void IntDoor()  //Mian Fun To Open And Close
    {
            if(!doorOpen)
            {
                animator.SetBool("EDOpen",true);
                
            }else
            {
                animator.SetBool("EDOpen",false);
            }
            doorOpen=!doorOpen;

    }

}
