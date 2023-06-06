using UnityEngine;
using Fusion;
using UnityEngine.InputSystem;

// From Fusion tutorial: https://doc.photonengine.com/fusion/current/tutorials/shared-mode-basics/3-movement-and-camera
public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] float speed = 5f;

    [SerializeField] InputAction moveAction;
    private void OnEnable() { moveAction.Enable(); }
    private void OnDisable() { moveAction.Disable(); }

    public float rotationSpeed = 5f;
    void OnValidate()
    {
        // Provide default bindings for the input actions.
        // Based on answer by DMGregory: https://gamedev.stackexchange.com/a/205345/18261
        if (moveAction == null)
            moveAction = new InputAction(type: InputActionType.Button);
        if (moveAction.bindings.Count == 0)
            moveAction.AddCompositeBinding("2DVector")
                .With("Up", "<Keyboard>/upArrow")
                .With("Down", "<Keyboard>/downArrow")
                .With("Left", "<Keyboard>/leftArrow")
                .With("Right", "<Keyboard>/rightArrow");
    }


    private CharacterController _controller;
    public override void Spawned()
    {
        _controller = GetComponent<CharacterController>();
    }

    private Vector3 velocity = new Vector3(0, 0, 0);
    public override void FixedUpdateNetwork()
    {
        // Only move own player and not every other player. Each player controls only its own player object.
        if (!HasStateAuthority)
        {
            return;
            // NetworkTransform only synchronizes changes from the StateAuthority.             
            // If someone without StateAuthority tries to change, the change will be local, and not transmitted to other players.
        }

        // RotateTowardsMouse();

        Vector3 movement = moveAction.ReadValue<Vector2>();
        velocity.x = movement.x * speed;
        velocity.z = movement.y * speed;
        _controller.Move(velocity * Runner.DeltaTime);

        RotateTowardsMouse();
        

        



    }
    private void RotateTowardsMouse()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector3 mousePosition3D = new Vector3(mousePosition.x, mousePosition.y, 0f);
        Ray mouseRay = Camera.main.ScreenPointToRay(mousePosition);
        Plane groundPlane = new Plane(Vector3.up, transform.position); // Assuming the ground is at Y=0

        float rayDistance;
        if (groundPlane.Raycast(mouseRay, out rayDistance))
        {
            Vector3 worldMousePosition = mouseRay.GetPoint(rayDistance);
            Vector3 lookDirection = worldMousePosition - transform.position;
            lookDirection.y = 0f; // Ignore any vertical difference

            if (lookDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }


    }

}

