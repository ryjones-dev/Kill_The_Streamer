using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFGBullet : MonoBehaviour
{

    private float timeAlive = 3.0f; // Amount of time until the projectile is destroyed (range)
    private float speed = 15f;
    public Rigidbody myBody;
    public GameObject m_bfgPrefab;

    // Use this for initialization
    void Start()
    {
        myBody = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //
    }

    //should be destroyed if hits wall
    void OnCollisionEnter(Collision collision)
    {

        if (collision.collider.gameObject.CompareTag("Enemy") || collision.collider.gameObject.CompareTag("Terrain"))
        {
            GameObject explosion = (GameObject)Instantiate(m_bfgPrefab, new Vector3(myBody.position.x, 0, myBody.position.z), Quaternion.identity);
            CameraShake.AddFadeShake(7.0f);
            Destroy(gameObject);
        }
    }
}
