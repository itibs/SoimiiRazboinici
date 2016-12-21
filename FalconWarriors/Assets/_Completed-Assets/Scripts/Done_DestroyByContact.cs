using UnityEngine;
using System.Collections;

public class Done_DestroyByContact : MonoBehaviour
{
    public GameObject explosion;
    public int scoreValue;
    private Done_GameController gameController;
    public int lives = 3;

    void Start()
    {
        GameObject gameControllerObject = GameObject.FindGameObjectWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<Done_GameController>();
        }
        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(lives);
        if ((other.tag == "Boundary" || other.tag == "Enemy") && other.tag != "Player")
        {
            return;
        }

        if (explosion != null)
        {
            Instantiate(explosion, transform.position, transform.rotation);
        }

        if (other.tag == "Player")
        {
            gameController.SubtractLives(1);
            Destroy(gameObject);
        }

        if ((other.tag != "Player"))
        {
            gameController.AddScore(scoreValue);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        //else
        //{
        //    if (lives == 0)
        //    {
        //        gameController.AddScore(scoreValue);
        //        Destroy(other.gameObject);
        //        Destroy(gameObject);
        //    }
        //    else
        //    {
        //        gameController.SubtractLives(1);
        //    }
        //}

    }
}