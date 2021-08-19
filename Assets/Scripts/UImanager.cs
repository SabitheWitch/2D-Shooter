﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UImanager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;

    [SerializeField]
    private Text _ammoText;

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

    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _ammoText.text = "Ammo: " + 15;
        _gameOver.gameObject.SetActive(false);
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _slider.value = 5f;
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
            _ammoText.text = "Ammo: " + ammoCount.ToString();
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
            _ammoText.text = "Ammo: " + 0;
            yield return new WaitForSeconds(.5f);
            _ammoText.text = "Ammo: ";
            yield return new WaitForSeconds(.5f);
        }
    }

    public void UpdateLives(int currentLife)
    {
        Debug.Log(currentLife);
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
}
