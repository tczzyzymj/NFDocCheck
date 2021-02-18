using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


public class NFDocCheckDrawForSingleDoc : NFDocCheckDrawBase
{
    public override void Draw()
    {
        if (GUILayout.Button("更改配置"))
        {
            NFDocCheckWindow.Ins.ShowChangeConfig();
        }
    }
}
