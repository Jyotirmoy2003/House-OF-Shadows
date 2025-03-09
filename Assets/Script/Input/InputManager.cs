using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Jy_Util;


public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set;}

   
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }else{
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }


    public PlayerInput playerInput;
    [SerializeField] GameEvent  InputDeviceChangedEvent,ActionMapChangedEvent;
    [SerializeField] List<InputReader> allinoutreadedInGame=new List<InputReader>();
    [Header("Rumble Settings")]

    private Gamepad pad;
    private bool ISRumbling = false;
    private Coroutine rumbleCoroutine;

    [SerializeField] float lightRumbleAmount = 0.2f;
    [SerializeField] float mediumRumbleAmount = 0.5f;
    [SerializeField] float hardRumbleAmount = 1f;

    public E_InputType currentInput;
    public string currentActionMap;


    void Start()
    {
        playerInput.onControlsChanged+=OnSwitchControlls;
       
       
       OnSwitchControlls(playerInput);

        Invoke(nameof(stopRumble),5f);
       
    }

    void stopRumble()
    {
        pad=Gamepad.current;
        pad?.SetMotorSpeeds(0f,0f);
        ISRumbling=false;
    }

   

    public void OnSwitchControlls(PlayerInput input)
    {
        Debug.Log(input.currentControlScheme);
        if(input.currentControlScheme =="Gamepad")
        {
            InputDeviceChangedEvent.Raise(this,E_InputType.Gamepad);
            currentInput=E_InputType.Gamepad;
        }else if(input.currentControlScheme=="Keyboard"){
            InputDeviceChangedEvent.Raise(this,E_InputType.Keyboard);
            currentInput=E_InputType.Keyboard;
        }else{
            InputDeviceChangedEvent.Raise(this,E_InputType.None);
            currentInput=E_InputType.None;
        }
    }

   private void Update()
    {
        currentActionMap=playerInput.currentActionMap.ToString();
        // Automatically detect if the gamepad is disconnected and reset if needed
        // Ensure we always have a valid gamepad reference
        if (pad == null || pad != Gamepad.current)
        {
            ISRumbling = false; // Reset rumble state if controller changed
            pad = Gamepad.current; // Try to get the current connected gamepad
        }
    }

    public void Rumble(float amount)
    {
        if (ISRumbling)
        {
            Debug.Log("Already rumbling");
            return;
        }
        Rumble(amount, amount, 0.2f);
    }

    public void Rumble(float lowFrq, float highFrq, float duration)
    {
        if (ISRumbling) return; // Prevent overlapping calls

        pad = Gamepad.current;
        if (pad == null) return; // Ensure controller is valid

        Debug.Log("Starting rumble");
        ISRumbling = true;
        pad.SetMotorSpeeds(lowFrq, highFrq);

        // Stop previous coroutine if it's running
        if (rumbleCoroutine != null)
            StopCoroutine(rumbleCoroutine);

        rumbleCoroutine = StartCoroutine(RumbleTime(duration));
    }

    public void StopRumble()
    {
        if (pad != null)
        {
            pad.SetMotorSpeeds(0f, 0f);
        }
        ISRumbling = false;
        Debug.Log("Forced rumble stop");
    }

    private IEnumerator RumbleTime(float duration)
    {
        yield return new WaitForSeconds(duration);

        if (pad != null)
        {
            pad.SetMotorSpeeds(0f, 0f);
        }
        ISRumbling = false;
        Debug.Log("Rumble stopped");
    }

    public void LightRumble() => Rumble(lightRumbleAmount);
    public void MediumRumble() => Rumble(mediumRumbleAmount);
    public void HardRumble() => Rumble(hardRumbleAmount);
  

    // void Update()
    // {
    //     currentActionMap=playerInput.currentActionMap.ToString();
    // }
    
}
