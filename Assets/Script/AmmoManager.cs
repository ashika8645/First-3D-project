using TMPro;
using UnityEngine;

public class AmmoManager : MonoBehaviour
{
    public static AmmoManager instance { get; set; }
    public TextMeshProUGUI ammoDisplay;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
}
