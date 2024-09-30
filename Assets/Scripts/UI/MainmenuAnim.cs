using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainmenuAnim : MonoBehaviour
{
    [SerializeField] private Text title;
    [SerializeField] private Button[] buttons;
    [SerializeField] private GameObject MenuPanel;
    [SerializeField] private GameObject OptionPanel;

    private const float TYPING_DURATION = 5f;
    private const float MOVE_DURATION = 1f;

    private void Awake()
    {
        foreach(var button in buttons)
        {
            button.gameObject.SetActive(false);
            button.onClick.AddListener(PlayCllickSound);
        }
    }

    private void Start()
    {
        title.DOText("Dungeon Metronome", TYPING_DURATION).OnComplete(() => StartCoroutine(ActiveButtons()));

        var allButtons = GetComponentsInChildren<Button>();
        foreach(var button in allButtons)
        {
            button.onClick.AddListener(PlayCllickSound);
        }
    }

    IEnumerator ActiveButtons()
    {
        foreach(var button in buttons)
        {
            button.gameObject.SetActive(true);
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
