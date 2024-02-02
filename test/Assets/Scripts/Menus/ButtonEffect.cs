using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    RectTransform buttonRect;
    Button btn;
    float scaleAmount = 0.15f;
    Vector3 initialScale;

    private void Start()
    {
        buttonRect = gameObject.GetComponent<RectTransform>();
        btn = gameObject.GetComponent<Button>();
        btn.onClick.AddListener(Select);
        initialScale = buttonRect.localScale;
    }
    private void OnDisable()
    {
        buttonRect.localScale = initialScale;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonRect.localScale = new Vector3(buttonRect.localScale.x + scaleAmount, buttonRect.localScale.y + scaleAmount, 1);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonRect.localScale = new Vector3(buttonRect.localScale.x - scaleAmount, buttonRect.localScale.y - scaleAmount, 1);
    }
    void Select()
    {
        //play audio
    }

}