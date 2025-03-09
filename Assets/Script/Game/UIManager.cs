using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;

public class UIManager : MonoSingleton<UIManager>
{
    [Header("Battery")]
    [SerializeField] GameObject batteryContainer;
   [SerializeField] Slider batteryHealth;
   [SerializeField] Gradient gradient;
   [SerializeField] Image fill;
   [SerializeField] GameObject keySuggestPanel;
   [SerializeField] TMP_Text text_key;
   [Header("Bullet")]
   [SerializeField] GameObject bulletConatiner;
   [SerializeField] TMP_Text textNoOfBullet;
   [Header("Tapes")]
   [SerializeField] TMP_Text tapeCaption;
   [SerializeField] float showCaptionTime=10;




    void Start()
    {
        batteryContainer.SetActive(false);
        bulletConatiner.SetActive(false);
    }
  
  //fun to set battery amount in UI
    public void SetBattery(float amount)
    {
        batteryHealth.value=amount;
        fill.color=gradient.Evaluate(batteryHealth.normalizedValue);
        
    }

    //set max Value Of battery
    public void SetMaxBattery(float amount)
    {
        batteryHealth.maxValue=amount;
        batteryHealth.value=amount;
        fill.color=gradient.Evaluate(1f);
        batteryContainer.SetActive(true);
    }


    public void SetBulletUI(int amount)
    {
        textNoOfBullet.text="0"+amount.ToString();
    }

    public void SetKeySuggest(string keyCode)
    {
        if(keyCode=="empty")
        {
            keySuggestPanel.SetActive(false);
            return;
        }
        keySuggestPanel.SetActive(true);
        text_key.text=keyCode.ToUpper();
    }

    public void SetCaption(string caption)
    {
        tapeCaption.text=caption;
        StartCoroutine(StopCaption());
    }

    IEnumerator StopCaption()
    {
        yield return new WaitForSeconds(showCaptionTime);
        tapeCaption.text="";
    }


    public void ListenToPlayerGotGun(Component sernder, object data)
    {
        
        bulletConatiner.SetActive((bool)data);
        
    }
    
}
