using UnityEngine;
using UnityEngine.UI;

public class DashBar : MonoBehaviour
{
    public Image fill;
    public Image available;

    public void UpdateBar(float percent)
    {
        fill.fillAmount = Mathf.Clamp01(percent);
        if (fill.fillAmount < 1)
        {
            available.transform.localScale = new Vector3(0, 0, 0);
        }
        else
        {
            available.transform.localScale = new Vector3(1, 1, 0);
        }
    }
}
