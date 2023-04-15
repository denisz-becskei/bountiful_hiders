using Cinemachine;
using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class TransformToObject : MonoBehaviour
{
    [SerializeField] private GameObject originalForm;
    [SerializeField] private CinemachineFreeLook cinemachineCamera;

    [SerializeField] private GameObject rootedIndicator;
    [SerializeField] private Sprite rootedUnlocked;
    [SerializeField] private Sprite rootedLocked;

    [SerializeField] private CastingToBountyPaper ctbp;
    [SerializeField] private BountyProgressHandler bph;

    public bool isBountySet = false;

    private GameObject container = null;
    private GameObject newForm = null;
    private Material[] newMats = null;

    public void Transform(GameObject objectToTransformInto)
    {
        rootedIndicator.GetComponent<Image>().sprite = rootedUnlocked;
        if (originalForm.activeSelf)
        {
            originalForm.SetActive(false);
            container = new GameObject("Container");
            container.transform.position = originalForm.transform.position;
            newForm = Instantiate(objectToTransformInto, originalForm.transform.position, Quaternion.identity, container.transform);
        } else
        {
            Vector3 originalPosition = container.transform.position;
            Destroy(newForm);
            Destroy(container);
            container = new GameObject("Container");
            container.transform.position = originalPosition;
            newForm = Instantiate(objectToTransformInto, originalPosition, Quaternion.identity, container.transform);
        }

        try
        {
            RecreateMaterials();
        } catch (OverflowException)
        {
            RecreateMaterials();
        } finally
        {
            Destroy(newForm.GetComponent<MeshCollider>());
            container.AddComponent<BoxCollider>().size = new Vector3(newForm.GetComponent<Renderer>().bounds.size.x, newForm.GetComponent<Renderer>().bounds.size.y - 0.07f, newForm.GetComponent<Renderer>().bounds.size.z);
            container.GetComponent<BoxCollider>().center = new Vector3(0, container.GetComponent<BoxCollider>().size.y / 2, 0);
            newForm.GetComponent<MeshRenderer>().materials = newMats;
            newForm.GetComponent<Outline>().enabled = false;
            container.AddComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            container.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            container.AddComponent<ObjectMovement>();
            rootedIndicator.SetActive(true);
            container.GetComponent<ObjectMovement>().rootedIndicator = rootedIndicator;
            container.GetComponent<ObjectMovement>().rootedLocked = rootedLocked;
            container.GetComponent<ObjectMovement>().rootedUnlocked = rootedUnlocked;
            container.layer = 2;
            newForm.tag = "Player";

            cinemachineCamera.Follow = container.transform;
            cinemachineCamera.LookAt = container.transform;

            if(newForm.GetComponent<PropProperties>().id == ctbp.activeBountyId)
            {
                isBountySet = true;
            } else
            {
                isBountySet = false;
            }
        }
    }
    
    public void Retain()
    {
        if (newForm != null)
        {
            rootedIndicator.GetComponent<Image>().sprite = rootedUnlocked;
            rootedIndicator.SetActive(false);
            originalForm.SetActive(true);
            originalForm.transform.position = newForm.transform.position;
            Destroy(newForm);
            Destroy(container);
            newForm = null;
            container = null;
            cinemachineCamera.Follow = originalForm.transform;
            cinemachineCamera.LookAt = originalForm.transform;

            if (bph.timerRunning)
            {
                bph.StopTimer();
            }
        }

    }

    private void RecreateMaterials()
    {
        newMats = new Material[newForm.GetComponent<MeshRenderer>().materials.Length - 4];
        for (int i = 0; i < newForm.GetComponent<MeshRenderer>().materials.Length - 4; i++)
        {
            newMats[i] = newForm.GetComponent<MeshRenderer>().materials[i];
        }
    }
}
