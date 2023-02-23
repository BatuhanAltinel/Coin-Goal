using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SportMan : MonoBehaviour
{
    Animator _anim;
    Vector2 _randomShootVector;
    float _maxShootPower = 1000;

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
            PlayShootAnim();

            coin.MoveTo(_randomShootVector,_maxShootPower);

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
        float randomVectorY = Random.Range(1,0);
        float randomVectorX = Random.Range(-0.5f,0.5f);

        _randomShootVector = new Vector2(randomVectorX,randomVectorY);
    }
    
}
