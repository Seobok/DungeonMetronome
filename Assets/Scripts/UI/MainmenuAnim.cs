using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class MainmenuAnim : MonoBehaviour
{
    [SerializeField] private Text title;
    [SerializeField] private Button[] buttons;
    [SerializeField] private GameObject MenuPanel;
    [SerializeField] private GameObject OptionPanel;

    private string gameTitle = "Dungeon Metronome";
    private const float TYPING_DURATION = 3f;
    private const float MOVE_DURATION = 1f;

    private void Awake()
    {
        //메인 메뉴 버튼 4개
        foreach(var button in buttons)
        {
            button.gameObject.SetActive(false);
            button.onClick.AddListener(PlayCllickSound);
        }
    }

    private void Start()
    {
        //title.DOText(gameTitle, TYPING_DURATION).OnComplete(() => StartCoroutine(ActiveButtons()));
        StartCoroutine(TypingText());

        //메인 버튼 4개를 제외한 버튼들
        var allButtons = GetComponentsInChildren<Button>();
        foreach(var button in allButtons)
        {
            button.onClick.AddListener(PlayCllickSound);
        }
    }

    IEnumerator TypingText()
    {
        yield return new WaitForSeconds(1f);

        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("");
        title.text = stringBuilder.ToString();

        int idx = 0;

        while(idx != gameTitle.Length)
        {
            stringBuilder.Append(gameTitle[idx]);
            title.text = stringBuilder.ToString();
            SoundManager.instance.PlaySFX("TypingSound");
            idx++;
            yield return new WaitForSeconds(TYPING_DURATION / gameTitle.Length);
        }

        StartCoroutine(ActiveButtons());
    }

    IEnumerator ActiveButtons()
    {
        foreach(var button in buttons)
        {
            button.gameObject.SetActive(true);
            SoundManager.instance.PlaySFX("ButtonGenerate");
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void StartNewGame()
    {
        SoundManager.instance.PlayBGM("Lobby", 0.5f);
        LoadingAnim.LoadScene("Dungeon");
    }

    public void MoveOptionPanel()
    {
        if(Camera.main != null)
        {
            Camera.main.transform.DOMoveX(OptionPanel.transform.position.x, MOVE_DURATION);
        }
    }

    public void MoveMenuPanel()
    {
        if (Camera.main != null)
        {
            Camera.main.transform.DOMoveX(MenuPanel.transform.position.x, MOVE_DURATION);
        }
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }

    public void PlayCllickSound()
    {
        if(SoundManager.instance != null)
        {
            SoundManager.instance.PlaySFX("ClickButton");
        }
    }
}
