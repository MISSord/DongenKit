using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell
{
    public class PlayerMovement : MonoBehaviour
    {
        public GameObject[] guns;
        public float speed;
        private Vector2 input;
        private Vector2 mousePos;
        private Animator animator;
        private Rigidbody2D rigidbody;
        private int gunNum;
        void Start()
        {
            animator = GetComponent<Animator>();
            rigidbody = GetComponent<Rigidbody2D>();
            for (int i = 0; i < guns.Length; i++)
            {
                guns[i].transform.position = BaseData.gunLocalPosition;
                guns[i].transform.localScale = BaseData.gunScale;
                guns[i].GetComponent<Gun>().Init();
                guns[i].gameObject.SetActive(false);
            }
            guns[0].SetActive(true);
        }

        void Update()
        {
            guns[gunNum].GetComponent<Gun>().UpdateGunPosture();
            SwitchGun();
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            rigidbody.velocity = input.normalized * speed;
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (mousePos.x > transform.position.x)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
            else
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            }

            if (input != Vector2.zero)
                animator.SetBool("isMoving", true);
            else
                animator.SetBool("isMoving", false);
        }

        void SwitchGun()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                guns[gunNum].SetActive(false);
                if (--gunNum < 0)
                {
                    gunNum = guns.Length - 1;
                }
                guns[gunNum].SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                guns[gunNum].SetActive(false);
                if (++gunNum > guns.Length - 1)
                {
                    gunNum = 0;
                }
                guns[gunNum].SetActive(true);
            }
        }
    }
}