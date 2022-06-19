using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class ColorPalette : MonoBehaviour
{
    public List<Color> colors;
    [SerializeField] private Image potion;
    [SerializeField] private Text potionText;

    [SerializeField] private Image flower;
    [SerializeField] private Text flowerText;

    private Dictionary<Color, int> potionInventory;
    public Dictionary<Color, int> flowersInventory;
    private int selectedColor;

    private void Start()
    {
        potionInventory = new Dictionary<Color, int>();
        flowersInventory = new Dictionary<Color, int>();
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
            potionText.text = potionInventory[potion.color].ToString();
            flowerText.text = flowersInventory[potion.color].ToString();
            flower.color = GetSelectedColor();
        }
        else
        {
            potionText.text = "";
            flowerText.text = "";
            flower.color = Color.clear;
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
        }
        selectedColor = colors.IndexOf(color);
    }

    public void UsePotion(GameObject platform)
    {
        var color = GetSelectedColor();
        var platformColor = platform.GetComponent<Tilemap>().color;
        
        if (platformColor == color) return;
        
        if (color == Color.white)
        {
            AddPotion(platformColor, 1);
            platform.GetComponent<Tilemap>().color = color;
            return;
        }
        
        if (potionInventory[color] < 1) return;
        
        if (platformColor != Color.white) return;
        
        if (!potionInventory.TryGetValue(color, out _)) return;
        potionInventory[color] -= 1;
        platform.GetComponent<Tilemap>().color = color;
        potionText.text = potionInventory[color].ToString();
    }
}