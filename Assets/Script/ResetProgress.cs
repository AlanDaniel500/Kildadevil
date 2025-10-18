using UnityEngine;
using UnityEngine.UI;

public class ResetProgress : MonoBehaviour
{
    [Header("Opcional: botón para resetear")]
    public Button resetButton;

    private void Start()
    {
        if (resetButton != null)
        {
            resetButton.onClick.RemoveAllListeners();
            resetButton.onClick.AddListener(ResetAllUpgrades);
            resetButton.onClick.AddListener(UpdateUI);
        }
    }

    
    public void ResetAllUpgrades()
    {

        PlayerPrefs.SetFloat("perm_speed", 1f);
        PlayerPrefs.SetFloat("perm_damage", 1f);
        PlayerPrefs.SetFloat("perm_fireRate", 1f);
        PlayerPrefs.SetFloat("perm_maxHealth", 1f);

        PlayerPrefs.SetInt("currency", 0);
        PlayerPrefs.SetInt("damageLevel", 0);
        PlayerPrefs.SetInt("healthLevel", 0);
        PlayerPrefs.SetInt("speedLevel", 0);
        PlayerPrefs.SetInt("fireRateLevel", 0);


        PlayerPrefs.Save();

        Debug.Log("PlayerPrefs reseteados: monedas y upgrades permanentes a 0.");

        
        PersistentUpgrades.Instance.Load();
    }

    public void UpdateUI()
    {
        var ui = FindFirstObjectByType<PermanentUpgradesMenu>();
        if (ui != null) 
        {
            ui.UpdateUI();
        }
    }
}
