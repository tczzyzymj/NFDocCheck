using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


public class NFDocCheckDrawForMainCatalog : NFDocCheckDrawBase
{
    public override void Draw()
    {
        if (GUILayout.Button("更改总配置"))
        {
            NFDocCheckWindow.Ins.ShowChangeConfig();
        }

        if (GUILayout.Button("浏览文件配置"))
        {
            NFDocCheckWindow.Ins.ShowTotalDoc();
        }
    }
}
