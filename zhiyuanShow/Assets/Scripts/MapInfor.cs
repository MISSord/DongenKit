using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapInfor", menuName = "CreateMapInfor", order = 0)]
public class MapInfor : ScriptableObject
{
    //采用地图的id
    public string m_MapID ;
    //怪物的信息
    public List<MonsterInfor> m_MonsterList = new List<MonsterInfor>();

    [System.Serializable]
    public struct MonsterInfor
    {
        public int Hp;
        public string MonsterID;
        public int amount;
    }
}
