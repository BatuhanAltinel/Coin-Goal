using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _faultText;   
    [SerializeField] TextMeshProUGUI _goalText;

    public Button RestartButton;

    [SerializeField]  GameObject _PullAndThrowPanel;

    void OnEnable()
    {
        EventManager.OnGoal += TheGoal;
        EventManager.OnPassFail += FaultScreenOn;
        EventManager.onCoinSelect += FaultScreenOff;
    }


    void TheGoal()
    {
        _goalText.gameObject.SetActive(true);
        RestartButton.gameObject.SetActive(true);
    }
    void FaultScreenOn()
    {
        _faultText.gameObject.SetActive(true);
    }

    void FaultScreenOff()
    {
        _faultText.gameObject.SetActive(false);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
        _faultText.gameObject.SetActive(false);
        _goalText.gameObject.SetActive(false);
    }

    void OnDisable()
    {
        EventManager.OnGoal -= TheGoal;
        EventManager.OnPassFail -= FaultScreenOn;
        EventManager.onCoinSelect -= FaultScreenOff;
    }
}
