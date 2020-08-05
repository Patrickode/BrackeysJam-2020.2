using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointInTime
{
    public Vector3 position;
    public Quaternion rotation;

    public PointInTime(Vector3 position, Quaternion rotation)
    {
        this.position = position;
        this.rotation = rotation;
    }
}

public class PlayerRewind : MonoBehaviour
{
    [SerializeField] Rigidbody rb = null;
    [SerializeField] GameObject outerCore = null;

    [SerializeField] [Range(0f, 5f)] float recordCooldown = 1f;
    private float recordTimer = 0f;
    private bool onRecordCooldown = false;

    private PointInTime recordedPoint = null;

    private void Update()
    {
        if (!onRecordCooldown)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (recordedPoint == null)
                {
                    RecordPoint();
                }
                else
                {
                    RewindToPoint();
                    onRecordCooldown = true;
                }
            }
        }
        else
        {
            recordTimer += Time.deltaTime;

            if (recordTimer >= recordCooldown)
            {
                recordTimer = 0f;
                onRecordCooldown = false;
            }
        }
    }

    /// <summary>
    /// Record a point in time and do all appropriate logic.
    /// </summary>
    private void RecordPoint()
    {
        //Store the current position and rotation in a PointInTime.
        recordedPoint = new PointInTime(transform.position, transform.rotation);

        //Now uparent the player's outer core from the player and put it in the recorded position.
        outerCore.transform.parent = null;
        outerCore.transform.position = recordedPoint.position;
        outerCore.transform.rotation = recordedPoint.rotation;
    }

    private void RewindToPoint()
    {
        //Move the player to the recorded PointInTime.
        transform.position = recordedPoint.position;
        transform.rotation = recordedPoint.rotation;

        //Reparent the outer core to the player and make sure it lines up with the player.
        outerCore.transform.parent = transform;
        outerCore.transform.position = transform.position;
        outerCore.transform.rotation = transform.rotation;

        //Erase the old recorded point because we just used it up.
        recordedPoint = null;
    }
}
