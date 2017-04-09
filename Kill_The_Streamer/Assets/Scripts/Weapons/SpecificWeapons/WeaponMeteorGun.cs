using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMeteorGun : Weapon
{
    public const float METEOR_FIRE_RATE = 0.3f;
    public const int METEOR_MAX_AMMO = 10;
    public const string METEOR_NAME = "Meteor Gun";
    public const int METEOR_SPAWNRATE = 50;
    public Sprite METEOR_SPRITE;

    /// <summary>
    /// Prefab of the bullet to be fired.
    /// </summary>
    public GameObject m_bulletPrefab;

    /// <summary>
    /// Speed the bullet travels.
    /// </summary>
    public const float BULLET_SPEED = 20.0f;

    /// <summary>
    /// The minimum number of meteors to spawn when fired.
    /// </summary>
    public const int minMeteors = 3;

    /// <summary>
    /// The maximum number of meteors to spawn when fired.
    /// </summary>
    public const int maxMeteors = 7;

    /// <summary>
    /// The maximum amount of variance between a meteor's spawn position and the mouse position.
    /// </summary>
    public const float spawnVariance = 5.0f;

    public override float FIRE_RATE { get { return METEOR_FIRE_RATE; } }

    public override int MAX_AMMO { get { return METEOR_MAX_AMMO; } }

    public override string NAME { get { return METEOR_NAME; } }

    public override int SPAWNRATE { get { return METEOR_SPAWNRATE; } }

    public override Sprite WEAPON_SPRITE { get { return METEOR_SPRITE; } }

    public override void Fire(Vector3 position, Vector3 direction)
    {
        if (m_timer <= 0.0f && m_ammo > 0)
        {
            m_ammo--;

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.y = 0;

            Vector3 meteorPosition = mousePosition + new Vector3(-direction.x * 20, 10, -direction.z * 20);
            Vector3 meteorDirection = (mousePosition - meteorPosition).normalized;

            float numMeteors = UnityEngine.Random.Range(minMeteors, maxMeteors);

            for (int i = 0; i < numMeteors; i++)
            {
                Vector2 variance = UnityEngine.Random.insideUnitCircle.normalized * spawnVariance;
                Vector3 finalMeteorPosition = new Vector3(meteorPosition.x + variance.x, meteorPosition.y, meteorPosition.z + variance.y);

                GameObject meteorBullet = Instantiate<GameObject>(m_bulletPrefab, finalMeteorPosition, Quaternion.identity);
                meteorBullet.GetComponent<Rigidbody>().velocity = meteorDirection * BULLET_SPEED;
                meteorBullet.transform.FindChild("Shadow").position += new Vector3(direction.x * 7.5f , 0, direction.z * 7.5f);
            }

            m_timer = FIRE_RATE;
        }
    }

    public override void Update()
    {
        base.Update();

        if (m_timer > 0.0f)
        {
            m_timer -= Time.deltaTime;
        }
    }
}
