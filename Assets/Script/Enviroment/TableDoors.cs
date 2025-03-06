using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableDoors : MonoBehaviour,IInteractable
{
        [SerializeField] Animator openandclose;
		[SerializeField] bool open; //bool to keep trak of if door is open or close
       private bool isLooking=false;
       [SerializeField] Outline outline;

		void Start()
		{
			open = false;
            openandclose=GetComponent<Animator>();
            outline=GetComponent<Outline>();
            outline.enabled=false;
            
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

    //When player try to call this
    public void Interact()
    {
        if(!open) StartCoroutine(opening());
        else StartCoroutine(closing());

        AudioManager.instance.PlaySound("Closet");
    }
    #endregion
		
		IEnumerator opening()
		{
			openandclose.Play("Opening");
			open = true;
			yield return new WaitForSeconds(.5f);
		}

		IEnumerator closing()
		{
			openandclose.Play("Closing");
			open = false;
			yield return new WaitForSeconds(.5f);
		}
}
