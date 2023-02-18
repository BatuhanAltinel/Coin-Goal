using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    Rigidbody myBody;
    LineRenderer lr;
    Vector3 normalScale;
    [SerializeField] float _moveSpeed;

    void OnEnable()
    {
        EventManager.onCoinSelect += CoinScaleUp;
        EventManager.onCoinSelect += CoinNormalScale;
        EventManager.OnPrepareToThrow += SetTheArrow;
        EventManager.OnThrow += RemoveArrow;
    }
    
    void Start()
    {
        normalScale = new Vector3(1,0.1f,1);
        myBody = GetComponent<Rigidbody>();    
        lr = GetComponent<LineRenderer>();
    }

    public void MoveTo(Vector2 dir)
    {
        Vector3 movePos = new Vector3(dir.x,transform.position.y,dir.y);
        myBody.AddForce(-movePos * _moveSpeed ,ForceMode.Impulse);
    }
    void SetTheArrow()
    {
        if(CoinManager.Instance.selectedCoin == this)
        {
            lr.enabled = true;
            lr.positionCount = 2;
            lr.SetPosition(0,Vector3.zero);
            Vector3 newPos = new Vector3(CoinManager.Instance.moveTargetPos.x,0,CoinManager.Instance.moveTargetPos.y);
            lr.SetPosition(1,-newPos.normalized*4);
        }
        
    }

    void RemoveArrow()
    {
        lr.enabled = false;
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
    }
}
