using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class GameObjectUtils
{
    public static List<GameObject> SortByDistnace(this List<GameObject> objects, Vector3 mesureFrom)
    {
        return objects.OrderBy(object_x => Vector3.Distance(object_x.transform.position, mesureFrom)).ToList();
    }
}

public class LevelManager : MonoBehaviour
{
    private int Number_of_Rows = 0;
    private int Number_of_Columns = 0;
    private int A = 0;
    private int B = 0;
    private int C = 0;
    private int K = 0;

    [Header("Game Elements")]
    [SerializeField] private GameObject Block = null;
    [SerializeField] private GameObject BlockHolder = null;

    private int Counter_Row = 0;
    private float timer_instantiate = 0f, timer_Checker;
    private bool is_InstantiateTimer = false, is_CheckerTimer = false;

    private List<GameObject> PlayableBlocks = new List<GameObject>();
    private List<GameObject> TempBlocks = new List<GameObject>();
    private List<GameObject> OrderedBlocks = new List<GameObject>();

    private Dictionary<string, int> BlockDictionary = new Dictionary<string, int>();
    private Dictionary<string, int> Ins_BlockDictionary = new Dictionary<string, int>();

    private List<float> Postion_of_Y = new List<float>();

    // Start is called before the first frame update
    private void Start()
    {
        Number_of_Rows = GameManager.Number_of_Rows;
        Number_of_Columns = GameManager.Number_of_Columns;
        A = GameManager.A;
        B = GameManager.B;
        C = GameManager.C;
        K = GameManager.K;

        GameObject.Find("Bar").transform.localScale = new Vector3(1 + Number_of_Columns * 1.11f, .5f, .2f);
        GameObject.Find("Bar").transform.position = new Vector3(0f, -1-Number_of_Rows/2 * 1.11f, 0f);
        InstantiateBlocks();

        for(int i = 0; i < Number_of_Rows; i++)
            Postion_of_Y.Add(GameObject.Find("Bar").transform.position.y + .25f + 1.11f/2  + i * 2.519833f / 2);
    }

    private void OnEnable()
    {
        BlockManager.OnBlockClicked += HandleWhenBlockClicked;
        BlockManager.OnReadyForPlay += HandleWhenReadyForPlay;
    }

    private void OnDisable()
    {
        BlockManager.OnBlockClicked -= HandleWhenBlockClicked;
        BlockManager.OnReadyForPlay -= HandleWhenReadyForPlay;
    }

    // Update is called once per frame
    void Update()
    {
        Timer_Instantiate();
        Timer_Checker();
    }

    #region TIMERs

    private void Timer_Instantiate()
    {
        if (!is_InstantiateTimer)
            return;

        timer_instantiate -= Time.deltaTime; 

        if(timer_instantiate <= 0)
        {
            is_InstantiateTimer = false;
            InstantiateBlocks();
            timer_instantiate = .5f;
        }
    }

    private void Timer_Checker()
    {
        if (!is_CheckerTimer)
            return;

        timer_Checker -= Time.deltaTime;

        if(timer_Checker <= 0)
        {
            int c = 0;
            foreach (var b in PlayableBlocks)
            {
                if (MathF.Abs(b.transform.position.y - Postion_of_Y[Postion_of_Y.Count - 1]) < .075f)
                    c++;

                if (c == Number_of_Columns)
                {
                    is_CheckerTimer = false;
                    //Debug.Log("Ready");
                    Checker();
                    c = 0;
                }
            }

            timer_Checker = .2f;
        }
    }
    #endregion

    private void InstantiateBlocks()
    {
        float distance = 1.11f / 2;

        for (int i = 0; i < Number_of_Columns; i++)
        {
            int randN = UnityEngine.Random.Range(0, K);
            GameObject block = BlockCreator();
            block.SetActive(true);

            block.GetComponent<BlockManager>().Block_Number = randN;
            block.transform.SetParent(BlockHolder.transform, false);
            PlayableBlocks.Add(block);

            if (i + 1 == Number_of_Columns && Counter_Row + 1 == Number_of_Rows)
                block.GetComponent<BlockManager>().isLastBlock = true;
            else
                block.GetComponent<BlockManager>().isLastBlock = false;

            if (Number_of_Columns % 2 == 0)
            {
                if (i < Number_of_Columns / 2)
                    block.transform.localPosition = new Vector3(-distance - 1.11f * i, BlockHolder.transform.position.y, 0f);
                else
                    block.transform.localPosition = new Vector3(distance + 1.11f * (i - Number_of_Columns / 2), BlockHolder.transform.position.y, 0f);
            }
            else
            {
                if (i <= Number_of_Columns / 2)
                    block.transform.localPosition = new Vector3(-1.11f * i, BlockHolder.transform.position.y, 0f);
                else
                    block.transform.localPosition = new Vector3(1.11f * (i - Number_of_Columns / 2), BlockHolder.transform.position.y, 0f);
            }
        }

        Counter_Row++;
        if (Counter_Row < Number_of_Rows)
        {
            is_InstantiateTimer = true;
            timer_instantiate = .5f;
        }
    }

    private void Checker()
    {
        OrderedBlocks.Clear();

        int counter = 0;
        for(int i = 0; i < Number_of_Rows; i++)
        {
            foreach(GameObject g in PlayableBlocks)
            {
                if (Mathf.Abs(g.transform.position.y - Postion_of_Y[i]) < .1f)
                {
                    TempBlocks.Add(g);
                    g.GetComponent<BlockManager>().ex_position = i;
                }
            }

            TempBlocks = TempBlocks.SortByDistnace(new Vector3(-20f, Postion_of_Y[i], 0f));

            foreach (var g in TempBlocks)
                OrderedBlocks.Add(g);

            TempBlocks.Clear();
        }

        counter = 0;
        foreach (GameObject block in OrderedBlocks)
        {
            block.name = "Block_" + counter.ToString();
            block.GetComponent<BlockManager>().isNameChanged = false;
            counter++;
        }

        counter = 0;
        foreach (GameObject block in OrderedBlocks)
        {
            foreach (GameObject c_block in OrderedBlocks)
            {
                if (Mathf.Abs(block.transform.position.x - c_block.transform.position.x) < 1.55f && Mathf.Abs(block.transform.position.y - c_block.transform.position.y) < 1f)
                    if (block.GetComponent<BlockManager>().Block_Number == c_block.GetComponent<BlockManager>().Block_Number)
                    {
                        if(c_block.GetComponent<BlockManager>().isNameChanged && block.GetComponent<BlockManager>().isNameChanged)
                        {
                            foreach (GameObject g in OrderedBlocks)
                                if (g.name == c_block.name)
                                    g.name = block.name;

                        }
                        else if (c_block.GetComponent<BlockManager>().isNameChanged)
                            block.name = c_block.name;
                        else
                        {
                            c_block.name = block.name;
                            c_block.GetComponent<BlockManager>().isNameChanged = true;
                        }
                    }

                if (Mathf.Abs(block.transform.position.y - c_block.transform.position.y) < 2f && Mathf.Abs(block.transform.position.x - c_block.transform.position.x) < 1f)
                    if (block.GetComponent<BlockManager>().Block_Number == c_block.GetComponent<BlockManager>().Block_Number)
                    {
                        if (c_block.GetComponent<BlockManager>().isNameChanged)
                            block.name = c_block.name;
                        else
                        {
                            c_block.name = block.name;
                            c_block.GetComponent<BlockManager>().isNameChanged = true;
                        }
                    }
            }

            counter++;
        }

        CheckForDeath();
    }

    private void CheckForDeath()
    {
        BlockDictionary.Clear();

        foreach (GameObject g in OrderedBlocks)
        {
            if (!BlockDictionary.ContainsKey(g.name))
            {
                BlockDictionary.Add(g.name, 1);
            }
            else
            {
                int count = 0;
                BlockDictionary.TryGetValue(g.name, out count);
                BlockDictionary.Remove(g.name);
                BlockDictionary.Add(g.name, count + 1);
            }
        }

        int death_counter = 0;
        foreach (KeyValuePair<string, int> entry in BlockDictionary)
        {
            if (entry.Value > 1)
                death_counter++;
        }

        if(death_counter == 0)
        {
            List<int> rand_numbers = new List<int>();

            is_CheckerTimer = false;
            TempBlocks.Clear();
            int n = (int)Mathf.Sqrt(Number_of_Columns * Number_of_Rows);
            Debug.Log(n);

            for(int i = 0; i < n;)
            {
                int rand_n = UnityEngine.Random.Range(0, Number_of_Columns * Number_of_Rows);

                if (!rand_numbers.Contains(rand_n))
                {
                    rand_numbers.Add(rand_n);
                    i++;
                }
            }

            foreach(var i in rand_numbers)
                TempBlocks.Add(PlayableBlocks[i]);

            foreach (var b in TempBlocks)
            {
                b.SetActive(false);
                PlayableBlocks.Remove(b);
            }

            InstantiateNewBlocks();
        }
        else
        {
            ChangeIconOfBlocks();
        }
    }

    private void ChangeIconOfBlocks()
    {
        foreach (KeyValuePair<string, int> entry in BlockDictionary)
        {
            if (entry.Value > A && entry.Value <= B)
            {
                foreach (GameObject b in PlayableBlocks)
                    if (b.name == entry.Key)
                        b.GetComponent<BlockManager>().ChangeIcon("A");
            }
            else if(entry.Value > B && entry.Value <= C)
            {
                foreach (GameObject b in PlayableBlocks)
                    if (b.name == entry.Key)
                        b.GetComponent<BlockManager>().ChangeIcon("B");
            }
            else if(entry.Value > C)
            {
                foreach (GameObject b in PlayableBlocks)
                    if (b.name == entry.Key)
                        b.GetComponent<BlockManager>().ChangeIcon("C");
            }
            else
            {
                foreach (GameObject b in PlayableBlocks)
                    if (b.name == entry.Key)
                        b.GetComponent<BlockManager>().ChangeIcon("");
            }
        }

        GameManager.CanPlayerPlay = true;
    }

    #region HANDLERS

    private void HandleWhenBlockClicked(GameObject block)
    {
        // CHECK IF BLOCK HAS NO PAIR
        int counter = 0;
        foreach (GameObject B in PlayableBlocks)
            if (B.name == block.name)
                counter++;

        if (counter == 1)
            return;

        // ELSE
        GameManager.CanPlayerPlay = false;
        TempBlocks.Clear();

        foreach (GameObject B in PlayableBlocks)
        {
            if (B.name == block.name)
            {
                B.SetActive(false);
                TempBlocks.Add(B);
            }
        }

        foreach (GameObject B in TempBlocks)
            PlayableBlocks.Remove(B);

        InstantiateNewBlocks();
    }

    private void HandleWhenReadyForPlay()
    {
        is_CheckerTimer = true; timer_Checker = .1f;

        foreach (GameObject block in PlayableBlocks)
            block.GetComponent<BlockManager>().isLastBlock = false;
    }

    #endregion

    private void InstantiateNewBlocks()
    {
        foreach(GameObject b in TempBlocks)
            b.name = b.GetComponent<BlockManager>().ex_position.ToString();

        Ins_BlockDictionary.Clear();

        foreach (GameObject g in TempBlocks)
        {
            if (!Ins_BlockDictionary.ContainsKey(g.name))
            {
                Ins_BlockDictionary.Add(g.name, 1);
            }
            else
            {
                int count = 0;
                Ins_BlockDictionary.TryGetValue(g.name, out count);
                Ins_BlockDictionary.Remove(g.name);
                Ins_BlockDictionary.Add(g.name, count + 1);
            }
        }

        int counter = 0;
        foreach (KeyValuePair<string, int> entry in Ins_BlockDictionary)
        {
            foreach (GameObject t in TempBlocks)
                if(entry.Key == t.name)
                    t.GetComponent<BlockManager>().ex_position = counter;

            counter++;
        }

        for (int i = 0; i < TempBlocks.Count; i++)
        {
            int randN = UnityEngine.Random.Range(0, K);
            GameObject temp_block = BlockFounder();

            if(temp_block != null)
            {
                temp_block.SetActive(true);
                temp_block.GetComponent<BlockManager>().Block_Number = randN;
                temp_block.GetComponent<BlockManager>().ReInstantiate();
                temp_block.transform.localPosition = new Vector3(temp_block.transform.position.x, BlockHolder.transform.position.y + (temp_block.GetComponent<BlockManager>().ex_position * 5), 0f);
            }
            else
            {
                temp_block = BlockCreator();
                temp_block.GetComponent<BlockManager>().Block_Number = randN;
                temp_block.transform.SetParent(BlockHolder.transform, false);
                temp_block.transform.localPosition = new Vector3(TempBlocks[i].transform.position.x, BlockHolder.transform.position.y + (TempBlocks[i].transform.position.y + 11) * 1.5f, 0f);
            }

            PlayableBlocks.Add(temp_block);

            if (i == 0)
                temp_block.GetComponent<BlockManager>().isLastBlock = true;
        }

        TempBlocks.Clear();
    }

    private GameObject BlockCreator()
    {
        GameObject block = Instantiate(Block);

        return block;
    }

    private GameObject BlockFounder()
    {

        foreach (Transform t in BlockHolder.transform)
        {
            if (!t.gameObject.activeSelf)
                return t.gameObject;
        }

        return null;
    }

    public void B_ReturnMenu() => UnityEngine.SceneManagement.SceneManager.LoadScene(0);
}
