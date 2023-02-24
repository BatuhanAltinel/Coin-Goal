using UnityEngine;
using System;
public class EventManager : MonoBehaviour
{
    public static Action OnCoinSelect;
    public static Action OnPrepareToThrow;
    public static Action OnThrow;
    public static Action OnThrowEnd;
    public static Action OnAfterThrow;
    public static Action OnPassSucces;
    public static Action OnPassFail;
    public static Action OnGoal;
    public static Action OnKickBack;
}
