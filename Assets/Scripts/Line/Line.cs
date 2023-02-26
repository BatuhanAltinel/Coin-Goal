using UnityEngine;
using DG.Tweening;

public class Line : MonoBehaviour
{
    [SerializeField] GameObject _passTheLineText;
    

    void OnEnable()
    {
        EventManager.OnCoinSelect += ShowTheWarningElements;
        EventManager.OnCoinSelect += PassTheLineTextBouncing;
        EventManager.OnThrow += DisappearWarningElements;
    }
    void ShowTheWarningElements()
    {
        _passTheLineText.SetActive(true);
    }
    
    void DisappearWarningElements()
    {
        _passTheLineText.SetActive(false);
    }

    void PassTheLineTextBouncing()
    {
        _passTheLineText.gameObject.transform.DOScale(new Vector3(1.2f,1.2f,1),0.8f).SetEase(Ease.Linear).SetLoops(4,LoopType.Yoyo).
        OnComplete(() => _passTheLineText.gameObject.transform.localScale = new Vector3(1,1,1));
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out Coin coin))
        {
            if(coin == CoinManager.Instance.SelectedCoin)
            {
                GameManager.Instance.PassTheLine = true;
                GameManager.Instance.FaultCount = 0;

                EventManager.OnPassSucces.Invoke();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.TryGetComponent(out Coin coin))
        {
            if(coin == CoinManager.Instance.SelectedCoin)
            {
                GameManager.Instance.PassTheLine = true;
                GameManager.Instance.FaultCount = 0;

                EventManager.OnPassSucces.Invoke();
            }
        }
    }

    void OnDisable()
    {
        EventManager.OnCoinSelect -= ShowTheWarningElements;
        EventManager.OnThrow -= DisappearWarningElements;
        EventManager.OnCoinSelect -= PassTheLineTextBouncing;
    }
}
