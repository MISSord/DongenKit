using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseData 
{
    public static readonly Vector3 gunLocalPosition = new Vector3(0f, -0.02f, 0);
    public static readonly Vector3 gunScale = Vector3.one;
    public static readonly Vector3 MuzzlePosition = new Vector3(0.2f, 0.01f, 0);
    public static readonly Vector3 BulletShellPosition = new Vector3(0.1f, 0.01f, 0);
    public static Vector3 normalScale = new Vector3(0.6f,0.6f, 0.6f);

    public static readonly Vector3 Level0Player = new Vector3(0, 0, 0);
    public static readonly Vector3 Level1Player = new Vector3(0, 0, 0);
    public static Vector3 offset = new Vector3(0, 0, -10);


    public static float bulletMinDamage = 10;
    public static float bulletMaxDamage = 20;

    public static string UI = "UI";
    public static float DamageTime = 1.0f;

    #region 管理器名字
    public static readonly string MainMenuManager = "MainMenuManager";
    public static readonly string AudioManager = "AudioManager";
    public static readonly string SceneManager = "ScenesManager";
    public static readonly string AnimatorManager = "AnimatorManager";
    public static readonly string GameManager = "GameManager";
    public static readonly string UIGameManager = "UIGameManager";
    #endregion

    #region 场景名称
    public static readonly string FirstGameScene = "Lvl_0";
    public static readonly string SecondGameScene = "Lvl_1";
    public static readonly string GameUI = "GameUI";
    public static readonly string MainMenuScene = "MainMenu";
    #endregion

    #region 玩家信息
    public static readonly string Hp = "Saved_HP";
    public static readonly string Money = "Saved_Money";
    public static readonly string Bottles = "Saved_Bottles";
    public static readonly string GameLevel = "Saved_Level";

    #endregion

    #region LevelOneGameObPosition
    public static readonly Vector3 NextLevelDoolPosititon = new Vector3(1.633f, -0.747f, 0);
    public static readonly Vector3 AI_Shop = new Vector3(1.281f, -0.705f, 0);

    #endregion

    #region 游戏场景道具
    public static readonly string NextDoorGameOb = "NextLevelDoor";
    public static readonly string Player = "Player/Player";

    #endregion

    #region
    public static readonly string Bullet = "Player/Bullet/Bullet";
    public static readonly string BulletShell = "Player/Bullet/BulletShell";
    public static readonly string BulletTracer = "Player/Bullet/BulletTracer";
    public static readonly string Effect = "Player/Bullet/Effect";
    public static readonly string Explosion = "Player/Bullet/Explosion";
    public static readonly string Rocket = "Player/Bullet/Rocket";
    public static readonly string RocketExplosion = "Player/Bullet/RocketExplosion";
    public static readonly string Smoke = "Player/Bullet/Smoke";
    #endregion

    #region UI
    public static readonly string HealthPoint = "HealthPoint";
    public static readonly string DamageText = "DamageText";
    #endregion

    #region 音乐
    public static readonly string BattleBG = "Music/CleytonRX - Battle RPG Theme";
    public static readonly string MainMenuBG = "Music/medieval_loop";
    public static readonly string UseItem = "Sound/drinking-and-swallow";
    public static readonly string EnemyDamage = "Sound/EnemyDamage";
    public static readonly string ItemUp = "Sound/item-pickup";
    public static readonly string OpenDoor = "Sound/opening-door-1";
    public static readonly string CoinUp = "Sound/Pickup_Coin69";
    public static readonly string PlayerDamage = "Sound/PlayerDamage";
    #endregion


    public static readonly int[,] m_Map1 = new int[5, 5]{
        {0,0,2,5,0},
        {0,0,2,0,0},
        {0,0,2,0,0},
        {0,0,2,0,0},
        {4,2,2,0,0},
    };

    public static readonly int[,] m_Map2 = new int[5, 5]{
        {5,2,2,0,0},
        {0,0,2,0,0},
        {0,0,2,0,0},
        {0,0,2,0,0},
        {4,2,2,0,0},
    };

    public static readonly int[,] m_Map3 = new int[5, 5]{
        {0,0,2,2,2},
        {0,0,2,0,2},
        {0,0,2,0,5},
        {4,2,2,0,0},
        {0,0,0,0,0},
    };

    public static readonly string MapStart = "Map/A1000";
    public static readonly string MapOne = "Map/A1200";
    public static readonly string MapEnd = "Map/A1201";

    public static readonly string WallUp = "Items/WallUp";
    public static readonly string WallLeft = "Items/WallLeft";
    public static readonly string WallRight = "Items/WallRight";
    public static readonly string WallDown = "Items/WallDown";

    public static readonly string DoorY = "Items/Door2";
    public static readonly string DoorX = "Items/Door";
    //public static readonly string MapOne = "Map/A1200";

    public static string GetAIName(string name)
    {
        switch (name)
        {
            case "AI_Orc":
                return "AI/AI_Orc";
            case "AI_Boss":
                return "AI/AI_Boss";
            case "AI_Mage":
                return "AI/AI_Mage";
        }
        return "AI_Orc";
    }

}
