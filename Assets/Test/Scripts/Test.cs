using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = false; //마우스 커서 보이기 상태
        Cursor.lockState = CursorLockMode.Locked; //마우스 커서를 고정
    }
}
