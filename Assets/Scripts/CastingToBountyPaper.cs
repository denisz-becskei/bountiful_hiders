using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class CastingToBountyPaper : MonoBehaviour
{
    [SerializeField]
    private GameObject highlightedObject;

    [SerializeField]
    private CanvasGroup backgroundUI;

    [Space(10)]
    [SerializeField]
    private Image bountySprite;
    [SerializeField]
    private TMP_Text bountyName;
    [SerializeField]
    private TMP_Text bountyTime;
    [SerializeField]
    private TMP_Text bountyScore;

    [SerializeField]
    private StartBounty sb;

    private RaycastHit objectHit;

    public bool bountyActive = false;
    public string activeBountyId = string.Empty;



    [SerializeField] TransformToObject transformationController;

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out objectHit))
        {
            if (objectHit.transform.gameObject.CompareTag("BountyPaper") && !bountyActive)
            {

                float distance = Vector3.Distance(transform.position, objectHit.transform.position);
                if (distance <= 10.35f)
                {
                    highlightedObject = objectHit.transform.gameObject;
                    highlightedObject.GetComponent<Outline>().enabled = true;
                    UpdateData();
                    StartCoroutine(HoverBackgroundIn());
                }
            }
            else
            {
                if (highlightedObject != null)
                {
                    highlightedObject.GetComponent<Outline>().enabled = false;
                    highlightedObject = null;
                }
                if (backgroundUI.GetComponent<CanvasGroup>().alpha > 0)
                {
                    StartCoroutine(HoverEverythingOut());
                }
                
            }
        }

        if (Input.GetMouseButtonDown(0) && highlightedObject != null && !bountyActive)
        {
            transformationController.Retain();
            StartCoroutine(HoverEverythingOut());
            bountyActive = true;
            activeBountyId = highlightedObject.GetComponent<SetBounty>().bountyId;

            sb.Begin(activeBountyId);

            foreach (SetBounty setBounty in GameObject.FindObjectsOfType<SetBounty>() )
            {
                setBounty.SetPaperBounty();
            }
            highlightedObject.GetComponent<Outline>().enabled = false;
            highlightedObject = null;
        }
    }

    IEnumerator HoverBackgroundIn()
    {
        yield return new WaitForSeconds(0.01f);
        if (backgroundUI.GetComponent<CanvasGroup>().alpha != 1)
        {
            backgroundUI.GetComponent<CanvasGroup>().alpha += 0.01f;
            StartCoroutine(HoverBackgroundIn());
        }
    }

    IEnumerator HoverEverythingOut()
    {
        yield return new WaitForSeconds(0.01f);
        if (backgroundUI.GetComponent<CanvasGroup>().alpha != 0)
        {
            backgroundUI.GetComponent<CanvasGroup>().alpha -= 0.01f;
            StartCoroutine(HoverEverythingOut());
        }
    }

    void UpdateData()
    {
        SetBounty data = highlightedObject.GetComponent<SetBounty>();
        bountySprite.sprite = data.bountyTargetSprite;
        bountyName.text = data.bountyTargetName;
        bountyTime.text = string.Format("{0:F1}s", data.bountyTargetTime);
        bountyScore.text = data.bountyTargetScore.ToString();
    }
}
