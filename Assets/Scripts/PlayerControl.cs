using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;





public class PlayerControl : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private SoundManager soundManager;
    public Transform bulletSpawn;
    [SerializeField] private GameObject vfxShoot;
    [SerializeField] private Animation animation;
    
    private Camera mainCam;
    public  bool canShoot;
    
    private Quaternion rotateTo;
    private RaycastHit hit;


    
    
    
    private void Start()
    {
        mainCam = Camera.main;
    }
    
    private bool IsPointerOverUIObject() 
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
    
    private void Update()
    {
        if (!IsPointerOverUIObject())
        {
            if (Input.GetMouseButton(0) && canShoot)
            {
                Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~((1 << 6) | (1 << 7) | (1 << 8))))
                {
                    Vector3 difference = hit.point - transform.position;
                    difference.Normalize();
                    float rotation_y = Mathf.Atan2(difference.x, difference.z) * Mathf.Rad2Deg;
                    rotateTo = Quaternion.Euler(0, rotation_y, 0);
                    transform.rotation = rotateTo;
                }
            }

            if (Input.GetMouseButtonUp(0) && canShoot)
            {
                canShoot = false;
                
                GameObject vfxShot = Instantiate(vfxShoot, bulletSpawn.position, Quaternion.identity) as GameObject;
                vfxShot.transform.rotation = rotateTo * Quaternion.Euler(0, -90, 0);
                animation.Play();
            
                soundManager.PlayShotSound();
                gameManager.SpawnBullet(bulletSpawn.position, hit.point, 0);
            }
        }
    }
}
