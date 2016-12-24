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

        GameObject activeHazard = controller.activeHazard;
        if (activeHazard != null)
        {
            TypeScript typeScript = activeHazard.GetComponentInChildren<TypeScript>();
            if (!typeScript.isDone() && Input.GetKeyDown(typeScript.textMesh.text[0].ToString()))
            {
                debugText.text = controller.activeHazard.transform.position.ToString() + " " + controller.crtHazards.Count.ToString();
                shot.GetComponent<Done_Mover>().target = activeHazard;
                Instantiate(shot, shotSpawn.position, Quaternion.Euler(line));
                GetComponent<AudioSource>().Play();
            }
        }

		////The Killing Joke
		//if (Input.GetButton("Killing_Joke"))
		//{
		//	Vector3 rot = shotSpawn.rotation.eulerAngles;
		//	rot = new Vector3(rot.x,(rot.y - Random.Range(-1,100))%100,rot.z);
		//	shotSpawn.rotation=Quaternion.Euler(rot);
		//	Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
		//	GetComponent<AudioSource>().Play ();
		//}

	
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
