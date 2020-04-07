using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {
    [SerializeField]
    private Gun currentGun;

    private float currentFireRate;

    private AudioSource audioSC;

    bool IsWalk = false;

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

    private void GunFireRateCalc() {
        if (currentFireRate > 0)
            currentFireRate -= Time.deltaTime;
    }

    private void TryFire() {
        if (Input.GetKey(KeyCode.Space) && currentFireRate <= 0)
        {
            Fire();
        }
    }

    private void Fire() {
        currentFireRate = currentGun.fireRate;
        Shoot();
    }

    private void Shoot() {
        PlaySE(currentGun.fire_Sound);
        currentGun.muzzleFlash.Play();
        Debug.Log("발사");
    }

    private void PlaySE(AudioClip _clip) {
        audioSC.clip = _clip;
        audioSC.Play();
    }

}
