using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionHandler : MonoBehaviour
{
    [SerializeField] private Animator spiralAnim = null;
    [SerializeField] private Animator clockAnim = null;

    private void Awake()
    {
        EventDispatcher.AddListener<EventDefiner.Rewind>(OnRewind);
    }
    private void OnDestroy()
    {
        EventDispatcher.RemoveListener<EventDefiner.Rewind>(OnRewind);
    }

    private void OnRewind(EventDefiner.Rewind evt)
    {
        spiralAnim.SetTrigger("Start");
        clockAnim.SetTrigger("Start");
    }
}
