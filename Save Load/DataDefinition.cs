using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//数据描述定义 生成GUID Globally Unique Identifier
public class DataDefinition : MonoBehaviour
{
    public PersistentType persistentType;
    public string ID;
    private void OnValidate() 
    {
        //如果数据类型为可读可写且ID为空 自动生成ID 否则清空
        if(persistentType ==PersistentType.ReadWrite)
        {
                if(ID == string.Empty)
                ID = System.Guid.NewGuid().ToString();
        }
        else
        {
            ID = string.Empty;
        }
    }
}
