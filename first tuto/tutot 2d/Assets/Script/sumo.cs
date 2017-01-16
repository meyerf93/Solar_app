using UnityEngine;
using System.Collections;

public class sumo : MonoBehaviour {

    public float speedRun;
    public float ForceJump;

    public float groundRadisu = 0.3f;
    public LayerMask groundMask;
    public Transform groundTest;

    private Rigidbody2D playerRigid;
    private Animator anim;

    private bool isLeft = true;
    private bool jump = false;
    private bool isGrounded = false;

	// Use this for initialization
	void Start () {
        playerRigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        if (!jump)
        {
            float jumpVal = Input.GetAxis("Jump");
            if (jumpVal > 0)
            {
                Debug.Log("JUMP");
                jump = true;
                Vector2 jumpVec = Vector2.up * ForceJump;
                playerRigid.AddForce(jumpVec,ForceMode2D.Impulse);
                anim.SetTrigger("jump");
            }
            
        }
	}

    // update each regular time
    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundTest.position, groundRadisu,groundMask);

        if (jump && isGrounded)
        {
            jump = false;
            anim.SetTrigger("TouchGround");
        }

        float move = Input.GetAxis("Horizontal");
        anim.SetFloat("SpeedRun", Mathf.Abs(move));
        Vector2 speedV = new Vector2(move*speedRun,playerRigid.velocity.y);

        playerRigid.velocity = speedV;
        if(move > 0 && isLeft)
            flip();
        else if (move < 0 && !isLeft)
            flip();
    }

    private void flip()
    {
        isLeft = !isLeft;
        Vector3 scale = transform.lossyScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
