using UnityEngine;
using System.Collections;

public class RotateAsteroid : MonoBehaviour {

    public float tumble;

    private Vector3 angVelo;

	// Use this for initialization
	void Start () {
        angVelo = Random.insideUnitSphere * tumble;
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.Rotate(angVelo);
	}
}
