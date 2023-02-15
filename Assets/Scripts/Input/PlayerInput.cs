using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SelectCoin();
    }

    void SelectCoin()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit))
            {
               if(hit.transform.TryGetComponent<Coin>(out Coin coin))
               {
                    CoinManager.Instance.SetTheCoinSelected(coin);
                    EventManager.onCoinSelect.Invoke();
                    EventManager.OnUnselectedCoins.Invoke();
               }
            }
        }else if(Input.GetMouseButton(0))
        {
            
        }
    }
    void OnMouseDrag()
    {
        Vector3 mouseStartPos = Input.mousePosition;
        
    }
}
