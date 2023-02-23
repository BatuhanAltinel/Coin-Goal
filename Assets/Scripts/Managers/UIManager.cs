using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _faultText;   
    [SerializeField] TextMeshProUGUI _goalText;
    [SerializeField] TextMeshProUGUI _successText;

    [SerializeField] Image _faultRefereeImage;
    [SerializeField] Image _goalImage;

    
    public Button RestartButton;
    [SerializeField]  GameObject _PullAndThrowPanel;

    [SerializeField] string[] successWords;

    void OnEnable()
    {
        EventManager.OnGoal += TheGoal;
        EventManager.OnPassFail += FaultScreenOn;
        EventManager.OnPassFail += PassSuccessTextOff;
        EventManager.OnPassSucces += PassSuccessTextOn;
        EventManager.onCoinSelect += FaultScreenOff;
        EventManager.onCoinSelect += DeActivePullAndThrowPanel;
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
        _goalImage.gameObject.SetActive(true);
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
    }

    void PassSuccessTextOff()
    {
        _successText.gameObject.SetActive(false);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
        _faultText.gameObject.SetActive(false);
        _goalText.gameObject.SetActive(false);
    }

    void OnDisable()
    {
        EventManager.onCoinSelect -= DeActivePullAndThrowPanel;
        EventManager.OnGoal -= TheGoal;
        EventManager.OnPassFail -= FaultScreenOn;
        EventManager.OnPassFail -= PassSuccessTextOff;
        EventManager.OnPassSucces -= PassSuccessTextOn;
        EventManager.onCoinSelect -= FaultScreenOff;
    }
}
