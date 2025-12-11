using System;
using UnityEngine;
using MathNet.Numerics;

public class Lift : MonoBehaviour
{
    private GameObject player;
    private Rigidbody playerPhysics;
    [SerializeField] private float dragCoefficient = 1f;
    [SerializeField] private float area;
    [SerializeField] private float fluidDensity = 0.01f;


    private float height;
    private float width;

    private double areaIntegrationByR;
    public float dragDirection;
    public Vector3 liftDirection;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var renderer = GetComponent<MeshRenderer>();
        height =renderer.bounds.size.y;
        width =renderer.bounds.size.x;
        area = height * width;
        Func<double, double, double> f = (x,y) => Math.Pow((x*x + y*y), 3d/2d);
        areaIntegrationByR = Integrate.OnRectangle(f, - width/2f, width/2f, -height/2f, height/2f);



        player = GameObject.Find("Player");
        playerPhysics = player.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        applyDrag();
        //applyLift();
        applyAngularDrag();

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
        Vector3 Force = -(0.5f * playerPhysics.linearVelocity * playerPhysics.linearVelocity.magnitude * area * projection * fluidDensity * dragCoefficient);

        playerPhysics.AddForce(Force);
        applyTorque(Force);
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
        Vector3 angularDrag = -(1f/2f)*dragCoefficient * (float)areaIntegrationByR * fluidDensity * playerPhysics.angularVelocity * playerPhysics.angularVelocity.magnitude;
        playerPhysics.AddTorque(angularDrag);
    }
}
