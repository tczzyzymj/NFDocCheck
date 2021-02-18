using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorTools
{
    [MenuItem("Nine Force Tools/DocChecker Config window")]
    public static void ShowDocCheckConfigWindow()
    {
        NFDocCheckConfigWindow.Showwindow();
    }
}
