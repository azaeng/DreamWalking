using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �Ҹ� ������ ��ũ���ͺ� ������Ʈ
[CreateAssetMenu(fileName = "Consumable", menuName = "Item/ConsumableItem")]
public class ConsumableItem : Item
{
    // �Ҹ� ������ Ÿ��
    [SerializeField] private EnumTypes.CB_TYPE cbType;

    // ������ ��ġ
    [SerializeField] private int upValue;

    // HP���� ������
    [SerializeField] private GameObject[] hpPosion;

    protected EnumTypes.CB_TYPE CbType { get => cbType; set => cbType = value; }
    protected int UpValue { get => upValue; set => upValue = value; }
    public GameObject[] HpPosion { get => hpPosion; set => hpPosion = value; }

    // �ʱ�hp ������ �Ҹ� ó�� ��� �޼���
    public virtual void SConsume()
    {
        Debug.Log($"{itemName}�� ����߽��ϴ�.");
    }

    // �߰� hp ���� �Ҹ� ó�� ��� �޼���
    public virtual void MConsume()
    {
        Debug.Log($"{itemName}�� ����߽��ϴ�.");
    }

    // �뷮�� hp ���� �Ҹ� ó�� ��� �޼���
    public virtual void LConsume()
    {
        Debug.Log($"{itemName}�� ����߽��ϴ�.");
    }
}
