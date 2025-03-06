using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;
//using UnityEngine.UIElements;
[System.Serializable]
struct GraphicsToggle{
    public Toggle toggle;
    public TMP_Text text;
}

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject generalPanel,graphicsPanel;
    [SerializeField] Image buttonGeneralPanel,buttonGraphicsPanel;
   [SerializeField] GameObject menuPanel,settingPanel,controlPanel;
   [Header("Mouse")]
   [SerializeField] Slider mouseSensetivitySlider;
   [SerializeField] TMP_InputField mouseInputField;
   [Header("Sound")]
   [SerializeField] Slider soundSlider;
   [SerializeField] TMP_InputField soundInputField;



   [Header("Graphics Menu")]
    [SerializeField] List<Toggle> graphicsToggles=new List<Toggle>();


    void Start()
    {
        AudioManager.instance.PlaySound("InSearchOfSilence");
        //sound slider
        float currentSoundValue=PlayerPrefs.GetFloat("soundlvl",100f);
        soundSlider.value=currentSoundValue;
        soundInputField.text=currentSoundValue.ToString();

        //mouse slider
         
        float currentMouseValue=PlayerPrefs.GetFloat("mouse",100f);
        mouseSensetivitySlider.value=currentMouseValue;
        mouseInputField.text=currentMouseValue.ToString();
        
        foreach(Toggle item in graphicsToggles)
        {
            item.isOn=false;
        }
        int index=QualitySettings.GetQualityLevel();
        graphicsToggles[index].isOn=true;

        controlPanel.SetActive(false);

    }
#region  Mian Menu
    public void OnClickStartGame()
    {
        SceneManager.LoadScene("Game");
        Time.timeScale=1;
    }
    public void OnClickExit()
    {
        Application.Quit();
    }
    public void OnClickSettings()
    {
        menuPanel.SetActive(false);
        settingPanel.SetActive(true);
        OnCLickGENERAL();
    }

    public void OnClickControl()
    {
        menuPanel.SetActive(false);
        controlPanel.SetActive(true);
    }
#endregion

#region Settings

    #region  slider
    public void SetFromSlider(int switch_on)
    {
        switch (switch_on)
        {
            case 1:
                soundInputField.text=soundSlider.value.ToString();
                AudioManager.instance.SetAudioVolumeFactor(soundSlider.value);
                PlayerPrefs.SetFloat("soundlvl",soundSlider.value);
                break;

            case 3: 
                mouseInputField.text=mouseSensetivitySlider.value.ToString();
                PlayerPrefs.SetFloat("mouse",mouseSensetivitySlider.value);
                break;
                
            
            default:
                Debug.Log("Wrong Choice");
                break;
        }

    }
    
    public void SetFromInputField(int switch_on)
    {
        switch (switch_on)
        {
            case 1:
                int temp=int.Parse(soundInputField.text);
                if(temp>100) temp=100;
                else if(temp<0) temp=0;
                soundSlider.value=temp;
                AudioManager.instance.SetAudioVolumeFactor(temp);
                soundInputField.text=temp.ToString();
                
                PlayerPrefs.SetFloat("soundlvl",soundSlider.value);
                break;

            case 3:
                int temp1=int.Parse(mouseInputField.text);
                if(temp1>1000) temp=1000;
                else if(temp1<0) temp=0;
                mouseSensetivitySlider.value=temp1;
                mouseInputField.text=temp1.ToString();
                PlayerPrefs.SetFloat("mouse",mouseSensetivitySlider.value);
                break;
            
            default:
                Debug.Log("Wrong Choice");
                break;
        }
    }
    #endregion
    public void OnClickBackSettings()
    {
        settingPanel.SetActive(false);
        controlPanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    public void OnCLickGENERAL()
    {
        buttonGeneralPanel.color=new Color(72,72,72,255);
        buttonGraphicsPanel.color=new Color(72,72,72,0);
        generalPanel.SetActive(true);
        graphicsPanel.SetActive(false);
    }
    public void OnCLickGRAPHICS()
    {
        buttonGeneralPanel.color=new Color(72,72,72,0);
        buttonGraphicsPanel.color=new Color(72,72,72,255);
        generalPanel.SetActive(false);
        graphicsPanel.SetActive(true);
    }

    public void SetGraphics(int index)
    {
        //set quality
        QualitySettings.SetQualityLevel(index);
        for(int i=0;i<graphicsToggles.Count;i++)
        {
            if(index==i) continue;
            graphicsToggles[i].isOn=false;
        }

    }

#endregion
}
