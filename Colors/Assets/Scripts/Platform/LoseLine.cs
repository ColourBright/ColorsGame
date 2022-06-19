using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseLine : MonoBehaviour
{
    [SerializeField] private int levelNumber;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene($"Level{levelNumber}");
        }
    }
}
