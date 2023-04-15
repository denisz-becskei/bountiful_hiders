using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum MinigameState { TOP, MID, BOT, FIN }

public class StabilizeCrystallineStructure : MonoBehaviour
{
    public GameObject invoker = null;

    [SerializeField] GameObject topCrystalPiece;
    [SerializeField] GameObject midCrystalPiece;
    [SerializeField] GameObject botCrystalPiece;
    [SerializeField] TMP_Text spaceText;
    [SerializeField] TMP_Text stabilityText;
    private CoreMechanics cm;

    [SerializeField] float[] edges;

    private float[] moveSpeeds = new float[3] { 0, 0, 0 };

    private bool[] isSpacable = new bool[3] { false, false, false };
    private bool running = true;

    private void Start()
    {
        cm = GameObject.FindGameObjectWithTag("GameController").GetComponent<CoreMechanics>();
        LoadCrystalTextures();

        float leftStartingPoint, rightStartingPoint;
        for (int i = 0; i < 3; i++)
        {
            leftStartingPoint = Random.Range(edges[0], edges[1]);
            rightStartingPoint = Random.Range(edges[2], edges[3]);

            switch (i)
            {
                case 0:
                    topCrystalPiece.GetComponent<RectTransform>().localPosition = new Vector2(Chance(50) ? leftStartingPoint : rightStartingPoint, 0.5f); break;
                case 1:
                    midCrystalPiece.GetComponent<RectTransform>().localPosition = new Vector2(Chance(50) ? leftStartingPoint : rightStartingPoint, 0); break;
                case 2:
                    botCrystalPiece.GetComponent<RectTransform>().localPosition = new Vector2(Chance(50) ? leftStartingPoint : rightStartingPoint, -0.5f); break;
            }
        }
        for (int i = 0; i < 3; i++)
        {
            moveSpeeds[i] = Random.Range(0.15f, 1f);
        }
        StartCoroutine(VolatilityTimer());
    }


    private int[] directions = new int[3] { 1, -1, 1 };
    private bool[] successes = new bool[3] { false, false, false };

    private void Update()
    {
            if (Input.GetKeyDown(KeyCode.Space) && isSpacable[0])
            {
                successes[0] = true;
                spaceText.color = Color.white;
            }

            if (Input.GetKeyDown(KeyCode.Space) && isSpacable[1])
            {
                successes[1] = true;
                spaceText.color = Color.white;
                
            }

            if (Input.GetKeyDown(KeyCode.Space) && isSpacable[2])
            {
                successes[2] = true;
                spaceText.color = Color.white;
            }

        Checks();
    }

    private void Checks()
    {
        if(running)
        {
            int successesNum = successes.Count(x => x == true);
            switch (successesNum)
            {
                case 0:
                    break;
                case 1:
                    stabilityText.text = "Current Crystalline Stability: 33%";
                    break;
                case 2:
                    stabilityText.text = "Current Crystalline Stability: 67%";
                    break;
                case 3:
                    stabilityText.text = "Current Crystalline Stability: 100%";
                    StartCoroutine(DelayedExitRoutine());
                    running = false;
                    break;
                default:
                    Debug.Log("We in a bitta pickle");
                    break;
            }
        }
    }

    private void FixedUpdate()
    {
        MoveObject(topCrystalPiece, 0);
        MoveObject(midCrystalPiece, 1);
        MoveObject(botCrystalPiece, 2);

        if (topCrystalPiece.GetComponent<RectTransform>().localPosition.x > -1 && topCrystalPiece.GetComponent<RectTransform>().localPosition.x < 1)
        {
            spaceText.color = Color.red;
            isSpacable[0] = true;
        }
        else
        {
            spaceText.color = Color.white;
            isSpacable[0] = false;
        }

        if (midCrystalPiece.GetComponent<RectTransform>().localPosition.x > -1 && midCrystalPiece.GetComponent<RectTransform>().localPosition.x < 1)
        {
            spaceText.color = Color.red;
            isSpacable[1] = true;
        }
        else
        {
            spaceText.color = Color.white;
            isSpacable[1] = false;
        }

        if (botCrystalPiece.GetComponent<RectTransform>().localPosition.x > -1 && botCrystalPiece.GetComponent<RectTransform>().localPosition.x < 1)
        {
            spaceText.color = Color.red;
            isSpacable[2] = true;
        }
        else
        {
            spaceText.color = Color.white;
            isSpacable[2] = false;
        }
    }

    private void ModifyXPosOfObject(GameObject gameObject, float modifier)
    {
        gameObject.GetComponent<RectTransform>().localPosition = new Vector2(gameObject.GetComponent<RectTransform>().localPosition.x + modifier, gameObject.GetComponent<RectTransform>().localPosition.y);
    }

    private void LoadCrystalTextures()
    {
        Texture2D topTexture, midTexture, botTexture;
        switch (invoker.name)
        {
            case "Stone1(Clone)":
                topTexture = Resources.Load("Crystals/Stone1/Stone1Top") as Texture2D;
                midTexture = Resources.Load("Crystals/Stone1/Stone1Mid") as Texture2D;
                botTexture = Resources.Load("Crystals/Stone1/Stone1Bot") as Texture2D;
                break;
            case "Stone2(Clone)":
                topTexture = Resources.Load("Crystals/Stone2/Stone2Top") as Texture2D;
                midTexture = Resources.Load("Crystals/Stone2/Stone2Mid") as Texture2D;
                botTexture = Resources.Load("Crystals/Stone2/Stone2Bot") as Texture2D;
                break;
            case "Stone3(Clone)":
                topTexture = Resources.Load("Crystals/Stone3/Stone3Top") as Texture2D;
                midTexture = Resources.Load("Crystals/Stone3/Stone3Mid") as Texture2D;
                botTexture = Resources.Load("Crystals/Stone3/Stone3Bot") as Texture2D;
                break;
            case "Stone4(Clone)":
                topTexture = Resources.Load("Crystals/Stone4/Stone4Top") as Texture2D;
                midTexture = Resources.Load("Crystals/Stone4/Stone4Mid") as Texture2D;
                botTexture = Resources.Load("Crystals/Stone4/Stone4Bot") as Texture2D;
                break;
            case "Stone5(Clone)":
                topTexture = Resources.Load("Crystals/Stone5/Stone5Top") as Texture2D;
                midTexture = Resources.Load("Crystals/Stone5/Stone5Mid") as Texture2D;
                botTexture = Resources.Load("Crystals/Stone5/Stone5Bot") as Texture2D;
                break;
            case "Stone6(Clone)":
                topTexture = Resources.Load("Crystals/Stone6/Stone6Top") as Texture2D;
                midTexture = Resources.Load("Crystals/Stone6/Stone6Mid") as Texture2D;
                botTexture = Resources.Load("Crystals/Stone6/Stone6Bot") as Texture2D;
                break;
            default:
                topTexture = null;
                midTexture = null;
                botTexture = null;
                Debug.Log("We hit a bitta pickle: " + invoker.name);
                break;
        }

        topCrystalPiece.GetComponent<Image>().sprite = create(topTexture);
        midCrystalPiece.GetComponent<Image>().sprite = create(midTexture);
        botCrystalPiece.GetComponent<Image>().sprite = create(botTexture);
    }


    private Sprite create(Texture2D input)
    {
        return Sprite.Create(input, new Rect(0.0f, 0.0f, input.width, input.height), new Vector2(0.5f, 0.5f), 100.0f);
    }

    private void MoveObject(GameObject gameObject, int objectIndex)
    {
        if (successes[objectIndex]) return;

        if (directions[objectIndex] == 1 && gameObject.GetComponent<RectTransform>().localPosition.x + moveSpeeds[objectIndex] < edges[3])
        {
            ModifyXPosOfObject(gameObject, moveSpeeds[objectIndex]);
        }
        else if (directions[objectIndex] == 1 && gameObject.GetComponent<RectTransform>().localPosition.x + moveSpeeds[objectIndex] >= edges[3])
        {
            directions[objectIndex] -= 2;
        }
        else if (directions[objectIndex] == -1 && gameObject.GetComponent<RectTransform>().localPosition.x - moveSpeeds[objectIndex] > edges[0])
        {
            ModifyXPosOfObject(gameObject, -moveSpeeds[objectIndex]);
        }
        else
        {
            directions[objectIndex] += 2;
        }
    }

    public static bool Chance(float percentage)
    {
        return Random.Range(0, 100) < percentage;
    }

    IEnumerator DelayedExitRoutine()
    {
        yield return new WaitForSeconds(2);
        cm.SpawnBountyBoard(invoker);
    }

    IEnumerator VolatilityTimer()
    {
        yield return new WaitForSeconds(2);
        for (int i = 0; i < 3; i++)
        {
            moveSpeeds[i] = Random.Range(0.15f, 1f);
        }
        StartCoroutine(VolatilityTimer());
    }

}
