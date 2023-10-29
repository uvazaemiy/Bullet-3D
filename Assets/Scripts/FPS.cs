using System.Collections;
using UnityEngine;
using UnityEngine.UI;






public class FPS : MonoBehaviour
{
    public bool isOn;
    [SerializeField] private Text FPSText;
    private float count;

    
    
    

    private void Start()
    {
        StartCoroutine(CountFPS());
    }

    private IEnumerator CountFPS()
    {
        if (isOn)
        {
            count = Mathf.Round(1 / Time.deltaTime);
            FPSText.gameObject.SetActive(true);
            FPSText.text = "FPS: " + count.ToString();
        }
        else
            FPSText.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.2f);
        StartCoroutine(CountFPS());
    }

    public void ChangeFPS(bool isOn)
    {
        this.isOn = isOn;
    }
}