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
    [Header("References")]
    [SerializeField] private Transform outerCore = null;
    [SerializeField] private Transform innerCore = null;
    [SerializeField] private Renderer outerCoreRend = null;
    [SerializeField] private GameObject recoveryBurst = null;

    [Header("Cooldown")]
    [SerializeField] [Range(0f, 5f)] private float recordCooldown = 1f;
    private float cooldownTimer = 0f;
    private bool onRecordCooldown = false;

    [Header("Rewind Pause")]
    [SerializeField] [Range(0f, 2f)] private float rewindPauseTime = 1f;
    private float pauseTimer = 0f;
    private bool rewindPause;

    private Vector3 outerCoreScale;
    private Vector3 innerCoreScale;
    private Color outerCoreColor;

    private PointInTime recordedPoint = null;

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

    private void Start()
    {
        outerCoreScale = outerCore.localScale;
        innerCoreScale = innerCore.localScale;
        outerCoreColor = outerCoreRend.material.color;
    }

    private void Update()
    {
        if (rewindPause)
        {
            pauseTimer += Time.unscaledDeltaTime / rewindPauseTime;

            if (pauseTimer >= 1)
            {
                rewindPause = false;
                pauseTimer = 0;
                Time.timeScale = 1;
            }
        }
        //If recording is not cooling down,
        else if (!onRecordCooldown)
        {
            //Allow recording / rewinding to the recorded point, if the level isn't ending.
            if (!levelEnding && Input.GetMouseButtonDown(0))
            {
                if (recordedPoint == null)
                {
                    RecordPoint();
                }
                else
                {
                    RewindToPoint();
                    SetCooldown(true);
                }
            }
        }
        else
        {
            //Add to the cooldown timer, formatted to go from 0 to 1.
            cooldownTimer += Time.deltaTime / recordCooldown;

            if (cooldownTimer < 1)
            {
                //Gradually scale the outer core up during cooldown.
                outerCore.localScale = Vector3.Lerp(innerCoreScale, outerCoreScale, cooldownTimer);

                //Gradually fade the outer core in during cooldown.
                Color smoothStepColor = outerCoreColor;
                smoothStepColor.a = Mathf.SmoothStep(0, outerCoreColor.a, cooldownTimer);
                outerCoreRend.material.color = smoothStepColor;
            }
            else { SetCooldown(false); }
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
        outerCore.parent = null;
        outerCore.position = recordedPoint.position;
        outerCore.rotation = recordedPoint.rotation;
    }

    /// <summary>
    /// Moves the player back to the recorded point in time, and does other relevant logic.
    /// </summary>
    private void RewindToPoint()
    {
        //Move the player to the recorded PointInTime.
        transform.position = recordedPoint.position;
        transform.rotation = recordedPoint.rotation;

        //Reparent the outer core to the player and make sure it lines up with the player.
        outerCore.parent = transform;
        outerCore.position = transform.position;
        outerCore.rotation = transform.rotation;

        //Erase the old recorded point because we just used it up.
        recordedPoint = null;
    }

    /// <summary>
    /// Turns the record cooldown on or off, and does other relevant logic.
    /// </summary>
    /// <param name="onCooldown"></param>
    private void SetCooldown(bool onCooldown)
    {
        //Set the cooldown status accordingly and reset the cooldownTimer.
        onRecordCooldown = onCooldown;
        cooldownTimer = 0f;

        //Set the outer core's scale to the inner core if on cooldown. Otherwise, set it to its normal scale.
        //This is so the outer core can grow during cooldown.
        outerCore.localScale = onCooldown ? innerCoreScale : outerCoreScale;
        //Make the outer core transparent if on cooldown. Otherwise, make it normally colored.
        //This is so the outer core fades in during cooldown.
        outerCoreRend.material.color = onCooldown ? Color.clear : outerCoreColor;

        //Enable the recovery burst object, which will let out a burst of particles and disable itself
        if (!onCooldown) { recoveryBurst.SetActive(true); }
        //Pause everything for a short time after rewinding time
        else
        {
            rewindPause = true;
            Time.timeScale = 0;
            EventDispatcher.Dispatch(new EventDefiner.Rewind(rewindPauseTime));
        }
    }
}