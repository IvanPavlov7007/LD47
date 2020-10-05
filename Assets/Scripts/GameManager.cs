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

    

    [Header("Mushrooms")]
    public GameObject MushroomPrefab;
    public List<Enemy> mushrooms;
    public int maxMushroomsCount;
    GameObject mushroomsFolder;
    public float areaHalfSide, areaBroadness, minDistToDog, waitForSpawnNextMushMin, waitForSpawnNextMushMax;

    RopeGod dog;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
        currentLives = MaxLives;
        mushrooms = new List<Enemy>();
        mushroomsFolder = GameObject.Find("Enemies");
    }

    [SerializeField]
    bool crMushrooms = false;

    private void Start()
    {
        dog = RopeGod.instance;
    }

    public void StartSpawningMushrooms()
    {
        StartCoroutine(SpawnMushrooms());
    }

    IEnumerator SpawnMushrooms()
    {
        while(true)
        {
            CreateMushroom();
            yield return new WaitForSeconds(Random.Range(waitForSpawnNextMushMin,waitForSpawnNextMushMax));
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || pauseMenu.activeSelf)
        {
            Pause();
        }

        if(crMushrooms)
        {
            crMushrooms = false;
            StartSpawningMushrooms();
        }
        //scoreCounter.SetText(currentScore.ToString());
        //livesCounter.SetText(currentLives.ToString());

        //if(enemies.Count< maxEnemiesCount)
        //{
        //    CreateMushroom();
        //}
    }

    public void CreateMushroom()
    {
        //float x = Random.Range(-1f,1f) * areaBroadness;
        //if (x > 0)
        //    x += areaHalfSide;
        //else
        //    x -= areaHalfSide;

        //float y = Random.Range(-1f, 1f) * areaBroadness;
        //if (y > 0)
        //    y += areaHalfSide;
        //else
        //    y -= areaHalfSide;

        Vector3 point = new Vector3(Random.Range(-1f, 1f) * areaHalfSide, 2f, Random.Range(-1f, 1f) * areaHalfSide);
        Vector3 dist = transform.position - dog.position;
        if (dist.magnitude < minDistToDog)
            point += dist.normalized * minDistToDog;

        Instantiate(MushroomPrefab, point, Quaternion.identity, mushroomsFolder.transform);

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
