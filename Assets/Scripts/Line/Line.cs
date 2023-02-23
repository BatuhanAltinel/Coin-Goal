using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    [SerializeField] GameObject _passTheLineText;
    

    void OnEnable()
    {
        EventManager.onCoinSelect += ShowTheWarningElements;
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


    void OnTriggerStay(Collider other)
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
        EventManager.onCoinSelect -= ShowTheWarningElements;
        EventManager.OnThrow -= DisappearWarningElements;
    }
}
