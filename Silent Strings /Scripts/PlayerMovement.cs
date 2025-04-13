using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool moving = false;
    public bool movingUp = false;
    public bool movingDown = false;
    public bool movingLeft = false;
    public bool movingRight = false;    
    public float speed = 8.10f;
    
    [SerializeField] private float stealthSpeed = 4f;
    [SerializeField] private float noiseRadius = 5f;
    [SerializeField] private PlayerStealth playerStealth;
    
    private bool isSneaking = false;

    void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        moving = false;

        Vector3 moveDirection = Vector3.zero;

        movingUp = Input.GetKey(KeyCode.W);
        movingDown = Input.GetKey(KeyCode.S);
        movingLeft = Input.GetKey(KeyCode.A);
        movingRight = Input.GetKey(KeyCode.D);
        isSneaking = Input.GetKey(KeyCode.LeftControl);

        if ((movingUp && movingDown) || (movingLeft && movingRight))
        {
            return;
        }

        if (movingUp) moveDirection += Vector3.up;
        if (movingDown) moveDirection += Vector3.down;
        if (movingLeft) moveDirection += Vector3.left;
        if (movingRight) moveDirection += Vector3.right;

        if (moveDirection != Vector3.zero)
        {
            float currentSpeed = isSneaking ? stealthSpeed : speed;
            transform.Translate(moveDirection.normalized * currentSpeed * Time.deltaTime, Space.World);
            moving = true;

            if (!isSneaking && !playerStealth.IsInShadow())
            {
                NoiseSystem.Instance.MakeNoise(transform.position, noiseRadius);
            }
        }
    }
}