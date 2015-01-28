using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]       
public class Controller : MonoBehaviour
{

    public enum DirectionInput
    {
        Null, Left, Right
    }
    public enum Position
    {
        Middle, Left, Right
    }  
    public float speedMove; 
    [HideInInspector]
    public CharacterController characterController;     
    [HideInInspector]                          
    private bool activeInput;                           
    private Vector3 moveDir;
    private Vector2 currentPos;                         
    public bool keyInput;
    public bool touchInput;                             
    private Position positionStand;
    private DirectionInput directInput;                   
    private Vector3 direction;
    private Vector3 currectPosCharacter;                  
    public static Controller instace;                        
    void Start()
    {
        instace = this;
        characterController = this.GetComponent<CharacterController>();
        speedMove = 5;
        Invoke("WaitStart", 0.2f);
    }
    void WaitStart()
    {
        StartCoroutine(UpdateAction());
    }   
    IEnumerator UpdateAction()
    {
        while (GameParameters.gameParameters.Score >= 0)
        {     
                if (keyInput)
                    KeyInput();     
                if (touchInput)
                {
                    DirectionAngleInput();
                }
                CheckLane();
                MoveForward();           
            yield return 0;
        }    
        yield return new WaitForSeconds(2);
    }       
    private void MoveForward()
    {
        speedMove = GameParameters.gameParameters.speed;  
        if (characterController.isGrounded)
        {
            moveDir = Vector3.zero;        
        }               
        moveDir.z = 0;
        moveDir += transform.TransformDirection(Vector3.forward * speedMove);
        moveDir.y -= 20*Time.deltaTime;      
        characterController.Move((moveDir + direction) * Time.deltaTime);
    }                                            
    private bool checkSideCollision;    
    private void CheckLane()
    {
        if (positionStand == Position.Middle)
        {
            if (directInput == DirectionInput.Right)
            {     
                positionStand = Position.Right;   
            }
            else if (directInput == DirectionInput.Left)
            {
                positionStand = Position.Left;
            }
            if (transform.position.x > 0.05f)
            {
                direction = Vector3.Lerp(direction, Vector3.left * 6, Time.deltaTime * 500);
            }
            else if (transform.position.x < -0.05f)
            {
                direction = Vector3.Lerp(direction, Vector3.right * 6, Time.deltaTime * 500);
            }
            else
            {
                direction = Vector3.zero;
                checkSideCollision = false;
                transform.position = Vector3.Lerp(transform.position, new Vector3(0, transform.position.y, transform.position.z), 6 * Time.deltaTime);
            }
        }
        else if (positionStand == Position.Left)
        {
            if (directInput == DirectionInput.Right)
            {        
                positionStand = Position.Middle;
            }
            if (transform.position.x > -1.8f)
            {
                direction = Vector3.Lerp(direction, Vector3.left * 6, Time.deltaTime * 500);
            }
            else
            {
                direction = Vector3.zero;
                checkSideCollision = false;
                transform.position = Vector3.Lerp(transform.position, new Vector3(-1.8f, transform.position.y, transform.position.z), 6 * Time.deltaTime);
            }
        }
        else if (positionStand == Position.Right)
        {
            if (directInput == DirectionInput.Left)
            {
                positionStand = Position.Middle;
            } 
            if (transform.position.x < 1.8f)
            {
                direction = Vector3.Lerp(direction, Vector3.right * 6, Time.deltaTime * 500);
            }
            else
            {
                direction = Vector3.zero;
                checkSideCollision = false;
                transform.position = Vector3.Lerp(transform.position, new Vector3(1.8f, transform.position.y, transform.position.z), 6 * Time.deltaTime);
            }
        }
    }
    private void KeyInput()
    {
        if (Input.anyKeyDown)
        {
            activeInput = true;
        }       
        if (activeInput && checkSideCollision == false)
        {
            if (Input.GetKey(KeyCode.A))
            {
                directInput = DirectionInput.Left;
                activeInput = false;
            }
            else         
                if (Input.GetKey(KeyCode.D))
                {
                    directInput = DirectionInput.Right;
                    activeInput = false;
                }         
        }
        else
        {
            directInput = DirectionInput.Null;
        }            
    }
    private void TouchInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            currentPos = Input.mousePosition;
            activeInput = true;
        }
        if (Input.GetMouseButton(0) && checkSideCollision == false)
        {
            if (activeInput)
            {
                if ((Input.mousePosition.x - currentPos.x) > 40)
                {
                    directInput = DirectionInput.Right;
                    activeInput = false;
                }
                else if ((Input.mousePosition.x - currentPos.x) < -40)
                {
                    directInput = DirectionInput.Left;
                    activeInput = false;
                }
            }
            else
            {
                directInput = DirectionInput.Null;
            }

        }
        if (Input.GetMouseButtonUp(0))
        {
            directInput = DirectionInput.Null;
        }
        currentPos = Input.mousePosition;
    }

    private void DirectionAngleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            currentPos = Input.mousePosition;
            activeInput = true;
        }           
        if (Input.GetMouseButton(0) && checkSideCollision == false)
        {
            if (activeInput)
            {
                float ang = CalculateAngle.GetAngle(currentPos, Input.mousePosition);
                if ((Input.mousePosition.x - currentPos.x) > 20)
                {
                    if (ang < 45 && ang > -45)
                    {
                        directInput = DirectionInput.Right;
                        activeInput = false;
                    }
                }
                else if ((Input.mousePosition.x - currentPos.x) < -20)
                {
                    if (ang < 45 && ang > -45)
                    {
                        directInput = DirectionInput.Left;
                        activeInput = false;
                    }         
                }
                else if ((Input.mousePosition.y - currentPos.y) > 20)
                {
                    if ((Input.mousePosition.x - currentPos.x) > 0)
                    {        
                        if (ang <= 45 && ang >= -45)
                        {
                            directInput = DirectionInput.Right;
                            activeInput = false;
                        }
                    }
                    else if ((Input.mousePosition.x - currentPos.x) < 0)
                    {
                        if (ang >= -45)
                        {
                            directInput = DirectionInput.Left;
                            activeInput = false;
                        }
                    }
                }
                else if ((Input.mousePosition.y - currentPos.y) < -20)
                {
                    if ((Input.mousePosition.x - currentPos.x) > 0)
                    {
                       
                        if (ang >= -45)
                        {
                            directInput = DirectionInput.Right;
                            activeInput = false;
                        }
                    }
                    else if ((Input.mousePosition.x - currentPos.x) < 0)
                    {
                       if (ang <= 45)
                        {
                            directInput = DirectionInput.Left;
                            activeInput = false;
                        }
                    } 
                }
            }
        }          
        if (Input.GetMouseButtonUp(0))
        {
            directInput = DirectionInput.Null;
            activeInput = false;
        }   
    }
}

