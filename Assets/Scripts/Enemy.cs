using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;

    private Player _player;

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        //move down 4m per second
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        /* 
          if at bottom of screen
          respawn at top with random x position 
        */
        if(transform.position.y < -4)
        {
            transform.position = new Vector3(Random.Range(-9f, 9f), 7, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        

        if (other.tag == "Player")
        {
           
            if(_player != null)
            {
                _player.Damage();
            }

            Destroy(gameObject);
        }else if (other.tag == "Laser")
        {
            Destroy(other.gameObject);

            if (_player != null)
            {
                _player.AddScore();
            }

            Destroy(this.gameObject);
        }
    }
}
