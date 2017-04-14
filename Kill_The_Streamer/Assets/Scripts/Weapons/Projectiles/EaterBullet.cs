using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EaterBullet : MonoBehaviour {

    private float timeAlive = 1.5f; // Amount of time until the projectile is destroyed (range)
    private float speed = 15f;

    public GameObject emoteEaterWeapon;
    private EmoteEaterWeapon weaponScript;
    
    // Use this for initialization
    void Start()
    {
        Destroy(gameObject, timeAlive);
        weaponScript = emoteEaterWeapon.GetComponent<EmoteEaterWeapon>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed); //moves projectile in target direction
    }

    //should be destroyed if hits wall
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            //Debug.Log("Collided with obstacle");
            Destroy(gameObject);
        }
        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Collided with player");
            weaponScript.Weapon_Ammo = weaponScript.Weapon_Ammo + 1;
            Destroy(gameObject);
        }
    }
}
