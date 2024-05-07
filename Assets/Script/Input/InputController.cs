using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField] PlayerController player = null;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.gameManager.TogglePause();
        }

        if (Time.timeScale != 0)
        {
            PlayerInput();
        }
    }

    void PlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            GameManager.gameManager.SetView(!GameManager.gameManager.GetView());
        }

        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");
        Vector2 moveInput = new Vector2(xInput, yInput);
        player.SetMoveInput(moveInput);

        if (GameManager.gameManager.GetView())
        {
            if (Input.GetButtonDown("Jump"))
            {
                player.SetJumpInput(0, true);
            }
            player.SetJumpInput(1, Input.GetButton("Jump"));
        }

        if (Input.GetMouseButton(1))
        {
            Vector2 mousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
            player.SetMousePosInput(mousePos);
            player.SetLongAttackInput(true);
        }
        else
        {
            player.SetLongAttackInput(false);
        }

        player.SetMeleeAttackInput(Input.GetMouseButton(0));
    }
}
