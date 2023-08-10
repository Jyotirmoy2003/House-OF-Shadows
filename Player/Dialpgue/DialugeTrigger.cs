 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialugeTrigger : MonoBehaviour
{
    public Dialouge dialouge;
    private int stopAfterTime=5;
    private int currentIteration=0;



    public void Trigger()
    {

        FindObjectOfType<DialougeManager>().StartDialouge(dialouge);
        currentIteration=0;
        StartCoroutine(TimerOfDialouge());


    }
    public void EndTrigger()
    {
        FindObjectOfType<DialougeManager>().EndDialouge();
    }

    IEnumerator TimerOfDialouge()
    {
        for(currentIteration=0;currentIteration<stopAfterTime;currentIteration++)
        {
            yield return new WaitForSeconds(1);
        }
        
       FindObjectOfType<DialougeManager>().EndDialouge();
        
    }

    public void SetTimeLngth(int amount)
    {
        stopAfterTime=amount;
    }

    public void AddDialouge(string speech)
    {
        dialouge.sentences.Clear();
        dialouge.sentences.Add(speech);
    }



}
