using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
public class InputReader : ScriptableObject, PlayerInput.IPlayerActions, PlayerInput.IChatActions
{
    private PlayerInput playerInput;
    public event UnityAction<Vector2> MoveEvent = delegate { }; 
    public event UnityAction JumpEvent = delegate { }; 
    public event UnityAction ChatEvent = delegate { }; 
    public event UnityAction CancelChatEvent = delegate { }; 
    public event UnityAction SendChatEvent = delegate { }; 
    
    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent.Invoke(context.ReadValue<Vector2>());
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.performed)
            JumpEvent.Invoke();
    }

    public void OnChat(InputAction.CallbackContext context)
    {
        if(context.performed)
            ChatEvent.Invoke();
    }

    public void OnCancelChat(InputAction.CallbackContext context)
    {
        if(context.performed)
            CancelChatEvent.Invoke();
    }

    public void OnSendChat(InputAction.CallbackContext context)
    {
        if(context.performed)
            SendChatEvent.Invoke();
    }
    
    private void OnEnable()
    {
        if (playerInput == null)
        {
            playerInput = new PlayerInput();
            playerInput.Player.SetCallbacks(this);
            playerInput.Player.Enable();
            playerInput.Chat.SetCallbacks(this);
            playerInput.Chat.Enable();
        }
    }
}