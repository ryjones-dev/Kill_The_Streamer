using System;
using UnityEngine;

public class WeaponGatling : Weapon
{
    public const float GATLING_GUN_FIRE_RATE = 0.005f;
    public const int GATLING_GUN_START_AMMO = 150;
    public const int GATLING_GUN_MAX_AMMO = 200;
    public const string GATLING_GUN_NAME = "Gatling Gun";
    public const int GATLING_GUN_SPAWNRATE = 300;
    public Sprite GATLING_GUN_SPRITE;
	float angle = 1.0f;
	public Quaternion[] m_angles;
    public Vector3 recoil;


    /// <summary>
    /// Prefab of the bullet to be fired.
    /// </summary>
    public GameObject m_bulletPrefab;

    /// <summary>
    /// Speed the bullet travels.
    /// </summary>
    public const float BULLET_SPEED = 60.0f;

    /// <summary>
    /// Rate of fire
    /// </summary>
    public override float FIRE_RATE { get { return GATLING_GUN_FIRE_RATE; } }

    /// <summary>
    /// Maximum ammo remaining 
    /// </summary>
    public override int MAX_AMMO { get { return GATLING_GUN_MAX_AMMO; } }

    public override int START_AMMO { get { return GATLING_GUN_START_AMMO; } }

    /// <summary>
    /// Name of the weapon type (e.g. Pistol, Sniper, Etc)
    /// </summary>
    public override string NAME { get { return GATLING_GUN_NAME; } }

    public override Sprite WEAPON_SPRITE { get { return GATLING_GUN_SPRITE; } }

    public override int SPAWNRATE { get { return GATLING_GUN_SPAWNRATE; } }

    /// <summary>
    /// Fires the weapon in the direction given.
    /// </summary>
    /// <param name="direction">The direction the character is aiming in.</param>
    public override void Fire(Vector3 position, Vector3 direction)
    {
        if (m_timer <= 0.0f && m_ammo > 0)
        {
            m_ammo--;

			int choice = UnityEngine.Random.Range (0, m_angles.Length);

            GameObject bullet = (GameObject)Instantiate(m_bulletPrefab, new Vector3(position.x, 0, position.z), Quaternion.identity);
			bullet.GetComponent<Rigidbody>().velocity = (m_angles[choice] * direction).normalized * BULLET_SPEED;

			recoil = bullet.GetComponent<Rigidbody>().velocity * -1;
			recoil = recoil * 20.0f;

			Player.s_Player.m_rigidbody.AddForce(recoil);

            m_timer = FIRE_RATE;
            CameraShake.AddShake(new Shake(0.1f, 0.1f));
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

		m_angles = new Quaternion[21];
		for (int i = -10; i < m_angles.Length - 10; ++i) {
			m_angles [i + 10] = Quaternion.AngleAxis (angle * i, Vector3.up); 
		}
	}
}