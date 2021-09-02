using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankEnemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 1.5f;
    
    private Player _player;

    [SerializeField]
    private GameObject _explosion;

    private Transform _target;

    SpawnManager _spawnManager;

    private int _life;

    private AudioSource _explosionSound;

    [SerializeField]
    private GameObject _hitOne;
    [SerializeField]
    private GameObject _hitTwo;
    void Start()
    {
        _life = 3;
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.Log("The player is NULL");
        }        

        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

        _target = _player.transform;
        _hitOne.gameObject.SetActive(false);
        _hitTwo.gameObject.SetActive(false);

        _explosionSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement(); 
    }

    void CalculateMovement()
    {
        if(transform.position.y > _target.position.y)
        {
            float distance = _speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _target.position, distance);
        }
        else
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }

        if (transform.position.y < -4)
        {
            transform.position = new Vector3(Random.Range(-9f, 9f), 9f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" || collision.tag == "Laser")
        {            
            _life--;
            _explosionSound.Play();
            if(collision.tag == "Player")
            {
                if(_player != null)
                {
                    _player.Damage();
                } 
            }
            else if (collision.tag == "Laser")
            {
                Destroy(collision.gameObject);
            }

            if (_life == 2)
            {
                _hitOne.SetActive(true);
            }
            else if (_life == 1)
            {
                _hitTwo.SetActive(true);
            }
            else if (_life == 0)
            {
                if (_player != null)
                {
                    _player.AddScore(30);
                }
                _spawnManager.EnemyDeath();
                _speed = 0;
                Instantiate(_explosion, transform.position, Quaternion.identity);
                Destroy(GetComponent<Collider2D>());
                Vector3 enemyPosition = gameObject.transform.position;
                _spawnManager.EnemyDrop(enemyPosition);
                Destroy(this.gameObject);
            }
        }
    }
}
