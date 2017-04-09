using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAlienInjector : Weapon
{

    public const float ALIEN_FIRE_RATE = 0.5f;
    public const int ALIEN_START_AMMO = 1;
    public const int ALIEN_MAX_AMMO = 2;
    public const string ALIEN_NAME = "Alien Injector";
    public const int ALIEN_SPAWNRATE = 50;
    public Sprite ALIEN_SPRITE;

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
    public override float FIRE_RATE { get { return ALIEN_FIRE_RATE; } }

    /// <summary>
    /// Maximum ammo remaining 
    /// </summary>
    public override int MAX_AMMO { get { return ALIEN_MAX_AMMO; } }

    public override int START_AMMO { get { return ALIEN_START_AMMO; } }

    /// <summary>
    /// Name of the weapon type (e.g. Pistol, Sniper, Etc)
    /// </summary>
    public override string NAME { get { return ALIEN_NAME; } }

    public override Sprite WEAPON_SPRITE { get { return ALIEN_SPRITE; } }

    public override int SPAWNRATE { get { return ALIEN_SPAWNRATE; } }

    /// <summary>
    /// Fires the weapon in the direction given.
    /// </summary>
    /// <param name="direction">The direction the character is aiming in.</param>
    public override void Fire(Vector3 position, Vector3 direction)
    {
        if (m_timer <= 0.0f && m_ammo > 0)
        {
            m_ammo--;

            GameObject bullet = (GameObject)Instantiate(m_bulletPrefab, new Vector3(position.x, 0, position.z), Player.s_Player.FastTransform.Trans.rotation);
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

    public override void Start()
    {
        base.Start();
    }
}
