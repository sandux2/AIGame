using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Guard Patrols area", story: "[guard] patrols [area] for [seconds]", category: "Action", id: "19762204eb0505fe362452645c2c4ba8")]
public partial class GuardPatrolsAreaAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Guard;
    [SerializeReference] public BlackboardVariable<List<GameObject>> Area;
    [SerializeReference] public BlackboardVariable<float> Seconds;
    NavMeshAgent agent;
    int currentSearchPoint = 1;
    Animator animator;
    float timer;

    protected override Status OnStart()
    {
        agent = Guard.Value.GetComponent<NavMeshAgent>(); // get NavMeshAgent component from guard
        animator = Guard.Value.GetComponentInChildren<Animator>(); // get Animator component from guard
        timer = Seconds.Value; // set patrol timer
        if(Guard.Value == null || Area.Value == null || Seconds.Value <= 0)
        {
            Debug.LogWarning("Guard, Area, or Seconds reference is invalid!");
            return Status.Failure;
        }
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        agent.SetDestination(Area.Value[currentSearchPoint].transform.position); // set destination to current search point

        if (Vector3.Distance(Guard.Value.transform.position, Area.Value[currentSearchPoint].transform.position) < 1f)
        {
            
            animator.SetBool("LookAround", true);
                while(timer > 0)
                {
                    timer -= Time.deltaTime; // wait at search point

                    return Status.Running;
                }

            animator.SetBool("LookAround", false);
            timer = Seconds.Value; // reset timer for next search point 
         
            currentSearchPoint = (currentSearchPoint + 1) % Area.Value.Count;
            if(currentSearchPoint == 0) // if we've looped through all search points
            {
                animator.SetBool("LookAround", false);
                return Status.Success; // patrol complete
            }
          
        }
        return Status.Running; // still patrolling
    }

    protected override void OnEnd()
    {
    }
}

