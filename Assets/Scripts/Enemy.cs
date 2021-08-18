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

    void Start()
    {
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
       
        if(Time.time > _canFire)
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
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -4)
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
            _enemyDestroyed.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _explosionSound.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(gameObject, 2.5f);
        }else if (other.tag == "Laser")
        {
            Destroy(other.gameObject);

            if (_player != null)
            {
                _player.AddScore();
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
