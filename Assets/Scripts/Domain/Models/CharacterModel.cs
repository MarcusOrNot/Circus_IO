using UnityEngine;

[System.Serializable]

public class CharacterModel
{        
    [Range (1f, 30f)] public float Speed = 10f;
    [Range (100f, 2000f)] public float RotationSpeed = 1000f;    
}
