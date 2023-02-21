using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SportMan : MonoBehaviour
{
    Animator _anim;
    Vector2 _randomShootVector;
    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        if(CoinManager.Instance.SelectedCoin != null)
        {
            transform.parent.LookAt(CoinManager.Instance.SelectedCoin.transform,Vector3.up);
        }
              
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<Coin>(out Coin coin))
        {
            Debug.Log("Coin collided");

            RandomShootVector();
            PlayShootAnim();
            coin.MoveTo(_randomShootVector);

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
}
