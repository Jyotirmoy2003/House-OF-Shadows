using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GrabObjectReceiver : MonoBehaviour,IInteractable
{
    [SerializeField] GameEvent OnObjectIsUsed;
    [SerializeField] List<GameObject> realObjects=new List<GameObject>();
    [SerializeField] GrabObject.ItemId neededId;
    [SerializeField] string beforeReceived,afterReceived;
    [SerializeField] bool isDestroy=false;
    [SerializeField] string hintForNextItem;
    [Range(1,15)]
    [SerializeField] float showHintTimmer=5;
    private GrabObject currentGrabObject=null;
    private DialugeTrigger dt;
    private IGrabable grabable;
    private Outline outline;
    private bool isLooking=false;
    private int index;

    public bool Recived=false;
    

    
    void Start()
    {
        outline=GetComponent<Outline>();
        outline.enabled=false;
        dt=GetComponent<DialugeTrigger>();
        index=realObjects.Count-1;
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

            if(TryGetComponent<DialugeTrigger>(out var dt)) dt.Trigger();
        }
    }
   
    public void Interact()
    {
       if(currentGrabObject!=null && !Recived)
       {
            OnTriggerObject();
            //Activate real objects
            realObjects[index].SetActive(true);
            index--;
            //set dialouge
            dt.AddDialouge(beforeReceived);
            //let other know which object is received
            OnObjectIsUsed.Raise(this,currentGrabObject);
       }
       if(index<0)
       {
            Recived=true;
            //set hint for next item
            PlayerHintManager.Instance.SetPlayerHintText(hintForNextItem,showHintTimmer,this);
            //show dialouge after receving
            dt.AddDialouge(afterReceived);
            if(isDestroy) 
            {
                outline.enabled=false;
                this.enabled=false;
            }
       }
    }

    //event to know if player is holding Fuel
    public void EventWhatIsInHand(Component sender,object data=null)
    {
        if(data is GrabObject)
        {
            currentGrabObject=(GrabObject)data;
            //if sended object is not the desired one then just set it to null
            if(currentGrabObject.myId!=neededId)
            {
                currentGrabObject=null;
                return;
            }
        
        }else{
            currentGrabObject=null;
        }
    }

    private void OnTriggerObject() //when player hit use then do the visuals
    {
       currentGrabObject.TwickRigidbody(true);
       Destroy(currentGrabObject);
       currentGrabObject=null;
    }
}
