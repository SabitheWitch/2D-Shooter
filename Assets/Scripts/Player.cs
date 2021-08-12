﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8.0f;
    private bool _canThrust = true;
    [SerializeField]
    private float _chargingTime = 5f;
    private float _chargeRate = 1f;
    

    [SerializeField]
    private GameObject _laserPreFab;
    [SerializeField]
    private GameObject _tripleShotPreFab;

    [SerializeField]
    private float _fireRate = 0.2f;
    private float _canFire = -1f;

    [SerializeField]
    private int _life = 3;
    private SpawnManager _spawnManager;
   
    bool _isTripleShotActive = false;  
    bool _isSpeedBoostActive = false;
    bool _isShieldActive = false;
    private int _shieldLife;

    [SerializeField]
    private GameObject _shieldVisual;

    private int _score;

    private UImanager _uiManager;

    [SerializeField]
    private GameObject _rightEngine;
    [SerializeField]
    private GameObject _leftEngine;

    [SerializeField]
    private AudioClip _laserShot;
    private AudioSource _audioSource;
   
    void Start()
    {      
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

        _uiManager = GameObject.Find("Canvas").GetComponent<UImanager>();

        _audioSource = GetComponent<AudioSource>();       

        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is NULL!");
        }

        if (_uiManager == null)
        {
            Debug.LogError("UI Manager is NULL!");
        }

        if (_audioSource == null)
        {
            Debug.LogError("Audio Source in player is NULL!");
        } else
        {
            _audioSource.clip = _laserShot;
        }

        _rightEngine.SetActive(false);
        _leftEngine.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        Thrust();
        if (Input.GetKey(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }

        
    }

    private void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        

       
        transform.Translate(direction * _speed * Time.deltaTime);
       

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x > 11.24f)
        {
            transform.position = new Vector3(-11.24f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.24f)
        {
            transform.position = new Vector3(11.24f, transform.position.y, 0);
        }
        
    }
    void Thrust()
    {
        if (Input.GetKey(KeyCode.LeftShift) && _canThrust == true)
        {
            _speed = 12f;

            if(_chargingTime == 5)
            { 
                StartCoroutine(ThrustOverHeatRoutine());
            }
            if(_chargingTime == 0f)
            {
                _canThrust = false;
            }
            
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _speed = 8f;

            if (_chargingTime < 5f)
            {
                StartCoroutine(ThrustChargeRoutine());
            }
            _canThrust = true;
        }
    }

    IEnumerator ThrustOverHeatRoutine()
    {
        while(Input.GetKey(KeyCode.LeftShift) && _chargingTime > 0)
        {
            _chargingTime--;
            yield return new WaitForSeconds(1f);

        }
    }
    IEnumerator ThrustChargeRoutine()
    {
        while (_chargingTime < 5f)
        {
            _chargingTime++;
            yield return new WaitForSeconds(1f);            
        }
    }
   
    void FireLaser()
    {
        
            _canFire = Time.time + _fireRate;
        if (_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPreFab, transform.position, Quaternion.identity);
        }
        else
        { 
            Instantiate(_laserPreFab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity); 
        }
        _audioSource.Play();
        
    }

    public void Damage()
    {
       if(_isShieldActive == true)
        {
            _isShieldActive = false;
            _shieldVisual.SetActive(false);
            return;
        }
        _life--;


        if (_life == 2)
        {
            _rightEngine.SetActive(true);
        } else if(_life == 1)
        {
            _leftEngine.SetActive(true);
        }        

        if (_life < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
        if (!(_life < 0))
        {
            _uiManager.UpdateLives(_life);
        }
    }

    public void TripleShotActive()
    {
         _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        while (_isTripleShotActive == true)
        {
            yield return new WaitForSeconds(5f);
            _isTripleShotActive = false;
        }
    }
    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        _speed += 10;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        while (_isSpeedBoostActive == true)
        {
            yield return new WaitForSeconds(5f);
            _isSpeedBoostActive = false;
            _speed -= 10;
        }
    }

    public void ShieldActive()
    {
        _isShieldActive = true;
        _shieldVisual.SetActive(true);
    }

    public void AddScore()
    {
        _score += 10;
        _uiManager.UpdateScore(_score);
    }
}
