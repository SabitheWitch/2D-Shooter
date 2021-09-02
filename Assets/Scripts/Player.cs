using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8.0f;
    private bool _canThrust = true;
    [SerializeField]
    private float _chargingTime = 100f;
    [SerializeField]
    private GameObject _thruster;
    

    [SerializeField]
    private GameObject _laserPreFab;
    [SerializeField]
    private GameObject _tripleShotPreFab;
    [SerializeField]
    private GameObject _multiShotPreFab;
    [SerializeField]
    private float _fireRate = 0.2f;
    private float _canFire = -1f;    
    private int _ammoCount = 15;
    private bool _isAmmoSpawning;


    private int _life = 3;
    private SpawnManager _spawnManager;
   
    bool _isTripleShotActive = false;
    bool _isMultiShotActive = false;
    bool _isSpeedBoostActive = false;
    bool _isShieldActive = false;
    bool _isNegSpeedActive = false;
    
    
    private int _shieldLife;
    SpriteRenderer _shieldColor;
    [SerializeField]
    private GameObject _shieldVisual;    
    private int _score;

    private Animator _cameraShake;    
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

        _shieldColor = _shieldVisual.GetComponent<SpriteRenderer>();

        _cameraShake = GameObject.Find("Main Camera").GetComponent<Animator>();

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
        if (_chargingTime > 100f)
        {
            _chargingTime = 100f;
        }
        if (Input.GetKey(KeyCode.Space) && Time.time > _canFire)
        {
            if (_ammoCount > 0)
            {
                _ammoCount--;
                FireLaser();
                _uiManager.UpdateAmmo(_ammoCount);
            }
            if (_isAmmoSpawning == false) 
            {                 
                if (_ammoCount == 0)
                {                    
                    _uiManager.UpdateAmmo(_ammoCount);
                    _spawnManager.AmmoDrop();
                    _isAmmoSpawning = true;
                }
            }
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
            _thruster.transform.localScale= new Vector3( 1f, 1f, 1f);
            _thruster.transform.position = new Vector3(transform.position.x, transform.position.y - 1.75f, transform.position.z);

            if(_chargingTime > 0f)
            {                
                _chargingTime -= 20f * Time.deltaTime;
                _uiManager.UpdateSlider(_chargingTime);
            }
            else
            {
                _chargingTime = 0f;
                _canThrust = false;
                _thruster.transform.localScale = new Vector3(.5f, .5f, .5f);
                _thruster.transform.position = new Vector3(transform.position.x, transform.position.y - 1.25f, transform.position.z);
                StartCoroutine(ThrustOverHeatRoutine());
            }
            
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _speed = 8f;
            _thruster.transform.localScale = new Vector3(.5f, .5f, .5f);
            _thruster.transform.position = new Vector3(transform.position.x, transform.position.y -1.25f, transform.position.z);

            if (_chargingTime < 100f && _chargingTime > 0f)
            {                
                StartCoroutine(ThrustChargeRoutine());
            }
            
        }
    }
    IEnumerator ThrustOverHeatRoutine()
    {
        while (_canThrust == false)
        {
            yield return new WaitForSeconds(1f);
            _chargingTime += 20f;
            _uiManager.UpdateSlider(_chargingTime);
            if (_chargingTime == 100f) 
            { 
                _canThrust = true; 
            }
        }
    }
    IEnumerator ThrustChargeRoutine()
    {
        while (_chargingTime < 100f && Input.GetKey(KeyCode.LeftShift) == false)
        {
            _chargingTime += 1f;
            _uiManager.UpdateSlider(_chargingTime);
            yield return new WaitForSeconds(.1f);
        }
    }


    void FireLaser()
    {
        
            _canFire = Time.time + _fireRate;
        if (_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPreFab, transform.position, Quaternion.identity);
        }else if( _isMultiShotActive == true)
        {
            Instantiate(_multiShotPreFab, transform.position, Quaternion.identity);
        }
        else
        { 
            Instantiate(_laserPreFab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity); 
        }
        _audioSource.Play();
        
    }

    public void Damage()
    {        
        _cameraShake.SetTrigger("isDamaged");
        if (_isShieldActive == true)
        {
            _shieldLife--;
            if(_shieldLife == 0) 
            {
            _isShieldActive = false;
            _shieldVisual.SetActive(false);
            return;
            } else if (_shieldLife == 1)
            {
                _shieldColor.color = Color.red;
                return;
            }
            else if (_shieldLife == 2)
            {
                _shieldColor.color = Color.green;
                return;
            }
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

    public void MultiShotActive()
    {
        _isMultiShotActive = true;
        StartCoroutine(MultiShotPowerDownRoutine());
    }

    IEnumerator MultiShotPowerDownRoutine()
    {
        while(_isMultiShotActive == true)
        {
            yield return new WaitForSeconds(5f);
            _isMultiShotActive = false;
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

    public void NegSpeedActive()
    {
        _isNegSpeedActive = true;
        _speed -= 5;
        StartCoroutine(NegSpeedPowerDownRoutine());
    }

    IEnumerator NegSpeedPowerDownRoutine()
    {
        while (_isNegSpeedActive == true)
        {
            yield return new WaitForSeconds(5f);
            _speed += 5;
            _isNegSpeedActive = false;
        }
    }
    public void ShieldActive()
    {
        if (_shieldLife < 3)
        {
            _shieldLife++;
            _isShieldActive = true;
            _shieldVisual.SetActive(true);           
        }
        if (_shieldLife == 1)
        {
            _shieldColor.color = Color.red;
        }
        else if (_shieldLife == 2)
        {
            _shieldColor.color = Color.green;
        }
        else if (_shieldLife == 3)
        {
            _shieldColor.color = Color.cyan;
        }
    }   
    public void AddAmmo()
    {
        if(_ammoCount <= 15)
        {
            _ammoCount = 15;
            _isAmmoSpawning = false;
            _uiManager.UpdateAmmo(_ammoCount);
            _spawnManager.HasAmmo();
        }
    }
    public void AddLife()
    {
        _life++;
        if(_life == 2)
        {
            _leftEngine.SetActive(false);
        }
        if(_life >= 3)
        {
            _life = 3;
            _rightEngine.SetActive(false);
        }
        _uiManager.UpdateLives(_life);
    }
    
    public void AddScore()
    {
        _score += 10;
        _uiManager.UpdateScore(_score);
    }


}
