using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimingBar : MonoBehaviour
{
    public float speed = 1;
    public bool isMove = false;
    private void Update()
    {
        if(isMove)
        {
            transform.Translate(CenterTap.BAR_OUT_RANGE * 2 * speed * Time.deltaTime / 60f, 0, 0);
            if (transform.localPosition.x > CenterTap.BAR_OUT_RANGE)
            {
                MoveLeftEnd();
            }
        }
    }

    public void MoveLeftEnd()
    {
        transform.localPosition = new Vector3(-CenterTap.BAR_OUT_RANGE, transform.localPosition.y, transform.localPosition.z);
    }

    public void ChangeColor(Color color)
    {
        GetComponent<Image>().color = color;
    }
}
