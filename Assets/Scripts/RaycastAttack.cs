using Fusion;
using UnityEngine;
using UnityEngine.InputSystem;

// from Fusion tutorial: https://doc.photonengin e.com/fusion/current/tutorials/shared-mode-basics/5-remote-procedure-calls
public class RaycastAttack : NetworkBehaviour {
    [SerializeField] int Damage;

    [SerializeField] InputAction attack;
    [SerializeField] InputAction attackLocation;


    private Transform firePoint;
    public GameObject bulletPrefab;

    public NetworkObject player;
    private CharacterController _controller;

    

    public float bulletForce = 10f;

    private void OnEnable() { attack.Enable(); attackLocation.Enable();  }
    private void OnDisable() { attack.Disable(); attackLocation.Disable(); }
    void OnValidate() {
        // Provide default bindings for the input actions. Based on answer by DMGregory: https://gamedev.stackexchange.com/a/205345/18261
        if (attack == null)
            attack = new InputAction(type: InputActionType.Button);
        if (attack.bindings.Count == 0)
            attack.AddBinding("<Mouse>/leftButton");

        if (attackLocation == null)
            attackLocation = new InputAction(type: InputActionType.Value, expectedControlType: "Vector2");
        if (attackLocation.bindings.Count == 0)
            attackLocation.AddBinding("<Mouse>/position");
    }
    private void shoot()
    {
        // NetworkObject bullet = Runner.Spawn(bulletPrefab, firePoint.position, firePoint.rotation);
        NetworkObject bullet = Runner.Spawn(bulletPrefab, firePoint.position+firePoint.forward, firePoint.rotation,Object.InputAuthority);
        
        NetworkRigidbody rb = bullet.GetComponent<NetworkRigidbody>();
        var bullet_info = bullet.GetComponent<BulletScript>();
        bullet_info.shooter = player;

        rb.Rigidbody.AddForce(firePoint.forward * bulletForce, ForceMode.Impulse);
    }


    void Update() {
        if (!HasStateAuthority)  return;

        if (attack.WasPerformedThisFrame()) {
            // Vector2 attackLocationInScreenCoordinates = attackLocation.ReadValue<Vector2>();

            // var camera = Camera.main;
            // Ray ray = camera.ScreenPointToRay(attackLocationInScreenCoordinates);
            // ray.origin += camera.transform.forward;

            // Debug.DrawRay(ray.origin, ray.direction * shootDistance, Color.red, duration: 1f);

            // if (Runner.GetPhysicsScene().Raycast(ray.origin, ray.direction * shootDistance, out var hit)) {
            //     GameObject hitObject = hit.transform.gameObject;
            //     Debug.Log("Raycast hit: name="+ hitObject.name+" tag="+hitObject.tag+" collider="+hit.collider);
            //     if (hitObject.TryGetComponent<Health>(out var health)) {
            //         Debug.Log("Dealing damage");
            //         health.DealDamageRpc(Damage);
            //     }
            // }
            // firePoint = GameObject.Find()).transform;

            // firepoint equals the position of the instance of the player
            _controller = GetComponent<CharacterController>();
            Vector3 movement = attackLocation.ReadValue<Vector2>();
            firePoint = transform;

            
            
            shoot();


        }
    }

}