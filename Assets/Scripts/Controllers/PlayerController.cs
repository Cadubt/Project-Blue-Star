using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerController : BaseController
{
    #region -- VARIAVEIS GLOBAIS --

    public int playerLife;
    public Slider LifeBar;
    public float moveSpeed;
    private Rigidbody rb;
    public BoxCollider col;
    public float jumpForce;
    public LayerMask groundLayers;
    public Vector3 targetPosition;
    public Vector3 lookAtTarget;
    Quaternion playerRot;
    private GameObject Enemy;

    #endregion

    #region ## CICLO ##

    private void Awake()
    {
        Enemy = GameObject.FindWithTag("Enemy");
        playerLife = 150;
        transform.tag = "Player";
        
    }

    // Use this for initialization
    void Start()
    {
        
        moveSpeed = 3.5f;
        jumpForce = 5f;
        rb = GetComponent<Rigidbody>();
        col = GetComponent<BoxCollider>();
        LifeBar.minValue = 0;
        LifeBar.maxValue = playerLife;
        LifeBar.value = playerLife;
    }

    // Update is called once per frame
    void Update()
    {
        //- Iniciando
        if (Input.GetMouseButton(0))
        {
            SetTargetPosition();
        }

        //- Controle de Vida
        LifeControl();

        //- Movimentações - Mouse ou Joystick
        //MoveByMouse();
        MoveByJoystick();

        //- Condição para o pulo e queda (B)
        if ((Input.GetKeyDown(KeyCode.Joystick1Button1) || Input.GetKeyDown(KeyCode.Space)) && IsGrounded())
            Jump();
        if ((Input.GetKeyUp(KeyCode.Joystick1Button1) || Input.GetKeyUp(KeyCode.Space)))
            JumpFall();

        //Verificação de morte
        if (playerLife <= 0)
        {
            playerLife = 0;
            Morte();
        }

        //Chama a ação de ataque (X)
        if (Input.GetKeyDown(KeyCode.Joystick1Button2))
        {
            Atack();
        }
    }

    #endregion

    #region ## AÇÕES ##

    /// <summary>
    /// Movimentação pelo Mouse - Estilo Ragnarok
    /// </summary>
    void MoveByMouse()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, playerRot, moveSpeed * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }


    /// <summary>
    /// Movimentação pelo Joystick - Estilo ToS
    /// </summary>
    void MoveByJoystick()
    {
        float horizontalMove = Input.GetAxis("Horizontal");
        float verticalMove = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalMove, 0, verticalMove);

        transform.Translate(
            moveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime,
            0f,
            moveSpeed * Input.GetAxis("Vertical") * Time.deltaTime
            );

        rb.AddForce(movement * moveSpeed);
    }

    /// <summary>
    /// Metodo que identifica se o "Enemy" colidiu com ele
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Enemy")
        {
            Debug.Log("Colidiu");
        }
    }

    void Morte()
    {
        SceneManager.LoadScene("YouAreDied");
    }

    /// <summary>
    /// Action of Atack Pressing (X)
    /// </summary>
    void Atack()
    {
        AtackAnimarion();

        if (Enemy != null &&  Vector3.Distance(transform.position, Enemy.transform.position) < 1.5f)
        {
            Destroy(Enemy);
        }

    }

    void AtackAnimarion()
    {
        Vector3 um = new Vector3(1.1f, 1, transform.position.z);
        Vector3 dois = new Vector3(1.1f, 1, transform.position.z);
        transform.localScale = Vector3.Scale(um, dois);
        StartCoroutine(ExampleCoroutine());
        
    }

    IEnumerator ExampleCoroutine()
    {
        yield return new WaitForSeconds(0.4f);
        Vector3 um = new Vector3(1f, 1, transform.position.z);
        Vector3 dois = new Vector3(1f, 1, transform.position.z);
        transform.localScale = Vector3.Scale(um, dois);
    }

    /// <summary>
    /// Ação de pulo
    /// </summary>
    void Jump()
    {
        //rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    /// <summary>
    /// Ação de pulo
    /// </summary>
    void JumpFall()
    {
        rb.AddForce(Vector3.down * jumpForce, ForceMode.Impulse);
    }


    /// <summary>
    /// Verifica se o Players esta tocando o Chão
    /// </summary>
    /// <returns></returns>
    private bool IsGrounded()
    {
        return Physics.CheckCapsule(col.bounds.center, new Vector3(col.bounds.center.x, col.bounds.min.y, col.bounds.min.z), groundLayers);
        //return true;
    }
    #endregion

    #region ## EVENTOS ##

    /// <summary>
    /// Seta a posição do Player
    /// </summary>
    void SetTargetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000))
        {
            targetPosition = hit.point;
            lookAtTarget = new Vector3(targetPosition.x - transform.position.x, transform.position.y, targetPosition.z - transform.position.z);
            playerRot = Quaternion.LookRotation(lookAtTarget);
        }
    }


    private void LifeControl()
    {
        if (LifeBar.value >= playerLife)
            LifeBar.value = playerLife;
        if (LifeBar.value <= LifeBar.minValue)
            LifeBar.value = LifeBar.minValue;
    }

    #endregion

}
