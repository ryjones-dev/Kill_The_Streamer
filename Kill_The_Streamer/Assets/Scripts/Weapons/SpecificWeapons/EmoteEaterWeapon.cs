using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmoteEaterWeapon : Weapon {

    public const float EMOTE_EATER_FIRE_RATE = 0.08f;
    public const int EMOTE_EATER_START_AMMO = 800;
    public const int EMOTE_EATER_MAX_AMMO = 800;
    public const string EMOTE_EATER_NAME = "Emote Eater";
    public const int EMOTE_EATER_SPAWNRATE = 80;
    public Sprite EMOTE_EATER_SPRITE;

    private float rateDecrease = 5f;
    private float currentTimer = 0f;
    /// <summary>
    /// Prefab of the bullet to be fired.
    /// </summary>
    public GameObject m_bulletPrefab;


    //setting up get and set for the current ammo so can add to it if player kills an enemy
    public int Weapon_Ammo
    {
        get { return m_ammo; }
        set { m_ammo = value; }
    }

    /// <summary>
    /// Speed the bullet travels.
    /// </summary>
    public const float BULLET_SPEED = 21.0f;

    /// <summary>
    /// Rate of fire
    /// </summary>
    public override float FIRE_RATE { get { return EMOTE_EATER_FIRE_RATE; } }

    /// <summary>
    /// Maximum ammo remaining 
    /// </summary>
    public override int MAX_AMMO { get { return EMOTE_EATER_MAX_AMMO; } }

    public override int START_AMMO { get { return EMOTE_EATER_START_AMMO; } }

    /// <summary>
    /// Name of the weapon type (e.g. Pistol, Sniper, Etc)
    /// </summary>
    public override string NAME { get { return EMOTE_EATER_NAME; } }

    public override Sprite WEAPON_SPRITE { get { return EMOTE_EATER_SPRITE; } }

    public override int SPAWNRATE { get { return EMOTE_EATER_SPAWNRATE; } }

    /// <summary>
    /// Fires the weapon in the direction given.
    /// </summary>
    /// <param name="direction">The direction the character is aiming in.</param>
    public override void Fire(Vector3 position, Vector3 direction)
    {
        //if(currentTimer >)

        if (m_timer <= 0.0f && m_ammo > 0)
        {

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
         currentTimer++;
        
        if(currentTimer >= rateDecrease && m_ammo > 0)
        {
            currentTimer = 0;
            m_ammo--;
        }
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
