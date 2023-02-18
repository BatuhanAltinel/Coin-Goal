using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;
    LineRenderer _lr;
    float powerMultiplier = 0;
    public float PowerMultiplier { get{return powerMultiplier;} set{powerMultiplier = value;} }
    [SerializeField] List<Coin> coins;
    public Coin selectedCoin;
    public Vector2 moveTargetPos;
    [SerializeField] Vector2 maxPowerVector;
    

    void OnEnable()
    {
        EventManager.OnUnselectedCoins += DrawLineBetweenUnselectedCoins;
        EventManager.onCoinSelect += ShowLineRenderer;
        EventManager.OnPrepareToThrow += CalculateThePowerMultiplier;
        EventManager.OnThrow += DisappearLineRenderer;
        EventManager.OnThrow += ThrowTheSelectedCoin;
    }
    void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    void Start()
    {
        _lr = GetComponent<LineRenderer>();
        _lr.positionCount = coins.Count-1;
    }

    public void SetTheCoinSelected(Coin coin)
    {
        selectedCoin = coin;
    }

    void DrawLineBetweenUnselectedCoins()
    {
        int indx = 0;
        _lr.material = _lr.materials[0];
        foreach (var coin in coins)
        {
            if(selectedCoin != coin)
            {
                _lr.SetPosition(indx,coin.transform.position);
                indx++;
            }   
        }
    }
    
    void CalculateThePowerMultiplier()
    {
        powerMultiplier = moveTargetPos.magnitude / maxPowerVector.magnitude;
        maxPowerVector = new Vector2(200,200);

        if(moveTargetPos.magnitude > maxPowerVector.magnitude)
            powerMultiplier = 1;
        
        Debug.Log("powermultiplier = " + powerMultiplier);
    }

    void ThrowTheSelectedCoin()
    {
        if(selectedCoin != null)
            selectedCoin.MoveTo(moveTargetPos.normalized);
    }

    void ShowLineRenderer()
    {
        _lr.enabled =true;
    }
    void DisappearLineRenderer()
    {
        _lr.enabled = false;
    }

   
    void OnDisable()
    {
        EventManager.OnUnselectedCoins -= DrawLineBetweenUnselectedCoins;
        EventManager.onCoinSelect -= ShowLineRenderer;
        EventManager.OnPrepareToThrow -= CalculateThePowerMultiplier;
        EventManager.OnThrow -= ThrowTheSelectedCoin;
        EventManager.OnThrow -= DisappearLineRenderer;
    }
}

