using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    [SerializeField] private HandType.HAND_TYPE handType;

    private void Start()
    {
        if (transform.parent.parent.name.Equals("RightHand"))
        {
            handType = HandType.HAND_TYPE.RIGHT;
        }
        else
        {
            handType = HandType.HAND_TYPE.LEFT;
        }
    }
}
