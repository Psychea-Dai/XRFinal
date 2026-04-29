using UnityEngine;
using UnityEngine.InputSystem;

public class OrganismTester : MonoBehaviour
{
    private OrganismController organismController;

    void Start()
    {
        organismController = GetComponent<OrganismController>();
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            Debug.Log("Test: Activate Block 1 (Rhythm)");
            organismController.UpdateActiveCount(1);
        }
        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            Debug.Log("Test: Activate Block 2 (Bass)");
            organismController.UpdateActiveCount(2);
        }
        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            Debug.Log("Test: Activate Block 3 (Harmony)");
            organismController.UpdateActiveCount(3);
        }
        if (Keyboard.current.digit4Key.wasPressedThisFrame)
        {
            Debug.Log("Test: Activate Block 4 (Melody)");
            organismController.UpdateActiveCount(4);
        }
        if (Keyboard.current.digit5Key.wasPressedThisFrame)
        {
            Debug.Log("Test: Activate Block 5 (Sample)");
            organismController.UpdateActiveCount(5);
        }
        if (Keyboard.current.digit0Key.wasPressedThisFrame)
        {
            Debug.Log("Test: Reset to Idle");
            organismController.UpdateActiveCount(0);
        }
    }
}