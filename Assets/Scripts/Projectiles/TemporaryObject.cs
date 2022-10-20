using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryObject : MonoBehaviour
{
    [SerializeField] private float lifeTime = 2;
    
    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(DestroyObject), lifeTime);
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
