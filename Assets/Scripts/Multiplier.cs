using UnityEngine;
using UnityEngine.UI;





public class Multiplier : MonoBehaviour
{
    public int multiplier;
    [SerializeField] private Text multipleText;

    
    
    
    
    private void Start()
    {
        multipleText.text = "x" + multiplier.ToString();
    }
}