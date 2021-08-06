using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingltonMap 
{
    public bool isPlaying = false;
    public List<AIStats> m_MonsterAIStats;
    public Vector3 m_LeftDown;
    public Vector3 m_RightUp;
    public bool isFinishThis = false;

    public void Init(Vector3 parent)
    {
        m_LeftDown = parent + new Vector3(-9, -5, 0);
        m_RightUp = parent + new Vector3(13, 11, 0);
    }

    public bool IsInside(Vector3 target)
    {
        if(target.x > m_LeftDown.x && target.x < m_RightUp.x)
        {
            if(target.y > m_LeftDown.y && target.y < m_RightUp.y)
            {
                return true;
            }
        }
        return false;
    }

    // Update is called once per frame
    public void Update()
    {
        if (isFinishThis)
        {
            return;
        }
        if (!isPlaying)
        {
            if (IsInside(GameManager.Instance.player.transform.position))
            {
                MessageServer.Broadcast(EventType.CloseDoor);
                isPlaying = true;
                StartGame();
            }
        }
    }

    public void StopGame()
    {
        foreach(AIStats item in m_MonsterAIStats)
        {
            item.StopGame();
        }
    }

    public void ContinuetGame()
    {
        foreach (AIStats item in m_MonsterAIStats)
        {
            item.ContinueGame();
        }
    }

    public void StartGame()
    {
        MessageServer.AddListener<int>(EventType.EnemyDeath, DeadEnemy);
        MessageServer.AddListener(EventType.ContinueGame, ContinuetGame);
        MessageServer.AddListener(EventType.StopGame, StopGame);
        MessageServer.AddListener(EventType.EndGame, StopGame);
        foreach (AIStats item in m_MonsterAIStats)
        {
            item.StartGame();
        }
    }

    public void NextLevel()
    {
        foreach(AIStats item in m_MonsterAIStats)
        {
            if(item != null)
               item.Death();
        }
        m_MonsterAIStats.Clear();
        isPlaying = false;
        isFinishThis = false;
    }

    public void DeadEnemy(int id)
    {
        for(int i = 0; i < m_MonsterAIStats.Count; i++)
        {
            if(m_MonsterAIStats[i].ID == id)
            {
                m_MonsterAIStats.Remove(m_MonsterAIStats[i]);
                break;
            }
        }
        if (IsFinishThisLevel())
        {
            MessageServer.Broadcast(EventType.OpenDoor);
            MessageServer.RemoveListener<int>(EventType.EnemyDeath, DeadEnemy);
            MessageServer.RemoveListener(EventType.ContinueGame, ContinuetGame);
            MessageServer.RemoveListener(EventType.StopGame,StopGame);
            MessageServer.RemoveListener(EventType.EndGame, StopGame);
            isPlaying = false;
            isFinishThis = true;
        }
    }

    public bool IsFinishThisLevel()
    {
        if(m_MonsterAIStats.Count == 0)
        {
            return true;
        }
        for(int i = 0; i < m_MonsterAIStats.Count; i++)
        {
            if(m_MonsterAIStats[i] != null)
            {
                return false;
            }
        }
        return true;
    }

}
