using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInpyt : MonoBehaviour
{
    public Vector2 mausPosition { get; private set; }
    public Vector2 playerDirektion { get; private set; }
    public bool recharge { get; private set; }
    private bool firing;

    public event Action Fired = default;

    private void Update()
    {
        mausPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        playerDirektion = new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));
        firing = Input.GetButton("Fire1");
        recharge = Input.GetKeyDown(KeyCode.R);

        if (firing && Fired != null)
            Fired();
    }

}
