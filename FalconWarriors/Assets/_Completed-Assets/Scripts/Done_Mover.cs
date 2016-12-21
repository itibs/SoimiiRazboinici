using UnityEngine;
using System.Collections;

public class Done_Mover : MonoBehaviour
{
	public float speed;
    public GameObject target;

    void Start ()
	{
		GetComponent<Rigidbody>().velocity = transform.forward * speed;
	}
}
