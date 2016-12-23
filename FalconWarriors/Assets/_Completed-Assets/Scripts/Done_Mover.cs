using UnityEngine;
using System.Collections;

public class Done_Mover : MonoBehaviour
{
	public float speed;
    public GameObject target = null;

    void Start ()
	{
        if (target != null)
        {
            Vector3 dir = target.transform.position - transform.position;
            dir.Normalize();
            GetComponent<Rigidbody>().velocity = dir * speed;
            transform.rotation = Quaternion.LookRotation(dir, new Vector3(0, 1));
        } else
        {
            GetComponent<Rigidbody>().velocity = (transform.forward) * speed;
        }
    }

    void Update()
    {
        if (target != null)
        {
            Vector3 dir = target.transform.position - transform.position;
            dir.Normalize();
            GetComponent<Rigidbody>().velocity = dir * speed;
            transform.rotation = Quaternion.LookRotation(dir, new Vector3(0, 1));
        }
        else
        {
            GetComponent<Rigidbody>().velocity = (transform.forward) * speed;
        }
    }
}
