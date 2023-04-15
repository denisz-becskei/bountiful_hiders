using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreMechanics : MonoBehaviour
{
    [SerializeField] GameObject bountyBoard;
    GameObject minigamesController;
    [SerializeField] GameObject slime;

    [SerializeField] GameObject originalBountyBoard;
    [SerializeField] GameObject bountyPaper;

    private void Start()
    {
        minigamesController = GameObject.FindGameObjectWithTag("TempDir");
        SpawnBountyPaperOnBoard(originalBountyBoard);
    }

    public void SpawnBountyBoard(GameObject invoker)
    {
        Instantiate(bountyBoard, new Vector3(invoker.transform.position.x, 0, invoker.transform.position.z), invoker.transform.rotation);
        slime.GetComponent<SlimeMovement>().ToggleActions();
        GameObject newTemp = new GameObject("TempDirectory");
        newTemp.transform.parent = minigamesController.transform.parent;
        newTemp.tag = "TempDir";
        newTemp.AddComponent<RectTransform>().localPosition = new Vector2(0, 0);
        newTemp.layer = 5;
        Destroy(minigamesController);
        minigamesController = newTemp;
        Destroy(invoker);
    }

    private void SpawnBountyPaperOnBoard(GameObject bountyBoard)
    {
        for (int i = -1; i < 2; i++)
        {
            GameObject newPaper = Instantiate(bountyPaper, bountyBoard.transform);
            newPaper.transform.localPosition = new Vector3(newPaper.transform.localPosition.x + i * 0.6f, newPaper.transform.localPosition.y, newPaper.transform.localPosition.z);
            newPaper.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(-30, 30));
        }
    }
}
