using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject[] enemyPrefabs;

    [SerializeField]
    private GameObject[] rarePowerUps;

    [SerializeField]
    private GameObject _enemyContainer;

    [SerializeField]
    private GameObject[] powerUps;    

    private bool _stopSpawning = false;
    private bool _stopPowerUpsSpawning = false;

    private bool _isAmmoEmpty;

    private int _waveNumber;
    [SerializeField]
    private int _enemiesDead;
    [SerializeField]
    private int _maxEnemies;
    [SerializeField]
    private int _enemiesLeft;

    int probability;
    int randomRarePower;
    int randomPower;
    int powerupNum;

    private UImanager _uiManager;
    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UImanager>();
    }

    public void StartSpawning(int waveNumber)
    {
        StopAllCoroutines();
        _stopSpawning = false;
        _stopPowerUpsSpawning = false;
        _waveNumber = waveNumber;
        _enemiesLeft = _waveNumber + 10;
        _maxEnemies = _waveNumber + 10;
        _enemiesDead = 0;
        powerupNum = 0;
        
        Debug.Log("Spawn Start! Wave: " + _waveNumber + "Enemies: " + _enemiesLeft);
        Debug.Log("Enemy check: " + _maxEnemies);

        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
        StartCoroutine(AmmoDropRoutine());
    }
    // Update is called once per frame
    void Update()
    {        
        
    }
    
    IEnumerator SpawnEnemyRoutine()
    {        
        while (_stopSpawning == false)
        {
            yield return new WaitForSecondsRealtime(5f);
            Vector3 posToSpawn = new Vector3(Random.Range(-8.5f, 8.5f), 7, 0);
            Vector3 bottomSpawn = new Vector3(Random.Range(-8.5f, 8.5f), -6, 0);
            
            if(_waveNumber < 5)
            {
                GameObject newEnemy = Instantiate(enemyPrefabs[0], posToSpawn, Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;
            } else if( _waveNumber  >= 5 && _waveNumber < 10)
            {
                int randomEnemy = Random.Range(0, 6);
                if(randomEnemy == 5)
                {
                    GameObject newEnemy = Instantiate(enemyPrefabs[1], posToSpawn, Quaternion.identity);
                    newEnemy.transform.parent = _enemyContainer.transform;
                }
                else
                {
                    GameObject newEnemy = Instantiate(enemyPrefabs[0], posToSpawn, Quaternion.identity);
                    newEnemy.transform.parent = _enemyContainer.transform;
                }                               
            } else if(_waveNumber >= 10)
            {
                int randomEnemy = Random.Range(0, 6);
                if(randomEnemy > 1 && randomEnemy < 5)
                {
                    GameObject newEnemy = Instantiate(enemyPrefabs[1], posToSpawn, Quaternion.identity);
                    newEnemy.transform.parent = _enemyContainer.transform;
                }else if(randomEnemy == 5)
                {
                    GameObject newEnemy = Instantiate(enemyPrefabs[2], bottomSpawn, Quaternion.identity);
                    newEnemy.transform.parent = _enemyContainer.transform;
                }
                else
                {
                    GameObject newEnemy = Instantiate(enemyPrefabs[0], posToSpawn, Quaternion.identity);
                    newEnemy.transform.parent = _enemyContainer.transform;
                }
            }                                
            if(_enemiesLeft > 0)
            {
                _enemiesLeft--;
            }else if (_enemiesLeft == 0)
            {
                Debug.Log("Enemy spawn over");
                _stopSpawning = true;                
            }
        }                
    }

    public void EnemyDeath()
    {        
        _enemiesDead++;
        Debug.Log("Enemies Dead: " + _enemiesDead);
        if (_enemiesLeft == 0 && _enemiesDead == _maxEnemies)
        {
            _stopPowerUpsSpawning = true;
            Debug.Log("Powerup spawn over" + _stopPowerUpsSpawning);
            _waveNumber++;            
            _uiManager.DisplayWaveNumber(_waveNumber);
        }
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSecondsRealtime(6f);
        while(_stopPowerUpsSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9f, 9f), 7, 0);
            probability = Random.Range(0, 11);
            randomRarePower = Random.Range(0, 1);
            randomPower = Random.Range(1, 6);

            if (probability == 0)
            {                
                Instantiate(rarePowerUps[randomRarePower], posToSpawn, Quaternion.identity);
            }
            else
            {                
                Instantiate(powerUps[randomPower], posToSpawn, Quaternion.identity);
            }           
            yield return new WaitForSecondsRealtime(Random.Range(6f, 11f));
        }
    }

    public void EnemyDrop(Vector3 enemyPosition)
    {
        int randomNum = Random.Range(0, 10);
        if (randomNum == 0)
        {
            Instantiate(powerUps[0], enemyPosition, Quaternion.identity);
        }
    }

     public void AmmoDrop()
    {
        _isAmmoEmpty = true;
        StartCoroutine(AmmoDropRoutine());
    }
    IEnumerator AmmoDropRoutine()
    {
        while (_isAmmoEmpty == true)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9f, 9f), 7, 0);
            Instantiate(powerUps[3], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(10f);
        }
    }
    public void OnPlayerDeath()
    {
        _stopSpawning = true;                
    }   

    public void HasAmmo()
    {        
        _isAmmoEmpty = false;
    }
}
