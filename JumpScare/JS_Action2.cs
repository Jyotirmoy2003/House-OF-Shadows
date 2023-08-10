using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class JS_Action2 : MonoBehaviour,IJumpScare
{
    [SerializeField] GameEvent jumpscareTrigger;
    [SerializeField] int cutsceneIndex=2;
    [SerializeField] float cutsceneLength=30f;
    

    
    void Start()
    {
        //test if it is a Cutscene
        if(FindObjectOfType<LevelLoder>().GetCuurentSceneIndex()==cutsceneIndex)
        {
            //get the cutscene length
            cutsceneLength= FindObjectOfType<ManagerScene>().cutsceneLength;
            
            Invoke("BackToGame",cutsceneLength);
        } 
        //test if returning from a cut scene
        if(FindObjectOfType<ManagerScene>().returingFromCutScene)
        {
            //load the saved game
            //FindObjectOfType<SaveAndLoad>().LoadGame();
            //Destroy this Object so that it can never trigger again
            FindObjectOfType<GameManager>().ChooseObject();
            Destroy(this.gameObject);
        }     
    }

    

    void OnTriggerEnter(Collider info)
    {
        if(info.gameObject.tag=="Player"&&!FindObjectOfType<ManagerScene>().returingFromCutScene)
        {
            jumpscareTrigger.Raise(this,true);
        }
    }
    void BackToGame()
    {
        FindObjectOfType<ManagerScene>().returingFromCutScene=true;
        //load game scene
        FindObjectOfType<LevelLoder>().LoadLevel(2);
        //loaded the saved game in JumpscareManager
    }
    public void Action()
    {
        ManagerScene ms=FindObjectOfType<ManagerScene>();
        //save the game before leaving the scene
        //FindObjectOfType<SaveAndLoad>().SaveGame();
        //Save Cutscene length in ManagerScene
        ms.cutsceneLength=cutsceneLength;
        //load the scene
        ms.LoadLevelAndDestroy(cutsceneIndex,this);
       
    }
}
