using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if (UNITY_EDITOR)

[CustomEditor(typeof(Mob), true)]
[CanEditMultipleObjects]
public class MobEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Mob mob = (Mob)target;

        if (GUILayout.Button("Attach Client"))
        {
            if (NetworkController.localClient != null)
            {
                mob.TransferClientToMob(NetworkController.localClient);
            }
            else
            {
                Debug.Log("You can only run this in play mode.");
            }
        }

    }

}

#endif
