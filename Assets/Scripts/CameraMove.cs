using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float panSpeed = 20f;
    public float panBorderThickness = 10f;
    public static Vector2 panLimit;
    public static Vector2 cameraCenteredCoords;

    private void Update()
    {
        Vector3 pos = transform.position;
        if (Input.GetKey("w"))
        {
            pos.y += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("s"))
        {
            pos.y -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("d"))
        {
            pos.x += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("a"))
        {
            pos.x -= panSpeed * Time.deltaTime;
        }


        pos.x = Mathf.Clamp(pos.x, cameraCenteredCoords.x - panLimit.x, cameraCenteredCoords.x + panLimit.x);
        pos.y = Mathf.Clamp(pos.y, cameraCenteredCoords.y - panLimit.y, cameraCenteredCoords.y + panLimit.y);

        transform.position = pos;
    }
}
