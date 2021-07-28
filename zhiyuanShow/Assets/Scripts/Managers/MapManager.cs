﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : BaseManager
{
    public int[,] m_Map;
    public SingltonMapInfor[,] m_MapInfor;
    public int m_MapLength = 0;
    public AIStats[] m_MonsterStats;
    public SingltonMap[] m_MapLevel;
    public int[,] m_Occupy;

    public void MakeNewMap()
    {
        m_Occupy = new int[20, 7];

        ReSetZero();

        int ia = Random.Range(1, 3);
        switch (ia)
        {
            case 1: m_Map = BaseData.m_Map1;
                break;
            case 2:
                m_Map = BaseData.m_Map2;
                break;
            case 3:
                m_Map = BaseData.m_Map3;
                break;
            default:
                m_Map = BaseData.m_Map1;
                break;
        }

        m_MapLength = (int)Mathf.Sqrt(m_Map.Length);

        m_MapInfor = new SingltonMapInfor[m_MapLength, m_MapLength];

        int a, b = 0;

        BranchNew(out a, out b);

        IsConntect(a, b);
        //除去没有相连的节点
        for (int i = 0; i < m_MapLength; i++)
        {
            for (int j = 0; j < m_MapLength; j++)
            {
                if (m_MapInfor[i, j] == null)
                {
                    continue;
                }

                if (!m_MapInfor[i, j].m_isConect)
                {
                    m_MapInfor[i, j] = null;
                    m_Map[i, j] = 0;
                }
            }
        }

        InstantiteMap();


    }

    public void ReSetZero()
    {
        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                m_Occupy[i, j] = 0;
            }
        }
    }

    //生成分支
    public void BranchNew(out int ab, out int bc)
    {
        ab = 0;
        bc = 0;
        //随机生成节点，赋值给空节点
        for (int i = 0; i < m_MapLength; i++)
        {
            for (int j = 0; j < m_MapLength; j++)
            {
                if (m_Map[i, j] != 0)
                {
                    continue;
                }
                m_Map[i, j] = IsBranch();
            }
        }

        //把新生成的节点图赋值给信息节点图
        for (int i = 0; i < m_MapLength; i++)
        {
            for (int j = 0; j < m_MapLength; j++)
            {
                if (m_Map[i, j] != 0)
                {
                    m_MapInfor[i, j] = new SingltonMapInfor();
                    m_MapInfor[i, j].m_MapID = m_Map[i, j];
                }
                if (m_Map[i, j] == 5)
                {
                    ab = i;
                    bc = j;
                }
            }
        }
    }

    /// <summary>
    /// 遍历数组，采用深度遍历算法
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void IsConntect(int x, int y)
    {

        if (m_MapInfor[x, y] == null)
        {
            return;
        }

        if (m_MapInfor[x, y].m_isConect)
        {
            return;
        }

        m_MapInfor[x, y].m_isConect = true;

        int a = x;
        int b = y;

        if (a - 1 >= 0)
        {
            if (m_MapInfor[a - 1, b] != null)
            {
                m_MapInfor[a, b].m_isDoor[3] = true;
                IsConntect(a - 1, b);
            }
        }

        if (b + 1 < m_MapLength)
        {
            if (m_MapInfor[a, b + 1] != null)
            {
                m_MapInfor[a, b].m_isDoor[0] = true;
                IsConntect(a, b + 1);
            }
        }

        if (a + 1 < m_MapLength)
        {
            if (m_MapInfor[a + 1, b] != null)
            {
                m_MapInfor[a, b].m_isDoor[1] = true;
                IsConntect(a + 1, b);
            }
        }

        if (b - 1 >= 0)
        {
            if (m_MapInfor[a, b - 1] != null)
            {
                m_MapInfor[a, b].m_isDoor[2] = true;
                IsConntect(a, b - 1);
            }
        }

    }

    //按照最终的地图生成地图实例
    public void InstantiteMap()
    {
        int ab = 0;
        for (int i = 0; i < m_MapLength; i++)
        {
            for (int j = 0; j < m_MapLength; j++)
            {
                if (m_Map[i, j] != 0)
                {
                    ab++;
                }
            }
        }
        m_MapLevel = new SingltonMap[ab];

        for (int i = 0; i < m_MapLength; i++)
        {
            for (int j = 0; j < m_MapLength; j++)
            {
                if (m_Map[i, j] != 0)
                {
                    InstantiteSingletonMap(i, j);
                }
            }
        }

        
    }

    public void InstantiteSingletonMap(int X, int Y)
    {
        GameObject item;
        if (m_Map[X, Y] == 5)
        {
            item = MessageServer.Broadcast<GameObject, string, bool>(ReturnMessageType.GetGameObject, BaseData.MapStart, false);
            SetDoorOrWall(m_MapInfor[X, Y], item.transform);
        }
        else if (m_Map[X, Y] == 4)
        {
            item = MessageServer.Broadcast<GameObject, string, bool>(ReturnMessageType.GetGameObject, BaseData.MapEnd, false);
            SetDoorOrWall(m_MapInfor[X, Y], item.transform);
        }
        else
        {
            item = MessageServer.Broadcast<GameObject, string, bool>(ReturnMessageType.GetGameObject, BaseData.MapOne, false);
            SetDoorOrWall(m_MapInfor[X, Y], item.transform);
            SingltonMap singltonMap = new SingltonMap();
            singltonMap.m_MonsterAIStats = new List<AIStats>();
            singltonMap.Init(item.transform.position);
            m_MapInfor[X, Y].singltonMonster = MessageServer.Broadcast<SingltonMonsterInfor>(ReturnMessageType.GetMonsterInfor);
            InstanceSingltMonster(singltonMap, m_MapInfor[X, Y].singltonMonster, item.transform);
            ReSetZero();
        }

        item.transform.position = new Vector3(X * 26f, Y * 13f, 0);
    }

    public void SetDoorOrWall(SingltonMapInfor map, Transform item)
    {
        if (map.m_isDoor[0])
        {
            SetDoor(new Vector3(2f, 4.5f, 0), item.transform, BaseData.DoorX);
        }
        else
        {
            SetDoor(new Vector3(1.5f, 4.5f, 0), item.transform, BaseData.WallUp);
        }
        if (map.m_isDoor[1])
        {
            SetDoor(new Vector3(13.2f, 0f, 0), item.transform, BaseData.DoorY);
        }
        else
        {
            SetDoor(new Vector3(13.5f, 0.5f, 0), item.transform, BaseData.WallRight);
        }
        if (map.m_isDoor[2])
        {
            SetDoor(new Vector3(2f, -5.5f, 0), item.transform, BaseData.DoorX);
        }
        else
        {
            SetDoor(new Vector3(1.5f, -5.5f, 0), item.transform, BaseData.WallDown);
        }
        if (map.m_isDoor[3])
        {
            SetDoor(new Vector3(-9.2f, 0f, 0), item.transform, BaseData.DoorY);
        }
        else
        {
            SetDoor(new Vector3(-10.5f, 0.5f, 0), item.transform, BaseData.WallLeft);
        }
    }

    public void SetDoor(Vector3 position, Transform parent, string DoorOrWall)
    {
        GameObject Door = MessageServer.Broadcast<GameObject, string, bool>(ReturnMessageType.GetGameObject, DoorOrWall, true);
        if(DoorOrWall == BaseData.DoorX || DoorOrWall == BaseData.DoorY)
        {
            Door.transform.GetComponent<Door>().Init();
        }
        Door.transform.SetParent(parent);
        Door.transform.position = position;
    }

    public void InstanceSingltMonster(SingltonMap sing, SingltonMonsterInfor singltonMonster,Transform parent)
    {
        for(int a = 0; a < singltonMonster.m_MonsterList.Count; a++)
        {
            int count = singltonMonster.m_MonsterList[a].amount;
            for(int i = 0; i < count; i++)
            {
                GameObject Monster = MessageServer.Broadcast<GameObject, string, bool>(
                    ReturnMessageType.GetGameObject,BaseData.GetAIName(singltonMonster.m_MonsterList[a].MonsterID), true);
                Monster.transform.GetComponent<AIStats>().Init(singltonMonster.m_MonsterList[a].Hp,a + i);
                Monster.transform.parent = parent;
                Monster.transform.position = GetTruePosititon();
                sing.m_MonsterAIStats.Add(Monster.transform.GetComponent<AIStats>());
            }
        }
    }

    public Vector3 GetTruePosititon()
    {
        int m_X = 0;
        int m_Y = 0;
        GetRangePosition(out m_X, out m_Y);

        return new Vector3(-7 + m_X, - 3 + m_Y, 0);
    }

    public void GetRangePosition(out int X, out int Y)
    {
        Debug.Log("666");
        bool isFind = false;
        X = 0;
        Y = 0;
        while(!isFind)
        {
            X = UnityEngine.Random.Range(0, 20);
            Y = UnityEngine.Random.Range(0, 7);

            if (X - 1 >= 0)
            {
                if (m_Occupy[X - 1, Y] == 1)
                {
                    continue;
                }
            }

            if (Y + 1 < 6)
            {
                if (m_Occupy[X, Y + 1] == 1)
                {
                    continue;
                }
            }

            if (X + 1 < 19)
            {
                if (m_Occupy[X + 1, Y] == 1)
                {
                    continue;
                }
            }

            if (Y - 1 >= 0)
            {
                if (m_Occupy[X, Y - 1] == 1)
                {
                    continue;
                }
            }

            isFind = true;
        }

        m_Occupy[X, Y] = 1;
        if (X - 1 >= 0)
        {
            m_Occupy[X - 1, Y] = 1;
        }

        if (Y + 1 < 6)
        {
            m_Occupy[X, Y + 1] = 1;
        }

        if (X + 1 < 19)
        {
            m_Occupy[X + 1, Y] = 1;
        }

        if (Y - 1 >= 0)
        {
            m_Occupy[X, Y - 1] = 1;
        }
    }



    //是否生成分支
    public int IsBranch()
    {
        return Random.Range(0, 2);
    }
}
