using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawer : MonoBehaviour,IInteractable
{
    [SerializeField] float Speed;
    [SerializeField] Vector3 distance;
    [SerializeField] List<Vector3> target=new List<Vector3>();
    private int triggered=4;
    private Outline outline;
    private bool isLooking=false;
    private Transform myTransform;





    void Start()
    {
        target[1]=transform.localPosition; //Next Destination pos
        target[2]=transform.localPosition+distance;//Open Pos
        target[0]=transform.localPosition;//close Pos
        triggered=4; //To Keeptrack of where the drawer currently is
        outline=GetComponent<Outline>();
        outline.enabled=false;
        
        myTransform=GetComponent<Transform>();
    }

    void Update()
    {
   
        if(triggered==2)
        {
           target[1]=target[2];
           triggered=3;
           
        }else if(triggered==1)
        {
            target[1]=target[0];
            triggered=4;
        }
        //Smoothly Reach to Destination 
        myTransform.localPosition = Vector3.Lerp(transform.localPosition,target[1], Speed * Time.deltaTime);

    }
    #region  Interface
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
        if(triggered==3)
        {
            triggered=1;
        }else if(triggered==4){
            triggered=2;
        }
        AudioManager.instance.PlaySound("Drawer",this.gameObject);
    }
    #endregion

    
}
