using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class NFDocCheckScriptableData : ScriptableObject
{
    public NFDocCheckConfigData ConfigData = new NFDocCheckConfigData();


    /// <summary>
    /// key 是文件名，文件的路径放在 config 里面，自行组装就可以了
    /// </summary>
    public Dictionary<string, List<NFDocCheckLogicData>> LogicDataMap =
        new Dictionary<string, List<NFDocCheckLogicData>>();
}
