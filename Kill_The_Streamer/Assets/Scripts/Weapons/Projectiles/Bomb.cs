using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{

    private float timeAlive = 3.0f; // Amount of time until the projectile is destroyed (range)
    public Rigidbody myBody;
    public GameObject m_bombPrefab;
    public const float life = 1.0f;
    public float lifeTime;

    // Use this for initialization
    void Start()
    {
        myBody = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        myBody.velocity *= 0.90f;
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            GameObject explosion = (GameObject)Instantiate(m_bombPrefab, new Vector3(myBody.position.x, 0, myBody.position.z), Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    //should be destroyed if hits wall
    void OnCollisionEnter(Collision other)
    {
        
    }
}
