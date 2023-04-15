using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveBoards : MonoBehaviour
{
    [SerializeField] GameObject[] gemTypes;
    [SerializeField] Vector3[] possibleLocations;
    [SerializeField] float[] rotations;
    [SerializeField] GameObject slime;

    private GameObject canvas;
    [SerializeField] GameObject stabilizeCrystallineStructure;

    void Start()
    {
        List<int> selectedIndices = new List<int>();
        while (selectedIndices.Count < 6)
        {
            int randomIndex = Random.Range(0, gemTypes.Length);
            if (selectedIndices.Contains(randomIndex))
            {
                continue;
            } else
            {
                selectedIndices.Add(randomIndex);
                Instantiate(gemTypes[randomIndex], possibleLocations[randomIndex], Quaternion.Euler(0, rotations[randomIndex], 0));
            }
        }

        canvas = GameObject.FindGameObjectWithTag("TempDir");
    }

    public void LaunchMinigame(GameObject invoker, int minigameIndex)
    {
        

        switch(minigameIndex)
        {
            case 0:
                canvas = GameObject.FindGameObjectWithTag("TempDir");
                slime.GetComponent<SlimeMovement>().ToggleActions();
                GameObject crystallineStructure = Instantiate(stabilizeCrystallineStructure, canvas.transform);
                Debug.Log(crystallineStructure);
                crystallineStructure.GetComponent<StabilizeCrystallineStructure>().invoker = invoker;
                break;
        }
    }
}
