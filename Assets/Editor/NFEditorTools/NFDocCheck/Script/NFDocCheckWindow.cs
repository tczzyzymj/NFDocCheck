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


    private NFDocCheckDrawBase mCurrentDrawer = null;


    private NFDocCheckDrawForConfig mConfigDrawer = null;


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


    public void ShowMainCatalog()
    {
        mCurrentDrawer = mMainCatalogDrawer;
    }


    private void OnGUI()
    {
        mCurrentDrawer?.Draw();
    }
}
