using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastingToObject : MonoBehaviour
{
    [SerializeField]
    private GameObject highlightedObject;
    private RaycastHit objectHit;

    private GameObject[] props;
    [SerializeField]
    private TransformToObject transformationController;

    private void Start()
    {
        props = GameObject.FindGameObjectsWithTag("Prop");
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out objectHit))
        {
            if (objectHit.transform.gameObject.CompareTag("Prop"))
            {
                foreach(GameObject prop in props)
                {
                    if (objectHit.transform.gameObject.GetInstanceID() == prop.GetInstanceID())
                    {
                        continue;
                    }

                    prop.GetComponent<Outline>().enabled = false;
                }

                float distance = Vector3.Distance(transform.position, objectHit.transform.position);
                if (distance <= 10.35f)
                {
                    highlightedObject = objectHit.transform.gameObject;
                    highlightedObject.GetComponent<Outline>().enabled = true;
                }
            } else
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
            transformationController.Transform(highlightedObject);
        }
        if (Input.GetMouseButtonDown(1))
        {
            transformationController.Retain();
        }

    }
}
