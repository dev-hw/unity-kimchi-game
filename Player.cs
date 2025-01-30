using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Settings")]
    public float jumpForve;

    [Header("References")]
    public Rigidbody2D PlayerRidgeBody;

    public Animator PlayerAnimator;

    public BoxCollider2D PlayerCollider;

    private bool isGrounded = true;

    public bool isInvicible = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded){
            // 방법1
            // PlayerRidgeBody.linearVelocityX = 10;
            // PlayerRidgeBody.linearVelocityY = 20;
            
            // 방법2
            PlayerRidgeBody.AddForceY(jumpForve, ForceMode2D.Impulse);
            isGrounded = false;
            PlayerAnimator.SetInteger("state", 1);

        }
        
    }

    public void KillPlayer(){
        PlayerCollider.enabled = false;
        PlayerAnimator.enabled = false;
        PlayerRidgeBody.AddForceY(jumpForve, ForceMode2D.Impulse);
    }

    void Hit(){
        GameManager.Instance.Lives -= 1;

    }

    void Heal(){
        GameManager.Instance.Lives = Mathf.Min(3, GameManager.Instance.Lives+1);
    }

    void StartInvincible(){
        isInvicible = true;
        Invoke("StopInvincible", 5f);
    }

    void StopInvincible(){
        isInvicible = false;
    }


    void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.name == "Platform"){
            if (!isGrounded){
                PlayerAnimator.SetInteger("state", 2);
            }
            isGrounded = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collider){
        if(collider.gameObject.tag == "enemy"){
            if(!isInvicible){
                Destroy(collider.gameObject);
                Hit();
            }
        }else if(collider.gameObject.tag == "food"){
            Destroy(collider.gameObject);
            Heal();
        }else if(collider.gameObject.tag == "golden"){
            Destroy(collider.gameObject);
            StartInvincible();
        }
    }

}
