﻿using System;
using UnityEditor;
using UnityEngine;

public class BaseScriptableObject : ScriptableObject
{
    [ObjectId]
    [SerializeField]
    private string _guid;

    public string GUID => _guid;
}