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
                Debug.LogErrorFormat("����Ϊ{0}�ĵ����Ѵ��ڣ��������ٴ����µ�ʵ��", typeof(T).ToString());
            }
            OnInit();
        }

        public virtual void OnInit()
        {
        }
    }
}
