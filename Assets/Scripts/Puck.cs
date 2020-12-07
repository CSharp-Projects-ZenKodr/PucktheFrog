using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Puck : MonoBehaviour {

    public Rigidbody rb;
    [SerializeField]private float jumpPower;
    [SerializeField]private float maxJump;
    [SerializeField] private float jumpNumMax;
    [SerializeField] private Transform footPlane;
    private float jumpNum = 1;
    private float jumpInt;
    private bool jump;
    private bool doubleJump;
    private bool inAir = false;
    private float yScale = 0.3f;
    private float zScale = 1;
    private float xScale = 1;
    private bool isCharging;
    private bool lTurn;
    private bool rTurn;

    
    

    private void YScale()
    {
        //float yDif = yScale - (yScale * 0.98f);
        xScale = xScale * 1.01f;
        yScale = yScale * 0.98f;
        zScale = zScale * 1.01f;

        transform.localScale = new Vector3(xScale, yScale, zScale);
    }


// Use this for initialization
void Start () {
        jumpInt = jumpPower;
        rb = GetComponent<Rigidbody>();
        jump = false;
	}

   
    // Update is called once per frame
    void Update () {
        if (Input.GetKey(KeyCode.Space) && !inAir)
        {
            
            //Debug.Log(transform.localScale.y);

            if (jumpPower < maxJump)
            {
                isCharging = true;
                jumpPower = jumpPower + 0.3f;
                
            }
            else
            {
                isCharging = false;
            }
            
        }

        if(Input.GetKeyUp(KeyCode.Space) && inAir && jumpNum>0) //used for double jumps
        {
            doubleJump = true;
        }

        if (Input.GetKeyUp(KeyCode.Space) && !inAir)
        {
            jump = true;
            isCharging = false;
            transform.localScale = new Vector3(1, 0.3f, 1);
            xScale = 1;
            yScale = 0.3f;
            zScale = 1;
            jumpNum--;
            Debug.Log(transform.localScale.y);
        }

        if (Input.GetKey(KeyCode.A))
        {
            lTurn = true;
        }
        else
        {
            lTurn = false;
        }

        if (Input.GetKey(KeyCode.D))
        {
            rTurn = true;
        }
        else
        {
            rTurn = false;
        }

        if (Physics.OverlapSphere(footPlane.position, 0.1f).Length > 0)
        {
            inAir = false;
            jumpNum = jumpNumMax;
        }
        else
        {
            inAir = true;
        }

        if (rb.position.y < -3)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }


	}

    private void FixedUpdate()
    {

        if (jump)
        {
            rb.AddForce(new Vector3(0, jumpPower, 0), ForceMode.VelocityChange);
            rb.AddForce(transform.forward * 20 * jumpPower);
            jump = false;
            jumpPower = jumpInt;
        }

        if (doubleJump)
        {
            //rb.AddForce(new Vector3(0, 5, 0), ForceMode.VelocityChange);
            Vector3 v = rb.velocity;
            v.y = 3;
            rb.velocity = v;
            rb.AddForce(transform.forward * 50);
            doubleJump = false;
            jumpNum--;

        }

        if (lTurn)
        {
            transform.Rotate(0, -6, 0, Space.Self);
        }
        if (rTurn)
        {
            transform.Rotate(0, 6, 0, Space.Self);
        }

        if (isCharging)
        {
            YScale();
        }
    }
}
