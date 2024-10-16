using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class QTEManager : MonoBehaviour
{
    public static QTEManager instance;

    [Header("QTE Panel")]
    [SerializeField] private DwindlingCircle DwindlingCircle;
    [SerializeField] private CenterTap CenterTap;


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
        StartCoroutine(DwindlingCircle.Activate(timing, duration, causer, damagableList));
    }

    public void ActiveCenterTap(float barSpeed, Player causer, List<IDamagable> damagableList)
    {
        CenterTap.gameObject.SetActive(true);
        StartCoroutine(CenterTap.Activate(barSpeed, causer, damagableList));
    }
}
