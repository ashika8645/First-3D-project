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

    [Header("Throwable")]
    public float throwForce = 40f;
    public GameObject throwspawnPoint;

    public int lethalCount = 0;
    public Throwable.ThrowableType equippedLethalType;
    public GameObject grenadePrefab;

    public int TacticalCount = 0;
    public Throwable.ThrowableType equippedTacticalType;
    public GameObject smokePrefab;

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

        equippedLethalType = Throwable.ThrowableType.None;
        equippedTacticalType = Throwable.ThrowableType.None;
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

        if (Input.GetKeyUp(KeyCode.G))
        {
            if (lethalCount > 0)
            {
                ThrowLethal();
            }
        }

        if (Input.GetKeyUp(KeyCode.T))
        {
            if (TacticalCount > 0)
            {
                ThrowTactical();
            }
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
            weaponToDrop.GetComponent<Weapon>().animator.enabled = false;

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



    public void PickedupThrowable(Throwable throwable)
    {
        switch (throwable.throwableType)
        {
            case Throwable.ThrowableType.Grenade:
                PickedupThrowableAsLethal(Throwable.ThrowableType.Grenade);
                break;
            case Throwable.ThrowableType.Smoke:
                PickedupThrowableAsTactical(Throwable.ThrowableType.Smoke);
                break;

        }
    }

    private void PickedupThrowableAsTactical(Throwable.ThrowableType tactical)
    {
        if (equippedTacticalType == tactical || equippedTacticalType == Throwable.ThrowableType.None)
        {
            equippedTacticalType = tactical;

            if (TacticalCount < 3)
            {
                TacticalCount += 1;
                Destroy(InteractionManager.instance.hoveredThrowable.gameObject);
                HUDManager.instance.UpdateThrowables();
            }
        }
    }

    private void PickedupThrowableAsLethal(Throwable.ThrowableType lethal)
    {
        if (equippedLethalType == lethal || equippedLethalType == Throwable.ThrowableType.None)
        {
            equippedLethalType = lethal;

            if (lethalCount < 3)
            {
                lethalCount += 1;
                Destroy(InteractionManager.instance.hoveredThrowable.gameObject);
                HUDManager.instance.UpdateThrowables();
            }
        }
    }


    public void ThrowLethal()
    {
        GameObject lethal = GetThrowablePrefab(equippedLethalType);

        GameObject throwable = Instantiate(grenadePrefab, throwspawnPoint.transform.position, throwspawnPoint.transform.rotation);
        Rigidbody rb = throwable.GetComponent<Rigidbody>();

        rb.AddForce(throwspawnPoint.transform.forward * throwForce, ForceMode.Impulse);

        throwable.GetComponent<Throwable>().hasBeenThrown = true;

        lethalCount -= 1;

        if (lethalCount == 0)
        {
            equippedLethalType = Throwable.ThrowableType.None;
        }

        HUDManager.instance.UpdateThrowables();
    }

    public void ThrowTactical()
    {
        GameObject tactical = GetThrowablePrefab(equippedTacticalType);

        GameObject throwable = Instantiate(smokePrefab, throwspawnPoint.transform.position, throwspawnPoint.transform.rotation);
        Rigidbody rb = throwable.GetComponent<Rigidbody>();

        rb.AddForce(throwspawnPoint.transform.forward * throwForce, ForceMode.Impulse);

        throwable.GetComponent<Throwable>().hasBeenThrown = true;

        TacticalCount -= 1;

        if (TacticalCount == 0)
        {
            equippedTacticalType = Throwable.ThrowableType.None;
        }

        HUDManager.instance.UpdateThrowables();
    }

    private GameObject GetThrowablePrefab(Throwable.ThrowableType throwableType)
    {
        switch (throwableType)
        {
            case Throwable.ThrowableType.Grenade:
                return grenadePrefab;

            case Throwable.ThrowableType.Smoke:
                return smokePrefab;

            default:
                return null;
        }
    }
}
