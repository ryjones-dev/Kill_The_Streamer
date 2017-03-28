using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    //Get the player
    public static PlayerController s_Player;

    //variables


    public bool dash = false;
    float dashTime = 0.0f;


    public const int MAX_HEALTH = 40000;//40,000

    public int m_health;
    public bool m_isAlive = true;

    private Dictionary<string, int> m_damageDoneByViewers;
    private string m_killedBy;

    public GameObject m_pistolPrefab;
    [HideInInspector]
    public GameObject m_weaponRenderer;
    public GameObject m_HealthBarObject;
    [HideInInspector]
    public Image m_HealthBar;
    [HideInInspector]
    public Text m_HealthBarText;
    [HideInInspector]
    public Text m_weaponPickupText;

    [HideInInspector]
    public Weapon m_primaryWeapon;
    [HideInInspector]
    public Weapon m_secondaryWeapon;
    [HideInInspector]
    public SpriteRenderer m_weaponSpriteRenderer;

    [HideInInspector]
    public GameObject m_primaryWeaponObject;
    [HideInInspector]
    public GameObject m_secondaryWeaponObject;
    public GameObject m_primaryWeaponUIObject;
    public GameObject m_secondaryWeaponUIObject;

    public Rigidbody m_rigidbody;

    private Image m_primaryWeaponUI;
    private Image m_secondaryWeaponUI;

    private Text m_primaryWeaponAmmo;
    private Text m_secondaryWeaponAmmo;

    public Transform m_myTransform;
    public Vector3 m_myPosition;

    void Awake()
    {
        s_Player = this;
    }

    // Use this for initialization
    void Start()
    {
        m_myTransform = this.transform;
        m_myPosition = this.transform.position;

        m_health = MAX_HEALTH;
        m_HealthBar = m_HealthBarObject.GetComponent<Image>();
        m_HealthBarText = m_HealthBarObject.GetComponentInChildren<Text>();

        m_weaponPickupText = this.GetComponentInChildren<Text>();
        Debug.Log(m_weaponPickupText);

        m_weaponRenderer = this.GetComponentInChildren<WeaponRotation>().gameObject;
        m_weaponSpriteRenderer = m_weaponRenderer.GetComponent<SpriteRenderer>();

        m_primaryWeaponUI = m_primaryWeaponUIObject.GetComponent<Image>();
        m_secondaryWeaponUI = m_secondaryWeaponUIObject.GetComponent<Image>();

        m_primaryWeaponAmmo = m_primaryWeaponUIObject.GetComponentInChildren<Text>();
        m_secondaryWeaponAmmo = m_secondaryWeaponUIObject.GetComponentInChildren<Text>();

        m_primaryWeaponObject = Instantiate(m_pistolPrefab);
        m_primaryWeapon = m_primaryWeaponObject.GetComponent<Weapon>();
        m_primaryWeapon.m_held = true;
        m_primaryWeapon.m_ammo = m_primaryWeapon.MAX_AMMO;

        m_primaryWeaponObject.transform.parent = this.transform;
        m_primaryWeaponObject.GetComponent<SpriteRenderer>().enabled = false;

        m_secondaryWeapon = null;
        UpdateWeaponUI();

        m_damageDoneByViewers = new Dictionary<string, int>();

        m_rigidbody = GetComponent<Rigidbody>();
    }

    void LateUpdate()
    {
        m_myPosition = m_myTransform.position;
    }

    /// <summary>
    /// Deals damage to the player and updates the health bar.
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage, string name = "Default")
    {
        if (!m_isAlive)
        {
            return;
        }

        if(damage > m_health){
            damage = m_health;
        }

        m_health -= damage;
        if (m_damageDoneByViewers.ContainsKey(name))
        {
            m_damageDoneByViewers[name] += damage;
        }
        else
        {
            m_damageDoneByViewers.Add(name, damage);
        }

        if (m_health <= 0)
        {
            m_killedBy = name;
            m_isAlive = false;
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
        List<KeyValuePair<string, int>> sortedList = m_damageDoneByViewers.ToList();

        sortedList.Sort(
        delegate (KeyValuePair<string, int> kv, KeyValuePair<string, int> kv2)
        {
            return kv2.Value.CompareTo(kv.Value);
        });

        // Change to ending screen here.
        for(int i = 0; i < sortedList.Count; ++i)
        {
            Debug.Log(sortedList[i].Key + ": " + sortedList[i].Value);
        }
    }

    /// <summary>
    /// Swaps the active weapon with the secondary weapon, and destroys the active weapon
    /// if it has no ammo.
    /// </summary>
    private void SwapWeapon()
    {
        GameObject tempObj = m_primaryWeaponObject;
        m_primaryWeaponObject = m_secondaryWeaponObject;
        Weapon temp = m_primaryWeapon;
        m_primaryWeapon = m_secondaryWeapon;
        if (temp.m_ammo == 0)
        {
            Destroy(tempObj);
            m_secondaryWeaponObject = null;
            m_secondaryWeapon = null;
            m_secondaryWeaponUIObject.SetActive(false);
        }
        else
        {
            m_secondaryWeapon = temp;
            m_secondaryWeaponObject = m_primaryWeaponObject;
        }

        UpdateWeaponUI();

    }

    private void GrabWeapon()
    {
        GameObject[] weapons = GameObject.FindGameObjectsWithTag("Weapon");

        if(weapons.Length == 0)
        {
            return;
        }
        bool weaponExists = false;
        GameObject closestWeapon = null;
        Weapon newWeapon = null;
        float length = 3.0f;
        for(int i = 0; i < weapons.Length; ++i)
        {
            newWeapon = weapons[i].GetComponent<Weapon>();
            float newLength = (weapons[i].transform.position - transform.position).sqrMagnitude;
            if (!newWeapon.m_held)
            {
                
                if (newLength <= length)
                {
                    closestWeapon = weapons[i];
                    length = newLength;
                    weaponExists = true;
                }
            }
        }
        if(!weaponExists)
        {
            return;
        }

        newWeapon = closestWeapon.GetComponent<Weapon>();
        if (m_primaryWeapon.NAME == newWeapon.NAME)
        {
            m_primaryWeapon.m_ammo += newWeapon.m_ammo;
            if (m_primaryWeapon.m_ammo > m_primaryWeapon.MAX_AMMO)
            {
                m_primaryWeapon.m_ammo = m_primaryWeapon.MAX_AMMO;
            }
            Destroy(closestWeapon);
        }

        else if (m_secondaryWeapon == null)
        {
            
            m_secondaryWeaponUIObject.SetActive(true);

            m_secondaryWeaponObject = closestWeapon;
            m_secondaryWeapon = newWeapon;
            m_secondaryWeaponObject.transform.parent = this.transform;
            m_secondaryWeaponObject.GetComponent<SpriteRenderer>().enabled = false;
            m_secondaryWeapon.m_held = true;

            SwapWeapon();
        }
        else
        {
            if (m_secondaryWeapon.NAME == newWeapon.NAME)
            {
                m_secondaryWeapon.m_ammo += newWeapon.m_ammo;
                if (m_secondaryWeapon.m_ammo > m_secondaryWeapon.MAX_AMMO)
                {
                    m_secondaryWeapon.m_ammo = m_secondaryWeapon.MAX_AMMO;
                }
                Destroy(closestWeapon);
                SwapWeapon();
            }
            else
            {
                if (m_primaryWeapon.m_ammo == 0)
                {
                    Destroy(m_primaryWeaponObject);

                }
                else
                {
                    m_primaryWeapon.m_timer = 0;
                    m_primaryWeapon.transform.parent = null;
                    m_primaryWeapon.GetComponent<SpriteRenderer>().enabled = true;
                    m_primaryWeapon.m_held = false;
                }

                m_primaryWeaponObject = closestWeapon;
                m_primaryWeapon = newWeapon;
                m_primaryWeaponObject.transform.parent = this.transform;
                m_primaryWeaponObject.GetComponent<SpriteRenderer>().enabled = false;
                m_primaryWeapon.m_held = true;


            }
        }

        UpdateWeaponUI();
        
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

    public void Update()
    {
        //Debug:
        Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorPos.y = 0;
        Debug.DrawLine(transform.position, cursorPos, Color.red);

        transform.forward = (cursorPos - transform.position).normalized;

        if (!dash)
        {
            Vector3 dir = Vector3.zero;
            if (Input.GetKey(KeyCode.W))
            {
                dir += new Vector3(0, 0, 1);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                dir += new Vector3(0, 0, -1);
            }
            if (Input.GetKey(KeyCode.D))
            {
                dir += new Vector3(1, 0, 0);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                dir += new Vector3(-1, 0, 0);
            }
            //Movement:
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (dir != Vector3.zero)
                {
                    m_rigidbody.velocity = dir.normalized * 100.0f;
                    dash = true;
                    dashTime = 0.4f;
                }
            }
            else
            {
                m_rigidbody.velocity = dir.normalized * 18.0f;
            }
        }
        else
        {
            dashTime -= Time.deltaTime;
            if(dashTime <= 0.0f)
            {
                dash = false;
            }
        }


        m_weaponPickupText.enabled = false;
        //shooting
        if (Input.GetMouseButton(0))
        {
            m_primaryWeapon.Fire(m_weaponRenderer.transform.position, this.transform.forward);
            UpdatePrimaryWeaponAmmo();
            if(m_primaryWeapon.m_ammo == 0 && m_secondaryWeapon == null)
            {
                Destroy(m_primaryWeaponObject);
                m_primaryWeaponObject = (GameObject)Instantiate(m_pistolPrefab);
                m_primaryWeapon = m_primaryWeaponObject.GetComponent<Weapon>();
                m_primaryWeapon.m_held = true;
                m_primaryWeapon.m_ammo = m_primaryWeapon.MAX_AMMO;
            }
        }

        if (Input.GetKeyDown(KeyCode.Q) && m_secondaryWeapon != null)
        {

            SwapWeapon();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            GrabWeapon();
        }
    }
}
