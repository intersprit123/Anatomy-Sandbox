using UnityEngine;

namespace AnatomySandbox.Interaction
{
    /// <summary>
    /// Drag to rotate the generated model and pinch/scroll to zoom.
    /// Attach to the GeneratedAnatomy root or another pivot centered on the body.
    /// </summary>
    public sealed class ModelOrbitController : MonoBehaviour
    {
        [SerializeField] private float dragSensitivity = 0.35f;
        [SerializeField] private float zoomSensitivity = 0.008f;
        [SerializeField] private float minCameraDistance = 2.2f;
        [SerializeField] private float maxCameraDistance = 6.5f;

        private Camera targetCamera;
        private Vector2 previousPointer;
        private bool dragging;
        private float initialDistance;

        private void Start()
        {
            targetCamera = Camera.main;
            if (targetCamera != null)
                initialDistance = Vector3.Distance(targetCamera.transform.position, transform.position);
        }

        private void Update()
        {
            if (targetCamera == null) targetCamera = Camera.main;
            if (targetCamera == null) return;

            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    previousPointer = touch.position;
                    dragging = true;
                }
                else if (touch.phase == TouchPhase.Moved && dragging)
                {
                    Vector2 delta = touch.position - previousPointer;
                    Rotate(delta);
                    previousPointer = touch.position;
                }
                else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    dragging = false;
                }
            }
            else if (Input.touchCount >= 2)
            {
                dragging = false;
                Touch a = Input.GetTouch(0);
                Touch b = Input.GetTouch(1);
                Vector2 aPrev = a.position - a.deltaPosition;
                Vector2 bPrev = b.position - b.deltaPosition;
                float previousGap = Vector2.Distance(aPrev, bPrev);
                float currentGap = Vector2.Distance(a.position, b.position);
                Zoom((previousGap - currentGap) * zoomSensitivity);
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    previousPointer = Input.mousePosition;
                    dragging = true;
                }
                else if (Input.GetMouseButton(0) && dragging)
                {
                    Vector2 current = Input.mousePosition;
                    Rotate(current - previousPointer);
                    previousPointer = current;
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    dragging = false;
                }

                float scroll = Input.mouseScrollDelta.y;
                if (Mathf.Abs(scroll) > 0.01f)
                    Zoom(-scroll * 0.15f);
            }
        }

        private void Rotate(Vector2 delta)
        {
            transform.Rotate(Vector3.up, -delta.x * dragSensitivity, Space.World);
            transform.Rotate(Vector3.right, delta.y * dragSensitivity, Space.Self);
        }

        private void Zoom(float delta)
        {
            Vector3 fromPivot = targetCamera.transform.position - transform.position;
            float distance = Mathf.Clamp(fromPivot.magnitude + delta, minCameraDistance, maxCameraDistance);
            targetCamera.transform.position = transform.position + fromPivot.normalized * distance;
            targetCamera.transform.LookAt(transform.position + Vector3.up * 0.35f);
        }
    }
}
