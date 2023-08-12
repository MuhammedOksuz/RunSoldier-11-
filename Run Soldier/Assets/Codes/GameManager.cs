using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

//Attiributes
//[DisallowMultipleComponent]                           //Objeye baðlý ayný scriptten sadece bir tane olmasýný saðlar.
//[RequireComponent(typeof(BoxCollider))]               //Script ile type'ýný girdiðimiz component'in eklenmesini saðlar.
//[UnityEditor.MenuItem("Tool/GameManager/Restart()")]  //Metot'un üzerine yazýldýðýnda Tools menüsünde metodu gösterir ve ordan istediðimiz gibi çaðýrabiliriz. 
//[ContextMenu("Create")]                               //Metot'un üzerine yazýldýðýnda Script menüsünde metodu çalýþtýrmak için bir eklenti ekler ve ordan istediðimiz gibi çaðýrabiliriz. 

public class GameManager : MonoBehaviour
{
    //Script
    PlayerController playerController;
    //Enemy Spawn
    [SerializeField] GameObject[] enemys;
    [SerializeField] float randomX = 2.5f;
    [SerializeField] float enemySpawnTime;
    float enemyTimeCounter;
    [Tooltip("Sahneden ne kadar süre sonra yok olacaðýný belirler.")]
    [SerializeField] int destroyTime = 20;
    GameObject enemyParent;
    int enemyCounter = 0;
    //Bullet_UI
    List<GameObject> list = new();
    [SerializeField] GameObject bullet_UI;
    [SerializeField] Transform bulletTransform;
    [SerializeField] float y_Distance;
    public int bullet_UICounter = 3;
    [SerializeField] Transform bulletParent;
    public Image img;
    public TMP_Text UICounter;
    public float validBulletCount =100;
    public float percent;
    public bool fuel = false;
    //Gate
    [SerializeField] GameObject gate;
    [SerializeField] Transform gateParent;
    [SerializeField] float gateSpawnTime;
    float gateTimeCounter;
    GameObject gameObje = null;
    //Score
    [SerializeField] GameObject scorePanel;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text highScoreText;
    public int score = 0;
    public int highScore;

    int gateCounter = 0;
    private void Awake()
    {
        enemyTimeCounter = enemySpawnTime;
        gateTimeCounter = gateSpawnTime;
        enemyParent = new GameObject(transform.name = "Enemy");
        playerController = Object.FindObjectOfType<PlayerController>();
        for (int i = 0; i < 7; i++)
        {
            Vector3 pos = new Vector3(bulletTransform.position.x, y_Distance * i + bulletTransform.position.y, bulletTransform.position.z);
            GameObject go = Instantiate(bullet_UI, pos, bullet_UI.transform.rotation);
            go.transform.parent = bulletTransform.transform;
            go.gameObject.active = false;
            list.Add(go);
        }
        //PlayerPrefs.DeleteAll();
    }

    private void Update()
    {
        BulletUpdate();
        if (playerController.start && !playerController.gameOver)
        {
            EnemySpawner();
            GateSpawner();
        }
        print(score.ToString());
    }
    void EnemySpawner()
    {
        enemyTimeCounter -= Time.deltaTime;
        if (enemyTimeCounter <= 0)
        {
            int chance = Random.Range(0, enemys.Length);
            enemyCounter++;
            var go = Instantiate(enemys[chance], enemyParent.transform, true);
            var position = go.transform.position + new Vector3(0, 0, 10 * enemyCounter);
            go.transform.position = position;
            go.transform.parent = enemyParent.transform;


            go.transform.position = go.transform.position + new Vector3(Random.Range(-randomX, randomX), 0, 0);
            Destroy(go, destroyTime);
            enemyTimeCounter = enemySpawnTime;
        }
    }
    public void BulletUI_Spawner()
    {
        if (bullet_UICounter <= 6)
        {
            fuel = false;

            bullet_UICounter++;
            BulletUpdate();
            
            percent = validBulletCount / 7;
            img.fillAmount += percent / 100;
            UICounter.text = bullet_UICounter + "";
        }
        else
        {
            fuel = true;

            percent = validBulletCount / 7;
            img.fillAmount += percent / 100;
            UICounter.text = "Fuel";
            print("Fuel Bullet");
        }
    }
    public void BulletUpdate()
    {
        for (int i = 0; i <= bullet_UICounter; i++)
        {
            GameObject go = list[i];
            go.gameObject.active = true;
            for (int j = bullet_UICounter; j < 7; j++)
            {
                GameObject obje = list[j];
                obje.gameObject.active = false;
            }
        }
    }
    public void GateSpawner()
    {
        gateTimeCounter -= Time.deltaTime;
        if (gateTimeCounter <= 0)
        {
            gateCounter++;
            if (!playerController.gunActive)
            {
                gameObje = Instantiate(gate, gateParent.transform);
                var position = gameObje.transform.position + new Vector3(Random.Range(-2.3f, 2.3f), 0, 80 * gateCounter);
                gameObje.transform.position = position;
                gameObje.transform.parent = gateParent.transform;
                Destroy(gameObje, destroyTime);
                gateTimeCounter = gateSpawnTime;
            }
        }
        if (playerController.gunActive)
        {
            Destroy(gameObje);
        }
    }
    public void GameOver()
    {
        print("Game Over");
        playerController.anim.SetTrigger("Idle");
        playerController.anim.SetBool("Start", false);
        playerController.gameOver = true;
        playerController.bulletOne.SetActive(false);
        playerController.bulletTwo.SetActive(false);
        Score();
    }
    public void Score()
    {
        scorePanel.SetActive(true);
        scoreText.text = "Score : " + score.ToString();
        if (score > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", score);
        }
        highScoreText.text = "High Score :" + PlayerPrefs.GetInt("HighScore").ToString();
    }
    public void PlayAgain()
    {
        SceneManager.LoadScene("1");
    }
    
}
