using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BountyProgressHandler : MonoBehaviour
{
    [SerializeField] private CastingToBountyPaper ctbp;
    [SerializeField] private Image progressBarFill;
    [SerializeField] private Image activeBountyImage;
    
    public bool timerRunning = false;
    private float timeRemaining;
    private Bounty activeBounty;

    private Coroutine timerRoutine;
    

    public void SetProgressBar()
    {
        if (timeRemaining == 0)
        {
            activeBounty = SetBounty.GetBounty(ctbp.activeBountyId);
            timeRemaining = activeBounty.bountyTime;
            progressBarFill.fillAmount = 0;
        }
    }

    public void StartTimer()
    {
        timerRoutine = StartCoroutine(BountyTimer());
    }

    public void StopTimer()
    {
        if (timerRunning)
        {
            timerRunning = false;
            StopCoroutine(timerRoutine);
            timerRoutine = null;
            SendNotification.Notify("Bounty Progress halted. Root Lock yourself to continue!");
        }
    }


    IEnumerator BountyTimer()
    {
        timerRunning = true;
        yield return new WaitForSeconds(1f);
        timeRemaining -= 1;

        float fillAmount = ScaleNumberToRange(activeBounty.bountyTime - timeRemaining, 0f, activeBounty.bountyTime);
        progressBarFill.fillAmount = fillAmount;

        if (timeRemaining > 0)
        {
            timerRoutine = StartCoroutine(BountyTimer());
        } else
        {
            FinishBounty();
        }
    }


    void FinishBounty()
    {
        ctbp.bountyActive = false;
        GlobalScore.score += activeBounty.bountyScore;
        GlobalScore.UpdateBar();
        ctbp.activeBountyId = "";
        activeBounty = null;
        activeBountyImage.transform.parent.GetComponent<CanvasGroup>().alpha = 0;
        timerRunning = false;
        timerRoutine = null;
        timeRemaining = 0;
    }


    public static float ScaleNumberToRange(float number, float lowerLimit, float upperLimit)
    {
        float range = upperLimit - lowerLimit;
        float scaledNumber = (number - lowerLimit) / range;
        return Mathf.Clamp01(scaledNumber);
    }

}
