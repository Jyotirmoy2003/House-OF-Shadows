using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSoundManager : MonoBehaviour
{
    [SerializeField] List<string> soundsName=new List<string>();
    private int index=0;
    [SerializeField] float rangeAmount;
    [SerializeField] float baseTimeAmount;
    private float cuurentTimeInterval;

    void Start()
    {
        cuurentTimeInterval=baseTimeAmount;
        Invoke("SetNewValuesSound",40f);
       
    }

    

    void SetNewValuesSound()
    {
        cuurentTimeInterval=baseTimeAmount+Random.Range(0,rangeAmount);
        index=Random.Range(0,soundsName.Count);
        StartCoroutine(Play());
    }

    IEnumerator Play()
    {
        
        //wait for time
        yield return new WaitForSeconds(cuurentTimeInterval);
        //play sound
        AudioManager.instance.PlaySound(soundsName[index]);
        SetNewValuesSound();
    }
}
