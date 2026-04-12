using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class WorldButtonFeedback : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Material Alvo Do Objeto No Mundo)")]
    public Material targetMaterial;

    [Header("Configs")]
    public float fadeDuration = 0.3f;

    private Coroutine fadeRoutine;

    private static readonly int Metallic = Shader.PropertyToID("_Metallic");

    public void OnPointerEnter(PointerEventData eventData)
    {
        StartFade(1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StartFade(0f);
    }

    /*public void OnPointerClick(PointerEventData eventData)
    {
        GetComponent<UnityEngine.UI.Button>().onClick.Invoke();
    }*/

    void StartFade(float target)
    {
        if (fadeRoutine != null) StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(FadeMetallic(target));
    }

    IEnumerator FadeMetallic(float target)
    {
        float start = targetMaterial.GetFloat(Metallic);
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = time / fadeDuration;

            float value = Mathf.Lerp(start, target, t);
            targetMaterial.SetFloat(Metallic, value);

            yield return null;
        }

        targetMaterial.SetFloat(Metallic, target);
    }

    public void ResetMaterial()
    {
        if (fadeRoutine != null) StopCoroutine(fadeRoutine);

        targetMaterial.SetFloat(Metallic, 0f);
    }
}
