using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool CanMove {get;set;}
    public bool PassTheLine { get; set; }
    public bool IsGoal { get; set; }
    [SerializeField] float _timeToWait = 3f;
    
    public TextMeshProUGUI GoalText;
    public Button RestartButton;

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

        
        if(PassTheLine)     // CHECK FIRST IS PASS THE LINE ??? ***************************
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
        GoalText.gameObject.SetActive(true);
        RestartButton.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
        RestartButton.gameObject.SetActive(false);
        GoalText.gameObject.SetActive(false);
    }

    void OnDisable()
    {
        EventManager.OnAfterThrow -= WaitForCoinMovement;
        EventManager.OnGoal -= TheGoal;
    }
}
