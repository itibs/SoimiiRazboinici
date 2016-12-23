using UnityEngine;
using System.Collections;


[System.Serializable]
public class Done_Boundary 
{
	public float xMin, xMax, zMin, zMax;
}

public class Done_PlayerController : MonoBehaviour
{
	public float speed;
	public float tilt;
	public Done_Boundary boundary;

    public Done_GameController controller;

    public GUIText debugText;

    public GameObject shot;
	public Transform shotSpawn;
	public float fireRate;
	 
	private float nextFire;
	private string  check_for_char = "abcdefghikljmnopqrstuvwqyz";

	private Vector3 line;

	void Start()
	{
		line = shotSpawn.rotation.eulerAngles;
	}


	void Update ()
	{
		
        if (controller.activeHazard != null)
        {
            Vector3 dir = controller.activeHazard.transform.position - transform.position;
            transform.rotation = Quaternion.LookRotation(dir, new Vector3(0, 1));
        } else
        {
            transform.rotation = Quaternion.LookRotation(new Vector3(0, 0, 1), new Vector3(0, 1));
        }

		var i=0;
		for(i=0;i<check_for_char.Length;i++)
			if ((Input.GetKeyDown(check_for_char[i].ToString()) && Time.time > nextFire)) 
			{
                GameObject activeHazard = controller.activeHazard;
                if (activeHazard != null)
                {
                    debugText.text = controller.activeHazard.transform.position.ToString() + " " + controller.crtHazards.Count.ToString();
                    nextFire = Time.time + fireRate;
                    shot.GetComponent<Done_Mover>().target = activeHazard;
                    Instantiate(shot, shotSpawn.position, Quaternion.Euler(line));
                    GetComponent<AudioSource>().Play();
                }
			}


		//The Killing Joke
		if (Input.GetButton("Killing_Joke"))
		{
			nextFire= Time.time + (float) 0.01; 
			Vector3 rot = shotSpawn.rotation.eulerAngles;
			rot = new Vector3(rot.x,(rot.y - Random.Range(-1,100))%100,rot.z);
			shotSpawn.rotation=Quaternion.Euler(rot);
			Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
			GetComponent<AudioSource>().Play ();
		}

	
	}

	void FixedUpdate ()
	{
		/*
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
		*/
		//Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		Vector3 movement = new Vector3 (0.0f, 0.0f, 0.0f);
		GetComponent<Rigidbody>().velocity = movement * speed;
		
		GetComponent<Rigidbody>().position = new Vector3
		(
			Mathf.Clamp (GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax), 
			0.0f, 
			Mathf.Clamp (GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
		);
				
		GetComponent<Rigidbody>().rotation = Quaternion.Euler (0.0f, 0.0f, GetComponent<Rigidbody>().velocity.x * -tilt);

	}
}
