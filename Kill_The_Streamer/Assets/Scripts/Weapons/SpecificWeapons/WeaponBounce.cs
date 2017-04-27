using System;
using UnityEngine;

public class WeaponBounce : Weapon
{
    public const float BOUNCE_FIRE_RATE = 0.4f;
    public const int BOUNCE_MAX_AMMO = 10;
    public const string BOUNCE_NAME = "Bouncer";
    public const int BOUNCE_SPAWNRATE = 250;
    public Sprite BOUNCE_SPRITE;

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
    public override float FIRE_RATE { get { return BOUNCE_FIRE_RATE; } }

    /// <summary>
    /// Maximum ammo remaining 
    /// </summary>
    public override int MAX_AMMO { get { return BOUNCE_MAX_AMMO; } }

    /// <summary>
    /// Name of the weapon type (e.g. Pistol, Sniper, Etc)
    /// </summary>
    public override string NAME { get { return BOUNCE_NAME; } }

    public override Sprite WEAPON_SPRITE { get { return BOUNCE_SPRITE; } }

    public override int SPAWNRATE { get { return BOUNCE_SPAWNRATE; } }

    /// <summary>
    /// Fires the weapon in the direction given.
    /// </summary>
    /// <param name="direction">The direction the character is aiming in.</param>
    public override void Fire(Vector3 position, Vector3 direction)
    {
        if (m_timer <= 0.0f) // && m_ammo > 0 except pistols have infinite ammo
        {
            m_ammo--;// Pistols have infinite ammo

            GameObject bullet = (GameObject)Instantiate(m_bulletPrefab, new Vector3(position.x, 0, position.z), Quaternion.identity);
            bullet.GetComponent<Rigidbody>().velocity = direction * BULLET_SPEED;

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