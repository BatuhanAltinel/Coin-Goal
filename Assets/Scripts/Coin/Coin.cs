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

    public void MoveTo(Vector3 dir)
    {
        myBody.AddForce(dir * _moveSpeed * Time.deltaTime,ForceMode.Impulse);
    }

    void CoinScaleUp()
    {
        if(CoinManager.Instance.selectedCoin == this)
            transform.localScale = new Vector3(2,0.1f,2);
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
