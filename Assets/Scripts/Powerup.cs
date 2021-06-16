using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float speed = 3f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        /*Move down at speed of 3
         * Destroy when past screen*/
        transform.Translate(Vector3.down * speed * Time.deltaTime);
        if (transform.position.y < -5f)
        {
            Destroy(gameObject);
        }
    }
    /*OnTriggerCollision
     * ONLY collectible by player (tags are your friend)*/
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //communicate with player script
        
        if (collision.tag == "Player")
        {
            Player player = collision.transform.GetComponent<Player>();
            if(player != null)
            {
                player.TripleShotActive();
            }

            Destroy(this.gameObject);
        }
    }
}
