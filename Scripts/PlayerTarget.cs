using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class PlayerTarget : Target
{
    public static PlayerTarget Instance;

    protected override void Awake()
    {
        base.Awake();

        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        Instance = this;
    }
}
