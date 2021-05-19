using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 10.5f;

    [SerializeField]
    private GameObject _laserPreFab;

    void Start()
    {      
        transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        //if i hit space key
        //spawn gameObject

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(_laserPreFab);
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * _speed * Time.deltaTime);

        //if(transform.position.y >= 0)
        //{
        //    transform.position = new Vector3(transform.position.x, 0, 0);
        //} else if (transform.position.y <= -3.8f)
        //{
        //    transform.position = new Vector3(transform.position.x, -3.8f, 0);
        //}

        // Line below works the same as the the if statement above that is commented.
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x > 11.24f)
        {
            transform.position = new Vector3(-11.24f, transform.position.y, 0);
        } else if (transform.position.x < -11.24f)
        {
            transform.position = new Vector3(11.24f, transform.position.y, 0);
        }
    }
}
