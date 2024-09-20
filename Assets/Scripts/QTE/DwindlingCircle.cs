using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DwindlingCircle : MonoBehaviour
{
    [SerializeField] private Transform timerCircle;
    [SerializeField] private GameObject actionCircle;
    [SerializeField] private GameObject correctCircle;

    private List<GameObject> correctCircleList;
    private List<GameObject> actionCircleList;

    private int actionCount = 0;
    private int curCount = 0;
    private int correctCount = 0;

    private const float WAIT_TIME = 0.5f;
    private const float QUIT_TIME = 1f;
    private const float ERROR_RANGE = 0.05f;
    private float duration = 0;

    private void Awake()
    {
        correctCircleList = new List<GameObject>();
        actionCircleList = new List<GameObject>();
    }

    private void OnAction(InputValue inputValue)
    {
        Debug.Log("OnAction");
        if (curCount >= actionCount) return;

        if(actionCircle != null)
        {
            var go = Instantiate(actionCircle);
            go.transform.SetParent(transform);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = timerCircle.localScale;
            foreach(var correctCircle in correctCircleList)
            {
                if((correctCircle.transform.localScale.x - ERROR_RANGE <= go.transform.localScale.x)&&(go.transform.localScale.x <= correctCircle.transform.localScale.x + ERROR_RANGE))
                {
                    //CORRECT
                    go.GetComponent<Image>().color = Color.green;
                    Debug.Log("Correct");
                    correctCount++;
                }
            }

            actionCircleList.Add(go);

            curCount++;
        }
    }

    public IEnumerator Activate(List<float> timing, float duration, Player causer, List<IDamagable> damagableList)
    {
        Debug.Log("Activate");
        this.duration = duration;
        if(timerCircle != null)
        {
            timerCircle.localScale = new Vector3(2f, 2f, 1);
            correctCount = 0;
            curCount = 0;
            if (correctCircle != null)
            {
                foreach (float t in timing)
                {
                    var go = Instantiate(correctCircle);
                    go.transform.SetParent(transform);
                    go.transform.localScale = new Vector3(t, t, 0);
                    go.transform.localPosition = Vector3.zero;

                    correctCircleList.Add(go);
                }

                actionCount = timing.Count;
            }

            yield return new WaitForSeconds(WAIT_TIME);
            //시작 사운드 추가
            timerCircle.DOScale(0, duration).SetEase(Ease.Linear);
            StartCoroutine(Deactivate(causer, damagableList));
        }
    }

    public IEnumerator Deactivate(Player causer, List<IDamagable> damagableList)
    {
        yield return new WaitForSeconds(duration + QUIT_TIME);

        while(actionCircleList.Count!=0)
        {
            var go = actionCircleList[0];
            actionCircleList.Remove(go);
            Destroy(go);
        }

        while(correctCircleList.Count!=0)
        {
            var go = correctCircleList[0];
            correctCircleList.Remove(go);
            Destroy(go);
        }

        GameManager.instance.ResultQTE(actionCount == correctCount, causer, damagableList);

        gameObject.SetActive(false);
    }
}
