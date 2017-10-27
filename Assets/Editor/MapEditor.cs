using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (GridBoardWithQuads))]
public class MapEditor : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GridBoardWithQuads battleField = target as GridBoardWithQuads;

       // battleField.GenerateBoard();
    }
}
