using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class ColorPalette : MonoBehaviour
{
    private List<Color> colors;
    [SerializeField] private Image potion;
    [SerializeField] private Text countText;

    private Dictionary<Color, int> potionInventory;
    private int selectedColor;

    private void Start()
    {
        potionInventory = new Dictionary<Color, int>();
        colors = new List<Color> { Color.white };
    }

    private void Update()
    {
        if (colors.Count > 1)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                selectedColor += 1;
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                selectedColor -= 1;
            }
            selectedColor %= colors.Count;
        }
        potion.color = GetSelectedColor();
        if (potion.color != Color.white)
        {
            countText.text = potionInventory[potion.color].ToString();
        }
        else
        {
            countText.text = "";
        }
    }

    private Color GetSelectedColor()
    {
        return colors[Math.Abs(selectedColor)];
    }

    public void AddPotion(Color color, int count)
    {
        if (potionInventory.TryGetValue(color, out _))
        {
            potionInventory[color] += count;
        }
        else
        {
            potionInventory.Add(color, count);
            colors.Add(color);
            selectedColor = colors.Count - 1;
        }
    }

    public void UsePotion(GameObject platform)
    {
        Debug.Log(selectedColor);
        var color = GetSelectedColor();
        var platformColor = platform.GetComponent<Tilemap>().color;
        
        if (platformColor == color) return;
        
        if (color == Color.white)
        {
            AddPotion(platformColor, 1);
            platform.GetComponent<Tilemap>().color = color;
            return;
        }
        
        if (platformColor != Color.white) return;
        
        if (!potionInventory.TryGetValue(color, out _)) return;
        potionInventory[color] -= 1;
        platform.GetComponent<Tilemap>().color = color;
        if (potionInventory[color] == 0)
        {
            colors.Remove(color);
            potionInventory.Remove(color);
            selectedColor--;
        }
    }
}