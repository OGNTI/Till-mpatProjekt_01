using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]float speed = 5;
    [SerializeField]float timeBetweenShots = 1;
    float shotTimer;

    [SerializeField] Transform gunBarrelPos;
    [SerializeField] GameObject bulletPrefab;

    Vector3 worldPosition;



    void Update()
    {
        float walkSpeed = speed * Time.deltaTime;
        float rotationSpeed = 10 * Time.deltaTime;

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(moveX, moveY, 0).normalized * walkSpeed;

        transform.position += movement;
        // transform.Translate(movement); 

        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; 
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed);

        // Vector3 mousePos = Input.mousePosition;
        // mousePos.z = Camera.main.nearClipPlane;
        // worldPosition = Camera.main.ScreenToWorldPoint(mousePos);

        // transform.position = Vector2.MoveTowards(transform.position, worldPosition, walkSpeed);

        shotTimer += Time.deltaTime;

        if (Input.GetAxisRaw("Fire1") > 0 && shotTimer > timeBetweenShots)
        {
            Instantiate(bulletPrefab, gunBarrelPos.position, transform.rotation);
            shotTimer = 0;
        }

    }
}
