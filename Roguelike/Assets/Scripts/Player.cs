using System.Collections;
using System.Collections.Generic;
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

    Vector3 lookDirection;
    
    private float food;
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
    private Weapon[] weapons;

    //관리 쉽게 무기 접근 가능
    private Dictionary<string, Gun> gunDictionary = new Dictionary<string, Gun>();
    private Dictionary<string, Weapon> weaponDictionary = new Dictionary<string, Weapon>();
    
    //현재 무기 타입
    [SerializeField]
    private string currentHandType;

    //필요한 컴포넌트
    [SerializeField]
    private GunController theGunController;
    [SerializeField]
    private WeaponController theWeaponController;
    //----------------------------------------------------------
    // Use this for initialization
    public void Start()
    {
        //Player.isChangeHand = true;

        food = GameManager.instance.playerFoodPoints;

        foodText.text = "Time: " + food;

        //------------------------------------------------------
        for (int i = 0; i < guns.Length; i++)
        {
            gunDictionary.Add(guns[i].gunName, guns[i]);
        }
        for (int i = 0; i < weapons.Length; i++)
        {
            weaponDictionary.Add(weapons[i].weaponName, weapons[i]);
        }
        //------------------------------------------------------
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

        //-------------------------------------------------
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
        }
        //-------------------------------------------------
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Exit")
        {
            Invoke("Restart", restartLevelDelay);
            enabled = false;
        }
        else if (other.tag == "Food")
        {
            food += pointsPerFood;
            foodText.text = "Time: " + food + ("+" + pointsPerFood);
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Soda")
        {
            food += pointsPerSoda;
            foodText.text = "Time: " + food + ("+" + pointsPerFood);
            other.gameObject.SetActive(false);
        }
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoseFood(int loss)
    {
        food -= loss;
        foodText.text = "Time: " + (food).ToString("0") + "-" + loss;
        CheckIfGameOver();
    }

    private void CheckIfGameOver()
    {
        if (food <= 0)
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
        }
    }

    private void HandChange(string _type, string _name)
    {
        if (_type == "GUN")
            theGunController.GunChange(gunDictionary[_name]);
        else if (_type == "WEAPON")
            theWeaponController.WeaponChange(weaponDictionary[_name]);
    }
}
