using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : BaseManager
{
    public int[,] m_Map;
    public SingltonMapInfor[,] m_MapInfor;
    public int m_MapLength = 0;
    public List<SingltonMap> m_MapLevel;
    public int[,] m_Occupy;
    public Vector3 m_playerBorn;
    public List<GameObject> m_Mapitem;
    public List<GameObject> m_DoorItem;

    protected ClassObjectPool<SingltonMap> m_SingltonMapPool = ObjectManager.Instance.GetOrCreatClassPool<SingltonMap>(30);

    public override void Init()
    {
        
    }

    public void Update()
    {
        for(int i = 0;i < m_MapLevel.Count; i++)
        {
            if(m_MapLevel[i] != null)
            {
                m_MapLevel[i].Update();
            }
        }
    }

    public Vector3 MakeNewMap()
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

        m_Mapitem = new List<GameObject>();
        m_DoorItem = new List<GameObject>();

        InstantiteMap();

        return m_playerBorn;
    }

    public void PushMapToPool()
    {
        foreach(GameObject item in m_Mapitem)
        {
            AssetServer.Instance.PushObjectToPool(item);
        }
        m_Mapitem.Clear();
        foreach(GameObject item in m_DoorItem)
        {
            AssetServer.Instance.PushObjectToPool(item);
        }
        m_DoorItem.Clear();
        foreach(SingltonMap item in m_MapLevel)
        {
            item.NextLevel();
            m_SingltonMapPool.Recycle(item);
        }
        m_MapLevel.Clear();
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
        m_MapLevel = new List<SingltonMap>();

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
            item.transform.position = new Vector3(X * 26f, Y * 20f, 0);
            m_playerBorn = item.transform.Find("Born").gameObject.transform.position;
        }
        else if (m_Map[X, Y] == 4)
        {
            item = MessageServer.Broadcast<GameObject, string, bool>(ReturnMessageType.GetGameObject, BaseData.MapEnd, false);
            SetDoorOrWall(m_MapInfor[X, Y], item.transform);
            item.transform.position = new Vector3(X * 26f, Y * 20f, 0);
        }
        else
        {
            item = MessageServer.Broadcast<GameObject, string, bool>(ReturnMessageType.GetGameObject, BaseData.MapOne, false);
            SetDoorOrWall(m_MapInfor[X, Y], item.transform);
            item.transform.position = new Vector3(X * 26f, Y * 20f, 0);
            SingltonMap singltonMap = m_SingltonMapPool.Spawn(true);
            singltonMap.m_MonsterAIStats = new List<AIStats>();
            singltonMap.Init(item.transform.position);
            m_MapLevel.Add(singltonMap);
            m_MapInfor[X, Y].singltonMonster = MessageServer.Broadcast<SingltonMonsterInfor>(ReturnMessageType.GetMonsterInfor);
            InstanceSingltMonster(singltonMap, m_MapInfor[X, Y].singltonMonster, item.transform);
            ReSetZero();
        }
        m_Mapitem.Add(item);
    }

    public void SetDoorOrWall(SingltonMapInfor map, Transform item)
    {
        if (map.m_isDoor[0])
        {
            SetDoor(new Vector3(2f, 11.5f, 0), item.transform, BaseData.Door1);
        }
        else
        {
            SetDoor(new Vector3(1.4f, 11.5f, 0), item.transform, BaseData.WallUp);
        }
        if (map.m_isDoor[1])
        {
            SetDoor(new Vector3(13.2f, 3f, 0), item.transform, BaseData.Door2);
        }
        else
        {
            SetDoor(new Vector3(13f, 4f, 0), item.transform, BaseData.WallRight);
        }
        if (map.m_isDoor[2])
        {
            SetDoor(new Vector3(2f, -5.5f, 0), item.transform, BaseData.Door3);
        }
        else
        {
            SetDoor(new Vector3(1.4f, -5.5f, 0), item.transform, BaseData.WallDown);
        }
        if (map.m_isDoor[3])
        {
            SetDoor(new Vector3(-9.2f, 3f, 0), item.transform, BaseData.Door4);
        }
        else
        {
            SetDoor(new Vector3(-9f, 4f, 0), item.transform, BaseData.WallLeft);
        }
    }

    public void SetDoor(Vector3 position, Transform parent, string DoorOrWall)
    {
        GameObject Door = MessageServer.Broadcast<GameObject, string, bool>(ReturnMessageType.GetGameObject, DoorOrWall, true);
        if(DoorOrWall.Contains("Door"))
        {
            Door.transform.GetComponent<Door>().Init();
        }
        Door.transform.SetParent(parent);
        Door.transform.localPosition = position;
        m_DoorItem.Add(Door);
    }

    public void InstanceSingltMonster(SingltonMap sing, SingltonMonsterInfor singltonMonster,Transform parent)
    {
        int id = 0;
        for(int a = 0; a < singltonMonster.m_MonsterList.Count; a++)
        {
            int count = singltonMonster.m_MonsterList[a].amount;
            for(int i = 0; i < count; i++)
            { 
                GameObject Monster = MessageServer.Broadcast<GameObject, string, bool>(
                    ReturnMessageType.GetGameObject,BaseData.GetAIName(singltonMonster.m_MonsterList[a].MonsterID), true);
                Monster.transform.GetComponent<AIStats>().Init(singltonMonster.m_MonsterList[a].Hp,id,sing);
                Monster.transform.parent = parent;
                Monster.transform.localPosition = GetTruePosititon();
                sing.m_MonsterAIStats.Add(Monster.transform.GetComponent<AIStats>());
                id++;
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
