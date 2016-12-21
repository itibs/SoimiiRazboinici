using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Done_GameController : MonoBehaviour
{
    public GameObject[] hazards;
    public Vector3 spawnValues;
    public GameObject playerExplosion;
    public GameObject player;
    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;

    public GUIText scoreText;
    public GUIText livesText;
    public GUIText restartText;
    public GUIText gameOverText;

    private bool gameOver;
    private bool restart;
    private int score;
    private int lives;

    void Start ()
	{
        gameOver = false;
        restart = false;
        restartText.text = "";
        gameOverText.text = "";
        score = 0;
        lives = 3;
        UpdateScore();
        UpdateLives();
        StartCoroutine(SpawnWaves());
    }
	
	void Update ()
	{
		if (restart)
		{
			if (Input.GetKeyDown (KeyCode.R))
			{
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
			}
		}
	}
	
	IEnumerator SpawnWaves ()
	{
		yield return new WaitForSeconds (startWait);
		while (true)
		{
			for (int i = 0; i < hazardCount; i++)
			{
				GameObject hazard = hazards [Random.Range (0, hazards.Length)];
				Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
				Quaternion spawnRotation = Quaternion.identity;
				Instantiate (hazard, spawnPosition, spawnRotation);
				yield return new WaitForSeconds (spawnWait);
			}
			yield return new WaitForSeconds (waveWait);
			
			if (gameOver)
			{
				restartText.text = "Press 'R' for Restart";
				restart = true;
				break;
			}

		}
	}
	
	public void AddScore (int newScoreValue)
	{
		score += newScoreValue;
		UpdateScore ();
	}

    public void SubtractLives(int livesTaken)
    {
        lives -= livesTaken;

        UpdateLives();
    }

    void UpdateScore ()
	{
		scoreText.text = "Score: " + score + " \nPress space for The Killing Joke";
	}

    void UpdateLives()
    {
        if (gameOver)
        {
            return;
        }
        livesText.text = "Lives: " + lives;
        if (lives < 1)
        {
            Instantiate(playerExplosion, player.transform.position, player.transform.rotation);
            GameOver();
        }
    }

    public void GameOver ()
	{
		gameOverText.text = "Game Over!";
		gameOver = true;
	}
}