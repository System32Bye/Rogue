using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

    public string gunName;          //총 이름
    public float range;             //사정 거리
    public float accuracy;          //정확도
    public float fireRate;          //연사 속도
    public float reloadTime;        //재장전 속도

    public int damage;              //총의 데미지

    public int reloadBulletCount;   //총알 장전 개수
    public int currentBulletCount;  //현재 총알 개수

    public Animator anim;

    public ParticleSystem muzzleFlash;

    public AudioClip fire_Sound;
}
