using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public LayerMask ground;

    public float moveSpeed = 5f;
    public float rotateSpeed = 5f;
    public Camera camera;

    private PlayerInput playerInput;
    private Animator playerAnimator;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<Animator>();
        camera = Camera.main;
    }

    private void FixedUpdate()
    {
        Rotate();
        Move();

        var dir = new Vector3(playerInput.moveH, 0f, playerInput.moveV);

        playerAnimator.SetFloat("Move", dir.magnitude);
    }

    private void Move()
    {
        var forward = camera.transform.forward;
        forward.y = 0;
        forward.Normalize();
        var right = camera.transform.right;
        right.y = 0;
        right.Normalize();

        var dir = forward * playerInput.moveV;
        dir += right * playerInput.moveH;

        if (dir.magnitude > 1)
        {
            dir.Normalize();
        }
        var delta = dir * moveSpeed * Time.deltaTime;

        transform.position += delta;
        camera.transform.position += delta;
    }

    private void Rotate()
    {
        RaycastHit hit;
        var ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
        {
            var forward = hit.point - transform.position;
            forward.y = 0;
            forward.Normalize();

            transform.rotation = Quaternion.LookRotation(forward);
        }
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.blue);
    }
}
