using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//引用Addressable
using UnityEngine.AddressableAssets;
[CreateAssetMenu( menuName = "Game Scene/GameSceneSO")]
public class GameScene_SO : ScriptableObject
{
    public SceneType sceneType;
    //场景资源
    public AssetReference sceneRefernce;
}
