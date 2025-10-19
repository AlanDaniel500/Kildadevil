using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image fill;

    public void UpdateBar(float percent)
    {
        fill.fillAmount = Mathf.Clamp01(percent);
    }
}
