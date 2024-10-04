using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Help : MonoBehaviour
{
    [Serializable]
    public class ButtonPanelPair
    {
        public Button button;
        public GameObject Panel;
    }

    [SerializeField] List<ButtonPanelPair> buttonPanelPairs;

    private void Start()
    {
        foreach (var pair in buttonPanelPairs)
        {
            pair.button.onClick.AddListener((() =>
            {
                foreach (var pair in buttonPanelPairs)
                {
                    if (pair.button == this) continue;

                    pair.Panel.SetActive(false);
                }
            }));

            pair.button.onClick.AddListener(() => 
            {
                if (pair.Panel.activeSelf) { pair.Panel.SetActive(false); } 
                else { pair.Panel.SetActive(true); } 
            });
        }
    }
}
