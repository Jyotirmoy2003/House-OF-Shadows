using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    [SerializeField] InputReader gameInput;
    [SerializeField] float maxCharge;
    [SerializeField] float currCharge;
    [SerializeField] float amountOfBatteryConsumePerUnit;
    [SerializeField] Light flash;
    private UIManager uiManager;
    public bool torchOn=false;
    
    
    void Start()
    {
        currCharge=maxCharge;
        uiManager=FindObjectOfType<UIManager>();
        //set max value in ui slider
        uiManager.SetMaxBattery(maxCharge);
        SubcribeToInput(true);
    }

    void OnDisable()
    {
        SubcribeToInput(false);
    }

    void SubcribeToInput(bool val)
    {
        if(val)
        {
            gameInput.OnFlashlightEvent += FlashLightButtonPressed;
        }else{
            gameInput.OnFlashlightEvent -= FlashLightButtonPressed;
        }
    }

   
    void Update()
    {
       
        if(!torchOn) return;
        if(currCharge<0)
        { 
            torchOn=false;
            flash.enabled=torchOn;
        }
        else{
             currCharge-=amountOfBatteryConsumePerUnit*Time.deltaTime;
             uiManager.SetBattery(currCharge);
        }
    }

    //fun to rechange torch when player receive battery
    public void RechargeTorch(float amount)
    {
        if(currCharge<maxCharge) 
            currCharge+=amount;
        if(currCharge>=maxCharge) currCharge=maxCharge;
    }

    void FlashLightButtonPressed()
    {
        torchOn=!torchOn; //toggle the value
        flash.enabled=torchOn;
    }
}
