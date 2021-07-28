using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class ConfigManager : BaseManager
{
    private MonsterInfor m_monsterInfor;
    private List<SingltonMonsterInfor> m_SingltonMonster;

    public override void Init()
    {
        m_SingltonMonster = new List<SingltonMonsterInfor>();
        LoadAllMonsterInfo();
        MessageServer.AddListener<SingltonMonsterInfor>(ReturnMessageType.GetMonsterInfor, GetRangMonsterInfor);
        
        base.Init();
    }

    public void LoadAllMonsterInfo()
    {
        string filePath = Application.dataPath + "/Config/AllMonster.json";
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

    public SingltonMonsterInfor GetRangMonsterInfor()
    {
        int a = Random.Range(0, m_SingltonMonster.Count);
        return m_SingltonMonster[a];
    }

}
