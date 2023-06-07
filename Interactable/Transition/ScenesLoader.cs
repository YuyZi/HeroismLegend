using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
public class ScenesLoader : MonoBehaviour,ISaveable
{
    public Transform playerTrans;
    public Vector3 firstPos;
    public Vector3 menuPos;
    [Header("事件监听")]
    public ScenesLoadEvent_SO loadEventSO;
    public VoidEvent_SO newGameEvent;
    public VoidEvent_SO backToMenuEvent;
   [Header("广播")]
   //场景加载完毕后广播
   public VoidEvent_SO afterSceneLoadEvent;
   //淡入淡出
   public FadeEvent_SO fadeEvent;
   public ScenesLoadEvent_SO unloadSceneEvent;
    [Header("场景")]
   public GameScene_SO menuScene;
   public GameScene_SO firstLoadScene;
   private GameScene_SO currentLoadScene;
   //存储变量
   private GameScene_SO sceneToLoad;
   private Vector3 positionToGo;
   private bool isLoading;
   private bool fadeScreen;
   public float fadeDuration;
   private void Awake() 
   {
        //加载场景      对象  模式
        //Addressables.LoadSceneAsync(firstLoadScene.sceneRefernce,LoadSceneMode.Additive);
        // currentLoadScene = firstLoadScene;
        // currentLoadScene.sceneRefernce.LoadSceneAsync(LoadSceneMode.Additive);

   }
   private void OnEnable()
    {
        loadEventSO.LoadRequestEvent += OnLoadRequestEvent;
        newGameEvent.OnEventRaised += NewGame;
         backToMenuEvent.OnEventRaised += OnBackToMenuEvent;
        //FIXME:bug 无法注册        持久化场景放在Start中会覆盖原有数据无法传递数值 maybe编辑器BUG
        ISaveable saveable = this;
        saveable.RegisterSaveData();
   }
   private void OnDisable() 
   {
        loadEventSO.LoadRequestEvent -= OnLoadRequestEvent;
        newGameEvent.OnEventRaised -= NewGame;
        backToMenuEvent.OnEventRaised -= OnBackToMenuEvent;
        ISaveable saveable = this;
        saveable.UnRegisterSaveData();
   }
    private void Start() 
    {
        //加载菜单
        loadEventSO.RaiseLoadRequestEvent(menuScene,menuPos,true);
        //NewGame();
    }
   private void OnBackToMenuEvent()
   {
        sceneToLoad =menuScene;
        loadEventSO.RaiseLoadRequestEvent(sceneToLoad,menuPos,true);
   }
   private void NewGame()
   {
        sceneToLoad = firstLoadScene;
        //OnLoadRequestEvent(sceneToLoad,firstPos,true);
        //主动呼叫，触发监听者的方法
        loadEventSO.RaiseLoadRequestEvent(sceneToLoad,firstPos,true);
   }


   /// <summary>
   /// 场景加载事件请求
   /// </summary>
   /// <param name="locationToLoad"></param>
   /// <param name="posToGo"></param>
   /// <param name="fadeScreen"></param>
   public void OnLoadRequestEvent(GameScene_SO locationToLoad,Vector3 posToGo,bool fadeScreen)
   {
        //避免重复加载
        if(isLoading)
            return;
        isLoading = true;
        sceneToLoad =locationToLoad;
        positionToGo = posToGo;
        this.fadeScreen = fadeScreen; 
        //卸载和加载场景
        if(currentLoadScene!=null)
        {
            StartCoroutine(UnLoadPreviouScene());
        }
        else
        {
            LoadNewScene();
        }

   }
   private IEnumerator  UnLoadPreviouScene()
   {
        //关闭人物
        playerTrans.gameObject.SetActive(false);
        if(fadeScreen)
        {
            //黑幕淡入
            fadeEvent.FadeIn(fadeDuration);
        }
        //等待淡入淡出
        yield return new WaitForSeconds(fadeDuration);
        //广播事件调整血条显示      启用UIManager中的逻辑
        unloadSceneEvent.RaiseLoadRequestEvent(sceneToLoad,positionToGo,true);
        //卸载场景 
        yield return currentLoadScene.sceneRefernce.UnLoadScene();
        //加载场景
        LoadNewScene();
   }
   private void LoadNewScene()
   {
        //临时变量查看类型  AsyncOperationHandle<TObject>   TObject SceneInstance
        var loadingOption = sceneToLoad.sceneRefernce.LoadSceneAsync(LoadSceneMode.Additive,true);
        loadingOption.Completed += OnLoadCompleted;

   }
   /// <summary>
   /// 场景加载完成后
   /// </summary>
   /// <param name="obj"></param>
       private void OnLoadCompleted(AsyncOperationHandle<SceneInstance> obj)
    {
        //更改当前场景
        currentLoadScene = sceneToLoad;
        //移动角色
        playerTrans.position = positionToGo;
        //开启人物
        playerTrans.gameObject.SetActive(true);
        if(fadeScreen)
        {
            //白幕淡出 
            fadeEvent.FadeOut(fadeDuration);
        }

        isLoading = false;
        if(currentLoadScene.sceneType != SceneType.Menu)
            //通知所有场景加载完毕需要执行的内容
            afterSceneLoadEvent.RaiseEvent();
    }

    public DataDefinition GetDataID()
    {
        if(GetComponent<DataDefinition>())
            return GetComponent<DataDefinition>();
        else
        {  
            Debug.Log("you need add DataDefinition in this gameObject"+this.gameObject.name);
            return null;
        }
    }

    public void GetSaveData(Data data)
    {
        //工厂模式  使用方法 不考虑细节 
       data.SaveGameScene(currentLoadScene);
    }

    public void LoadData(Data data)
    {
       //根据保存的玩家数据判断是否有数据
       var playerID = playerTrans.GetComponent<DataDefinition>().ID;
       if(data.characterPosDict.ContainsKey(playerID))
       {
            //避免反复加载位置出现问题
            positionToGo = data.characterPosDict[playerID];
            //在工厂中完成反序列化  读取场景
            sceneToLoad = data.GetSaveScene();
            //加载场景
            OnLoadRequestEvent(sceneToLoad,positionToGo,true);
       }
    }
}
