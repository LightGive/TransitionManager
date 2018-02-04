using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example1 : MonoBehaviour
{
    [SerializeField]
    private string sceneName;

    public void OnButtonDown()
    {
        TransitionManager.Instance.LoadLevel(sceneName, 1.0f);
    }
}
