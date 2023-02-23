using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    Touch _touch;
    Vector2 _firstFingerPos = Vector2.zero;
    Vector2 _lastFingerPos = Vector2.zero;

    void Update()
    {
        Inputs();
    }

    void Inputs()
    {
        if(Input.touchCount > 0 && GameManager.Instance.CanMove)
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
                        _firstFingerPos = _touch.position;
                        
                        CoinManager.Instance.SetTheCoinSelected(coin);
                        CoinManager.Instance.SetThePreviousCoin(coin);

                        EventManager.onCoinSelect.Invoke();
                    }
                }
            }
            else if(_touch.phase == TouchPhase.Moved)
            {
                _lastFingerPos = _touch.position;
                Vector2 targetPos = _lastFingerPos - _firstFingerPos;

                CoinManager.Instance.targetVector = targetPos;
                
                EventManager.OnPrepareToThrow.Invoke();

                
            }
            else if(_touch.phase == TouchPhase.Ended)
            {
                if(CoinManager.Instance.SelectedCoin != null && GameManager.Instance.CanMove)
                {
                    EventManager.OnThrow.Invoke();
                    EventManager.OnAfterThrow.Invoke();
                }
            }    
        }
    }
}

