using UnityEngine;

public class Knife : MonoBehaviour
{
    private string myEnemy = "Enemy";
    private string myOwner = "Player";
    private ParticleSystem myKnifeParticle;

    #region Unity
    private void Awake()
    {
        Destroy(gameObject, 0.4f);
        myKnifeParticle = GetComponent<ParticleSystem>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(myEnemy))
        {
            //Destroy(collision.gameObject);
            Explode();
        }
    }
    #endregion

    #region Set
    public void SetKnife(string owner, string enemy, float angle)
    {
        myOwner = owner;
        myEnemy = enemy;
        if (myKnifeParticle != null)
        {
            myKnifeParticle.Play();
        }
        int myDirection = angle == 0 ? 0 : 180; // Angle 0 ise sağa bakıyor, 180 ise sola bakıyor demektir
        transform.eulerAngles = Vector3.up * myDirection;
    }
    #endregion

    #region Genel
    private void Explode()
    {
        Invoke("DestroyKnife", 1);
    }
    private void DestroyKnife()
    {
        Destroy(gameObject);
    }
    #endregion
}