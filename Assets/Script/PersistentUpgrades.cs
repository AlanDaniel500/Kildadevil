using UnityEngine;

[System.Serializable]
public class PermanentStats
{
    public float speed = 1f;
    public float damage = 1f;
    public float fireRateMultiplier = 1f;
    public float maxHealthMultiplier = 1f;
}


public class PersistentUpgrades : MonoBehaviour
{
    public static PersistentUpgrades Instance;

    public PermanentStats stats = new PermanentStats();

    // Campos para el sistema de upgrades permanentes
    public int currency = 0;
    public int damageLevel = 0;
    public int healthLevel = 0;
    public int speedLevel = 0;
    public int fireRateLevel = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }
        DontDestroyOnLoad(gameObject);
        Load();
    }

    public void Save()
    {
        PlayerPrefs.SetFloat("perm_speed", stats.speed);
        PlayerPrefs.SetFloat("perm_damage", stats.damage);
        PlayerPrefs.SetFloat("perm_fireRate", stats.fireRateMultiplier);
        PlayerPrefs.SetFloat("perm_maxHealth", stats.maxHealthMultiplier);

        PlayerPrefs.SetInt("currency", currency);
        PlayerPrefs.SetInt("damageLevel", damageLevel);
        PlayerPrefs.SetInt("healthLevel", healthLevel);
        PlayerPrefs.SetInt("speedLevel", speedLevel);
        PlayerPrefs.SetInt("fireRateLevel", fireRateLevel);

        PlayerPrefs.Save();
    }

    public void Load()
    {
        stats.speed = PlayerPrefs.GetFloat("perm_speed", 1f);
        stats.damage = PlayerPrefs.GetFloat("perm_damage", 1f);
        stats.fireRateMultiplier = PlayerPrefs.GetFloat("perm_fireRate", 1f);
        stats.maxHealthMultiplier = PlayerPrefs.GetFloat("perm_maxHealth", 1f);

        currency = PlayerPrefs.GetInt("currency", 0);
        damageLevel = PlayerPrefs.GetInt("damageLevel", 0);
        healthLevel = PlayerPrefs.GetInt("healthLevel", 0);
        speedLevel = PlayerPrefs.GetInt("speedLevel", 0);
        fireRateLevel = PlayerPrefs.GetInt("fireRateLevel", 0);
    }
}
