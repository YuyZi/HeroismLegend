using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 饿汉式单例模式通用父类
/// </summary>
/// <typeparam name="T"></typeparam>
//添加泛型约束为T必须为其本身或子类
public class SingletonMono<T> : MonoBehaviour where T : SingletonMono<T>
{
        private static T mInstance;
        public static T Instance 
        { 
            get
            {
                if (mInstance == null)
                { 
                    //不能New来创建继承于MonoBehaviour的对象
                    // mInstance = System.Activator.CreateInstance(typeof(T), true) as T;
                    //生成新对象
                    GameObject obj = new GameObject();
                    //命名
                    obj.name = typeof(T).ToString();
                    //创造空对象 给予类型
                    mInstance = obj.AddComponent<T>();
                }
                return mInstance;
            }
        }

        protected virtual void Awake()
        {
            OnInit();
        }

        /// <summary>
        /// 可选初始化函数
        /// </summary>
        protected virtual void OnInit()
        {

        }


}
