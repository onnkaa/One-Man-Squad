using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//bakma ayarlanacak
//animasyonlar ayarlacak
//öldürme - ölme 
// belli bir mesafede yürüyecek belli bir mesafede saldırı gerçekleştirecek

public class undeadsController : MonoBehaviour
{
    bool dead = false;

    public Animator animator;

    public float hiz, mesafe;
    public bool yurume, saldiri;
    public Transform player;
    Vector3 poz;

    public int health = 100;

    public int alinanHasar;
    public int hasar = 0;
    public float atackTime = 0;

    public GameObject kilic;
    Collider kilicCol;
    //private WalkingOrc dwarf;
    //public GameObject dwarfGetir;

    public Slider m_Slider;
    public Image m_FillImage;
    public Color m_FullHealthColor = Color.green;
    public Color m_ZeroHealthColor = Color.red;
    private float m_CurrentHealth;

    void Start()
    {
        //dwarf = dwarfGetir.GetComponent<WalkingOrc>();
        kilicCol = kilic.GetComponent<Collider>();
    }
    private void OnEnable()
    {

        m_CurrentHealth = health;



        SetHealthUI();
    }



    private void SetHealthUI()
    {

        m_Slider.value = m_CurrentHealth;


        m_FillImage.color = Color.Lerp(m_ZeroHealthColor, m_FullHealthColor, m_CurrentHealth / health);
    }


    void Update()
    {
        if (!dead)
        {



            poz = new Vector3(player.position.x, transform.position.y, player.position.z);
            mesafe = Vector3.Distance(transform.position, player.position);     //undead ile dwarf arasındaki mesafe burda hesaplanacak
            if (mesafe < 20)                                                     //mesafa ayarlanacak ve o mesafeya ulaştıgı zaman bu komt çalışacak
            {

                yurume = true;
                saldiri = false;                            //Drawrf mesafeye girdiği zaman undead onu takip edecek ona doğru bakacak
            }
            if (mesafe <= 2)
            {

                yurume = false;
                saldiri = true;
            }


            if (yurume)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.position, hiz * Time.deltaTime);
                transform.LookAt(poz);


                animator.SetFloat("Speed", 1);
            }
            if (!yurume)
            {

                animator.SetFloat("Speed", 0);
            }

            atackTime += Time.deltaTime;

            if (saldiri && atackTime > 1.0f)
            {
                kilicCol.enabled = !kilicCol.enabled;
                animator.SetTrigger("Attack");

                //hasar += 5;
                atackTime = 0;


            }
            Debug.Log(hasar);

        }
        
        if (health <= 0)
        {
            dead = true;
            animator.SetTrigger("isDead");
        }
    }

    //public IEnumerator selfdestruct()
    //{
    //    animator.SetTrigger("isDead");
    //    GetComponent<Rigidbody>().velocity = Vector3.zero;


    //}


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "bomb" || other.gameObject.tag == "bolt" || other.gameObject.tag == "dwarfbalta")
        {
            alinanHasar = Random.Range(0, 5);
            health = health-alinanHasar;
            //Debug.Log("healt" + health); çalışıyor
            m_CurrentHealth -= alinanHasar;
            //Debug.Log("savascı alınan hasar " + health); 
            SetHealthUI();
        }
    }
}
