using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Goal : Unit
{
    private string info;
    [SerializeField] Text infoText;

    public void SetInfo(string info)
    {
        this.info = info;
        infoText.text = info;
    }
}
