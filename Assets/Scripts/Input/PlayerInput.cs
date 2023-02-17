using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    Touch _touch;
    Coin previousCoin;
    Vector2 targetPos;
    bool isMoved = false;

    void Update()
    {
        Inputs();
    }

    void Inputs()
    {
        Vector2 firstFingerPos = Vector3.zero;
        Vector2 lastFingerPos = Vector2.zero;

        if(Input.touchCount > 0)
        {
            _touch = Input.GetTouch(0);

            if(_touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(_touch.position);
                RaycastHit hit;
                if(Physics.Raycast(ray,out hit))
                {
                    if(hit.transform.TryGetComponent<Coin>(out Coin coin))
                    {
                        // firstFingerPos = new Vector2(coin.transform.position.x,coin.transform.position.z);
                        firstFingerPos = _touch.position;
                        
                        CoinManager.Instance.SetTheCoinSelected(coin);
                        EventManager.onCoinSelect.Invoke();
                        EventManager.OnUnselectedCoins.Invoke();
                        previousCoin = coin;
                    }
                }
            }
            if(_touch.phase == TouchPhase.Moved && previousCoin != null)
            {
                lastFingerPos = _touch.deltaPosition;
                targetPos = _touch.deltaPosition - _touch.position;

                Debug.Log("FirstFinher position : "+firstFingerPos);
                // Debug.DrawRay(firstFingerPos,lastFingerPos,Color.grey,2f);
                Debug.Log("Last finger position : "+lastFingerPos);

                CoinManager.Instance.moveTargetPos = targetPos;
                EventManager.OnPrepareToThrow.Invoke();

                // EventManager.OnUnselectedCoins.Invoke();
                if(targetPos.magnitude > 0)
                    isMoved = true;
                Debug.Log("target vector : "+targetPos);
                
            }
            if(_touch.phase == TouchPhase.Ended)
            {
                if(CoinManager.Instance.selectedCoin != null && isMoved)
                    EventManager.OnThrow.Invoke();
                CoinManager.Instance.selectedCoin = null;
                isMoved = false;
            }
        }
    }
}
