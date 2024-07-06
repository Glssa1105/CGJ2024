using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static PlayerInputAction;

public class InputController : IGameplayActions, IUIActions
{
    public enum InputType
    {
        Gameplay ,UI,
    }
    private PlayerInputAction playerInput;
    private InputController() {
        playerInput = new PlayerInputAction();
        playerInput.Gameplay.SetCallbacks(this);
        playerInput.UI.SetCallbacks(this);  
        //BeginPlayType
        SetInputType(InputType.Gameplay);

    }
    private static InputController instance;
    public static InputController Instance {  get {
            if(instance == null)
            {
                instance = new InputController();  
            }
            return instance; } 
    }

    public void DeleteController()
    {

    }


    public Vector2 moveVector { get; private set; }
    public UnityAction pauseAction;
    public UnityAction resumeAction;

    public void SetInputType(InputType type)
    {
        foreach(InputActionMap t in playerInput.asset.actionMaps)
        {
            if (t.name == type.ToString())
            {
                t.Enable();
            }
            else
            {
                t.Disable();
            }
        }
        
    }


    #region Gameplay
    public void OnMove(InputAction.CallbackContext context)
    {
        moveVector = context.ReadValue<Vector2>();
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
        {
            pauseAction?.Invoke();
            SetInputType(InputType.UI);
        }
    }

    #endregion
    public void OnResume(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
        {
            resumeAction?.Invoke();
            SetInputType(InputType.Gameplay);
        }
    }

    #region UI



    #endregion
}
