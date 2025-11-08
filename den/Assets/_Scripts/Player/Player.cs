using Unity.Cinemachine;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private CinemachineCamera mainCamera;
    [SerializeField] private float velocitySmoothTiming;
    public GameObject Orientation;
    public Rigidbody Rb{ get; set; }
    private Vector3 movementForward;
    private Vector3 movementRight;
    private Quaternion lookRotation;
    private Vector3 vel;
    private MovementDirectionCalculator moveDir;
    void Start()
    {
        moveDir = GetComponent<MovementDirectionCalculator>();
        Rb = GetComponent<Rigidbody>();
        Cursor.visible = false;
    }

    public void Update()
    {
        movementForward = mainCamera.transform.forward.normalized;
        movementRight = mainCamera.transform.right.normalized;
        movementForward.y = 0;
        movementRight.y = 0;
        moveDir.FindWall(movementForward);
        moveDir.ClimbOnWall();
        lookRotation = Quaternion.LookRotation(movementForward);
        Orientation.transform.rotation = Quaternion.Lerp(Orientation.transform.rotation, lookRotation, 10f * Time.deltaTime);
    }
    void FixedUpdate()
    {
        Rb.linearVelocity = Vector3.SmoothDamp(Rb.linearVelocity, moveDir.MovementDirection(), ref vel, velocitySmoothTiming);
    }
  
}
