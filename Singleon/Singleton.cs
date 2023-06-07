using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MyFramework
{

    public class Singleton<T> : ISingleton where T : Singleton<T>
    {
        private static T mInstance;

        public static T Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = System.Activator.CreateInstance<T>();
                }
                return mInstance;
            }
        }

        public Singleton()
        {
            if (null != mInstance)
            {
                Debug.LogErrorFormat("类名为{0}的单例已存在，不可以再创建新的实例", typeof(T).ToString());
            }
            OnInit();
        }

        public virtual void OnInit()
        {
        }
    }
}
