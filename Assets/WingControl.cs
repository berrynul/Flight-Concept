using UnityEngine;
using UnityEngine.InputSystem;

public class WingControl : MonoBehaviour
{
    [SerializeField] private float bufferOffset = 0f;
    [SerializeField] private float maxPitch = 5f;
    [SerializeField] private float maxSway = 5f;
    [SerializeField] private float rotationSpeed = 20f;

    InputAction flapInput;

    private Transform leftWing;
    private Transform rightWing;
    private Quaternion leftCache;
    private Quaternion rightCache;

    void Awake()
    {
        leftWing = transform.Find("Left");
        rightWing = transform.Find("Right");
        flapInput = InputSystem.actions.FindAction("Flight");
    }

    void Start()
    {
        leftCache = leftWing.localRotation;
        rightCache = rightWing.localRotation;
    }


    Vector2 convertCoords()
    {

        float radius = Screen.height/2f - bufferOffset; // float conversion? idk.. weird
        Vector2 origin = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector2 heading = mousePos - origin;
        Vector2 clamped = Vector2.ClampMagnitude(heading, radius);
        return clamped / radius;  // [-1, 1] range
    }

    void Update()
    {
        Vector2 cursorRelativePosition = convertCoords();


        float pitchDegrees = cursorRelativePosition.y * maxPitch;
        float swayDegrees = cursorRelativePosition.x * maxSway;


        UpdateWing(leftWing, pitchDegrees, swayDegrees, leftCache);
        UpdateWing(rightWing, pitchDegrees, swayDegrees, rightCache);
    }

    private void UpdateWing(Transform wing, float pitchDegrees, float swayDegrees, Quaternion cache)
    {
        float step = rotationSpeed * Time.deltaTime;

        // Build the combined target rotation from the cached base

        Quaternion pitch = Quaternion.AngleAxis(pitchDegrees, Vector3.right);
        Quaternion sway = Quaternion.AngleAxis(swayDegrees, Vector3.up);

        Quaternion target = cache * pitch * sway;

        wing.localRotation = Quaternion.RotateTowards(wing.localRotation, target, step);

    }
}
