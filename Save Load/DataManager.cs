using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DataManager : SingletonMono<DataManager>
{
    //监听传送点的广播
    [Header("事件监听")]
    public VoidEvent_SO saveDataEvent;
    public VoidEvent_SO loadDataEvent;

    //public static DataManager instance;

    private List<ISaveable> saveableList = new List<ISaveable>();
    private Data saveData;
    protected override void Awake() 
    {
        base.Awake();
        saveData = new Data();
    } 
    
    private void OnEnable() 
    {
        saveDataEvent.OnEventRaised += Save;
        loadDataEvent.OnEventRaised += Load;
    }
    private void OnDisable() 
    {
        saveDataEvent.OnEventRaised -= Save;
        loadDataEvent.OnEventRaised -= Load;
    }
    private void Update() 
    {
        if(Keyboard.current.lKey.wasPressedThisFrame)
            Load();

    }
    //观察者模式实现统一存储与加载        
    public void RegisterSaveData(ISaveable saveable)
    {
        //避免反复加载
        if(!saveableList.Contains(saveable))
            saveableList.Add(saveable);

    }
    public void UnRegisterSaveData(ISaveable saveable)
    {
        saveableList.Remove(saveable);
    }
    public void Save()
    {
        foreach(var saveable in saveableList)
        {
            saveable.GetSaveData(saveData);
        }
        //打印输出检查
        Debug.Log("save");
        // foreach(var item in saveData.characterPosDict)
        // {
        //   Debug.Log(item.Key+"   "+ item.Value);
        // }
    }
    public void Load()
    {
        foreach(var saveable in saveableList)
        {
            saveable.LoadData(saveData);
        }
        Debug.Log("load");
    }
}
