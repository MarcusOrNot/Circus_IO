using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KaufmoSpike: MonoBehaviour, INeedKaufmoMaterial
{
    // public Color Color { set { GetComponentInChildren<Renderer>(true).materials[0].color = value; } }

    public Color Color { set { _mesh.materials[0].color = value; } }

    public Material Material { set { _mesh.sharedMaterial = value; } }

    [SerializeField] private float _maxScaleMultiplierX = 2f;
    [SerializeField] private float _minScaleMultiplierX = 0.01f;

    [SerializeField] private float _maxScaleMultiplierY = 2f;
    [SerializeField] private float _minScaleMultiplierY = 0.01f;

    [SerializeField] private float _maxScaleMultiplierZ = 2f;
    [SerializeField] private float _minScaleMultiplierZ = 0.01f;

    [SerializeField] private float _maxAnimationChangePeriod = 0.5f;
    [SerializeField] private float _minAnimationChangePeriod = 0.1f;

    [SerializeField] private float _timeBetweenAnimationFrames = 0.05f;
    [SerializeField] private int _maxAnimationFramesCount = 250;
    [SerializeField] private int _minAnimationFramesCount = 50;

    //private Vector3 _baseScale = Vector3.zero;
    //private Vector3 _minScale = Vector3.zero;
    private Renderer _mesh = null;
    



    //private List<MeshRenderer> _visualElements = new List<MeshRenderer>();

    private void Awake()
    {
        //_visualElements.AddRange(GetComponentsInChildren<MeshRenderer>(true));
        _mesh = GetComponentInChildren<MeshRenderer>(true);
        
        //_baseScale = _mesh.transform.localScale;
        //_minScale = Vector3.Scale(_baseScale, new Vector3(_minScaleMultiplierX, _minScaleMultiplierY, _minScaleMultiplierZ));
    }

    private void Start()
    {
        
        StartCoroutine(SpikeAnimator());
    }

    private IEnumerator SpikeAnimator()
    {
        Vector3 baseScale = _mesh.transform.localScale;
        Vector3 minScale = Vector3.Scale(baseScale, new Vector3(_minScaleMultiplierX, _minScaleMultiplierY, _minScaleMultiplierZ));
        while (true)
        {
            _mesh.transform.localScale = minScale;
            Vector3 targetScale = Vector3.Scale(baseScale, new Vector3(Random.Range(_minScaleMultiplierX, _maxScaleMultiplierX),
                Random.Range(_minScaleMultiplierY, _maxScaleMultiplierY), Random.Range(_minScaleMultiplierZ, _maxScaleMultiplierZ)))
                - minScale;
            int animationFramesCount = Random.Range(_minAnimationFramesCount, _maxAnimationFramesCount + 1);
            Vector3 step = targetScale / animationFramesCount;
            for (int i = 1; i <= animationFramesCount; i++)
            {
                _mesh.transform.localScale = minScale + step * i;
                yield return new WaitForSeconds(_timeBetweenAnimationFrames);
            }
            for (int i = animationFramesCount; i > 0; i--)
            {
                _mesh.transform.localScale = minScale + step * i;
                yield return new WaitForSeconds(_timeBetweenAnimationFrames);
            }

            yield return new WaitForSeconds(Random.Range(_minAnimationChangePeriod, _maxAnimationChangePeriod));
        }
    }


    /*
    private void OnEnable()
    {
        if (_mesh == null) _mesh = GetComponentInChildren<MeshRenderer>(true);
        if (_mesh != null)
        {
            if (_baseScale == Vector3.zero) _baseScale = _mesh.transform.localScale;
            if (_minScale == Vector3.zero) _minScale = Vector3.Scale(_baseScale, new Vector3(_minScaleMultiplierX, _minScaleMultiplierY, _minScaleMultiplierZ));
            
        }
        
        
        StartCoroutine(SpikeAnimator());
    }
    */
    /*
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    */
    /*
    private IEnumerator SpikeAnimator()
    {        
        while (true)
        {
            _mesh.transform.localScale = _minScale;
            Vector3 targetScale = Vector3.Scale(_baseScale, new Vector3(Random.Range(_minScaleMultiplierX, _maxScaleMultiplierX), 
                Random.Range(_minScaleMultiplierY, _maxScaleMultiplierY), Random.Range(_minScaleMultiplierZ, _maxScaleMultiplierZ)))
                - _minScale;
            int animationFramesCount = Random.Range(_minAnimationFramesCount, _maxAnimationFramesCount + 1);
            Vector3 step = targetScale / animationFramesCount;
            for (int i = 1; i <= animationFramesCount; i++)
            {
                _mesh.transform.localScale = _minScale + step * i;                
                yield return new WaitForSeconds(_timeBetweenAnimationFrames);
            }
            for (int i = animationFramesCount; i > 0; i--)
            {
                _mesh.transform.localScale = _minScale + step * i;
                yield return new WaitForSeconds(_timeBetweenAnimationFrames);
            }

            yield return new WaitForSeconds(Random.Range(_minAnimationChangePeriod, _maxAnimationChangePeriod));
        }
    }
    */
}
