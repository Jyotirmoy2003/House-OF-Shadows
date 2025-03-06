using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
namespace Jy_Util{
public class Jy_Mono : MonoBehaviour
{
    public static Jy_Mono instance;
    NoArgumentFun noArgumentFun,noArgumentFun1,noArgumentFun2,noArgumentFun3;
    void Awake()
    {
        instance = this;
    }


    public void Delay(NoArgumentFun fun,float time)
    {
        noArgumentFun=fun;
        Invoke(nameof(Execute),time);
    }
    public void Delay1(NoArgumentFun fun,float time)
    {
        noArgumentFun1=fun;
        Invoke(nameof(Execute1),time);
    }

    public void Delay2(NoArgumentFun fun,float time)
    {
        noArgumentFun2=fun;
        Invoke(nameof(Execute2),time);
    }

    public void Delay3(NoArgumentFun fun,float time)
    {
        noArgumentFun3=fun;
        Invoke(nameof(Execute3),time);
    }


    void Execute()=>noArgumentFun();
    void Execute1()=>noArgumentFun1();
    void Execute2()=>noArgumentFun2();
    void Execute3()=>noArgumentFun3();
}
public class util{

public void NullFun()
{

}
}



public delegate void NoArgumentFun();
public enum E_InputType{
    None,
    Gamepad,
    Keyboard,
}
public enum E_ActionMap{
    Game,
    UI,
}
public enum E_DamageType{
    Head,
    Body,
    Leg,
}

public enum E_button_Transitiontype{
    Color,
    Image,
}
public enum E_GameMap{
    Desert_Secrets,
    Urban_Pursuit,

    The_Underground_Facility,
    The_Mountain_Fortress,

}
[System.Serializable]
public struct Pair_Map_Playable
{
    public E_GameMap e_GameMap;
    public PlayableDirector playableDirector;
}
}