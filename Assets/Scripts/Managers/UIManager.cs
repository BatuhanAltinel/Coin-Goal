using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _faultText;   
    [SerializeField] TextMeshProUGUI _goalText;
    [SerializeField] TextMeshProUGUI _successText;

    [SerializeField] Image _faultRefereeImage;

    
    public Button RestartButton;
    [SerializeField]  GameObject _PullAndThrowPanel;

    [SerializeField] string[] successWords;

    [SerializeField] float _successTextSpeed = 0.7f;
    [SerializeField] float _goalTextSpeed = 1.5f;

    void OnEnable()
    {
        EventManager.OnGoal += TheGoal;
        EventManager.OnPassFail += FaultScreenOn;
        EventManager.OnPassSucces += PassSuccessTextOn;
        EventManager.OnCoinSelect += FaultScreenOff;
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
        _goalText.gameObject.transform.DOScale(new Vector3(1,1,1),_goalTextSpeed).SetEase(Ease.InOutBack);  // TRY DIFFERENT EAESES.
    }
    void FaultScreenOn()
    {
        _faultText.gameObject.SetActive(true);
        _faultRefereeImage.gameObject.SetActive(true);
    }

    void FaultScreenOff()
    {
        _faultText.gameObject.SetActive(false);
        _faultRefereeImage.gameObject.SetActive(false);
    }

    void PassSuccessTextOn()
    {
        int rand = Random.Range(0,successWords.Length);
        _successText.text = successWords[rand];
        _successText.gameObject.SetActive(true);
        _successText.gameObject.transform.DOScale(new Vector3(2,1,1),_successTextSpeed).SetEase(Ease.Linear).
        OnComplete(() => _successText.gameObject.transform.localScale = new Vector3(0,1,1));
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
        _faultText.gameObject.SetActive(false);
        _goalText.gameObject.SetActive(false);
    }

    void OnDisable()
    {
        EventManager.OnCoinSelect -= DeActivePullAndThrowPanel;
        EventManager.OnGoal -= TheGoal;
        EventManager.OnPassFail -= FaultScreenOn;
        EventManager.OnPassSucces -= PassSuccessTextOn;
        EventManager.OnCoinSelect -= FaultScreenOff;
    }
}
