﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {

    //현재 총
    [SerializeField]
    private Gun currentGun;

    private float currentFireRate;      //연사속도

    private AudioSource audioSC;

    bool IsWalk = false;
    private bool isReload = false;

    private RaycastHit hitInfo;         //레이저 충돌 정보

    //피격
    [SerializeField]
    private GameObject hitEffectPrefab;
    

    void Start() {
        audioSC = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update () {
        GunFireRateCalc();
        TryFire();

        currentGun.anim.SetBool("Walk", IsWalk);

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

    //연사속도
    private void GunFireRateCalc() {
        if (currentFireRate > 0)
            currentFireRate -= Time.deltaTime;
    }

    //발사 시도
    private void TryFire() {
        if (Input.GetKey(KeyCode.Space) && currentFireRate <= 0 && !isReload)
        {
            Fire();
        }
    }

    //발사 전
    private void Fire() {
        if (!isReload) {
            if (currentGun.currentBulletCount > 0)
                Shoot();
            else
                StartCoroutine(ReloadCoroutine());
        }
    }

    //발사 후
    private void Shoot() {
        currentGun.currentBulletCount--;
        currentFireRate = currentGun.fireRate;  //연사속도 재계산
        PlaySE(currentGun.fire_Sound);
        currentGun.muzzleFlash.Play();
        Hit();
        //Debug.Log("발사");
    }

    private void Hit() {
        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, currentGun.range)){
            var clone = Instantiate(hitEffectPrefab, new Vector3(hitInfo.point.x, hitInfo.point.y+1f, hitInfo.point.z), hitInfo.transform.rotation);
            Destroy(clone, 2f);
        }
    }
    

    //재장전
    IEnumerator ReloadCoroutine()
    {
        if (currentGun.currentBulletCount == 0) {

            isReload = true;

            currentGun.anim.SetTrigger("Reload");

            yield return new WaitForSeconds(currentGun.reloadTime);

            currentGun.currentBulletCount = currentGun.reloadBulletCount;
        }

        isReload = false;
    }

    //사운드 재생
    private void PlaySE(AudioClip _clip) {
        audioSC.clip = _clip;
        audioSC.Play();
    }

}