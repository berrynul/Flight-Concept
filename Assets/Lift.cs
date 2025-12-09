using UnityEngine;

public class Lift : MonoBehaviour
{
    private GameObject player;
    private PhysicsBody playerPhysics;
    [SerializeField] private float dragCoefficient = 1f;
    [SerializeField] private float area = 60f;
    [SerializeField] private float fluidDensity = 0.01f;

    public float dragDirection;
    public Vector3 liftDirection;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        player = GameObject.Find("Player");
        playerPhysics = player.GetComponent<PhysicsBody>();
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
        Debug.DrawRay(transform.position, dragDirection * Vector3.Dot(transform.forward, playerPhysics.velocity.normalized) * Vector3.Normalize(playerPhysics.velocity), Color.green);

        //lift
        Debug.DrawRay(transform.position, liftDirection, Color.blue);
    }
    void applyDrag()
    {


        Vector3 normal = transform.forward;
        float projection = Vector3.Dot(normal, playerPhysics.velocity.normalized);
        float Force = (0.5f * Mathf.Pow(Vector3.Magnitude(playerPhysics.velocity), 2) * area * fluidDensity * dragCoefficient);
        if(projection > 0){

            dragDirection = -1;

        }else if(projection < 0){
            dragDirection = 1;

        }else{
            dragDirection = 0;
        }

        playerPhysics.netForce += (dragDirection * projection * Force * Vector3.Normalize(playerPhysics.velocity));
        }

    void applyLift()
    {
        Vector3 normal = transform.forward;

        float projection = 1-Mathf.Abs((Vector3.Dot(normal, Vector3.Normalize(playerPhysics.velocity))));
        float planeform = projection * area;
        float Force = (1f/2f) * fluidDensity * Mathf.Pow(playerPhysics.velocity.magnitude, 2) * planeform;
        liftDirection =  Vector3.Normalize(Vector3.Cross(transform.right, Vector3.Normalize(playerPhysics.velocity)));



        Debug.Log(transform.up * Force);
        playerPhysics.netForce = ( transform.up * Force) + playerPhysics.netForce;


    }


}
