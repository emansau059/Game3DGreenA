using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterMainEngine : MonoBehaviour
{
    private Rigidbody helicopterRigid;
    public BladesController MainBlade;
    public BladesController SubBlade;

    private float enginePower;
    public float EnginePower
    {
        get
        {
            return enginePower;
        }
        set
        {
            MainBlade.BladeSpeed = value * 250;
            SubBlade.BladeSpeed = value * 500;
            enginePower = value;
        }
    }

    public float effectiveHeight;
    public float EngineLift = 0.3f;

    public float ForwardForce;
    public float BackwardForce;
    public float TurnForce;
    private float TurnForcehelper = 1.5f;
    public float ForwardtiltForce;
    public float TurntiltForcer;

    private Vector2 movement = Vector2.zero;
    private Vector2 TILTING = Vector2.zero;

    public LayerMask groundLayer;

    private float distanceToground;
    public bool isOnGround = true;
    private float turning = 0f;
    // Start is called before the first frame update
    void Start()
    {
        helicopterRigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleGroundCheck();
        HandleInputs();

    }
    private void FixedUpdate()
    {
        HelicopterHover();
        HelicopterMovement();
        HelicopterTilting();
    }
    void HandleInputs()
    {
        if (!isOnGround)
        {
            movement.x = Input.GetAxis("Horizontal");
            movement.y = Input.GetAxis("Vertical");

            if (Input.GetKey(KeyCode.C))
            {
                EnginePower -= EngineLift;
                if (enginePower < 0)
                {
                    EnginePower = 0;
                }
            }
        }

        if (Input.GetAxis("Throttle") > 0)
        {
            EnginePower += EngineLift;
        }
        else if (Input.GetAxis("Vertical") > 0 && !isOnGround)
        {
            EnginePower = Mathf.Lerp(EnginePower, 17.5f, 0.003f);
        }
        else if (Input.GetAxis("Throttle") < 0.5f && !isOnGround)
        {
            EnginePower = Mathf.Lerp(EnginePower, 10f, 0.003f);
        }
    }
    void HandleGroundCheck()
    {
        RaycastHit hit;
        Vector3 direction = transform.TransformDirection(Vector3.down);
        Ray ray = new Ray(transform.position, direction);
        if (Physics.Raycast(ray, out hit, 3000, groundLayer))
        {
            distanceToground = hit.distance;
            if (distanceToground < 2)
            {
                isOnGround = true;
            }
            else
            {
                isOnGround = false;
            }
        }
    }
    void HelicopterHover()
    {
        float upForce = 1 - Mathf.Clamp(helicopterRigid.transform.position.y / effectiveHeight, 0, 1);
        upForce = Mathf.Lerp(0, EnginePower, upForce) * helicopterRigid.mass;
        helicopterRigid.AddRelativeForce(Vector3.up * upForce);
    }
    void HelicopterMovement()
    {
        if (Input.GetAxis("Vertical") > 0)
        {
            helicopterRigid.AddRelativeForce(Vector3.forward * Mathf.Max(0f, movement.y * ForwardForce * helicopterRigid.mass));

        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            helicopterRigid.AddRelativeForce(Vector3.back * Mathf.Max(0f, -movement.y * BackwardForce * helicopterRigid.mass));
        }

        float turn = TurnForce * Mathf.Lerp(movement.x, movement.x * (TurnForcehelper - Mathf.Abs(movement.y)), Mathf.Max(0f, movement.y));
        turning = Mathf.Lerp(turning, turn, Time.fixedDeltaTime * TurnForce);
        helicopterRigid.AddRelativeTorque(0f, turning * helicopterRigid.mass, 0f);
    }
    void HelicopterTilting()
    {
        TILTING.y = Mathf.Lerp(TILTING.y, movement.y * ForwardtiltForce, Time.deltaTime);
        TILTING.x = Mathf.Lerp(TILTING.x, movement.x * TurntiltForcer, Time.deltaTime);
        helicopterRigid.transform.localRotation = Quaternion.Euler(TILTING.y, helicopterRigid.transform.localEulerAngles.y, -TILTING.x);
    }
}
