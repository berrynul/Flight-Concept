using UnityEngine;
using MathNet.Numerics;

public class Lift : MonoBehaviour
{
    private GameObject player;
    private Rigidbody playerPhysics;
    [SerializeField] private float dragCoefficient = 1f;
    [SerializeField] private float area = 60f;
    [SerializeField] private float fluidDensity = 0.01f;

    public float dragDirection;
    public Vector3 liftDirection;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        player = GameObject.Find("Player");
        playerPhysics = player.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        applyDrag();
        applyLift();

    }

    void LateUpdate()
    {

        //drag direction/projected normal
        Debug.DrawRay(transform.position, dragDirection * Vector3.Dot(transform.forward, playerPhysics.linearVelocity.normalized) * Vector3.Normalize(playerPhysics.linearVelocity), Color.green);

        //lift
        //Debug.DrawRay(transform.position, liftDirection, Color.blue);
    }
    void applyDrag()
    {


        Vector3 normal = transform.forward;
        float projection = Vector3.Dot(normal, playerPhysics.linearVelocity.normalized);
        float Force = (0.5f * Mathf.Pow(Vector3.Magnitude(playerPhysics.linearVelocity), 2) * area * fluidDensity * dragCoefficient);
        if(projection > 0){

            dragDirection = -1;

        }else if(projection < 0){
            dragDirection = 1;

        }else{
            dragDirection = 0;
        }

        playerPhysics.AddForce(dragDirection * projection * Force * Vector3.Normalize(playerPhysics.linearVelocity));
        applyTorque(dragDirection * projection * Force * Vector3.Normalize(playerPhysics.linearVelocity));
    }

    void applyLift()
    {
        Vector3 normal = transform.forward;

        float projection = 1-Mathf.Abs((Vector3.Dot(normal, Vector3.Normalize(playerPhysics.linearVelocity))));
        float planeform = projection * area;
        float Force = (1f/2f) * fluidDensity * Mathf.Pow(playerPhysics.linearVelocity.magnitude, 2) * planeform;
        liftDirection =  Vector3.Normalize(Vector3.Cross(transform.right, Vector3.Normalize(playerPhysics.linearVelocity)));



        playerPhysics.AddForce(( liftDirection * Force));
        applyTorque(Force * liftDirection);

    }

    void applyTorque(Vector3 Force)
    {

        Vector3 r = transform.position - playerPhysics.worldCenterOfMass;
        playerPhysics.AddTorque(Vector3.Cross(r, Force));

    }

    void applyAngularDrag()
    {
        float angcoeff = 10f;
        playerPhysics.AddTorque( playerPhysics.angularVelocity);
    }
}
