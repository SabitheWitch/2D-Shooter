using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detection : MonoBehaviour
{
    private TurningEnemy _enemy;
    
    void Start()
    {
        _enemy = transform.parent.GetComponent<TurningEnemy>();
        if(_enemy == null)
        {
            Debug.Log("Turning Enemy is null!");
        }        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Powerup")
        {
            _enemy.MakePowerupTarget();            
        }
    }
}
