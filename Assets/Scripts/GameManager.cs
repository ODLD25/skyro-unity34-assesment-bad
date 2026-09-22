using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Enemies")]
    [SerializeField]private GameObject enemyPrefab;

    [Header("Wave Settings")]
    [SerializeField]private float difficulty = 1f;
    [SerializeField]private int wave = 1;
    [SerializeField]private AnimationCurve spawnCooldown;
    private bool gameOver;
    private bool paused;

    [Header("Stats")]
    [SerializeField]private int score = 0;
    
    [Header("References")]
    [SerializeField]private GameObject player;
    [SerializeField]private GameObject pauseCanvas;
    [SerializeField]private HudStuff hud;

    public static GameManager instance;

#region Unity Methods
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Game Manager Instance already exists!!!");
        }

        StartCoroutine(SpawnEnemies());
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            Pause();
        }
    }
#endregion

    private IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(5);

        Vector3 pos;
        while (!gameOver)
        {
            pos = new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), 0);
            
            if (Vector3.Distance(pos, player.transform.position) < 1.5f)
            {
                pos.x += 3f;
            }

            GameObject spawnedEnemy = Instantiate(enemyPrefab, pos, Quaternion.identity);
            difficulty += .01f;
            yield return new WaitForSeconds(spawnCooldown.Evaluate(difficulty));

            yield return null;
        }
    }

    public void Pause()
    {
        paused = !paused;
        Time.timeScale = paused ? 0f : 1f;
        pauseCanvas.SetActive(!pauseCanvas.activeSelf);
    }

    public void PlayAgain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameScene");
    }

    public void AddScore(int amount)
    {
        score += amount;
        hud.UpdateScore(score);
    }

    public void EndGame()
    {
        Time.timeScale = 0f;
    }

    public GameObject GetPlayer()
    {
        return player;
    }
}
