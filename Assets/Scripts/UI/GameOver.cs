using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] Text scoreText;
    [SerializeField] Text causerText;
    [HideInInspector] public string causerName;
    private void OnEnable()
    {
        if (GameManager.instance == null) return;
        scoreText.text = GameManager.instance.totalScore.ToString();
        causerText.text = causerName;
        SaveManager.instance.isSaved = false;
    }

    public void OnAction(InputValue inputValue)
    {
        SoundManager.instance.PlayBGM("Mainmenu");
        LoadingAnim.LoadScene("Mainmenu");
    }
}
