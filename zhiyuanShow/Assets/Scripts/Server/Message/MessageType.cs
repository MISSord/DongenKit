using UnityEngine;

public enum EventType 
{
    PlayerHpDown,//玩家扣掉的血量
    PlayerHpUp,
    MonsterHpDown,//哪个怪物，怪物扣掉的血量
    MonsterHpUp,

    InitGame,//初始化游戏（游戏加载）
    StartGame,//开始游戏，指游戏初始化后
    StopGame,//暂停游戏
    ContinueGame,//指暂停后继续游戏
    EndGame,//游戏结束，指玩家死亡
    GameFinish,//游戏完成，指玩家打通关
    LoadGame,

    TakeDamage,
    EnemyDeath,
    PlayDamge,

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

    AddGun,

    SaveInfor,
    ReadInfor,
}

public enum ReturnMessageType
{
    GetGameObject,
    GetUIObject,
    GetMonsterInfor,
    GetShopInfor,
    GetSprite,
    GetInfor,
}

public enum InteractionShowType
{
    Shop,
    NextDoor,

}

public delegate void CallBack();
public delegate void CallBack<T>(T arg);
public delegate T ReturnCallBack<T>();
public delegate T ReturnCallBack<T, Z>(Z arg1);
public delegate T ReturnCallBack<T,Z,F>(Z arg1, F arg2);
public delegate void CallBack<T, X>(T arg1, X arg2);
public delegate void CallBack<T, X, Y>(T arg1, X arg2, Y arg3);
public delegate void CallBack<T, X, Y, Z>(T arg1, X arg2, Y arg3, Z arg4);
public delegate void CallBack<T, X, Y, Z, W>(T arg1, X arg2, Y arg3, Z arg4, W arg5);
