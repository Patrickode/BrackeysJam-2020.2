using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkInEnding : MonoBehaviour
{
    [SerializeField] [Range(0f, 1.5f)] private float shrinkSpeed = 0.5f;
    private Vector3 originalScale;

    private void Awake()
    {
        EventDispatcher.AddListener<EventDefiner.LevelEnd>(OnLevelEnd);
    }
    private void OnDestroy()
    {
        EventDispatcher.RemoveListener<EventDefiner.LevelEnd>(OnLevelEnd);
    }

    private void OnLevelEnd(EventDefiner.LevelEnd evt)
    {
        if (evt.LevelSuccess)
        {
            StartCoroutine(PlayerShrink());
        }
    }

    private IEnumerator PlayerShrink()
    {
        float progress = 0f;
        originalScale = transform.localScale;

        while (progress < 1)
        {
            progress += Time.deltaTime / shrinkSpeed;
            transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, progress);
            yield return null;
        }

        progress = 0f;
    }
}
