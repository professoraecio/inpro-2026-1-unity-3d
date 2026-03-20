using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class Player : MonoBehaviour
{

    [Header("Movimento")]
    public float velocidade = 6f;
    [Header("Joystick")]
    [Range(0f,0.5f)] public float deadzone = 0.15f;
    public bool usarDpadComoDigital = true;
    private Vector3 direcao;

    private CharacterController controller;

    Animator animator;
    bool isWalk = false;
    public bool ataque = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
        // Ataque via teclado
        if(Keyboard.current != null && Keyboard.current.spaceKey.isPressed)
        {
            ataque = true;
        }              

        float dirX = 0f;
        float dirY = 0f;

        // Teste input teclado
        if(Keyboard.current != null)
        {
            if(Keyboard.current.leftArrowKey.isPressed)
                dirX -= 1f;
            if(Keyboard.current.rightArrowKey.isPressed)
                dirX += 1f;
            if(Keyboard.current.downArrowKey.isPressed)
                dirY -= 1f;
            if(Keyboard.current.upArrowKey.isPressed)
                dirY += 1f;
        }

        // Teste input joystick
        var gp = Gamepad.current;
        if(gp != null)
        {
            Vector2 stick = gp.leftStick.ReadValue();
            if(Math.Abs(stick.x) > deadzone) dirX += stick.x;
            if(Math.Abs(stick.y) > deadzone) dirY += stick.y;

            if(usarDpadComoDigital)
            {
                if(gp.dpad.left.isPressed) dirX -= 1f;
                if(gp.dpad.right.isPressed) dirX += 1f;
                if(gp.dpad.down.isPressed) dirY -= 1f;
                if(gp.dpad.up.isPressed) dirY += 1f;
            }
        }

        dirX = Mathf.Clamp(dirX,-1f,1f);
        dirY = Mathf.Clamp(dirY,-1f,1f);

        // Aplicar movimento independente
        // se veio do joystick ou teclado
        direcao = new Vector3(dirX,0,dirY).normalized;
        // se a velocidade do player não e nula
        if(direcao.magnitude > 0.1f)
        {
            // A partir do ângulo direcionar o seu norte
            // Converti radianos para graus
            float tangetAngle = Mathf.Atan2(direcao.x,direcao.z ) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0,tangetAngle,0);
            isWalk = true;
        }
        else
        {
            isWalk = false;
        }
        controller.Move(direcao * velocidade * Time.deltaTime);
        animator.SetBool("isWalk",isWalk);

        // Ataque via joystick
        if(gp != null)
        {
            bool a = gp.buttonSouth.isPressed ? true : false;
            bool b = gp.buttonEast.isPressed ? true : false;
            bool x = gp.buttonWest.isPressed ? true : false;
            bool y = gp.buttonNorth.isPressed ? true : false;
            if(a || b || x || y)
                ataque = true;
        }

        // Ativar ou não a animação de ataque
        if(ataque)
        {
            animator.SetTrigger("Attack");
            print("Rodar animação de ataque...");
        }
            
        ataque = false;
    }
}
