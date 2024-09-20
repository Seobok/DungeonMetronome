using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance = null;
    [SerializeField] private GameObject hitEffect_prefab;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public GameObject HitEffect(Vector3 position)
    {
        return Instantiate(hitEffect_prefab, position, Quaternion.identity);
    }
}
