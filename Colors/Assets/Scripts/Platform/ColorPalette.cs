using System;
using UnityEngine;
using UnityEngine.UI;

public class ColorPalette : MonoBehaviour
{
    [SerializeField] private Color[] colors;
    [SerializeField] private Image potion;
    private int selectedColor;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            selectedColor += 1;
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            selectedColor -= 1;
        }
        selectedColor %= colors.Length;
        potion.color = GetSelectedColor();
    }

    public Color GetSelectedColor()
    {
        return colors[Math.Abs(selectedColor)];
    }
}