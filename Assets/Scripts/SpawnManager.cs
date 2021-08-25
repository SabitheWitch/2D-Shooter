using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject _enemyPrefab;

    [SerializeField]
    private GameObject _enemyContainer;

    [SerializeField]
    private GameObject[] powerUps;

    

    private bool _stopSpawning = false;

    private bool _isAmmoEmpty;
    void Start()
    {
        
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.5f);
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9f, 9f), 7, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5f);
        }
        
        
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(4f);
        while(_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9f, 9f), 7, 0);
            int probability = Random.Range(0, 15);
            if (probability == 0)
            {
                Instantiate(powerUps[5], posToSpawn, Quaternion.identity);
            }else
            {
                int randomPowerup = Random.Range(0, 3);
                Instantiate(powerUps[randomPowerup], posToSpawn, Quaternion.identity);
            }
            yield return new WaitForSeconds(Random.Range(3f, 8f));
        }
    }

    public void EnemyDrop(Vector3 enemyPosition)
    {
        int randomNum = Random.Range(0, 5);
        if (randomNum == 3 || randomNum == 4)
        {
            Instantiate(powerUps[randomNum], enemyPosition, Quaternion.identity);
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
        Debug.Log("Ammo collected");
        _isAmmoEmpty = false;
    }
}
