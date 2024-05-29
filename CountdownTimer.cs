using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class CountdownTimer : MonoBehaviour
{
    public int countDownTime;
    public TextMeshProUGUI countDownDisplay;
    public UnityEvent onCountdownFinished;

    private void Start()
    {
        StartCoroutine(CountdownToStart());
    }

    IEnumerator CountdownToStart()
    {
        AudioManager.Instance.PlaySFX("CountDown");
        while (countDownTime > 0)
        {
            countDownDisplay.text = countDownTime.ToString();
            yield return new WaitForSeconds(1f);
            countDownTime--;
        }
        countDownDisplay.text = "Start";
        yield return new WaitForSeconds(1f);
        countDownDisplay.gameObject.SetActive(false);
        onCountdownFinished.Invoke(); // ´¥·¢ÊÂ¼þ
    }
}
