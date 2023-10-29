using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;





public class UIManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Canvas canvas;
    [Space] 
    [Header("Buttons")] 
    [Space]
    [SerializeField] private Transform ResetButton;
    [SerializeField] private Transform SettingsButton;
    [SerializeField] private Image NextButton;
    [SerializeField] private Transform SFXButton;
    [SerializeField] private Transform MusicButton;
    [SerializeField] private Transform GunsButton;
    [Space] 
    [Header("Other")] 
    [Space] 
    [SerializeField] private Image LevelPanel;
    public Text LevelText;
    [SerializeField] private Image Background;
    [SerializeField] private Image GameLogo;
    [SerializeField] private Text TapToPlayText;
    [SerializeField] private GameObject WinningPanel;
    [SerializeField] private Text WinningText;
    [SerializeField] private Image FadeImage;
    [SerializeField] private Transform AddImage;
    [Space] 
    [Header("Logic")] 
    [Space] 
    [SerializeField] private Text moneyText;
    [SerializeField] private float ySettingsMoving;
    [SerializeField] private float xGunsMoving;
    [SerializeField] private float time = 0.2f;

    private Image SFXImage;
    private Image MusicImage;
    private Image ResetImage;
    
    private bool stateOfSettings;
    private int stateOfGuns = 1;
    private bool isMoving;
    private float settingYOffset = 0;
    private float yOffset = 1;
    private float xOffset = 1;

    private bool stateOfStart;
    private bool stateOfVolume;





    private void Start()
    {
        FadeImage.gameObject.SetActive(true);
        FadeImage.DOFade(0, time * 2.5f);
        StartCoroutine(DisableFade());
        
        xOffset = canvas.transform.position.x / 360;
        yOffset = canvas.transform.position.y / 800;

        SFXImage = SFXButton.GetComponent<Image>();
        MusicImage = MusicButton.GetComponent<Image>();
        ResetImage = ResetButton.GetComponent<Image>();

        SFXImage.color = new Color(SFXImage.color.r, SFXImage.color.g, SFXImage.color.b, 0);
        MusicImage.color = new Color(MusicImage.color.r, MusicImage.color.g, MusicImage.color.b, 0);
        ResetImage.color = new Color(ResetImage.color.r, ResetImage.color.g, ResetImage.color.b, 0);
    }

    private IEnumerator DisableFade()
    {
        yield return new WaitForSeconds(time);
        FadeImage.gameObject.SetActive(false);
    }

    private void EnableDisableObjects(bool state)
    {
        GameLogo.gameObject.SetActive(state);
        TapToPlayText.gameObject.SetActive(state);
        Background.gameObject.SetActive(state);
        NextButton.gameObject.SetActive(state);
        WinningPanel.SetActive(state);
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
            if (Input.GetMouseButtonUp(0) && !stateOfStart)
            {
                stateOfStart = !stateOfStart;
                StartCoroutine(HideExtraUI());
            }
        }
    }
    
    

    private IEnumerator HideExtraUI()
    {
        GameLogo.DOFade(0, time * 5);
        TapToPlayText.DOFade(0, time * 5);
        GunsButton.DOMoveX(GunsButton.position.x - xGunsMoving * xOffset, time * 4).SetEase(Ease.InBack);
        yield return Background.DOFade(0, time * 5).WaitForCompletion();
        
        EnableDisableObjects(false);

        //LevelPanel.DOMoveY(LevelPanel.position.y - ySettingsMoving * yOffset, time * 5);
        LevelPanel.DOFade(1, time * 5);
        LevelText.DOFade(1, time * 5);
        
        gameManager.StartGame();
    }

    public IEnumerator ShowExtraUI(string winningText, string nextButtonText)
    {
        EnableDisableObjects(true);
        
        NextButton.GetComponentInChildren<Text>().text = nextButtonText;
        
        WinningText.text = winningText;
        //Background.color = new Color(winColor.r, winColor.g, winColor.b, Background.color.a);
        
        GameLogo.DOFade(1, time * 5);
        yield return Background.DOFade(1, time * 5).WaitForCompletion();
        
        //GunsButton.DOMoveX(GunsButton.position.x + xGunsMoving * xOffset, time * 4);

        NextButton.DOFade(1, time * 5);
        NextButton.GetComponentInChildren<Text>().DOFade(1, time * 5);
    }
    
    public void ResetLevel()
    {
        StartCoroutine(ChangeLevel());
    }

    private IEnumerator ChangeLevel()
    {
        FadeImage.gameObject.SetActive(true);
        yield return FadeImage.DOFade(1, time * 2.5f).WaitForCompletion();
        
        gameManager.SceneChange();
    }





    public void MoveSettingsButtons()
    {
        if (!isMoving)
            StartCoroutine(MoveButtonsRoutine());
    }
    
    private IEnumerator MoveButtonsRoutine()
    {
        isMoving = true;

        if (!stateOfSettings)
        {
            SFXButton.gameObject.SetActive(true);
            MusicButton.gameObject.SetActive(true);
            ResetButton.gameObject.SetActive(true);

            SFXImage.DOFade(1, time);
            MusicImage.DOFade(1, time);
            ResetImage.DOFade(1, time);

            SFXButton.DOMoveY(SettingsButton.position.y - yOffset * ySettingsMoving, time);
            MusicButton.DOMoveY(SettingsButton.position.y - yOffset * ySettingsMoving * 2, time);
            yield return ResetButton.DOMoveY(SettingsButton.position.y - yOffset * ySettingsMoving * 3, time).WaitForCompletion();
        }
        else
        {
            SFXImage.DOFade(0, time);
            MusicImage.DOFade(0, time);
            ResetImage.DOFade(0, time);
            
            SFXButton.DOMoveY(SettingsButton.position.y, time);
            MusicButton.DOMoveY(SettingsButton.position.y, time);
            yield return ResetButton.DOMoveY(SettingsButton.position.y, time).WaitForCompletion();
            
            SFXButton.gameObject.SetActive(false);
            MusicButton.gameObject.SetActive(false);
            ResetButton.gameObject.SetActive(false);
        }
        
        stateOfSettings = !stateOfSettings;
        isMoving = false;
    }

    public void MoveGunsButton()
    {
        if (!isMoving)
            StartCoroutine(MoveGunsRoutine());
    }

    private IEnumerator MoveGunsRoutine()
    {
        isMoving = true;

        if (Convert.ToBoolean(stateOfGuns))
            AddImage.gameObject.SetActive(Convert.ToBoolean(stateOfGuns));
        
        AddImage.DOMoveY(AddImage.position.y + 220 * yOffset * stateOfGuns, time * 2);
        yield return GunsButton.DOMoveX(GunsButton.transform.position.x + xGunsMoving / 3 * xOffset * stateOfGuns, time * 2).WaitForCompletion();
        
        if (!Convert.ToBoolean(stateOfGuns))
            AddImage.gameObject.SetActive(!Convert.ToBoolean(stateOfGuns));

        stateOfGuns *= -1;
        
        
        isMoving = false;
    }






    public void IncreaseMoney(int money)
    {
        moneyText.text =  (int.Parse(moneyText.text) + money).ToString();
    }
}