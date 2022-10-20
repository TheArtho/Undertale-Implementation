using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectPhysics : MonoBehaviour
{
    public const float gravity = 1;
    
    public bool hasGravity;
    public float gravityMultiplier;
    public Vector2 velocity = new Vector2();

    private RectTransform _rect;
    
    void Awake()
    {
        _rect = GetComponent<RectTransform>();
    }
    
    void Update()
    {
        CalculateVelocity();

        _rect.position += new Vector3(velocity.x, velocity.y, 0) * Time.deltaTime * 100;
    }

    private void CalculateVelocity()
    {
        if (hasGravity)
        {
            velocity -= Vector2.one * _rect.up * gravity * gravityMultiplier * Time.deltaTime;
        }
    }

    public void Impulse(Vector2 v)
    {
        velocity += v;
    }
}
