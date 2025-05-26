using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아이템 데이터(DB)를 담는 스크립터블 오브젝트
[CreateAssetMenu(fileName = "ItemList", menuName = "Item/ItemList")]
public class ItemList : ScriptableObject
{
    [SerializeField] private List<Item> list; // 아이템 리스트

    public List<Item> List { get => list; set => list = value; }
}
