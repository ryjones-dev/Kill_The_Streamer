using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponVacuum : Weapon {

    public const float VACUUM_FIRE_RATE = 0.01f;
    public const int VACUUM_MAX_AMMO = 500;
    public const string VACUUM_NAME = "Vacuum";
    public const int VACUUM_SPAWNRATE = 100;
    public const float VACUUM_RANGE = 15 * 15;
    public const float VACUUM_KILL_RANGE = 4 * 4;

    public Sprite VACUUM_SPRITE;

    /// <summary>
    /// Rate of fire
    /// </summary>
    public override float FIRE_RATE { get { return VACUUM_FIRE_RATE; } }

    /// <summary>
    /// Maximum ammo remaining 
    /// </summary>
    public override int MAX_AMMO { get { return VACUUM_MAX_AMMO; } }

    /// <summary>
    /// Name of the weapon type (e.g. Pistol, Sniper, Etc)
    /// </summary>
    public override string NAME { get { return VACUUM_NAME; } }

    public override Sprite WEAPON_SPRITE { get { return VACUUM_SPRITE; } }

    public override int SPAWNRATE { get { return VACUUM_SPAWNRATE; } }

    /// <summary>
    /// Fires the weapon in the direction given.
    /// </summary>
    /// <param name="direction">The direction the character is aiming in.</param>
    public override void Fire(Vector3 position, Vector3 direction)
    {
        if(m_ammo > 0)
        {
            VacuumParticleSpawner.s_Instance.active = true;
            VacuumParticleSpawner.s_Instance.gameObject.SetActive(true);
        }
        if (m_timer <= 0.0f && m_ammo > 0)
        {
            m_ammo--;

            int numEnemiesAffected = 0;

            int enemyTypesLength = Enum.GetNames(typeof(EnemyType)).Length;
            for (int i = 0; i < enemyTypesLength; ++i) {
                int length = 0;
                AIBase[] enemies = EnemyManager.GetAllEnemyAI((EnemyType)i, out length);

                for(int j = 0; j < length; ++j)
                {
                    // If an enemy is deactived while looping, make sure that it's not destroyed again
                    if (!enemies[j].gameObject.activeSelf) continue;

                    Vector3 differenceVector = Player.s_Player.FastTransform.Position - enemies[j].FastTransform.Position;
                    float sqrDistance = differenceVector.sqrMagnitude;

                    
                    if (sqrDistance < VACUUM_RANGE)
                    {
                        bool positiveLeft = Vector3.Dot(differenceVector, Player.s_Player.LeftVisionAngle) < 0;
                        bool positiveRight = Vector3.Dot(differenceVector, Player.s_Player.RightVisionAngle) < 0;

                        if (positiveLeft & positiveRight)
                        {
                            enemies[j].RigidBody.AddForce(differenceVector.normalized * 50);
                            numEnemiesAffected++;

                            if (sqrDistance < VACUUM_KILL_RANGE)
                            {
                                enemies[j].TakeDamage();
                            }
                        }
                    }
                }

            }

            m_timer += FIRE_RATE;

            CameraShake.AddShake(new Shake(0.02f * numEnemiesAffected + 0.02f, 0.01f));
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
