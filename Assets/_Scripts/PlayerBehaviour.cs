using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBehaviour : MonoBehaviour
{
    public Transform bulletSpawn;
    public GameObject bullet;
    public int fireRate;
    public Vector3 forward;             //For Task1 

    public BulletManager bulletManager;

    [Header("Movement")]
    public float speed;
    public bool isGrounded;


    public RigidBody3D body;
    public CubeBehaviour cube;
    public Camera playerCam;

    void start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _Fire();
        _Move();

        forward.x = playerCam.transform.forward.x;                  //Task1
        forward.z = playerCam.transform.forward.z;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            // Only specifying the sceneName or sceneBuildIndex will load the Scene with the Single mode
            UnityEngine.SceneManagement.SceneManager.LoadScene("Start", LoadSceneMode.Single);
        }

    }

    private void _Move()
    {
        if (isGrounded)
        {
            if (Input.GetAxisRaw("Horizontal") > 0.0f)
            {
                // move right
                body.velocity = playerCam.transform.right * speed * Time.deltaTime;
            }

            if (Input.GetAxisRaw("Horizontal") < 0.0f)
            {
                // move left
                body.velocity = -playerCam.transform.right * speed * Time.deltaTime;
            }

            if (Input.GetAxisRaw("Vertical") > 0.0f)
            {
                // move forward
                body.velocity = playerCam.transform.forward * speed * Time.deltaTime;

                if (Input.GetAxisRaw("Horizontal") > 0.0f)
                {
                    // move right

                    body.velocity += playerCam.transform.right * speed * Time.deltaTime;
                    body.velocity.x *= 0.5f;
                    body.velocity.z *= 0.5f;
                }

                else if (Input.GetAxisRaw("Horizontal") < 0.0f)
                {
                    // move left
                    body.velocity += -playerCam.transform.right * speed * Time.deltaTime;
                    body.velocity.x *= 0.5f;
                    body.velocity.z *= 0.5f;
                }
            }

            if (Input.GetAxisRaw("Vertical") < 0.0f) 
            {
                // move Back
                body.velocity = -playerCam.transform.forward * speed * Time.deltaTime;

                if (Input.GetAxisRaw("Horizontal") > 0.0f)
                {
                    // move right

                    body.velocity += playerCam.transform.right * speed * Time.deltaTime;
                    body.velocity.x *= 0.5f;
                    body.velocity.z *= 0.5f;
                }

                else if (Input.GetAxisRaw("Horizontal") < 0.0f)
                {
                    // move left
                    body.velocity += -playerCam.transform.right * speed * Time.deltaTime;
                    body.velocity.x *= 0.5f;
                    body.velocity.z *= 0.5f;
                }
            }

            body.velocity = Vector3.Lerp(body.velocity, Vector3.zero, 0.9f);
            body.velocity = new Vector3(body.velocity.x, 0.0f, body.velocity.z); // remove y
            

            if (Input.GetAxisRaw("Jump") > 0.0f)
            {
                body.velocity = transform.up * speed * 0.1f * Time.deltaTime;
            }

            transform.position += body.velocity;
        }
    }


    private void _Fire()
    {
        if (Input.GetAxisRaw("Fire1") > 0.0f)
        {
            // delays firing
            if (Time.frameCount % fireRate == 0)
            {

                var tempBullet = bulletManager.GetBullet(bulletSpawn.position, bulletSpawn.forward);
                tempBullet.transform.SetParent(bulletManager.gameObject.transform);
            }
        }
    }

    void FixedUpdate()
    {
        GroundCheck();
    }

    private void GroundCheck()
    {
        isGrounded = cube.isGrounded;
    }

}
