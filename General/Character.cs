using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour,ISaveable
{
    [Header("事件监听")]
    public VoidEvent_SO newGameEvent;

    [Header("基本属性")]
    public float maxHealth;
    public float currentHealth;
    public float maxPower;
    public float currentPower;
    public float powerRecoverSpeed;
    [Header("受伤无敌")]
    public float invulnerableDuration;
    [HideInInspector]public  float invulnerableCounter;
    public bool invulnerable;
    //事件 与订阅
    //血条变化时启用事件广播,UI管理组件监听
    public UnityEvent<Character> OnHealthChange;
    public UnityEvent<Transform> OnTakeDamage;
    public UnityEvent OnDie;
    private void OnEnable() 
    {
        newGameEvent.OnEventRaised += NewGame;
        //初始化启用已经编写的实现方法  手动生成与注销
        ISaveable saveable = this;
        saveable.RegisterSaveData();
    }
    private void OnDisable() 
    {
        newGameEvent.OnEventRaised -= NewGame;
        ISaveable saveable = this;
        saveable.UnRegisterSaveData();
    }
    private void NewGame() 
    {
        currentHealth = maxHealth;
        //开始时调用更新血量
        currentPower = maxPower;
        OnHealthChange?.Invoke(this);
    }
    //为了敌人能在开始获得血量
    private void Start()
    {
        currentHealth = maxHealth;
    }
    private void Update() 
    {
        //计时器
        if(invulnerable)
        {
            invulnerableCounter -= Time.deltaTime;
            if(invulnerableCounter<=0)
            {
                invulnerable = false;
            }
        }
        if(currentPower<maxPower)
        {
            //Ceil(float)	进位取整（返回浮点数）CeilToInt(float)	进位取整（返回整数）Floor(float)	退位取整（返回浮点数）
            //print(Time.deltaTime * powerRecoverSpeed);
            currentPower += Time.deltaTime * powerRecoverSpeed;
        }
        else if(currentPower>maxPower)
        {
            currentPower = maxPower;
        }
    }
    private void OnTriggerStay2D(Collider2D other) 
    {
       if(other.CompareTag("Water")) 
       {
            if(currentHealth>0)
            {
                //生命值减少  死亡
                currentHealth =0;
                OnHealthChange?.Invoke(this);
                OnDie?.Invoke();
            }

       }
    }
    //受伤
    public void TakeDamage(Attack attacker)
    {
        //如果无敌则取消计算伤害
        if(invulnerable)
            return;
        if(currentHealth -attacker.damage>0)
        {
            currentHealth -=attacker.damage;
            TriggerInvulnerable();
            //受伤动画      AnimationLayer叠加
            OnTakeDamage?.Invoke(attacker.transform);
        }
        else
        {
            currentHealth = 0;
            //触发死亡
            OnDie?.Invoke();
        }
        //传递Character 到事件中计算百分比
        OnHealthChange?.Invoke(this);

    }
    //受伤后短暂无敌
    private void TriggerInvulnerable()
    {
        if(!invulnerable)
        {
            invulnerable = true;

            invulnerableCounter = invulnerableDuration;

        }
    }
    //滑铲消耗      UI更新
    public void OnSlide(int cost)
    {
        currentPower -= cost;
        OnHealthChange?.Invoke(this);
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
        if(data.characterPosDict.ContainsKey(GetDataID().ID))
        {
            //存储位置
            data.characterPosDict[GetDataID().ID]   = transform.position;
            //存储数值      添加string进行标识
            data.floatSaveDict[GetDataID().ID + "health"] =  currentHealth;
            data.floatSaveDict[GetDataID().ID + "power"] =  currentPower;
        }
        else
        {
            data.characterPosDict.Add(GetDataID().ID,transform.position);
            data.floatSaveDict.Add(GetDataID().ID + "health" , this.currentHealth);
            data.floatSaveDict.Add(GetDataID().ID + "power" , this.currentPower);
        }
    }

    public void LoadData(Data data)
    {
       if(data.characterPosDict.ContainsKey(GetDataID().ID))
        {
            transform.position = data.characterPosDict[GetDataID().ID] ;
            this.currentHealth = data.floatSaveDict[GetDataID().ID + "health"];
            this.currentPower = data.floatSaveDict[GetDataID().ID + "power"];

            //更新UI
            OnHealthChange?.Invoke(this);
        }
    }
}
