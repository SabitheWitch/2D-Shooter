using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurningEnemy : MonoBehaviour
{
    [SerializeField]
    private GameObject _missile;    

    [SerializeField]
    private float _speed = 3f;

    [SerializeField]
    private GameObject _explosion;

    private Player _player;
    private SpawnManager _spawnManager;    

    private bool _isPlayerTarget = false;
    private bool _isPowerupTarget = false;
    private bool _canFire = true;
    private bool _canShootMissile = true;
  

    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.Log("The player is NULL");
        }

        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if(transform.position.y > 4 && _canFire == true)
        {
            _isPlayerTarget = true;            
            FireMissile();
        }

        if(_isPowerupTarget == true && _canShootMissile == true)
        {
            FireMissile();
        }
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
             
        if(transform.position.y >= 12)
        {
            transform.Rotate(new Vector3(0, 0, 180));
        }

        if (transform.position.y < -10)
        {
            transform.Rotate(new Vector3(0, 0, 180));
        }
    }

    public void FireMissile()
    {        
        if (_isPlayerTarget == true)
        {                        
            Instantiate(_missile, transform.position, Quaternion.identity);
            _canFire = false;
        } else if (_isPowerupTarget == true)
        {
            Instantiate(_missile, transform.position, Quaternion.identity);
            _canShootMissile = false;
        }    
    }

    public void MakePowerupTarget()
    {
        _isPowerupTarget = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" || collision.tag == "Laser")
        {
            if(collision.tag == "Player")
            {
                if(_player != null)
                {
                    _player.Damage();
                }
                _speed = 0f;
                Instantiate(_explosion, transform.position, Quaternion.identity);
                Destroy(GetComponent<Collider2D>());
                Destroy(this.gameObject, 2.5f);
            }

            if(collision.tag == "Laser")
            {
                if(_player != null)
                {
                    _player.AddScore(20);
                }
                _spawnManager.EnemyDeath();
                _speed = 0f;
                Instantiate(_explosion, transform.position, Quaternion.identity);
                Destroy(GetComponent<Collider2D>());
                Vector3 enemyPosition = gameObject.transform.position;
                _spawnManager.EnemyDrop(enemyPosition);
                Destroy(this.gameObject, 2.5f);
            }
        }
    }
}
