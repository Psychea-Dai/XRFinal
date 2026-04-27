using UnityEngine;


public class ExperienceManager : MonoBehaviour
{
    [Header("References")]
    public OrganismController organism;
    public Light ambientLight;
    public ParticleSystem particles;
    public GameObject spectrumAnalyzer; // 你的SpectrumLine

    [Header("Settings")]
    public Color[] ambientColors = new Color[5];
    private int placedCount = 0;

    void Start()
    {
        if (organism == null)
        organism = FindObjectOfType<OrganismController>();
    
    Debug.Log("Organism found: " + organism);

        // 初始状态
        if (particles != null) particles.Stop();
        if (ambientLight != null)
        {
            ambientLight.intensity = 0.1f;
            ambientLight.color = Color.black;
        }
    }

    // 由DropZone的onBlockCountChanged调用
    public void OnBlockPlaced(int count)
    {
        Debug.Log("OnBlockPlaced called: " + count);
        placedCount = count;
        UpdateEnvironment();
    }

    void UpdateEnvironment()
    {
        float progress = placedCount / 5f;

        // 渐进式环境光
        if (ambientLight != null)
        {
            ambientLight.intensity = Mathf.Lerp(0.1f, 2f, progress);
            ambientLight.color = Color.Lerp(Color.black, Color.white, progress);
        }

        // 3个以上触发粒子
        if (placedCount >= 3 && particles != null)
            particles.Play();
          // ← 加这一行，把placedCount传给生命体
        if (organism != null)
        organism.UpdateActiveCount(placedCount);
        
        // 5个全放置 — 完全绽放
        if (placedCount >= 5)
            FullBloom();
    }

    void FullBloom()
    {
        if (particles != null)
        {
            var main = particles.main;
            main.simulationSpeed = 2f;
        }

        if (ambientLight != null)
            ambientLight.color = new Color(1f, 0.9f, 0.7f); // 暖黄光
    }
    



}
