using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShaderEffect : MonoBehaviour
{
    public Material shadowMaterial;

    public Image effectImage;
    public float fadeSpeed = 0.3f;

    bool isInvert = false;

    public int repeatCount = 2;
   // private bool isAnimating = false;

    /*void Update()
    {
        // V 키를 눌렀을 때 애니메이션 시작
        if (Input.GetKeyDown(KeyCode.V) && !isAnimating)
        {
            StartCoroutine(FadeAlpha((EDrugColor)r++));
        }
    }*/

    private void Start()
    {
        DrugManager.Instance.shaderEffect = this;
    }

    public void StartDrugEffect(EDrugColor drugColor)
    {
        StartCoroutine(FadeAlpha(drugColor));
    }


    IEnumerator FadeAlpha(EDrugColor drugColor)
    {
        ChangeColor(drugColor);

        //isAnimating = true;

        for (int i = 0; i < repeatCount; i++)
        {
            yield return StartCoroutine(ChangeAlpha(0.1f));
            yield return StartCoroutine(InvertEffect());
            yield return StartCoroutine(ChangeAlpha(0f));
        }

        Color color = effectImage.color;
        color.a = 0f;
        effectImage.color = color;

        //isAnimating = false;
    }

    IEnumerator ChangeAlpha(float targetAlpha)
    {
        float startAlpha = effectImage.color.a;
        float elapsedTime = 0f;

        while (elapsedTime < fadeSpeed)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(elapsedTime / fadeSpeed); // t는 0에서 1 사이의 값
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, t);
            Color color = effectImage.color;
            color.a = newAlpha;
            effectImage.color = color;

            yield return null;
        }

        Color finalColor = effectImage.color;
        finalColor.a = targetAlpha;
        effectImage.color = finalColor;
    }

    IEnumerator InvertEffect()
    {
        //float duration = Random.Range(0.2f, 0.5f);

        isInvert = true;

        yield return new WaitForSeconds(0.2f);

        isInvert = false;
    }

    void ChangeColor(EDrugColor drugColor)
    {
        switch (drugColor)
        {
            case EDrugColor.red:
                effectImage.color = new Color(1, 0, 0, 0);
                break;
            case EDrugColor.orange:
                effectImage.color = new Color(1, 0.7f, 0, 0);
                break;
            case EDrugColor.yellow:
                effectImage.color = new Color(1, 0.92f, 0.016f, 0);
                break;
            case EDrugColor.green:
                effectImage.color = new Color(0, 1, 0, 0);
                break;
            case EDrugColor.blue:
                effectImage.color = new Color(0, 0, 1, 0);
                break;
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (!isInvert) Graphics.Blit(source, destination);

        else Graphics.Blit(source, destination, shadowMaterial);
    }

    public void StartMirage()
    {
        effectImage.color = new Color(0, 0, 0, 0);

        StartCoroutine(MirageEffect());
    }
    
    private IEnumerator MirageEffect()
    {
        //isAnimating = true;

        for (int i = 0; i < repeatCount; i++)
        {
            yield return StartCoroutine(ChangeAlpha(0.8f));
            yield return StartCoroutine(InvertEffect());
            yield return StartCoroutine(ChangeAlpha(0f));
        }

        Color color = effectImage.color;
        color.a = 0f;
        effectImage.color = color;

        //isAnimating = false;
    }
}
