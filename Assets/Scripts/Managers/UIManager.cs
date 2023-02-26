using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class UIManager : MonoBehaviour
{  
    [SerializeField] TextMeshProUGUI _goalText;
    [SerializeField] TextMeshProUGUI _successText;
    [SerializeField] TextMeshProUGUI _levelText;

    int levelCount = 1;
    [SerializeField] Image _faultRefereeImage;

    
    public GameObject _NextLevelButton;
    public Button _RestartButton;
    [SerializeField]  GameObject _PullAndThrowPanel;

    [SerializeField] string[] successWords;

    [SerializeField] float _successTextSpeed = 0.7f;
    [SerializeField] float _goalTextSpeed = 1.5f;

    void OnEnable()
    {
        EventManager.OnGoal += TheGoal;
        EventManager.OnGoal += ShowNextLevelButton;
        EventManager.OnPassFail += FaultScreenOn;
        EventManager.OnPassFail += RestartButtonBouncing;
        EventManager.OnPassSucces += PassSuccessTextOn;
        EventManager.OnCoinSelect += DeActivePullAndThrowPanel;
    }

    void Start()
    {
        _PullAndThrowPanel.SetActive(true);
        _successText.gameObject.SetActive(false);
    }

    void DeActivePullAndThrowPanel()
    {
        _PullAndThrowPanel.SetActive(false);
    }
    void TheGoal()
    {
        _goalText.gameObject.SetActive(true);
        _goalText.gameObject.transform.DOScale(new Vector3(1,1,1),_goalTextSpeed).SetEase(Ease.InOutBack);
    }
    void FaultScreenOn()
    {
        _faultRefereeImage.gameObject.SetActive(true);

        _faultRefereeImage.gameObject.transform.DOScale(new Vector3(1,1,1),1f).SetEase(Ease.Linear)
        .OnComplete(() => _faultRefereeImage.gameObject.SetActive(false));
        _faultRefereeImage.gameObject.transform.localScale = new Vector3(0.9f,0.9f,1);

    }
    

    void PassSuccessTextOn()
    {
        int rand = Random.Range(0,successWords.Length);

        _successText.text = successWords[rand];
        _successText.gameObject.SetActive(true);

        _successText.gameObject.transform.DOScale(new Vector3(2,2,1),_successTextSpeed).SetEase(Ease.Linear).
        OnComplete(() => _successText.gameObject.transform.localScale = new Vector3(0,1,1));
    }

    public void RestartGame()
    {
        _goalText.gameObject.transform.localScale = new Vector3(0,1,1);
        _goalText.gameObject.SetActive(false);
        _NextLevelButton.gameObject.SetActive(false);
        EventManager.OnRestartLevel.Invoke();
    }

    void RestartButtonBouncing()
    {
        if(GameManager.Instance.FaultCount == 3)
        {
            _RestartButton.gameObject.transform.DOScale(new Vector3(1.2f,1.2f,1),1f).SetEase(Ease.InBounce).SetLoops(5,LoopType.Restart).
            OnComplete(() => _RestartButton.gameObject.transform.localScale = new Vector3(1,1,1));
            GameManager.Instance.FaultCount = 0;
        }
        
    }


    void ShowNextLevelButton()
    {
        _NextLevelButton.gameObject.SetActive(true);
    }

    public void NextLevel()
    {
        _goalText.gameObject.SetActive(false);
        levelCount++;
        _levelText.text = "LEVEL " + levelCount;

        _NextLevelButton.gameObject.SetActive(false);
        _goalText.gameObject.transform.localScale = new Vector3(0,1,1);
        EventManager.OnNextLevel.Invoke();
    }

    void OnDisable()
    {
        EventManager.OnCoinSelect -= DeActivePullAndThrowPanel;
        EventManager.OnGoal -= TheGoal;
        EventManager.OnGoal -=ShowNextLevelButton;
        EventManager.OnPassFail -= FaultScreenOn;
        EventManager.OnPassFail -= RestartButtonBouncing;
        EventManager.OnPassSucces -= PassSuccessTextOn;
    }
}
