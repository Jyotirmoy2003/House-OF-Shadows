using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CD : MonoBehaviour,IInteractable
{
    [SerializeField] Sound sound;
    [SerializeField] string caption;
    private float timmerForOutline;
    private Outline outline;
    private bool isLooking=false;
    private UIManager uIManager;





    void Start()
    {
        outline=GetComponent<Outline>();
        outline.enabled=false;
        uIManager=FindObjectOfType<UIManager>();
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
        
        Radio radio=GameObject.FindGameObjectWithTag("Player").GetComponent<Radio>();
        if(radio.enabled)
        {
            radio.PlayeOnRadio(sound);
        }
        uIManager.SetCaption(caption);
    }
    
   
     

}
