using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private static EnemyController _instance;
    public static EnemyController instance => _instance;

    [SerializeField] Transform target;
    [SerializeField] private bool isMovingBack;
    [SerializeField] private bool isHomePos;
    [SerializeField] Vector3 homePos;
    [SerializeField] private Vector3 _dirTarget;
    public Vector3 dirTarget => _dirTarget;
    [SerializeField] private float _distanceToHome;
    public float distanceToHome => _distanceToHome;
    [SerializeField] private float followSpeed;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float limitChase;

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else if(_instance.gameObject.GetInstanceID() != this.gameObject.GetInstanceID())
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        homePos = this.transform.position;
        isMovingBack = false;
       
    }

    // Update is called once per frame
    void Update()
    {
        ChaseThePlayer();
        

    }
    void ChaseThePlayer()
    {
        if (!target) return;

        _dirTarget = target.position - transform.position;
        Vector3 dirHomePos = homePos - transform.position;

        float distanceToTarget = _dirTarget.sqrMagnitude;
        _distanceToHome = dirHomePos.sqrMagnitude;

        // Check if target is too far and we need to return home
        if (distanceToTarget >= limitChase && !isMovingBack)
        {
            isMovingBack = true;
        }

        Vector3 targetPosition;
        float currentSpeed = followSpeed;

        if (isMovingBack)
        {
            targetPosition = homePos;
            //// Gradually slow down as we approach home position
            //if (_distanceToHome < 5f)
            //{
            //    currentSpeed = followSpeed * (_distanceToHome / 5f);
            //}

            if (_distanceToHome < 0.1f)
            {
                transform.position = homePos; // Snap to exact position
                isMovingBack = false;
                return;
            }
        }
        else
        {
            // Don't chase if too close to target
            if (distanceToTarget <= 3.5f) return;
            targetPosition = target.position;
        }

        // Calculate movement direction
        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        
        // Rotate towards movement direction
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Smooth movement
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, currentSpeed * Time.deltaTime);
    }

}
