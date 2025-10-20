using System;
using System.Collections;
using UnityEngine;

[Serializable]
public struct CurrencyUIData
{
    [field: SerializeField] public Sprite Icon {  get; private set; }
    [field: SerializeField] public Sprite BG {  get; private set; }
}