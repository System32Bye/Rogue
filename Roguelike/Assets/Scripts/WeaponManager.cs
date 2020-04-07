using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour {

    //무기 중복 교체 실행 방지
    public static bool isChangeWeapon = false;

    //무기 교체 딜레이
    [SerializeField]
    private float changeWeaponDelayTime;
    //무기 교체 끝난 시점
    [SerializeField]
    private float changeWeaponEndDelayTime;

    //무기 종류 전부 관리
    [SerializeField]
    private Weapon[] weapons;

    private Dictionary<string, Weapon> weaponDictionary = new Dictionary<string, Weapon>();

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
