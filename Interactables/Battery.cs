using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour,IInteractable
{
   private Outline outline;
   private Torch torch;
   [SerializeField] float amountOfCharge;


   private GameObject player;
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

    public void Interact()
    {
        player= GameObject.FindGameObjectWithTag("Player");
        torch= player.GetComponent<Torch>();
        if(torch.enabled)
        {
            torch.RechargeTorch(amountOfCharge);
            Destroy(this.gameObject);
        }
    }

    public void NotLooking()
    {
        outline.enabled=false;
    }
    
}


