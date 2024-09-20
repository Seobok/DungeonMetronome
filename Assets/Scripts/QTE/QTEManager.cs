using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTEManager : MonoBehaviour
{
    public static QTEManager instance;
    [SerializeField] private DwindlingCircle DwindlingCircle;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public void ActiveDwindlingCircle(List<float> timing, float duration, Player causer, List<IDamagable> damagableList)
    {
        DwindlingCircle.gameObject.SetActive(true);
        Debug.Log("InActiveFunc " + DwindlingCircle.gameObject.activeSelf);
        StartCoroutine(DwindlingCircle.Activate(timing, duration, causer, damagableList));
    }
}
