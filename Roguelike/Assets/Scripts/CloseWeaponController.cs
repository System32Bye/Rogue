using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//자식이 완성 시키는 클래스
public abstract class CloseWeaponController : MonoBehaviour
{
    
    //현재 장착한 무기
    [SerializeField]
    protected CloseWeapon currentCloseWeapon;


    //공격중
    protected bool isAttack = false;
    protected bool isSwing = false;

    protected RaycastHit hitInfo;

    protected bool IsWalk = false;


    // Update is called once per frame


    protected void TryWalk()
    {
        currentCloseWeapon.anim.SetBool("IsWalk", IsWalk);

        if (Input.GetKey(KeyCode.LeftArrow) ||
            Input.GetKey(KeyCode.RightArrow) ||
            Input.GetKey(KeyCode.UpArrow) ||
            Input.GetKey(KeyCode.DownArrow))
        {
            IsWalk = true;
        }
        else
            IsWalk = false;
    }

    protected void TryAttack()
    {
        if (Input.GetKey(KeyCode.C))
        {
            if (!isAttack)
            {
                
                //코루틴 실행
                StartCoroutine(AttackCoroutine());
            }
        }
    }

    protected IEnumerator AttackCoroutine()
    {
        isAttack = true;
        currentCloseWeapon.anim.SetTrigger("playerAttack");

        yield return new WaitForSeconds(currentCloseWeapon.attackDelayA);
        isSwing = true;

        //공격 활성화 시점
        StartCoroutine(HitCoroutine());
        
        yield return new WaitForSeconds(currentCloseWeapon.attackDelayB);
        isSwing = false;

        yield return new WaitForSeconds(currentCloseWeapon.attackDelay - currentCloseWeapon.attackDelayA - currentCloseWeapon.attackDelayB);
        isAttack = false;

    }

    //자식이 완성시킴
    protected abstract IEnumerator HitCoroutine();

    protected bool CheckObject()
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, currentCloseWeapon.range))
        {
            return true;
        }
        return false;
    }

    //완성이지만, 추가 편집 함수
    public virtual void CloseWeaponChange(CloseWeapon _closeWeapon)
    {
        if (Player.currentHand != null)
            Player.currentHand.gameObject.SetActive(false);

        currentCloseWeapon = _closeWeapon;
        Player.currentHand = currentCloseWeapon.GetComponent<Transform>();
        Player.currentHandAnim = currentCloseWeapon.anim;

        currentCloseWeapon.transform.localPosition = Vector3.zero;
        currentCloseWeapon.gameObject.SetActive(true);
        
    }
}
