using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StartBounty : MonoBehaviour
{
    [SerializeField]
    private Image activeBountyImage;
    [SerializeField]
    private TMP_Text activeBountyName;
    [SerializeField]
    private BountyProgressHandler bph;

    public void Begin(string bountyId)
    {
        Bounty selectedBounty = SetBounty.bounties.FirstOrDefault(n => n.bountyTargetId == bountyId);
        PropProperties selectedProp = PropSpriteGenerator.propProperties.FirstOrDefault(n => n.id == bountyId);
        bph.SetProgressBar();
        activeBountyImage.sprite = selectedProp.sprite;
        activeBountyName.text = selectedBounty.bountyName;
        activeBountyImage.transform.parent.GetComponent<CanvasGroup>().alpha = 1;
    }
}
