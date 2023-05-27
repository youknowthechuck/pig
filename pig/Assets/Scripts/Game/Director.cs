using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : PigScript
{
    [SerializeField]
    private List<Round> m_rounds = null;

    private int m_currentRound = -1;

    public int CurrentRound
    {
        get { return m_currentRound; }
    }    
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BeginNextRound(bool ignoreCurrentRoundComplete)
    {
        if (IsCurrentRoundComplete() || ignoreCurrentRoundComplete)
        {
            ++m_currentRound;

            if (m_currentRound >= 0 && m_currentRound < m_rounds.Count)
            {
                Round round = m_rounds[m_currentRound];

                round.Begin();
            }
        }
    }

    public bool IsCurrentRoundComplete()
    {
        if (m_currentRound >= 0 && m_currentRound < m_rounds.Count)
        {
            return m_rounds[m_currentRound].IsComplete();
        }
        //invalid rounds are done, i guess (we rely on this to begin round 0 since we start at round -1)
        return true;
    }

    [DebugCommand]
    public void SkipToRound(int round)
    {
        m_currentRound = round;
    }
}
