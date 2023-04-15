using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public enum SlimeAnimationStateOwn { Idle, Walk, Jump, Attack, Damage }
public class SlimeMovement : MonoBehaviour
{

    public GameObject SlimeBody;
    public SlimeAnimationStateOwn currentState;

    public Animator animator;

    // [SerializeField] float speed = 6f;
    float turnSmoothVelocity;
    private bool isActionable = true;


    void Start()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
    }

    //void LateUpdate()
    //{
    //    if (isActionable)
    //    {
    //        float horizontal = Input.GetAxisRaw("Horizontal");
    //        float vertical = Input.GetAxisRaw("Vertical");
    //        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

    //        if (direction.magnitude >= 0.1f)
    //        {
    //            currentState = SlimeAnimationStateOwn.Walk;
    //        }
    //        else
    //        {
    //            currentState = SlimeAnimationStateOwn.Idle;
    //        }

    //        switch (currentState)
    //        {
    //            case SlimeAnimationStateOwn.Idle:
    //                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) return;
    //                animator.SetFloat("Speed", 0);
    //                break;

    //            case SlimeAnimationStateOwn.Walk:
    //                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk")) return;

    //                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
    //                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, 0.1f);
    //                transform.rotation = Quaternion.Euler(0f, angle, 0f);

    //                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
    //                Move(moveDir, speed);

    //                animator.SetFloat("Speed", speed);

    //                break;
    //        }
    //    }
    //}


    private NavMeshAgent _agent;
    public Vector2 _move;
    public Vector2 _look;
    public float aimValue;
    public float fireValue;

    public Vector3 nextPosition;
    public Quaternion nextRotation;

    public float rotationPower = 3f;
    public float rotationLerp = 0.5f;

    public float speed = 1f;
    public Camera camera;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    public void OnMove(InputValue value)
    {
        _move = value.Get<Vector2>();
    }

    public void OnLook(InputValue value)
    {
        _look = value.Get<Vector2>();
    }

    public void OnAim(InputValue value)
    {
        aimValue = value.Get<float>();
    }

    public void OnFire(InputValue value)
    {
        fireValue = value.Get<float>();
    }

    public GameObject followTransform;


    private void Update()
    {
        #region Player Based Rotation

        //Move the player based on the X input on the controller
        //transform.rotation *= Quaternion.AngleAxis(_look.x * rotationPower, Vector3.up);

        #endregion

        #region Follow Transform Rotation

        //Rotate the Follow Target transform based on the input
        followTransform.transform.rotation *= Quaternion.AngleAxis(_look.x * rotationPower, Vector3.up);

        #endregion

        #region Vertical Rotation
        followTransform.transform.rotation *= Quaternion.AngleAxis(_look.y * rotationPower, Vector3.right);

        var angles = followTransform.transform.localEulerAngles;
        angles.z = 0;

        var angle = followTransform.transform.localEulerAngles.x;

        //Clamp the Up/Down rotation
        if (angle > 180 && angle < 340)
        {
            angles.x = 340;
        }
        else if (angle < 180 && angle > 40)
        {
            angles.x = 40;
        }


        followTransform.transform.localEulerAngles = angles;
        #endregion


        nextRotation = Quaternion.Lerp(followTransform.transform.rotation, nextRotation, Time.deltaTime * rotationLerp);

        if (_move.x == 0 && _move.y == 0)
        {
            nextPosition = transform.position;

            if (aimValue == 1)
            {
                //Set the player rotation based on the look transform
                transform.rotation = Quaternion.Euler(0, followTransform.transform.rotation.eulerAngles.y, 0);
                //reset the y rotation of the look transform
                followTransform.transform.localEulerAngles = new Vector3(angles.x, 0, 0);
            }

            return;
        }
        float moveSpeed = speed / 100f;
        Vector3 position = (transform.forward * _move.y * moveSpeed) + (transform.right * _move.x * moveSpeed);
        nextPosition = transform.position + position;


        //Set the player rotation based on the look transform
        transform.rotation = Quaternion.Euler(0, followTransform.transform.rotation.eulerAngles.y, 0);
        //reset the y rotation of the look transform
        followTransform.transform.localEulerAngles = new Vector3(angles.x, 0, 0);
    }

    public void Move(Vector3 moveDir, float speed)
    {
        GetComponent<Rigidbody>().MovePosition(transform.position + moveDir.normalized * speed * Time.deltaTime);
    }

    void OnAnimatorMove()
    {
        // apply root motion to AI
        Vector3 position = animator.rootPosition;
        transform.position = position;
    }

    public void RotateToCamera()
    {
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, Camera.main.transform.localEulerAngles.y, transform.localEulerAngles.z);
    }

    public void ToggleActions()
    {
        isActionable = !isActionable;
    }
}
