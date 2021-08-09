using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    [SerializeField]
    private Text _gameStart;
    void Start()
    {
        _gameStart.gameObject.SetActive(true);
        StartCoroutine(GameStartFlicker());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(1);
        }
    }

    IEnumerator GameStartFlicker()
    {
        while (true)
        {
            _gameStart.text = "Press Space to Start";
            yield return new WaitForSeconds(0.5f);
            _gameStart.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }
}
