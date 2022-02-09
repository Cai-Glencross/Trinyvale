using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public Transform target;
    public float smoothing;

    public Vector2 minPosition;
    public Vector2 maxPosition;
    private float halfHeight;
    private float halfWidth;
    Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        halfHeight = camera.orthographicSize;
        halfWidth = camera.aspect * halfHeight;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        halfHeight = camera.orthographicSize;
        halfWidth = camera.aspect * halfHeight;

        Vector3 targetPos = new Vector3(target.position.x, target.position.y, transform.position.z);

        targetPos.x = Mathf.Clamp(targetPos.x, minPosition.x + halfWidth, maxPosition.x-halfWidth);
        targetPos.y = Mathf.Clamp(targetPos.y, minPosition.y + halfHeight, maxPosition.y - halfHeight);

        if (transform.position != targetPos)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, smoothing);
        }

    }
}
