using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;





public class Enemy : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Animator animator;
    private Collider mainCollider;
    [SerializeField] private Rigidbody mainRigidbody;
    [SerializeField] private Rigidbody[] allRigidbodies;
    [SerializeField] private Collider[] allColliders;
    [SerializeField] private Transform[] allChildren;
    [SerializeField] private Text HPText;
    [SerializeField] private int life = 20;
    public int money;

    
    
    
    
    private void Start()
    {
        mainCollider = GetComponent<CapsuleCollider>();
        
        KinematicIs(true);
        HPText.text = life.ToString();
    }

    private void KinematicIs(bool state)
    {
        for (int i = 0; i < allRigidbodies.Length; i++)
        {
            allRigidbodies[i].isKinematic = state;
            allColliders[i].enabled = !state;
        }

        mainCollider.enabled = state;
        mainRigidbody.isKinematic = state;
        animator.enabled = state;
    }

    public void DecreaseLife(int damage, ContactPoint point, Vector3 bulletVelocity)
    {
        life -= damage;
        if (life >= 0)
            HPText.text = life.ToString();
        
        if (life == 0)
        {
            KinematicIs(false);
            mainRigidbody.AddForce(-bulletVelocity, ForceMode.VelocityChange);
            ScaleHP();
            
            foreach (Transform child in allChildren)
            {
                child.gameObject.layer = 6;
            }
            gameObject.layer = 6;
            
            gameManager.RemoveEnemy(this);
        }
    }

    private void ScaleHP()
    {
        HPText.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack);
    }
}
