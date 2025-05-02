using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class BumpyButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{

    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!button.interactable) return;

        LeanTween.cancel(button.gameObject);
        LeanTween.scale(button.gameObject, new Vector2(1.1f, 0.9f), 0.6f)
        .setEase(LeanTweenType.easeOutElastic) // scale it like an elastic bounce
        .setIgnoreTimeScale(true); // even if timescale is set to 0 this will still work
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!button.interactable) return;

        LeanTween.cancel(button.gameObject);
        LeanTween.scale(button.gameObject, Vector2.one, 0.6f)
        .setEase(LeanTweenType.easeOutElastic) // scale it like an elastic bounce
        .setIgnoreTimeScale(true); // even if timescale is set to 0 this will still work
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!button.interactable) return;

        LeanTween.cancel(button.gameObject);
        LeanTween.scale(button.gameObject, Vector2.one, 0.6f)
        .setEase(LeanTweenType.easeOutElastic) // scale it like an elastic bounce
        .setIgnoreTimeScale(true); // even if timescale is set to 0 this will still work
    }
}
