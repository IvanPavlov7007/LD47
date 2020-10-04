using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public GameObject pauseMenu;
    public GameObject RopeDrawerPrefab;
    public GameObject nodePrefab;
    

    public TextMeshProUGUI scoreCounter,livesCounter;

    public int currentScore = 0, scoreMultiplier;

    public int MaxLives, currentLives;

    public List<Enemy> enemies;
    public int maxEnemiesCount;

    [Header("Enemies")]
    public GameObject EnemyPrefab;
    GameObject enemiesFolder;
    public float areaHalfSide, areaBroadness;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
        currentLives = MaxLives;
        enemies = new List<Enemy>();
        enemiesFolder = GameObject.Find("Enemies");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || pauseMenu.activeSelf)
        {
            Pause();
        }
        scoreCounter.SetText(currentScore.ToString());
        livesCounter.SetText(currentLives.ToString());

        if(enemies.Count< maxEnemiesCount)
        {
            CreateNewEnemy();
        }
    }

    public void CreateNewEnemy()
    {
        float x = Random.Range(-1f,1f) * areaBroadness;
        if (x > 0)
            x += areaHalfSide;
        else
            x -= areaHalfSide;

        float y = Random.Range(-1f, 1f) * areaBroadness;
        if (y > 0)
            y += areaHalfSide;
        else
            y -= areaHalfSide;

        Instantiate(EnemyPrefab, new Vector3(x, 2f, y), Quaternion.identity, enemiesFolder.transform);

    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        gameObject.SetActive(false);
        Time.timeScale = 0f;
    }

    public static void LoseLife()
    {
        //if (--instance.currentLives == 0)
        //    instance.Pause();

    }

    public static void AddPoints(int points)
    {
        instance.currentScore += points;
    }

}
