using TMPro;
using UnityEngine;

public class HudStuff : MonoBehaviour
{
    //References
    public TextMeshProUGUI healthTxtObject;
    public TextMeshProUGUI scoreTxtObject;

    public void UpdateHealth(float health)
    {
        healthTxtObject.text = "Health: " + health;
    }

    public void UpdateScore(int score)
    {
        scoreTxtObject.text = "Score: " + score;
    }
}
