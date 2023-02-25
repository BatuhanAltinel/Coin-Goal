using System.Collections;
using UnityEngine;

public class Coin : MonoBehaviour
{
    Rigidbody _rb;
    LineRenderer _lr;
    Color _normalColor;
    [SerializeField] float _powerMeter = 3f;
    Vector3 _previousPosition;
    Vector3 _startPosition;
    [SerializeField] float _maxPower = 750f;

    void OnEnable()
    {
        EventManager.OnCoinSelect += CoinColorChange;
        EventManager.OnCoinSelect += CoinNormalColor;
        EventManager.OnCoinSelect += PreviousPosition;
        EventManager.OnPrepareToThrow += SetTheArrow;
        EventManager.OnCoinSelect += ResetCoinForces;
        EventManager.OnThrow += RemoveArrow;
        EventManager.OnThrowEnd += CoinNormalColor;
    }
    
    void Start()
    {
        _previousPosition = transform.position;
        _startPosition = transform.position;

        _normalColor = gameObject.GetComponent<MeshRenderer>().material.color;
        
        _rb = GetComponent<Rigidbody>();    
        _lr = GetComponent<LineRenderer>();
    }

    public void GotoStartPosition()
    {
        ResetCoinForces();
        transform.position = _startPosition;
    }

    void PreviousPosition()
    {
        _previousPosition = transform.position;
    }

    public void GoToPreviousPosition()
    {
        ResetCoinForces();
        StartCoroutine(GoToPreviousPositionRoutine());
    }

    IEnumerator GoToPreviousPositionRoutine()
    {
        this.GetComponent<MeshCollider>().enabled = false;

        while(Vector3.Distance(transform.position,_previousPosition) > 0.05f)
        {
            GameManager.Instance.CanMove = false;
            this.transform.position = Vector3.Lerp(this.transform.position,_previousPosition,0.1f);
            yield return new WaitForEndOfFrame();
        }

        transform.position = _previousPosition;
        this.GetComponent<MeshCollider>().enabled = true;
        GameManager.Instance.CanMove = true;

        yield break;
    }

    void ResetCoinForces()
    {
        _rb.velocity = new Vector3(0f,0f,0f);                           // Reset the rigidbody forces
        transform.rotation = Quaternion.Euler(new Vector3(0f,0f,0f)); 
        _rb.angularVelocity = new Vector3(0f,0f,0f);
    }

    public void MoveTo(Vector2 dir)
    {
        Vector3 targetVector = new Vector3(dir.x,transform.position.y,dir.y);
        _rb.AddForce(-targetVector * _maxPower * CoinManager.Instance.PowerMultiplier * Time.deltaTime ,ForceMode.Impulse);
    }
    
    public void MoveTo(Vector2 dir,float shootPower)
    {
        Vector3 targetVector = new Vector3(dir.x,transform.position.y,dir.y);
        _rb.AddForce(-targetVector * shootPower * Time.deltaTime ,ForceMode.Impulse);
    }

    void SetTheArrow()
    {
        if(CoinManager.Instance.SelectedCoin == this)
        {
            _lr.enabled = true;
            _lr.positionCount = 2;
            _lr.SetPosition(0,Vector3.zero);
            Vector3 newPos = new Vector3(CoinManager.Instance.targetVector.x,0,CoinManager.Instance.targetVector.y);

            _lr.SetPosition(1,-newPos.normalized * _powerMeter * CoinManager.Instance.PowerMultiplier);
        }
        
    }

    void RemoveArrow()
    {
        _lr.enabled = false;
    }

    void CoinColorChange()
    {
        if(CoinManager.Instance.SelectedCoin == this)
            gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
    }
    
    void CoinNormalColor()
    {
        if(CoinManager.Instance.SelectedCoin != this)
        {
            gameObject.GetComponent<MeshRenderer>().material.color = _normalColor;
        }
        
    }
    void OnDisable()
    {
        EventManager.OnCoinSelect -= CoinColorChange;
        EventManager.OnCoinSelect -= CoinNormalColor;
        EventManager.OnCoinSelect -= PreviousPosition;
        EventManager.OnPrepareToThrow -= SetTheArrow;
        EventManager.OnCoinSelect -= ResetCoinForces;
        EventManager.OnThrow -= RemoveArrow;
        EventManager.OnThrowEnd -= CoinNormalColor;
    }
}
