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
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if(Physics.Raycast(ray,out hit))
                {
                    if(hit.transform.TryGetComponent<Coin>(out Coin coin))
                    {
                        firstFingerPos = new Vector2(coin.transform.position.x,coin.transform.position.z);
                        CoinManager.Instance.SetTheCoinSelected(coin);
                        EventManager.onCoinSelect.Invoke();
                        EventManager.OnUnselectedCoins.Invoke();
                        previousCoin = coin;
                    }
                }
            }
            else if(_touch.phase == TouchPhase.Moved && previousCoin != null)
            {
                lastFingerPos = firstFingerPos + _touch.deltaPosition;
                targetPos = lastFingerPos-firstFingerPos;
                CoinManager.Instance.moveTargetPos = targetPos.normalized;

                if(targetPos.magnitude > 0)
                    isMoved = true;
                Debug.Log(targetPos.normalized);
                
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
