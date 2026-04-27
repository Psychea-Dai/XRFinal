using UnityEngine;

public class BlockInteractable : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip[] variants = new AudioClip[3];
    public AudioSource audioSource;

    [Header("State")]
    public int currentVariant = 0;
    public bool isPlaced = false;
    public bool isHeld = false;
    public bool isPaused = false;

    [Header("Visual")]
    public Renderer domeRenderer;

    // 👇 粒子
    [Header("Hit Feedback")]
    [SerializeField] private ParticleSystem hitParticles;

    // 👇 新增：波纹 Prefab
    [SerializeField] private GameObject ripplePrefab;

    // 每个变体的颜色
    private Color[] variantColors = new Color[]
    {
        new Color(2f, 0.6f, 2f),
        new Color(0.6f, 2f, 2f),
        new Color(2f, 1.5f, 0.3f)
    };

    void Start()
    {
        if (audioSource != null)
        {
            audioSource.loop = true;
            audioSource.volume = 0f;
            if (variants[0] != null)
                audioSource.clip = variants[0];
            audioSource.Play();
        }

        if (domeRenderer != null)
            domeRenderer.material.SetColor("_EmissionColor", variantColors[0] * 0.5f);
    }

    public void OnGrab()
    {
        isHeld = true;
        currentVariant = (currentVariant + 1) % variants.Length;

        if (audioSource != null && variants[currentVariant] != null)
        {
            audioSource.clip = variants[currentVariant];
            audioSource.volume = 1f;
            audioSource.Play();
        }

        if (domeRenderer != null)
            domeRenderer.material.SetColor("_EmissionColor", variantColors[currentVariant]);

        // 👇 粒子 + 波纹
        PlayHitParticles();
        SpawnRipple();
    }

    public void OnRelease()
    {
        isHeld = false;
    }

    public void OnTap()
    {
        currentVariant = (currentVariant + 1) % variants.Length;

        if (audioSource != null && variants[currentVariant] != null)
        {
            audioSource.clip = variants[currentVariant];
            audioSource.Play();
        }

        if (domeRenderer != null)
            domeRenderer.material.SetColor("_EmissionColor", variantColors[currentVariant]);

        // 👇 粒子 + 波纹
        PlayHitParticles();
        SpawnRipple();
    }

    public void OnPlaced()
    {
        isPlaced = true;

        if (audioSource != null)
            audioSource.volume = 1f;

        if (domeRenderer != null)
            domeRenderer.material.SetColor("_EmissionColor",
                variantColors[currentVariant] * 2f);
    }

    public void OnRemoved()
    {
        isPlaced = false;

        if (audioSource != null)
            audioSource.volume = 0f;

        if (domeRenderer != null)
            domeRenderer.material.SetColor("_EmissionColor",
                variantColors[currentVariant] * 0.5f);
    }

    public void OnTogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            audioSource?.Pause();
            if (domeRenderer != null)
                domeRenderer.material.SetColor("_EmissionColor",
                    variantColors[currentVariant] * 0.2f);
        }
        else
        {
            audioSource?.UnPause();
            if (domeRenderer != null)
                domeRenderer.material.SetColor("_EmissionColor",
                    variantColors[currentVariant] * 2f);
        }
    }

    // ===== 粒子 =====
    void PlayHitParticles()
    {
        if (hitParticles != null)
        {
            hitParticles.Play();
        }
    }

    // ===== 🌊 新增：波纹 =====
    void SpawnRipple()
    {
        if (ripplePrefab == null) return;

        Vector3 pos = transform.position;

        // 👉 关键：避免被地面遮住
        pos.y = 0.01f;

        Instantiate(ripplePrefab, pos, Quaternion.Euler(90, 0, 0));
    }
}