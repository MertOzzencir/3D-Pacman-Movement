using Unity.Cinemachine;
using UnityEngine;

public class MovementDirectionCalculator : MonoBehaviour
{
    [SerializeField] private GameObject climbRayObject;
    [SerializeField] private float rayDistance;
    [SerializeField] private LayerMask climbMask;
    [SerializeField] private float groundSpeed;
    [SerializeField] private float climbSpeed;

    public GameObject VisualPlayer;
    private InputManager inputManager;
    private Vector3 movementDirection;
    private Player player;
    private bool isFirstTimeHitting;

    private float tempRayDistance;

    private Vector3 rayPosition;
    private Vector3 rayForward;
    private Vector3 hitNormal;
    private bool canClimb;
    private bool wallCheckControl;
    void Start()
    {
        tempRayDistance = rayDistance;
        player = GetComponent<Player>();
        inputManager = FindAnyObjectByType<InputManager>();
        wallCheckControl = true;
    }

    public void FindWall(Vector3 cameraForward)
    {
        if (wallCheckControl)
        {
            if (Physics.Raycast(VisualPlayer.transform.position, VisualPlayer.transform.forward, out RaycastHit hit, tempRayDistance, climbMask))
            {
                canClimb = true;
                isFirstTimeHitting = true;
                hitNormal = hit.normal;
                wallCheckControl = false;
                Debug.Log("Hit the wall");
                Quaternion lookRotation = Quaternion.LookRotation(Vector3.up, hitNormal);
                VisualPlayer.transform.rotation = lookRotation;
            }
            else
            {
                Debug.Log("sa");
                Vector3 velocityVectorRef = player.Orientation.transform.forward * inputManager.MovementNormalized().y + player.Orientation.transform.right * inputManager.MovementNormalized().x;
                velocityVectorRef *= groundSpeed;
                Vector3 velocityVector = new Vector3(velocityVectorRef.x, 0, velocityVectorRef.z);
                movementDirection = velocityVector;
                tempRayDistance = rayDistance;
                Quaternion lookRotation = Quaternion.LookRotation(cameraForward);
                VisualPlayer.transform.rotation = Quaternion.Lerp(VisualPlayer.transform.rotation, lookRotation, 10f * Time.deltaTime);
                isFirstTimeHitting = false;

            }
        }

    }

    public void ClimbOnWall()
    {
        if (canClimb)
        {
          
            if (Physics.Raycast(climbRayObject.transform.position, climbRayObject.transform.forward, out RaycastHit hit, .75f, climbMask))
            {

                if (isFirstTimeHitting)
                {
                    player.Rb.linearVelocity = Vector3.zero;
                }
                Debug.Log("as?");

                movementDirection = Vector3.ProjectOnPlane(Vector3.up, hit.normal) * climbSpeed;
                movementDirection = new Vector3(0, movementDirection.y * inputManager.MovementNormalized().y, 0) + VisualPlayer.transform.right * inputManager.MovementNormalized().x * climbSpeed;
                if (inputManager.MovementNormalized() != Vector2.zero)
                {

                }
            }
            else
            {
                canClimb = false;
                wallCheckControl = true;
            }
        }
    }

    public Vector3 MovementDirection()
    {
        return movementDirection;
    }

    void OnDrawGizmos()
    {
        Vector3 rayDirection = (VisualPlayer.transform.forward).normalized;
        Gizmos.DrawRay(VisualPlayer.transform.position, rayDirection * tempRayDistance);
        Gizmos.DrawRay(climbRayObject.transform.position, climbRayObject.transform.forward * 0.75f);
    }
}
