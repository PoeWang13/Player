using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private Vector2 myPower;
    [SerializeField] private ScreenLimit screenLimitX = new ScreenLimit(9.5f, -9.5f);
    [SerializeField] private ScreenLimit screenLimitY = new ScreenLimit(6.0f, -6.0f);

    private int bounce;
    private GameObject myView;
    public int myDirection = 1;
    private string myEnemy = "Enemy";
    private string myOwner = "Player";
    private Rigidbody2D myRigidbody2D;
    private ParticleSystem myExplodeParticle;

    #region Unity
    private void Awake()
    {
        myView = transform.Find("Bomb-View").gameObject;
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myExplodeParticle = GetComponent<ParticleSystem>();
    }
    private void Update()
    {
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
            Explode();
        }
        if (collision.CompareTag("Ground"))
        {
            if (bounce == 1)
            {
                Explode();
                return;
            }
            bounce++;
            myRigidbody2D.velocity = Vector2.zero;
            myRigidbody2D.AddForce(new Vector2(myDirection * myPower.x, myPower.y), ForceMode2D.Impulse);
        }
    }
    #endregion

    #region Set
    public void SetBomb(string owner, string enemy, float angle)
    {
        myOwner = owner;
        myEnemy = enemy;
        myDirection = angle == 0 ? 1 : -1; // Angle 0 ise sağa bakıyor, 180 ise sola bakıyor demektir
        myRigidbody2D.AddForce(new Vector2(myDirection * myPower.x, myPower.y), ForceMode2D.Impulse);
    }
    #endregion

    #region Genel
    private void Explode()
    {
        myView.SetActive(false);
        myExplodeParticle.Play();
        myRigidbody2D.gravityScale = 0;
        Invoke("DestroyBomb", 0.1f);
        myRigidbody2D.velocity = Vector2.zero;
    }
    private void DestroyBomb()
    {
        Destroy(gameObject);
    }
    [ContextMenu("Throw")]
    private void Throw()
    {
        myRigidbody2D.AddForce(new Vector2(myDirection * myPower.x, myPower.y), ForceMode2D.Impulse);
    }
    #endregion
}