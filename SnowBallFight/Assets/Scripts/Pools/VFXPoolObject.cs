using System;
using System.Collections;
using UnityEngine;

public class VFXPoolObject : PoolObject
{
    new VFXPoolObjectSettings settings => (VFXPoolObjectSettings) base.settings;

    private void OnEnable()
    {
        StartCoroutine(DespawnAfterDelay());
    }

    private IEnumerator DespawnAfterDelay()
    {
        yield return new WaitForSeconds(settings.despawnDelay);
        Despawn();
    }

    private void Despawn()
    {
        PoolFactory.Instance.ReleaseObject(this);
    }
}
