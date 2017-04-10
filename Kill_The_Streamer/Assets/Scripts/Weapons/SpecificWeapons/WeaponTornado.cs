using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTornado : Weapon {

    public const float TORNADO_FIRE_RATE = 0.02f;
    public const int TORNADO_MAX_AMMO = 999;
    public const string TORNADO_NAME = "Tornado";
    public const int TORNADO_SPAWNRATE = 40;
    public const float TORNADO_RANGE = 13 * 13;

    public Sprite TORNADO_SPRITE;
    private Quaternion TORNADO_ROTATION = Quaternion.AngleAxis(90.0f, Vector3.up);

    /// <summary>
    /// Rate of fire
    /// </summary>
    public override float FIRE_RATE { get { return TORNADO_FIRE_RATE; } }

    /// <summary>
    /// Maximum ammo remaining 
    /// </summary>
    public override int MAX_AMMO { get { return TORNADO_MAX_AMMO; } }

    /// <summary>
    /// Name of the weapon type (e.g. Pistol, Sniper, Etc)
    /// </summary>
    public override string NAME { get { return TORNADO_NAME; } }

    public override Sprite WEAPON_SPRITE { get { return TORNADO_SPRITE; } }

    public override int SPAWNRATE { get { return TORNADO_SPAWNRATE; } }

    /// <summary>
    /// Fires the weapon in the direction given.
    /// </summary>
    /// <param name="direction">The direction the character is aiming in.</param>
    public override void Fire(Vector3 position, Vector3 direction)
    {

        if (m_timer <= 0.0f && m_ammo > 0)
        {
            m_ammo--;

            for (int i = 0; i < 6; ++i)
            {
                int length = 0;
                AIBase[] enemies = EnemyManager.GetAllEnemyAI((EnemyType)i, out length);

                for (int j = 0; j < length; ++j)
                {
                    Vector3 differenceVector = enemies[j].FastTransform.Position - Player.s_Player.FastTransform.Position;
                    float sqrDistance = differenceVector.sqrMagnitude;

                    if (sqrDistance < TORNADO_RANGE)
                    {

                            enemies[j].RigidBody.AddForce(TORNADO_ROTATION * (differenceVector.normalized * 50));

                    }
                }

            }
            m_timer += FIRE_RATE;
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
