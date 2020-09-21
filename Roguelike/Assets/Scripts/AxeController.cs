using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AxeController : CloseWeaponController
{
    //활성화
    public static bool isActivate = true;

    private void Start()
    {
        Player.currentHand = currentCloseWeapon.GetComponent<Transform>();
        Player.currentHandAnim = currentCloseWeapon.anim;
    }

    void Update()
    {
        if (isActivate)
        {
            TryAttack();
            TryWalk();
        }
    }

    protected override IEnumerator HitCoroutine()
    {
        while (isSwing) {
            if (CheckObject()) {
                if (hitInfo.transform.tag == "Monster") {
                    hitInfo.transform.GetComponent<EnemyControll>().Damage(currentCloseWeapon.damage, transform.position);
                }
                isSwing = false;
                Debug.Log(hitInfo.transform.name);
            }
            yield return null;
        }
    }

    public override void CloseWeaponChange(CloseWeapon _closeWeapon)
    {
        base.CloseWeaponChange(_closeWeapon);
        isActivate = true;
    }
}
