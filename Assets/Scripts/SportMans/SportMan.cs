using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SportMan : MonoBehaviour
{
    Animator _anim;
    Vector2 _randomShootVector;
    Vector3 _startPosition;
    Quaternion _startRotation;
    float _maxShootPower = 1100f;


    void OnEnable()
    {
        EventManager.OnGoal += PlaySadAnim;
        EventManager.OnRestartLevel += PlayIdleAnim;
        EventManager.OnNextLevel += PlayIdleAnim;
        EventManager.OnRestartLevel += OnRestart;
        EventManager.OnNextLevel += OnRestart;
    }
    void Start()
    {
        _anim = GetComponent<Animator>();
        _startPosition = transform.parent.position;
        _startRotation = transform.parent.rotation;
    }

    void Update()
    {
        if(CoinManager.Instance.PreviousCoin != null && !GameManager.Instance.IsGoal)
        {
            transform.parent.LookAt(CoinManager.Instance.PreviousCoin.transform,Vector3.up);
        }
              
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<Coin>(out Coin coin) && !GameManager.Instance.IsGoal)
        {
            if(coin.coinState == CoinStates.powerUp)
            {
                EventManager.OnCoinStateChanged.Invoke(CoinStates.normal);
                OnCrushPowerUpCoin();
            }else
            {
                RandomShootVector();
                PlayShootAnim();

                coin.MoveTo(_randomShootVector,_maxShootPower);
                EventManager.OnKickBack.Invoke();
                Invoke("PlayIdleAnim",0.2f);
            }
            

            
        }
    }

    void OnRestart()
    {
        transform.parent.position = _startPosition;
        transform.parent.rotation = _startRotation;
    }

    void OnCrushPowerUpCoin()
    {
        StartCoroutine(OnCrushPowerUpCoinRoutine());
    }

    IEnumerator OnCrushPowerUpCoinRoutine()
    {
        Vector3 targetPos = new Vector3(50,50,transform.position.z);
        Quaternion targetRot = Quaternion.Euler(-155,180,-128);

        while(Vector3.Distance(transform.position,targetPos) > 0.1f)
        {
            transform.parent.position = Vector3.Lerp(transform.parent.position,targetPos,0.1f);
            transform.parent.rotation = Quaternion.Lerp(transform.parent.rotation,targetRot,0.1f);

            yield return new WaitForEndOfFrame();
        }
        transform.parent.position = targetPos;
        transform.parent.rotation = targetRot;
        yield break;
    }
    void PlayIdleAnim()
    {
        _anim.SetBool("IsCoinCollision",false);
        _anim.SetBool("IsGoal",false);
    }

    void PlayShootAnim()
    {
        _anim.SetBool("IsCoinCollision",true);
    }

    void PlaySadAnim()
    {
        _anim.SetBool("IsGoal",true);
    }

    void RandomShootVector()
    {
        float randomVectorY = Random.Range(1,0);
        float randomVectorX = Random.Range(-0.5f,0.5f);

        _randomShootVector = new Vector2(randomVectorX,randomVectorY);
    }

    void OnDisable()
    {
        EventManager.OnGoal -= PlaySadAnim;
        EventManager.OnRestartLevel -= PlayIdleAnim;
        EventManager.OnNextLevel -= PlayIdleAnim;
        EventManager.OnRestartLevel -= OnRestart;
        EventManager.OnNextLevel -= OnRestart;
    }
}
