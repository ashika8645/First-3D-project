using UnityEngine;

public class GoblaReferences : MonoBehaviour
{
    public static GoblaReferences instance {  get; set; }

    public GameObject bulletImpactEffectPrefab;

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
