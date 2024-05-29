using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class countDownLong : MonoBehaviour
{
    public int countDownTime;
    public TextMeshProUGUI countDownDisplay;
    public CountdownTimer countdownTimer;
    public GameObject scoreBoard;
    public GameObject[] monsters;
    void Start()
    {
        if (countdownTimer != null)
        {
            countdownTimer.onCountdownFinished.AddListener(StartLongCountdown);
        }
    }
    void StartLongCountdown()
    {
        StartCoroutine(CountdownToStart());
    }

    IEnumerator CountdownToStart()
    {
        while (countDownTime > 0)
        {
            countDownDisplay.text = countDownTime.ToString();
            ActivateMonsters();
            yield return new WaitForSeconds(1f);
            countDownTime--;
        }
        EndCountdown();
    }
    
    void EndCountdown()
    {
        countDownDisplay.gameObject.SetActive(false);
        ShowScoreBoard();  
    }

    private void OnDestroy()
    {
        if (countdownTimer != null)
        {
            countdownTimer.onCountdownFinished.RemoveListener(StartLongCountdown);
        }
    }
    void ShowScoreBoard()
    {
        if (scoreBoard != null)
        {
            AudioManager.Instance.PauseMusic();
            AudioManager.Instance.StopRunSound();
            AudioManager.Instance.StopBossEnragedRunSound();
            AudioManager.Instance.StopBossRunSound();
            scoreBoard.SetActive(true); 
        }
        Time.timeScale = 0;
        AudioManager.Instance.PlaySFX("ScoreBoard");
    }
    void ActivateMonsters()
    {
        // 在指定的时间点激活怪物
        if (countDownTime == 59 || countDownTime == 50 || countDownTime == 40 || countDownTime == 30 || countDownTime == 20)
        {
            foreach (GameObject monster in monsters)
            {
                if (!monster.activeInHierarchy) // 检查怪物是否已经激活
                {
                    StartCoroutine(FadeInMonster(monster));
                    break; // 每次只激活一个怪物
                }
            }
        }
    }
    IEnumerator FadeInMonster(GameObject monster)
    {
        monster.SetActive(true);
        SpriteRenderer renderer = monster.GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            float duration = 1f;  // 渐显持续时间
            float elapsed = 0;  // 已过时间
            while (elapsed < duration)
            {
                float alpha = elapsed / duration;  // 计算当前透明度
                renderer.color = new Color(1f, 1f, 1f, alpha);  // 设置颜色和透明度
                elapsed += Time.deltaTime;  // 增加已过时间
                yield return null;  // 等待下一帧
            }
            renderer.color = new Color(1f, 1f, 1f, 1f);  // 确保完全不透明
        }
    }
}
