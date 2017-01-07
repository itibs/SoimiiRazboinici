using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;

public class Done_GameController : MonoBehaviour
{
    public GameObject[] hazards;
    public Vector3 spawnValues;
    public GameObject playerExplosion;
    public GameObject player;

    public ArrayList crtHazards;
    public GameObject activeHazard;

    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;
    public int waveNo;
    public int maxWordLength;

    public GUIText scoreText;
    public GUIText livesText;
    public GUIText restartText;
    public GUIText gameOverText;

    private bool gameOver;
    private bool restart;
    private int score;
    private int lives;

    public Dictionary<int, List<string>> words;

    void Start ()
	{
        initializeWords();
        waveNo = 0;
        gameOver = false;
        restart = false;
        restartText.text = "";
        gameOverText.text = "";
        activeHazard = null;
        crtHazards = new ArrayList();
        score = 0;
        lives = 3;
        UpdateScore();
        UpdateLives();
        StartCoroutine(SpawnWaves());
    }
	
	void Update ()
	{
        while (crtHazards.Count > 0)
        {
            if (crtHazards[0].Equals(null))
            {
                crtHazards.RemoveAt(0);
            } else
            {
                //activeHazard = (GameObject)crtHazards[0];
                //TypeScript typeScript = activeHazard.GetComponentInChildren<TypeScript>();
                //typeScript.setActive(true);
                break;
            }
        }
        if (crtHazards.Count == 0)
        {
            activeHazard = null;
        }

        if (activeHazard == null || activeHazard.Equals(null)
            || activeHazard.GetComponentInChildren<TypeScript>().isDone()) // if no active hazard
        {
            for (char c = 'a'; c <= 'z'; c++) // see if a key is pressed
            {
                if (Input.GetKeyDown(c.ToString()))
                {
                    // and find if there is an asteroid which starts with that letter
                    for (int i = 0; i < crtHazards.Count; ++i)
                    {
                        GameObject hazard = (GameObject)crtHazards[i];
                        if (hazard.Equals(null))
                        {
                            continue; // skip destroyed asteroids
                        }
                        TypeScript typeScript = hazard.GetComponentInChildren<TypeScript>();
                        if (typeScript.isDone())
                        {
                            continue;
                        }
                        if (typeScript.orig_text.Length > 0
                            && char.ToLowerInvariant(typeScript.orig_text[0]) == c)
                        {
                            activeHazard = hazard;
                            typeScript.setActive(true);
                            Vector3 dir = activeHazard.transform.position - player.transform.position;
                            player.transform.rotation = Quaternion.LookRotation(dir, new Vector3(0, 1));
                            GameObject shot = player.GetComponent<Done_PlayerController>().shot;
                            Transform shotSpawn = player.GetComponent<Done_PlayerController>().shotSpawn;
                            shot.GetComponent<Done_Mover>().target = activeHazard;
                            Instantiate(shot, shotSpawn.position, Quaternion.Euler(shotSpawn.rotation.eulerAngles));
                            GetComponent<AudioSource>().Play();
                            break;
                        }
                    }
                    if (activeHazard != null && !activeHazard.Equals(null)) // found active hazard, exit loop
                    {
                        break;
                    }
                }
            }
        }
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
            waveNo += 1;
            for (int i = 0; i < hazardCount; i++)
			{
                // create asteroid
				GameObject hazard = hazards [Random.Range (0, hazards.Length)];
				Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
				Quaternion spawnRotation = Quaternion.identity;
				hazard = (GameObject) Instantiate (hazard, spawnPosition, spawnRotation);

                // set text
                TypeScript typeScript = hazard.GetComponentInChildren<TypeScript>();
                int wordLength = (waveNo - 1) % maxWordLength + 2;
                int cnt = words[wordLength].Count;
                typeScript.orig_text = words[wordLength][i % cnt];


                if (waveNo > maxWordLength)
                {
					//iterative application of animations on text
                    typeScript.showMirror();
					typeScript.ZoomInOut ();
                }

                // set score
                hazard.GetComponent<Done_DestroyByContact>().scoreValue = 5 + wordLength * wordLength;

                // add to list
                crtHazards.Add(hazard);
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

    private void initializeWords()
    {
        words = new Dictionary<int, List<string>>();
        string line;
        for (int i = 1; i <= 15; ++i)
        {
            words.Add(i, new List<string>());
            StreamReader sr = new StreamReader("Assets/Files/" + i + ".txt");
            using (sr)
            {
                do
                {
                    line = sr.ReadLine();
                    if (line != null)
                    {
                        words[i].Add(line);
                    }
                } while (line != null);
            }

            sr.Close();
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
		scoreText.text = "Score: " + score;
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
        Destroy(player);
		gameOverText.text = "Game Over!";
		gameOver = true;
	}
}