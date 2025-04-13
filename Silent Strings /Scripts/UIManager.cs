using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI killCountText;
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private TextMeshProUGUI gunNameText;
    [SerializeField] private TextMeshProUGUI moreText;

    private int killCount = 0;

    public void UpdateKillCount()
    {
        killCount++;
        killCountText.text = "Kills: " + killCount;
    }

    public void UpdateAmmo(int currentAmmo, int maxAmmo)
    {
        ammoText.text = currentAmmo + " / " + maxAmmo;
    }

    public void UpdateGunName(string gunName)
    {
        gunNameText.text = gunName;
    }

    public void ShowMoreText()
    {
        moreText.text = "More";
        Invoke("HideMoreText", 2f);
    }

    private void HideMoreText()
    {
        moreText.text = "";
    }
}