using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    // speed variable
    [SerializeField]
    private float _speed = 12.5f;
    void Start()
    {
        
    }

    
    void Update()
    {

        //offset laser spawn position north

        
        
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

       

        if(transform.position.y > 8)
        {
            Destroy(gameObject);
        }
    }
}
