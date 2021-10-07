using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectCollision : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private float lastMaxVelocityX = 0;
    private float lastMaxVelocityY = 0;

    // Start is called before the first frame update
    void Start()
    {
        this._rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Math.Abs(lastMaxVelocityX) <= Math.Abs(_rigidbody.velocity.x))
            lastMaxVelocityX = _rigidbody.velocity.x;
        if (Math.Abs(lastMaxVelocityY) <= Math.Abs(_rigidbody.velocity.y))
            lastMaxVelocityY = _rigidbody.velocity.y;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Note")
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
        else
            this._rigidbody.velocity = new Vector3(-lastMaxVelocityX, lastMaxVelocityY, 0);
    }
}
