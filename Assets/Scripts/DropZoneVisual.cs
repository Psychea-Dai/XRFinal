using UnityEngine;

public class DropZoneVisual : MonoBehaviour
{
    public float radius = 0.4f;
    public Color idleColor = new Color(0.5f, 0.5f, 0.8f, 1f);
    public Color activeColor = new Color(0.4f, 0.8f, 1f, 1f);

    private Transform[] ringSegments;
    private Transform[] spokes;
    private int segmentCount = 48;
    private int placedCount = 0;

    void Start()
    {
        ringSegments = new Transform[segmentCount];

        for (int i = 0; i < segmentCount; i++)
        {
            float angle = (i / (float)segmentCount) * Mathf.PI * 2f;

            GameObject seg = GameObject.CreatePrimitive(PrimitiveType.Cube);
            seg.name = "RingSeg_" + i;
            seg.transform.SetParent(transform);
            Destroy(seg.GetComponent<BoxCollider>());

            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;
            seg.transform.localPosition = new Vector3(x, 0f, z);
            seg.transform.localRotation = Quaternion.Euler(0f,
                -angle * Mathf.Rad2Deg + 90f, 0f);

            float segWidth = (2f * Mathf.PI * radius / segmentCount) * 0.8f;
            seg.transform.localScale = new Vector3(segWidth, 0.004f, 0.012f);

            Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            mat.EnableKeyword("_EMISSION");
            mat.SetColor("_EmissionColor", idleColor * 0.8f);
            mat.color = idleColor * 0.3f;
            seg.GetComponent<MeshRenderer>().material = mat;

            ringSegments[i] = seg.transform;
        }

        // 5个spoke
        spokes = new Transform[5];
        for (int i = 0; i < 5; i++)
        {
            float angle = (i / 5f) * Mathf.PI * 2f;

            GameObject spoke = GameObject.CreatePrimitive(PrimitiveType.Cube);
            spoke.name = "Spoke_" + i;
            spoke.transform.SetParent(transform);
            Destroy(spoke.GetComponent<BoxCollider>());

            float x = Mathf.Cos(angle) * radius * 0.6f;
            float z = Mathf.Sin(angle) * radius * 0.6f;
            spoke.transform.localPosition = new Vector3(x, 0f, z);
            spoke.transform.localRotation = Quaternion.Euler(0f,
                -angle * Mathf.Rad2Deg + 90f, 0f);
            spoke.transform.localScale = new Vector3(0.004f, 0.004f, radius * 0.4f);

            Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            mat.EnableKeyword("_EMISSION");
            mat.SetColor("_EmissionColor", idleColor * 0.4f);
            mat.color = idleColor * 0.2f;
            spoke.GetComponent<MeshRenderer>().material = mat;

            spokes[i] = spoke.transform;
        }
    }

    public void OnBlockPlaced(int count)
    {
        placedCount = count;
        float t = count / 5f;
        Color current = Color.Lerp(idleColor, activeColor, t);
        float intensity = Mathf.Lerp(0.8f, 2.5f, t);

        foreach (var seg in ringSegments)
        {
            seg.GetComponent<MeshRenderer>().material
                .SetColor("_EmissionColor", current * intensity);
        }

        for (int i = 0; i < 5; i++)
        {
            Color sc = i < count ? activeColor * 2f : idleColor * 0.4f;
            spokes[i].GetComponent<MeshRenderer>().material
                .SetColor("_EmissionColor", sc);
        }
    }

    void Update()
    {
        transform.Rotate(0f, 12f * Time.deltaTime, 0f);
    }
}