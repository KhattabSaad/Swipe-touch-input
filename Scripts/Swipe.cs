using UnityEngine;
using UnityEngine.InputSystem;

public class Swipe : MonoBehaviour
{
    private InputController inputController;

    private bool OnTouchAction = false;

    private bool isSwiping;
    private bool hasSwiped;

    private bool startPositionSet = false;

    private float swipeStartTime;
    private float swipeEndTime;

    private Vector2 swipeStartPosition;
    private Vector2 swipeEndPosition;

    [SerializeField] private float swipeDistanceThreshold = 50f;
    [SerializeField] private float swipeTimeThreshold = 0.5f;


    void Awake()
    {
        inputController = new InputController();
        inputController.Touch.Touchinndicator.started += OnTouch;
        inputController.Touch.Touchinndicator.canceled += OnTouch;
        inputController.Touch.TouchPosition.started += OnTouchPosition;
        inputController.Touch.TouchPosition.canceled += OnTouchPosition;
    }

    private void OnEnable()
    {
        inputController?.Enable();
    }

    private void OnDisable()
    {
        inputController.Disable();
    }

    void Update()
    {

        Debug.Log("start position: " + swipeStartPosition);
        Debug.Log("end position: " + swipeEndPosition);
        Debug.Log("start time position: " + swipeStartTime);
        Debug.Log("end time position: " + swipeEndTime);

        if (OnTouchAction)
        {
            Debug.Log("Jump");
            OnTouchAction = false;
        }
        else if (!OnTouchAction)
        {
            Debug.Log("No Jump");
        }

    }

    void OnTouch(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            swipeStartTime = Time.time;
            isSwiping = true;
            hasSwiped = false;
            startPositionSet = false;
        }
        else if (context.canceled && isSwiping)
        {
            swipeEndTime = Time.time;
            isSwiping = false;
            hasSwiped = true;
        }
    }

    private void OnTouchPosition(InputAction.CallbackContext context)
    {
        Vector2 touchPosition = context.action.ReadValue<Vector2>();

        if (isSwiping && !startPositionSet)
        {
            swipeStartPosition = touchPosition;
            startPositionSet = true;
        }
        else if (!isSwiping && hasSwiped)
        {
            swipeEndPosition = touchPosition;
            hasSwiped = false;

            Vector2 swipeDelta = swipeEndPosition - swipeStartPosition;
            float swipeDistance = swipeDelta.magnitude;
            float swipeTime = swipeEndTime - swipeStartTime;

            if (swipeDistance >= swipeDistanceThreshold && swipeTime <= swipeTimeThreshold)
            {
                OnTouchAction = true;
            }
            else if (OnTouchAction == false)
            {
                swipeStartPosition = Vector2.zero;
                swipeEndPosition = Vector2.zero;
                swipeStartTime = 0f;
                swipeEndTime = 0f;
            }
        }
    }
}
