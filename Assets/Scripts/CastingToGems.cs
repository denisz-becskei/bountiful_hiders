using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastingToGems : MonoBehaviour
{
    [SerializeField]
    private GameObject highlightedObject;
    [SerializeField]
    private ObjectiveBoards objectiveBoards;
    private RaycastHit objectHit;


    [SerializeField] TransformToObject transformationController;

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out objectHit))
        {
            if (objectHit.transform.gameObject.CompareTag("Gem"))
            {

                float distance = Vector3.Distance(transform.position, objectHit.transform.position);
                if (distance <= 10.35f)
                {
                    highlightedObject = objectHit.transform.gameObject;
                    highlightedObject.GetComponent<Outline>().enabled = true;
                }
            }
            else
            {
                if (highlightedObject != null)
                {
                    highlightedObject.GetComponent<Outline>().enabled = false;
                    highlightedObject = null;
                }
            }
        }

        if (Input.GetMouseButtonDown(0) && highlightedObject != null)
        {
            transformationController.Retain();
            objectiveBoards.LaunchMinigame(highlightedObject, 0);
            highlightedObject = null;

        }
    }
}
