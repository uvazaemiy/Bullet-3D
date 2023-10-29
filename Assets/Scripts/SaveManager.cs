using UnityEngine;






public class SaveManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private int IdOfSavedLevel;
    [Space]
    [SerializeField] private LevelData[] allLevels;

    private Camera camera;
    
    
    
    
    
    private void Start()
    {
        camera = Camera.main;

        foreach (LevelData level in allLevels)
            level.gameObject.SetActive(false);
        
        if (PlayerPrefs.GetInt("SavedLevel") == null)
            PlayerPrefs.SetInt("SavedLevel", 1);
        
        if (PlayerPrefs.GetInt("Money") == null)
            PlayerPrefs.SetInt("Money", 0);
        
        uiManager.IncreaseMoney(PlayerPrefs.GetInt("Money"));

        IdOfSavedLevel = PlayerPrefs.GetInt("SavedLevel");
        if (IdOfSavedLevel == 0)
            IdOfSavedLevel++;

        allLevels[IdOfSavedLevel - 1].gameObject.SetActive(true);
        allLevels[IdOfSavedLevel - 1].Enemies.SetActive(false);
        allLevels[IdOfSavedLevel].gameObject.SetActive(true);

        gameManager.allEnemiesOfLevel = allLevels[IdOfSavedLevel].AllEnemies;
        gameManager.allMultipliers = allLevels[IdOfSavedLevel].allMultipliers;
        camera.transform.position = allLevels[IdOfSavedLevel].CamPosition.position;
        gameManager.playerControl.transform.position = allLevels[IdOfSavedLevel].GunPosition.position;
        uiManager.LevelText.text = "Level " + (IdOfSavedLevel).ToString();
    }

    public void IncreaseMoney(int money)
    {
        PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + money);
    }
    
    public void SaveLevel()
    {
        if (IdOfSavedLevel == allLevels.Length - 1)
            IdOfSavedLevel = 1;
        else
            IdOfSavedLevel++;
        PlayerPrefs.SetInt("SavedLevel", IdOfSavedLevel);
    }

    public void DeleteAllPrefs()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("All prefs have been deleted!");
    }
}
