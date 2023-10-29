using UnityEngine;
using UnityEngine.UI;

public class SoundButtonData : MonoBehaviour
{
    [SerializeField] private Image currentImage;
    [SerializeField] private Sprite soundOn;
    [SerializeField] private Sprite soundOff;
    private float savedVolume;
    private bool stateOfVolume;

    public float ChangeVolume(float currentVolume)
    {
        float returnedVolume;
        
        if (!stateOfVolume)
        {
            savedVolume = currentVolume;
            currentImage.sprite = soundOff;
            returnedVolume = 0;
        }
        else
        {
            currentImage.sprite = soundOn;
            returnedVolume = savedVolume;
        }
        
        stateOfVolume = !stateOfVolume;
        return returnedVolume;
    }
}