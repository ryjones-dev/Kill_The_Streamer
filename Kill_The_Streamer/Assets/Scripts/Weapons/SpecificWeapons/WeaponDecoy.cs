using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDecoy : Weapon
{
    public const float DECOY_FIRE_RATE = 0.1f;
    public const int DECOY_MAX_AMMO = 1;
    public const string DECOY_NAME = "Decoy";
    public const int DECOY_SPAWNRATE = 75;
    public Sprite DECOY_SPRITE;

    /// <summary>
    /// Prefab of the bullet to be fired.
    /// </summary>
    public GameObject m_bulletPrefab;

    /// <summary>
    /// Speed the bullet travels.
    /// </summary>
    public const float BULLET_SPEED = 30.0f;

    /// <summary>
    /// Rate of fire
    /// </summary>
    public override float FIRE_RATE { get { return DECOY_FIRE_RATE; } }

    /// <summary>
    /// Maximum ammo remaining 
    /// </summary>
    public override int MAX_AMMO { get { return DECOY_MAX_AMMO; } }

    /// <summary>
    /// Name of the weapon type (e.g. Pistol, Sniper, Etc)
    /// </summary>
    public override string NAME { get { return DECOY_NAME; } }

    public override Sprite WEAPON_SPRITE { get { return DECOY_SPRITE; } }

    public override int SPAWNRATE { get { return DECOY_SPAWNRATE; } }

    /// <summary>
    /// Fires the weapon in the direction given.
    /// </summary>
    /// <param name="direction">The direction the character is aiming in.</param>
    public override void Fire(Vector3 position, Vector3 direction)
    {
        if (m_timer <= 0.0f && m_ammo > 0)
        {
            m_ammo--;

            GameObject bullet = (GameObject)Instantiate(m_bulletPrefab, new Vector3(position.x, 0, position.z), Quaternion.identity);
            bullet.GetComponent<Rigidbody>().velocity = direction * BULLET_SPEED;

            for (int i = 0; i < 7; i++)
            {
                int index;
                AIBase[] ais = EnemyManager.GetAllEnemyAI((EnemyType)i, out index);
                for (int j = 0; j < ais.Length; j++)
                {
                    ais[j].Target = bullet.transform;
                }
            }

            m_timer = FIRE_RATE;
        }
    }

    /// <summary>
    /// Updates any changes necessary to the weapon.
    /// </summary>
    public override void Update()
    {
        base.Update();

        if (m_timer > 0.0f)
        {
            m_timer -= Time.deltaTime;
        }
    }
}
