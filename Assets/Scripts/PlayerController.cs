using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Player player;
    private Animator playerAnimator;
    private Rigidbody2D rigidbody2D;
    public float inputX = 0f;
    public float inputY = 0f;
    public bool isWalking;
    public Vector2 movement = Vector2.zero;

    void Start()
    {
        isWalking = false;
        playerAnimator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
    }

    void Update()
    {
    }

    private void FixedUpdate() {

        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");
        isWalking = (inputX != 0 || inputY != 0);
        movement = new Vector2(inputX, inputY);
        movement = movement.normalized;

        if (isWalking)
        {
            playerAnimator.SetFloat("InputX", inputX);
            playerAnimator.SetFloat("InputY", inputY);
        }

        playerAnimator.SetBool("isWalking", isWalking);
        rigidbody2D.MovePosition(rigidbody2D.position + movement * player.entity.speed * Time.fixedDeltaTime);
    }
}
