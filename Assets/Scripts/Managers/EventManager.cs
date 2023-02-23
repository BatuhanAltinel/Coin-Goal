using UnityEngine;
using System;
public class EventManager : MonoBehaviour
{
    public static Action onCoinSelect;
    public static Action OnPrepareToThrow;
    public static Action OnThrow;
    public static Action OnThrowEnd;
    public static Action OnAfterThrow;
    public static Action OnPassFail;
    public static Action OnGoal;
}
