
//抽象类 虚函数  不写内容仅定义方法     类似接口
//子类继承中必须实现所有的方法
public abstract class BaseState 
{
    //判断当前敌人类型
    protected Enemy currentEnemy;
    public abstract void OnEnter(Enemy enemy);
    public abstract void LogicUpdate();
    //fixed
    public abstract void PhysicsUpdate();
    public abstract void OnExit();
}   
