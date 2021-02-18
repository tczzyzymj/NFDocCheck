using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


public class NFDocCheckDrawForConfig : NFDocCheckDrawBase
{
    public override void Draw()
    {
        // 这里绘制一下设置
        EditorGUILayout.BeginHorizontal();

        {
            if (GUILayout.Button("选择Doc文件夹", GUILayout.Width(100)))
            {
                NFDocCheckWindow.Ins.DocCheckConfig.DocFolderFullPath = EditorUtility.OpenFolderPanel(
                    "选择Doc文件夹",
                    NFDocCheckWindow.Ins.DocCheckConfig.DocFolderFullPath,
                    string.Empty
                );
            }

            EditorGUILayout.LabelField(NFDocCheckWindow.Ins.DocCheckConfig.DocFolderFullPath);
        }

        EditorGUILayout.EndHorizontal();

        NFDocCheckWindow.Ins.DocCheckConfig.StartRowindex = EditorGUILayout.IntField(
            "表格开始行下标",
            NFDocCheckWindow.Ins.DocCheckConfig.StartRowindex
        );

        NFDocCheckWindow.Ins.DocCheckConfig.StartColIndex = EditorGUILayout.IntField(
            "表格开始列下标",
            NFDocCheckWindow.Ins.DocCheckConfig.StartColIndex
        );

        NFDocCheckWindow.Ins.DocCheckConfig.SplitSymbol = EditorGUILayout.TextField(
            "分割符",
            NFDocCheckWindow.Ins.DocCheckConfig.SplitSymbol
        );

        EditorGUILayout.BeginHorizontal();

        {
            if (GUILayout.Button("保存设置"))
            {
                OnClickSaveButton();
            }

            if (GUILayout.Button("返回主目录"))
            {
                OnClickReturnButton();
            }
        }

        EditorGUILayout.EndHorizontal();
    }


    private void OnClickSaveButton()
    {
        // 这里先检测一下文件夹是否存在
        FileInfo _info = new FileInfo(NFDocCheckWindow.Ins.ConfigFilePath);

        if (_info == null || _info.Directory == null)
        {
            Debug.LogError("错误，无法获取路径：" + NFDocCheckWindow.Ins.ConfigFilePath);

            return;
        }

        if (!_info.Directory.Exists)
        {
            _info.Directory.Create();
        }

        bool _success = false;

        try
        {
            if (File.Exists(NFDocCheckWindow.Ins.ConfigFilePath))
            {
                EditorUtility.SetDirty(NFDocCheckWindow.Ins.DocCheckConfig);

                AssetDatabase.SaveAssets();
            }
            else
            {
                AssetDatabase.CreateAsset(
                    NFDocCheckWindow.Ins.DocCheckConfig,
                    NFEditorHelper.GetAssetDatabasePath(NFDocCheckWindow.Ins.ConfigFilePath)
                );
            }

            _success = true;
        }
        catch (Exception _e)
        {
            Debug.LogError(_e.ToString());
        }

        if (_success)
        {
            EditorUtility.DisplayDialog(string.Empty, "保存成功", "OK");
        }
    }


    private void OnClickReturnButton()
    {
        NFDocCheckWindow.Ins.ShowMainCatalog();
    }
}
