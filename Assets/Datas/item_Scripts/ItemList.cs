using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ ���(DB) ��ũ���ͺ� ������Ʈ
[CreateAssetMenu(fileName = "ItemList", menuName = "Item/ItemList")]
public class ItemList : ScriptableObject
{
    [SerializeField] private List<Item> list; // ������ ����Ʈ

    public List<Item> List { get => list; set => list = value; }
}
