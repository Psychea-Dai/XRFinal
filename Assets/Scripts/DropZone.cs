using UnityEngine;
using UnityEngine.Events;

public class DropZone : MonoBehaviour
{
    public float snapDistance = 0.3f;
    public UnityEvent<int> onBlockCountChanged;

    private int placedCount = 0;
    private BlockInteractable[] allBlocks;

    void Start()
    {
        allBlocks = FindObjectsByType<BlockInteractable>(FindObjectsSortMode.None);
    }

    void Update()
    {
        foreach (var block in allBlocks)
        {
            float dist = Vector3.Distance(block.transform.position, transform.position);

            // 放入zone
            if (!block.isPlaced && !block.isHeld && dist < snapDistance)
            {
                placedCount++;
                block.OnPlaced();
                Vector3 offset = Random.insideUnitSphere * 0.15f;
                offset.y = 0;
                block.transform.position = transform.position + offset;
                onBlockCountChanged?.Invoke(placedCount);
            }

            // 离开zone — 被拿起且距离够远
            if (block.isPlaced && block.isHeld && dist > snapDistance * 2f)
            {
                placedCount = Mathf.Max(0, placedCount - 1);
                block.OnRemoved();
                onBlockCountChanged?.Invoke(placedCount);
            }
        }
    }
}