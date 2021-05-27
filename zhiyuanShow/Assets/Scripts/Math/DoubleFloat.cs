﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class DoubleFloat
{
    public float current;
    public float max;

    //Constructor for initialization in other scripts
    public DoubleFloat(float currentFloat, float maxFloat)
    {
        current = currentFloat;
        max = maxFloat;
    }

    public float RandomFloat() //Random value of variables
    {
        return UnityEngine.Random.Range(current, max);
    }
}

[Serializable]
public class DoubleInt
{
    public int current;
    public int max;

    //Constructor for initialization in other scripts
    public DoubleInt(int currentInt, int maxInt)
    {
        current = currentInt;
        max = maxInt;
    }
}


