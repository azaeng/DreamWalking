using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SlotTag { None, Head, Chest, Legs, Feet }

// Scriptable Objects/Item�̶�� �ӽ� �޴� �׸�
[CreateAssetMenu(menuName = "Scriptable Objects/Item")]


public class Item : ScriptableObject
{
    // �������� �̹����� ��Ÿ���� Sprite ����
    public Sprite sprite;
    // �������� � ������ ���Կ� ������ �� �ִ����� ��Ÿ���� SlotTag ����
    public SlotTag itemTag;

    // �ν����Ϳ��� "If the item can be equipped"��� ����� ǥ��
    [Header("If the item can be equipped")]
    // �������� ���� ������ ���, ���� ���� ���忡 ��Ÿ�� ��� �������� �����ϴ� ����.
    public GameObject equipmentPrefab;
}