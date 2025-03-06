using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameEnd : MonoBehaviour
{
   [SerializeField] PlayableDirector cutsceneTimeline;
   [SerializeField] LevelLoder levelLoder;
    
   
   public void SwitchLvl()
   {
      cutsceneTimeline.Stop();
      levelLoder.LoadLevel(0);
   }

  

   void Update()
   {
      if(Input.GetMouseButtonDown(2))
      {
         Cursor.lockState=CursorLockMode.Locked;
      }else if(Input.GetMouseButtonDown(1))
      {
         Cursor.lockState=CursorLockMode.None;
      }
      //when click esc skip the scene
      if(Input.GetKeyDown(KeyCode.Escape))
      {
         
         SwitchLvl();
      }
   }
}
