using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameButton : MonoBehaviour
{
    public Button startButton;
    public Button trainMode;
    public Button instructionButton;
    public Button exitButton;
    public GameObject instructionMenu;
    public float blinkTime = 0.1f;  
    public int numberOfBlinks = 5;
    public Animator transition;
    public float transitionTime = 1f;
    private List<EventTrigger> triggers = new List<EventTrigger>();
    
    public void Start()
    {
        Button[] buttons = { startButton, trainMode, instructionButton, exitButton };
        foreach (Button button in buttons)
        {
            if (button != null)
            {
                button.onClick.AddListener(() => StartCoroutine(BlinkAndLoadScene(button)));
                EventTrigger trigger = button.gameObject.AddComponent<EventTrigger>();
                var entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerEnter;
                entry.callback.AddListener((data) => { OnHover(); });
                trigger.triggers.Add(entry);
                triggers.Add(trigger);
            }
        }
    }

    IEnumerator BlinkAndLoadScene(Button button)
    {
        AudioManager.Instance.PlaySFX("ButtonClick");
        DisableTriggers();
        button.interactable = false;
        for (int i = 0; i < numberOfBlinks; i++)
        {
            button.gameObject.SetActive(false);
            yield return new WaitForSeconds(blinkTime);
            button.gameObject.SetActive(true);
            yield return new WaitForSeconds(blinkTime);
        }
        if (button == startButton)
        {
            AudioManager.Instance.ChangeMusic("BGM-firststage");
            LoadNextLevel();
        }
        else if (button == trainMode)
        {
            AudioManager.Instance.ChangeMusic("TrainMode");
            LoadTrainingMode();
        }
        else if (button == instructionButton)
        {
            instructionMenu.SetActive(true);
        }
        else if (button == exitButton)
        {
            Application.Quit();
        }
        button.interactable = true;
    }

    public void OnHover()
    {
        AudioManager.Instance.PlaySFX("PopClickUI");  
    }

    private void DisableTriggers()
    {
        foreach (EventTrigger trigger in triggers)
        {
            trigger.enabled = false; 
        }
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }
    public void LoadTrainingMode()
    {
        StartCoroutine(LoadLevel(3)); 
    }
}
