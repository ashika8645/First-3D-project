using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager instance { get; set; }

    public List<GameObject> weaponSlots;

    public GameObject activeWeaponSlot;

    [Header("Ammo")]
    public int totalRifleAmmo = 0;
    public int totalPistolAmmo = 0;
    public int totalSMGAmmo = 0;
    public int totalShortgunAmmo = 0;
    public int totalSniperAmmo = 0;
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

    private void Start()
    {
        activeWeaponSlot = weaponSlots[0];
    }

    private void Update()
    {
        foreach (GameObject weaponSlot in weaponSlots)
        {
            if (weaponSlot == activeWeaponSlot)
            {
                weaponSlot.SetActive(true);
            }
            else
            {
                weaponSlot.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchActiveWeaponSlot(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchActiveWeaponSlot(1);
        }
    }

    public void PickedupWeapon(GameObject pickedupWeapon)
    {
        AddWeaponInToActiveSlot(pickedupWeapon);
    }

    public void PickedupAmmo(AmmoBox ammo)
    {
        switch (ammo.ammoType)
        {
            case AmmoBox.AmmoType.PistolAmmo:
                totalPistolAmmo += ammo.ammoAmount;
                break;

            case AmmoBox.AmmoType.RifeAmmo:
                totalRifleAmmo += ammo.ammoAmount;
                break;

            case AmmoBox.AmmoType.SMGAmmo:
                totalSMGAmmo += ammo.ammoAmount;
                break;

            case AmmoBox.AmmoType.ShortgunAmmo:
                totalShortgunAmmo += ammo.ammoAmount;
                break;

            case AmmoBox.AmmoType.SniperAmmo:
                totalSniperAmmo += ammo.ammoAmount; 
                break;
        }
    }

    public void AddWeaponInToActiveSlot(GameObject pickupedWeapon)
    {
        DropCurrentWeapon(pickupedWeapon);

        pickupedWeapon.transform.SetParent(activeWeaponSlot.transform, false);

        Weapon weapon = pickupedWeapon.GetComponent<Weapon>();

        pickupedWeapon.transform.localPosition = new Vector3(weapon.spawnPosition.x, weapon.spawnPosition.y, weapon.spawnPosition.z);
        pickupedWeapon.transform.localRotation = Quaternion.Euler(weapon.spawnRotation.x, weapon.spawnRotation.y, weapon.spawnRotation.z);

        weapon.isActiveWeapon = true;
        weapon.animator.enabled = true;

    }

    public void DropCurrentWeapon(GameObject pickedupWeapon)
    {
        if (activeWeaponSlot.transform.childCount > 0)
        {
            var weaponToDrop = activeWeaponSlot.transform.GetChild(0).gameObject;

            weaponToDrop.GetComponent<Weapon>().isActiveWeapon = false;
            weaponToDrop.GetComponent <Weapon>().animator.enabled = false;

            weaponToDrop.transform.SetParent(pickedupWeapon.transform.parent);
            weaponToDrop.transform.localPosition = pickedupWeapon.transform.localPosition;
            weaponToDrop.transform.localRotation = pickedupWeapon.transform.localRotation;
        }
    }

    public void SwitchActiveWeaponSlot(int SlotNumber)
    {
        if (SlotNumber >= weaponSlots.Count || weaponSlots[SlotNumber] == null)
        {
            Debug.LogWarning("Invalid weapon slot number or slot is null.");
            return;
        }

        if (activeWeaponSlot != null && activeWeaponSlot.transform.childCount > 0)
        {
            Transform child = activeWeaponSlot.transform.GetChild(0);
            if (child != null)
            {
                Weapon currentWeapon = child.GetComponent<Weapon>();
                if (currentWeapon != null)
                {
                    currentWeapon.isActiveWeapon = false;
                }
            }
        }

        activeWeaponSlot = weaponSlots[SlotNumber];

        if (activeWeaponSlot != null && activeWeaponSlot.transform.childCount > 0)
        {
            Transform child = activeWeaponSlot.transform.GetChild(0);
            if (child != null)
            {
                Weapon newWeapon = child.GetComponent<Weapon>();
                if (newWeapon != null)
                {
                    newWeapon.isActiveWeapon = true;
                }
            }
        }
    }

    internal void DecreaseTotalAmmo(int bulletToDecrease, Weapon.WeaponModel thisWeaponModel)
    {
        switch (thisWeaponModel)
        {
            case Weapon.WeaponModel.M4_8:
                totalRifleAmmo -= bulletToDecrease;
                break;

            case Weapon.WeaponModel.Pistol1911:
                totalPistolAmmo -= bulletToDecrease;
                break;

            case Weapon.WeaponModel.AK47:
                totalRifleAmmo -= bulletToDecrease;
                break;

            case Weapon.WeaponModel.Uzi:
                totalSMGAmmo -= bulletToDecrease;
                break;
        }
    }
    public int CheckAmmoLeftFor(Weapon.WeaponModel thisWeaponModel)
    {
        switch (thisWeaponModel)
        {
            case Weapon.WeaponModel.Pistol1911:
                return totalPistolAmmo;

            case Weapon.WeaponModel.M4_8:
                return totalRifleAmmo;

            case Weapon.WeaponModel.AK47:
                return totalRifleAmmo;

            case Weapon.WeaponModel.Uzi:
                return totalSMGAmmo;

            default:
                return 0;
        }
    }
}
