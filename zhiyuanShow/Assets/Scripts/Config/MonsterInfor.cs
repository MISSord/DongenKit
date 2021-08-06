using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SingltonMonsterInfor", menuName = "CreateSingltonMonsterInfor", order = 2)]
public class SingltonMonsterInfor
{
    [SerializeField]
    //这组怪兽的id
    public string m_MonsterID;

    [SerializeField]
    //怪物的信息
    public List<MonsterInfor> m_MonsterList = new List<MonsterInfor>();

    [System.Serializable]
    public class MonsterInfor
    {
        public int Hp;
        public string MonsterID;
        public int amount;
        public int coin;
    }
}

public class SingltonMapInfor
{
    public int m_MapID = 0;
    public bool m_isConect = false;
    public bool[] m_isDoor = new bool[4] { false, false, false, false };
    public SingltonMonsterInfor singltonMonster;
}

[CreateAssetMenu(fileName = "MonsterInfor", menuName = "CreateMonsterInfor", order = 0)]
public class MonsterInfor : ScriptableObject
{
    [SerializeField]
    public List<SingltonMonsterInfor> m_MonsterInfor = new List<SingltonMonsterInfor>();
}



