using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public float noteSpeed;
    private void FixedUpdate()
    {
        transform.position += Vector3.left * noteSpeed * Time.fixedDeltaTime;
    }
}
