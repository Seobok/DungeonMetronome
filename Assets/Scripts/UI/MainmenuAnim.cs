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
        }
    }

    private void Start()
    {
        title.DOText("Dungeon Metronome", TYPING_DURATION).OnComplete(() => StartCoroutine(ActiveButtons()));
    }

    IEnumerator ActiveButtons()
    {
        foreach(var button in buttons)
        {
            button.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }
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
}
