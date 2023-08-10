using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class JumpScareData
{
    public MonoBehaviour trigger;
    public MonoBehaviour jumpScare;
    public float timer;

    public JumpScareData()
    {
        trigger=null;jumpScare=null;
    }

    
}

public class JumpScareManager : MonoBehaviour
{
    
    public Dictionary<MonoBehaviour,IJumpScare> jumpScareDictionary=new Dictionary<MonoBehaviour, IJumpScare>();
    public JumpScareData[] jumpScareDatas;

   

    

    
    //fun to find the caller if its exists in our data,
    //if so then we trigger our ijumpsacre interface after the given amount of time
    public void SetAction(MonoBehaviour sender)
    {
        JumpScareData temp=Array.Find(jumpScareDatas,JumpScareData=>JumpScareData.trigger==sender);
        if(temp == null){ return;}
        StartCoroutine(TriggerAfterTime(temp));
        
    }

    //Timer function
    IEnumerator TriggerAfterTime(JumpScareData temp)
    { 
        yield return new WaitForSeconds(temp.timer);
        temp.jumpScare.GetComponent<IJumpScare>().Action();
    }



    //If Any trigger is triggered then we will listen from this event 
    //and if it is MonoBehaviour then we will trigger its corrosponding ijumpscare
    public void ListenJumScareTrigger(Component sender,object data)
    {
        if(sender is MonoBehaviour)
        {
            SetAction((MonoBehaviour)sender);
            
        }
    }
}
