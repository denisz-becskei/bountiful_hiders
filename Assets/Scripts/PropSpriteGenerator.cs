using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UI;
using System.Linq;

public class PropSpriteGenerator : MonoBehaviour
{
    public static List<PropProperties> propProperties = new List<PropProperties>();
    [SerializeField]
    private RenderTexture renderTexture;
    [SerializeField]
    private GameObject heightReference;
    [SerializeField]
    private Camera originalMainCamera;

    private void Awake()
    {
        PropProperties[] nonUniqueProperties = GameObject.FindObjectsByType<PropProperties>(FindObjectsSortMode.None);
        List<string> alreadyAdded = new List<string>();

        foreach (PropProperties pp in nonUniqueProperties)
        {
            if (!alreadyAdded.Contains(pp.id))
            {
                alreadyAdded.Add(pp.id);
                propProperties.Add(pp);
            }
        }

        propProperties = propProperties.OrderBy(x => x.id).ToList();
    }

}
