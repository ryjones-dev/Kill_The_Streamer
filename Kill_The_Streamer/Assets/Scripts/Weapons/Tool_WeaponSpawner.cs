using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool_WeaponSpawner : MonoBehaviour {

    public static Tool_WeaponSpawner s_instance;
    public GameObject[] SpawnableWeaponPrefabs;
	private Quaternion m_rotate;

    void Start()
    {
        if (!s_instance)
        {
            s_instance = this;
        }
		m_rotate = Quaternion.Euler (90, 0, 0);

    }

    public void SpawnWeapon(Vector3 position)
    {
		Instantiate(SpawnableWeaponPrefabs[Random.Range(0, SpawnableWeaponPrefabs.Length)], position, m_rotate);

    }

}
