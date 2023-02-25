using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;
    LineRenderer _lr;


    float powerMultiplier = 0;
    public float PowerMultiplier { get{return powerMultiplier;} set{powerMultiplier = value;} }


    [SerializeField] List<Coin> coins;
    public Coin SelectedCoin { get ; set;}
    public Coin PreviousCoin { get; set;}

    [SerializeField] GameObject[] _PassLinePrefab;
    [SerializeField] float _lineScaleOffset = 2.2f;
    public Vector2 targetVector;
    [SerializeField] Vector2 maxPowerVector;
    
    [SerializeField] List<Vector3> unSelectedCoinsPositions = new();

    void OnEnable()
    {
        EventManager.OnCoinSelect += FindUnselectedCoins;
        EventManager.OnCoinSelect += ShowLineRenderer;
        EventManager.OnCoinSelect += DrawLineBetweenUnselectedCoins;
        EventManager.OnCoinSelect += SetThePassLineTransform;
        EventManager.OnCoinSelect += LineColorChange;
        EventManager.OnPrepareToThrow += CalculateThePowerMultiplier;
        EventManager.OnThrow += DisappearLineRenderer;
        EventManager.OnThrow += ThrowTheSelectedCoin;
        EventManager.OnThrow += ResetUnselectedCoins;
        EventManager.OnPassFail += AllCoinsGoToPreviousPosition;
        EventManager.OnRestartLevel += AllCoinsGoToStartPosition;
        EventManager.OnNextLevel += AllCoinsGoToStartPosition;
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
        SelectedCoin = coin;
    }
    
    public void SetThePreviousCoin(Coin coin)
    {
        PreviousCoin = coin;
    }
    
    void FindUnselectedCoins()
    {
        foreach (var coin in coins)
        {
            if(coin != SelectedCoin)
            {
                unSelectedCoinsPositions.Add(coin.transform.position);
            }
        }
    }

    void DrawLineBetweenUnselectedCoins()
    {
        _lr.material = _lr.materials[0];

        if(unSelectedCoinsPositions.Count > 1)
        {
            for (int i = 0; i < unSelectedCoinsPositions.Count; i++)
            {
                _lr.SetPosition(i,unSelectedCoinsPositions[i]);
            }
        }
        
    }

    void LineColorChange()
    {
        _lr.DOColor(new Color2(Color.blue,Color.blue),new Color2(Color.green,Color.green),0.5f).SetEase(Ease.Linear).SetLoops(-1,LoopType.Yoyo);
    }

    void SetThePassLineTransform()
    {
        if(unSelectedCoinsPositions.Count > 1)
        {
            for(int i = 0; i < unSelectedCoinsPositions.Count - 1; i++)
            {
                float X_NewScale = Vector3.Magnitude(unSelectedCoinsPositions[i+1] - unSelectedCoinsPositions[i]);
                X_NewScale -= _lineScaleOffset;
                _PassLinePrefab[i].transform.localScale = new Vector3(X_NewScale,1,0.2f);

                float X_NewPos = unSelectedCoinsPositions[i].x + (unSelectedCoinsPositions[i+1].x - unSelectedCoinsPositions[i].x) / 2;
                float Z_NewPos = unSelectedCoinsPositions[i].z + (unSelectedCoinsPositions[i+1].z - unSelectedCoinsPositions[i].z) / 2;
                _PassLinePrefab[i].transform.position = new Vector3(X_NewPos,1,Z_NewPos);

                float Y_NewRotation = Mathf.Atan2((unSelectedCoinsPositions[i].x - unSelectedCoinsPositions[i+1].x),
                                                    (unSelectedCoinsPositions[i].z - unSelectedCoinsPositions[1].z)) * 180 / Mathf.PI;
                _PassLinePrefab[i].transform.rotation = Quaternion.Euler(0,Y_NewRotation + 90,0);
            }

        }
                
    }

    void ResetUnselectedCoins()
    {
        unSelectedCoinsPositions.Clear();
    }
    
    void CalculateThePowerMultiplier()
    {
        maxPowerVector = new Vector2(200,200);
        powerMultiplier = targetVector.magnitude / maxPowerVector.magnitude;
        
        if(targetVector.magnitude > maxPowerVector.magnitude)
            powerMultiplier = 1;
    }

    void ThrowTheSelectedCoin()
    {
        if(SelectedCoin != null)
            SelectedCoin.MoveTo(targetVector.normalized);
    }

    void AllCoinsGoToPreviousPosition()
    {
        foreach (var coin in coins)
        {
            coin.GoToPreviousPosition();
        }
    }

    void AllCoinsGoToStartPosition()
    {
        foreach (var coin in coins)
        {
            coin.GotoStartPosition();
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
        EventManager.OnCoinSelect -= FindUnselectedCoins;
        EventManager.OnCoinSelect -= ShowLineRenderer;
        EventManager.OnCoinSelect -= DrawLineBetweenUnselectedCoins;
        EventManager.OnCoinSelect -= SetThePassLineTransform;
        EventManager.OnCoinSelect -= LineColorChange;
        EventManager.OnPrepareToThrow -= CalculateThePowerMultiplier;
        EventManager.OnThrow -= ThrowTheSelectedCoin;
        EventManager.OnThrow -= DisappearLineRenderer;
        EventManager.OnThrow -= ResetUnselectedCoins;
        EventManager.OnPassFail -= AllCoinsGoToPreviousPosition;
        EventManager.OnRestartLevel -= AllCoinsGoToStartPosition;
        EventManager.OnNextLevel -= AllCoinsGoToStartPosition;
    }
}

