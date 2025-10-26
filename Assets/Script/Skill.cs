using UnityEngine;
using System.Collections;

public class Skill : MonoBehaviour
{
    private PlayerController playerController;
    private SkillBar skillBar;
    private bool canUse = true;
    public float skillCooldown = 120f;
    private float currentCooldown = 0f;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        skillBar = playerController.skillBar.GetComponent<SkillBar>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && canUse)
        {
            StartCoroutine(UseSkill());
        }
        if (!canUse)
        {
            currentCooldown += Time.smoothDeltaTime;
            skillBar.UpdateBar(currentCooldown / skillCooldown);
        }
    }

    private IEnumerator UseSkill()
    {
        canUse = false;
        playerController.FireSkill();

        yield return new WaitForSeconds(skillCooldown);
        canUse = true;
        currentCooldown = 0f;
    }
}
