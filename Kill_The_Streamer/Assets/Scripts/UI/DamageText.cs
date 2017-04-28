using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageText : MonoBehaviour {

	const int MIN_SIZE = 54;
	const int MAX_SIZE = 300;
	const int MAX_DAMAGE = 10000;
	float timer;
	const float TIME_ALIVE = 0.4f;

	void Awake() {
		this.GetComponent<Renderer> ().sortingOrder = 99;
		timer = TIME_ALIVE;
	}

	public void Initialize(int damage, bool isHeal = false){
		int fontSize = MIN_SIZE + (int)((damage / (float)MAX_DAMAGE) * (MAX_SIZE - MIN_SIZE));
		this.GetComponent<TextMesh> ().fontSize = fontSize;



		if (isHeal) {
			this.GetComponent<TextMesh> ().color = Color.green;
			this.GetComponent<TextMesh> ().text = "+" + damage;
		}
		else{
			this.GetComponent<TextMesh> ().text = "-" + damage;
		}

	}

	void Update(){
		timer -= Time.deltaTime;

		if (timer < 0) {
			GameObject.Destroy (this.gameObject);
		}

		this.transform.position += new Vector3 (0, 0, 5 * Time.deltaTime);
	}

}
