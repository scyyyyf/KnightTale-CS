using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDisPotion : MonoBehaviour
{
    public float DistanceBoost = 0.05f;
    public float floatSpeed = 4f;
    public float floatHeight = 0.2f;
    public SkillBar rangeUpSkillBar;
    private Vector3 startPosition;
    private void Start()
    {
        startPosition = transform.position;
    }
    private void Update()
    {
        FloatEffect();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            WeaponManager weaponManager = other.GetComponent<WeaponManager>();
            if (weaponManager != null)
            {
                weaponManager.IncreaseBulletLifetime(DistanceBoost);
                SkillBar[] skillBars = FindObjectsOfType<SkillBar>();
                foreach (var skillBar in skillBars)
                {
                    if (skillBar.skillType == SkillType.AttackRange)
                    {
                        skillBar.UpdateSkillLevel(SkillType.AttackRange, weaponManager.GetAttackRangeLevel());
                        break;
                    }
                }
                Destroy(gameObject);
            }
        }
    }

    private void FloatEffect()
    {
        Vector3 newPosition = startPosition;
        newPosition.y += Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = newPosition;
    }
}
