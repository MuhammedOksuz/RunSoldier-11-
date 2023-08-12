using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class PlayerController : MonoBehaviour
{
    //Scripts
    GameManager gameManager;
    //Rigidbody
    public Rigidbody rb;
    Touch touch;
    [SerializeField] int rbSpeed;
    [SerializeField] int forwardSpeed;
    //Clamp
    [SerializeField] float minX;
    [SerializeField] float maxX;
    //Start
    [SerializeField] GameObject camera;
    [SerializeField] LayerMask mask;
    public bool start = false;
    [SerializeField] GameObject panel;
    [SerializeField] GameObject[] startBullet;
    //Animation
    public Animator anim;
    //Fire
    [SerializeField] GameObject bullet, bulletOut;
    [SerializeField] float fireTime;
    float time = 0;
    float fireTimeCounter;
    public bool itCanFire = true;
    //Gate
    public GameObject bulletOne;
    public GameObject bulletTwo;
    public bool gunActive = false;
    //GameOver
    public bool gameOver = false;
    [SerializeField] TMP_Text text;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        gameManager = Object.FindObjectOfType<GameManager>();
        fireTimeCounter = fireTime;
        gameOver = false;
    }
    private void Update()
    {
        if (!gameOver)
        {
            if (start)
            {
                CameraController camera = Object.FindObjectOfType<CameraController>();
                camera.FollowCamera();
                Swerve();
                Fire();
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, forwardSpeed);
                Destroy(startBullet[0].gameObject);
                Destroy(startBullet[1].gameObject);
            }
            if (Input.GetMouseButtonDown(0))
            {
                start = true;
                camera.GetComponent<Camera>().cullingMask = mask;
                panel.GetComponent<RectTransform>().DOScale(0, 0.1f);
                anim.SetBool("Start", true);
            }

        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }
    void Swerve()
    {
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minX, maxX), transform.position.y, transform.position.z);

        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                rb.velocity = new Vector3(touch.deltaPosition.x * rbSpeed * Time.deltaTime, rb.velocity.y, rb.velocity.z);
            }
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }
    void Fire()
    {
        if (gameManager.bullet_UICounter > 0)
        {
            itCanFire = true;
        }
        else
        {
            itCanFire = false;
        }
        if (itCanFire)
        {
            text.gameObject.SetActive(false);
            if (fireTimeCounter > 0)
            {
                fireTimeCounter -= Time.deltaTime;
                if (fireTimeCounter <= 0)
                {
                    fireTimeCounter = fireTime;

                    text.gameObject.SetActive(false);

                    gameManager.bullet_UICounter--;
                    if (gameManager.bullet_UICounter == 0)
                    {
                        time = 5;
                        gameManager.BulletUpdate();
                    }
                    float percent = 100f / 7f;
                    gameManager.img.fillAmount -= percent / 100;
                    gameManager.UICounter.text = gameManager.bullet_UICounter + "";
                    StartCoroutine(Spawn());
                    IEnumerator Spawn()
                    {
                        Instantiate(bullet, bulletOut.transform.position, bullet.transform.rotation);
                        yield return new WaitForSeconds(0.2f);
                        if (gunActive)
                        {
                            Instantiate(bullet, bulletOut.transform.position, bullet.transform.rotation);
                        }
                    }
                }
            }
        }
        else
        {
            text.text = ((int)time) + "";
            text.gameObject.SetActive(true);
            time -= Time.deltaTime; ;
            if (time <= 0)
            {
                text.text = "Game Over";
                FireController();
            }
        }
    }
    void FireController()
    {
        gameManager.img.fillAmount = 0;
        gameManager.UICounter.text = "0";
        gameManager.GameOver();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemy") || other.CompareTag("enemy1"))
        {
            text.gameObject.SetActive(true);
            text.text = "Game Over";
            gameManager.GameOver();
        }
        if (other.CompareTag("Bullet"))
        {
            if (!gameManager.fuel)
            {
                gameManager.score += 10;
                Destroy(other.gameObject);
                gameManager.BulletUI_Spawner();
            }
        }
        if (other.CompareTag("Gate"))
        {
            bulletOne.SetActive(false);
            bulletTwo.SetActive(true);
            gunActive = true;
        }  
        
    }
}

   
    

