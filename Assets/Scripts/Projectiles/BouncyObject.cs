using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BouncyObject : MonoBehaviour
{
    [SerializeField] private float horizontalSpeed = 1;
    [SerializeField] private float bounciness = 1;
    private RectTransform _rect;
    private BoxBoundaries _boxBoundaries;
    private RectPhysics _rigidBody;

    private float _horizontalDirection;
    private float _horizontalCoefficient;
    
    // Start is called before the first frame update
    void Start()
    {
        _rect = GetComponent<RectTransform>();
        _boxBoundaries = transform.parent.parent.parent.GetComponent<BoxBoundaries>();
        _rigidBody = GetComponent<RectPhysics>();

        _horizontalCoefficient = (Random.value > 0.5f) ? 1 : -1;
    }

    // Update is called once per frame
    void Update()
    {
        // Move
        _rect.position += new Vector3(horizontalSpeed*_horizontalCoefficient, 0, 0) * Time.deltaTime;
        
        // Check boundaries
        if (_boxBoundaries.OnEnterBoundaries(BoxBoundaries.Left, _rect))
        {
            _rect.position = new Vector3(_boxBoundaries.getBoundariesLeft() + _rect.sizeDelta.x*0.5f, _rect.position.y, 0);
            _horizontalCoefficient *= -1;
        }
        else if (_boxBoundaries.OnEnterBoundaries(BoxBoundaries.Right, _rect))
        {
            _rect.position = new Vector3(_boxBoundaries.getBoundariesRight() - _rect.sizeDelta.x*0.5f, _rect.position.y, 0);
            _horizontalCoefficient *= -1;
        }
        
        // Bounce up when hitting the floor
        if (_boxBoundaries.OnEnterBoundaries(BoxBoundaries.Down, _rect))
        {
            _rect.position = new Vector3(_rect.position.x, _boxBoundaries.getBoundariesDown() + _rect.sizeDelta.y*0.5f, 0);
            _rigidBody.velocity = Vector2.zero;
            _rigidBody.Impulse(new Vector2(0, bounciness));
        }
    }
}
