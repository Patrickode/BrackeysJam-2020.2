using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Renderer rend;
    [SerializeField] private Collider coll;
    [SerializeField] private ParticleSystem pSystem = null;

    [Header("Materials")]
    [SerializeField] private Material onMaterial = null;
    [SerializeField] private Material offMaterial = null;

    private float maxScaleAxis = 0f;

#if UNITY_EDITOR
    [Space(10)]
    [SerializeField] private bool toggleRendAndColl = false;

    private void OnValidate()
    {
        if (toggleRendAndColl)
        {
            rend.material = !coll.enabled ? onMaterial : offMaterial;
            coll.enabled = !coll.enabled;

            toggleRendAndColl = false;
        }
    }
#endif

    private void Start()
    {
        Vector3 scale = transform.localScale;
        maxScaleAxis = Mathf.Max(scale.x, scale.y);
        maxScaleAxis = Mathf.Max(maxScaleAxis, scale.z);
    }

    public void ToggleAndBurst()
    {
        rend.material = !coll.enabled ? onMaterial : offMaterial;
        coll.enabled = !coll.enabled;
        pSystem.Emit(Mathf.CeilToInt(maxScaleAxis * 10));
    }
}
