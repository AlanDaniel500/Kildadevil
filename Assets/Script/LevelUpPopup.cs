using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LevelUpPopup : MonoBehaviour
{
    public Button option1, option2, option3;

    public void Show(PlayerController player)
    {
        gameObject.SetActive(true);
        
        option1.onClick.RemoveAllListeners();
        option2.onClick.RemoveAllListeners();
        option3.onClick.RemoveAllListeners();

        option1.GetComponentInChildren<Text>().text = "+1 Projectile";
        option2.GetComponentInChildren<Text>().text = "+20% Attack Speed";
        option3.GetComponentInChildren<Text>().text = "+10% Damage";

        option1.onClick.AddListener(() => { player.ApplyTemporaryUpgrade_TurnToProjectiles(1); gameObject.SetActive(false); });
        option2.onClick.AddListener(() => { player.ApplyTemporaryUpgrade_FireRateMultiplier(0.8f); gameObject.SetActive(false); });
        option3.onClick.AddListener(() => { player.ApplyTemporaryUpgrade_DamageMultiplier(1.1f); gameObject.SetActive(false); });
    }

}
