using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Witch : MonoBehaviour
{
    [SerializeField] public float speed;
    [SerializeField] public float jumpforce;
    [SerializeField] private float scale;
    [SerializeField] private ColorPalette colorPalette;

    private float defaultSpeed;
    private float defaultJumpforce;

    private Rigidbody2D body;
    private Animator _animator;
    private BoxCollider2D collider;

    private bool grounded;
    private bool onColorful;
    private GameObject platformToColor;

    void Start()
    {
        defaultSpeed = speed;
        defaultJumpforce = jumpforce;
        body = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        var horizontalInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(horizontalInput * runSpeed, body.velocity.y);
        
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
        _animator.SetFloat("vSpeed", body.velocity.y);
    }

    private void ColorPlatform()
    {
        if (!onColorful) return;
        colorPalette.UsePotion(platformToColor.gameObject);
    }

    private void Jump()
    {
        if (!grounded) return;
        body.velocity = new Vector2(body.velocity.x, jumpforce);
        _animator.SetTrigger("Jump");
        grounded = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Cauldron"))
        {
            var cauldron = other.GetComponent<Cauldron>();
            if (colorPalette.flowersInventory[cauldron.color] >= cauldron.flowersCount)
            {
                colorPalette.flowersInventory[cauldron.color] -= cauldron.flowersCount;
                colorPalette.AddPotion(cauldron.color, cauldron.potionsCount);
            }
        }

        if (other.gameObject.CompareTag("Colorful"))
        {
            onColorful = true;
            platformToColor = other.gameObject;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Platform"))
        {
            grounded = true;
            speed = defaultSpeed;
        }

        if (col.gameObject.CompareTag("Flower"))
        {
            PickupFlower(col);
        }
    }

    private void PickupFlower(Collision2D col)
    {
        var flowerColor = col.gameObject.GetComponent<SpriteRenderer>().color;
        if (colorPalette.flowersInventory.TryGetValue(flowerColor, out _))
        {
            colorPalette.flowersInventory[flowerColor]++;
        }
        else
        {
            colorPalette.flowersInventory.Add(flowerColor, 1);
        }
        
        colorPalette.AddPotion(flowerColor, 0);
        Debug.Log(colorPalette.flowersInventory[flowerColor]);
        Destroy(col.gameObject);
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Colorful") && onColorful)
        {
            platformToColor = other.gameObject;
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            grounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform")
            && !Physics2D.OverlapBoxAll(collider.bounds.center, collider.bounds.size, 0).Any(c => c.CompareTag("Platform")))
        {
            grounded = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Colorful"))
        {
            onColorful = false;
            platformToColor = null;
        }
    }
}
