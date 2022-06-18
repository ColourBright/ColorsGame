using System;
using UnityEngine;
using UnityEngine.Tilemaps;

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

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
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

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Space))
        {
            Jump();
        }
        
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            ColorPlatform();
        }
        
        _animator.SetBool("run", horizontalInput != 0);
        _animator.SetBool("grounded", grounded);
    }

    private void ColorPlatform()
    {
        if (!onPlatform) return;
        platform.gameObject.GetComponent<Tilemap>().color = colorPalette.GetSelectedColor();
    }

    private void Jump()
    {
        if (!grounded) return;
        body.velocity = new Vector2(body.velocity.x, speed);
        _animator.SetTrigger("jump");
        grounded = false;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Platform")) return;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Platform"))
        {
            grounded = true;
            onPlatform = true;
            platform = col;
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
