using UnityEngine;

public class FloatTexture : MonoBehaviour
{
    [SerializeField] Material material;

    float xOffset = 0;
    float yOffset = 0;

    [SerializeField] float deltaX = 1f;
    [SerializeField] float deltaY = 0;

    void Update()
    {
        material.mainTextureOffset = new Vector2(xOffset, yOffset);
        if (CompareTag("ScaleIn"))
        {
            xOffset += deltaX * Time.deltaTime;
            yOffset += deltaY * Time.deltaTime;
        }
        else if (CompareTag("ScaleInX"))
        {
            xOffset += -deltaX * Time.deltaTime;
            yOffset += -deltaY * Time.deltaTime;
        }

        #region test
        // Mesh mesh = GetComponent<MeshFilter>().mesh;
        // Vector3[] vertices = mesh.vertices;
        // Vector3[] normals = mesh.normals;

        // // for (var i = 0; i < vertices.Length; i++)
        // //     vertices[i] += normals[i] * Mathf.Sin(Time.time);
        // vertices[7].y += 0.01f;
        
        // mesh.vertices = vertices;
        #endregion //test
    }
}
