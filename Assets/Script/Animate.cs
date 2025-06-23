using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Animate : MonoBehaviour
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
        float motion = agent.velocity.magnitude;

        _anim.SetFloat("motion", motion);
    }
}
