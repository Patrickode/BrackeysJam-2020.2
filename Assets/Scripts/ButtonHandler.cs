using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    [SerializeField] private List<BreakableHandler> linkedBreakables;
    [SerializeField] private float buttonCooldown = 0.5f;
    private Coroutine cooldownCor = null;
    private bool coolingDown = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (!coolingDown && collision.gameObject.CompareTag("Player"))
        {
            foreach (BreakableHandler breakable in linkedBreakables)
            {
                breakable.ToggleAndBurst();
            }

            if (cooldownCor != null) { StopCoroutine(cooldownCor); }
            cooldownCor = StartCoroutine(ButtonCooldown(buttonCooldown));
        }
    }

    private IEnumerator ButtonCooldown(float cooldownLength)
    {
        coolingDown = true;
        yield return new WaitForSeconds(cooldownLength);
        coolingDown = false;
    }
}
