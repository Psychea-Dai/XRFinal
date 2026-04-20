using UnityEngine;

public class RadialAnalyzer : MonoBehaviour
{
    [Header("Block Audio Sources")]
    public AudioSource[] blockSources;

    [Header("Visual Settings")]
    public int sampleCount = 128;
    public float radius = 0.5f;
    public float minHeight = 0.02f;
    public float maxHeight = 0.4f;
    public float smoothSpeed = 8f;

    [Header("Color")]
    public Color innerColor = new Color(0.4f, 0.8f, 1f, 1f);
    public Color outerColor = new Color(1f, 0.3f, 0.8f, 1f);

    private Transform[] bars;
    private MeshRenderer[] barRenderers;
    private float[] spectrum;
    private float[] combinedSpectrum;
    private float[] smoothed;

    void Start()
    {
        spectrum = new float[256];
        combinedSpectrum = new float[256];
        smoothed = new float[sampleCount];
        bars = new Transform[sampleCount];
        barRenderers = new MeshRenderer[sampleCount];

        for (int i = 0; i < sampleCount; i++)
        {
            GameObject bar = GameObject.CreatePrimitive(PrimitiveType.Cube);
            bar.name = "Bar_" + i;
            bar.transform.SetParent(transform);

            Destroy(bar.GetComponent<BoxCollider>());

            Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            mat.EnableKeyword("_EMISSION");
            barRenderers[i] = bar.GetComponent<MeshRenderer>();
            barRenderers[i].material = mat;

            bars[i] = bar.transform;
        }
    }

    void Update()
    {
        System.Array.Clear(combinedSpectrum, 0, 256);

        foreach (var src in blockSources)
        {
            if (src != null && src.isPlaying)
            {
                src.GetSpectrumData(spectrum, 0, FFTWindow.Blackman);
                for (int i = 0; i < 256; i++)
                    combinedSpectrum[i] += spectrum[i];
            }
        }

        for (int i = 0; i < sampleCount; i++)
        {
            float angle = (i / (float)sampleCount) * Mathf.PI * 2f;
            float target = Mathf.Log(Mathf.Max(combinedSpectrum[i], 1e-5f) + 1f) * maxHeight;
            smoothed[i] = Mathf.Lerp(smoothed[i], target, Time.deltaTime * smoothSpeed);
            float height = Mathf.Max(smoothed[i], minHeight);

            // 位置：放在圆环上，从内向外延伸
            float midRadius = radius + height * 0.5f;
            float x = Mathf.Cos(angle) * midRadius;
            float z = Mathf.Sin(angle) * midRadius;

            bars[i].localPosition = new Vector3(x, 0f, z);

            // 旋转朝向圆心
            bars[i].localRotation = Quaternion.Euler(0f,
                -angle * Mathf.Rad2Deg + 90f, 0f);

            // 尺寸：宽度固定，高度随频谱变化
            float barWidth = (2f * Mathf.PI * radius / sampleCount) * 0.85f;
            bars[i].localScale = new Vector3(barWidth, 0.008f, height);

            // 颜色
            float t = Mathf.InverseLerp(minHeight, maxHeight * 0.5f, height);
            Color c = Color.Lerp(innerColor, outerColor, t);
            barRenderers[i].material.SetColor("_EmissionColor", c * 1.5f);
            barRenderers[i].material.color = c * 0.3f;
        }
    }
}