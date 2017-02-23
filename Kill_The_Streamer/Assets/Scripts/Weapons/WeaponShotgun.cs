using System;
using UnityEngine;

public class WeaponShotgun : Weapon
{
    public const float SHOTGUN_FIRE_RATE = 1.0f;
    public const int SHOTGUN_MAX_AMMO = 2;
    public const string SHOTGUN_NAME = "Shotgun";

    /// <summary>
    /// Prefab of the bullet to be fired.
    /// </summary>
    public GameObject m_bulletPrefab;

    /// <summary>
    /// Speed the bullet travels.
    /// </summary>
    public const float BULLET_SPEED = 10.0f;

    /// <summary>
    /// Rate of fire
    /// </summary>
    public override float FIRE_RATE { get { return SHOTGUN_FIRE_RATE; } }

    /// <summary>
    /// Maximum ammo remaining 
    /// </summary>
    public override int MAX_AMMO { get { return SHOTGUN_MAX_AMMO; } }

    /// <summary>
    /// Name of the weapon type (e.g. Pistol, Sniper, Etc)
    /// </summary>
    public override string NAME { get { return SHOTGUN_NAME; } }

    /// <summary>
    /// Fires the weapon in the direction given.
    /// </summary>
    /// <param name="direction">The direction the character is aiming in.</param>
    public override void Fire(Vector3 position, Vector3 direction)
    {
        if (m_timer <= 0.0f && m_ammo > 0)
        {
            m_ammo--;

            //spawn bullets in an arc
            float radius = 5.0f;
            for (int i = 0; i < 5; i++)
            {
                int check = i;
                if (i % 2 == 0 && i != 0)
                {
                    check *= -1;
                }
                float angle = 18 * check;
                Vector3 pos = position + new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad) * radius, 0, Mathf.Cos(angle * Mathf.Deg2Rad) * radius);
                GameObject bullet = (GameObject)Instantiate(m_bulletPrefab, pos, Quaternion.identity);
                bullet.GetComponent<Rigidbody>().velocity = direction * BULLET_SPEED;
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
