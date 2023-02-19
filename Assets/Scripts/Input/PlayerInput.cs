using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    Touch _touch;
    bool _canMove = false;
    Vector2 _firstFingerPos = Vector2.zero;
    Vector2 _lastFingerPos = Vector2.zero;

    void Update()
    {
        Inputs();
    }

    void Inputs()
    {
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
                        _firstFingerPos = _touch.position;
                        
                        CoinManager.Instance.SetTheCoinSelected(coin);
                        EventManager.onCoinSelect.Invoke();
                        EventManager.OnUnselectedCoins.Invoke();
                    }
                }
            }
            else if(_touch.phase == TouchPhase.Moved)
            {
                _lastFingerPos = _touch.position;
                Vector2 targetPos = _lastFingerPos - _firstFingerPos;

                CoinManager.Instance.targetVector = targetPos;
                EventManager.OnPrepareToThrow.Invoke();

                if(targetPos.magnitude > 0)
                    _canMove = true;
                // Debug.Log("target vector : "+targetPos.normalized);
                
            }
            else if(_touch.phase == TouchPhase.Ended)
            {
                if(CoinManager.Instance.SelectedCoin != null && _canMove)
                    EventManager.OnThrow.Invoke();

                // CoinManager.Instance.SetTheCoinSelected(null);
                // EventManager.OnThrowEnd.Invoke();
                _canMove = false;
            }
        }
    }
}
