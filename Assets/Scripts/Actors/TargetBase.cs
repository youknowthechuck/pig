using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class TargetBase : PigScript
{

    protected override void OnEnabled()
    {
        EventCore.AddListener<DamageEvent>(HandleDamageEvent);
    }

    protected override void OnDisabled()
    {
        EventCore.RemoveListener<DamageEvent>(HandleDamageEvent);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void HandleDamageEvent(DamageEvent e)
    {
        Health hp = GetComponent<Health>();

        hp.TakeDamage(e.damageAmmount);

        if (!hp.Alive)
        {
            Destroy(gameObject);
        }
    }
}
