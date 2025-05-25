using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 소모성 아이템 스크립터블 오브젝트
[CreateAssetMenu(fileName = "Consumable", menuName = "Item/ConsumableItem")]
public class ConsumableItem : Item
{
    // 소모성 아이템 타입
    [SerializeField] private EnumTypes.CB_TYPE cbType;

    // 아이템 수치
    [SerializeField] private int upValue;

    // HP포션 프리팹
    [SerializeField] private GameObject[] hpPosion;

    protected EnumTypes.CB_TYPE CbType { get => cbType; set => cbType = value; }
    protected int UpValue { get => upValue; set => upValue = value; }
    public GameObject[] HpPosion { get => hpPosion; set => hpPosion = value; }

    // 초급hp 아이템 소모 처리 기능 메서드
    public virtual void SConsume()
    {
        Debug.Log($"{itemName}을 사용했습니다.");
    }

    // 중간 hp 포션 소모 처리 기능 메서드
    public virtual void MConsume()
    {
        Debug.Log($"{itemName}을 사용했습니다.");
    }

    // 대량의 hp 포션 소모 처리 기능 메서드
    public virtual void LConsume()
    {
        Debug.Log($"{itemName}을 사용했습니다.");
    }
}
