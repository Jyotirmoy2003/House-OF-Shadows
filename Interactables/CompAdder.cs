using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompAdder : MonoBehaviour,IInteractable
{
   public enum componentType
   {
    Torch,
    Radio,
   }

   [SerializeField] componentType mycomp;
   private float timmerForOutline;
   private Outline outline;
   private bool isLooking=false;
    
    





    void Start()
    {
        outline=GetComponent<Outline>();
        if(outline!=null)outline.enabled=false;
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

            if(outline!=null)outline.enabled=isLooking;
            if(TryGetComponent<DialugeTrigger>(out DialugeTrigger dt)) dt.Trigger();
        }
    }

    public void Interact()
    {
        CheckAndAdd();
    }
    
    public void NotLooking()
    {
        outline.enabled=false;
    }
    void CheckAndAdd()
    {
        switch(mycomp)
        {
            case componentType.Torch:
                Torch torch=GameObject.FindGameObjectWithTag("Player").GetComponent<Torch>();
                torch.enabled=true;
                Destroy(this.gameObject);
                break;
            case componentType.Radio:
                Radio radio=GameObject.FindGameObjectWithTag("Player").GetComponent<Radio>();
                radio.enabled=true;
                Destroy(this.gameObject);
                break;

        }

    }
}
