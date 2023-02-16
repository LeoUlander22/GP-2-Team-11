using System.Collections.Generic;
using Team11.Interactions;
using UnityEngine;

public class PlaceableGroup
{
    public Dictionary<Transform, List<PlaceableObject>> Groups;
    public List<PlaceableObject> NoParentObjects;

    public PlaceableGroup()
    {
        NoParentObjects = new List<PlaceableObject>();
        Groups = new Dictionary<Transform, List<PlaceableObject>>();
    }

    public void AddObject(PlaceableObject obj)
    {
        if (obj.transform.parent == null)
        {
            NoParentObjects.Add(obj);
            return;
        }

        var parent = obj.transform.root;
        if (Groups.ContainsKey(parent))
        {
            Groups[parent].Add(obj);
        }
        else
        {
            Groups.Add(parent, new List<PlaceableObject>());
            Groups[parent].Add(obj);
        }
    }

    public void RemoveObject(PlaceableObject obj)
    {
        if (NoParentObjects.Contains(obj))
        {
            NoParentObjects.Remove(obj);
            return;
        }

        foreach (var parent in Groups.Keys)
        {
            if (!Groups[parent].Contains(obj)) continue;
            Groups[parent].Remove(obj);
            return;
        }
    }
}