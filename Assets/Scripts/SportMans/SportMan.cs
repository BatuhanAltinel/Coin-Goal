using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SportMan : MonoBehaviour
{
    Animator _anim;
    Vector2 _randomShootVector;
    float _randomShootPower;
    float _minShootPower = 500;
    float _maxShootPower = 750;

    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        if(CoinManager.Instance.PreviousCoin != null)
        {
            transform.parent.LookAt(CoinManager.Instance.PreviousCoin.transform,Vector3.up);
        }
              
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<Coin>(out Coin coin))
        {
            RandomShootVector();
            RandomShootPower();
            PlayShootAnim();

            coin.MoveTo(_randomShootVector,_randomShootPower);

            Invoke("PlayIdleAnim",0.2f);
        }
    }

    void PlayIdleAnim()
    {
        _anim.SetBool("IsCoinCollision",false);
    }

    void PlayShootAnim()
    {
        _anim.SetBool("IsCoinCollision",true);
    }

    void RandomShootVector()
    {
        float randomX = Random.Range(-1,1);
        float randomY = Random.Range(-1,1);

        _randomShootVector = new Vector2(randomX,randomY);
    }
    
    void RandomShootPower()
    {
        _randomShootPower = Random.Range(_minShootPower,_maxShootPower);
    }
}
