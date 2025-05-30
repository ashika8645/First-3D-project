using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; set; }

    public AudioSource ShootingChannel;
    public AudioSource emptyMagazineChannel;
    public AudioSource reloadingChannel;
    public AudioSource grenadeExplosion;

    public AudioClip colt1911Shot;
    public AudioClip M4_8Shot;

    public AudioClip colt1911Reloading;
    public AudioClip M4_8SReloading;

    public AudioClip colt1911EmptyMagazine;
    public AudioClip M4_8EmptyMagazine;

    public AudioClip grenadeExpl;
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

    public void PlayShootinSound(Weapon.WeaponModel weapon)
    {
        switch (weapon)
        {
            case Weapon.WeaponModel.Pistol1911:
                ShootingChannel.PlayOneShot(colt1911Shot);
                break;

            case Weapon.WeaponModel.M4_8:
                ShootingChannel.PlayOneShot(M4_8Shot);
                break;

            case Weapon.WeaponModel.AK47:
                ShootingChannel.PlayOneShot(M4_8Shot);
                break;

            case Weapon.WeaponModel.Uzi:
                ShootingChannel.PlayOneShot(M4_8Shot);
                break;
        }

    }

    public void PlayEmptyMagazineSound(Weapon.WeaponModel weapon)
    {
        switch (weapon)
        {
            case Weapon.WeaponModel.Pistol1911:
                emptyMagazineChannel.PlayOneShot(colt1911EmptyMagazine);
                break;

            case Weapon.WeaponModel.M4_8:
                emptyMagazineChannel.PlayOneShot(M4_8EmptyMagazine);
                break;

            case Weapon.WeaponModel.AK47:
                emptyMagazineChannel.PlayOneShot(M4_8EmptyMagazine);
                break;

            case Weapon.WeaponModel.Uzi:
                emptyMagazineChannel.PlayOneShot(M4_8EmptyMagazine);
                break;
        }

    }

    public void PlayReloadSound(Weapon.WeaponModel weapon)
    {
        switch (weapon)
        {
            case Weapon.WeaponModel.Pistol1911:
                reloadingChannel.PlayOneShot(colt1911Reloading);
                break;

            case Weapon.WeaponModel.M4_8:
                reloadingChannel.PlayOneShot(M4_8SReloading);
                break;

            case Weapon.WeaponModel.AK47:
                reloadingChannel.PlayOneShot(M4_8SReloading);
                break;

            case Weapon.WeaponModel.Uzi:
                reloadingChannel.PlayOneShot(M4_8SReloading);
                break;
        }

    }
}
