using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KaufmoEye : MonoBehaviour, INeedKaufmoMaterial
{   
    public Material Material { set { _eyeballMesh.sharedMaterial = value; } }

    [SerializeField] private GameObject _eyeball;
    [SerializeField] private GameObject _lids;
    [SerializeField] private GameObject _eyepupil;
    [SerializeField] private GameObject _eyecornea;


    [SerializeField] private float _maxAnimationChangePeriod = 2f;
    [SerializeField] private float _minAnimationChangePeriod = 1f;

    [SerializeField] private int _maxAnimationFramesCount = 100;
    [SerializeField] private int _minAnimationFramesCount = 50;

    
    [SerializeField] private Color[] _lidsColors;
    [SerializeField] private Color[] _eyepupilColors;
    [SerializeField] private Color[] _eyecorneaColors;

    private Renderer _eyeballMesh = null;

   
    private void Awake()
    {
        _eyeballMesh = _eyeball.GetComponentInChildren<Renderer>();
        if (_eyepupilColors.Length > 0) _eyepupil.GetComponentInChildren<Renderer>(true).materials[0].color = _eyepupilColors[Random.Range(0, _eyepupilColors.Length)];
        if (_eyecorneaColors.Length > 0) _eyecornea.GetComponentInChildren<Renderer>(true).materials[0].color = _eyecorneaColors[Random.Range(0, _eyecorneaColors.Length)];
        if (_lidsColors.Length > 0)
        {
            Color randomColor = _lidsColors[Random.Range(0, _lidsColors.Length)];
            Renderer[] lids = _lids.GetComponentsInChildren<Renderer>(true);            
            foreach (Renderer lid in lids) lid.materials[0].color = randomColor;
        }

    }

    private void Start()
    {
        StartCoroutine(EyepupilAnimator());
    }

    private IEnumerator EyepupilAnimator()
    {       
        Vector3 eyepupilStartPosition = _eyepupil.transform.localPosition;       
        while (true)
        {
            float offsetX = 0.07f * _eyepupil.transform.localScale.x;
            float offsetZ = 0.025f * _eyepupil.transform.localScale.z;
            Vector3 currentEyepupilPosition = _eyepupil.transform.localPosition;
            Vector3 newEyepupilPosition = eyepupilStartPosition
                + new Vector3(Random.Range(-offsetX, offsetX), 0, Random.Range(-offsetZ, offsetZ));
            int animationFramesCount = Random.Range(_minAnimationFramesCount, _maxAnimationFramesCount);
            float step = Vector3.Distance(newEyepupilPosition, currentEyepupilPosition) / animationFramesCount;
            for (int i = 1; i <= animationFramesCount; i++)
            {
                _eyepupil.transform.localPosition = Vector3.MoveTowards(currentEyepupilPosition, newEyepupilPosition, step * i);                
                yield return null;
            }
            yield return new WaitForSeconds(Random.Range(_minAnimationChangePeriod, _maxAnimationChangePeriod));
        }
    }    

}
