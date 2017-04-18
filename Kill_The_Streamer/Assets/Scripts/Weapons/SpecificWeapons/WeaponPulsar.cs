using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPulsar : Weapon {

    public const float PULSAR_FIRE_RATE = 0.5f;
    public const int PULSAR_MAX_AMMO = 100;
    public const string PULSAR_NAME = "Pulsar";
    public const int PULSAR_SPAWNRATE = 600;
    public Sprite PULSAR_SPRITE;

    /// <summary>
    /// Prefab of the bullet to be fired.
    /// </summary>
    public GameObject m_bulletPrefab;

    /// <summary>
    /// Speed the bullet travels.
    /// </summary>
    public const float BULLET_SPEED = 20.0f;
 

    /// <summary>
    /// Rate of fire
    /// </summary>
    public override float FIRE_RATE { get { return PULSAR_FIRE_RATE; } }

    /// <summary>
    /// Maximum ammo remaining 
    /// </summary>
    public override int MAX_AMMO { get { return PULSAR_MAX_AMMO; } }

    /// <summary>
    /// Name of the weapon type (e.g. Pistol, PULSAR, Etc)
    /// </summary>
    public override string NAME { get { return PULSAR_NAME; } }

    public override Sprite WEAPON_SPRITE { get { return PULSAR_SPRITE; } }

    public override int SPAWNRATE { get { return PULSAR_SPAWNRATE; } }

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
            bullet.GetComponent<PulsarBullet>().Speed = BULLET_SPEED;
            bullet.GetComponent<PulsarBullet>().Direction = direction;
            //bullet.GetComponent<Rigidbody>().velocity = direction * BULLET_SPEED;

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
