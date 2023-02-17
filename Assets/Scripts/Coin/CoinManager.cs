using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;
    LineRenderer lineRenderer;
    [SerializeField] List<Coin> coins;
    public Coin selectedCoin;
    public GameObject _arrow;
    public Vector2 moveTargetPos;
    public Vector2 maxPowerVector;
    

    void OnEnable()
    {
        EventManager.OnUnselectedCoins += DrawLineBetweenUnselectedCoins;
        EventManager.onCoinSelect += DisplayArrow;
        EventManager.onCoinSelect += ShowLineRenderer;
        EventManager.onCoinSelect += ArrowPositioning;
        EventManager.OnPrepareToThrow += ArrowRotation;
        EventManager.OnThrow += DisappearLineRenderer;
        EventManager.OnThrow += ThrowTheSelectedCoin;
        EventManager.OnThrow += DisappearArrow;
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
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = coins.Count-1;
    }

    public void SetTheCoinSelected(Coin coin)
    {
        selectedCoin = coin;
    }

    void DrawLineBetweenUnselectedCoins()
    {
        int indx = 0;
        lineRenderer.material = lineRenderer.materials[0];
        foreach (var coin in coins)
        {
            if(selectedCoin != coin)
            {
                lineRenderer.SetPosition(indx,coin.transform.position);
                indx++;
            }   
        }
    }

    void ThrowTheSelectedCoin()
    {
        moveTargetPos = new Vector2((Mathf.Abs(moveTargetPos.x) > maxPowerVector.x ? maxPowerVector.x:moveTargetPos.x),
        (Mathf.Abs(moveTargetPos.y) > maxPowerVector.y ? maxPowerVector.y:moveTargetPos.y));
        if(selectedCoin != null)
            selectedCoin.MoveTo(moveTargetPos);
    }

    void ShowLineRenderer()
    {
        lineRenderer.enabled =true;
    }
    void DisappearLineRenderer()
    {
        lineRenderer.enabled = false;
    }

    void DisplayArrow()
    {
        _arrow.SetActive(true);
    }

    void DisappearArrow()
    {
        _arrow.SetActive(false);
    }
    
    void ArrowPositioning()
    {
        _arrow.transform.position = selectedCoin.transform.position;
    }
    void ArrowRotation()
    {
        float targetRot = Mathf.Sin(Mathf.Sqrt(moveTargetPos.x * moveTargetPos.x + moveTargetPos.y * moveTargetPos.y));
        _arrow.gameObject.transform.rotation = Quaternion.AngleAxis(targetRot*30,Vector3.up);
        // _arrow.gameObject.transform.rotation = Quaternion.Euler(moveTargetPos);
    }
    void OnDisable()
    {
        EventManager.OnUnselectedCoins -= DrawLineBetweenUnselectedCoins;
        EventManager.onCoinSelect -= ShowLineRenderer;
        EventManager.onCoinSelect -= DisplayArrow;
        EventManager.onCoinSelect -= ArrowPositioning;
        EventManager.OnPrepareToThrow -= ArrowRotation;
        EventManager.OnThrow -= ThrowTheSelectedCoin;
        EventManager.OnThrow -= DisappearLineRenderer;
        EventManager.OnThrow += DisappearArrow;
    }
}

