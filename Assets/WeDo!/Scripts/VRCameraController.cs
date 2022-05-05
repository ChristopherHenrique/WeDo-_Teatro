using UnityEngine;
using UnityEditor;

public class VRCameraController : MonoBehaviour
{
    [Tooltip("Enable/disable rotation control.")]
    public bool rotationEnabled = true;

    [Tooltip("Mouse sensitivity")]
    public float mouseSensitivity = 1f;


    private float minimumX = -360f;
    private float maximumX = 360f;

    private float minimumY = -90f;
    private float maximumY = 90f;

    private float rotationX = 0f;
    private float rotationY = 0f;

    float MaxZoomValue = 2f;
    float MinZoomValue = 0f;

    float ZoomSpeed = 0.1f;

    Quaternion originalRotation;

    Transform cameraZoom;

    void Start()
    {
        originalRotation = transform.localRotation;
        cameraZoom = transform.GetChild(0);

        LimitAngle(15f, 30f);
#if !UNITY_EDITOR
        if (GameManager.CheckIsMobile())
        {
            Debug.Log("Is mobile");
            mouseSensitivity = 0.5f;
        }
#endif

    }

    void Update()
    {
        if (!rotationEnabled)
            return;
#if !UNITY_EDITOR
        if (GameManager.CheckIsMobile())
        {
            Debug.Log("Is mobile");
            if (Input.touchCount == 1)
                ControlRotationMobile();
        }
        else
        {
            if (Input.GetMouseButton(0))
                ControlRotationWeb();
        }
#elif UNITY_EDITOR
        if (Input.GetMouseButton(0))
            ControlRotationWeb();
#endif



        if (Input.mouseScrollDelta.y != 0)
        {
            ControlZoom();
        }
    }

    void ControlZoom()
    {
        float a = Input.mouseScrollDelta.y * ZoomSpeed;
        float positionZ = cameraZoom.localPosition.z + a;

        positionZ = Mathf.Clamp(positionZ, MinZoomValue, MaxZoomValue);


        Vector3 zoom = new Vector3(cameraZoom.localPosition.x, cameraZoom.localPosition.y, positionZ);
        cameraZoom.localPosition = zoom;
    }

    void ControlRotationWeb()
    {
        rotationX -= Input.GetAxis("Mouse X") * mouseSensitivity;
        rotationY -= Input.GetAxis("Mouse Y") * mouseSensitivity;

        rotationX = ClampAngle(rotationX, minimumX, maximumX);
        rotationY = ClampAngle(rotationY, minimumY, maximumY);

        // Debug.Log("rotation x == " + rotationX);
        // Debug.Log("rotation y == " + rotationY);

        Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
        Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, Vector3.left);

        transform.localRotation = originalRotation * xQuaternion * yQuaternion;
    }

    void ControlRotationMobile()
    {
        // if (Input.touchCount == 1)
        // {
        // Store touch.
        Touch touchZero = Input.GetTouch(0);

        if (touchZero.phase == TouchPhase.Moved)
        {
            // Find the position in the previous frame of the touch.
            Vector2 touchZeroPosition = touchZero.position;
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;

            // Find the difference in the distances between each frame.
            float diffX = touchZeroPrevPos.x - touchZeroPosition.x;
            float diffY = touchZeroPrevPos.y - touchZeroPosition.y;

            rotationX -= diffX * mouseSensitivity;
            rotationY -= diffY * mouseSensitivity;

            rotationX = ClampAngle(rotationX, minimumX, maximumX);
            rotationY = ClampAngle(rotationY, minimumY, maximumY);

            Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
            Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, Vector3.left);

            transform.localRotation = originalRotation * xQuaternion * yQuaternion;
        }
        // }
    }

    void DisableEverything()
    {
        rotationEnabled = false;
    }

    void ResetZoom()
    {
        cameraZoom.localPosition = Vector3.zero;
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360f)
            angle += 360f;
        if (angle > 360f)
            angle -= 360f;
        return Mathf.Clamp(angle, min, max);
    }

    public void resetCamera()
    {
        rotationX = 0;
        rotationY = 0;
        transform.localRotation = originalRotation;
        ResetZoom();
        // Debug.Log("Reset");
    }
    public void SetCameraHeight(float height)
    {
        Vector3 position = new Vector3(0, height, 0);
        transform.position = position;
    }
    public void LimitAngle(float valueX = 360, float valueY = 90)
    {
        valueX = Mathf.Clamp(valueX, 0, 360);
        valueY = Mathf.Clamp(valueY, 0, 90);
        // Debug.Log("value x == " + valueX + ", valueY == " + valueY);
        minimumX = -valueX;
        maximumX = valueX;
        minimumY = -valueY;
        maximumY = valueY;
    }
    public void SaveNewRotation(float newX, float newY)
    {
        rotationX = newX;
        rotationY = newY;
        rotationX = ClampAngle(rotationX, minimumX, maximumX);
        rotationY = ClampAngle(rotationY, minimumY, maximumY);
    }
}
