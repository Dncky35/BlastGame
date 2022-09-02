using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("UI ELEMENTS")]
    [SerializeField] private GameObject Label_Row = null;
    [SerializeField] private GameObject Label_Column = null;
    [SerializeField] private GameObject Label_K = null;
    [SerializeField] private GameObject Label_A = null;
    [SerializeField] private GameObject Label_B = null;
    [SerializeField] private GameObject Label_C = null;

    private void Awake()
    {
        GameManager.Number_of_Rows = 2;
        GameManager.Number_of_Columns = 2;
        GameManager.K = 1;
        GameManager.A = 1;
        GameManager.B = 2;
        GameManager.C = 3;
    }

    void Start()
    {
        Update_UI();

        Label_Row.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(B_Increase_Row);
        Label_Row.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(B_Decrease_Row);
        Label_Row.transform.GetChild(2).GetComponent<Button>().interactable = false;

        Label_Column.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(B_Increase_Col);
        Label_Column.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(B_Decrease_Col);
        Label_Column.transform.GetChild(2).GetComponent<Button>().interactable = false;

        Label_K.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(B_Increase_K);
        Label_K.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(B_Decrease_K);
        Label_K.transform.GetChild(2).GetComponent<Button>().interactable = false;

        Label_A.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(B_Increase_A);
        Label_A.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(B_Decrease_A);
        Label_A.transform.GetChild(1).GetComponent<Button>().interactable = false;
        Label_A.transform.GetChild(2).GetComponent<Button>().interactable = false;

        Label_B.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(B_Increase_B);
        Label_B.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(B_Decrease_B);
        Label_B.transform.GetChild(1).GetComponent<Button>().interactable = false;
        Label_B.transform.GetChild(2).GetComponent<Button>().interactable = false;

        Label_C.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(B_Increase_C);
        Label_C.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(B_Decrease_C);
        Label_C.transform.GetChild(2).GetComponent<Button>().interactable = false;
    }

    void Update()
    {
        
    }

    private void Update_UI()
    {
        Label_Row.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Row = " + GameManager.Number_of_Rows.ToString();
        Label_Column.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Column = " + GameManager.Number_of_Columns.ToString();
        Label_K.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "K = " + GameManager.K.ToString();
        Label_A.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "A = " + GameManager.A.ToString();
        Label_B.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "B = " + GameManager.B.ToString();
        Label_C.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "C = " + GameManager.C.ToString();
    }

    #region BUTTONS 

    public void B_Start_Level_1()
    {
        GameManager.Number_of_Rows = 10;
        GameManager.Number_of_Columns = 12;
        GameManager.K = 6;
        GameManager.A = 4;
        GameManager.B = 7;
        GameManager.C = 9;

        SceneManager.LoadScene(1);
    }

    public void B_Start_Level_2()
    {
        GameManager.Number_of_Rows = 5;
        GameManager.Number_of_Columns = 8;
        GameManager.K = 4;
        GameManager.A = 4;
        GameManager.B = 6;
        GameManager.C = 8;

        SceneManager.LoadScene(1);
    }

    // PRIVATE LEVEL

    private void B_Increase_Row()
    {
        Label_Row.transform.GetChild(2).GetComponent<Button>().interactable = true;
        GameManager.Number_of_Rows++;

        if(GameManager.Number_of_Rows >= 12)
            Label_Row.transform.GetChild(1).GetComponent<Button>().interactable = false;

        Update_UI();
    }

    private void B_Decrease_Row()
    {
        Label_Row.transform.GetChild(1).GetComponent<Button>().interactable = true;
        GameManager.Number_of_Rows--;

        if (GameManager.Number_of_Rows <= 2)
            Label_Row.transform.GetChild(2).GetComponent<Button>().interactable = false;

        Update_UI();
    }

    private void B_Increase_Col()
    {
        Label_Column.transform.GetChild(2).GetComponent<Button>().interactable = true;
        GameManager.Number_of_Columns++;

        if (GameManager.Number_of_Columns >= 12)
            Label_Column.transform.GetChild(1).GetComponent<Button>().interactable = false;

        Update_UI();
    }

    private void B_Decrease_Col()
    {
        Label_Column.transform.GetChild(1).GetComponent<Button>().interactable = true;
        GameManager.Number_of_Columns--;

        if (GameManager.Number_of_Columns <= 2)
            Label_Column.transform.GetChild(2).GetComponent<Button>().interactable = false;

        Update_UI();
    }

    private void B_Increase_K()
    {
        Label_K.transform.GetChild(2).GetComponent<Button>().interactable = true;
        GameManager.K++;

        if (GameManager.K >= 6)
            Label_K.transform.GetChild(1).GetComponent<Button>().interactable = false;

        Update_UI();
    }

    private void B_Decrease_K()
    {
        Label_K.transform.GetChild(1).GetComponent<Button>().interactable = true;
        GameManager.K--;

        if (GameManager.K <= 1)
            Label_K.transform.GetChild(2).GetComponent<Button>().interactable = false;

        Update_UI();
    }

    private void B_Increase_A()
    {
        GameManager.A++;
        Label_A.transform.GetChild(2).GetComponent<Button>().interactable = true;

        if (GameManager.A >= GameManager.B - 1)
        {
            Label_A.transform.GetChild(1).GetComponent<Button>().interactable = false;
            Label_B.transform.GetChild(2).GetComponent<Button>().interactable = false;
        }

        Update_UI();
    }

    private void B_Decrease_A()
    {
        GameManager.A--;
        Label_B.transform.GetChild(2).GetComponent<Button>().interactable = true;

        if (GameManager.A == 1)
        {
            Label_A.transform.GetChild(2).GetComponent<Button>().interactable = false;
        }

        Update_UI();
    }

    private void B_Increase_B()
    {
        Label_A.transform.GetChild(1).GetComponent<Button>().interactable = true;
        Label_B.transform.GetChild(2).GetComponent<Button>().interactable = true;
        GameManager.B++;

        if (GameManager.B >= GameManager.C - 1)
        {
            Label_B.transform.GetChild(1).GetComponent<Button>().interactable = false;
            Label_C.transform.GetChild(2).GetComponent<Button>().interactable = false;
        }

        Update_UI();
    }

    private void B_Decrease_B()
    {
        GameManager.B--;
        Label_C.transform.GetChild(2).GetComponent<Button>().interactable = true;
        Label_B.transform.GetChild(1).GetComponent<Button>().interactable = true;

        if (GameManager.B == GameManager.A + 1)
        {
            Label_B.transform.GetChild(2).GetComponent<Button>().interactable = false;
            Label_A.transform.GetChild(1).GetComponent<Button>().interactable = false;
        }

        Update_UI();
    }

    private void B_Increase_C()
    {
        Label_B.transform.GetChild(1).GetComponent<Button>().interactable = true;
        Label_C.transform.GetChild(2).GetComponent<Button>().interactable = true;
        GameManager.C++;

        if(GameManager.C >= 12)
            Label_C.transform.GetChild(1).GetComponent<Button>().interactable = false;

        Update_UI();
    }

    private void B_Decrease_C()
    {
        GameManager.C--;

        if (GameManager.C == GameManager.B + 1)
        {
            Label_C.transform.GetChild(2).GetComponent<Button>().interactable = false;
            Label_B.transform.GetChild(1).GetComponent<Button>().interactable = false;
        }

        Update_UI();
    }

    public void B_Start_Level_Private()
    {
        SceneManager.LoadScene(1);
    }
    #endregion
}
