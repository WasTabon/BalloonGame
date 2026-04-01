using UnityEngine;

public class GameCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float offsetY = 1.5f;
    [SerializeField] private float smoothSpeed = 6f;

    private void LateUpdate()
    {
        Debug.Assert(target != null, "GameCamera target is null!");

        float targetY = target.position.y + offsetY;
        float newY = Mathf.Lerp(transform.position.y, targetY, smoothSpeed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
