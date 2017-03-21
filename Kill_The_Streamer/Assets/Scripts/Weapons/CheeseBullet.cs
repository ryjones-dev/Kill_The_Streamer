using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeseBullet : MonoBehaviour {

	private float timeAlive = 3.0f; // Amount of time until the projectile is destroyed (range)
	private float speed = 15f;
	public Vector3 stopPoint = Vector3.forward * 3.0f;
	public Rigidbody myBody;
	public GameObject m_blobPrefab;

	// Use this for initialization
	void Start () {
		myBody = this.GetComponent<Rigidbody> ();
	}

	// Update is called once per frame
	void FixedUpdate () {
		myBody.velocity *= 0.90f;
		if(myBody.velocity.sqrMagnitude <= 5.0f){
			GameObject blob = (GameObject)Instantiate(m_blobPrefab, new Vector3(myBody.position.x, 0, myBody.position.z), Quaternion.identity);
			Destroy (gameObject);
		}
	}

	//should be destroyed if hits wall
	void OnCollisionEnter(Collision other)
	{
		/*
		if (other.gameObject.CompareTag("Obstacle"))
		{
			GameObject blob = (GameObject)Instantiate(m_blobPrefab, new Vector3(myBody.position.x, 0, myBody.position.z), Quaternion.identity);
			//Debug.Log("Collided with obstacle");
			Destroy(gameObject);
		}*/
	}
}
