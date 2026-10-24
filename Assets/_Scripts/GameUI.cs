using UnityEngine;
using UnityEngine.UI;


public class GameUI : MonoBehaviour
{
    [Header("In Game")]
    public Image levelSlider;
    public Image currentLevelImg;
    public Image nextLevelImg;

    private Material ballMat;

    private void Awake()
    {
        ballMat = FindAnyObjectByType<Ball>().transform.GetChild(0).GetComponent<MeshRenderer>().material;

        levelSlider.transform.parent.GetComponent<Image>().color = ballMat.color + Color.gray;
        levelSlider.color = ballMat.color;
        currentLevelImg.color = ballMat.color;
        nextLevelImg.color = ballMat.color;
    }

    private void Update()
    {
        
    }

    public void LevelSliderFill(float fillAmount)
    {
        levelSlider.fillAmount = fillAmount;
    }

}
