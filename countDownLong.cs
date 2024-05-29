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
        // ��ָ����ʱ��㼤�����
        if (countDownTime == 59 || countDownTime == 50 || countDownTime == 40 || countDownTime == 30 || countDownTime == 20)
        {
            foreach (GameObject monster in monsters)
            {
                if (!monster.activeInHierarchy) // �������Ƿ��Ѿ�����
                {
                    StartCoroutine(FadeInMonster(monster));
                    break; // ÿ��ֻ����һ������
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
            float duration = 1f;  // ���Գ���ʱ��
            float elapsed = 0;  // �ѹ�ʱ��
            while (elapsed < duration)
            {
                float alpha = elapsed / duration;  // ���㵱ǰ͸����
                renderer.color = new Color(1f, 1f, 1f, alpha);  // ������ɫ��͸����
                elapsed += Time.deltaTime;  // �����ѹ�ʱ��
                yield return null;  // �ȴ���һ֡
            }
            renderer.color = new Color(1f, 1f, 1f, 1f);  // ȷ����ȫ��͸��
        }
    }
}
