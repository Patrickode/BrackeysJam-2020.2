using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionHandler : MonoBehaviour
{
    [SerializeField] private Animator spiralAnim = null;
    [SerializeField] private Animator clockAnim = null;
    [SerializeField] private Animator winAnim = null;
    [SerializeField] private Animator loseAnim = null;

    private bool levelEnding = false;

    private void Awake()
    {
        EventDispatcher.AddListener<EventDefiner.Rewind>(OnRewind);
        EventDispatcher.AddListener<EventDefiner.LevelEnd>(OnLevelEnd);
    }
    private void OnDestroy()
    {
        EventDispatcher.RemoveListener<EventDefiner.Rewind>(OnRewind);
        EventDispatcher.RemoveListener<EventDefiner.LevelEnd>(OnLevelEnd);
    }

    private void OnRewind(EventDefiner.Rewind evt)
    {
        if (!levelEnding)
        {
            spiralAnim.SetTrigger("Start");
            clockAnim.SetTrigger("Start");
        }
    }

    private void OnLevelEnd(EventDefiner.LevelEnd evt)
    {
        if (!levelEnding)
        {
            levelEnding = true;

            if (evt.LevelSuccess)
            {
                winAnim.SetTrigger("Start");
            }
            else
            {
                loseAnim.SetTrigger("Start");
            }
        }
    }
}
