using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 노트가 닿으면 
public class DeadLine : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Note"))
        {
            // TODO - Bad 판정하기
            
            // 노트 삭제하기
            Destroy(other.gameObject);
        }
    }
}
