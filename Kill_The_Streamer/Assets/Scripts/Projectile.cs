using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    private float timeAlive = 1.5f; // Amount of time until the projectile is destroyed (range)
    private float speed = 15f;

	// Use this for initialization
	void Start () {
        Destroy(gameObject, timeAlive);
    }
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector3.forward * Time.deltaTime * speed); //moves projectile in target direction
	}

    //should be destroyed if hits wall
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
<<<<<<< HEAD
            //Debug.Log("Collided with obstacle");
            Destroy(gameObject);
        }
        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Collided with player");
=======
            //Debug.Log("Collided with wall");
>>>>>>> f09986c652f2c37acb19e10571e816e69725d9d6
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Collided with player");
            Destroy(gameObject);
            //Damage code here
        }
    }
}
