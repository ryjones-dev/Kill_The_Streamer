using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperBullet : MonoBehaviour
{

    private float timeAlive = 1.5f; // Amount of time until the projectile is destroyed (range)
    private float speed = 15f;

    // Use this for initialization
    void Start()
    {
        Destroy(gameObject, timeAlive);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //should be destroyed if hits wall
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("BooAi"))
        {
            EnemyManager.DestroyEnemy(EnemyType.BooEnemy, collision.collider.GetComponent<EnemyData>().m_Index);

        }
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            //Debug.Log("Collided with obstacle");
            Destroy(gameObject);
        }
    }
}
