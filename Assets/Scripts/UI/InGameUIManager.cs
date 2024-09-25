using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
    public static InGameUIManager Instance;

    [Header("Health")]
    [SerializeField] private List<Image> healthImgList;
    [SerializeField] private List<Sprite> healthSpriteList; // 0 : FULL, 1 : HALF, 2 : EMPTY

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    public void SetHealthBar(int maxHP, int currentHP)
    {
        int remainMaxHP = maxHP;
        int remainCurrentHP = currentHP;

        for(int i=0;i<6;i++)
        {
            if(remainMaxHP >= 1)
            {
                healthImgList[i].enabled = true;
                remainMaxHP -= 2;
                if(remainCurrentHP >= 2)
                {
                    healthImgList[i].sprite = healthSpriteList[0];
                    remainCurrentHP -= 2;
                }
                else if(remainCurrentHP == 1)
                {
                    healthImgList[i].sprite = healthSpriteList[1];
                    remainCurrentHP -= 1;
                }
                else
                {
                    healthImgList[i].sprite = healthSpriteList[2];
                }
            }
            else
            {
                healthImgList[i].enabled = false;
            }
        }
    }
}
