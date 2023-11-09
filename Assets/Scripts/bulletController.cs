using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletController : MonoBehaviour
{
    [SerializeField] float speed = 12;

    void Update()
    {
        Vector2 movement = new Vector2(speed, 0) * Time.deltaTime;

        transform.Translate(movement);

        if (transform.position.y > Camera.main.orthographicSize + 1)
        {
            GameObject.Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Wall")
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}
