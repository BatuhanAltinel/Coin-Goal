using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] float _turnSpeed = 90f;

    void OnEnable()
    {
        EventManager.OnRestartLevel += OnRestart;
        EventManager.OnNextLevel += OnRestart;    
    }

    void Update()
    {
        PowerUpTurning();
    }

    void PowerUpTurning()
    {
        transform.GetChild(0).Rotate(new Vector3(0,_turnSpeed * Time.deltaTime,0));
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<Coin>(out Coin coin))
        {
            if(GameManager.Instance.PassTheLine && coin.coinState == CoinStates.normal)
            {
                gameObject.transform.GetChild(0).gameObject.SetActive(false);  
                gameObject.transform.GetChild(1).gameObject.SetActive(false);
                GetComponent<SphereCollider>().enabled = false; 
                EventManager.OnCoinStateChanged.Invoke(CoinStates.powerUp);
            }
        }
    }

    void OnRestart()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        gameObject.transform.GetChild(1).gameObject.SetActive(true);
        GetComponent<SphereCollider>().enabled = true;
    }

    void OnDisable()
    {
        EventManager.OnRestartLevel -= OnRestart;
        EventManager.OnNextLevel -= OnRestart;
    }
}
