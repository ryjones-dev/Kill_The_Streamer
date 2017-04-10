using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMoonGolden : Weapon {

    public const float MOONGOLDEN_FIRE_RATE = 0.03f;
    public const int MOONGOLDEN_MAX_AMMO = 600;
    public const string MOONGOLDEN_NAME = "Moon Golden";
    public const int MOONGOLDEN_SPAWNRATE = 100;

    private bool leftRight = false;//left is false, right is true.


    public Sprite m_moonGoldenSprite;

    float angle = 18.0f;
    private Quaternion[] leftAngles= new Quaternion[3];
    private Quaternion[] rightAngles= new Quaternion[3];
    public Quaternion m_leftAngle;
    public Quaternion m_leftAngle1;
    public Quaternion m_leftAngle2;
    public Quaternion m_rightAngle;
    public Quaternion m_rightAngle1;
    public Quaternion m_rightAngle2;

    /// <summary>
    /// Prefab of the bullet to be fired.
    /// </summary>
    public GameObject m_bulletPrefab;

    /// <summary>
    /// Speed the bullet travels.
    /// </summary>
    public const float BULLET_SPEED = 27.0f;


    /// <summary>
    /// Rate of fire
    /// </summary>
    public override float FIRE_RATE { get { return MOONGOLDEN_FIRE_RATE; } }

    /// <summary>
    /// Maximum ammo remaining 
    /// </summary>
    public override int MAX_AMMO { get { return MOONGOLDEN_MAX_AMMO; } }

    /// <summary>
    /// Name of the weapon type (e.g. Pistol, Sniper, Etc)
    /// </summary>
    public override string NAME { get { return MOONGOLDEN_NAME; } }

    public override int SPAWNRATE { get { return MOONGOLDEN_SPAWNRATE; } }

    public override Sprite WEAPON_SPRITE
    {
        get
        {
            return m_moonGoldenSprite;
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
            if (leftRight==false)
            {
                Vector3 bulletAngle = leftAngles[Random.Range(0,leftAngles.Length)] * direction;
                    GameObject bullet = (GameObject)Instantiate(m_bulletPrefab, new Vector3(position.x, 0, position.z), Player.s_Player.FastTransform.Trans.rotation);
                    bullet.GetComponent<Rigidbody>().velocity = bulletAngle * BULLET_SPEED;
                    leftRight = true;
                
            }
            else if(leftRight==true)
            {
                Vector3 bulletAngle = rightAngles[Random.Range(0, rightAngles.Length)] * direction;
                    GameObject bullet = (GameObject)Instantiate(m_bulletPrefab, new Vector3(position.x, 0, position.z), Player.s_Player.FastTransform.Trans.rotation);
                    bullet.GetComponent<Rigidbody>().velocity = bulletAngle * BULLET_SPEED;
                    leftRight = false;
                
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
        m_leftAngle1 = Quaternion.AngleAxis(angle/2, Vector3.up);
        m_leftAngle2 = Quaternion.AngleAxis(angle/3, Vector3.up);
        m_rightAngle = Quaternion.AngleAxis(-angle, Vector3.up);
        m_rightAngle1 = Quaternion.AngleAxis(-angle/2, Vector3.up);
        m_rightAngle2 = Quaternion.AngleAxis(-angle/3, Vector3.up);
        leftAngles[0] = m_leftAngle;
        leftAngles[1] = m_leftAngle1;
        leftAngles[2] = m_leftAngle2;
        rightAngles[0] = m_rightAngle;
        rightAngles[1] = m_rightAngle1;
        rightAngles[2] = m_rightAngle2;
    }
}
