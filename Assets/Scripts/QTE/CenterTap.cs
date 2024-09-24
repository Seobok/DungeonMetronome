using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CenterTap : MonoBehaviour
{
    [SerializeField] TimingBar timingBar;
    [SerializeField] Slider timer;
    public const float ERROR_RANGE = 20f;
    public const float BAR_OUT_RANGE = 235f;
    public const float MAX_TIME = 3f;
    public const float WAIT_TIME = 1f;

    private bool IsGameStart = false;

    private int correctCount = 0;

    private void Awake()
    {
        timer.maxValue = MAX_TIME;
    }

    private void Update()
    {
        if(timer != null && timingBar != null)
        {
            if(timingBar.isMove)
            {
                if(timer.value > 0)
                {
                    timer.value -= Time.deltaTime;
                }
            }
        }
    }

    private void OnAction(InputValue inputValue)
    {
        if (!IsGameStart) return;
        if (timingBar == null) return;

        timingBar.isMove = false;
        if(-ERROR_RANGE <= timingBar.transform.localPosition.x && timingBar.transform.localPosition.x <= ERROR_RANGE)
        {
            correctCount++;
            timingBar.ChangeColor(Color.green);
        }
    }

    public IEnumerator Activate(float barSpeed, Player causer, List<IDamagable> damagableList)
    {
        if (timingBar == null) yield break;

        IsGameStart = true;

        timingBar.isMove = false;
        timingBar.MoveLeftEnd();
        timingBar.ChangeColor(Color.red);

        timer.value = MAX_TIME;

        correctCount = 0;

        yield return new WaitForSeconds(WAIT_TIME);

        QTEManager.instance.PlaySFXSound(QTEManager.instance.preparePattern);
        timingBar.isMove = true;
        StartCoroutine(Deactivate(causer, damagableList));
    }

    public IEnumerator Deactivate(Player causer, List<IDamagable> damagableList)
    {
        yield return new WaitForSeconds(MAX_TIME);

        IsGameStart = false;

        GameManager.instance.ResultQTE(correctCount, causer, damagableList);

        gameObject.SetActive(false);
    }
}
