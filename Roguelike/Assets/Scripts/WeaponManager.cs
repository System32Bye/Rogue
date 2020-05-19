using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour {

    //무기 중복 교체 실행 방지
    public static bool isChangeWeapon = false;

    //현재 무기와 무기 애니메이션
    public static Transform currentWeapon;
    public static Animator currentWeaponAnim;

    //현재 무기 타입
    [SerializeField]
    private string currentWeaponType;
    
    //무기 교체 딜레이
    [SerializeField]
    private float changeWeaponDelayTime;
    //무기 교체 끝난 시점
    [SerializeField]
    private float changeWeaponEndDelayTime;

    //무기 종류(원/근) 관리
    [SerializeField]
    private Gun[] guns;
    [SerializeField]
    private CloseWeapon[] weapons;

    //관리 쉽게 무기 접근 가능
    private Dictionary<string, Gun> gunDictionary = new Dictionary<string, Gun>();
    private Dictionary<string, CloseWeapon> weaponDictionary = new Dictionary<string, CloseWeapon>();

    //필요한 컴포넌트
    [SerializeField]
    private GunController theGunController;
    [SerializeField]
    private WeaponController theWeaponController;
    
    // Use this for initialization
    void Start () {
        for (int i = 0; i < guns.Length; i++)
        {
            gunDictionary.Add(guns[i].gunName, guns[i]);
        }
        for (int i = 0; i < weapons.Length; i++)
        {
            weaponDictionary.Add(weapons[i].closeWeaponName, weapons[i]);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (!isChangeWeapon) {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                StartCoroutine(ChangeWeaponCoroutine("WEAPON", "Weapon"));
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                StartCoroutine(ChangeWeaponCoroutine("GUN", "Gun"));
            }
        }
	}

    public IEnumerator ChangeWeaponCoroutine(string _type, string _name) {
        isChangeWeapon = true;
        currentWeaponAnim.SetTrigger("WeaponOut");

        yield return new WaitForSeconds(changeWeaponDelayTime);

        CancelPreWeaponAction();
        WeaponChange(_type, _name);

        yield return new WaitForSeconds(changeWeaponEndDelayTime);

        currentWeaponType = _type;
        isChangeWeapon = false;
    }

    private void CancelPreWeaponAction() {
        switch (currentWeaponType) {
            case "GUN":
                theGunController.CancelReload();
                GunController.isActivate = false;
                break;
            case "WEAPON":
                WeaponController.isActivate = false;
                break;
        }
    }

    private void WeaponChange(string _type, string _name) {
        if (_type == "GUN")
            theGunController.GunChange(gunDictionary[_name]);
        else if (_type == "WEAPON")
            theWeaponController.CloseWeaponChange(weaponDictionary[_name]);
    }
}
