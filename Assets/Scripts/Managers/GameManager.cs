using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool CanMove {get;set;}
    public bool PassTheLine { get; set; }
    public bool IsGoal { get; set; }
    [SerializeField] float _timeToWait = 2f;

    

    void OnEnable()
    {
        EventManager.OnCoinSelect += PassTheLineFalse;
        EventManager.OnAfterThrow += WaitForCoinMovement;
    }
    
    void Awake()
    {
        if(Instance == null)
            Instance = this;
        else 
            Destroy(gameObject);
        
        CanMove = true;
        PassTheLine = false;
        IsGoal = false;
    }

    void PassTheLineFalse()
    {
        PassTheLine = false;
    }
    void WaitForCoinMovement()
    {
        StartCoroutine(WaitForCoinMovementRoutine());
    }

    IEnumerator WaitForCoinMovementRoutine()
    {
        float elapsedTime = 0;
        CanMove = false;
        
        while(elapsedTime < _timeToWait && !PassTheLine)
        {
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        
        if(!PassTheLine)
        {
            EventManager.OnPassFail.Invoke();
        } 

        CanMove = true;
        CoinManager.Instance.SetTheCoinSelected(null);
        EventManager.OnThrowEnd.Invoke();
    }

    void OnDisable()
    {
        EventManager.OnCoinSelect -= PassTheLineFalse;
        EventManager.OnAfterThrow -= WaitForCoinMovement;
        
    }
}
