using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;

public class NFDocCheckConfigWindow : EditorWindow
{
    private static NFDocCheckConfigWindow mIns;


    private string mConfigFilePath = string.Empty;


    private NFDocCheckConfig mConfig = null;


    private void Awake()
    {
        // 这里去读取一下
        mConfigFilePath = Path.Combine(
            Application.dataPath,
            string.Format(
                "Editor/{0}/NFDocCheck/Config/Config.asset",
                NFEditorPath.EditorFolderName
            )
        );

        mConfig = AssetDatabase.LoadAssetAtPath<NFDocCheckConfig>(NFEditorHelper.GetAssetDatabasePath(mConfigFilePath));

        if (mConfig == null)
        {
            mConfig = ScriptableObject.CreateInstance<NFDocCheckConfig>();
        }
    }


    public static void Showwindow()
    {
        if (mIns == null)
        {
            mIns = EditorWindow.CreateInstance<NFDocCheckConfigWindow>();
        }

        if (mIns == null)
        {
            return;
        }

        mIns.Show();

        mIns.Focus();
    }


    private void OnGUI()
    {
        // 这里绘制一下设置
        {
            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("选择Doc文件夹", GUILayout.Width(100)))
                {
                    mConfig.DocFolderFullPath = EditorUtility.OpenFolderPanel("选择Doc文件夹", mConfig.DocFolderFullPath, string.Empty);
                }

                EditorGUILayout.LabelField(mConfig.DocFolderFullPath);
            }
            EditorGUILayout.EndHorizontal();

            mConfig.StartRowindex = EditorGUILayout.IntField("表格开始行下标", mConfig.StartRowindex);

            mConfig.StartColIndex = EditorGUILayout.IntField("表格开始列下标", mConfig.StartColIndex);

            mConfig.SplitSymbol = EditorGUILayout.TextField("分割符", mConfig.SplitSymbol);
        }

        if (GUILayout.Button("保存设置"))
        {
            // 这里先检测一下文件夹是否存在
            FileInfo _info = new FileInfo(mConfigFilePath);
            if (!_info.Directory.Exists)
            {
                _info.Directory.Create();
            }

            bool _success = false;

            try
            {
                if (File.Exists(mConfigFilePath))
                {
                    EditorUtility.SetDirty(mConfig);

                    AssetDatabase.SaveAssets();
                }
                else
                {
                    AssetDatabase.CreateAsset(mConfig, NFEditorHelper.GetAssetDatabasePath(mConfigFilePath));
                }

                _success = true;
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }

            if (_success)
            {
                EditorUtility.DisplayDialog(string.Empty, "保存成功", "OK");
            }
        }
    }
}
