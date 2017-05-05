using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBigBertha : Weapon {

    public const float BERTHA_FIRE_RATE = 0.2f;
    public const int BERTHA_MAX_AMMO = 30;
    public const string BERTHA_NAME = "Big Bertha";
    public const int BERTHA_SPAWNRATE = 300;


    public Sprite m_bigBerthaSprite;

    /// <summary>
    /// Prefab of the bullet to be fired.
    /// </summary>
    public GameObject m_bulletPrefab;

    /// <summary>
    /// Speed the bullet travels.
    /// </summary>
    public const float BULLET_SPEED = 8.0f;


    /// <summary>
    /// Rate of fire
    /// </summary>
    public override float FIRE_RATE { get { return BERTHA_FIRE_RATE; } }

    /// <summary>
    /// Maximum ammo remaining 
    /// </summary>
    public override int MAX_AMMO { get { return BERTHA_MAX_AMMO; } }

    /// <summary>
    /// Name of the weapon type (e.g. Pistol, Sniper, Etc)
    /// </summary>
    public override string NAME { get { return BERTHA_NAME; } }

    public override int SPAWNRATE { get { return BERTHA_SPAWNRATE; } }

    public override Sprite WEAPON_SPRITE
    {
        get
        {
            return m_bigBerthaSprite;
        }
    }


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
