using UnityEngine;

public class Flower : MonoBehaviour
{
    [SerializeField] private Sprite[] flowersSprites;
    [SerializeField] private Color color;
    
    void Start()
    {
        var flowers = GetComponentsInChildren<SpriteRenderer>();
        foreach (var flower in flowers)
        {
            flower.sprite = flowersSprites[Mathf.RoundToInt(Random.Range(0, 3))];
            flower.color = color;
            
            flower.gameObject.GetComponent<Transform>().localScale = 
                Random.Range(-1, 1) < 0 
                    ? new Vector3(-0.25f, 0.25f, 0.25f) 
                    : new Vector3(0.25f, 0.25f, 0.25f);
        }
    }
}
