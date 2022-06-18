using System.Collections.Generic;
using UnityEngine;

public class Witch : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float scale;
    [SerializeField] private ColorPalette colorPalette;

    private Rigidbody2D body;
    private Animator _animator;

    private bool grounded;
    private bool onPlatform;
    private Collision2D platform;

    private Dictionary<Color, int> flowersInventory;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        flowersInventory = new Dictionary<Color, int>();
    }
    
    void Update()
    {
        var horizontalInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
        
        if (horizontalInput > 0.01f)
        {
            body.transform.localScale = new Vector3(scale, scale, scale);
        }
        else if (horizontalInput < -0.01f)
        {
            body.transform.localScale = new Vector3(-scale, scale, scale);
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            ColorPlatform();
        }

        _animator.SetBool("run", horizontalInput != 0);
        _animator.SetBool("grounded", grounded);
    }

    private void ColorPlatform()
    {
        if (!onPlatform) return;
        colorPalette.UsePotion(platform.gameObject);
    }

    private void Jump()
    {
        if (!grounded) return;
        body.velocity = new Vector2(body.velocity.x, speed);
        _animator.SetTrigger("jump");
        grounded = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Cauldron"))
        {
            var cauldron = other.GetComponent<Cauldron>();
            if (flowersInventory[cauldron.color] >= cauldron.flowersCount)
            {
                flowersInventory[cauldron.color] -= cauldron.flowersCount;
                colorPalette.AddPotion(cauldron.color, cauldron.potionsCount);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Platform"))
        {
            grounded = true;
            onPlatform = true;
            platform = col;
        }

        if (col.gameObject.CompareTag("Flower"))
        {
            var flowerColor = col.gameObject.GetComponent<SpriteRenderer>().color;
            if (flowersInventory.TryGetValue(flowerColor, out var a))
            {
                flowersInventory[flowerColor]++;
            }
            else
            {
                flowersInventory.Add(flowerColor, 1);
            }
            Debug.Log(flowersInventory[flowerColor]);
            Destroy(col.gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform") && onPlatform)
        {
            platform = other;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            onPlatform = false;
            platform = null;
        }
    }
}
