﻿using System.Collections.Generic;
using Team11.Interactions;
using UnityEngine;

public class LevelWindowData : ScriptableObject
{
    public List<PlaceableObject> prefabs;
    public int selectedPrefabIndex;
    public bool orientToNormal;
}