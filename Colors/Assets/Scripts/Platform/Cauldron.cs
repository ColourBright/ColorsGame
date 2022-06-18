using UnityEngine;

public class Cauldron : MonoBehaviour
{
    public Color color;
    public int flowersCount;
    public int potionsCount;
    
    [SerializeField] private GameObject book;
    [SerializeField] private Sprite[] flowersSprites;
    [SerializeField] private Sprite[] numbersSprites;

    void Start()
    {
        var children = book.GetComponentsInChildren<SpriteRenderer>();
        foreach (var child in children)
        {
            if (child.name == "Flower")
            {
                child.sprite = flowersSprites[Mathf.RoundToInt(Random.Range(0, 3))];
            }
            if (child.name == "Symbol")
            {
                child.sprite = numbersSprites[flowersCount - 1];
            }
            child.color = color;
        }
        book.GetComponent<SpriteRenderer>().color = Color.white;
    }
}
