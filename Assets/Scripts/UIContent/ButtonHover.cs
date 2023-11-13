using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Button button;
    private Color originalColor;
    public Color hoverColor = Color.yellow;

    void Start()
    {
        button = GetComponent<Button>();
        originalColor = button.colors.normalColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ChangeButtonColor(hoverColor);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ChangeButtonColor(originalColor);
    }

    private void ChangeButtonColor(Color color)
    {
        ColorBlock colors = button.colors;
        colors.normalColor = color;
        button.colors = colors;
    }
}
