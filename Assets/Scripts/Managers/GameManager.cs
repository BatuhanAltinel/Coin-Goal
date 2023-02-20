using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool CanMove {get;set;}
    public bool PassTheLine { get; set; }
    public bool IsGoal { get; set; }
    [SerializeField] float _timeToWait = 3f;

    void OnEnable()
    {
        EventManager.OnAfterThrow += WaitForCoinMovement;
        EventManager.OnGoal += TheGoal;
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

    void WaitForCoinMovement()
    {
        StartCoroutine(WaitForCoinMovementRoutine());
    }

    IEnumerator WaitForCoinMovementRoutine()
    {
        
        CanMove = false;
        Debug.Log("Wait for seconds....");
            
        yield return new WaitForSeconds(_timeToWait);

        
        if(PassTheLine)
        {
            CanMove = true;
            PassTheLine = false;
        }    
        else
        {
            EventManager.OnPassFail.Invoke();
            CanMove = true;
        }
        CoinManager.Instance.SetTheCoinSelected(null);
        EventManager.OnThrowEnd.Invoke();
        

    }

    void TheGoal()
    {
        Debug.Log("GOOOOOOOOOOOOOOOOOOOOOOAAAAAALLLLLL");
    }

    void OnDisable()
    {
        EventManager.OnAfterThrow -= WaitForCoinMovement;
        EventManager.OnGoal -= TheGoal;
    }
}
