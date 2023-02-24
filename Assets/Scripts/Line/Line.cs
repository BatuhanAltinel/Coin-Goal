using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    [SerializeField] GameObject _passTheLineText;
    

    void OnEnable()
    {
        EventManager.OnCoinSelect += ShowTheWarningElements;
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


    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out Coin coin))
        {
            if(coin == CoinManager.Instance.SelectedCoin)
            {
                GameManager.Instance.PassTheLine = true;
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
                EventManager.OnPassSucces.Invoke();
            }
        }
    }

    void OnDisable()
    {
        EventManager.OnCoinSelect -= ShowTheWarningElements;
        EventManager.OnThrow -= DisappearWarningElements;
    }
}
