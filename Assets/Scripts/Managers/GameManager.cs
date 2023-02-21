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
    [SerializeField] float _timeToWait = 2f;
    
    public TextMeshProUGUI GoalText;
    public Button RestartButton;

    void OnEnable()
    {
        EventManager.onCoinSelect += PassTheLineFalse;
        EventManager.OnAfterThrow += WaitForCoinMovement;
        EventManager.OnPassFail += FaultScreen;
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

            // Debug.Log("Waiting coin move for a " + elapsedTime + " seconds");

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

    void FaultScreen()
    {
        Debug.Log("Fauult!!");
    }

    void OnDisable()
    {
        EventManager.onCoinSelect -= PassTheLineFalse;
        EventManager.OnAfterThrow -= WaitForCoinMovement;
        EventManager.OnPassFail -= FaultScreen;
        EventManager.OnGoal -= TheGoal;
    }
}
