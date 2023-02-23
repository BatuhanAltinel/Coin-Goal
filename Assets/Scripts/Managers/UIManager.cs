using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _faultText;   
    [SerializeField] TextMeshProUGUI _goalText;
    [SerializeField] Image _faultRefereeImage;

    public Button RestartButton;

    [SerializeField]  GameObject _PullAndThrowPanel;

    void OnEnable()
    {
        EventManager.OnGoal += TheGoal;
        EventManager.OnPassFail += FaultScreenOn;
        EventManager.onCoinSelect += FaultScreenOff;
        EventManager.onCoinSelect += DeActivePullAndThrowPanel;
    }

    void Start()
    {
        _PullAndThrowPanel.SetActive(true);
    }

    void DeActivePullAndThrowPanel()
    {
        _PullAndThrowPanel.SetActive(false);
    }
    void TheGoal()
    {
        _goalText.gameObject.SetActive(true);
        RestartButton.gameObject.SetActive(true);
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
        EventManager.onCoinSelect -= FaultScreenOff;
    }
}
