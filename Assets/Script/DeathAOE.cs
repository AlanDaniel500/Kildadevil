using UnityEngine;
using System.Collections;

public class DeathAOE : MonoBehaviour
{
    public void Setup(GameObject prefab, Vector2 size, float warningTime, float fadeOutTime, int damage)
    {
        StartCoroutine(PlayDeathAOE(prefab, size, warningTime, fadeOutTime, damage));
    }

    private IEnumerator PlayDeathAOE(GameObject prefab, Vector2 size, float warningTime, float fadeOutTime, int damage)
    {
        GameObject warning = Instantiate(prefab, transform.position, Quaternion.identity);
        SpriteRenderer sr = warning.GetComponent<SpriteRenderer>();
        BoxCollider2D col = warning.GetComponent<BoxCollider2D>();

        sr.color = new Color(1f, 0f, 0f, 0.3f);
        warning.transform.localScale = new Vector3(size.x, size.y, 1f);
        if (col != null) col.isTrigger = true;

        // Fade in
        float fadeInTime = 0.3f;
        for (float t = 0; t < fadeInTime; t += Time.deltaTime)
        {
            float alpha = Mathf.Lerp(0.1f, 0.5f, t / fadeInTime);
            sr.color = new Color(1f, 0f, 0f, alpha);
            yield return null;
        }

        yield return new WaitForSeconds(warningTime - fadeInTime);

        // Daño
        sr.color = new Color(1f, 0.3f, 0.3f, 0.8f);
        if (col != null)
        {
            Collider2D[] hits = Physics2D.OverlapBoxAll(warning.transform.position, size, 0f);
            foreach (Collider2D hit in hits)
            {
                if (hit.CompareTag("Player"))
                {
                    var player = hit.GetComponent<PlayerController>();
                    if (player != null) player.TakeDamage(damage);
                }
            }
        }

        // Fade out
        for (float t = 0; t < fadeOutTime; t += Time.deltaTime)
        {
            float alpha = Mathf.Lerp(0.8f, 0f, t / fadeOutTime);
            sr.color = new Color(1f, 0.3f, 0.3f, alpha);
            yield return null;
        }

        Destroy(warning);
        Destroy(gameObject); //  Se destruye solo cuando termina
    }
}
