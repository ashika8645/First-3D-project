using UnityEngine;

public class GlobalReferences : MonoBehaviour
{
    public static GlobalReferences instance { get; set; }

    public GameObject bulletImpactEffectPrefab;

    public GameObject grenadeExplosionEffectl;
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
