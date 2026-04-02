using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private Camera mainCam;
    private float destroyOffset = 5f;

    private void Start()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        if (mainCam == null) return;

        float camBottom = mainCam.transform.position.y - mainCam.orthographicSize;
        if (transform.position.y < camBottom - destroyOffset)
            Destroy(gameObject);
    }
}
