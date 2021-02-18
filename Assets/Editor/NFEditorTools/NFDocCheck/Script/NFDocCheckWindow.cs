using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;


public class NFDocCheckWindow : EditorWindow
{
    private static NFDocCheckWindow mIns;


    private string mConfigFilePath = string.Empty;


    public string ConfigFilePath
    {
        get
        {
            return mConfigFilePath;
        }
    }


    private NFDocCheckConfig mConfig = null;


    public NFDocCheckConfig DocCheckConfig
    {
        get
        {
            return mConfig;
        }
    }


    /// <summary>
    /// DOC 文件夹的全路径，保存的是一个相对路径
    /// </summary>
    public string DocFolderFullPath
    {
        get;
        set;
    }


    private NFDocCheckDrawBase mCurrentDrawer = null;


    private NFDocCheckDrawForConfig mConfigDrawer = null;


    private NFDocCheckDrawForTotalDoc mTotalDocDraw = null;


    private NFDocCheckDrawForMainCatalog mMainCatalogDrawer = null;


    public static NFDocCheckWindow Ins
    {
        get
        {
            return mIns;
        }
    }


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

        if (mMainCatalogDrawer == null)
        {
            mMainCatalogDrawer = new NFDocCheckDrawForMainCatalog();
        }

        mCurrentDrawer = mMainCatalogDrawer;
    }


    public static void Showwindow()
    {
        if (mIns == null)
        {
            mIns = EditorWindow.CreateInstance<NFDocCheckWindow>();
        }

        if (mIns == null)
        {
            return;
        }

        mIns.Show();

        mIns.Focus();
    }


    public void ShowChangeConfig()
    {
        if (mConfigDrawer == null)
        {
            mConfigDrawer = new NFDocCheckDrawForConfig();
        }

        mCurrentDrawer = mConfigDrawer;
    }


    /// <summary>
    /// 显示所有文件的总配置
    /// </summary>
    public void ShowTotalDocConfig()
    {
        if (mTotalDocDraw == null)
        {
            mTotalDocDraw = new NFDocCheckDrawForTotalDoc();
        }

        mCurrentDrawer = mTotalDocDraw;
    }


    public void ShowMainCatalog()
    {
        mCurrentDrawer = mMainCatalogDrawer;
    }


    private void OnGUI()
    {
        mCurrentDrawer?.Draw();
    }
}
