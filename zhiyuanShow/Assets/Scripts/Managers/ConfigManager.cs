using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System;
using System.IO;
using System.Text;


public class ConfigManager : BaseManager
{
    //private MonsterInfor m_monsterInfor;
    private List<SingltonMonsterInfor> m_SingltonMonster;

    public override void Init()
    {
        m_SingltonMonster = new List<SingltonMonsterInfor>();
        LoadAllMonsterInfo();
        MessageServer.AddListener<SingltonMonsterInfor,int>(ReturnMessageType.GetMonsterInfor, GetRangMonsterInfor);
        MessageServer.AddListener<List<ShopItemInfor>>(ReturnMessageType.GetShopInfor, LoadShopItemInfor);
        MessageServer.AddListener<PlayerInfor>(EventType.SaveInfor, Save);
        MessageServer.AddListener<PlayerInfor>(ReturnMessageType.GetInfor, Load);
        base.Init();
    }

    public void LoadAllMonsterInfo()
    {
        string filePath = BaseData.MonsterInfor;
        if (File.Exists(filePath))
        {
            StreamReader sr = new StreamReader(filePath);
            string jsonStr = sr.ReadToEnd();
            sr.Close();
            //解析JSON.
            JsonData jsonData = JsonMapper.ToObject(jsonStr);
            for (int i = 0; i < jsonData.Count; i++)
            {
                SingltonMonsterInfor ii = JsonMapper.ToObject<SingltonMonsterInfor>(jsonData[i].ToJson());
                List<SingltonMonsterInfor.MonsterInfor> tempList = new List<SingltonMonsterInfor.MonsterInfor>();
                JsonData jd = jsonData[i]["MonsterInfor"];
                for (int j = 0; j < jd.Count; j++)
                {
                    tempList.Add(JsonMapper.ToObject<SingltonMonsterInfor.MonsterInfor>(jd[j].ToJson()));
                }
                ii.m_MonsterList = tempList; //一个数组里面包含对象，每个对象又包含一个数组
                m_SingltonMonster.Add(ii);
            }

        }
        else
        {
            Debug.Log("文件不存在，文件路径" + filePath);
        }
    }

    public List<ShopItemInfor> LoadShopItemInfor()
    {
        string jsonStr = BaseData.ShopInfor;
        List<ShopItemInfor> item = new List<ShopItemInfor>();
        if (File.Exists(jsonStr))
        {
            //StreamReader sr = new StreamReader(jsonStr);
            //string json = sr.ReadToEnd();
            //sr.Close();
            //解析JSON.
            JsonData jsonData = JsonMapper.ToObject(File.ReadAllText(jsonStr, Encoding.GetEncoding("utf-8")));
            for (int i = 0; i < jsonData.Count; i++)
            {
                ShopItemInfor ii = JsonMapper.ToObject<ShopItemInfor>(jsonData[i].ToJson());
                item.Add(ii);
            }
        }
        else
        {
            Debug.Log("文件不存在，文件路径" + jsonStr);
        }
        return item;
    }


    ///
    /// 保存JSON数据到本地的方法
    ///
    /// 要保存的对象
    public void Save(PlayerInfor m_player)
    {
        //打包后Resources文件夹不能存储文件，如需打包后使用自行更换目录
        string filePath = BaseData.inforLoad;
        Debug.Log(filePath);

        //找到当前路径
        FileInfo file = new FileInfo(filePath);
        //判断有没有文件，有则打开文件，，没有创建后打开文件
        StreamWriter sw = file.CreateText();
        //ToJson接口将你的列表类传进去，，并自动转换为string类型
        string json = JsonMapper.ToJson(m_player);
        //将转换好的字符串存进文件，
        sw.WriteLine(json);
        //注意释放资源
        sw.Close();
        sw.Dispose();

    }
    /// 读取保存数据的方法
    ///
    public PlayerInfor Load()
    {
        //调试用的  //Debug.Log(1);
        //TextAsset该类是用来读取配置文件的
        string filePath = BaseData.inforLoad;
        if (!File.Exists(filePath))
        {
            Debug.Log("没有本地文件");
            return null;
        }
        PlayerInfor m_PlayerInfor = JsonMapper.ToObject<PlayerInfor>(File.ReadAllText(filePath, Encoding.GetEncoding("utf-8")));
        if (m_PlayerInfor == null)
        {
            Debug.Log("本地文件读取出错");
            return null;
        }
        return m_PlayerInfor;
            
    }

    public SingltonMonsterInfor GetRangMonsterInfor(int level)
    {
        int a = 0;
        if (level > 0)
        {
            if (level >= m_SingltonMonster.Count)
                level = m_SingltonMonster.Count - 1;
            a = UnityEngine.Random.Range(level - 1, level+1);
        }
        
        return m_SingltonMonster[a];
    }

}
