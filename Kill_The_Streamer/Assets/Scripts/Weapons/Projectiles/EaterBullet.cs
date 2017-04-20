using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EaterBullet : MonoBehaviour {

    private float timeAlive = 1.5f; // Amount of time until the projectile is destroyed (range)
    private float speed = 15f;

    public GameObject emoteEaterWeapon;
    private WeaponEmoteEater weaponScript;
    
    // Use this for initialization
    void Start()
    {
        weaponScript = emoteEaterWeapon.GetComponent<WeaponEmoteEater>();
    }


	void OnTriggerEnter(Collider collision)
	{
		if (collision.gameObject.CompareTag("Enemy"))
		{
			AIBase ai = collision.GetComponent<AIBase>();
			ai.TakeDamage();
			//Ig the max ammo hasnt been hit and there is still enoguh to add more, increase
			if (weaponScript.Weapon_Ammo + 2 < weaponScript.MAX_AMMO) {
				weaponScript.Weapon_Ammo += 2;
			}
			Destroy(gameObject);
		}

		if (collision.gameObject.CompareTag("Shield"))
		{
			AiShieldSeek shieldAI = collision.GetComponentInParent<AiShieldSeek>();
			shieldAI.ShieldTakeDamage();
			Destroy(gameObject);

		}


		if (collision.gameObject.CompareTag("Terrain"))
		{
			Destroy(gameObject);
		}

	}
		
}
