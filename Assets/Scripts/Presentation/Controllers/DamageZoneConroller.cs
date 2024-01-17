using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Zenject;

public class DamageZoneConroller : MonoBehaviour, IZoneController
{
    [Inject] private IEventBus _eventBus;
    [Inject] private ILevelInfo _levelInfo;
    private const int TRANSITION_TIME = 10;
    private const float VISUAL_KOEF = 0.3f;
    public int MinDamage = 100;
    //public int MinZoneSize = 30;
    public int TransitionsCount = 3;
    [SerializeField] private List<DamageZoneModel> _damageZones;
    private Transform _visual;
    public float StartSize;
    private Vector3 _startPos;
    private float _currentSize;
    private int _currentPos=0;
    private void Awake()
    {
        var levelParams = _levelInfo.GetLevelParams();
        if (levelParams != null)
        {
            StartSize = levelParams.MaxZoneSize;
            //MinZoneSize = StartSize / 3;
            GenerateZones(StartSize, TransitionsCount, levelParams.ZoneChangeTime);
        }

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
                DOTween.To(() => currentValue, x => currentValue = x, _damageZones[position].Size, timeTransition).OnUpdate(() =>
                {
                    SetSize(currentValue);
                })
                    .OnComplete(() => _eventBus.NotifyObservers(GameEventType.ZONE_CHANGED));
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

    /*private void DamageHunters()
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
    }*/

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
        var effective = size * VISUAL_KOEF;
        _visual.transform.localScale = new Vector3(effective, 1, effective);
    }

    //public float Size => _currentSize;
    public bool IsDanger(Vector3 position)
    {
        return Vector3.Distance(_startPos, position) > _currentSize;
    }

    private void OnDestroy()
    {
        Stop();
    }

    public void Stop()
    {
        DOTween.KillAll(false);
        gameObject.SetActive(false);
    }

    public float GetSize()
    {
        return _currentSize;
    }

    private void GenerateZones(float startSize, int zonesCount, int transitionTime)
    {
        _damageZones = new List<DamageZoneModel>();
        var zonePart = startSize / zonesCount;
        for (int i=1; i<zonesCount; i++)
        {
            _damageZones.Add(new DamageZoneModel(transitionTime, startSize - i * zonePart));
        }
        //float zoneTime = startParams.TimeSeconds / TransitionsCount;
        //float sizeStep = (startParams.Size - MinZoneSize) / TransitionsCount;
        //for (int i=0; i<TransitionsCount; i++)
            
    }
}
