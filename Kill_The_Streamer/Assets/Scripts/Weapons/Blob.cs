using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blob : MonoBehaviour {
	public const float life = 1.0f;
	public float lifeTime;
	// Use this for initialization
	void Start () {
		lifeTime = life;
	}
	
	// Update is called once per frame
	void Update () {
		lifeTime -= Time.deltaTime;
		if(lifeTime <= 0){
			Destroy (this.gameObject);
		}
	}

	void OnCollisionEnter(Collision collision)
	{

		if (collision.collider.gameObject.CompareTag("BooAi")){     
			EnemyManager.DestroyEnemy(EnemyType.BooEnemy, collision.collider.GetComponent<AIBase>().Index);

		}
	}
}
