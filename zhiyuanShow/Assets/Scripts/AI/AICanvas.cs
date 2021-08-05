using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AICanvas : MonoBehaviour
{
    AIStats aIStats;

    [Header("Elements")]
    public Image hpBar;

    public void Init(AIStats m_stats)
    {
        aIStats = m_stats;
    }

    //Method for updating the UI
    public void UpdateUI()
    {
        hpBar.fillAmount = aIStats.HP.current / aIStats.HP.max;
    }

}

