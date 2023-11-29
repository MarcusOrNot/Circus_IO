using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterEye : MonoBehaviour
{
    public Color Color { set { _eyeball.GetComponentInChildren<Renderer>().materials[0].color = value; } }

    [SerializeField] private GameObject _eyeball;
    [SerializeField] private GameObject _lids;
    [SerializeField] private GameObject _eyepupil;
    [SerializeField] private GameObject _eyecornea;
       
    
    [SerializeField] private Color[] _lidsColors;
    [SerializeField] private Color[] _eyepupilColors;
    [SerializeField] private Color[] _eyecorneaColors;

      
    private void Start()
    {         
        if (_eyepupilColors.Length > 0) _eyepupil.GetComponentInChildren<Renderer>().materials[0].color = _eyepupilColors[Random.Range(0, _eyepupilColors.Length)];   
        if (_eyecorneaColors.Length > 0) _eyecornea.GetComponentInChildren<Renderer>().materials[0].color = _eyecorneaColors[Random.Range(0, _eyecorneaColors.Length)];
        if (_lidsColors.Length > 0)
        {
            Renderer[] lids = _lids.GetComponentsInChildren<Renderer>();
            Color randomColor = _lidsColors[Random.Range(0, _lidsColors.Length)];
            foreach (Renderer lid in lids) lid.materials[0].color = randomColor;
        }
        
    }


   

}
