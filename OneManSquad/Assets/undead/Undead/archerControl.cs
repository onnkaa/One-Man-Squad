using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class archerControl : MonoBehaviour
{

    bool dead = false;

    public Animator animator;

    public float hiz, mesafe;
    public bool yurume, saldiri;
    public Transform player;
    Vector3 poz;
    public float atackTime = 0;

    public int health = 100;

    public int alınanHasar;
    public int hasar = 0;

    public Slider m_Slider;
    public Image m_FillImage;
    public Color m_FullHealthColor = Color.green;
    public Color m_ZeroHealthColor = Color.red;
    private float m_CurrentHealth;

    void Start()
    {

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
            if (mesafe < 25)                                                     //mesafa ayarlanacak ve o mesafeya ulaştıgı zaman bu komt çalışacak
            {

                yurume = true;
                saldiri = false;                            //Drawrf mesafeye girdiği zaman undead onu takip edecek ona doğru bakacak
            }
            if (mesafe <= 18)
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
                transform.LookAt(poz);
                animator.SetFloat("Speed", 0);
            }

            atackTime += Time.deltaTime;
            Debug.Log(atackTime);

            if (saldiri && atackTime > 2.0f)
            {
                //transform.LookAt(poz);

                animator.SetTrigger("Attack");

                atackTime = 0;
            }
        }
        if (health <= 0)
        {
            dead = true;
            animator.SetTrigger("isDead");
        }

    }

    void OnTriggerEnter(Collider other)
    { 
        if (other.gameObject.tag == "bomb" || other.gameObject.tag == "bolt" || other.gameObject.tag == "dwarfbalta")
        {
            alınanHasar = Random.Range(0, 5);
            health = health - alınanHasar;
            Debug.Log("healt" + health);
            m_CurrentHealth -= alınanHasar;
            //Debug.Log("savascı alınan hasar " + health); 
            SetHealthUI();
        }
    }
}
