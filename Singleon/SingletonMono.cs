using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//泛型  约束
public class SingletonMono<T> : MonoBehaviour where T : SingletonMono<T>
{
    //声明一个类型为T的静态变量，T可以被任何类型代替例如GameManager
     private static T minstance;
     //声明一个类型的属性来包装Instancce,使得可以让外部访问到他
    public static T Instance 
    {
        get { return minstance;}
    }

    protected virtual void Awake()//声明一个只能被 继承类访问的虚方法
    {
        if (minstance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            minstance = (T)this;
        }
        //DontDestroyOnLoad(gameObject);
    }

    //继承且可重写的虚函数
    // protected virtual void Awake()
    // {
    //     OnInit();
    // }
    // public virtual void OnInit()
    // {
    //     //DontDestroyOnLoad(this);
    // }
}
