using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpWeapon : MonoBehaviour
{
    private KeyCode pickupKey = KeyCode.E;
    public GameObject currentWeapon;
    private SwordMechanics _swordMechanics;
    public Transform hand;
    private Animator _anim;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _swordMechanics = GameObject.FindGameObjectWithTag("Player").GetComponent<SwordMechanics>();
    }
    void Update()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        
        if (Physics.Raycast(ray, out hit, 1))
        {
            if ((hit.transform.CompareTag("Weapon") || hit.transform.CompareTag("Spear")) && Input.GetKeyDown(pickupKey))
            {
                if (currentWeapon != null)
                    Destroy(currentWeapon);
                currentWeapon = Instantiate(hit.transform.gameObject, hand.transform.position, hand.transform.rotation, hand);
                currentWeapon.GetComponent<BoxCollider>().enabled = false;
                AnimatorLabelHandle(currentWeapon.tag);
            }
        }
    }
    void AnimatorLabelHandle(string tag)
    {
        if (tag == "Spear")
        {
            _swordMechanics.firstAttackTimer = 1.1f;
            _swordMechanics.maxComboDelay = 1.6f;
            _anim.SetLayerWeight(1, 1);
        }
        else if (tag == "Weapon")
        {
            _swordMechanics.firstAttackTimer = 0.7f;
            _swordMechanics.maxComboDelay = 1.3f;
            _anim.SetLayerWeight(1, 0);
        }
    }
}
