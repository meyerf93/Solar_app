using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public Text countText;
    public Text winText;

    private Rigidbody2D rb2d;

    private int Count;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        Count = 0;
        winText.text = "";
        SetCountText();
    }
   void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector2 movment = new Vector2(moveHorizontal,moveVertical);
        rb2d.AddForce(movment*speed);
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            Count++;
            other.gameObject.SetActive(false);
            SetCountText();
        }        
    }

    void SetCountText()
    {
        countText.text = "Count: " + Count.ToString();
        if(Count >= 12)
        {
            winText.text = "You win";
        }
    }
}
