using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public enum Key
    {
        UpArrow = 0,
        DownArrow = 1,
        LeftArrow = 2,
        RightArrow = 3,
        Select = 4,
        Cancel = 5
    }

    public static InputManager Main;
    private PlayerController _playerControls;

    private void Awake()
    {
        if (Main != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Main = this;
            _playerControls = new PlayerController();
        }
    }

    private void OnEnable()
    {
        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }

    public Vector2 GetPlayerMovement()
    {
        return Main._playerControls.Player.Move.ReadValue<Vector2>();
    }
    
    public bool GetKeyDown(Key k)
    {
        return k switch
        {
            Key.UpArrow => Main._playerControls.Player.Up.WasPressedThisFrame(),
            Key.DownArrow => Main._playerControls.Player.Down.WasPressedThisFrame(),
            Key.LeftArrow => Main._playerControls.Player.Left.WasPressedThisFrame(),
            Key.RightArrow => Main._playerControls.Player.Right.WasPressedThisFrame(),
            Key.Select => Main._playerControls.Player.Select.WasPressedThisFrame(),
            Key.Cancel => Main._playerControls.Player.Cancel.WasPressedThisFrame(),
            _ => false
        };
    }
    
    public bool GetKeyUp(Key k)
    {
        return k switch
        {
            Key.UpArrow => Main._playerControls.Player.Up.WasReleasedThisFrame(),
            Key.DownArrow => Main._playerControls.Player.Down.WasReleasedThisFrame(),
            Key.LeftArrow => Main._playerControls.Player.Left.WasReleasedThisFrame(),
            Key.RightArrow => Main._playerControls.Player.Right.WasReleasedThisFrame(),
            Key.Select => Main._playerControls.Player.Select.WasReleasedThisFrame(),
            Key.Cancel => Main._playerControls.Player.Cancel.WasReleasedThisFrame(),
            _ => false
        };
    }
}

