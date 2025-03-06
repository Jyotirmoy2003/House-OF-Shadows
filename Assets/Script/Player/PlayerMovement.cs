using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] InputReader gameInput;
    [SerializeField] GameEvent OnPlayerCrouch;
    [SerializeField]  CharacterController controller;
    [SerializeField]  float speed;

    [Range(.1f,3f)]
    [SerializeField] float FootStepPitch=.2f;
    Vector3 velocity;
    private AudioSource footStep;
    private Transform myTransform;
    private Vector2 inputVector = Vector2.zero;

    void Start()
    {
        //Play footstep sound
        AudioManager.instance.PlaySound("FootStep",this.gameObject);
        footStep=GetComponent<AudioSource>();
        myTransform=GetComponent<Transform>();
        //start the corotine to play the sound after a little delay
        StartCoroutine(playFootStep());

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
            gameInput.OnMoveEvent += MoveInput;
            gameInput.OnCrouchEvent += CrouchInput;
        }else{
            gameInput.OnMoveEvent -= MoveInput;
            gameInput.OnCrouchEvent -= CrouchInput;
        }
    }

    void MoveInput(Vector2 inputVector)
    {
        this.inputVector = inputVector;
    }


    void CrouchInput(bool performed)
    {
        //crouch
        if(performed)
        {
            myTransform.localScale=new Vector3(myTransform.lossyScale.x,myTransform.localScale.y/2,myTransform.localScale.z);
            speed=speed/2;
            OnPlayerCrouch.Raise(this,true);
            FootStepPitch/=2;

        }else
        {
            myTransform.localScale=new Vector3(myTransform.lossyScale.x,myTransform.localScale.y*2,myTransform.localScale.z);
            speed=speed*2;
            OnPlayerCrouch.Raise(this,false);
            FootStepPitch*=2;
        }
    }


    void Update()
    {
        Move();
    }

    void Move()
    {
        //input
        Vector3 move=myTransform.right*inputVector.x+myTransform.forward*inputVector.y;
        controller.Move(move*speed*Time.deltaTime);
        controller.Move(velocity*Time.deltaTime);

        if(move.magnitude>=0.1f)
        {
            footStep.pitch=FootStepPitch;
        }else{
            footStep.pitch=0;
        }
    }

    IEnumerator playFootStep()
    {
        footStep.enabled=false;
        yield return new WaitForSeconds(2);
        footStep.enabled=true;
        PlayerHintManager.Instance.SetPlayerHintText("Where am I?...\n Its Too Dark here,I should find lights",10,this);
    }
}
