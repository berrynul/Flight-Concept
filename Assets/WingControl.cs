using UnityEngine;
using UnityEngine.InputSystem;


public class WingControl : MonoBehaviour
{

    [SerializeField] private float bufferOffset = 0f;
    [SerializeField] private float minPitch = -30f;
    [SerializeField] private float maxPitch = 30f;
    [SerializeField] private float minTilt = -30f;
    [SerializeField] private float maxTilt = 30f;
    private Transform leftWing;
    private Transform rightWing;

    InputAction flapInput; 

    private Quaternion leftCache;
    private Quaternion rightCache;
    // Rotate once when the scene starts

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

    //Converts the mouse coords to be relative to the center of the screen, clamps the magnitude to keep the max values within the radius of the circle, and then inversely scales them to the radius
    Vector3 convertCoords(Vector3 coords)
    {
       float radius = Screen.height/2 - bufferOffset;
       Vector3 origin = new Vector3(Screen.width/2, Screen.height/2, 0);

       Vector3 heading = coords - origin;
       Vector3 hClamped = Vector3.ClampMagnitude(heading, radius);
       Vector3 hScaled = hClamped/radius;
       Vector3 normalized = (hScaled + new Vector3(1, 1))/2;

       return hScaled;

    }



    void Update()
    {
        //maybe need to change how mouse coords are accessed in the future
        Vector3 wingFactor = convertCoords(Mouse.current.position.ReadValue());

        WingPitch(leftWing, Mathf.Lerp(minPitch, maxPitch, wingFactor.y), -45, leftCache);
        WingPitch(rightWing, Mathf.Lerp(minPitch, maxPitch, wingFactor.y), 45, rightCache);
        //Debug.Log( Mathf.Lerp(minPitch, maxPitch, wingFactor.y));

        WingSway(leftWing, Mathf.Lerp(-30f, 30f, wingFactor.x), leftCache);
        WingSway(rightWing, Mathf.Lerp(-30f, 30f, wingFactor.x), rightCache);

    }

    private void WingSway(Transform t, float degrees, Quaternion cache)
    {
        var step = 20f * Time.deltaTime;
        Vector3 axisOfRotation = Vector3.up;


        var swayDelta =  Quaternion.AngleAxis(degrees, Vector3.up);
        var swayTo = swayDelta * cache;
        t.rotation = Quaternion.RotateTowards(t.rotation, swayTo, step);
    }

    private void WingPitch(Transform t, float degrees, float axis, Quaternion cache)
    {
        var step = 20f * Time.deltaTime;


        var axisOfRotation = Vector3.right /* *Quaternion.AngleAxis(axis, Vector3.forward) */ ;

        //var relativeRotation = Quaternion.AngleAxis(axis, Vector3.forward);
        //var baseRotation = Quaternion.AngleAxis(45, Vector3.forward);

        // Rotate 30 degrees around the Y axis in local space
        //
        var pitchDelta = Quaternion.AngleAxis(degrees, axisOfRotation);


        var pitchTo = pitchDelta * cache;
        t.localRotation = Quaternion.RotateTowards(t.localRotation, pitchTo, step);
    }
}

