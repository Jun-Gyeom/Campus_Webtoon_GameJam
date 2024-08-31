using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static GameControls InputAsset;
    
    // 델리게이트 선언
    public delegate void KeyAction();
    
    // 입력 이벤트 선언
    public static event KeyAction OnUp;     // 위쪽 노트 입력
    public static event KeyAction OnDown;   // 아래쪽 노트 입력
    public static event KeyAction OnLeft;   // 왼쪽 노트 입력
    public static event KeyAction OnRight;  // 오른쪽 노트 입력 

    private void Awake()
    {
        InputAsset = new GameControls();
    }

    private void Update()
    {
        InputAsset.GamePlay.Up.performed += ctx => Up();
        InputAsset.GamePlay.Down.performed += ctx => Down();
        InputAsset.GamePlay.Left.performed += ctx => Left();
        InputAsset.GamePlay.Right.performed += ctx => Right();
        
        /*
        switch(Input.inputString)
        {
            case "w":
                Up();
                break;
            case "s":
                Down();
                break;
            case "a":
                Left();
                break;
            case "d":
                Right();
                break;
        }
        
        */
    }

    private void OnEnable()
    {
        InputAsset.Enable();
    }

    private void OnDisable()
    {
        InputAsset.Disable();
    }

    private void Up()
    {
        Debug.Log("Up");
        OnUp?.Invoke();
    }
    
    private void Down()
    {
        OnDown?.Invoke();
    }
    
    private void Left()
    {
        OnLeft?.Invoke();
    }
    
    private void Right()
    {
        OnRight?.Invoke();
    }
}
