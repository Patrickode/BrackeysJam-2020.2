using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwivel : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 125;

    private float currentSwivelValue;

    private bool levelEnding = false;

    private void Awake()
    {
        EventDispatcher.AddListener<EventDefiner.LevelEnd>(OnLevelEnd);
    }
    private void OnDestroy()
    {
        EventDispatcher.RemoveListener<EventDefiner.LevelEnd>(OnLevelEnd);
    }
    private void OnLevelEnd(EventDefiner.LevelEnd evt) { levelEnding = true; }

    private void GetSwivelInput()
    {
        currentSwivelValue = 0;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            currentSwivelValue--;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            currentSwivelValue++;
        }
    }

    void Update()
    {
        if (!levelEnding)
        {
            GetSwivelInput();
        }

        if (!Mathf.Approximately(currentSwivelValue, 0))
        {
            transform.Rotate(0, rotateSpeed * currentSwivelValue * Time.deltaTime, 0);
        }
    }
}
