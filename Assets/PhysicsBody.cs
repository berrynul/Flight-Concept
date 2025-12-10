using UnityEngine;

public class PhysicsBody : MonoBehaviour
{

    private float mass = 10f;

    public Vector3 netForce = new Vector3(0, 0, 0);
    public Vector3 velocity = new Vector3(0, 0, 0);
    private Vector3 acceleration = new Vector3(0, 0, 0);
    private Vector3 torque;
    private float momentInertia;
    private Rigidbody rigidBody;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.mass = mass;
    }

    // Update is called once per frame
    void Update()
    {


        ApplyGravity();

    }

    void LateUpdate(){


        Debug.Log("Velocity "+ Vector3.Magnitude(rigidBody.linearVelocity));
        Debug.Log("Force"+ netForce);
        Debug.Log("RealForce" + acceleration* (mass));
        Debug.DrawRay(transform.position, netForce, Color.red);
        Debug.DrawRay(transform.position, velocity, Color.mediumAquamarine);

        netForce = new Vector3(0, 0, 0);


    }


    void ApplyTorque(){


    }

    void ApplyGravity()
    {
        rigidBody.AddForce(new Vector3(0, -0.2f, 0) * mass);

    }
}
