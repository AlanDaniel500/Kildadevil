using UnityEngine;
using UnityEngine.UI;

public class ExperienceBar : MonoBehaviour
{
    public static ExperienceBar Instance;
    public Image fill;

    void Awake() => Instance = this;

    public void UpdateBar(float percent)
    {
        fill.fillAmount = Mathf.Clamp01(percent);
    }

}
