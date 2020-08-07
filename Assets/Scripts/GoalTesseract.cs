using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalTesseract : MonoBehaviour
{
    [SerializeField] [Range(0f, 6f)] private float newRotationInterval = 3f;

    private bool levelEnding = false;
    private Quaternion lastRot;
    private Quaternion destRot;
    private float nextRotTimer = 0f;

    private void Awake()
    {
        EventDispatcher.AddListener<EventDefiner.LevelEnd>(OnLevelEnd);
    }
    private void OnDestroy()
    {
        EventDispatcher.RemoveListener<EventDefiner.LevelEnd>(OnLevelEnd);
    }
    private void OnLevelEnd(EventDefiner.LevelEnd _) { levelEnding = true; }

    private void OnTriggerEnter(Collider other)
    {
        if (!levelEnding && other.CompareTag("Player"))
        {
            EventDispatcher.Dispatch(new EventDefiner.LevelEnd(true));
        }
    }

    private void Start() { ResetRots(); }
    private void Update()
    {
        nextRotTimer += Time.deltaTime / newRotationInterval;

        if (nextRotTimer < 1)
        {
            transform.rotation = Quaternion.Lerp(lastRot, destRot, nextRotTimer);
        }
        else { ResetRots(); }
    }

    private void ResetRots()
    {
        nextRotTimer = 0;
        lastRot = transform.rotation;
        destRot = Random.rotation;
    }
}
