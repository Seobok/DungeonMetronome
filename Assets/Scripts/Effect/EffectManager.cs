using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance = null;

    private Dictionary<string, GameObject> particleDict;
    public GameObject[] particlePerfabs;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);


        particleDict = new Dictionary<string, GameObject>();
        foreach (var pref in particlePerfabs)
        {
            particleDict.Add(pref.name, pref);
        }
    }

    public void PlayParticle(string particleName, Vector2 position, int layerOrder = 0)
    {
        if (!particleDict.ContainsKey(particleName))
        {
            Debug.LogError($"파티클을 찾을 수 없습니다. : {particleName}");
            return;
        }

        GameObject particle = Instantiate(particleDict[particleName], position, Quaternion.identity);

        particle.name = particleName + "_Particle";
        SpriteRenderer spriteRenderer = particle.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = layerOrder;
        }

        Animator animator = particle.GetComponent<Animator>();
        if (animator != null)
        {
            animator.Play("ParticleAnimation");
            StartCoroutine(DestroyParticleAnimation(particle, animator));
        }
        else
        {
            Destroy(particle);
        }
    }

    IEnumerator DestroyParticleAnimation(GameObject particle, Animator animator)
    {
        float animationLength = animator.GetCurrentAnimatorStateInfo(0).length;

        yield return new WaitForSeconds(animationLength);

        Destroy(particle);
    }
}
