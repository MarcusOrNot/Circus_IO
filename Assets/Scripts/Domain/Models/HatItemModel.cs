using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New HatItem", menuName = "Hat Shop Item")]
public class HatItemModel: ScriptableObject
{
    public HatType Hat;
    public Sprite HatSprite;
    public int price;
}
