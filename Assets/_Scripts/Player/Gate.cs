using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public void Open()
    {
        GetComponent<Animator>().SetTrigger("Open");
        // Play Audio
    }
}
