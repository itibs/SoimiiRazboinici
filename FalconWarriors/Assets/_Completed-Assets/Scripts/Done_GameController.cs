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
    public string username = "";
    public int highScore = 0;
    public string highScoreUser = "";
    public string highScoreKey = "";
    public string userHighScoreKey = "";
    public bool showText = true;

    int[] highScores = new int[5];
    public int j;

    public Dictionary<int, List<string>> words;

    void OnGUI()
    {
        if (restart)
        {
            if (showText)
            {
                GUI.Label(new Rect(Screen.width * .42f, Screen.height * .45f, Screen.width * .25f, 40), "Enter your username");
                username = GUI.TextField(new Rect(Screen.width * .42f, Screen.height * .55f, Screen.width * .17f, 40), username, 25);

                if (string.Equals("", username))
                {
                    GUI.enabled = false;
                }
                else
                {
                    GUI.enabled = true;
                }

                if (GUI.Button(new Rect(Screen.width * .42f, Screen.height * .72f, Screen.width * .17f, Screen.height * .1f), "Submit"))
                {
                    
                    bool changed = false;
                    for (j = 0; j < highScores.Length; j++)
                    {

                        //Get the highScore from 1 - 5
                        highScoreKey = "HighScore" + (j + 1).ToString();
                        highScore = PlayerPrefs.GetInt(highScoreKey, 0);
                        userHighScoreKey = "HighScoreUser" + (j + 1).ToString();

                        if (score > highScore)
                        {
                            int temp = highScore;
                            string temp2 = PlayerPrefs.GetString("HighScoreUser" + (j+1).ToString(), "");
                            PlayerPrefs.SetInt(highScoreKey, score);
                            if (changed)
                            {
                                PlayerPrefs.SetString(userHighScoreKey, temp2);
                            }
                            else
                            {
                                PlayerPrefs.SetString(userHighScoreKey, username);
                            }
                            
                            score = temp;
                            changed = true;
                        }
                    }
                    showText = false;
                }
            }
            else
            {
                for (j = 0; j < highScores.Length; j++)
                {
                    highScoreKey = "HighScore" + (j + 1).ToString();
                    userHighScoreKey = "HighScoreUser" + (j + 1).ToString();
                    highScore = PlayerPrefs.GetInt(highScoreKey, 0);
                    highScoreUser = PlayerPrefs.GetString(userHighScoreKey, "");
                    if (highScore > 0)
                    {
                        GUI.Label(new Rect(Screen.width * .42f, Screen.height * (.45f + (.1f * j)), Screen.width * .25f, 40), highScoreUser + " " + highScore.ToString());
                    }

                    Debug.Log(highScoreUser);
                    Debug.Log(highScore);
                }
            }

            
        }
    }

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

                Done_Mover mover = hazard.GetComponent<Done_Mover>();
                mover.speed = -1.75f - (1.5f * waveNo / 50.0f);

                // set text
                TypeScript typeScript = hazard.GetComponentInChildren<TypeScript>();
                
                int wordLength = (waveNo - 1) % maxWordLength + 2;
                int cnt = words[wordLength].Count;
                typeScript.orig_text = words[wordLength][i % cnt];


                if ((waveNo / maxWordLength) % 4 == 1)
                {
					typeScript.flag_zoom = true;
                    typeScript.GetComponent<RotateAsteroid>().enabled = true;
                }
                if ((waveNo / maxWordLength) % 4 == 2)
                {
                    typeScript.flag_color = true;
                    typeScript.flag_tran = true;
                }
                if ((waveNo / maxWordLength) % 4 == 3)
                {
                    typeScript.showMirror();
                    typeScript.flag_color = true;
                    typeScript.flag_tran = true;
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
        for (int i = 1; i <= 22; ++i)
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
                        if (!words[i].Contains(line))
                        {
                            words[i].Add(line);
                        }
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