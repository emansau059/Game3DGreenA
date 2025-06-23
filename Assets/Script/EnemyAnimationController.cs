using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimationController : MonoBehaviour
{
    private Animator _anim;

    NavMeshAgent agent;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if(EnemyController.instance.distanceToHome > 0.1f)
        {
            float motion = EnemyController.instance.dirTarget.sqrMagnitude;

            _anim.SetFloat("motion", motion);
        }
        else
        {
            _anim.SetFloat("motion", 0f);
        }
    }
}
