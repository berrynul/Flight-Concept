using UnityEngine;
using UnityEngine.InputSystem;


public class WingControl : MonoBehaviour
{

    [SerializeField] private float bufferOffset = 0f;
    [SerializeField] private float minPitch = -30f;
    [SerializeField] private float maxPitch = 30f;
    [SerializeField] private float minTilt = 30f;
    [SerializeField] private float maxTilt = 30f;
    private Transform leftWing;
    private Transform rightWing;

    InputAction flapInput; 

    // Rotate once when the scene starts

    void Awake()
    {
        leftWing = transform.Find("Left");
        rightWing = transform.Find("Right");
        flapInput = InputSystem.actions.FindAction("Flight");
    }

    void Start()
    {

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
       Debug.Log(hScaled);

       return hScaled;

    }



    void Update()
    {
        //maybe need to change how mouse coords are accessed in the future
        Vector3 wingFactor = convertCoords(Mouse.current.position.ReadValue());

        WingPitch(leftWing, Mathf.Lerp(minPitch, maxPitch, wingFactor.y));
        WingPitch(rightWing, Mathf.Lerp(minPitch, maxPitch, wingFactor.y));
        //Debug.Log( Mathf.Lerp(minPitch, maxPitch, wingFactor.y));



    }

    private void wingTilt(Transform t, float degrees)
    {
        var step = 20f * Time.deltaTime;
        var tiltTo = Quaternion.AngleAxis(degrees, Vector3.right);
        t.rotation = Quaternion.RotateTowards(t.rotation, tiltTo, step);
    }

    private void WingPitch(Transform t, float degrees)
    {
        var step = 20f * Time.deltaTime;

        // Rotate 30 degrees around the Y axis in local space
        var pitchTo = Quaternion.AngleAxis(degrees, Vector3.right);

        t.rotation = Quaternion.RotateTowards(t.rotation, pitchTo, step);
    }
}

