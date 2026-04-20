using UnityEngine;

public class Analyzer : MonoBehaviour
{
    AudioSource aud;
    public LineRenderer line;

    public float xSpacing = 0.05f;
    public float yScale = 2f;
    public float zDepth = 0f;

    void Start()
    {
        aud = GetComponent<AudioSource>();
        line.positionCount = 255;
    }

    void Update()
    {
        float[] spectrum = new float[256];
        aud.GetSpectrumData(spectrum, 0, FFTWindow.Blackman);

        Vector3[] points = new Vector3[255];

        for (int i = 0; i < 255; i++)
        {
            float x = i * xSpacing;
            float y = Mathf.Log(Mathf.Max(spectrum[i], 1e-5f)) * yScale;
            points[i] = new Vector3(x, y, zDepth);
        }

        line.SetPositions(points);
    }
}