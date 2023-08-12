using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    GameManager gameManager;
    Rigidbody rb;
    [SerializeField] float speed;
    [SerializeField] float time;
    float timeCounter;
    private void Start()
    {
        gameManager = Object.FindObjectOfType<GameManager>();
        rb = GetComponent<Rigidbody>();
        timeCounter = time;
    }
    private void Update()
    {
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, speed * Time.deltaTime);

        timeCounter -= Time.deltaTime;
        if (timeCounter <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemy"))
        {
            gameManager.score += 10;
            other.GetComponent<Enemy>().Die();
            Destroy(gameObject);
        } 
        if (other.CompareTag("enemy1"))
        {
            gameManager.score += 10;
            other.GetComponent<Enemy1>().Die();
            Destroy(gameObject);
        }
    }
}
