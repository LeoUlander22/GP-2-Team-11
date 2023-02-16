using System.Collections.Generic;
using System.Linq;
using Team11.Interactions;
using UnityEditor;
using UnityEngine;

public class LevelWindow : EditorWindow
{
    private LevelWindowData windowData;
    
    private SerializedObject so;
    private SerializedProperty propPrefabs;
    private SerializedProperty propOrientToNormal;

    private PlaceableGroup _placeableGroup;
    private Transform _currentParent;
    private Vector2 _scrollPos;
    
    [MenuItem("Tools/Leveler")]
    private static void CreateWindow() => 
        GetWindow<LevelWindow>("Leveler").Show();

    private void OnEnable()
    {
        if (windowData == null)
        {
            windowData = AssetDatabase.LoadAssetAtPath<LevelWindowData>("Assets/WindowData.asset");
            if(windowData != null)
            {
                InitSo();
                SceneView.duringSceneGui += DuringSceneGUI;
                return;
            }

            windowData = CreateInstance<LevelWindowData>();
            AssetDatabase.CreateAsset(windowData, "Assets/WindowData.asset");
            AssetDatabase.Refresh();
        }

        InitSo();
        SceneView.duringSceneGui += DuringSceneGUI;
    }

    private void OnDisable() => SceneView.duringSceneGui -= DuringSceneGUI;

    private void InitSo()
    {
        so = new SerializedObject(windowData);
        propPrefabs = so.FindProperty(nameof(windowData.prefabs));
        propOrientToNormal = so.FindProperty(nameof(windowData.orientToNormal));
        
        InitGroups();
    }

    private void InitGroups()
    {
        var objs = FindObjectsOfType<PlaceableObject>().ToList();
        _placeableGroup = new PlaceableGroup();
        foreach (var placeableObject in objs)
            _placeableGroup.AddObject(placeableObject);
    }

    private void DuringSceneGUI(SceneView scene)
    {
        var camTf = scene.camera;
        
        if(Event.current.type == EventType.MouseMove)
            scene.Repaint();
        
        //var ray = new Ray(camTf.transform.position, camTf.transform.forward);
        var mousePos = new Vector3(Event.current.mousePosition.x, Screen.height - Event.current.mousePosition.y);   
        var ray = camTf.ScreenPointToRay(mousePos);
        if(Physics.Raycast(ray, out RaycastHit hitInfo, 25))
        {
            Handles.color = Color.black;
            Handles.DrawSolidDisc(hitInfo.point, hitInfo.normal, .2f);
            
            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.C)
            {
                Event.current.Use();
                PlacePrefab(hitInfo);
            }
        }
    }

    private void PlacePrefab(RaycastHit hit)
    {
        if(windowData.prefabs == null) return;
        var prefab = windowData.prefabs[windowData.selectedPrefabIndex];
        if(prefab == null) return;

        var go = (PlaceableObject) PrefabUtility.InstantiatePrefab(prefab);
        go.transform.position = hit.point;
        if (windowData.orientToNormal) 
            go.transform.rotation = Quaternion.LookRotation(hit.normal);
        if(_currentParent != null)
            go.transform.SetParent(_currentParent);
        _placeableGroup.AddObject(go);
        
        Undo.RegisterCreatedObjectUndo(go.gameObject, "spawn prefab");
    }

    private void OnGUI()
    {
        so.Update();
        
        EditorGUILayout.PropertyField(propPrefabs);
        EditorGUILayout.PropertyField(propOrientToNormal);
        _currentParent = (Transform) EditorGUILayout.ObjectField("Parent", _currentParent, typeof(Transform), true);
        EditorGUILayout.LabelField("Choose a Prefab from the List and Press C to Place");
        DrawPrefabSelectors();
        
        _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, GUILayout.ExpandWidth(true));
        DrawObjectsList();
        EditorGUILayout.EndScrollView();

        so.ApplyModifiedProperties();
        Repaint();
    }

    private void DrawPrefabSelectors()
    {
        EditorGUILayout.Space();
        if(windowData.prefabs == null) return;
        using (new EditorGUILayout.HorizontalScope())
        {
            foreach (var prefab in windowData.prefabs)
            {
                if (prefab == null) continue;
                GUIStyle style = new(GUI.skin.GetStyle("Button"));
                if (windowData.selectedPrefabIndex == windowData.prefabs.IndexOf(prefab)) 
                    style.fontStyle = FontStyle.BoldAndItalic;

                if (GUILayout.Button(prefab.name, style))
                    windowData.selectedPrefabIndex = windowData.prefabs.IndexOf(prefab);
            }
        }
    }

    private void DrawObjectsList()
    {
        EditorGUILayout.Space();

        EditorGUILayout.Space();
        DrawObjectsGroup(_placeableGroup.NoParentObjects);
        foreach (var group in _placeableGroup.Groups)
        {
            if(group.Key == null || group.Value.Count == 0) continue;
            
            EditorGUILayout.Space();
            EditorGUILayout.LabelField(group.Key.gameObject.name);
            DrawObjectsGroup(group.Value);
        }
    }

    private void DrawObjectsGroup(List<PlaceableObject> objectList)
    {
        var objToRemove = new List<PlaceableObject>();
        foreach (var obj in objectList)
        {
            if (obj == null)
            {
                objToRemove.Add(obj);
                continue;
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                DrawObjectDetail(obj);
            }
        }

        foreach (var obj in objToRemove)
        {
            _placeableGroup.RemoveObject(obj);
        }
    }

    private static void DrawObjectDetail(PlaceableObject obj)
    {
        GUI.enabled = false;
        EditorGUILayout.ObjectField(obj, obj.GetType());
        GUI.enabled = true;
        if (GUILayout.Button("Delete"))
        {
            Undo.DestroyObjectImmediate(obj.gameObject);
        }
        if (GUILayout.Button("Select")) Selection.activeGameObject = obj.gameObject;
        if (GUILayout.Button("Focus"))
        {
            Selection.activeGameObject = obj.gameObject;
            SceneView.FrameLastActiveSceneView();
        }
    }
}
