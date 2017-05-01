using System;
using UnityEngine;

public class WeaponShmorg : Weapon
{
	public const float SHMORG_FIRE_RATE = 0.1f;
	public const int SHMORG_MAX_AMMO = 30;
	public const string SHMORG_NAME = "Shmorgishborg";
	public const int SHMORG_SPAWNRATE = 80;
	public Sprite SHMORG_SPRITE;

	/// <summary>
	/// Speed the bullet travels.
	/// </summary>
	public const float BULLET_SPEED = 30.0f;

	/// <summary>
	/// Rate of fire
	/// </summary>
	public override float FIRE_RATE { get { return SHMORG_FIRE_RATE; } }

	/// <summary>
	/// Maximum ammo remaining 
	/// </summary>
	public override int MAX_AMMO { get { return SHMORG_MAX_AMMO; } }

	/// <summary>
	/// Name of the weapon type (e.g. Pistol, Sniper, Etc)
	/// </summary>
	public override string NAME { get { return SHMORG_NAME; } }

	public override Sprite WEAPON_SPRITE { get { return SHMORG_SPRITE; } }

	public override int SPAWNRATE { get { return SHMORG_SPAWNRATE; } }

	/// <summary>
	/// Fires the weapon in the direction given.
	/// </summary>
	/// <param name="direction">The direction the character is aiming in.</param>
	public override void Fire(Vector3 position, Vector3 direction)
	{
		if (m_timer <= 0.0f && m_ammo > 0)
		{
			m_ammo--;
			//GameObject[] bullets = { m_bulletPrefab, m_bulletPrefab2, m_bulletPrefab3, m_bulletPrefab4, m_bulletPrefab5};
			int choice = UnityEngine.Random.Range (0, Tool_WeaponSpawner.s_instance.m_shmorgBullets.Length);


			GameObject bullet = (GameObject)Instantiate(Tool_WeaponSpawner.s_instance.m_shmorgBullets[choice], new Vector3(position.x, 0, position.z), Quaternion.identity);
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