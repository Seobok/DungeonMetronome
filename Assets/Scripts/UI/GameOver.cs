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
    }

    public void OnAction(InputValue inputValue)
    {
        Debug.Log("메인화면으로 이동");
        LoadingAnim.LoadScene("Mainmenu");
    }
}
