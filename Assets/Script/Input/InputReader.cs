using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using static GameInput;



[CreateAssetMenu(fileName = "New Input Reader", menuName = "Input/Input Reader")]
public class InputReader : ScriptableObject,IGameActions
{
    public enum E_ActionMap
    {
        Game,
    }



    [SerializeField] E_ActionMap e_ActionMap;
    public event Action<bool> OnJumpEvent,OnInteractEvent,OnGrabEvent,OnCrouchEvent;
    public event Action<bool> OnFireEvent;
    public event Action OnBackEvent,OnFlashlightEvent;
    public event Action<Vector2> OnMoveEvent,OnAimEvent;
    private GameInput control;


    





    private void OnEnable()
    {
        if(control==null)
        {
            control=new GameInput();
            control.Game.SetCallbacks(this);
        }
        
            control.Enable();

    }
    #region  Game
    
    #endregion


    public void OnControlMapChanged(E_ActionMap maptype)
    {
        if(maptype!=e_ActionMap){ //currectly activated map is not this map
            Debug.Log("Deactivating map:"+e_ActionMap);
            control.Disable();
        }else{
            control.Enable();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        OnMoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        OnAimEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if(context.performed)OnInteractEvent?.Invoke(true);
        else if(context.canceled)OnInteractEvent?.Invoke(false);
    }

    public void OnGrab(InputAction.CallbackContext context)
    {
        if(context.performed)OnGrabEvent?.Invoke(true);
        else if(context.canceled)OnGrabEvent?.Invoke(false);
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if(context.performed)OnFireEvent?.Invoke(true);
        else if(context.canceled)OnFireEvent?.Invoke(false);
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if(context.performed)OnCrouchEvent?.Invoke(true);
        else if(context.canceled)OnCrouchEvent?.Invoke(false);
    }

    public void OnFlashLight(InputAction.CallbackContext context)
    {
        if(context.performed) OnFlashlightEvent?.Invoke();
    }
}