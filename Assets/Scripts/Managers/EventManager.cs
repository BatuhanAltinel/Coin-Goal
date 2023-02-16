using UnityEngine;
using System;
public class EventManager : MonoBehaviour
{
    public static Action onCoinSelect;
    public static Action OnUnselectedCoins;
    public static Action OnPrepareToThrow;
    public static Action OnThrow;
}
