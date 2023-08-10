using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChangesInObject
{
   public GameObject Object;
    public Transform newTranform,OldTransform;
}

public class HideObject : MonoBehaviour,IInteractable
{
    [SerializeField] Transform PositonInObjectForCamera;
    [SerializeField] ChangesInObject[] changes;
    private Outline outline;
   private bool isLooking=false;

    void Start()
    {
        outline=GetComponent<Outline>();
        outline.enabled=false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region  interface
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

    }
    public void Interact()
    {
        FindObjectOfType<PlayerMachanics>().Hide(PositonInObjectForCamera,this);   
    }

    
   
    #endregion
     //player currently Hiding
    //calling from PlayerMachanics class when player tried to hide
    public void SetPos()
     {
        foreach (ChangesInObject item in changes) 
        {
            item.OldTransform=item.Object.transform;
            item.Object.transform.position=item.newTranform.position;
            item.Object.transform.rotation=item.newTranform.rotation;
            item.Object.transform.localScale=item.newTranform.localScale;
            
        }


     }
     //player want back to game from hiding
     public void SetBackPos()
     {
        foreach (ChangesInObject item in changes)
        {
            item.Object.transform.position=item.OldTransform.position;
            item.Object.transform.rotation=item.OldTransform.rotation;
            item.Object.transform.localScale=item.OldTransform.localScale;

        }
     }
}
