using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public int Block_Number { get; set; }
    public bool isLastBlock { get; set; }
    public bool isNameChanged { get; set; }
    public float ex_position { get; set; }

    public static event Action<GameObject> OnBlockClicked;
    public static event Action OnReadyForPlay;

    private bool isGrounded = false;

    [Header("Sprites")]
    [SerializeField] private Sprite[] Default_Icon = null;

    [SerializeField] private Sprite[] Blue_Icos = null;
    [SerializeField] private Sprite[] Green_Icos = null;
    [SerializeField] private Sprite[] Pink_Icos = null;
    [SerializeField] private Sprite[] Purple_Icos = null;
    [SerializeField] private Sprite[] Red_Icos = null;
    [SerializeField] private Sprite[] Yellow_Icos = null;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = Default_Icon[Block_Number];
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<CapsuleCollider>().isTrigger = true;
        isNameChanged = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if (!GameManager.CanPlayerPlay)
        //    return;

        if (Input.GetMouseButtonDown(0) && GameManager.CanPlayerPlay)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.name == gameObject.name)
                {
                    OnBlockClicked?.Invoke(gameObject);
                }
            }
        }

        if (!isGrounded)
            transform.position -= new Vector3(0f, 15f, 0f) * Time.deltaTime; 
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Block") || other.gameObject.CompareTag("Bar"))
        {
            isGrounded = true;
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<CapsuleCollider>().isTrigger = false;
            gameObject.tag = "Block";

            if (isLastBlock)
                OnReadyForPlay?.Invoke();
        }
    }

    public void ReInstantiate()
    {
        GetComponent<SpriteRenderer>().sprite = Default_Icon[Block_Number];
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<CapsuleCollider>().isTrigger = true;

        gameObject.tag = "Untagged";
        isGrounded = false;
        isLastBlock = false;
    }

    public void ChangeIcon(string level)
    {
        if(Block_Number == 0) // Blue
        {
            if (level == "A")
                GetComponent<SpriteRenderer>().sprite = Blue_Icos[0];
            else if (level == "B")
                GetComponent<SpriteRenderer>().sprite = Blue_Icos[1];
            else if (level == "C")
                GetComponent<SpriteRenderer>().sprite = Blue_Icos[2];
            else
                GetComponent<SpriteRenderer>().sprite = Default_Icon[0];
        }
        else if(Block_Number == 1) // Green
        {
            if (level == "A")
                GetComponent<SpriteRenderer>().sprite = Green_Icos[0];
            else if (level == "B")
                GetComponent<SpriteRenderer>().sprite = Green_Icos[1];
            else if (level == "C")
                GetComponent<SpriteRenderer>().sprite = Green_Icos[2];
            else
                GetComponent<SpriteRenderer>().sprite = Default_Icon[1];
        }
        else if (Block_Number == 2) // Pink
        {
            if (level == "A")
                GetComponent<SpriteRenderer>().sprite = Pink_Icos[0];
            else if (level == "B")
                GetComponent<SpriteRenderer>().sprite = Pink_Icos[1];
            else if (level == "C")
                GetComponent<SpriteRenderer>().sprite = Pink_Icos[2];
            else
                GetComponent<SpriteRenderer>().sprite = Default_Icon[2];
        }
        else if (Block_Number == 3) // Purple
        {
            if (level == "A")
                GetComponent<SpriteRenderer>().sprite = Purple_Icos[0];
            else if (level == "B")
                GetComponent<SpriteRenderer>().sprite = Purple_Icos[1];
            else if (level == "C")
                GetComponent<SpriteRenderer>().sprite = Purple_Icos[2];
            else
                GetComponent<SpriteRenderer>().sprite = Default_Icon[3];
        }
        else if (Block_Number == 4) // Red
        {
            if (level == "A")
                GetComponent<SpriteRenderer>().sprite = Red_Icos[0];
            else if (level == "B")
                GetComponent<SpriteRenderer>().sprite = Red_Icos[1];
            else if (level == "C")
                GetComponent<SpriteRenderer>().sprite = Red_Icos[2];
            else
                GetComponent<SpriteRenderer>().sprite = Default_Icon[4];
        }
        else if (Block_Number == 5) // Yellow
        {
            if (level == "A")
                GetComponent<SpriteRenderer>().sprite = Yellow_Icos[0];
            else if (level == "B")
                GetComponent<SpriteRenderer>().sprite = Yellow_Icos[1];
            else if (level == "C")
                GetComponent<SpriteRenderer>().sprite = Yellow_Icos[2];
            else
                GetComponent<SpriteRenderer>().sprite = Default_Icon[5];
        }
    }

}//class
