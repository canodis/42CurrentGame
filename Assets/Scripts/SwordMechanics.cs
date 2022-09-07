using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordMechanics : MonoBehaviour
{
    private Animator anim;
    private bool firstAttack;
    private bool secondAttack;
    public bool attack;
    float _time = 0f;
    public GameObject player;
    public float maxComboDelay = 1.3f;
    public float firstAttackTimer;
    private playerMove _playerMove;
    CharacterController _characterController;
    private BoxCollider[] allColliders;
    public BoxCollider swordCollider;

    void Start()
    {
        anim = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
        _playerMove = player.GetComponent<playerMove>();
    }

    void Update()
    {
        attack = false;
        _time += Time.deltaTime; // sayac mantigi
        if (Input.GetMouseButtonDown(0) && !_playerMove.canRoll && !_playerMove.canDash) // eger ki sol mouse'a basildiysa
            OnClick();
        if (_time >= maxComboDelay) // zaman max combo suresini gecti ise attacklar sifirlaniyor.
        {
            firstAttack = false;
            secondAttack = false;
            _playerMove._characterSpeed = 4;
            _time = 0;
        }
        // eger ki ilk vurus yapildiysa ve ikinci vurus daha yapilmadiysa normal hiza geri doner.
        if (!secondAttack && firstAttack && _time >= firstAttackTimer + 0.1f) 
            _playerMove._characterSpeed = 4;
    }

    void OnClick()
    {
        // ilk atack yapilmadiysa ilk attack'i yapar ve sureyi sifirlar ayriyeten playerin hizini yariya dusurur.
        if (!firstAttack) 
        {
            attack = true;
            anim.SetTrigger("Attack1");
            firstAttack = true;
            if (_characterController.isGrounded) // havadayken hizini yavaslatmak istemedigim icin yazdim
                _playerMove._characterSpeed = 1;
            _time = 0;
        }
        // ilk atack yapildiysa ve ilk attack yapildiktan sonra 0.7 saniye gectiyse ikinci attagi yap
        // sureyi 0.8 e cek bu sayede 2. attack son kombo saniyesinde yapilsa dahi direk olarak _time sifirlanmasin.
        if (firstAttack && !secondAttack && _time >= firstAttackTimer)
        {
            anim.SetTrigger("Attack2");
            if (_characterController.isGrounded)
                _playerMove._characterSpeed = 1;
            secondAttack = true;
            _time = firstAttackTimer + 0.1f;
        }
    }
    void collidersetActive() // kilicin collider'ini bulur ve onu aktif eder.
    {
        allColliders = gameObject.GetComponentsInChildren<BoxCollider>();
        foreach (BoxCollider item in allColliders)
        {
            if (item.CompareTag("Weapon") || item.CompareTag("Spear"))
            {
                swordCollider = item;
                break;
            }
        }
        if (swordCollider != null)
            swordCollider.enabled = true;
    }

    void colliderSetDeactive()
    {
        allColliders = gameObject.GetComponentsInChildren<BoxCollider>();
        foreach (BoxCollider item in allColliders)
        {
            if (item.CompareTag("Weapon") || item.CompareTag("Spear"))
            {
                swordCollider = item;
                break;
            }
        }
        if (swordCollider != null)
            swordCollider.enabled = false;

    }
}
