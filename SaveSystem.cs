using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SaveSystem : MonoBehaviour
{
    public string playerHealthKey = "PlayerHealth", sceneKey = "SceneIndex", savePresentKey = "SavePresent", chargeAttackUnlockedKey = "ChargeAttackUnlocked";
    
    public LoadedData LoadedData { get; private set; }

    public UnityEvent<bool> OnDataLoadedResult;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        var result = LoadData();
        OnDataLoadedResult?.Invoke(result);
    }
    public void ResetData()
    {
        PlayerPrefs.DeleteKey(playerHealthKey);
        PlayerPrefs.DeleteKey(sceneKey);
        PlayerPrefs.DeleteKey(savePresentKey);
        LoadedData = null;
    }

    public bool LoadData()
    {
        if (PlayerPrefs.GetInt(savePresentKey) == 1)
        {
            LoadedData = new LoadedData();
            LoadedData.playerHealth = PlayerPrefs.GetInt(playerHealthKey);
            LoadedData.sceneIndex = PlayerPrefs.GetInt(sceneKey);
            LoadedData.attackDamage = PlayerPrefs.GetInt("AttackDamage");
            LoadedData.attackLevel = PlayerPrefs.GetInt("AttackLevel");
            LoadedData.attackRate = PlayerPrefs.GetInt("AttackRate");
            LoadedData.attackRateLevel = PlayerPrefs.GetInt("AttackRateLevel");
            LoadedData.bulletLifetime = PlayerPrefs.GetInt("BulletLifetime");
            LoadedData.attackRangeLevel = PlayerPrefs.GetInt("AttackRangeLevel");
            LoadedData.isChargeAttackUnlocked = PlayerPrefs.GetInt(chargeAttackUnlockedKey) == 1;
            return true;
        }
        else
        {
            SetDefaultValues();
            return false;
        }
    }
    private void SetDefaultValues()
    {
        PlayerPrefs.SetInt(playerHealthKey, 100); // 假设满血是100
        PlayerPrefs.SetInt(sceneKey, 1); // 默认加载第一关
        PlayerPrefs.SetInt("AttackDamage", 15);
        PlayerPrefs.SetInt("AttackLevel", 0);
        PlayerPrefs.SetFloat("AttackRate", 2);
        PlayerPrefs.SetInt("attackRateLevel", 0);
        PlayerPrefs.SetFloat("bulletLifetime", 0.15f);
        PlayerPrefs.SetInt("attackRangeLevel", 0);
        PlayerPrefs.SetInt(savePresentKey, 1);
        PlayerPrefs.Save(); // 确保写入默认值

        // 加载这些默认值到LoadedData
        LoadedData = new LoadedData
        {
            playerHealth = 100,
            sceneIndex = 1,
            attackDamage = 15,
            attackLevel = 0,
            attackRate = 2f,
            attackRateLevel = 0,
            bulletLifetime = 0.15f,
            attackRangeLevel = 0
        };
    }


    public void SaveData(int sceneIndex, int playerHealth, WeaponManager weaponManager ,bool isChargeAttackUnlocked)
    {
        if (LoadedData == null)
            LoadedData = new LoadedData();

        LoadedData.playerHealth = playerHealth;
        LoadedData.sceneIndex = sceneIndex;
        LoadedData.attackDamage = weaponManager.attackDamage;
        LoadedData.attackLevel = weaponManager.attackLevel;
        LoadedData.attackRate = weaponManager.attackRate;
        LoadedData.attackRateLevel = weaponManager.attackRateLevel;
        LoadedData.bulletLifetime = weaponManager.bulletLifetime;
        LoadedData.attackRangeLevel = weaponManager.attackRangeLevel;

        PlayerPrefs.SetInt(playerHealthKey, playerHealth);
        PlayerPrefs.SetInt(sceneKey, sceneIndex);
        PlayerPrefs.SetInt("AttackDamage", weaponManager.attackDamage);
        PlayerPrefs.SetInt("AttackLevel", weaponManager.attackLevel);
        PlayerPrefs.SetFloat("AttackRate", weaponManager.attackRate);
        PlayerPrefs.SetInt("AttackRateLevel", weaponManager.attackRateLevel);
        PlayerPrefs.SetFloat("BulletLifetime", weaponManager.bulletLifetime);
        PlayerPrefs.SetInt("AttackRangeLevel", weaponManager.attackRangeLevel);
        PlayerPrefs.SetInt(chargeAttackUnlockedKey, isChargeAttackUnlocked ? 1 : 0);
        PlayerPrefs.SetInt(savePresentKey, 1);
    }

}

public class LoadedData
{
    public int playerHealth = -1;
    public int sceneIndex = -1;
    public int attackDamage = 15;
    public int attackLevel = 0;
    public float attackRate = 2;
    public int attackRateLevel = 0;
    public float bulletLifetime = 0.15f;
    public int attackRangeLevel = 0;
    public bool isChargeAttackUnlocked = false;
}