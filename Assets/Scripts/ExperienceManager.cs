using UnityEngine;

public class ExperienceManager : MonoBehaviour
{
    [Header("References")]
    public OrganismController organism;
    public Light ambientLight;
    public ParticleSystem particles;
    public GameObject spectrumAnalyzer;

    [Header("Settings")]
    public Color[] ambientColors = new Color[5];

    [Header("Debug")]
    [Range(0, 5)]
    public int debugCount = 0;

    private int placedCount = 0;

    void Start()
    {
        if (organism == null)
        {
            organism = FindAnyObjectByType<OrganismController>();
        }

        Debug.Log("Organism found: " + organism);

        if (particles != null)
        {
            particles.Stop();
        }

        if (ambientLight != null)
        {
            ambientLight.intensity = 0.1f;
            ambientLight.color = Color.black;
        }
    }

    public void OnBlockPlaced(int count)
    {
        Debug.Log("OnBlockPlaced called: " + count);

        placedCount = Mathf.Clamp(count, 0, 5);
        UpdateEnvironment();
    }

    void UpdateEnvironment()
    {
        float progress = placedCount / 5f;

        if (ambientLight != null)
        {
            ambientLight.intensity = Mathf.Lerp(0.1f, 2f, progress);
            ambientLight.color = Color.Lerp(Color.black, Color.white, progress);
        }

        if (particles != null)
        {
            if (placedCount >= 3)
                particles.Play();
            else
                particles.Stop();
        }

        if (organism != null)
        {
            Debug.Log("Sending count to OrganismController: " + placedCount);
            organism.UpdateActiveCount(placedCount);
        }
        else
        {
            Debug.LogWarning("OrganismController is missing.");
        }

        if (placedCount >= 5)
        {
            FullBloom();
        }
    }

    void FullBloom()
    {
        if (particles != null)
        {
            var main = particles.main;
            main.simulationSpeed = 2f;
        }

        if (ambientLight != null)
        {
            ambientLight.color = new Color(1f, 0.9f, 0.7f);
        }
    }

    // 🔥 Easy testing: change debugCount in Play Mode
    void OnValidate()
    {
        if (!Application.isPlaying) return;

        OnBlockPlaced(debugCount);
    }
}
