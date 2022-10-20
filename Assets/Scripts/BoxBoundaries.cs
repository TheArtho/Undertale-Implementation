using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxBoundaries : MonoBehaviour
{
    private enum Side
    {
        Left = 0,
        Right = 1,
        Up = 2,
        Down = 3
    }

    public const int Left = (int) Side.Left;
    public const int Right = (int) Side.Right;
    public const int Up = (int) Side.Up;
    public const int Down = (int) Side.Down;

    private BoxCollider2D[] _colliders;
    private Image _box;
    
    void Awake()
    {
        _colliders = GetComponents<BoxCollider2D>();
        _box = GetComponent<Image>();
    }
    
    void Update()
    {
        for (int i = 0; i < _colliders.Length; ++i)
        {
            switch ((Side) i)
            {
                case Side.Left:
                    _colliders[i].offset = new Vector2(-0.5f*_box.rectTransform.sizeDelta.x, 0);
                    _colliders[i].size = new Vector2(10, _box.rectTransform.sizeDelta.y);
                    break;
                case Side.Right:
                    _colliders[i].offset = new Vector2(0.5f*_box.rectTransform.sizeDelta.x, 0);
                    _colliders[i].size = new Vector2(10, _box.rectTransform.sizeDelta.y);
                    break;
                case Side.Up:
                    _colliders[i].offset = new Vector2(0, 0.5f*_box.rectTransform.sizeDelta.y);
                    _colliders[i].size = new Vector2(_box.rectTransform.sizeDelta.x, 10);
                    break;
                case Side.Down:
                    _colliders[i].offset = new Vector2(0, -0.5f*_box.rectTransform.sizeDelta.y);
                    _colliders[i].size = new Vector2(_box.rectTransform.sizeDelta.x, 10);
                    break;
            }
        }
    }

    public float getBoundariesLeft()
    {
        return _box.rectTransform.position.x + -0.5f * _box.rectTransform.sizeDelta.x + _box.sprite.border.x;
    }
    
    public float getBoundariesRight()
    {
        return _box.rectTransform.position.x + 0.5f * _box.rectTransform.sizeDelta.x - _box.sprite.border.x;
    }
    
    public float getBoundariesUp()
    {
        return _box.rectTransform.position.y + 0.5f*_box.rectTransform.sizeDelta.y - _box.sprite.border.y;
    }
    
    public float getBoundariesDown()
    {
        return _box.rectTransform.position.y + -0.5f*_box.rectTransform.sizeDelta.y + _box.sprite.border.y;
    }

    public bool OnEnterBoundaries(int b, RectTransform rect)
    {
        return ((Side) b) switch
        {
            Side.Left => rect.position.x < this.getBoundariesLeft() + rect.sizeDelta.x * 0.5f,
            Side.Right => rect.position.x > this.getBoundariesRight() - rect.sizeDelta.x * 0.5f,
            Side.Up => rect.position.y > this.getBoundariesUp() - rect.sizeDelta.y * 0.5f,
            Side.Down => rect.position.y < this.getBoundariesDown() + rect.sizeDelta.y * 0.5f,
            _ => throw new ArgumentOutOfRangeException(nameof(b), b, null)
        };
    }
}
