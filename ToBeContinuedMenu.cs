using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToBeContinuedMenu : MonoBehaviour
{
    public GameObject ToBeContinuedMenuUI;

    void Start()
    {
        ToBeContinuedMenuUI.SetActive(false);
    }

    public void ShowToBeContinuedScreen()
    {
        ToBeContinuedMenuUI.SetActive(true);
        AudioManager.Instance.StopRunSound();
        AudioManager.Instance.PauseMusic();
        AudioManager.Instance.PlaySFX("StageClear");
        Time.timeScale = 0f;
        PlayerMovement_Kdx playerController = FindObjectOfType<PlayerMovement_Kdx>();
        if (playerController != null)
        {
            playerController.enabled = false;
        }
    }

    public void LoadMenu()
    {
        AudioManager.Instance.StopRunSound();
        AudioManager.Instance.ResumeMusic();
        AudioManager.Instance.ChangeMusic("BGM");
        Time.timeScale = 1f;
        SceneManager.LoadScene("Start Screen");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
