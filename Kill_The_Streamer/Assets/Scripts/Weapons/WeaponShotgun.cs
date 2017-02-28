using System;
using UnityEngine;

public class WeaponShotgun : Weapon
{
    public const float SHOTGUN_FIRE_RATE = 1.0f;
    public const int SHOTGUN_MAX_AMMO = 2;
    public const string SHOTGUN_NAME = "Shotgun";
    float angle = 15.0f;
    public Quaternion m_leftAngle;
    public Quaternion m_rightAngle;

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

            GameObject bullet = (GameObject)Instantiate(m_bulletPrefab, new Vector3(position.x, 0, position.z), Quaternion.identity);
            bullet.GetComponent<Rigidbody>().velocity = direction * BULLET_SPEED;
            Vector3 newDir = direction;
            Debug.Log(m_leftAngle);
            Debug.Log(m_rightAngle);
            Debug.Log(newDir);
            //spawn bullets in an arc
            for (int i = 0; i < 2; ++i)
            {
                newDir = m_leftAngle * newDir;
                Debug.Log(newDir);
                bullet = (GameObject)Instantiate(m_bulletPrefab, new Vector3(position.x, 0, position.z), Quaternion.identity);
                bullet.GetComponent<Rigidbody>().velocity = newDir * BULLET_SPEED;
            }

            newDir = direction;

            for (int i = 0; i < 2; ++i)
            {
                newDir = m_rightAngle * newDir;
                Debug.Log(newDir);
                bullet = (GameObject)Instantiate(m_bulletPrefab, new Vector3(position.x, 0, position.z), Quaternion.identity);
                bullet.GetComponent<Rigidbody>().velocity = newDir * BULLET_SPEED;
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

    public override void Start()
    {
        base.Start();
        m_leftAngle = Quaternion.AngleAxis(angle, Vector3.up);
        m_rightAngle = Quaternion.AngleAxis(-angle, Vector3.up);
    }
}
