using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    Rigidbody myBody;
    Vector3 normalScale;
    [SerializeField] float _moveSpeed;

    void OnEnable()
    {
        EventManager.onCoinSelect += CoinScaleUp;
        EventManager.onCoinSelect += CoinNormalScale;
    }
    
    void Start()
    {
        normalScale = new Vector3(1,0.1f,1);
        myBody = GetComponent<Rigidbody>();    
    }

    public void MoveTo(Vector2 dir)
    {
        Vector3 movePos = new Vector3(dir.x,transform.position.y,dir.y);
        myBody.AddForce(-movePos * _moveSpeed ,ForceMode.Impulse);
    }

    void CoinScaleUp()
    {
        if(CoinManager.Instance.selectedCoin == this)
            transform.localScale = new Vector3(1.5f,0.1f,1.5f);
    }
    
    void CoinNormalScale()
    {
        if(CoinManager.Instance.selectedCoin != this)
        {
            transform.localScale = normalScale;
        }
        
    }
    void OnDisable()
    {
        EventManager.onCoinSelect -= CoinScaleUp;
        EventManager.onCoinSelect -= CoinNormalScale;
    }
}
