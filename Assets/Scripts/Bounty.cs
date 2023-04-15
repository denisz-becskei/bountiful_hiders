using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounty : MonoBehaviour
{
    public string bountyTargetId;
    public string bountyName;
    public float bountyTime;
    public int bountyScore;

    public Bounty(string bountyTargetId, string bountyName, float bountyTime, int bountyScore)
    {
        this.bountyTargetId = bountyTargetId;
        this.bountyName = bountyName;
        this.bountyTime = bountyTime;
        this.bountyScore = bountyScore;
    }
}
