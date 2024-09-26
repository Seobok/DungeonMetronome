using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
}
