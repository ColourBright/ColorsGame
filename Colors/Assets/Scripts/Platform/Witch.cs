using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Witch : MonoBehaviour
{
    public float speed;
    public float jumpforce;
    
    [SerializeField] private float scale;
    [SerializeField] private ColorPalette colorPalette;

    [SerializeField] public float defaultSpeed;
    [SerializeField] public float defaultJumpforce;
    [SerializeField] private float groundRadius;
    [SerializeField] private LayerMask ground;
    

    [SerializeField] private Transform groundChecker; 

    private Rigidbody2D body;
    private Animator _animator;
    private BoxCollider2D collider;

    [SerializeField] private bool grounded;
    private bool onColorful;
    public bool leftColorfulPlatform;
    private GameObject platformToColor;

    void Start()
    {
        speed = defaultSpeed;
        jumpforce = defaultJumpforce;
        body = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        var horizontalInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
        
        grounded = Physics2D.OverlapCircle(groundChecker.position, groundRadius, ground);

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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }

        _animator.SetBool("run", horizontalInput != 0);
        _animator.SetBool("grounded", grounded);
        _animator.SetFloat("vSpeed", body.velocity.y);
    }

    private void ColorPlatform()
    {
        if (!onColorful) return;
        if (colorPalette.GetSelectedColor().Equals(ColorPalette.colors[0]))
        {
            ResetStats();
        }
        colorPalette.UsePotion(platformToColor.gameObject);
    }

    private void Jump()
    {
        if (!grounded) return;
        body.velocity = new Vector2(body.velocity.x, jumpforce);
        _animator.SetTrigger("Jump");
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
            if (leftColorfulPlatform)
            {
                ResetStats();
            }
            onColorful = true;
            platformToColor = other.gameObject;
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

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Colorful"))
        {
            onColorful = false;
            platformToColor = null;
        }
    }
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Platform"))
        {
            grounded = true;
            if (leftColorfulPlatform)
            {
                ResetStats();
            }
        }

        if (col.gameObject.CompareTag("Flower"))
        {
            PickupFlower(col);
        }
    }

    public void ResetStats()
    {
        speed = defaultSpeed;
        jumpforce = defaultJumpforce;
    }
}
