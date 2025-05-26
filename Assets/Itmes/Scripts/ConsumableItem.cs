using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 소비품 아이템 스크립트 오브젝트
[CreateAssetMenu(fileName = "Consumable", menuName = "Item/ConsumableItem")]
public class ConsumableItem : Items
{
    // 소비품 종류
    [SerializeField] private EnumTypes.CB_TYPE cbType;

    // 증가 값
    [SerializeField] private int upValue;

    // HP 포션 프리팹 배열
    [SerializeField] private GameObject[] hpPosion;

    protected EnumTypes.CB_TYPE CbType { get => cbType; set => cbType = value; }
    protected int UpValue { get => upValue; set => upValue = value; }
    public GameObject[] HpPosion { get => hpPosion; set => hpPosion = value; }

    // 소량 HP 회복 소비 함수
    public virtual void SConsume()
    {
        Debug.Log($"{itemName}을(를) 사용했습니다.");
    }

    // 중량 HP 회복 소비 함수
    public virtual void MConsume()
    {
        Debug.Log($"{itemName}을(를) 사용했습니다.");
    }

    // 대량 HP 회복 소비 함수
    public virtual void LConsume()
    {
        Debug.Log($"{itemName}을(를) 사용했습니다.");
    }
}
