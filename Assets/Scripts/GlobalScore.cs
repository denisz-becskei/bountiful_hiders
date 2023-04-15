using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalScore : MonoBehaviour
{
    public static int score = 0;

    public static void UpdateBar()
    {
        GameObject.FindGameObjectWithTag("GlobalProgressBar").GetComponent<Image>().fillAmount = BountyProgressHandler.ScaleNumberToRange(score, 0f, 100f);
    }
}
