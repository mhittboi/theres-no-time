using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public Transform background1;
    public Transform background2;
    public float smoothing1 = 1f;
    public float smoothing2 = 0.5f;

    private Transform cam;
    private Vector3 previousCamPos;

    void Awake()
    {
        cam = Camera.main.transform;
    }

    void Start()
    {
        previousCamPos = cam.position;
    }

    void Update()
    {
        float parallax1 = (previousCamPos.x - cam.position.x) * smoothing1;
        float parallax2 = (previousCamPos.x - cam.position.x) * smoothing2;

        Vector3 background1TargetPos = new Vector3(background1.position.x + parallax1, background1.position.y, background1.position.z);
        Vector3 background2TargetPos = new Vector3(background2.position.x + parallax2, background2.position.y, background2.position.z);

        background1.position = Vector3.Lerp(background1.position, background1TargetPos, smoothing1 * Time.deltaTime);
        background2.position = Vector3.Lerp(background2.position, background2TargetPos, smoothing2 * Time.deltaTime);

        previousCamPos = cam.position;
    }
}
