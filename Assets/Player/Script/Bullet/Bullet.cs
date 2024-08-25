using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float mySpeed;
    [SerializeField] private Vector2 myDirection;
    [SerializeField] private GameObject myParticleStart;
    [SerializeField] private ScreenLimit screenLimitX = new ScreenLimit(9.5f, -9.5f);
    [SerializeField] private ScreenLimit screenLimitY = new ScreenLimit(6.0f, -6.0f);

    private string myEnemy = "Enemy";
    private string myOwner = "Player";
    private Rigidbody2D myRigidbody2D;

    #region Unity
    private void Awake()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        Destroy(Instantiate(myParticleStart, transform.position, Quaternion.identity), 0.5f);
    }
    private void Update()
    {
        myRigidbody2D.velocity = myDirection * mySpeed;
        if (transform.position.x > screenLimitX.maxLimit || transform.position.x < screenLimitX.minLimit || transform.position.y > screenLimitY.maxLimit || transform.position.y < screenLimitY.minLimit)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(myEnemy))
        {
            //Destroy(collision.gameObject);
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Elevator"))
        {
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Gravity"))
        {
            Destroy(gameObject);
        }
    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
    private void OnBecameVisible()
    {
        Destroy(gameObject);
    }
    #endregion

    #region Set
    public void SetBullet(string owner, string enemy, Vector2 direction)
    {
        myOwner = owner;
        myEnemy = enemy;
        myDirection = direction.normalized;
    }
    #endregion
}