using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data 
{   
    //编写需要存储的各种数据

    
    //保存场景
    public string sceneToSave;

    //坐标          使用GUID区分每个角色        Vector3类型无法序列化
    public Dictionary<string , SerializeVector3> characterPosDict = new Dictionary<string, SerializeVector3>();
    //数值 所有Float数值都在这里存储 使用GUID + 名称的方式
    public Dictionary<string , float> floatSaveDict = new Dictionary<string, float>();
    //记录玩家以及宝箱，传送点等物品的状态
    public Dictionary<string , bool> boolSaveDict = new Dictionary<string, bool>();

    //工厂模式      这里实现工作细节 而调用者无需知道这些
    //存储场景
    public void SaveGameScene(GameScene_SO saveScene)
    {
        //序列化场景        Objec -> Json
        sceneToSave =JsonUtility.ToJson(saveScene);
        Debug.Log ("Scene"+sceneToSave);
    }
    //反序列化      
    public GameScene_SO GetSaveScene()
    {
        //创建空的Scriptable 实例
        var newScene = ScriptableObject.CreateInstance<GameScene_SO>();
        //找到Object 对象   将Json反序列化覆盖
        JsonUtility.FromJsonOverwrite(sceneToSave,newScene);
        
        return newScene;
    }

public class SerializeVector3
    {
        public float x,y,z;
        //结构体    用于序列化Vector3
        public SerializeVector3(Vector3 pos)
        {
            this.x = pos.x;
            this.y = pos.y;
            this.z = pos.z;
        }
        //反序列化
        public Vector3 ToVector3()
        {
            return new Vector3(x,y,z);
        }
    }
}
