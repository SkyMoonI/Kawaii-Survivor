using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform m_target;

    [Header("Settings")]
    [SerializeField] private Vector2 m_offsetOfCamera;

    void LateUpdate()
    {
        SetCameraPosition(m_target);
    }

    private void SetCameraPosition(Transform target)
    {
        if (target == null)
        {
            Debug.LogWarning("Target is null. Please assign a target object.");
            return;
        }

        Vector3 targetPosition = target.position; // Get the position of the target object


        if (!GameManager.Instance.IsUsingInfiniteMap)
        {
            // restrict the camera movement to a certain range  
            targetPosition.x = Mathf.Clamp(targetPosition.x, -m_offsetOfCamera.x, m_offsetOfCamera.x);
            targetPosition.y = Mathf.Clamp(targetPosition.y, -m_offsetOfCamera.y, m_offsetOfCamera.y);
        }

        targetPosition.z = transform.position.z; // Maintain the same Z position of the camera (-10 in this case).

        transform.position = targetPosition; // Update the position of the camera
    }
}
