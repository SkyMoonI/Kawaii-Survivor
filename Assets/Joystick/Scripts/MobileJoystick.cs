using UnityEngine;

namespace Joystick
{
    public class MobileJoystick : MonoBehaviour
    {
        [Header("Elements")]
        [SerializeField] private RectTransform m_joystickOutline;  // Reference to the outer circle of the joystick.  Assigned in the Unity Editor.
        [SerializeField] private RectTransform m_joystickKnob;     // Reference to the inner movable part of the joystick. Assigned in the Unity Editor.

        [Header("Settings")]
        private Vector3 m_clickedPosition;       // Stores the initial position where the user touched/clicked to activate the joystick.
        private bool m_canControl;             // Flag to determine if the joystick is currently active and responding to input.
        [SerializeField] private float m_moveFactor = 0.1f; // A multiplier to control the sensitivity of the joystick's movement.
        private Vector3 m_move;                  // Stores the calculated movement vector based on the joystick's position.
        private float canvasScale; // Stores the scale of the canvas to ensure the joystick behaves correctly across different screen sizes.

        void Start()
        {
            canvasScale = GetComponentInParent<Canvas>().GetComponent<RectTransform>().localScale.x; //gets the scale of the canvas to adjust the joystick's behavior.
            HideJoystick(); // Initially hides the joystick when the scene starts.
        }

        private void OnDisable()
        {
            HideJoystick(); // Hides the joystick when the GameObject is disabled (e.g., when a UI panel is closed).
        }

        void Update()
        {
            if (m_canControl) // Checks if the joystick is active.
            {
                ControlJoystick(); // If active, calls the function to handle joystick movement.
            }
        }

        /// <summary>
        /// Callback function for when player clicks on joystick zone.
        /// Shows joystick
        /// Sets clicked position for joystick to current mouse position
        /// </summary>
        public void ClickedOnJoystickZoneCallback()
        {
            m_clickedPosition = Input.mousePosition;       // Stores the position of the initial touch/click.
            m_joystickOutline.position = m_clickedPosition; // Positions the joystick outline (the base) at the clicked position.

            ShowJoystick(); // Calls the function to display the joystick.
        }

        private void ShowJoystick()
        {
            m_joystickOutline.gameObject.SetActive(true); // Makes the joystick outline visible.
            m_canControl = true;                       // Sets the flag to allow joystick control.
        }

        private void HideJoystick()
        {
            m_joystickOutline.gameObject.SetActive(false); // Hides the joystick outline.
            m_canControl = false;                      // Disables joystick control.
            m_move = Vector3.zero;                    // Resets the movement vector.  Important to stop movement when the joystick is hidden.
        }

        /// <summary>
        /// Handles the movement and positioning of the joystick knob based on input.
        /// Calculates the direction and magnitude of the movement from the initial touch position
        /// to the current touch position, taking into account the canvas scale. Updates the joystick
        /// knob's position accordingly while ensuring it stays within the bounds of the joystick outline.
        /// </summary>
        private void ControlJoystick()
        {
            Vector3 currentPosition = Input.mousePosition; // Gets the current mouse/touch position.

            // Calculate the direction of the swipe with vector math
            // We subtract the current mouse position from the clicked position.
            // Because we need a vector that points from the clicked position to the current mouse position
            // It is like an arrow that from center of the joystick to the current mouse position
            Vector3 direction = currentPosition - m_clickedPosition; // Calculates the vector from the start point to the current point.

            float moveMagnitude = direction.magnitude * m_moveFactor * canvasScale; // Calculates how far the joystick has been moved, taking into account the moveFactor and canvas scale.

            float absoluteWidth = m_joystickOutline.rect.width / 2; //gets the radius of the joystick outline
            float realWidth = absoluteWidth * canvasScale; //calculates the radius of the joystick outline, taking into account the canvas scale.

            moveMagnitude = Mathf.Min(moveMagnitude, realWidth); // Clamps the magnitude to the joystick's bounds.  Prevents the knob from going too far.

            m_move = direction.normalized * moveMagnitude; // Calculates the final movement vector:  direction, and magnitude.

            Vector3 targetPosition = m_clickedPosition + m_move;  //calculates the target position

            m_joystickKnob.position = targetPosition; // Moves the joystick knob to the new position

            // Checks if the mouse button has been released. If so, hide the joystick.
            if (Input.GetMouseButtonUp(0))
            {
                HideJoystick();
            }
        }

        /// <summary>
        /// Returns the movement vector of the joystick, adjusted for canvas scale.
        /// </summary>
        /// <returns>The normalized movement vector.</returns>
        public Vector3 GetMoveVector()
        {
            if (!m_canControl) return Vector3.zero; // If the joystick is not active, return zero vector.

            return m_move / canvasScale; // Returns the movement vector, adjusted for canvas scale.
        }
    }
}
