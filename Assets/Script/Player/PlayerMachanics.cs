using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMachanics : MonoBehaviour
{
    [SerializeField] InputReader gameInput;
    private Ray ray;
    private RaycastHit hit;
    private IInteractable currentInteractable ;
    private IGrabable currentGrabable;
    
    [SerializeField] bool alreadyInHand=false;
    [SerializeField] IGrabable data;
    [SerializeField] float distance=5.0f;


    private bool isPlayerHiding=false;
    private Transform cameraTransofm;
    private Transform playerTransform;
    private HideObject hdObject;
    private UIManager uIManager;
    [SerializeField] Transform lastPlayerTransform,lastCameraTransfrom;
    [SerializeField] GameEvent playerIsHindingEvent;    
    
    
    

    void Start()
    {
        cameraTransofm=GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        playerTransform=GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        uIManager=FindObjectOfType<UIManager>();

        SubcribeToInput(true);
    }

    void Update()
    {   
        if(isPlayerHiding)
        {
            playerIsHindingEvent.Raise(hdObject,true); //player is hiding call the event
            if(Input.GetKeyDown(KeyCode.Q))
            {
                Hide(null,null);
                playerIsHindingEvent.Raise(hdObject,false);//player is back to game call the event
            }
            return;
        }

        
        #region IIntractable
       ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

       if (Physics.Raycast(ray, out hit, distance))
        {
            if(hit.collider.TryGetComponent<IInteractable>(out var interactable))
            {
                // hitting an IInteractable -> store
                SetInteractable(interactable);
            }
            else
            {
                // hitting something that is not IInteractable -> reset
                SetInteractable(null);
            }


            //IGrabable
            if(hit.collider.TryGetComponent<IGrabable>(out var grabable))
            {
                // hitting an IInteractable -> store
                SetIGrabable(grabable);
            }
            else
            {
                // hitting something that is not IInteractable -> reset
                SetIGrabable(null);
            }
            
        }else{
            SetInteractable(null);
        }
        #endregion
        if(Input.GetKeyDown(KeyCode.E))
            {
                // if(!alreadyInHand && currentGrabable!=null )
                // {
                //     currentGrabable.GrabInHand();
                //     alreadyInHand=true;
                // }else if(alreadyInHand)
                // {

                //     PlayerHand.RemoveFromHand();
                //     alreadyInHand=false;
                // }
            }  
       
        // if currently focusing an IInteractable and click -> interact
        if(currentInteractable != null && Input.GetKeyDown(KeyCode.Q))
        {
            //currentInteractable.Interact();
        }

        if(Input.GetMouseButtonDown(2))
        {
            Cursor.lockState=CursorLockMode.Locked;
        }else if(Input.GetMouseButtonDown(1))
        {
            Cursor.lockState=CursorLockMode.None;
        }

        

    }

    //if something is in hand Event
    public void CallByHandEvent(Component sender,object data)
    {
        if(data is IGrabable)
        {
            this.data=(IGrabable)data;
        }
        if(data is null)
        {
            alreadyInHand=false;
        }
    }

    //fun to hind in the sender object
    public void Hide(Transform PositonInObjectForCamera,HideObject obj)
    {
        if(!isPlayerHiding) //Player wants to hide
        {   
            hdObject=obj;
            hdObject.SetPos();
            isPlayerHiding=true;

            lastPlayerTransform.position=playerTransform.position;    //save player position before hinding

            lastCameraTransfrom.SetParent(playerTransform); //temp transform to keeptrack of position of camera
            lastCameraTransfrom.localPosition=cameraTransofm.localPosition; //camera position before hiding
            cameraTransofm.SetParent(PositonInObjectForCamera); //taking camera and putting in the HideObject
           
            cameraTransofm.position=PositonInObjectForCamera.position; 
            playerTransform.position=FindObjectOfType<GameManager>().GetDummyPosition(); //taking away player from the floor
        }else{
            if(hdObject!=null) hdObject.SetBackPos(); //set back the object in whcich palyer is hiding
            isPlayerHiding=false;
            playerTransform.position=lastPlayerTransform.position; //setting player back to its previous position
            cameraTransofm.position=lastCameraTransfrom.position; //setting camera 
            cameraTransofm.SetParent(playerTransform); //parenting camera to player

        }
    }


    private void SetInteractable(IInteractable interactable)
    {
        // if is same instance (or both null) -> ignore
        if(currentInteractable == interactable) return;

        // otherwise if current focused exists -> reset
        if(currentInteractable != null) currentInteractable.Looking=false;

        // store new focused
        currentInteractable = interactable;

        // if not null -> set looking
        if(currentInteractable != null) 
        {
            currentInteractable.Looking=true;
            uIManager.SetKeySuggest("Q");
        }else{
          
            uIManager.SetKeySuggest("empty");
        
        }
    }
    private void SetIGrabable(IGrabable grabable)
    {
        // if is same instance (or both null) -> ignore
        if(currentGrabable== grabable) return;

        // otherwise if current focused exists -> reset
        if(currentGrabable!= null) currentGrabable.Looking=false;

        // store new focused
        currentGrabable = grabable;

        // if not null -> set looking
        if(currentGrabable != null) {
            currentGrabable.Looking=true;
            uIManager.SetKeySuggest("E");
        }else{
          
            uIManager.SetKeySuggest("empty");
        
        }
    }
   

    //set player position
   public void SetlPlayerPos(Vector3 Pos)
   {
        try{playerTransform.position=Pos;}
        catch(UnassignedReferenceException ex)
        {
            print(ex);
        }
   }



    void OnDisable()
    {
        SubcribeToInput(false);
    }


    void SubcribeToInput(bool val)
    {
        if(val)
        {
            gameInput.OnInteractEvent += OnInteract;
            gameInput.OnGrabEvent += OnGrab;
        }else{
            gameInput.OnInteractEvent -= OnInteract;
            gameInput.OnGrabEvent -= OnGrab;
        }
    }






   void OnInteract(bool performed)
   {
    if(performed)
    {
       currentInteractable?.Interact();
    }
   }


   void OnGrab(bool performed)
   {
    if(performed)
        if(!alreadyInHand && currentGrabable!=null )
        {
            currentGrabable.GrabInHand();
            alreadyInHand=true;
        }else if(alreadyInHand)
        {

            PlayerHand.RemoveFromHand();
            alreadyInHand=false;
        }
   }
}
