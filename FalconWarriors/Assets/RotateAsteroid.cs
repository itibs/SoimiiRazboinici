using UnityEngine;
using System.Collections;

public class RotateAsteroid : MonoBehaviour {

    public float tumble;
    public bool onlyHorizontal;

    private Vector3 angVelo;

	// Use this for initialization
	void Start () {
        angVelo = Random.insideUnitSphere * tumble;
        if (onlyHorizontal)
        {
            angVelo = new Vector3(0, tumble, 0);
        }
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.Rotate(angVelo);
	}
}
