using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SetBounty : MonoBehaviour
{
    public static Bounty[] bounties;

    public string bountyId;
    public Sprite bountyTargetSprite;
    public string bountyTargetName;
    public float bountyTargetTime;
    public int bountyTargetScore;

    private void Start()
    {
        bounties = GameObject.FindGameObjectWithTag("BountyList").GetComponents<Bounty>();
        SetPaperBounty();
    }


    public void SetPaperBounty()
    {
        PropProperties bountySet = PropSpriteGenerator.propProperties[Random.Range(0, PropSpriteGenerator.propProperties.Count)];
        Bounty selectedBounty = bounties.FirstOrDefault(n => n.bountyTargetId == bountySet.id);

        bountyId = bountySet.id;
        bountyTargetSprite = bountySet.sprite;
        bountyTargetName = selectedBounty.bountyName;
        bountyTargetTime = selectedBounty.bountyTime;
        bountyTargetScore = selectedBounty.bountyScore;
    }

    public static Bounty GetBounty(string bountyId)
    {
        Bounty selectedBounty = bounties.FirstOrDefault(n => n.bountyTargetId == bountyId);
        return new Bounty(bountyId, selectedBounty.bountyName, selectedBounty.bountyTime, selectedBounty.bountyScore);
    }
}
