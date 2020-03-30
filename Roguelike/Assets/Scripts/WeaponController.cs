using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {

    //현재 장착한 무기
    [SerializeField]
    private Weapon currentWeapon;

    //공격중
    private bool isAttack = false;
    private bool isSwing = false;

    private RaycastHit hitInfo;
    
	
	// Update is called once per frame
	void Update () {
        TryAttack();
	}

    private void TryAttack() {
        if (Input.GetButton("Space")) {
            if (!isAttack) {
                //코루틴 실행
                StartCoroutine(AttackCoroutine());
            }
        }
    }

    IEnumerator AttackCoroutine() {
        isAttack = true;
        currentWeapon.anim.SetTrigger("Attack");

        yield return new WaitForSeconds(currentWeapon.attackDelayA);
        isSwing = true;

        //공격 활성화 시점
        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(currentWeapon.attackDelayB);
        isSwing = false;

        yield return new WaitForSeconds(currentWeapon.attackDelay - currentWeapon.attackDelayA - currentWeapon.attackDelayB);
        isAttack = false;
    }

    IEnumerator HitCoroutine() {
        while (isSwing) {
            if (CheckObject())
            {
                isSwing = false;
                //충돌됨
                Debug.Log(hitInfo.transform.name);
            }
            yield return null;
        }
    }

    private bool CheckObject() {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, currentWeapon.range)) {
            return true;
        }
        return false;
    }
}
