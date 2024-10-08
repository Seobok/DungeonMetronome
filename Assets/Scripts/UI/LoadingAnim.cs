using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingAnim : MonoBehaviour
{
    public static string nextScene;
    [SerializeField] private Image progressBar;
    [SerializeField] private Transform LoadingText;
    private TweenerCore<Vector3, Vector3, VectorOptions> LoadingTweener;

    private void Start()
    {
        StartCoroutine(LoadScene());

        if(LoadingText != null)
        {
            LoadingTweener = LoadingText.DOMoveY(LoadingText.position.y + 0.5f, 0.5f).SetLoops(-1, LoopType.Yoyo);
        }
    }
    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("Loading");
    }

    /// <summary>
    /// 비동기로 Scene을 불러옴
    /// 불러오는 동안 진행도에 따라 progressBar를 채움
    /// 출처 : https://wergia.tistory.com/59
    /// </summary>
    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;
        float timer = 0.0f;
        while (!op.isDone)
        {
            yield return null;
            timer += Time.deltaTime;
            if (op.progress < 0.9f)
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, op.progress, timer);
                if (progressBar.fillAmount >= op.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);
                if (progressBar.fillAmount == 1.0f)
                {
                    LoadingTweener.Kill();

                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}
