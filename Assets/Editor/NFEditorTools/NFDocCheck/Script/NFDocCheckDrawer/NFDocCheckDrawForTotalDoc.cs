using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


public class NFDocCheckDrawForTotalDoc : NFDocCheckDrawBase
{
    public NFDocCheckDrawForTotalDoc()
    {
        CacheAllFiles();
    }


    private string[] mAllFileArray = null;


    private void CacheAllFiles()
    {
        var _folderPath = NFDocCheckWindow.Ins.DocCheckConfig.DocFolderRelativePath;

        mAllFileArray = Directory.GetFiles(
            _folderPath,
            "(*.xlsx|*.xls)",
            SearchOption.AllDirectories
        );
    }


    public override void Draw()
    {
        EditorGUILayout.BeginHorizontal();

        {
            if (GUILayout.Button("返回主目录"))
            {
                NFDocCheckWindow.Ins.ShowMainCatalog();
            }

            if (GUILayout.Button("刷新文件"))
            {
                CacheAllFiles();
            }
        }

        EditorGUILayout.EndHorizontal();

        // 这里绘制所有的
        if (mAllFileArray == null || mAllFileArray.Length < 1)
        {
            EditorGUILayout.LabelField(
                string.Format(
                    "路径：{0} 下无法找到文件",
                    NFDocCheckWindow.Ins.DocCheckConfig.DocFolderRelativePath
                )
            );
        }
    }
}
