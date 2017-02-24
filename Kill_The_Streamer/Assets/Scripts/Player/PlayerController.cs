using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    //variables
    
    public float defaultSpeed = 8.0f;
    private float speed;
    public bool dash = false;
    public float dashSpeed = 40.0f;
    public Vector3 velocity = new Vector3(0, 0, 0);
    float dashTime = 0.0f;


    public const int MAX_HEALTH = 40000;//40,000

    public int m_health;

    public GameObject m_pistolPrefab;
	public GameObject m_weaponRenderer;
    public GameObject m_HealthBarObject;
    public Image m_HealthBar;
    public Text m_HealthBarText;

    public Weapon m_primaryWeapon;
    public Weapon m_secondaryWeapon;
    public SpriteRenderer m_weaponSpriteRenderer;

    public GameObject m_primaryWeaponUIObject;
    public GameObject m_secondaryWeaponUIObject;

    private Image m_primaryWeaponUI;
    private Image m_secondaryWeaponUI;

    private Text m_primaryWeaponAmmo;
    private Text m_secondaryWeaponAmmo;

    // Use this for initialization
    void Start()
    {
        speed = defaultSpeed;
        m_health = MAX_HEALTH;
        m_HealthBar = m_HealthBarObject.GetComponent<Image>();
        m_HealthBarText = m_HealthBarObject.GetComponentInChildren<Text>();

        m_weaponRenderer = this.GetComponentInChildren<WeaponRotation>().gameObject;
        m_weaponSpriteRenderer = m_weaponRenderer.GetComponent<SpriteRenderer>();

        m_primaryWeaponUI = m_primaryWeaponUIObject.GetComponent<Image>();
        m_secondaryWeaponUI = m_secondaryWeaponUIObject.GetComponent<Image>();

        m_primaryWeaponAmmo = m_primaryWeaponUIObject.GetComponentInChildren<Text>();
        m_primaryWeaponAmmo = m_primaryWeaponUIObject.GetComponentInChildren<Text>();

        GameObject primaryWeapon = (GameObject)Instantiate(m_pistolPrefab);
        m_primaryWeapon = primaryWeapon.GetComponent<WeaponPistol>();
        m_primaryWeapon.m_held = true;
        m_primaryWeapon.m_ammo = m_primaryWeapon.MAX_AMMO;

        m_secondaryWeapon = null;

        UpdateWeaponUI();
    }

    /// <summary>
    /// Deals damage to the player and updates the health bar.
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage)
    {
        m_health -= damage;
        if (m_health < 0)
        {
            m_health = 0;
            Die();
        }

        m_HealthBar.fillAmount = (float)m_health / MAX_HEALTH;
        if(m_health >= 10000)
        {
            m_HealthBarText.text = (m_health / 1000) + "k";
        }else
        {
            m_HealthBarText.text = m_health.ToString();
        }
    }

    /// <summary>
    /// Function called when player dies.
    /// </summary>
    public void Die()
    {

    }

    /// <summary>
    /// Swaps the active weapon with the secondary weapon, and destroys the active weapon
    /// if it has no ammo.
    /// </summary>
    private void SwapWeapon()
    {
        Weapon temp = m_primaryWeapon;
        m_primaryWeapon = m_secondaryWeapon;
        if (temp.m_ammo == 0)
        {
            Destroy(temp.gameObject);
            m_secondaryWeapon = null;
            m_secondaryWeaponUIObject.SetActive(false);
        }
        else
        {
            m_secondaryWeapon = temp;
        }

        UpdateWeaponUI();

    }

    private void GrabWeapon()
    {

    }

    /// <summary>
    /// Updates both weapons ammo and sprite as necessary.
    /// </summary>
    private void UpdateWeaponUI()
    {
        m_weaponSpriteRenderer.sprite = m_primaryWeapon.WEAPON_SPRITE;

        m_primaryWeaponUI.sprite = m_primaryWeapon.WEAPON_SPRITE;
        if (m_primaryWeapon.MAX_AMMO != -1)
        {
            m_primaryWeaponAmmo.text = m_primaryWeapon.m_ammo + "|" + m_primaryWeapon.MAX_AMMO;
        }
        else
        {
            m_primaryWeaponAmmo.text = "∞|∞";
        }

        if (m_secondaryWeapon != null)
        {
            m_secondaryWeaponUI.sprite = m_secondaryWeapon.WEAPON_SPRITE;
            if (m_secondaryWeapon.MAX_AMMO != -1)
            {
                m_secondaryWeaponAmmo.text = m_secondaryWeapon.m_ammo + "|" + m_secondaryWeapon.MAX_AMMO;
            }
            else
            {
                m_secondaryWeaponAmmo.text = "∞|∞";
            }
        }
    }

    /// <summary>
    /// Updates the primary weapon's ammo ammounts, to be used after every Fire();
    /// </summary>
    private void UpdatePrimaryWeaponAmmo()
    {
        if (m_primaryWeapon.MAX_AMMO != -1)
        {
            m_primaryWeaponAmmo.text = m_primaryWeapon.m_ammo + "|" + m_primaryWeapon.MAX_AMMO;
        }
        else
        {
            m_primaryWeaponAmmo.text = "∞|∞";
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //check for input
        Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 tempVelocity = new Vector3(0,0,0);
        cursorPos.y = 0;
        Debug.DrawLine(transform.position, cursorPos, Color.red);

        TakeDamage(40);
        //check for dash
        if (dash == true)
        {
            if (Time.time - dashTime >= 0.5f)
            {
                dash = false;
                speed = defaultSpeed;
            }
        }
        //movement
        
        if (Input.GetKey(KeyCode.W) && dash == false)
        {

           tempVelocity += new Vector3(0, 0, 1);
        }

        if (Input.GetKey(KeyCode.A) && dash == false)
        {
            tempVelocity += new Vector3(-1, 0, 0);
        }

        if (Input.GetKey(KeyCode.S) && dash == false)
        {
            tempVelocity += new Vector3(0, 0, -1);
        }

        if (Input.GetKey(KeyCode.D) && dash == false)
        {
            tempVelocity += new Vector3(1, 0, 0);
        }

        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) && dash == false)
        {
            speed = dashSpeed;
            dash = true;
            dashTime = Time.time;
        }

        if (tempVelocity != Vector3.zero)
        {
            tempVelocity.Normalize();
        }

        velocity += tempVelocity * speed;

        //cap the speed
        if(velocity.x >= defaultSpeed && dash == false)
        {
            velocity.x = defaultSpeed;
        }

        if (velocity.x <= -defaultSpeed && dash == false)
        {
            velocity.x = -defaultSpeed;
        }

        if (velocity.y >= defaultSpeed && dash == false)
        {
            velocity.y = defaultSpeed;
        }

        if (velocity.y <= -defaultSpeed && dash == false)
        {
            velocity.y = -defaultSpeed;
        }

        if (velocity.z >= defaultSpeed && dash == false)
        {
            velocity.z = defaultSpeed;
        }

        if (velocity.z <= -defaultSpeed && dash == false)
        {
            velocity.z = -defaultSpeed;
        }

        transform.position += velocity * Time.deltaTime;
        transform.forward = (cursorPos - transform.position).normalized;
        velocity = velocity * 0.8f;
        //shooting
        if (Input.GetMouseButton(0))
        {
			m_primaryWeapon.Fire(m_weaponRenderer.transform.position, this.transform.forward);
            UpdatePrimaryWeaponAmmo();
        }

        if (Input.GetKeyDown(KeyCode.Q) && m_secondaryWeapon != null)
        {
            SwapWeapon();
        }
    }
}
