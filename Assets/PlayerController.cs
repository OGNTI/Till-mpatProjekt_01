using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float speed = 5;

    Vector3 worldPosition;

    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        worldPosition = Camera.main.ScreenToWorldPoint(mousePos);

        float step = speed * Time.deltaTime;
        // float moveX = Input.GetAxisRaw("Horizontal");
        // float moveY = Input.GetAxisRaw("Vertical");

        // Vector2 movement = new Vector2(moveX, moveY).normalized * speed * Time.deltaTime;

        // transform.Translate(movement);

        transform.position = Vector2.MoveTowards(transform.position, worldPosition, step);
    }
}
