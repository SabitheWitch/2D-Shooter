﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    // speed variable
    [SerializeField]
    private float _speed = 8f;
    [SerializeField]
    private bool _isEnemyLaser = false;

    private Player _player;
    void Start()
    {
     
    }

    
    void Update()
    {       
       if(_isEnemyLaser == false)
        {
            MoveUp();
        }
        else
        {
            MoveDown();
        }
    }

    void MoveUp()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);



        if (transform.position.y > 8f)
        {

            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(gameObject);
        }
    }

    void MoveDown()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);



        if (transform.position.y < -8)
        {

            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(gameObject);
        }
    }

    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && _isEnemyLaser == true)
        {
            Player _player = collision.GetComponent<Player>();
           if(_player != null)
            {
                _player.Damage();
            }

            if(transform.parent != null) 
            { 
            Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }
}
