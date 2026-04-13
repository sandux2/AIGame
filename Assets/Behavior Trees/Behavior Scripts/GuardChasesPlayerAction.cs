using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Guard chases player", story: "[guard] chases [player]", category: "Action", id: "03e23c2ad8a009373d3bb7c0df398ebc")]
public partial class GuardChasesPlayerAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Guard;
    [SerializeReference] public BlackboardVariable<GameObject> Player;
    NavMeshAgent agent;
    Animator animator;
    protected override Status OnStart()
    {
        if(Guard.Value == null || Player.Value == null)
        {
            Debug.LogWarning("Guard or Player reference is invalid!");
            return Status.Failure;
        }
        agent = Guard.Value.GetComponent<NavMeshAgent>(); // get NavMeshAgent component from guard
        animator = Guard.Value.GetComponentInChildren<Animator>(); // get Animator component from guard
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
                        agent.SetDestination(Player.Value.transform.position);


                

                float distance = Vector3.Distance(
                    new Vector3(Guard.Value.transform.position.x, 0, Guard.Value.transform.position.z),
                    new Vector3(Player.Value.transform.position.x, 0, Player.Value.transform.position.z)
                );

                if (distance <= 1.5f) // if guard is close enough to player, consider player caught
                {
                    Debug.Log("Player caught!");


                    animator.SetTrigger("Chaught");

                    Player.Value.gameObject.GetComponent<PlayerMovement>().enabled = false; // disable player movement
                    Player.Value.gameObject.GetComponent<PlayerInteract>().enabled = false; // disable player interaction
                    return Status.Success; // player caught
                }
        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}

