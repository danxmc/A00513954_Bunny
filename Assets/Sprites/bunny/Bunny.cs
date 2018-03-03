using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bunny : MonoBehaviour {

    private Bunny instance;
    public int speed;
    public float force;
    public int lives;
    public Text livesText;

    protected LevelManager lm;
    private Vector2 move;
    private bool grounded;
    private int direction = 1;
    protected Rigidbody2D rigidbody;
    protected Animator animator;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start () {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        lm = GetComponent<LevelManager>();
        livesText.text = lives.ToString();
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKey(KeyCode.RightArrow))
        {
            Flip();
            animator.SetBool("Walking", true);
            direction = 1;
            transform.Translate(Vector3.right * speed * Time.deltaTime, Space.World);
            //transform.position += Vector3.right * speed * Time.deltaTime;
            //Debug.Log("move");
        }
        else 
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Flip();
            animator.SetBool("Walking", true);
            direction = -1;
            transform.Translate(Vector3.left * speed * Time.deltaTime, Space.World);
            //transform.position += Vector3.left * speed * Time.deltaTime;
            //Debug.Log("move");
        } else
        {
            animator.SetBool("Walking", false);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            if (grounded)
            {
                StartCoroutine(Jump());
            }
        }
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x = direction * 3;
        transform.localScale = scale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Carrot")
        {
            Win();
        }
    }

    IEnumerator Jump()
    {
        animator.SetBool("Jumping", true);
        rigidbody.AddForce(new Vector2(0f, 3f),ForceMode2D.Impulse);
        yield return new WaitForSeconds(1f);
        animator.SetBool("Jumping", false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.tag == "Enemy")
        {
            lives--;
            livesText.text = lives.ToString();
            Debug.Log(lives);
            if (lives == 0)
            {
                Die();
            }

            // Calculate Angle Between the collision point and the player
            Vector3 dir = collision.transform.position - transform.position;
            // We then get the opposite (-Vector3) and normalize it
            dir = - dir.normalized;
            // And finally we add force in the direction of dir and multiply it by force. 
            // This will push back the player
            rigidbody.AddForce(dir * force);
        }

        if (collision.gameObject.tag == "Floor" || collision.gameObject.tag == "Platform")
        {
            grounded = true;
        }

        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor" || collision.gameObject.tag == "Platform")
        {
            grounded = false;
        }
    }

    void Die()
    {
        Destroy(gameObject);
        LevelManager levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        levelManager.LoadLevel("Lose");
    }

    void Win()
    {
        Destroy(gameObject);
        LevelManager levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        levelManager.LoadLevel("Win");
    }
}
