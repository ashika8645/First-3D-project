using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager instance { get; set; }

    [Header("Ammo")]
    public TextMeshProUGUI magazineAmmoUI;
    public TextMeshProUGUI totalAmmoUI;
    public Image ammoTypeUI;

    [Header("Weapon")]
    public Image activeWeaponUI;
    public Image unActiveWeaponUI;

    [Header("ThrowAbles")]
    public Image lethalUI;
    public TextMeshProUGUI lethalAmountUI;

    public Image tacticalUI;
    public TextMeshProUGUI tacticalAmountUI;

    public Sprite emptySlot;
    public Sprite greSlot;

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

    private void Update()
    {
        Weapon activeWeapon  = WeaponManager.instance.activeWeaponSlot.GetComponentInChildren<Weapon>();
        Weapon unActiveWeapon = GetUnACtiveWeaponSlot().GetComponentInChildren<Weapon>();

        if (activeWeapon)
        {
            magazineAmmoUI.text = $"{activeWeapon.bulletsLeft / activeWeapon.bulletPerBurst}";
            totalAmmoUI.text = $"{WeaponManager.instance.CheckAmmoLeftFor(activeWeapon.thisWeaponModel)}";

            Weapon.WeaponModel model =  activeWeapon.thisWeaponModel;
            ammoTypeUI.sprite = GetAmmoSprite(model);   

            activeWeaponUI.sprite = GetWeaponSprite(model);

            if (unActiveWeapon)
            {
                unActiveWeaponUI.sprite = GetWeaponSprite(unActiveWeapon.thisWeaponModel);
            }
        }
        else
        {
            magazineAmmoUI.text = "";
            totalAmmoUI.text = "";

            ammoTypeUI.sprite = emptySlot;

            activeWeaponUI.sprite = emptySlot;
            unActiveWeaponUI.sprite= emptySlot;
        }

        if (WeaponManager.instance.lethalCount <= 0)
        {
            lethalUI.sprite = greSlot;
        }

        if (WeaponManager.instance.TacticalCount <= 0)
        {
            tacticalUI.sprite = greSlot;
        }
    }

    private Sprite GetWeaponSprite(Weapon.WeaponModel model)
    {
        GameObject prefab = null;

        switch (model)
        {
            case Weapon.WeaponModel.Pistol1911:
                prefab = Resources.Load<GameObject>("Pistol1911_Weapon");
                break;

            case Weapon.WeaponModel.M4_8:
                prefab = Resources.Load<GameObject>("M4_8_Weapon");
                break;

            case Weapon.WeaponModel.AK47:
                prefab = Resources.Load<GameObject>("AK47_Weapon");
                break;

            case Weapon.WeaponModel.Uzi:
                prefab = Resources.Load<GameObject>("Uzi_Weapon");
                break;
        }

        return prefab.GetComponent<SpriteRenderer>().sprite;
    }

    private Sprite GetAmmoSprite(Weapon.WeaponModel model)
    {
        GameObject prefab = null;

        switch (model)
        {
            case Weapon.WeaponModel.Pistol1911:
                prefab = Resources.Load<GameObject>("Pistol_Ammo");
                break;

            case Weapon.WeaponModel.M4_8:
                prefab = Resources.Load<GameObject>("Rifle_Ammo");
                break;

            case Weapon.WeaponModel.AK47:
                prefab = Resources.Load<GameObject>("Rifle_Ammo");
                break;

            case Weapon.WeaponModel.Uzi:
                prefab = Resources.Load<GameObject>("Rifle_Ammo");
                break;
        }

        return prefab.GetComponent<SpriteRenderer>().sprite;
    }

    private GameObject GetUnACtiveWeaponSlot()
    {
        foreach (GameObject weaponSlot in WeaponManager.instance.weaponSlots)
        {
            if (weaponSlot != WeaponManager.instance.activeWeaponSlot)
            {
                return weaponSlot;
            }
        }
        return null;
    }

    public void UpdateThrowables()
    {
        lethalAmountUI.text = $"{WeaponManager.instance.lethalCount}";
        tacticalAmountUI.text = $"{WeaponManager.instance.TacticalCount}";

        switch (WeaponManager.instance.equippedLethalType)
        {
            case Throwable.ThrowableType.Grenade:
                lethalUI.sprite = Resources.Load<GameObject>("Grenade").GetComponent<SpriteRenderer>().sprite;
                break;
        }

        switch (WeaponManager.instance.equippedTacticalType)
        {
            case Throwable.ThrowableType.Smoke:
                tacticalUI.sprite = Resources.Load<GameObject>("Smoke").GetComponent<SpriteRenderer>().sprite;
                break;
        }
    }
}

