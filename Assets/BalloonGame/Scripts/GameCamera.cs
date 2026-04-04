using UnityEngine;

public class GameCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float offsetY = 1.5f;
    [SerializeField] private float smoothSpeed = 6f;
    [SerializeField] private float baseOrthoSize = 10f;
    [SerializeField] private float maxOrthoSize = 12f;
    [SerializeField] private float zoomOutScore = 100f;

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        Debug.Assert(target != null, "GameCamera target is null!");

        float targetY = target.position.y + offsetY;
        float newY = Mathf.Lerp(transform.position.y, targetY, smoothSpeed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        if (cam != null && GameplayManager.Instance != null)
        {
            float scoreT = Mathf.Clamp01(GameplayManager.Instance.CurrentScore / zoomOutScore);
            float targetSize = Mathf.Lerp(baseOrthoSize, maxOrthoSize, scoreT * scoreT);
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, 2f * Time.deltaTime);
        }
    }
}
