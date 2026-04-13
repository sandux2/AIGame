using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using System.Threading;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "show game over screen", story: "show game over [screen]", category: "Action", id: "bbeefd1739934deb8145ac77f2995e99")]
public partial class ShowGameOverScreenAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Screen;

    protected override Status OnStart()
    {
        if (Screen.Value != null)
        {
            Screen.Value.SetActive(true); // show game over screen
            Time.timeScale = 0f; // pause game time
            return Status.Success;
        }
        else {

            Debug.LogWarning("Screen reference is null!");
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

