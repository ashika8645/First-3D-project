using System.Collections;
using TMPro;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public bool isActiveWeapon;

    public bool isShooting, readyToShot;
    bool allowReset = true;
    public float shootingDelay = 2f;

    public int bulletPerBurst = 3;
    public int burstBulletsLeft;

    public float spreadIntensity;

    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30;
    public float bulletPrefabLifeTime = 3f;

    public GameObject muzzleEffect;
    internal Animator animator;

    public float reloadTime;
    public float magazineSize, bulletsLeft;
    public bool isReloading;

    public Vector3 spawnPosition;
    public Vector3 spawnRotation;

    public enum WeaponModel
    {
        Pistol1911,
        M4_8,
        AK47,
        Uzi
    }

    public WeaponModel thisWeaponModel;

    public enum ShootingMode
    {
        Single,
        Burst,
        Auto
    }

    public ShootingMode currentShootingMode;

    public void Awake()
    {
        readyToShot = true;
        burstBulletsLeft = bulletPerBurst;
        animator = GetComponent<Animator>();

        bulletsLeft = magazineSize; 
    }

    void Start()
    {
    }


    void Update()
    {
        if (isActiveWeapon)
        {
            GetComponent<Outline>().enabled = false;

            if (currentShootingMode == ShootingMode.Auto)
            {
                isShooting = Input.GetKey(KeyCode.Mouse0);
            }
            else if (currentShootingMode == ShootingMode.Single)
            {
                isShooting = Input.GetKeyDown(KeyCode.Mouse0);
            }

            if (Input.GetKey(KeyCode.R) && bulletsLeft < magazineSize && !isReloading)
            {
                Reload();
            }

            //if (readyToShot && !isShooting && !isReloading && bulletsLeft <= 0)
            //{
            //    Reload();
            //}

            if (readyToShot && isShooting && bulletsLeft > 0)
            {
                burstBulletsLeft = bulletPerBurst;
                FireWeapon();
            }

            if (bulletsLeft == 0)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    FireEmpty();
                }
            }

        }
    }

    private void FireWeapon()
    {

        bulletsLeft--;

        muzzleEffect.GetComponent<ParticleSystem>().Play();
        animator.SetTrigger("RECOIL");

        SoundManager.instance.PlayShootinSound(thisWeaponModel);

        readyToShot = false;

        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);

        bullet.transform.forward = shootingDirection;

        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity, ForceMode.Impulse);


        StartCoroutine(DestroyBulletAfterTime(bullet, bulletPrefabLifeTime));

        if (allowReset)
        {
            Invoke("ResetShot", shootingDelay);
            allowReset = false;
        }

        if (currentShootingMode == ShootingMode.Burst && burstBulletsLeft > 1)
        {
            burstBulletsLeft--;
            Invoke("Fireweapon", shootingDelay);
        }
    }

    private void FireEmpty()
    {
        SoundManager.instance.PlayEmptyMagazineSound(thisWeaponModel);
        animator.SetTrigger("Empty");

        StartCoroutine(ResetTriggerAfterFrame("Empty"));
    }

    private IEnumerator ResetTriggerAfterFrame(string triggerName)
    {
        yield return null; 
        animator.ResetTrigger(triggerName);
    }

    private void Reload()
    {
        animator.SetTrigger("Reload");
        SoundManager.instance.PlayReloadSound(thisWeaponModel);
        isReloading = true;
        Invoke("ReloadCompleted", reloadTime);
    }

    private void ReloadCompleted()
    {
        bulletsLeft = magazineSize;
        isReloading = false;
    }

    private void ResetShot()
    {
        readyToShot = true;
        allowReset = true;  
    }

    public Vector3 CalculateDirectionAndSpread()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.5f));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100);
        }

        Vector3 direction = targetPoint - bulletSpawn.position;

        float x = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        return direction + new Vector3(x, y, 0);
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}
