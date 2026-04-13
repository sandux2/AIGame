using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "OpenDoor", story: "Open front [door]", category: "Action", id: "26158bb827bb1427fbbc7dc1e5d5edde")]
public partial class OpenDoorAction : Action
{
    [SerializeReference] public BlackboardVariable<DoorController> Door;

    protected override Status OnStart()
    {
        if (Door.Value != null)
        {
            Door.Value.transform.parent.GetComponent<Animator>().SetBool("OpenDoor", true); // toggle door open/close
            return Status.Success;
        }
        else {

            Debug.LogWarning("Door reference is null!");
            return Status.Failure;
        }
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

