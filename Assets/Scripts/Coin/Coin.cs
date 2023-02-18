using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    Rigidbody _rb;
    LineRenderer _lr;
    Vector3 normalScale;
    float powerMeter = 5;
    [SerializeField] float _maxPower;

    void OnEnable()
    {
        EventManager.onCoinSelect += CoinScaleUp;
        EventManager.onCoinSelect += CoinNormalScale;
        EventManager.OnPrepareToThrow += SetTheArrow;
        EventManager.OnThrow += RemoveArrow;
        EventManager.OnThrowEnd += CoinNormalScale;
    }
    
    void Start()
    {
        normalScale = new Vector3(1,0.1f,1);
        _rb = GetComponent<Rigidbody>();    
        _lr = GetComponent<LineRenderer>();
    }

    public void MoveTo(Vector2 dir)
    {
        Vector3 movePos = new Vector3(dir.x,transform.position.y,dir.y);
        _rb.AddForce(-movePos * _maxPower * CoinManager.Instance.PowerMultiplier ,ForceMode.Impulse);
    }
    void SetTheArrow()
    {
        if(CoinManager.Instance.selectedCoin == this)
        {
            _lr.enabled = true;
            _lr.positionCount = 2;
            _lr.SetPosition(0,Vector3.zero);
            Vector3 newPos = new Vector3(CoinManager.Instance.moveTargetPos.x,0,CoinManager.Instance.moveTargetPos.y);

            _lr.SetPosition(1,-newPos.normalized * powerMeter * CoinManager.Instance.PowerMultiplier);
        }
        
    }

    void RemoveArrow()
    {
        _lr.enabled = false;
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
        EventManager.OnPrepareToThrow -= SetTheArrow;
        EventManager.OnThrow -= RemoveArrow;
        EventManager.OnThrowEnd -= CoinNormalScale;
    }
}
