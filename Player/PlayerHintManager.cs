using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayerHintManager : MonoBehaviour
{
    public static PlayerHintManager Instance;
   [SerializeField] TMP_Text playerHintText;

   private Component lastReceiver,currentRecevier;

   void Awake()
   {
        Instance=this;
   }

   public void SetPlayerHintText(string hintText,float timer,Component receiver)
   {
        currentRecevier=receiver;
        //if its new receiver
        if(currentRecevier!=lastReceiver)
        {
            //stop the previous coroutine if its already running
            StopCoroutine(DisplayHintText(timer));
            //set new text
            playerHintText.text=hintText;
            lastReceiver=currentRecevier;
            //start Coroutine to Display text
            StartCoroutine(DisplayHintText(timer));
        }
        
   }

   IEnumerator DisplayHintText(float amount)
   {
        yield return new WaitForSeconds(amount);
        playerHintText.text=" ";

   }

}
