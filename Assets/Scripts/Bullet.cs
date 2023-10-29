using System;
using System.Collections;
using UnityEngine;


public class Bullet : MonoBehaviour
{
    private Rigidbody rb;
    private GameManager gameManager;
    private int multipleCount;
    private int maxMultipleCount;

    
    
    

    private void FixedUpdate()
    {
        if (transform.position.x > 300 || transform.position.x < -200 || transform.position.z > 150 || transform.position.z < 70)
        {
            gameManager.RemoveBullet(this);
            Destroy(gameObject);
        }
    }

    public void InitBullet(Vector3 target, float force, GameManager gameManager, int multipleCount, int maxMultipleCount)
    {
        rb = GetComponent<Rigidbody>();
        this.gameManager = gameManager;
        this.multipleCount = multipleCount;
        this.maxMultipleCount = maxMultipleCount;

        Vector3 direction = target - transform.position;
        direction.Normalize();
        direction = new Vector3(direction.x, 0, direction.z);
        rb.AddRelativeForce(direction * force, ForceMode.VelocityChange);
        
        float rotation_y = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        Quaternion rotateTo = Quaternion.Euler(0, rotation_y + 180, 0);
        transform.rotation = rotateTo;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Enemy>().DecreaseLife(1, collision.contacts[0], rb.velocity);
            gameManager.RemoveBullet(this);
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Multiplier" && multipleCount < maxMultipleCount * 2)
        {
            multipleCount++;
            gameManager.MultiplyBullets(collision, collision.gameObject.GetComponent<Multiplier>().multiplier, multipleCount);
        }

        /*if (collision.gameObject.tag != "Borders")
        {
            Vector3 difference = collision.GetContact(0).point - transform.position;
            Vector3 direction = Vector3.Reflect(difference, collision.GetContact(0).normal);
            direction.Normalize();
            float rotation_y = Mathf.Atan2(difference.x, difference.z) * Mathf.Rad2Deg;
            Quaternion rotateTo = Quaternion.Euler(0, rotation_y, 0);
            transform.rotation = rotateTo;
        }*/

        //StartCoroutine(SaveVelocity());
    }

    private IEnumerator SaveVelocity()
    {
        yield return new WaitForSeconds(0.1f);
        
        double xVelocity = rb.velocity.x;
        double zVelocity = rb.velocity.z;
        double yVelocity = rb.velocity.y;

        //Debug.Log("Vector 1: " + (xVelocity * xVelocity + yVelocity * yVelocity + zVelocity * zVelocity)); 
        //Debug.Log("StartX: " + rb.velocity.x + " StartY: " + rb.velocity.y + " StartZ: " + rb.velocity.z);

        double hypotenuse = Math.Sqrt(xVelocity * xVelocity + zVelocity * zVelocity);
        double sinAlpha =  Math.Abs(zVelocity) / hypotenuse;
        double hypotenuse2 = hypotenuse + Math.Abs(yVelocity);
        //Debug.Log("Hypotenuse: " + hypotenuse + " sinAlpha: " + sinAlpha + " hypotenuse2: " + hypotenuse2);

        double newZVelocity = hypotenuse2 * sinAlpha;
        double newXVelocity = Math.Sqrt(hypotenuse2 * hypotenuse2 - newZVelocity * newZVelocity);
        if (xVelocity < 0)
            newXVelocity *= -1;
        if (zVelocity < 0)
            newZVelocity *= -1;
        //Debug.Log("newXVelocity: " + newXVelocity + " newZVelocity: " + newZVelocity);
        //Debug.Log("Vector 2: " + (newXVelocity * newXVelocity + rb.velocity.y * rb.velocity.y + newZVelocity * newZVelocity)); 


        rb.velocity = new Vector3((float)newXVelocity, 0, (float)newZVelocity);
    }
}