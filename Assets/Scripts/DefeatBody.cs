using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatBody : MonoBehaviour
{
    private bool levelEnding = false;

    private void Awake()
    {
        EventDispatcher.AddListener<EventDefiner.LevelEnd>(OnLevelEnd);
    }
    private void OnDestroy()
    {
        EventDispatcher.RemoveListener<EventDefiner.LevelEnd>(OnLevelEnd);
    }
    private void OnLevelEnd(EventDefiner.LevelEnd _) { levelEnding = true; }

    //If the player touches this object, whether its a collider or trigger, make the level end in failure.
    //Only do this if the level is not ending already.
    private void OnTriggerEnter(Collider other)
    {
        if (!levelEnding && other.CompareTag("Player"))
        {
            EventDispatcher.Dispatch(new EventDefiner.LevelEnd(false));
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!levelEnding && collision.gameObject.CompareTag("Player"))
        {
            EventDispatcher.Dispatch(new EventDefiner.LevelEnd(false));
        }
    }
}
