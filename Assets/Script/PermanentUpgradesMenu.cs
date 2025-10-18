using UnityEngine;
using UnityEngine.UI;

public class PermanentUpgradesMenu : MonoBehaviour
{
    [Header("Botones de Mejora")]
    public Button upgradeDamageBtn;
    public Button upgradeHealthBtn;
    public Button upgradeSpeedBtn;
    public Button upgradeFireRateBtn;
    public Text currencyText;

    [Header("Costo Base y Escalado")]
    public int baseCost = 50;
    public float costMultiplier = 1.5f;

    private void Start()
    {
        UpdateUI();

        upgradeDamageBtn.onClick.AddListener(() => BuyUpgrade("damage"));
        upgradeHealthBtn.onClick.AddListener(() => BuyUpgrade("health"));
        upgradeSpeedBtn.onClick.AddListener(() => BuyUpgrade("speed"));
        upgradeFireRateBtn.onClick.AddListener(() => BuyUpgrade("firerate"));
    }

    public void UpdateUI()
    {
        currencyText.text = $"Coins: {PersistentUpgrades.Instance.currency}";
        upgradeDamageBtn.GetComponentInChildren<Text>().text =
            $"Damage Lv.{PersistentUpgrades.Instance.damageLevel} ({GetUpgradeCost("damage")} Coins)";
        upgradeHealthBtn.GetComponentInChildren<Text>().text =
            $"Health Lv.{PersistentUpgrades.Instance.healthLevel} ({GetUpgradeCost("health")} Coins)";
        upgradeSpeedBtn.GetComponentInChildren<Text>().text =
            $"Mov. Speed Lv.{PersistentUpgrades.Instance.speedLevel} ({GetUpgradeCost("speed")} Coins)";
        upgradeFireRateBtn.GetComponentInChildren<Text>().text =
            $"Attack Speed Lv.{PersistentUpgrades.Instance.fireRateLevel} ({GetUpgradeCost("firerate")} Coins)";
    }

    int GetUpgradeCost(string stat)
    {
        int level = 0;
        switch (stat)
        {
            case "damage": level = PersistentUpgrades.Instance.damageLevel; break;
            case "health": level = PersistentUpgrades.Instance.healthLevel; break;
            case "speed": level = PersistentUpgrades.Instance.speedLevel; break;
            case "firerate": level = PersistentUpgrades.Instance.fireRateLevel; break;
        }
        return Mathf.RoundToInt(baseCost * Mathf.Pow(costMultiplier, level));
    }

    void BuyUpgrade(string stat)
    {
        int cost = GetUpgradeCost(stat);
        if (PersistentUpgrades.Instance.currency < cost) return;

        PersistentUpgrades.Instance.currency -= cost;

        switch (stat)
        {
            case "damage": PersistentUpgrades.Instance.damageLevel++; break;
            case "health": PersistentUpgrades.Instance.healthLevel++; break;
            case "speed": PersistentUpgrades.Instance.speedLevel++; break;
            case "firerate": PersistentUpgrades.Instance.fireRateLevel++; break;
        }

        PersistentUpgrades.Instance.Save();
        UpdateUI();
    }

}
