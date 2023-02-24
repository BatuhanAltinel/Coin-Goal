using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{

    void OnEnable()
    {
        EventManager.OnRestartLevel += OnRestartGame;
        EventManager.OnNextLevel += OnRestartGame;
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out Coin coin))
        {
            if(GameManager.Instance.PassTheLine)
            {
                EventManager.OnGoal.Invoke();
                GameManager.Instance.IsGoal = true;
                gameObject.GetComponent<BoxCollider>().enabled = false;
            }
            
        }else
        {
            EventManager.OnPassFail.Invoke();
        }
    }

    void OnRestartGame()
    {
        gameObject.GetComponent<BoxCollider>().enabled = true;
    }

    void OnDisable()
    {
        EventManager.OnRestartLevel -= OnRestartGame;
        EventManager.OnNextLevel -= OnRestartGame;
    }
}
