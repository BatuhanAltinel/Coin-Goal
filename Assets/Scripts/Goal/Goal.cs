using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
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
            // Fault Screen
        }
    }
}
