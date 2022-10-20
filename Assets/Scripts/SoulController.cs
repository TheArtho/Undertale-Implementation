using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class SoulController : MonoBehaviour
{
    [SerializeField] private AudioDatabase audio;
    [SerializeField] private BoxBoundaries boundaries;
    [SerializeField] private Slider HPBar;
    [SerializeField] private Text HPText;
    public bool canMove;
    public float speed = 10;
    [SerializeField] private bool invincible;
    [SerializeField] private float invincibleDelay;
    
    private Vector2 _moveDirection;
    
    [SerializeField] private int hp;
    public int Hp
    {
        get => hp;
        set
        {
            if (value < 0)
            {
                hp = 0;
                GameOver();
            }
            else if (value > maxHP)
            {
                hp = maxHP;
            }
            else
            {
                hp = value;
            }
        }
    }
    [SerializeField] private int maxHP;

    // Start is called before the first frame update
    void Start()
    {
        //TODO set the values with the Player's stats
        
        // HP Bar Event Listener
        HPBar.onValueChanged.AddListener (delegate {OnHpUpdate();});
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
        _moveDirection = InputManager.Main.GetPlayerMovement();
    }

    private void Movement()
    {
        RectTransform rect = GetComponent<RectTransform>();
        rect.position += new Vector3(_moveDirection.x * speed * Time.deltaTime, _moveDirection.y * speed * Time.deltaTime, 0);
        
        if (boundaries.OnEnterBoundaries(BoxBoundaries.Right, rect))
        {
            rect.position = new Vector3(boundaries.getBoundariesRight() - rect.sizeDelta.x*0.5f, rect.position.y, 0);
        }
        else if (boundaries.OnEnterBoundaries(BoxBoundaries.Left, rect))
        {
            rect.position = new Vector3(boundaries.getBoundariesLeft() + rect.sizeDelta.x*0.5f, rect.position.y, 0);
        }
        
        if (boundaries.OnEnterBoundaries(BoxBoundaries.Up, rect))
        {
            rect.position = new Vector3(rect.position.x, boundaries.getBoundariesUp() - rect.sizeDelta.y*0.5f, 0);
        }
        else if (boundaries.OnEnterBoundaries(BoxBoundaries.Down, rect))
        {
            rect.position = new Vector3(rect.position.x, boundaries.getBoundariesDown() + rect.sizeDelta.y*0.5f, 0);
        }
    }

    public void Damage(int damage)
    {
        Debug.Log($"Player is damaged by #{damage}");
        AudioHandler.Main.PlaySFX(audio.Get("hurt"));
        Hp -= damage;
        HPBar.maxValue = maxHP;
        HPBar.value = Hp;
    }

    public void SetInvincible()
    {
        invincible = true;
    }

    public void SetVulnerable()
    {
        invincible = false;
    }

    public void GameOver()
    {
        Debug.Log("Game Over");
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("DamagePlayer") && !invincible)
        {
            // Damage value hard coded
            Damage(1);
            SetInvincible();
            Invoke(nameof(SetVulnerable), invincibleDelay);
        }
    }

    private void OnHpUpdate()
    {
        HPText.text = $"{Hp} / {maxHP}";
    }
}
