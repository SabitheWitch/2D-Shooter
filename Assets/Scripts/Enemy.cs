using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;
    void Start()
    {
        
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

    private void OnTriggerEnter(Collider other)
    {
        /* if other is player
         * Destroy enemy
         * 
         * if other is laser
         * destroy laser
         * destroy enemy */

        if (other.tag == "Player")
        {
            //damage player
            Player player = other.transform.GetComponent<Player>();
            if(player != null)
            {
                player.Damage();
            }

            Destroy(gameObject);
        }else if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }
}
