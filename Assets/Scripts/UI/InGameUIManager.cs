using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
    public static InGameUIManager Instance;

    [Header("Health")]
    [SerializeField] private List<Image> healths;
    [SerializeField] private List<Sprite> healthSprites; // 0 : FULL, 1 : HALF, 2 : EMPTY

    [Header("Inventory")]
    [SerializeField] private List<Image> inventorySlots;    // 0 : WEAPON
    [SerializeField] private List<Image> inventoryItems;    //Slots : Inventory Slot의 테두리 (BackGround), Items : 착용중인 아이템의 Sprite가 적용될 곳
    [SerializeField] private List<Sprite> emptySlotSprites; //EmptySlot : 장착중인 장비가 없을 때 부위를 확인할 수 있는 배경 Sprite
    [SerializeField] private Sprite noMarkSlot;             //NoMark : 착용중인 장비와 배경이 겹치는 상황을 막기 위한 빈 배경

    [Header("FadeInOut")]
    [SerializeField] private SpriteRenderer FadeInOutPanel;

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
                healths[i].enabled = true;
                remainMaxHP -= 2;
                if(remainCurrentHP >= 2)
                {
                    healths[i].sprite = healthSprites[0];
                    remainCurrentHP -= 2;
                }
                else if(remainCurrentHP == 1)
                {
                    healths[i].sprite = healthSprites[1];
                    remainCurrentHP -= 1;
                }
                else
                {
                    healths[i].sprite = healthSprites[2];
                }
            }
            else
            {
                healths[i].enabled = false;
            }
        }
    }

    public void SetInventorySlot(int slot, Sprite sprite)
    {
        if(sprite == null)
        {
            inventoryItems[slot].enabled = false;
            inventorySlots[slot].sprite = emptySlotSprites[slot];
        }
        else
        {
            inventoryItems[slot].enabled = true;
            inventoryItems[slot].sprite = sprite;
            inventorySlots[slot].sprite = noMarkSlot;
        }
    }

    public IEnumerator FadeImage(float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;

        Color panelColor = FadeInOutPanel.color;    //패널 색상 가져오기

        //페이드 효과 동안 Alpha 값을 점진적으로 변화
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);

            panelColor.a = newAlpha;
            FadeInOutPanel.color = panelColor;  //패널 색상 업데이트

            yield return null;  //프레임마다 갱신
        }

        //최종적으로 알파 값 설정
        panelColor.a = endAlpha;
        FadeInOutPanel.color = panelColor;
    }
}
