using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DamageZoneConroller : MonoBehaviour
{
    private const int TRANSITION_TIME = 10;
    public int MinDamage = 100;
    [SerializeField] private List<DamageZoneModel> _damageZones;
    private Transform _visual;
    public int StartSize;
    private Vector3 _startPos;
    private float _currentSize;
    private int _currentPos=0;
    private void Awake()
    {
        _visual = GetComponentInChildren<Transform>();
        //_currentSize = StartSize;
        SetSize(StartSize);
        _startPos = transform.position;
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DamageCoroutine());
        ShowZone(0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Vector3.Distance(_startPos, Level.Instance.GetPlayer().GetPosition()) > _currentSize*3)
        {
            //Debug.Log("char is outside");
            
        }*/
    }

    private void ShowZone(int position, int timeTransition)
    {
        if (position<_damageZones.Count)
        {
            _currentPos = position;
            if (timeTransition == 0)
                SetSize(_damageZones[position].Size);
            else
            {
                float currentValue = _damageZones[position - 1].Size;
                DOTween.To(() => currentValue, x => currentValue=x, _damageZones[position].Size, timeTransition).OnUpdate(() =>
                {
                    SetSize(currentValue);
                });
            }
            StartCoroutine(ZoneCoroutine(_damageZones[position].TimeSeconds));
        }
    }

    private IEnumerator DamageCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(2);
            //DamageHunters();
            DamageBurnables();
        }
    }

    private IEnumerator ZoneCoroutine(int zoneTime)
    {
        yield return new WaitForSeconds(zoneTime);
        ShowZone(_currentPos+1, TRANSITION_TIME);
    }

    private void DamageHunters()
    {
        var hunters = FindObjectsOfType<Hunter>();
        foreach (var hunter in hunters)
        {
            if (IsDanger(hunter.transform.position))
            {
                //Debug.Log("char is outside");
                hunter.AddDamage(Mathf.Max(hunter.Lifes/2, MinDamage));
            }
        }
    }

    private void DamageBurnables()
    {
        var gameObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject gameObject in gameObjects)
        {
            var current = gameObject.GetComponent<IBurnable>();
            if (current != null && IsDanger(gameObject.transform.position))
            {
                current.Burn();
            }
        }
    }

    private void SetSize(float size)
    {
        _currentSize = size;
        _visual.transform.localScale = new Vector3(size, 1, size);
    }

    //public float Size => _currentSize;
    public bool IsDanger(Vector3 position)
    {
        return Vector3.Distance(_startPos, position) > _currentSize * 3.2f;
    }

    private void OnDestroy()
    {
        DOTween.KillAll(false);
    }
}
