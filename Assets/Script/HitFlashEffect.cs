using UnityEngine;
using System.Collections;

public class HitFlashEffect : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Color flashColor;
    public float flashDuration = 0.1f;
    public int numberOfFlashes = 1;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TriggerHitFlash()
    {
        StartCoroutine(FlashCoroutine());
    }

    private IEnumerator FlashCoroutine()
    {
        Color originalColor = spriteRenderer.color;

        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRenderer.color = flashColor; 
            yield return new WaitForSeconds(flashDuration / 2);

            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(flashDuration / 2);
        }
    }
}
