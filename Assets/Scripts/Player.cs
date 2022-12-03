using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{   
    public float speed = 2.5f;
    public float mouseSpeed = 50f;
    public float jumpForce = 5f;    
    new Rigidbody rigidbody;
    public Transform feet;
    public Transform bulletPrefab;
    public Slider healthBar;
    public float bulletInterval;
    float bulletTimer = 0f;
    public float maxHealth;
    float health;
    float redTimer = 0f;
    public int bullets = 10;
    MeshRenderer rend;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rend = GetComponent<MeshRenderer>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        int zMotion = (Keyboard.current.wKey.isPressed? 1:0) + (Keyboard.current.sKey.isPressed? -1:0);
        int xMotion = (Keyboard.current.dKey.isPressed? 1:0) + (Keyboard.current.aKey.isPressed? -1:0);

        rigidbody.MovePosition(rigidbody.position + rigidbody.rotation * (new Vector3(xMotion, 0, zMotion) * speed * Time.deltaTime));

        Mouse currentMouse = Mouse.current;
        rigidbody.MoveRotation(rigidbody.rotation * Quaternion.Euler(0, currentMouse.delta.ReadValue().x * Time.deltaTime * mouseSpeed, 0));
        
        bool isGrounded = Physics.Raycast(feet.position, Vector3.down, 0.1f);
        //bool isGrounded = Physics.SphereCast(feet.position, 0.5f, Vector3.down, out _, 0.1f);
        if (isGrounded)
        {
            // Debug.Log("Grounded");
        }
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }


        bulletTimer -= Time.deltaTime;
        if(currentMouse.leftButton.isPressed && bulletTimer <= 0 && bullets > 0)
        {
            bullets -= 1;
            bulletTimer = bulletInterval;
            Transform bullet = Instantiate(bulletPrefab, transform.position + transform.forward * 0.7f, Camera.main.transform.rotation);
        }


        redTimer -= Time.deltaTime;
        if(redTimer > 0)
        {
            rend.material.color = Color.red;
        }
        else
        {
            rend.material.color = Color.white;
        }

        
        healthBar.value = health / maxHealth;
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }


    void OnCollisionStay(Collision col)
    {
        if(col.gameObject.tag == "Enemy")
        {
            health -= 0.1f;
            redTimer = 0.2f;
        }
    }
    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "Ammunition")
        {
            bullets += col.gameObject.GetComponent<AmmunitionBox>().bulletsContained;
            Destroy(col.gameObject);
        }
    }
}
