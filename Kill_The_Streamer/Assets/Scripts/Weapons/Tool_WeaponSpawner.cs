using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool_WeaponSpawner : MonoBehaviour {

    public static Tool_WeaponSpawner s_instance;
    public GameObject[] m_spawnableWeaponPrefabs;
    private int[] m_weaponWeights;
	private Quaternion m_rotate;
    private int m_totalWeight;

    void Start()
    {
        if (!s_instance)
        {
            s_instance = this;
        }
		m_rotate = Quaternion.Euler (90, 0, 0);

        m_totalWeight = 0;
        m_weaponWeights = new int[m_spawnableWeaponPrefabs.Length];
        for(int i = 0; i < m_spawnableWeaponPrefabs.Length; ++i)
        {
            m_weaponWeights[i] = m_spawnableWeaponPrefabs[i].GetComponent<Weapon>().SPAWNRATE;
            m_totalWeight += m_weaponWeights[i];
        }
    }

    public void SpawnWeapon(Vector3 position)
    {
        int value = Random.Range(0, m_totalWeight);
        for(int i = 0; i < m_spawnableWeaponPrefabs.Length; ++i)
        {
            value -= m_weaponWeights[i];
            if(value <= 0)
            {
                Instantiate(m_spawnableWeaponPrefabs[i], position, m_rotate);
                return;
            }
        }
    }

}
