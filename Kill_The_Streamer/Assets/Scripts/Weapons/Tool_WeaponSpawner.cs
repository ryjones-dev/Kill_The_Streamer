using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool_WeaponSpawner : MonoBehaviour {

    public static Tool_WeaponSpawner s_instance;
    public GameObject[] SpawnableWeaponPrefabs;

    void Start()
    {
        if (!s_instance)
        {
            s_instance = this;
        }
    }

    public void SpawnWeapon(Vector3 position)
    {
        Instantiate(SpawnableWeaponPrefabs[Random.Range(0, SpawnableWeaponPrefabs.Length)], position, Quaternion.identity);

    }

}
