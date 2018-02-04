using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example1 : MonoBehaviour
{
	void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}

    public void OnButtonDown()
    {
        TransitionManager.Instance.LoadLevel("Example2", 1.0f, TransitionManager.TransitionType.Radial180_Bottom, Color.red);
    }
}
