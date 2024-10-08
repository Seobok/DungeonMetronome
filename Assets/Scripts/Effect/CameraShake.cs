using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake
{
    private static bool isShaking = false;
    public static IEnumerator Shake(float duration, float magnitude, Vector3 originPos)
    {
        if (isShaking) yield break;

        if (Camera.main == null)
        {
            Debug.Log("MainCamera가 없습니다.");
            yield break;
        }

        isShaking = true;

        var mainCamera = Camera.main;

        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            var newPos = new Vector3(originPos.x + x, originPos.y + y, mainCamera.transform.position.z);

            mainCamera.transform.localPosition = newPos;

            elapsed += Time.deltaTime;
            yield return null;
        }

        isShaking = false;
    }
}