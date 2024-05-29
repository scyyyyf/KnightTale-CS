using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class WeaponManager : MonoBehaviour
{
    public int attackDamage = 15;
    public int attackLevel = 0;
    public float attackRate = 2f;
    public int attackRateLevel = 0;
    public float bulletLifetime = 0.15f;
    public int attackRangeLevel = 0;

    private void Start()
    {
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            var saveSystem = FindObjectOfType<SaveSystem>();

            // 在第一关时重置为默认值
            if (currentSceneIndex == 1 || currentSceneIndex == 3)
            {
                ResetToDefaults();
            }
            else
            {
                // 在其他关卡加载保存的状态
                if (saveSystem != null && saveSystem.LoadedData != null)
                {
                    attackDamage = saveSystem.LoadedData.attackDamage;
                    attackLevel = saveSystem.LoadedData.attackLevel;
                    attackRate = saveSystem.LoadedData.attackRate;
                    attackRateLevel = saveSystem.LoadedData.attackRateLevel;
                    bulletLifetime = saveSystem.LoadedData.bulletLifetime;
                    attackRangeLevel = saveSystem.LoadedData.attackRangeLevel;
                    UpdateAllSkillBars();
                }
                else
                {
                    // 如果没有保存的数据，也重置为默认值
                    ResetToDefaults();
                }
            }
        }
    }
    public void ResetToDefaults()
    {
        attackDamage = 15;
        attackLevel = 0;
        attackRate = 2f;
        attackRateLevel = 0;
        bulletLifetime = 0.15f;
        attackRangeLevel = 0;
    }
    public void IncreaseAttackDamage(int amount)
    {
        if (attackLevel < 5) 
        {
            attackDamage += amount;
            attackLevel++;
            AudioManager.Instance.PlaySFX("GainPotion");
            UpdateSkillBar(SkillType.AttackPower, attackLevel);
        }
    }
    public int GetAttackLevel()
    {
        return attackLevel; 
    }

    public void IncreaseAttackRate(float amount)
    {
        if (attackRateLevel < 5) 
        {
            attackRate += amount;
            attackRateLevel++;
            AudioManager.Instance.PlaySFX("GainPotion");
            UpdateSkillBar(SkillType.AttackSpeed, attackRateLevel);
        }
    }
    public int GetAttackSpeedLevel()
    {
        return attackRateLevel;
    }

    public void IncreaseBulletLifetime(float amount)
    {
        if (attackRangeLevel < 5) 
        {
            bulletLifetime += amount;
            attackRangeLevel++;
            AudioManager.Instance.PlaySFX("GainPotion");
            UpdateSkillBar(SkillType.AttackRange, attackRangeLevel);
        }
    }
    public int GetAttackRangeLevel()
    {
        return attackRangeLevel;
    }
    public void DecreaseAttackDamageLevel()
    {
        if (attackLevel > 0)
        {
            attackLevel--;
            attackDamage -= 5;
            AudioManager.Instance.PlaySFX("Debuff");
        }
    }

    public void DecreaseAttackRateLevel()
    {
        if (attackRateLevel > 0)
        {
            attackRateLevel--;
            attackRate -= 0.5f;
            AudioManager.Instance.PlaySFX("Debuff");
        }
    }

    public void DecreaseBulletLifetimeLevel()
    {
        if (attackRangeLevel > 0)
        {
            attackRangeLevel--;
            bulletLifetime -= 0.05f;
            AudioManager.Instance.PlaySFX("Debuff");
        }
    }
    private void UpdateSkillBar(SkillType skillType, int level)
    {
        SkillBar[] skillBars = FindObjectsOfType<SkillBar>();
        foreach (var skillBar in skillBars)
        {
            skillBar.UpdateSkillLevel(skillType, level);
        }
    }
    private void UpdateAllSkillBars()
    {
        UpdateSkillBar(SkillType.AttackPower, attackLevel);
        UpdateSkillBar(SkillType.AttackSpeed, attackRateLevel);
        UpdateSkillBar(SkillType.AttackRange, attackRangeLevel);
    }
}
