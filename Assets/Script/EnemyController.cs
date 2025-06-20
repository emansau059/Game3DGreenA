using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] private bool isMovingBack;
    [SerializeField] private bool isHomePos;
    [SerializeField] Vector3 homePos;
    [SerializeField] Vector3 startPos;
    [SerializeField] private float followSpeed;
    [SerializeField] private float limitChase;

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

        Vector3 dirTarget = target.position - transform.position;
        Vector3 dirHomePos = homePos - transform.position;

        float distanceToTarget = dirTarget.sqrMagnitude;
        float distanceToHome = dirHomePos.sqrMagnitude;

        if (distanceToTarget >= limitChase)
        {
            isMovingBack = true;
        }

        if (isMovingBack)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, homePos, followSpeed * Time.deltaTime);
            if (distanceToHome < 0.1f)
            {
                isMovingBack = false;
            }
        }
        else
        {
            Debug.Log(distanceToTarget);

            if (distanceToTarget <= 3.5f) return;
                this.transform.position = Vector3.MoveTowards(this.transform.position, target.position, followSpeed * Time.deltaTime);
        }
    }

}
