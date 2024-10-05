using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChamberTentacle : MonoBehaviour
{
    public static Action<ChamberTentacle> OnChamberTentacleDestroyed;

    private void OnDestroy() => OnChamberTentacleDestroyed?.Invoke(this);
}
