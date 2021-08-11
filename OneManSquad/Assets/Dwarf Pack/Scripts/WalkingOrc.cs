using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WalkingOrc : MonoBehaviour
{

	private Animator animator;

	public float walkspeed = 5;
	private float horizontal;
	private float vertical;
	//private float rotationDegreePerSecond = 1000;
	private bool isAttacking = false;

	//public GameObject gamecam;
	//public Vector2 camPosition;
	private bool dead;


	public GameObject[] characters;
	public int currentChar = 0;


    public GameObject kafaKamerasi;
    float kafaRotUstAlt = 0, kafaRotSagSol = 0;
    Vector3 kameraArasiMesafe;
    RaycastHit hit;
    
    public int alinanhasar;

    public GameObject balta;
    Collider baltacCol;

    public int health = 100;
    public Slider m_Slider;
    public Image m_FillImage;
    public Color m_FullHealthColor = Color.green;
    public Color m_ZeroHealthColor = Color.red;
    private float m_CurrentHealth;


    void Start()
	{
        baltacCol = balta.GetComponent<Collider>();
        kameraArasiMesafe = kafaKamerasi.transform.position - transform.position;
        setCharacter(0);
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



    void FixedUpdate()
	{
		if (animator && !dead)
		{
            Rotation();
			//walk
			horizontal = Input.GetAxis("Horizontal");
			vertical = Input.GetAxis("Vertical");

			Vector3 stickDirection = new Vector3(horizontal, 0, vertical);
			float speedOut;

			if (stickDirection.sqrMagnitude > 1) stickDirection.Normalize();

			if (!isAttacking)
				speedOut = stickDirection.sqrMagnitude;
			else
				speedOut = 0;

			if (stickDirection != Vector3.zero )
                //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(stickDirection, Vector3.up), rotationDegreePerSecond * Time.deltaTime);
                //GetComponent<Rigidbody>().velocity = transform.forward * speedOut * walkspeed + new Vector3(0, GetComponent<Rigidbody>().velocity.y, 0);

                stickDirection = transform.TransformDirection(stickDirection);
                GetComponent<Rigidbody>().position += stickDirection * Time.fixedDeltaTime * 5;

            animator.SetFloat("Speed", speedOut);

		}
	}

    void Rotation()
    {
        kafaKamerasi.transform.position = transform.position + kameraArasiMesafe;
        kafaRotUstAlt += Input.GetAxis("Mouse Y") * Time.fixedDeltaTime * -150;
        kafaRotSagSol += Input.GetAxis("Mouse X") * Time.fixedDeltaTime * 150;
        kafaRotUstAlt = Mathf.Clamp(kafaRotUstAlt, -20, 20);
        kafaKamerasi.transform.rotation = Quaternion.Euler(kafaRotUstAlt, kafaRotSagSol, transform.eulerAngles.z);

        if (horizontal != 0 || vertical != 0 )
        {
            Physics.Raycast(Vector3.zero, kafaKamerasi.transform.GetChild(0).forward, out hit);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(new Vector3(hit.point.x, 0, hit.point.z)), 0.5f);
            Debug.DrawLine(Vector3.zero, hit.point, Color.red);
        }
    }

    void Update()
	{
		if (!dead)
		{
            // move camera
            //if (gamecam)
            //	gamecam.transform.position = transform.position + new Vector3(0, camPosition.x, -camPosition.y);

            // attack

            

			if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Jump") && !isAttacking)
			{
                baltacCol.enabled = !baltacCol.enabled;
                isAttacking = true;
				//animator.Play("Attack");
				animator.SetTrigger("Attack");
				StartCoroutine(stopAttack(1));
				activateTrails(true);

                
			}

			animator.SetBool("isAttacking", isAttacking);

			//switch character

			if (Input.GetKeyDown("q"))
			{
				setCharacter(-1);
				isAttacking = true;
				StartCoroutine(stopAttack(1f));
			}

			if (Input.GetKeyDown("e"))
			{
				setCharacter(1);
				isAttacking = true;
				StartCoroutine(stopAttack(1f));
			}

			// death
			if (health <= 0)
				StartCoroutine(selfdestruct());
		}

	}

	public IEnumerator stopAttack(float lenght)
	{
		yield return new WaitForSeconds(lenght); // attack lenght
		isAttacking = false;
		activateTrails(false);
	}

	public IEnumerator selfdestruct()
	{
		animator.SetTrigger("isDead");
		GetComponent<Rigidbody>().velocity = Vector3.zero;
		dead = true;

		yield return new WaitForSeconds(1.3f);
		GameObject.FindWithTag("GameController").GetComponent<gameContoller>().resetLevel();
	}

	public void setCharacter(int i)
	{
	
		currentChar += i;

		if (currentChar > characters.Length - 1)
			currentChar = 0;
		if (currentChar < 0)
			currentChar = characters.Length - 1;

		foreach (GameObject child in characters)
		{
			if (child == characters[currentChar])
				child.SetActive(true);
			else
			{
				child.SetActive(false);

				if (child.GetComponent<triggerProjectile>())
					child.GetComponent<triggerProjectile>().clearProjectiles();
			}
		}

		animator = GetComponentInChildren<Animator>();
	}

	public void activateTrails(bool state)
	{
		var tails = GetComponentsInChildren<TrailRenderer>();
		foreach (TrailRenderer tt in tails)
		{
			tt.enabled = state;
		}
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "undeadbolt" || other.gameObject.tag == "undeadkilic")
        {
            alinanhasar = Random.Range(0, 5);
            health = health - alinanhasar;
            m_CurrentHealth -= alinanhasar;
            //Debug.Log("savascı alınan hasar " + health); 
            SetHealthUI();
        }
    }
}
