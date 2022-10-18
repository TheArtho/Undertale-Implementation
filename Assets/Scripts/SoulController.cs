using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class SoulController : MonoBehaviour
{
    public bool canMove;
    public float speed = 10;
    
    private Vector2 moveDirection;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            Input();
            Movement();
        }
    }

    private void Input()
    {
        moveDirection = InputManager.Main.GetPlayerMovement();
    }

    private void Movement()
    {
        transform.GetComponent<RectTransform>().localPosition += new Vector3(moveDirection.x * speed * Time.deltaTime, moveDirection.y * speed * Time.deltaTime, 0);
    }
}
