using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using FOV;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Guard detecting player", story: "[guard] detecting [player]", category: "Action", id: "d61866adecd8fc7489f49671d0990c4f")]
public partial class GuardDetectingPlayerAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Guard;
    [SerializeReference] public BlackboardVariable<GameObject> Player;
    public float DetectDistance = 7f; // distance at which guard can detect player
    public float DetectDistanceCrouched = 4f; // reduced detection distance when player is crouched
    FieldOfView view;
    Animator animator;
    protected override Status OnStart()
    {
        if(Guard.Value == null || Player.Value == null)
        {
            Debug.LogWarning("Guard or Player reference is invalid!");
            return Status.Failure;
        }
        view = Guard.Value.GetComponent<FieldOfView>(); // get FieldOfView component from guard
        animator = Guard.Value.GetComponentInChildren<Animator>(); // get Animator component from guard
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if(view.Field<Transform>().ToArray().Length != 0)
        {
            Debug.Log("Guard spotted the player in field of view!");
            
            animator.SetBool("LookAround", false);
            animator.SetBool("Walk", false);
            animator.SetBool("Run", true);
        } // check if player is in field of view

       if(Player.Value.GetComponent<PlayerMovement>().IsCrouched)
        {
            view.radius = 4f; // reduce view radius when player is crouched
        }
        else
        {
            view.radius = 7f; // reset view radius when player is not crouched
        }
        float DetectDistanceToUse = Player.Value.GetComponent<PlayerMovement>().IsCrouched ? DetectDistanceCrouched : DetectDistance; // use reduced distance if player is crouched

        if(Vector3.Distance(Guard.Value.transform.position, Player.Value.transform.position) < DetectDistanceToUse ) // if player is far away, stop chasing
        {
            Debug.Log("Guard spotted the player!");
                animator.SetBool("LookAround", false);
                animator.SetBool("Walk", false);
                animator.SetBool("Run", true);
                return Status.Success; // player detected
        }
        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}

