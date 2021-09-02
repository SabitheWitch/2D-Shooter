using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UImanager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;

    [SerializeField]
    private Text _ammoText;
    private int _maxAmmo = 15;

    [SerializeField]
    private Image _lifeDisplay;
    
    [SerializeField]
    private Sprite[] _liveSprites;

    [SerializeField]
    private Text _gameOver;

    [SerializeField]
    private Text _restartText;

    [SerializeField]
    private Slider _slider;    

    private GameManager _gameManager;

    private bool _hasAmmo;
   
    [SerializeField]
    private Text _waveDisplay;
    private bool _isWaveDisplayActive;
    private int _waveNumber;

    private SpawnManager _spawnManager;

    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _ammoText.text = "Ammo: " + 15 + "/" + _maxAmmo;
        _gameOver.gameObject.SetActive(false);
        _waveDisplay.gameObject.SetActive(false);
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _slider.value = 100f;

        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    public void UpdateAmmo(int ammoCount)
    {
        if(ammoCount > 0)
        {
            _hasAmmo = true;
            _ammoText.text = "Ammo: " + ammoCount + "/" + _maxAmmo;
        } else if (ammoCount == 0)
        {
            _hasAmmo = false;
            StartCoroutine(NoAmmoRoutine());
        }       
    }   

    IEnumerator NoAmmoRoutine()
    {
        while (_hasAmmo == false)
        {
            _ammoText.text = "Ammo: " + 0 + "/" + _maxAmmo;
            yield return new WaitForSeconds(.5f);
            _ammoText.text = "Ammo: ";
            yield return new WaitForSeconds(.5f);
        }
    }

    public void UpdateLives(int currentLife)
    {        
        _lifeDisplay.sprite = _liveSprites[currentLife];

        if(currentLife == 0)
        {
            GameOverSequence();
        }
    }
    
    public void UpdateSlider(float fuel)
    {
        _slider.value = fuel;
    }    

    void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOver.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlicker());
    }

    IEnumerator GameOverFlicker()
    {
        while (true)
        {
            _gameOver.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOver.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void DisplayWaveNumber(int waveNumber)
    {
        Debug.Log("wave: " + waveNumber);
        _waveDisplay.text = "Wave " + waveNumber;
        _waveDisplay.gameObject.SetActive(true);
        _isWaveDisplayActive = true;
        _spawnManager.StartSpawning(waveNumber);
        StartCoroutine(WaveDisplayRoutine());        
    }

    IEnumerator WaveDisplayRoutine()
    {        
        while (_isWaveDisplayActive == true)
        {
            Debug.Log("WaveDisplayisActive");
            yield return new WaitForSecondsRealtime(2.5f);            
            _waveDisplay.gameObject.SetActive(false);
            _isWaveDisplayActive = false;
        }        
    }
}
