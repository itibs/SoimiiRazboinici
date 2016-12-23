using UnityEngine;
using System.Collections;

public class Done_DestroyByContact : MonoBehaviour
{
    public GameObject explosion;
    public int scoreValue;
    private Done_GameController gameController;

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
        if ((other.tag == "Boundary" || other.tag == "Enemy") && other.tag != "Player")
        {
            return;
        }

        if (other.tag == "BottomBoundary")
        {
            Debug.Log("here");
            Destroy(gameObject);
            //Instantiate(explosion, transform.position, transform.rotation);
            return;
        }

        if (other.tag == "Bolt")
        {
            Done_Mover bolt = (Done_Mover)other.GetComponent<Done_Mover>();
            if (gameObject == bolt.target)
            {
                Destroy(other.gameObject);
                if (GetComponentInChildren<TypeScript>().isDone())
                {
                    Destroy(gameObject);
                    Instantiate(explosion, transform.position, transform.rotation);
                }
                return;
            } else
            {
                return;
            }
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