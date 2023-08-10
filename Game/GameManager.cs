using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private List<IInteractable> interactables=new List<IInteractable>();
    private AudioManager audioManager;
    [SerializeField] GameObject restartPanel;
    private Image restartPanelImage;
    [SerializeField] GameObject exitPanel;
    [SerializeField] float timeAfterGameOver=5;
    [Header("HindingObject")]
    [SerializeField] Transform dummyPositionForPlayer;

    [Header("Spawn Objects")]
    // [SerializeField] List<Room> rooms=new List<Room>();
    [SerializeField] List<Transform> largeSpawnPoints=new List<Transform>();
    [SerializeField] List<Transform> smallSpawnPoints=new List<Transform>();

    [SerializeField] List<ObjectToSpawn> objectsToSpawn=new List<ObjectToSpawn>();
    [Header("Spwan AI")]
    [SerializeField] GhoulStateMachine stateMachine;
    [SerializeField] float spawnAfter=20f;
    [Header("GameWin")]
    [SerializeField] int cutSceneWinGameIndex=3;

    

    void Start()
    {
        restartPanelImage=restartPanel.GetComponent<Image>();
        Time.timeScale=1; //set time scale to 1 when game is starting
        restartPanelImage.color=new Color(0,0,0,0); //set Alpha zero
        audioManager= FindObjectOfType<AudioManager>();
        //deactivate the exit and restart panels
        exitPanel.SetActive(false);
        restartPanel.SetActive(false);

        //fun to spawn the Ai;
        stateMachine.enabled=false;
        StartCoroutine(SpawnAI());

        audioManager.StopAllSounds();
        //call the fun to spawn objects in the scene
        ChooseObject();
       
    }
    



   
    IEnumerator SpawnAI()
    {
        yield return new WaitForSeconds(spawnAfter);
        stateMachine.enabled=true;
    }

    

    void FixedUpdate()
    {
        //when player want to exit the game
        if(Input.GetKey(KeyCode.Escape))
        {
            if(exitPanel.activeSelf)
            {
                exitPanel.SetActive(false);
                Time.timeScale=1;
                Cursor.lockState=CursorLockMode.Locked;
            }else{
                exitPanel.SetActive(true);
                Time.timeScale=0;
                Cursor.lockState=CursorLockMode.None;
            }
        }
       
       
    }


#region Exit Ui
    public void OnClickExit()
    {
        FindObjectOfType<LevelLoder>().LoadLevel(0);
    }
    public void OnClickReStart()
    {
        FindObjectOfType<LevelLoder>().LoadLevel(2);
    }
    public void OnClickNo()
    {
        exitPanel.SetActive(false);
        Time.timeScale=1;
        Cursor.lockState=CursorLockMode.Locked;
    }

    #endregion

    //Fun to send the dummy location when player is hiding
    public Vector3 GetDummyPosition(){
        return dummyPositionForPlayer.position;
    }
    

    #region Spawn Objects
   
    public void ChooseObject(){

        while(objectsToSpawn.Count!=0 )
        {
            int index=Random.Range(0,objectsToSpawn.Count);
            //select objects
            ObjectToSpawn temp_objectToSpawn=objectsToSpawn[index];

            Transform spawnPoint;
            //test if object need large place
            if(temp_objectToSpawn.NeedLargeSpace)
            {
                //if no more place left return
                if(largeSpawnPoints.Count==0) return;
                //chose a transfrom to spawn
                spawnPoint=largeSpawnPoints[Random.Range(0,objectsToSpawn.Count)];
                //rempve it from list
                largeSpawnPoints.Remove(spawnPoint);
            }else{
                if(smallSpawnPoints.Count==0) return;
                spawnPoint=smallSpawnPoints[Random.Range(0,objectsToSpawn.Count)];
                smallSpawnPoints.Remove(spawnPoint);
            }
           

            
            
            //Instantiate object
            temp_objectToSpawn.prefabTransform.position=spawnPoint.position;
            temp_objectToSpawn.prefabTransform.SetParent(spawnPoint);

            if(temp_objectToSpawn.number==0)
            {
                //remove ref from list
                objectsToSpawn.Remove(temp_objectToSpawn);
            }else{
                objectsToSpawn[index].number--;
                Instantiate(temp_objectToSpawn.prefabTransform.gameObject,spawnPoint.position,Quaternion.identity).transform.SetParent(spawnPoint);
            }
            
            
        }
    }












    #endregion
    

    #region Events
    //Event When GameOver
    public void ListenGameOver(Component sneder,object data)
    {
        if(data is bool && (bool)data)
        {
           StartCoroutine(GameOverAfter());
        }
    }
    //when game win
    public void ListGameWin(Component sender,object data)
    {
        if(data is bool &&(bool)data)
        {
            SceneManager.LoadScene(cutSceneWinGameIndex);
        }
    }
    IEnumerator GameOverAfter()
    {
        yield return new WaitForSeconds(timeAfterGameOver);
        restartPanel.SetActive(true);
        exitPanel.SetActive(false);
        Time.timeScale=1;
        Cursor.lockState=CursorLockMode.None;
    }

    #endregion
    
}


//custom class to Hold data about spawnable objects
[System.Serializable]
public class ObjectToSpawn
{
    public Transform prefabTransform;
    public bool NeedLargeSpace;
    public int number;

}