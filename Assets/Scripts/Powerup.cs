using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float speed = 3f;
    
    [SerializeField]
    private int powerupID;
    [SerializeField]
    private AudioClip _powerupSound;

    

    // Update is called once per frame
    void Update()
    {       
        transform.Translate(Vector3.down * speed * Time.deltaTime);
        if (transform.position.y < -5f)
        {
            Destroy(gameObject);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        
        if (collision.tag == "Player")
        {
            Player player = collision.transform.GetComponent<Player>();

            AudioSource.PlayClipAtPoint(_powerupSound, transform.position);

            if(player != null)
            {
                switch (powerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        player.ShieldActive();
                        break;
                    case 3:
                        player.AddAmmo();                        
                        break;
                    case 4:
                        player.AddLife();
                        break;
                    case 5:
                        player.MultiShotActive();
                        break;
                    case 6:
                        player.NegSpeedActive();
                        break;
                    default:
                        break;
                }
            }            
            Destroy(this.gameObject);
        }
    }
    
}
