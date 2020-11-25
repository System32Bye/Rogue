using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Player : MonoBehaviour
{
    public float moveTime = 0.08f;
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    public float restartLevelDelay = 0.5f;
    public Text foodText;

    public Image damageImage;
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);

    private bool damaged;

    Vector3 lookDirection;

    public static float food;
    //----------------------------------------------------------
    //무기 중복 교체 실행 방지
    public static bool isChangeHand = false;

    //현재 무기와 무기 애니메이션
    public static Transform currentHand;
    public static Animator currentHandAnim;

    //무기 교체 딜레이
    [SerializeField]
    private float changeHandDelayTime;
    //무기 교체 끝난 시점
    [SerializeField]
    private float changeHandEndDelayTime;

    //무기 종류(원/근) 관리
    [SerializeField]
    private Gun[] guns;
    [SerializeField]
    private CloseWeapon[] weapons;
    [SerializeField]
    private CloseWeapon[] axes;

    //관리 쉽게 무기 접근 가능
    private Dictionary<string, Gun> gunDictionary = new Dictionary<string, Gun>();
    private Dictionary<string, CloseWeapon> weaponDictionary = new Dictionary<string, CloseWeapon>();
    private Dictionary<string, CloseWeapon> axeDictionary = new Dictionary<string, CloseWeapon>();

    //현재 무기 타입
    [SerializeField]
    private string currentHandType;

    //필요한 컴포넌트
    [SerializeField]
    private GunController theGunController;
    [SerializeField]
    private WeaponController theWeaponController;
    [SerializeField]
    private AxeController theAxeController;

    [SerializeField]
    private GameObject soda_Text;
    [SerializeField]
    private GameObject food_Text;
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip eat_Item;
    [SerializeField]
    private GameObject bLock;

    [SerializeField]
    private CapsuleCollider PlayercapCol;
    //----------------------------------------------------------
    // Use this for initialization
    public void Start()
    {

        food = GameManager.instance.playerFoodPoints;

        foodText.text = "Time: " + food;

        for (int i = 0; i < guns.Length; i++)
        {
            gunDictionary.Add(guns[i].gunName, guns[i]);
        }
        for (int i = 0; i < weapons.Length; i++)
        {
            weaponDictionary.Add(weapons[i].closeWeaponName, weapons[i]);
        }
        for (int i = 0; i < axes.Length; i++)
        {
            axeDictionary.Add(axes[i].closeWeaponName, axes[i]);
        }
    }

    private void OnDisable()
    {
        GameManager.instance.playerFoodPoints = food;
    }

    // Update is called once per frame
    void Update()
    {
        int horizontal = 0;
        int vertical = 0;

        food -= Time.deltaTime;
        foodText.text = "Time: " + (food).ToString("0");
        if (food < 0f)
        {
            CheckIfGameOver();
        }
        if (!GameManager.isPause)
        {

            if (Input.GetKey(KeyCode.LeftArrow) ||
                Input.GetKey(KeyCode.RightArrow) ||
                Input.GetKey(KeyCode.UpArrow) ||
                Input.GetKey(KeyCode.DownArrow))
            {

                horizontal = (int)Input.GetAxisRaw("Horizontal");
                vertical = (int)Input.GetAxisRaw("Vertical");
                lookDirection = vertical * Vector3.forward + horizontal * Vector3.right;

                this.transform.rotation = Quaternion.LookRotation(lookDirection);
                this.transform.Translate(Vector3.forward * moveTime);
            }
        }

        if (!isChangeHand)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                StartCoroutine(ChangeHandCoroutine("WEAPON", "Weapon"));
                Debug.Log("weapon");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                StartCoroutine(ChangeHandCoroutine("GUN", "Gun"));
                Debug.Log("gun");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                StartCoroutine(ChangeHandCoroutine("AXE", "Axe"));
                Debug.Log("axe");
            }
        }
        //-------------------------------------------------

        if (damaged)
        {
            damageImage.color = flashColour;
        }
        else {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        damaged = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Exit")
        {
            if (GameManager.level != 50)
            {
                Invoke("Restart", restartLevelDelay);

                bLock.SetActive(true);

                enabled = false;
            }
            else if (GameManager.level == 50)
            {
                SceneManager.LoadScene("EndingScene");
            }
        }
        else if (other.tag == "Food")
        {
            food += pointsPerFood;
            foodText.text = "Time: " + food;
            audioSource.clip = eat_Item;
            audioSource.Play();

            bLock.SetActive(false);

            other.gameObject.SetActive(false);
            var clone = Instantiate(food_Text, PlayercapCol.bounds.center, Quaternion.identity);
            Destroy(clone, 1);
        }
        else if (other.tag == "Soda")
        {
            food += pointsPerSoda;
            foodText.text = "Time: " + food;
            audioSource.clip = eat_Item;
            audioSource.Play();

            bLock.SetActive(false);

            other.gameObject.SetActive(false);
            var clone = Instantiate(soda_Text, PlayercapCol.bounds.center, Quaternion.identity);
            Destroy(clone, 1);
        }
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void CheckIfGameOver()
    {
        if (food <= 0f)
            GameManager.instance.GameOver();
    }

    //---------------------------------------------------
    public IEnumerator ChangeHandCoroutine(string _type, string _name)
    {
        isChangeHand = true;
        currentHandAnim.SetTrigger("WeaponOut");

        yield return new WaitForSeconds(changeHandDelayTime);

        CancelPreHandAction();
        HandChange(_type, _name);

        yield return new WaitForSeconds(changeHandEndDelayTime);

        currentHandType = _type;
        isChangeHand = false;
    }

    //무기 취소
    private void CancelPreHandAction()
    {
        switch (currentHandType)
        {
            case "GUN":
                theGunController.CancelReload();
                GunController.isActivate = false;
                break;
            case "WEAPON":
                WeaponController.isActivate = false;
                break;
            case "AXE":
                AxeController.isActivate = false;
                break;
        }
    }

    //교체
    private void HandChange(string _type, string _name)
    {
        if (_type == "GUN")
            theGunController.GunChange(gunDictionary[_name]);
        else if (_type == "WEAPON")
            theWeaponController.CloseWeaponChange(weaponDictionary[_name]);
        else if (_type == "AXE")
            theAxeController.CloseWeaponChange(axeDictionary[_name]);
    }

    public void PlayerHit(float _dmg)
    {
        damaged = true;

        food -= _dmg;
        if (food <= 0)
            CheckIfGameOver();
    }
}
