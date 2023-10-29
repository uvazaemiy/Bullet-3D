using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;





public class GameManager : MonoBehaviour
{
    [Header("Other managers")]
    [Space]
    public PlayerControl playerControl;
    [SerializeField] private UIManager uIManager;
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private SaveManager saveManager;
    [Space] 
    [Header("GameLogic")] 
    [Space] 
    public List<Enemy> allEnemiesOfLevel;
    public List<Multiplier> allMultipliers;
    [SerializeField] private List<Bullet> allBullets;
    [SerializeField] private GameObject bulletPref;
    [SerializeField] private Transform bullletsParent;
    [Space] 
    [Header("Other")] 
    [Space]
    [SerializeField] private ParticleSystem[] allParticles;
    
    public float bulletForce = 5;

    
    
    

    private void Start()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
    }

    public void StartGame()
    {
        playerControl.canShoot = true;
    }
    
    public Bullet SpawnBullet(Vector3 spawnPosition, Vector3 target, int multipleCount)
    {
        GameObject bullet = Instantiate(bulletPref, spawnPosition, Quaternion.identity, bullletsParent) as GameObject;
        Bullet bulletComponent = bullet.GetComponent<Bullet>(); 
        
        bulletComponent.InitBullet(target, bulletForce, this, multipleCount, allMultipliers.Count);
        bullet.transform.position = new Vector3(bullet.transform.position.x, playerControl.bulletSpawn.position.y, bullet.transform.position.z);
        
        allBullets.Add(bulletComponent);
        return bulletComponent;
    }

    public void MultiplyBullets(Collision multibox, int multiplier, int multipleCount)
    {
        for (int i = 0; i < multiplier - 1; i++)
        {
            Vector3 difference = multibox.GetContact(0).point - multibox.transform.position;

            Vector3 boxSpawn = multibox.GetContact(0).point + difference * 2;

            float randomX = Random.Range(0.1f, 1f);
            float randomZ = 1 - randomX;
            int index1 = 0;
            int index2 = 0;
            while ((index1 == 0) || (index2 == 0))
            {
                index1 = Random.Range(-1, 2);
                index2 = Random.Range(-1, 2);
            } 
            
            randomX *= (float)index1;
            randomZ *= (float)index2;
            if ((index1 == 0) || (index2 == 0))
                Debug.Log("pizda");
            
            Vector3 randomVector = new Vector3(randomX, 0, randomZ);
            Vector3 target = boxSpawn + randomVector;
            Bullet newBullet = SpawnBullet(boxSpawn, target, multipleCount);
        }
    }

    public void RemoveEnemy(Enemy enemy)
    {
        allEnemiesOfLevel.Remove(enemy);
        
        saveManager.IncreaseMoney(enemy.money);
        uIManager.IncreaseMoney(enemy.money);
        
        if (allEnemiesOfLevel.Count == 0)
        {
            saveManager.SaveLevel();
            
            soundManager.PlayWinSound();
            foreach (ParticleSystem particleSystem in allParticles)
                particleSystem.Play();
            
            StartCoroutine(uIManager.ShowExtraUI("You won!", "NEXT"));
        }
    }

    public void RemoveBullet(Bullet bullet)
    {
        allBullets.Remove(bullet);
        if (allBullets.Count == 0)
        {
            //soundManager.PlayLoseSound();
            StartCoroutine(uIManager.ShowExtraUI("You lose!", "RESTART"));
        }
    }

    public void SceneChange()
    {
        SceneManager.LoadScene("MainGame");
    }
}