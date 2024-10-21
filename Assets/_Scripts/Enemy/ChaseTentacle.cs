using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseTentacle : MonoBehaviour
{
    public static Action OnChaseTentacleDeath;

    private void OnDisable() => OnChaseTentacleDeath?.Invoke();
}
