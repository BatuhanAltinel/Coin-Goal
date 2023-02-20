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
    Coin selectedCoin;
    public Coin SelectedCoin { get {return selectedCoin;} set{selectedCoin = value;} }

    [SerializeField] GameObject _PassLinePrefab;

    public Vector2 targetVector;
    [SerializeField] Vector2 maxPowerVector;
    
    List<Vector3> unSelectedCoinsPositions = new();

    void OnEnable()
    {
        EventManager.OnUnselectedCoins += DrawLineBetweenUnselectedCoins;
        EventManager.onCoinSelect += ShowLineRenderer;
        EventManager.OnPrepareToThrow += CalculateThePowerMultiplier;
        EventManager.OnThrow += DisappearLineRenderer;
        EventManager.OnThrow += ThrowTheSelectedCoin;
        EventManager.OnPassFail += AllCoinsGoToPreviousPosition;
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
                unSelectedCoinsPositions.Add(coin.transform.position);
                _lr.SetPosition(indx,coin.transform.position);
                indx++;
            }   
        }
        
        SetThePassLineTransform();
    }

    void SetThePassLineTransform()
    {

        float X_NewScale = Vector3.Magnitude(unSelectedCoinsPositions[1] - unSelectedCoinsPositions[0]);
        _PassLinePrefab.transform.localScale = new Vector3(X_NewScale,1,0.2f);

        float X_NewPos = unSelectedCoinsPositions[0].x + (unSelectedCoinsPositions[1].x - unSelectedCoinsPositions[0].x) / 2;
        float Z_NewPos = unSelectedCoinsPositions[0].z + (unSelectedCoinsPositions[1].z - unSelectedCoinsPositions[0].z) / 2;
        _PassLinePrefab.transform.position = new Vector3(X_NewPos,1,Z_NewPos);

        float Y_NewRotation = Mathf.Atan2((unSelectedCoinsPositions[0].x - unSelectedCoinsPositions[1].x),(unSelectedCoinsPositions[0].z - unSelectedCoinsPositions[1].z)) * 180 / Mathf.PI;
        _PassLinePrefab.transform.rotation = Quaternion.Euler(0,Y_NewRotation + 90,0);

        // foreach (var coin in coins)
        // {
        //     unSelectedCoinsPositions.Remove(coin.transform.position);
        // }
        unSelectedCoinsPositions.Clear();
    }
    
    void CalculateThePowerMultiplier()
    {
        maxPowerVector = new Vector2(200,200);
        powerMultiplier = targetVector.magnitude / maxPowerVector.magnitude;
        
        if(targetVector.magnitude > maxPowerVector.magnitude)
            powerMultiplier = 1;
        
        // Debug.Log("powermultiplier = " + powerMultiplier);
    }

    void ThrowTheSelectedCoin()
    {
        if(selectedCoin != null)
            selectedCoin.MoveTo(targetVector.normalized);
    }

    void AllCoinsGoToPreviousPosition()
    {
        foreach (var coin in coins)
        {
            coin.GoToPreviousPosition();
        }
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
        EventManager.OnPassFail -= AllCoinsGoToPreviousPosition;
    }
}

