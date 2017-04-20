using System;
using UnityEngine;

public class WeaponGatling : Weapon
{
    public const float GATLING_GUN_FIRE_RATE = 0.005f;
    public const int GATLING_GUN_START_AMMO = 150;
    public const int GATLING_GUN_MAX_AMMO = 200;
    public const string GATLING_GUN_NAME = "Gatling Gun";
    public const int GATLING_GUN_SPAWNRATE = 600;
    public Sprite GATLING_GUN_SPRITE;
	float angle = 1.0f;
	public Quaternion m_leftAngle;
	public Quaternion m_rightAngle;
	public Quaternion m_leftAngle2;
	public Quaternion m_rightAngle2;
	public Quaternion m_leftAngle3;
	public Quaternion m_rightAngle3;
	public Quaternion m_leftAngle4;
	public Quaternion m_rightAngle4;
	public Quaternion m_leftAngle5;
	public Quaternion m_rightAngle5;
	public Quaternion m_leftAngle6;
	public Quaternion m_rightAngle6;
	public Quaternion m_leftAngle7;
	public Quaternion m_rightAngle7;
	public Quaternion m_leftAngle8;
	public Quaternion m_rightAngle8;
	public Quaternion m_leftAngle9;
	public Quaternion m_rightAngle9;
	public Quaternion m_leftAngle10;
	public Quaternion m_rightAngle10;
    public Vector3 recoil;


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
			Vector3[] directions = { direction, (m_leftAngle * direction), (m_rightAngle * direction), (m_leftAngle2 * direction), (m_rightAngle2 * direction),
				(m_leftAngle3 * direction), (m_rightAngle3 * direction), (m_leftAngle4 * direction), (m_rightAngle4 * direction), (m_leftAngle5 * direction),
				(m_rightAngle5 * direction), (m_leftAngle6 * direction), (m_rightAngle6 * direction), (m_leftAngle7 * direction), (m_rightAngle7 * direction),
				(m_leftAngle8 * direction), (m_rightAngle8 * direction), (m_leftAngle9 * direction), (m_rightAngle9 * direction), (m_leftAngle10 * direction), (m_rightAngle10 * direction)};
			int choice = UnityEngine.Random.Range (0, 20);


            GameObject bullet = (GameObject)Instantiate(m_bulletPrefab, new Vector3(position.x, 0, position.z), Quaternion.identity);
			bullet.GetComponent<Rigidbody>().velocity = directions[choice] * BULLET_SPEED;

			recoil = directions[choice] * -1;
			recoil = recoil * 0.1f;

            Player.s_Player.FastTransform.Position += recoil;

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
		m_leftAngle2 = Quaternion.AngleAxis(angle * 2, Vector3.up);
		m_rightAngle2 = Quaternion.AngleAxis(-angle * 2, Vector3.up);
		m_leftAngle3 = Quaternion.AngleAxis(angle * 3, Vector3.up);
		m_rightAngle3 = Quaternion.AngleAxis(-angle * 3, Vector3.up);
		m_leftAngle4 = Quaternion.AngleAxis(angle * 4, Vector3.up);
		m_rightAngle4 = Quaternion.AngleAxis(-angle * 4, Vector3.up);
		m_leftAngle5 = Quaternion.AngleAxis(angle * 5, Vector3.up);
		m_rightAngle5 = Quaternion.AngleAxis(-angle * 5, Vector3.up);
		m_leftAngle6 = Quaternion.AngleAxis(angle * 6, Vector3.up);
		m_rightAngle6 = Quaternion.AngleAxis(-angle * 6, Vector3.up);
		m_leftAngle7 = Quaternion.AngleAxis(angle * 7, Vector3.up);
		m_rightAngle7 = Quaternion.AngleAxis(-angle * 7, Vector3.up);
		m_leftAngle8 = Quaternion.AngleAxis(angle * 8, Vector3.up);
		m_rightAngle8 = Quaternion.AngleAxis(-angle * 8, Vector3.up);
		m_leftAngle9 = Quaternion.AngleAxis(angle * 9, Vector3.up);
		m_rightAngle9 = Quaternion.AngleAxis(-angle * 9, Vector3.up);
		m_leftAngle10 = Quaternion.AngleAxis(angle * 10, Vector3.up);
		m_rightAngle10 = Quaternion.AngleAxis(-angle * 10, Vector3.up);
	}
}