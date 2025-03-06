using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerScene:MonoBehaviour
{

    public static ManagerScene instance;
    public bool returingFromCutScene=false;
    public float cutsceneLength=5;
    public JS_Action2 jumpscareTrigger;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
        
        
        DontDestroyOnLoad(gameObject);
    }
    public void LoadLevelAndDestroy(int index,Component sneder)
    {
        Destroy(sneder);
         FindObjectOfType<LevelLoder>().LoadLevel(2);
        //Invoke("Go",10f);
        
    }
    void Go()
    {
        FindObjectOfType<LevelLoder>().LoadLevel(2);
    }
}
