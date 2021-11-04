#if UNITY_EDITOR

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;
using UnityEditor;

[CustomEditor(typeof(Entity), true)]
[CanEditMultipleObjects]
public class EntityEditor : Editor
{

    Entity entityScript;

    private void OnEnable()
    {
        entityScript = (Entity) target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.LabelField("Networked Variables");
        foreach(string key in entityScript.variables.Keys)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(key);
            object o = entityScript.variables[key];
            switch(o.GetType())
            {
                default:
                    EditorGUILayout.LabelField(o.ToString());
                    break;
            }
            EditorGUILayout.EndHorizontal();
        }
        serializedObject.ApplyModifiedProperties();
    }

}

#endif
