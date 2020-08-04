using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwivel : MonoBehaviour
{
    [SerializeField]
    private float rotateSpeed = 125;

    private float currentSwivelValue;

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
        GetSwivelInput();

        if (!Mathf.Approximately(currentSwivelValue, 0))
        {
            transform.Rotate(0, rotateSpeed * currentSwivelValue * Time.deltaTime, 0);
        }
    }
}
