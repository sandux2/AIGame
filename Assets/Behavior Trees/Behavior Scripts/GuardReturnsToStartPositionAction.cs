using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "guard returns to start position", story: "[guard] returns to start [position]", category: "Action", id: "bb5a2b41d52b7461d21ed5cc85d4fb9f")]
public partial class GuardReturnsToStartPositionAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Guard;
    [SerializeReference] public BlackboardVariable<Vector3> Position;
    NavMeshAgent agent;
    BehaviorGraphAgent behaviorGraphAgent;
    Animator animator;
    protected override Status OnStart()
    {
        if(Guard.Value == null || Position.Value == null)
        {
            Debug.LogWarning("Guard or Position reference is invalid!");
            return Status.Failure;
        }
        agent = Guard.Value.GetComponent<NavMeshAgent>(); // get NavMeshAgent component from guard
        agent.SetDestination(Position.Value); // set destination to start Position
        behaviorGraphAgent = Guard.Value.GetComponent<BehaviorGraphAgent>(); // get BehaviorGraphAgent component from guard
        animator = Guard.Value.GetComponentInChildren<Animator>(); // get Animator component from guard
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if(Vector3.Distance(Guard.Value.transform.position, Position.Value) < 1f) // check if guard has reached start position
        {
            behaviorGraphAgent.BlackboardReference.SetVariableValue("Player Spotted", false); // reset chase variable in blackboard
                animator.SetBool("Walk", false); // stop walking animation
            return Status.Success; // reached start position
        }
        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}

