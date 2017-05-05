using System;
using UnityEngine;

public class WeaponMachineGun : Weapon
{
    public const float MACHINE_GUN_FIRE_RATE = 0.02f;
    public const int MACHINE_GUN_START_AMMO = 60;
    public const int MACHINE_GUN_MAX_AMMO = 120;
    public const string MACHINE_GUN_NAME = "Machine Gun";
    public const int MACHINE_GUN_SPAWNRATE = 500;
    public Sprite MACHINE_GUN_SPRITE;

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
    public override float FIRE_RATE { get { return MACHINE_GUN_FIRE_RATE; } }

    /// <summary>
    /// Maximum ammo remaining 
    /// </summary>
    public override int MAX_AMMO { get { return MACHINE_GUN_MAX_AMMO; } }

    public override int START_AMMO { get { return MACHINE_GUN_START_AMMO; } }

    /// <summary>
    /// Name of the weapon type (e.g. Pistol, Sniper, Etc)
    /// </summary>
    public override string NAME { get { return MACHINE_GUN_NAME; } }

    public override Sprite WEAPON_SPRITE { get { return MACHINE_GUN_SPRITE; } }

    public override int SPAWNRATE { get { return MACHINE_GUN_SPAWNRATE; } }

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
            CameraShake.AddShake(new Shake(0.06f, 0.1f));
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

	public override void Start(){
		base.Start ();
	}
}