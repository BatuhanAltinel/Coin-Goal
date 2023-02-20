using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    
    void Start()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out Coin coin))
        {
            if(coin == CoinManager.Instance.SelectedCoin)
            {
                GameManager.Instance.PassTheLine = true;
                Debug.Log("Seleceted coin passed.");
            }
        }
    }
}
