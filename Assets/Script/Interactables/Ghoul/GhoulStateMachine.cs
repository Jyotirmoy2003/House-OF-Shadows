using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class GhoulBase
{
    public abstract void EnterState(GhoulStateMachine gh);
    public abstract void UpdateState(GhoulStateMachine gh);

}

public class GhoulPetrolState:GhoulBase
{
    private Transform currentPetrolPoint,lastPetrolPoint;
    private int currentIndex=0;
    private Transform rayCastpos;
    private RaycastHit hitInfo;
    private LayerMask doors;
    private Door lastDoor;

    //Detecte player variables
    private float playerAngel;
    private float playerDistance;

    





    public override void EnterState(GhoulStateMachine gh)
    {
        gh.showStateColor=Color.green;
        GetNextPetrol(gh);
        this.rayCastpos=gh.rayCastpos;
        doors=gh.doorLayer;
        gh.navMeshAgent.speed=gh.PetrolSpeed;

        gh.notDetectedTillnow=false;
        

    }

    public override void UpdateState(GhoulStateMachine gh)
    {
        //Handel Doors
        #region Doors
       if(Physics.Raycast(rayCastpos.position,rayCastpos.TransformDirection(Vector3.forward),out hitInfo,7f,doors))
        {
          Door newDoor=hitInfo.collider.GetComponent<Door>();
            if(!newDoor.doorOpen && !newDoor.locked)
            { 
                gh.door.Enqueue(newDoor);

                gh.door.Peek().Interact();
                gh.StartCoroutine("DoorisOn");
                
            }
            else if(newDoor.locked && newDoor!=lastDoor) 
            {
                GetNextPetrol(gh);
                lastDoor=newDoor;
                
            }
        }
        #endregion
    
        #region Player Detecte
        playerDistance=Vector3.Distance(gh.myTransform.position,gh.player.position);
        playerAngel=Vector3.Angle(gh.myTransform.position,gh.player.position);
       
        if(playerDistance<gh.chaseDistance) //when player is too close
        {
           gh.lastKnownPLayerPosition=gh.player.position;
           gh.SwitchState(gh.ghoulPlayerDetected);

        }else if( playerDistance<gh.range&&gh.playerInVision && playerAngel<gh.FOV/2) //when player is infront and in range
        {
            gh.SwitchState(gh.ghoulPlayerDetected);
        }
        #endregion
    
        gh.navMeshAgent.destination=currentPetrolPoint.position;
        if(Vector2.Distance(gh.myTransform.position,currentPetrolPoint.position)<0.3 && !gh.isWaiting)
        {  //If reached petorl to other
            
            gh.StartCoroutine(gh.Wait(gh.waitTimeInEachPetrolPoint));;
            
        }
 
    }
     


    //Generate New Petrol Point
    void GetNextPetrol(GhoulStateMachine gh)
    {
        currentIndex=Random.Range(0,gh.petrolPoints.Count);
        currentPetrolPoint=gh.petrolPoints[currentIndex];
        
        
    }

   

    
}

public class GhoulPlayerDetected:GhoulBase
{
    private Vector3 target;
    private float Speed;
    private float newRange;
    private float oldRange;



    public override void EnterState(GhoulStateMachine gh)
    {
        //play sound and animation
        AudioManager.instance.PlaySound("Demonic_Scream",gh.gameObject);
        gh.animationGhoul.SetTrigger("Scream");
        target=gh.lastKnownPLayerPosition;
        gh.navMeshAgent.speed=gh.chaseSpeed;

        newRange=gh.range*2-2; //change new range as player is now detected
        oldRange=gh.range;
        gh.range=newRange;
        
        gh.notDetectedTillnow=false;
        gh.showStateColor=Color.red;
        AudioManager.instance.PlayBackgroundSound("Battle_for_freedom",20);

    }
    public override void UpdateState(GhoulStateMachine gh)
    {
        if(gh.playerInVision) target=gh.lastKnownPLayerPosition; //if player in vision then keep updateing target positon 
        gh.navMeshAgent.destination=target;

        if(Vector2.Distance(gh.myTransform.position,target)<0.2 )
        {
            gh.range=oldRange;
            gh.SwitchState(gh.ghoulLostPlayer);
        }
        //i will follow the player when it is in vision,if we lose the player then 
        //we will keep follwing the player for some amount of time, and if 
        //in this time we do not detect the player then we go back ..otherwise,
        //we rest the timer

    }

}




public class GhoulLostPlayer:GhoulBase
{
    public override void EnterState(GhoulStateMachine gh)
    {
        gh.StartCoroutine("ChasingPlayer");
        gh.navMeshAgent.speed=gh.PetrolSpeed;
        gh.showStateColor=Color.blue;
        //gh.animationGhoul.Blend("walk");
        //audio
    }
    public override void UpdateState(GhoulStateMachine gh)
    {
        //if detected player then go to PlayerDetected state
        if(gh.playerInVision)
        { 
            AudioManager.instance.PlayBackgroundSound("Battle_for_freedom",20);  
            gh.SwitchState(gh.ghoulPlayerDetected);
        }
        //if player is not detected till now go back to petrol
        if(gh.notDetectedTillnow && !gh.isWaiting) 
        {
            AudioManager.instance.StopSound("Battle_for_freedom");
            gh.SwitchState(gh.ghoulPetrolState);
            
        }
        //when player in hiding and our known lastposition of player is too close to the object in whcih player
        //is hiding then we got the player "Game Over"
        if(gh.isPlayerHiding && Vector3.Distance(gh.lastKnownPLayerPosition,gh.hideObject.transform.position)<gh.minDisToCatchPlayer)
        {
            gh.GameOver();
        }
        gh.navMeshAgent.destination=gh.player.position;
    }
}


public class GhoulDead: GhoulBase
{
    public override void EnterState(GhoulStateMachine gh)
    {

    }
    public override void UpdateState(GhoulStateMachine gh)
    {

    }
}

public class GhoulStateMachine : MonoBehaviour, IDamagable
{
    #region  variables
    public Color showStateColor; //var for understanding current state in inspection red->PlayerDetected blue->LostPlayer green->petrolState
    public GhoulBase currentState;
    public Animator animationGhoul;
    public Transform mySpawnPos;
    public GameObject myPrefab;
    public float respawnAfterDeathTime=50;
    public GhoulPetrolState ghoulPetrolState=new GhoulPetrolState();
    public GhoulPlayerDetected ghoulPlayerDetected=new GhoulPlayerDetected();
    public GhoulLostPlayer ghoulLostPlayer=new GhoulLostPlayer();
    public GhoulDead ghoulDead=new GhoulDead();
    public Transform myTransform;
    public GameObject myCamera;
    public MonoBehaviour myScript;

    public NavMeshAgent navMeshAgent;


    [Header("Petrol")]
    public List<Transform> petrolPoints=new List<Transform>();
    public float waitTimeInEachPetrolPoint=2f;
    public float PetrolSpeed=1;
    public Transform rayCastpos;
    public LayerMask doorLayer;
    public Queue<Door> door=new Queue<Door>();
    

    [Header("Player Detected")]
    public float minDisToCatchPlayer; //Game Over
    public Transform player;
    public float range=10f; //AI Vision
    public float chaseSpeed=1.5f; //Ai Speed When Detected Player
    public float waitTimeAfterLosing=3f; //After Losing The Player Keep Trackig Playr Position
    [HideInInspector]
    public bool isWaiting=false;
    public float FOV=120; 
    public Transform rayMaker;
    public Transform raychild;
    [HideInInspector]
    public bool isPlayerCrouch=false;
    public float chaseDistance;
    [HideInInspector]
    public Vector3 lastKnownPLayerPosition;
    public float timeKeepFollowingPlayer=4;
    [HideInInspector]
    public bool notDetectedTillnow=false;
    [HideInInspector]
    public bool playerInVision=false;
    [HideInInspector]
    public bool isPlayerHiding=false;
    [HideInInspector]
    public HideObject hideObject;
    public GameEvent OnGameOver;
    [HideInInspector]
    public Camera skelCam;
    
#endregion











    void Start()
    {
        //setNav Mesh Agent
        navMeshAgent=GetComponent<NavMeshAgent>();
        currentState=ghoulPetrolState;
        currentState.EnterState(this);
        //Turn Off Camera of AI
        skelCam=myCamera.GetComponent<Camera>();
        myCamera.gameObject.SetActive(false);
        skelCam.enabled=false;
        //set respawn position
        mySpawnPos=GameObject.FindGameObjectWithTag("Respawn").GetComponent<Transform>();
    }

    
    void Update()
    {
        //cast ray
        drawRay();
        currentState.UpdateState(this);
        //set animation
        animationGhoul.SetFloat("Speed",navMeshAgent.speed);
        
        //when player it too close lets catch 
        if(Vector3.Distance(myTransform.position,player.position)<minDisToCatchPlayer)
        {
           GameOver();
        }
    }
    public void GameOver()
    {
         //stop all ongoing corotuines
            StopAllCoroutines();
            //active camera to show animation
            myCamera.SetActive(true);
            skelCam.enabled=true;
            //Deavtivate player
            player.gameObject.SetActive(false);
            //animation of killing player
            animationGhoul.SetTrigger("NeckBite");
            //deactivate script 
            myScript.enabled=false;
            OnGameOver.Raise(this,true);

    }

    //switch function from one state to another state
    public void SwitchState(GhoulBase ghState)
    {
        currentState=ghState;
        currentState.EnterState(this);
    }



    #region  event

    //when Player crouch listen to that
    public void ListenWhenPlayerCrouch(Component sender,object data)
    {
        if(data is bool && (bool)data )
        {
            isPlayerCrouch=true;
        }else{
            isPlayerCrouch=false;
        }
    }

    //listen when palyer Hide
    public void ListenPlayeIsHinding(Component sender,object data)
    {
        if(data is bool)
        {
            isPlayerHiding=(bool)data;
            hideObject=(HideObject)sender;
        }
    }

    #endregion

    #region  IEnumerator Funs

    public IEnumerator DoorisOn()  //turning off Door
    {
        yield return new WaitForSeconds(5f);
        door.Dequeue().Interact();
    }
    public IEnumerator Wait(float amount) //wating in petrol point or for player
    {
        isWaiting=true;
        navMeshAgent.speed=0;
        yield return new WaitForSeconds(amount);
        isWaiting=false;
        SwitchState(ghoulPetrolState);

    }

    public IEnumerator ChasingPlayer() //chasing player after losing vison
    {
        yield return new WaitForSeconds(timeKeepFollowingPlayer);
        if(currentState==ghoulLostPlayer) 
        {
            //audio speech
            StartCoroutine(Wait(waitTimeAfterLosing));
            notDetectedTillnow=true;
        }
    }

    IEnumerator RespawnAfter(float time)
    {
        SwitchState(ghoulDead);
        navMeshAgent.speed=0;
        this.enabled=false;
        yield return new WaitForSeconds(time);
        myTransform.position=mySpawnPos.position;
        yield return new WaitForSeconds(5);
        this.enabled=true;
        animationGhoul.Play("IdleZoombi");
        SwitchState(ghoulPetrolState);

    }
    #endregion

    public void drawRay()
    {
        RaycastHit hitInfo;

        var ray=new Ray(rayMaker.position,raychild.forward);
        if(Physics.Raycast(ray,out hitInfo))
        {
            Debug.DrawLine(rayMaker.position,hitInfo.point,Color.red);
            if(hitInfo.transform.tag=="Player")
            {
                playerInVision=true;
                lastKnownPLayerPosition=hitInfo.transform.position; //give player position to Main state Machine
                
            }else{
                playerInVision=false;
            }
        }

    }



  public void TookDamage(float amount)
  {
    StopAllCoroutines();
    animationGhoul.SetTrigger("Death");
    StartCoroutine(RespawnAfter(respawnAfterDeathTime));

  } 



}
