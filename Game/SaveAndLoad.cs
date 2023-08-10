using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveAndLoad : MonoBehaviour
{

    private static  string SAVE_FOLDER;
    SaveObject saveObject=new SaveObject();
    SaveObject lodedSavobject=new SaveObject();
    string json;

    private Transform playerPos,zoombiPos,gascanPos;
    private Torch torchObj;



    void Awake()
    {
        try{
        SAVE_FOLDER=Application.dataPath + "/Saves/";
        //Take All Important Object Data
        playerPos=FindObjectOfType<PlayerMachanics>().gameObject.transform;
        zoombiPos=FindObjectOfType<GhoulStateMachine>().transform;
        //gascanPos=FindObjectOfType<Fuel>().transform;
        torchObj=FindObjectOfType<Torch>();}
        catch{}

        //Test if Save Folder Exists
        if(!Directory.Exists(SAVE_FOLDER))
        {
            //create Folder
            Directory.CreateDirectory(SAVE_FOLDER);
            print("Create Directory");
        }
    }
    


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            print("Loading Game");
            LoadGame();
        }
        if(Input.GetKeyDown(KeyCode.P))
        {
            SaveGame();
        }
    }


    public void SaveGame()
    {  
        try{ 
            //Set All Data in SaveObject 
        saveObject.playerPosition=playerPos.position;
        saveObject.zoombiPosition=zoombiPos.position;
        saveObject.keyList=FindObjectOfType<KeyHolder>().GetKeyList();
        //saveObject.gascanPosition=gascanPos.position;
        saveObject.torch=torchObj;
        saveObject.haveTorch=FindObjectOfType<PlayerMachanics>().GetComponent<Torch>().enabled;
        saveObject.haveRadio=FindObjectOfType<PlayerMachanics>().GetComponent<Radio>().enabled;


        //conver to Json String
        json= JsonUtility.ToJson(saveObject);
        //save it
        File.WriteAllText(SAVE_FOLDER +"/save.txt",json);
        Debug.Log(json);

        }
        catch(UnassignedReferenceException ex){
            print("Error"+ex);
        }
       
    }

    public void LoadGame()
    {
        //Text If file Exists
        if(File.Exists(SAVE_FOLDER + "/save.txt"))
        {
            //load the string
            string saveString=File.ReadAllText(SAVE_FOLDER + "/save.txt");
            lodedSavobject = JsonUtility.FromJson<SaveObject>(saveString);

            //set All data In Game
            FindObjectOfType<PlayerMachanics>().SetlPlayerPos(lodedSavobject.playerPosition);
            FindObjectOfType<GhoulStateMachine>().transform.position=lodedSavobject.zoombiPosition;
           // FindObjectOfType<Fuel>().transform.position=lodedSavobject.gascanPosition;

        }        

    }

    #region UnityEvent
    public void ListenToElectricity(Component Sender,object data)
    {
        saveObject.isElectricityOn=(bool)data;
    }

    #endregion
    
}

public class SaveObject
{
    public Vector3 playerPosition;
    public Vector3 zoombiPosition;
    public List<Key.KeyType> keyList;

    //public Vector3 gascanPosition;

    public Vector3 gunPosition;
    public bool isElectricityOn;
    public Torch torch;
    public bool haveRadio,haveTorch;

}
