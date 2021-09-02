using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private float _speed = 3f;

    [SerializeField]
    private GameObject _explosion;

    private SpawnManager _spawnManager;
    private UImanager _uiManager;
    void Start()
    {
        
        _uiManager = GameObject.Find("Canvas").GetComponent<UImanager>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {       
        transform.Rotate(Vector3.forward * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Laser")
        {
            Instantiate(_explosion, transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            int waveNumber = 1;
            _uiManager.DisplayWaveNumber(waveNumber);            
            Destroy(this.gameObject);            
        }
    }
}

