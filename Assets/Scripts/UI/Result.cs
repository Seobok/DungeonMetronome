using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    [SerializeField] private Text timeText;
    [SerializeField] private Text moveText;
    [SerializeField] private Text scoreText;

    private void OnEnable()
    {
        timeText.text = GameManager.instance.playTimer.ToString("n2");

        moveText.text = GameManager.instance.moveCnt.ToString();

        scoreText.text = GameManager.instance.score.ToString();
    }

    public void OnAction(InputValue inputValue)
    {
        StartCoroutine(GameManager.instance.MoveNextStage());
    }
}
