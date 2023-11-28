using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Strike : MonoBehaviour
{

	[SerializeField] private Image strikeBar;

    private bool activated = false;

	public void Activate()
	{
		activated = true;
		strikeBar.enabled = false;
	}

	public bool GetActivatedStatus()
	{
		return activated;
	}
}
