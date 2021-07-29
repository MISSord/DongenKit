using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingltonMap : MonoBehaviour
{
    public bool isPlaying = false;
    public List<AIStats> m_MonsterAIStats;
    public Vector3 m_LeftDown;
    public Vector3 m_RightUp;
    public bool isFinishThis = false;

    public void Init(Vector3 parent)
    {
        m_LeftDown = parent + new Vector3(-9, -5, 0);
        m_RightUp = parent + new Vector3(13, 4, 0);
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
    void Update()
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

    public void DeadEnemy(int id)
    {
        m_MonsterAIStats[id] = null;
        Debug.Log(m_MonsterAIStats.Count);
        Debug.Log(id);
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
