using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Hunter))]
[ExecuteAlways]


public class HunterPrefabEditor : MonoBehaviour
{
    private Hunter _hunter = null;
    private HunterType _prevHunterType = HunterType.HUNTER_BLACK;  

    private Renderer _kaufmoMesh = null;

    private Renderer _bubbleMesh = null;

    private ParticleSystemRenderer _hunterDestroyParticleMesh = null;
    
    [SerializeField] private bool _hunterControlledByAI = true;
    private bool _prevHunterControlledByAIStatus = true;   

    [SerializeField] private bool _recollectMaterials = false;
    [SerializeField] private bool _refindAllElements = false;
    [SerializeField] private bool _refreshAll = false;

    private Dictionary<HunterType, Material> _kaufmoMaterials = new();
    private Dictionary<HunterType, Material> _bubbleMaterials = new();
    private Dictionary<HunterType, Material> _hunterDestroyParticleMaterials = new();   



    private void Start()
    {
        if (Application.IsPlaying(gameObject)) return;    
        
        _hunter = GetComponent<Hunter>();
        CollectHunterElements();        
        CollectMaterialsFromResources();
        AddMaterialsToHunterElements();
    }


    private void Update()
    {
        if (Application.IsPlaying(gameObject)) return;

        if (_recollectMaterials)
        {
            CollectMaterialsFromResources();
            _recollectMaterials = false;
            return;
        }
        
        if (_refindAllElements)
        {
            CollectHunterElements();
            _refindAllElements = false;
            return;
        }

        if ((_hunter != null) && (_hunter.Model.HunterType != _prevHunterType))
        {
            AddMaterialsToHunterElements();
            _prevHunterType = _hunter.Model.HunterType;
            return;
        }

        if (_hunterControlledByAI != _prevHunterControlledByAIStatus)
        {
            if (_hunterControlledByAI)
            {
                if (!TryGetComponent(out AIHunter _)) gameObject.AddComponent<AIHunter>();
                if (TryGetComponent(out PlayerHunter playerHunter)) DestroyImmediate(playerHunter);
            }
            else
            {
                if (!TryGetComponent(out PlayerHunter _)) gameObject.AddComponent<PlayerHunter>();
                if (TryGetComponent(out AIHunter aiHunter)) DestroyImmediate(aiHunter);
            }
            _prevHunterControlledByAIStatus = _hunterControlledByAI;
            return;
        }

        if (_refreshAll)
        {
            CollectMaterialsFromResources();
            CollectHunterElements();
            AddMaterialsToHunterElements();
            _refreshAll = false;
            return;
        }
    }


    private void CollectMaterialsFromResources()
    {    
        _kaufmoMaterials.Clear();
        _bubbleMaterials.Clear();
        _hunterDestroyParticleMaterials.Clear();
        foreach (HunterType hunterType in Enum.GetValues(typeof(HunterType)))
        {
            _kaufmoMaterials.Add(hunterType, null);
            _bubbleMaterials.Add(hunterType, null);
            _hunterDestroyParticleMaterials.Add(hunterType, null);
        }
        Material[] materials = Resources.LoadAll<Material>("");
        foreach (Material material in materials)
        {
            string materialNameInUpper = material.name.ToUpper();
            if (materialNameInUpper.Contains("KAUFMO")) 
            {
                AddMaterialToMaterialsDictionary(ref _kaufmoMaterials, material, materialNameInUpper);
            }
            else if (materialNameInUpper.Contains("BUBBLE") && materialNameInUpper.Contains("HEAD"))
            {
                AddMaterialToMaterialsDictionary(ref _bubbleMaterials, material, materialNameInUpper);
            }
            else if (materialNameInUpper.Contains("HUNTER") && materialNameInUpper.Contains("DESTROY"))
            {
                AddMaterialToMaterialsDictionary(ref _hunterDestroyParticleMaterials, material, materialNameInUpper);
            }
        }   
    }
    private void AddMaterialToMaterialsDictionary(ref Dictionary<HunterType, Material> materials, Material material, string materialNameInUpper)
    {
        if (materialNameInUpper.Contains("BLACK")) materials[HunterType.HUNTER_BLACK] = material;
        else if (materialNameInUpper.Contains("BROWN")) materials[HunterType.HUNTER_BROWN] = material;
        else if (materialNameInUpper.Contains("BLUE")) materials[HunterType.HUNTER_BLUE] = material;
        else if (materialNameInUpper.Contains("RED")) materials[HunterType.HUNTER_RED] = material;
        else if (materialNameInUpper.Contains("GREEN")) materials[HunterType.HUNTER_GREEN] = material;
        else if (materialNameInUpper.Contains("YELLOW")) materials[HunterType.HUNTER_YELLOW] = material;
        else if (materialNameInUpper.Contains("ORANGE")) materials[HunterType.HUNTER_ORANGE] = material;
        else if (materialNameInUpper.Contains("PINK")) materials[HunterType.HUNTER_PINK] = material;
        else if (materialNameInUpper.Contains("VIOLET")) materials[HunterType.HUNTER_VIOLET] = material;
        else if (materialNameInUpper.Contains("SKYCOLOR")) materials[HunterType.HUNTER_SKYCOLOR] = material;
        else if (materialNameInUpper.Contains("GRAY")) materials[HunterType.HUNTER_GRAY] = material;
    }

    private void CollectHunterElements()
    {   
        _kaufmoMesh = GetComponentInChildren<KaufmoForm>()?.GetComponentInChildren<SkinnedMeshRenderer>();
        _bubbleMesh = GetComponentInChildren<BubbleForm>()?.GetComponentInChildren<SkinnedMeshRenderer>();
        _hunterDestroyParticleMesh = GetComponentInChildren<HunterDestroyEffect>()?.GetComponentInChildren<ParticleSystemRenderer>();        
    }

    private void AddMaterialsToHunterElements()
    {
        if (_kaufmoMesh != null) _kaufmoMesh.sharedMaterial = _kaufmoMaterials[_hunter.Model.HunterType];
        if (_bubbleMesh != null)
        {
            Material[] bubbleMeshMaterials = _bubbleMesh.sharedMaterials;
            bubbleMeshMaterials[1] = _bubbleMaterials[_hunter.Model.HunterType];
            _bubbleMesh.sharedMaterials = bubbleMeshMaterials;
        }
        if (_hunterDestroyParticleMesh != null) _hunterDestroyParticleMesh.sharedMaterial = _hunterDestroyParticleMaterials[_hunter.Model.HunterType];
    }

}
