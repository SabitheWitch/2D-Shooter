using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3f;
    [SerializeField]
    private GameObject _laserPrefab;

    private float _fireRate = 3.0f;
    private float _canFire = -1f;

    private Player _player;

    private Animator _enemyDestroyed;

    private AudioSource _explosionSound;

    SpawnManager _spawnManager;

    private int _movementType;

    private bool _canShoot;

    void Start()
    {
        _movementType = Random.Range(0, 3);
        _canShoot = true;

        _player = GameObject.Find("Player").GetComponent<Player>();
        if(_player == null)
        {
            Debug.Log("The player is NULL");
        }

        _enemyDestroyed = GetComponent<Animator>();
        if(_enemyDestroyed == null)
        {
            Debug.Log("_enemyDestroyed is NULL");
        }

        _explosionSound = GetComponent<AudioSource>();
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Time.time > _canFire && _canShoot == true)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();                
            }
        }
    }

    private void CalculateMovement()
    {
        
        switch (_movementType)
        {
            case 0:// Wave left to right
                transform.Translate(new Vector3(Mathf.Cos(Time.time * 3),-1,0) * _speed * Time.deltaTime);
                break;
            case 1:// Circle pattern
                transform.Translate(new Vector3(Mathf.Cos(Time.time / .5f), Mathf.Sin(Time.time / .5f), 0) * _speed * Time.deltaTime);
                break;
            default:
                transform.Translate(Vector3.down * _speed * Time.deltaTime);
                break;
        }

        if (transform.position.y < -4)
        {
            transform.position = new Vector3(Random.Range(-9f, 9f), 7f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" || other.tag == "Laser") 
        {
            _spawnManager.EnemyDeath();
            _canShoot = false;
            if (other.tag == "Player")
            {
                if (_player != null)
                {
                    _player.Damage();
                }
                _enemyDestroyed.SetTrigger("OnEnemyDeath");
                _speed = 0;
                _explosionSound.Play();
                Destroy(GetComponent<Collider2D>());
                Destroy(gameObject, 2.5f);
            }
            else if (other.tag == "Laser")
            {
                Destroy(other.gameObject);

                if (_player != null)
                {
                    _player.AddScore(10);
                }

                _enemyDestroyed.SetTrigger("OnEnemyDeath");
                _speed = 0;
                _explosionSound.Play();
                Destroy(GetComponent<Collider2D>());
                Vector3 enemyPosition = gameObject.transform.position;
                _spawnManager.EnemyDrop(enemyPosition);
                Destroy(this.gameObject, 2.5f);
            }
        }
    }
}
