using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    public static BossHealthBar Instance;
    public Image fill;

    void Awake() => Instance = this;

    public void UpdateBar(float percent)
    {
        fill.fillAmount = Mathf.Clamp01(percent);
    }
}
