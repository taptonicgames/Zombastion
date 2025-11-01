using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractEquipment
{
    public string Id {  get; protected set; }
    public EquipmentType Type { get; protected set; }
    public RarityType Rarity { get; protected set; }
}
