using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public SaveSystem saveSystem;

    private void Awake()
    {
        SceneManager.sceneLoaded += Initialize;
        DontDestroyOnLoad(gameObject);
    }

    private void Initialize(Scene scene, LoadSceneMode sceneMode)
    {
        Debug.Log("Loaded GM");
        var playerInput = FindObjectOfType<PlayerMovement_Kdx>();
        var playerAttack = FindObjectOfType<PlayerAttack>();
        if (playerInput != null)
            player = playerInput.gameObject;

        saveSystem = FindObjectOfType<SaveSystem>();
        if (player != null && saveSystem != null)
        {
            var damageable = player.GetComponentInChildren<PlayerMovement_Kdx>();
            if (damageable != null)
            {
                if (SceneManager.GetActiveScene().buildIndex == 1)
                {
                    damageable.currentHealth = damageable.maxHealth;
                    playerAttack.isChargeAttackUnlocked = false;
                }
                else if (saveSystem.LoadedData != null)
                {
                    damageable.currentHealth = saveSystem.LoadedData.playerHealth;
                    playerAttack.isChargeAttackUnlocked = saveSystem.LoadedData.isChargeAttackUnlocked;
                }
                damageable.healthBar.Sethealth(damageable.currentHealth);
            }

            if (saveSystem.LoadedData != null)
            {
                var weaponManager = player.GetComponent<WeaponManager>();
                if (weaponManager != null)
                {
                    weaponManager.attackDamage = saveSystem.LoadedData.attackDamage;
                    weaponManager.attackLevel = saveSystem.LoadedData.attackLevel;
                    weaponManager.attackRate = saveSystem.LoadedData.attackRate;
                    weaponManager.attackRateLevel = saveSystem.LoadedData.attackRateLevel;
                    weaponManager.bulletLifetime = saveSystem.LoadedData.bulletLifetime;
                    weaponManager.attackRangeLevel = saveSystem.LoadedData.attackRangeLevel;
                }
            }
        }
    }

    public void LoadLevel()
    {
        if (saveSystem.LoadedData != null)
        {
            SceneManager.LoadScene(saveSystem.LoadedData.sceneIndex);
            return;
        }
        LoadNextLevel();
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void SaveData()
    {
        if (player != null)
        {
            var weaponManager = player.GetComponent<WeaponManager>();
            var playerAttack = player.GetComponent<PlayerAttack>();  // 获取 PlayerAttack 组件
            if (weaponManager != null && playerAttack != null)
            {
                saveSystem.SaveData(
                    SceneManager.GetActiveScene().buildIndex,
                    player.GetComponent<PlayerMovement_Kdx>().currentHealth,
                    weaponManager,
                    playerAttack.isChargeAttackUnlocked  // 传递 isChargeAttackUnlocked 参数
                );
            }
        }
    }
}