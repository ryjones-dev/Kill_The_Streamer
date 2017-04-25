using System;
using UnityEngine;

public class WeaponCannon : Weapon
{
	public const float CANNON_FIRE_RATE = 1.0f;
	public const int GATLING_GUN_START_AMMO = 2;
	public const int CANNON_MAX_AMMO = 3;
	public const string CANNON_NAME = "Blast Cannon";
	public const int CANNON_SPAWNRATE = 60;
	public Sprite CANNON_SPRITE;

	/// <summary>
	/// Prefab of the bullet to be fired.
	/// </summary>
	public GameObject m_bulletPrefab;

	/// <summary>
	/// Speed the bullet travels.
	/// </summary>
	public const float BULLET_SPEED = 3.3f;

	/// <summary>
	/// Rate of fire
	/// </summary>
	public override float FIRE_RATE { get { return CANNON_FIRE_RATE; } }

	/// <summary>
	/// Maximum ammo remaining 
	/// </summary>
	public override int MAX_AMMO { get { return CANNON_MAX_AMMO; } }

	/// <summary>
	/// Name of the weapon type (e.g. Pistol, Sniper, Etc)
	/// </summary>
	public override string NAME { get { return CANNON_NAME; } }

	public override Sprite WEAPON_SPRITE { get { return CANNON_SPRITE; } }

	public override int SPAWNRATE { get { return CANNON_SPAWNRATE; } }

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

	public override void Start(){
		base.Start ();
	}
}