using UnityEngine;
using UnityEngine.UI;

public class Ground : MonoBehaviour
{
    [SerializeField] Text text;
    [SerializeField] LayerMask groundLayerMask;

    Vector3 worldPosition;
    Ray ray;
    RaycastHit hitData;

    Camera camera3;

    private void Awake()
    {
        // camera3 = Camera.allCameras[2];
        camera3 = Camera.allCameras[1];
    }

    private void OnMouseDown()
    {
        ray = camera3.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hitData, 1000, groundLayerMask))
        {
            worldPosition = hitData.point;
            // text.text = worldPosition.ToString();
        }
        else
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hitData, 1000, groundLayerMask))
            {
                worldPosition = hitData.point;
                // text.text = worldPosition.ToString();
            }
        }

        EventManager.TriggerEvent("GroundOnMouseDown", worldPosition);
    }
}
