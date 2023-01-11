using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    private Transform IMA;

    // Start is called before the first frame update
    void Start()
    {
        IMA = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Vector2 pos = IMA.position;
            pos.y += 0.5f;
            IMA.position = pos;

        }
        if (Input.GetKey(KeyCode.S))
        {
            Vector2 pos = IMA.position;
            pos.y -= 0.5f;
            IMA.position = pos;

        }
        if (Input.GetKey(KeyCode.A))
        {
            Vector2 pos = IMA.position;
            pos.x -= 0.5f;
            IMA.position = pos;

        }
        if (Input.GetKey(KeyCode.D))
        {
            Vector2 pos = IMA.position;
            pos.x += 0.5f;
            IMA.position = pos;

        }
    }
}
