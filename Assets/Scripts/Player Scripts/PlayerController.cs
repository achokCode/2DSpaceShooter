using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float min_Y, max_Y;

    [SerializeField]
    private GameObject player_Bullet;

    [SerializeField]
    private Transform attack_Point;

    public float attack_Timer = 0.35f;
    private float current_Attack_Timer;
    private bool canAttack;

    private Animator anim;
    private AudioSource explosionSound;

    public AudioSource laserAudio;

    public GameObject gameOverPanel;

    private void Awake()
    {
        laserAudio = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        current_Attack_Timer = attack_Timer;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        Attack();
    }

    void MovePlayer()
    {
        if(Input.GetAxisRaw("Vertical")> 0f)
        {
            Vector3 temp = transform.position;
            temp.y += speed * Time.deltaTime;

            if(temp.y > max_Y)
                temp.y = max_Y;
            
            transform.position = temp;
        }else if (Input.GetAxisRaw("Vertical") < 0f)
        {
            Vector3 temp = transform.position;
            temp.y -= speed * Time.deltaTime;

            if(temp.y < min_Y)
                temp.y = min_Y;
            
            transform.position = temp;
        }
    }

    void Attack()
    {
        attack_Timer += Time.deltaTime;
        if (attack_Timer > current_Attack_Timer)
        {
            canAttack = true;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            if (canAttack)
            {
                canAttack = false;
                attack_Timer = 0f;

                Instantiate(player_Bullet, attack_Point.position, Quaternion.identity);

                laserAudio.Play();


            }
        }
    }

    void TurnOffGameObject()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D target)
    {
        if(target.tag == "Bullet" || target.tag == "Enemy")
        {
            Input.GetAxisRaw("Vertical").Equals(0);

            if (canAttack == false)
            {
                canAttack = true;
                CancelInvoke("Attack");
                CancelInvoke("MovePlayer");
            }

            gameOverPanel.SetActive(true);
            gameObject.SetActive(false);
            Invoke("TurnOffGameObject", 3f);
            Destroy(gameObject);
        }
        
    }
}
