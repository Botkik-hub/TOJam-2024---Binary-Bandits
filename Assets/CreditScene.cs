using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditScene : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] private GameObject endOfCreditsText;
    [SerializeField] private GameObject creditsContent;
    [Space]
    [Header("Speeds")]
    [SerializeField] private float creditsContentMoveSpeed;
    [SerializeField] private float endOfCreditsTextFadeSpeed;
    [Space]
    [Header("Delays")]
    [SerializeField] private float endOfCreditsTextDelay;
    [SerializeField] private float endCreditsDelay;

    private RectTransform creditsContentRectTransform;
    private TMP_Text[] allTexts;

    private bool isMoving;
    private bool isFading;

    void Start()
    {
        isMoving = false;
        isMoving = false;

        endOfCreditsText.GetComponent<TMP_Text>().CrossFadeAlpha(0, 0, true);
        allTexts = GetComponentsInChildren<TMP_Text>();
        creditsContentRectTransform = creditsContent.GetComponent<RectTransform>();
        StartCreditsContentMove();
    }

    void StartCreditsContentMove()
    {
        if (creditsContentRectTransform == null) return;

        isMoving = true;
    }

    void Update()
    {
        if (isMoving)
        {
            if (Vector2.Distance(creditsContentRectTransform.transform.position, GetComponent<RectTransform>().position) > 10.0f)
            {
                MoveCreditsContent();
            }
            else
            {
                isMoving = false;
                StartCoroutine(BeforeFadeDelay());
            }
        }
        
        if (isFading)
        {
            FadeContentAnimation();


            if (endOfCreditsText.GetComponent<TMP_Text>().color.a >= 1)
            {
                StartCoroutine(EndCredits());
            }
        }
    }

    void MoveCreditsContent()
    {
        creditsContentRectTransform.transform.Translate(transform.up * creditsContentMoveSpeed * Time.deltaTime);
    }

    void FadeContentAnimation()
    {
        foreach (TMP_Text text in allTexts)
        {
            text.CrossFadeAlpha(0, endOfCreditsTextFadeSpeed / 2.0f, true);
        }

        endOfCreditsText.GetComponent<TMP_Text>().CrossFadeAlpha(2.0f,endOfCreditsTextFadeSpeed, true);
    }

    void StartFade()
    {
        isFading = true;
    }

    IEnumerator BeforeFadeDelay()
    {
        yield return new WaitForSeconds(endOfCreditsTextDelay);

        StartFade();
    }

    IEnumerator EndCredits()
    {
        yield return new WaitForSeconds(endCreditsDelay);

        if (LoadManager.Instance.Data.Checkpoint == 0)
            SceneManager.LoadScene("Level1");
        else
            SceneManager.LoadScene($"level{LoadManager.Instance.Data.Checkpoint}");
    }
}
