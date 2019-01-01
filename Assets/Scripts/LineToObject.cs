using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineToObject : MonoBehaviour {

    public Transform Target;
    private LineRenderer line;
    private Camera camera;

    void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    void Update()
    {
        var rect = transform as RectTransform;
        var camera = Camera.main;

        var worldPos = ScreenToWorld(camera, rect.position);

        line.SetPositions(new Vector3[] { worldPos, Target.position });
    }

    public static Vector3 ScreenToWorld(Camera camera, Vector3 pos, float? z = null)
    {
        if (camera.orthographic)
        {
            var p = camera.ScreenToWorldPoint(new Vector3(pos.x, pos.y, camera.nearClipPlane));
            p.z = z != null ? (float)z : 0f;
            return p;
        }
        else
        {
            var ray = camera.ScreenPointToRay(new Vector3(pos.x, pos.y, camera.nearClipPlane));
            var d = z != null ? (float)z : 20f;
            var p = camera.transform.position + ray.direction * d;
            return p;
        }
    }
}
