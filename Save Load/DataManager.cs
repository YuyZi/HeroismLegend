using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//序列化反序列化
using Newtonsoft.Json;
//input output
using System.IO;

//优先执行      数值越小越优先
[DefaultExecutionOrder(-100)]
public class DataManager : SingletonMono<DataManager>
{
    //监听传送点的广播
    [Header("事件监听")]
    public VoidEvent_SO saveDataEvent;
    public VoidEvent_SO loadDataEvent;

    //public static DataManager instance;

    private List<ISaveable> saveableList = new List<ISaveable>();
    private Data saveData;
    //存储路径
    private string jsonFolder;
    private new void Awake() 
    {
        saveDataEvent = (VoidEvent_SO)Resources.Load("Data_SO/Events_SO/SaveGameDataEvent");
        //print("saveDataEvent:"+saveDataEvent.name);
        loadDataEvent = (VoidEvent_SO)Resources.Load("Data_SO/Events_SO/LoadGameDataEvent");
        //print("loadDataEvent:"+loadDataEvent.name);
        // if(instance ==null)
        // {
        //     instance = this;
        // }
        // else
        // {
        //     Destroy(this.gameObject);
        // }
        //base.Awake();
        saveData = new Data();
        //Unity默认路径
        jsonFolder =Application.persistentDataPath + "/SAVE DATA/";
        //开始游戏读取数据
        ReadSaveData();
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

        var resultPath = jsonFolder + "data.sav";
        //数据转换(序列化)      转换为string类型的数据
        var jsonData = JsonConvert.SerializeObject(saveData);
        //写入文件      不存在则创建路径
        if(!File.Exists(resultPath))
        {
            Directory.CreateDirectory(jsonFolder);
        }
        //路径 内容
        File.WriteAllText(resultPath,jsonData);

        //打印输出检查
        //Debug.Log("save");
        // foreach(var item in saveData.characterPosDict)
        // {
        //   Debug.Log(item.Key+"   "+ item.Value);
        // }
    }
    public void Load()
    {
        //遍历bug
        // foreach(var saveable in saveableList)
        // {
        //     saveable.LoadData(saveData);
        // }
        for(int i=0 ; i < saveableList.Count ; i++)
        {
            ISaveable saveable = saveableList[i];
            saveable.LoadData(saveData);
        }
        //Debug.Log("load");
    }
    //读取数据
    private void ReadSaveData()
    {
        var resultPath = jsonFolder + "data.sav";

        if(File.Exists(resultPath))
        {
            var stringData =  File.ReadAllText(resultPath);
            //                  反序列化为Data文件        类型   文件内容
            var jsonData = JsonConvert.DeserializeObject<Data>(stringData);

            saveData = jsonData;
        }
    }
}
