using System.Collections;
using System.Collections.Generic;
using Jy_Util;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] InputReader gameInput;
    [SerializeField] float sensetivity;
    [SerializeField] Transform playerBody;
    
    private Vector2 lookInput;
    private float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        sensetivity = PlayerPrefs.GetFloat("mouse", 10f);

        SubscribeToInput(true);
    }

    void OnDisable()
    {
        SubscribeToInput(false);
    }

    void SubscribeToInput(bool val)
    {
        if (val)
            gameInput.OnAimEvent += LookInput;
        else
            gameInput.OnAimEvent -= LookInput;
    }

    void Update() // Use Update instead of FixedUpdate for mouse movement
    {
        //OldInput();
        NewInput();
    }

    void OldInput()
    {
        float mouseX=Input.GetAxis("Mouse X")*sensetivity*Time.deltaTime;
        float mouseY=Input.GetAxis("Mouse Y")*sensetivity*Time.deltaTime;

        xRotation-=mouseY;
        xRotation=Mathf.Clamp(xRotation, -90f,90f);
        transform.localRotation=Quaternion.Euler(xRotation,0f,0f);

        playerBody.Rotate(Vector3.up*mouseX);
    }

    void NewInput()
    {
        float mouseX = lookInput.x * sensetivity * Time.deltaTime;
        float mouseY = lookInput.y * sensetivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    void LookInput(Vector2 newLookInput)
    {
        lookInput = newLookInput;
    }

    public void ListenToOnInputChange(Component sender,object data)
    {
        switch((E_InputType)data)
        {
            case E_InputType.Gamepad:
                sensetivity = 100f;
                break;
            case E_InputType.Keyboard:
                sensetivity = 20f;
                break;
        }
    }
}

