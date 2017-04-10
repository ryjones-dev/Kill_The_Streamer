using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBubbleBlower : Weapon
{

    public const float BUBBLEBLOWER_FIRE_RATE = 0.7f;
    public const int BUBBLEBLOWER_MAX_AMMO = 20;
    public const string BUBBLEBLOWER_NAME = "Bubble Blower";
    public const int BUBBLEBLOWER_SPAWNRATE = 100;


    public Sprite m_bubbleBlowerSprite;



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
    public override float FIRE_RATE { get { return BUBBLEBLOWER_FIRE_RATE; } }

    /// <summary>
    /// Maximum ammo remaining 
    /// </summary>
    public override int MAX_AMMO { get { return BUBBLEBLOWER_MAX_AMMO; } }

    /// <summary>
    /// Name of the weapon type (e.g. Pistol, Sniper, Etc)
    /// </summary>
    public override string NAME { get { return BUBBLEBLOWER_NAME; } }

    public override int SPAWNRATE { get { return BUBBLEBLOWER_SPAWNRATE; } }

    public override Sprite WEAPON_SPRITE
    {
        get
        {
            return m_bubbleBlowerSprite;
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
