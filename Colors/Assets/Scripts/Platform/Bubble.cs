using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bubble : MonoBehaviour
{
    [SerializeField] private List<Tilemap> objectsToColor1;
    [SerializeField] private List<Tilemap> objectsToColor2;
    [SerializeField] private Color color1;
    [SerializeField] private Color color2;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (var obj in objectsToColor1)
            {
                obj.color = color1;
            }
            foreach (var obj in objectsToColor2)
            {
                obj.color = color2;
            }
        }
    }
}
