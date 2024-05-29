using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public int score = 0;
    public TMPro.TextMeshProUGUI scoreText;
    public TextMeshProUGUI finalScoreText;
    public Button AddHealthButton;
    //public PlayerMovement_Kdx playerHealth;
    //private bool healthAdded = false;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        if (AddHealthButton != null)
            AddHealthButton.gameObject.SetActive(false);  
    }
    //void Start()
    //{
    //    if (AddHealthButton != null)
    //    {
    //        AddHealthButton.onClick.AddListener(IncreaseMaxHealth);
    //        Debug.Log("Listener added to AddHealthButton");
    //    }
    //    else
    //    {
    //        Debug.Log("AddHealthButton is null");
    //    }

    //    if (playerHealth == null)
    //    {
    //        playerHealth = FindObjectOfType<PlayerMovement_Kdx>();
    //        Debug.Log($"Player health component found: {playerHealth != null}");
    //    }
    //}

    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScoreText();
    }

    public void UpdateScoreText()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;

        if (finalScoreText != null)
            finalScoreText.text = "Final Score: " + score;

        //if (score >= 500 && !healthAdded)
        //{
        //    if (AddHealthButton != null && !AddHealthButton.gameObject.activeSelf)
        //        AddHealthButton.gameObject.SetActive(true);  
        //}
    }
    //public void IncreaseMaxHealth()
    //{
    //    Debug.Log("IncreaseMaxHealth called");
    //    if (playerHealth != null && playerHealth.maxHealth < 150)
    //    {
    //        playerHealth.maxHealth += 50;
    //        playerHealth.currentHealth = playerHealth.maxHealth;
    //        healthAdded = true;
    //        AddHealthButton.gameObject.SetActive(false);
    //        Debug.Log("Health increased and button should now be inactive");
    //    }
    //    else
    //    {
    //        Debug.Log($"Button not working - Player Health null: {playerHealth == null}, Max Health: {playerHealth?.maxHealth}");
    //    }
    //}
}
