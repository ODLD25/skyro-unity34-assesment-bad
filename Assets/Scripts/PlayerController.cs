using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //Settings
    [SerializeField]private float speed = 5.5f;
    [SerializeField]private int maxHealth = 100;
    
    //Stats
    [SerializeField]private float curHealth;
    
    //Shooting
    [SerializeField]private GameObject ammoPrefab;
    [SerializeField]private float shootCooldown = 0.18f;
    [SerializeField]private float shootForce = 10f;
    [SerializeField]private Transform shootPoint;
    
    //References
    [SerializeField]private HudStuff hud;
    [SerializeField]private Rigidbody2D rb;

    private InputSystem_Actions inputActions;

    private bool canShoot;

    void Start()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Player.Enable();

        curHealth = maxHealth;

        canShoot = true;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && canShoot)
        {
            Shoot();
        }

        Rotate();
    }

    void FixedUpdate()
    {
        Move();
        SpeedControl();
    }

    private void Rotate()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        mousePos.z = Mathf.Abs(Camera.main.transform.position.z - transform.position.z);
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 dir = mousePos - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }

    private void Move()
    {
        float horizontal = inputActions.Player.Move.ReadValue<Vector2>().x;
        float vertical = inputActions.Player.Move.ReadValue<Vector2>().y;

        rb.AddForce(Vector3.up * vertical * speed * 100 * Time.fixedDeltaTime + Vector3.right * horizontal * speed * 100 * Time.fixedDeltaTime, ForceMode2D.Force);
    }

    private void SpeedControl()
    {
        if (rb.linearVelocity.magnitude > speed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * speed;
        }
    }

    void Shoot()
    {
        GameObject spawnedAmmo = Instantiate(ammoPrefab, shootPoint.position, Quaternion.identity);
        spawnedAmmo.GetComponent<Rigidbody2D>().AddForce(transform.up * shootForce, ForceMode2D.Force);

        canShoot = false;
        Destroy(spawnedAmmo, 3f);
        Invoke(nameof(ResetShoot), shootCooldown);
    }

    private void ResetShoot()
    {
        canShoot = true;
    }

    public void DealDmg(float dmg)
    {
        curHealth -= dmg;

        hud.UpdateHealth(curHealth);
        
        if (curHealth <= 0)
        {
            GameManager.instance.EndGame();
        }
    }

    void OnDisable()
    {
        inputActions.Player.Disable();
    }
}
