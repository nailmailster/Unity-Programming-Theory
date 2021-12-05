using UnityEngine;

public class Sight : MonoBehaviour
{
    public float distance;
    public float angle;
    public LayerMask objectsLayers;
    public LayerMask obstaclesLayers;

    public Collider detectedObject;

    void Update()
    {
        DetectFoodObject();
    }

    void DetectFoodObject()
    {
        float minDistance = 9999999;

        Collider[] colliders = Physics.OverlapSphere(transform.position, distance, objectsLayers);

        detectedObject = null;
        for (int i = 0; i < colliders.Length; i++)
        {
            Collider collider = colliders[i];

            Vector3 directionToCollider = Vector3.Normalize(collider.bounds.center - transform.position);

            float angleToCollider = Vector3.Angle(transform.forward, directionToCollider);

            if (angleToCollider < angle)
            {
                if (!Physics.Linecast(transform.position, collider.bounds.center, out RaycastHit hit, obstaclesLayers))
                {
                    float distance = Vector3.Distance(transform.position, collider.bounds.center);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        detectedObject = collider;
                    }
                }
                else
                {
                    Debug.DrawLine(transform.position, hit.point, Color.red);
                }
            }
        }

        if (detectedObject != null)
            Debug.DrawLine(transform.position, detectedObject.transform.position, Color.green);
    }

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distance);

        Vector3 rightDirection = Quaternion.Euler(0, angle, 0) * transform.forward;
        Gizmos.DrawRay(transform.position, rightDirection * distance);

        Vector3 leftDirection = Quaternion.Euler(0, -angle, 0) * transform.forward;
        Gizmos.DrawRay(transform.position, leftDirection * distance);
    }
}
