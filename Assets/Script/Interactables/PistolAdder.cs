using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolAdder : MonoBehaviour,IGrabable
{

    [SerializeField] Outline outline;
    [SerializeField] Rigidbody rb;
    private GameObject mainCamera;
    [SerializeField] GameEvent PlayerGotPistol;
    [SerializeField] bool inhand;
    private bool isLooking=false;



    void Start()
    {
        mainCamera=GameObject.FindGameObjectWithTag("MainCamera");
        outline.enabled=false;
       
    }
   

   
    #region  Igrabable
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
    public void TwickRigidbody(bool mode)
    {
        if(mode) rb.isKinematic=true;
        else rb.isKinematic=false;
    }
    public void GrabInHand()
    {
        if(inhand){
            PlayerHand.RemoveFromHand();
            PlayerGotPistol.Raise(this,false);
            UIManager.Instance.SetBulletUI(0);

        }

       else {
        PlayerHand.PutInHand(this.gameObject,this); //sending IGrabale script by "this" to send In Event
        PlayerGotPistol.Raise(this,true);
        //UIManager.instance.SetBulletUI(pistol.GetBullet());
        this.gameObject.transform.SetParent(mainCamera.transform);
        transform.rotation=mainCamera.transform.rotation;
        
        }  
    }
    public void OnDestroy()
    {
        Destroy(this.gameObject);
    }
    #endregion
    public void EventcheckInHand(Component sender,object data)  //Event what is in Hand
    {
       if(data is null) {inhand=false;
        PlayerGotPistol.Raise(this,false);
        transform.SetParent(null);
       }
       else inhand=true;
    }


   
}
