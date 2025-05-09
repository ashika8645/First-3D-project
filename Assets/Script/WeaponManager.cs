using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager instance { get; set; }

    public List<GameObject> weaponSlots;

    public GameObject activeWeaponSlot;

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

    public void pickedupWeapon(GameObject pickedupWeapon)
    {
        AddWeaponInToActiveSlot(pickedupWeapon);
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


}
