using FP.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviourSingleton<UIManager>
{
    [SerializeField] private AttackIndicatorUI _attackIndicatorUI;

    public AttackIndicatorUI AttackIndicatorUI { get => _attackIndicatorUI; }
}
