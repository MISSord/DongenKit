using UnityEngine;

public enum EventType 
{
    PlayerHpDown,//玩家扣掉的血量
    PlayerHpUp,
    MonsterHpDown,//哪个怪物，怪物扣掉的血量
    MonsterHpUp,

    InitGame,
    StartGame,
    StopGame,//暂停游戏
    ContinueGame,//指暂停后继续游戏
    EndGame,



    GetCoin,
    Buy,
    UseHpDownProps,

    ShowShopUI,
    CloseShopUI,
    ShowDamage,
    ShowDialog,
    CloseDialog,


    UpdateUI,

    NextLevel,
    LevelComplete,

    PlayMusicOrBG,//音乐文件地址，是否循环

    InitManagerDic,//初始化字典中管理器
    ClearDic,//清理字典中的管理器

    AddManager,
    ShowInteractionKey,
    CloseInteractionKey,

    PushToPool,
    GetAndSetGameObject,

    OpenDoor,
    CloseDoor,

    FinishSceneLoad,
}

public enum ReturnMessageType
{
    GetGameObject,
    GetUIObject,
    GetMonsterInfor,
}

public enum InteractionShowType
{
    Shop,
    NextDoor,

}

public delegate void CallBack();
public delegate void CallBack<T>(T arg);
public delegate T ReturnCallBack<T>();
public delegate T ReturnCallBack<T,Z,F>(Z arg1, F arg2);
public delegate void CallBack<T, X>(T arg1, X arg2);
public delegate void CallBack<T, X, Y>(T arg1, X arg2, Y arg3);
public delegate void CallBack<T, X, Y, Z>(T arg1, X arg2, Y arg3, Z arg4);
public delegate void CallBack<T, X, Y, Z, W>(T arg1, X arg2, Y arg3, Z arg4, W arg5);
