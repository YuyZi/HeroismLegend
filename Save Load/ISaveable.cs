
using UnityEngine;


//接口标记所有需要存储的物品
public interface ISaveable 
{
    //获取对应角色的唯一ID
    DataDefinition GetDataID();
    //新版本可以写函数实现方法
    //注册到DataManager管理类              =>一行简写
    void RegisterSaveData();
    //=> DataManager.instance.RegisterSaveData(this);
    
    //死亡和场景卸载时注销  
    void UnRegisterSaveData();
    // => DataManager.instance.UnRegisterSaveData(this);
    //作为中介获取需要保存的数据
    void GetSaveData(Data data);
    //加载时通知属性读取
    void LoadData(Data data);


}
