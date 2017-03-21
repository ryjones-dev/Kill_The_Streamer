using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{

    void OnCollisionEnter(Collision collision)
    {

        if (collision.collider.gameObject.CompareTag("BooAi")){     
            EnemyManager.DestroyEnemy(EnemyType.BooEnemy, collision.collider.GetComponent<AIBase>().Index);

        }

        Destroy(this.gameObject);
    }
}
