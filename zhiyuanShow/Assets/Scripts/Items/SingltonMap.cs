using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingltonMap : MonoBehaviour
{
    public bool isPlaying = false;
    public List<AIStats> m_MonsterAIStats;

    public Vector3 m_LeftDown;
    public Vector3 m_RightUp;

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

    public bool MonsterBorn(Vector3 target)
    {
        if (target.x > m_LeftDown.x + 2 && target.x < m_RightUp.x - 2)
        {
            if (target.y > m_LeftDown.y + 2 && target.y < m_RightUp.y - 2)
            {
                return true;
            }
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlaying)
        {
            if (IsInside(GameManager.Instance.player.transform.position))
            {
                MessageServer.Broadcast(EventType.CloseDoor);
                isPlaying = true;
            }
        }
    }





}
