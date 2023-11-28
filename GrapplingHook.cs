using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    public float grapplingDistance = 10f;
    public LayerMask grapplingMask;
    public float grappleForce = 10f;

    private LineRenderer lineRenderer;
    private Rigidbody playerRigidbody;
    private bool isGrappling = false;
    private Vector3 grapplePoint;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;

        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isGrappling)
        {
            TryGrapple();
        }
        else if (Input.GetMouseButtonUp(0) && isGrappling)
        {
            StopGrapple();
        }

        if (isGrappling)
        {
            ApplyGrappleForce();
            DrawGrappleLine();
        }
    }

    private void TryGrapple()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * grapplingDistance, Color.red, 2f);  // Add this line
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, grapplingDistance, grapplingMask))
        {
            Debug.Log("Hit point: " + hit.point);
            StartGrapple(hit.point);
        }
        else
        {
            Debug.Log("No hit detected.");
        }
    }



    private void StartGrapple(Vector3 targetPoint)
    {
        grapplePoint = targetPoint;
        isGrappling = true;
        lineRenderer.enabled = true;
    }

    private void StopGrapple()
    {
        isGrappling = false;
        lineRenderer.enabled = false;
    }

    private void ApplyGrappleForce()
    {
        Vector3 grappleDirection = (grapplePoint - transform.position).normalized;
        playerRigidbody.AddForce(grappleDirection * grappleForce, ForceMode.Force);
    }

    private void DrawGrappleLine()
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, grapplePoint);
    }
}
