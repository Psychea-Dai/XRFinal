using UnityEngine;
using Unity.PolySpatial.InputDevices;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem.LowLevel;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class VisionOSGrabHandler : MonoBehaviour
{
    private BlockInteractable blockInteractable;
    private bool isGrabbed = false;
    private Vector3 grabOffset;

    private float holdTimer = 0f;
    private bool holdTriggered = false;
    private float holdThreshold = 0.8f;

    void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    void Start()
    {
        blockInteractable = GetComponent<BlockInteractable>();
    }

    void Update()
    {
        var activeTouches = Touch.activeTouches;
        if (activeTouches.Count == 0)
        {
            // 没有touch时重置hold状态
            if (isGrabbed)
            {
                isGrabbed = false;
                holdTimer = 0f;
                holdTriggered = false;
                blockInteractable?.OnRelease();
            }
            return;
        }

        var touch = activeTouches[0];
        var touchData = EnhancedSpatialPointerSupport.GetPointerState(touch);

        if (touchData.Kind == SpatialPointerKind.IndirectPinch ||
            touchData.Kind == SpatialPointerKind.DirectPinch)
        {
            // 捏起开始
            if (touch.phase == TouchPhase.Began &&
                touchData.targetObject == gameObject)
            {
                isGrabbed = true;
                holdTimer = 0f;
                holdTriggered = false;
                grabOffset = transform.position - touchData.interactionPosition;
                blockInteractable?.OnGrab();
            }

            if (isGrabbed)
            {
                // 长按计时
                holdTimer += Time.deltaTime;
                if (!holdTriggered && holdTimer > holdThreshold)
                {
                    holdTriggered = true;
                    blockInteractable?.OnTogglePause();
                }

                // 移动
                if (touch.phase == TouchPhase.Moved)
                {
                    transform.position = touchData.interactionPosition + grabOffset;
                }

                // 放开
                if (touch.phase == TouchPhase.Ended ||
                    touch.phase == TouchPhase.Canceled)
                {
                    isGrabbed = false;
                    holdTimer = 0f;
                    holdTriggered = false;
                    blockInteractable?.OnRelease();
                }
            }
        }
    }
}