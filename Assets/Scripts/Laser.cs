using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    // speed variable
    [SerializeField]
    private float _speed = 8f;
    [SerializeField]
    private bool _isEnemyLaser = false;
    [SerializeField]
    private int _missileID;
    [SerializeField]
    private GameObject _explosion;

    private Vector3 _target;

    private Player _player;
    private Powerup _powerup;

    private bool _hasPassed = false;
    private bool _isTargetPowerup = false;
    private bool _isTargetPlayer = false;
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _hasPassed = false;
    }

    
    void Update()
    {       
       if(_isEnemyLaser == false && _missileID != 1 && _missileID != 2)
        {
            MoveUp();
        }
        else if (_isEnemyLaser == true && _missileID != 1 && _missileID != 2)
        {
            MoveDown();
        }
       
        if (_missileID == 1)
        {            
            //CalculatePlayerPosition();
            _target = _player.transform.position;          
            EnemyMissile();
        }else if(_missileID == 1 && _isTargetPowerup == true)
        {
            EnemyMissile();
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

    void EnemyMissile()
    {      
        if(_isTargetPowerup == false)
        {
            if (transform.position.y > (_player.transform.position.y) && _hasPassed == false)
            {
                float distance = (_speed - 4) * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, _target, distance);


                //look at target
                Vector2 direction = (transform.position - _target).normalized;
                var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                var offset = 90f;

                transform.rotation = Quaternion.Euler(Vector3.forward * (angle + offset));
            }
            else if (_hasPassed == true)
            {
                transform.Translate(Vector3.up * (_speed - 4) * Time.deltaTime);
            }

            if (transform.position.y <= (_player.transform.position.y + 1f) && _hasPassed == false)
            {
                _hasPassed = true;
                transform.Translate(Vector3.up * (_speed - 4) * Time.deltaTime);
            }
        }
        else
        {
            transform.Translate(Vector3.up * (_speed) * Time.deltaTime);
        }


    }

    public void SetMissileTargetPlayer()
    {
        _isTargetPlayer = true; 
    }

    public void SetMissileTargetPowerup()
    {
        _isTargetPowerup = true;
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

        if(collision.tag == "Player" && _missileID == 1)
        {
            Player _player = collision.GetComponent<Player>();
            if(_player != null)
            {
                _speed = 4f;
                Instantiate(_explosion, transform.position, Quaternion.identity);
                _player.Damage();
            }
            
            Destroy(this.gameObject);
        }

        if(collision.tag == "Powerup" && _missileID == 1)
        {
            _speed = 0f;
            Instantiate(_explosion, transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
    }
}
