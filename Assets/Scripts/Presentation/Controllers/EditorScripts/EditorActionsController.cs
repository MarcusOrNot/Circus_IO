using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorActionsController : MonoBehaviour
{
    private void Awake()
    {
#if !UNITY_EDITOR
    gameObject.SetActive(false);
#endif
    }

    public void ResetPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}



//https://learn.unity.com/tutorial/editor-scripting#