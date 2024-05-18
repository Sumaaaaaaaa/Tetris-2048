using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BuildUp : MonoBehaviour
{
    public int countDown = 1;
    public int _countDown;
    public int removeCountDown = 1;
    public GameObject UIGameObject;
    private TextMeshPro UI;
    public GameObject UIGameOver;
    public int blocknums = 5;
    public float waitTime = 0.2f;

    //产生的俄罗斯方块的所有模式
    private static Vector2Int[][] _blockCompare = new Vector2Int[][]
    {
        new Vector2Int[]{new Vector2Int(0,0), new Vector2Int(0,1), new Vector2Int(1,0), new Vector2Int(1,1) }, /*方块*/

        new Vector2Int[]{new Vector2Int(0,0), new Vector2Int(0,1), new Vector2Int(0,2), new Vector2Int(0,3) }, /*长条_竖*/
        new Vector2Int[]{new Vector2Int(0,0), new Vector2Int(1,0), new Vector2Int(2,0), new Vector2Int(3,0) },/*长条_横*/

        new Vector2Int[]{new Vector2Int(0,0), new Vector2Int(0,-1), new Vector2Int(1,-1),new Vector2Int(2,-1) },/*反L_1*/
        new Vector2Int[]{new Vector2Int(0,0), new Vector2Int(1,0), new Vector2Int(0,-1), new Vector2Int(0,-2) },/*反L_2*/
        new Vector2Int[]{new Vector2Int(0,0), new Vector2Int(1,0), new Vector2Int(2,0), new Vector2Int(2,-1) },/*反L_3*/
        new Vector2Int[]{new Vector2Int(0,0), new Vector2Int(0,-1), new Vector2Int(0,-2), new Vector2Int(-1,-2) },/*反L_4*/

         new Vector2Int[]{new Vector2Int(0,0), new Vector2Int(1,0), new Vector2Int(2,0), new Vector2Int(2,1) },/*正L_1*/
        new Vector2Int[]{new Vector2Int(0,0), new Vector2Int(0,-1), new Vector2Int(0,-2), new Vector2Int(1,-2) },/*正L_2*/
        new Vector2Int[]{new Vector2Int(0,0), new Vector2Int(1,0), new Vector2Int(2,0), new Vector2Int(0,-1) },/*正L_3*/
        new Vector2Int[]{new Vector2Int(0,0), new Vector2Int(1,0), new Vector2Int(1,-1), new Vector2Int(1,-2) },/*正L_4*/

        new Vector2Int[]{new Vector2Int(0,0), new Vector2Int(0,-1), new Vector2Int(-1,-1), new Vector2Int(0,-2) },/*坦克_1*/
        new Vector2Int[]{new Vector2Int(0,0), new Vector2Int(-1,-1), new Vector2Int(0,-1), new Vector2Int(1,-1) },/*坦克_2*/
        new Vector2Int[]{new Vector2Int(0,0), new Vector2Int(0,-1), new Vector2Int(0,-2), new Vector2Int(1,-1) },/*坦克_3*/
        new Vector2Int[]{new Vector2Int(0,0), new Vector2Int(1,0), new Vector2Int(2,0), new Vector2Int(1,-1) },/*坦克_4*/

        new Vector2Int[]{new Vector2Int(0,0), new Vector2Int(0,-1), new Vector2Int(-1,-1), new Vector2Int(-1,-2) },/*正Z_1*/
        new Vector2Int[]{new Vector2Int(0,0), new Vector2Int(1,0), new Vector2Int(1,-1), new Vector2Int(2,-1) },/*正Z_2*/

        new Vector2Int[]{new Vector2Int(0,0), new Vector2Int(1,0), new Vector2Int(1,1), new Vector2Int(2,1) },/*反Z_1*/
        new Vector2Int[]{new Vector2Int(0,0), new Vector2Int(0,-1), new Vector2Int(1,-1), new Vector2Int(1,-2) },/*反Z_1*/
    };
    private enum BlockID : int { O, I1, I2, Lm1, Lm2, Lm3, Lm4, L1, L2, L3, L4, T1, T2, T3, T4, Z1, Z2, Zm1, Zm2 }
    private enum Direction { up, down, left, right }

    private float _delayLength = 3.0f;

    private GameObject[,] _map;
    private bool _gameState = true;
    public bool _gameOver = false;
    private int _score = 0;


    private Color[] _colorTable = new Color[]
    {
    new Color(0.9019607843137255f,0.09803921568627451f,0.29411764705882354f),
    new Color(0.23529411764705882f,0.7058823529411765f,0.29411764705882354f),
    new Color(1.0f,0.8823529411764706f,0.09803921568627451f),
    new Color(0.2627450980392157f,0.38823529411764707f,0.8470588235294118f),
    new Color(0.9607843137254902f,0.5098039215686274f,0.19215686274509805f),
    new Color(0.5686274509803921f,0.11764705882352941f,0.7058823529411765f),
    new Color(0.25882352941176473f,0.8313725490196079f,0.9568627450980393f),
    new Color(0.9411764705882353f,0.19607843137254902f,0.9019607843137255f),
    new Color(0.7490196078431373f,0.9372549019607843f,0.27058823529411763f),
    new Color(0.9803921568627451f,0.7450980392156863f,0.8313725490196079f),
    new Color(0.27450980392156865f,0.6f,0.5647058823529412f),
    new Color(0.8627450980392157f,0.7450980392156863f,1.0f),
    new Color(0.6039215686274509f,0.38823529411764707f,0.1411764705882353f),
    new Color(1.0f,0.9803921568627451f,0.7843137254901961f),
    new Color(0.5019607843137255f,0.0f,0.0f),
    new Color(0.6666666666666666f,1.0f,0.7647058823529411f),
    new Color(0.5019607843137255f,0.5019607843137255f,0.0f),
    new Color(1.0f,0.8470588235294118f,0.6941176470588235f),
    new Color(0.0f,0.0f,0.4588235294117647f),
    new Color(0.6627450980392157f,0.6627450980392157f,0.6627450980392157f),
    new Color(1.0f,1.0f,1.0f),
    new Color(0.0f,0.0f,0.0f),
    };
    private int _colorIndex = -1;

    //循环获取颜色
    private Color GetColor()
    {
        _colorIndex += 1;
        if (_colorIndex >= _colorTable.Length) _colorIndex = 0;
        return _colorTable[_colorIndex];
    }


    // Start is called before the first frame update
    void Start()
    {
        UI = UIGameObject.GetComponent<TextMeshPro>();
        _map = new GameObject[blocknums, blocknums];
        BuildBackGroundDot(GameObject.Find("Circle"));
        GenerationOneGroup();
        _countDown = countDown;
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameOver)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            return;
        }
        if (_gameState)
        {
            StartCoroutine(Main2());/*协程方式！！！！！*/
            return;
        }
    }

    //主要操作
    /*
    IEnumerator Main()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _gameState = false;
            Move(Direction.down);
            yield return new WaitForSeconds(waitTime);
            if (DestoryComplete(Direction.down))
            {
                yield return new WaitForSeconds(waitTime);
                Move(Direction.down);
                yield return new WaitForSeconds(waitTime);
            }
            GenerationOneGroup();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _gameState = false;
            Move(Direction.left);
            yield return new WaitForSeconds(waitTime);
            if (DestoryComplete(Direction.left))
            {
                yield return new WaitForSeconds(waitTime);
                Move(Direction.left);
                yield return new WaitForSeconds(waitTime);
            }
            GenerationOneGroup();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _gameState = false;
            Move(Direction.right);
            yield return new WaitForSeconds(waitTime);
            if (DestoryComplete(Direction.right))
            {
                yield return new WaitForSeconds(waitTime);
                Move(Direction.right);
                yield return new WaitForSeconds(waitTime);
            }
            GenerationOneGroup();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _gameState = false;
            Move(Direction.up);
            yield return new WaitForSeconds(waitTime);
            if (DestoryComplete(Direction.up))
            {
                yield return new WaitForSeconds(waitTime);
                Move(Direction.up);
                yield return new WaitForSeconds(waitTime);
            }
            GenerationOneGroup();
        }
        _gameState = true;
    }
    */
    IEnumerator Main2()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _gameState = false;
            Move(Direction.down);
            yield return new WaitForSeconds(waitTime);
            if (DestoryComplete(Direction.down))
            {
                yield return new WaitForSeconds(waitTime);
                Move(Direction.down);
                yield return new WaitForSeconds(waitTime);
            }
            if (CountDown()) { GenerationOneGroup(); }

        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _gameState = false;
            Move(Direction.left);
            yield return new WaitForSeconds(waitTime);
            if (DestoryComplete(Direction.left))
            {
                yield return new WaitForSeconds(waitTime);
                Move(Direction.left);
                yield return new WaitForSeconds(waitTime);
            }
            if (CountDown()) { GenerationOneGroup(); }
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            _gameState = false;
            Move(Direction.right);
            yield return new WaitForSeconds(waitTime);
            if (DestoryComplete(Direction.right))
            {
                yield return new WaitForSeconds(waitTime);
                Move(Direction.right);
                yield return new WaitForSeconds(waitTime);
            }
            if (CountDown()) { GenerationOneGroup(); }
        }
        else if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            _gameState = false;
            Move(Direction.up);
            yield return new WaitForSeconds(waitTime);
            if (DestoryComplete(Direction.up))
            {
                yield return new WaitForSeconds(waitTime);
                Move(Direction.up);
                yield return new WaitForSeconds(waitTime);
            }
            if (CountDown()) { GenerationOneGroup(); }
        }
        else if(Input.GetKeyDown(KeyCode.Space))
        {
            Skip();
        }
        _gameState = true;
    }

    // 在几个操作后才反会True以触发生成
    private bool CountDown()
    {
        _countDown--;
        Debug.Log(_countDown);
        if (_countDown <= 0)
        {
            _countDown = countDown;
            return true;
        }
        return false;
    }
    // 跳过操作计数直接生成
    private void Skip()
    {
        _countDown = countDown;
        GenerationOneGroup();
    }
    //随机生成一个方块组
    private void GenerationOneGroup()
    {
        //生成用于随机pick的数据List
        List<Vector2Int> emptyPosition = GetAllAblePoint();

        do
        {
            //随机一个位置
            Vector2Int position = emptyPosition[UnityEngine.Random.Range(0, emptyPosition.Count)];
            //在给定的位置中尝试能否生成
            BlockID? blockId = RandomFindBlockAbleToSpawn(position);
            if (blockId is not null/*说明该位置可以生成*/)
            {
                AddGroup((BlockID)blockId, position);
                return;
            }
            /*说明该位置不可以生成*/
            emptyPosition.Remove(position);
        }
        while (emptyPosition.Count > 0);

        // 若List已经没有能够取的位置了，说明无法增加任何的方块组，说明游戏结束
        GameOver();
        return;

    }
    //在一个点位置，随机尝试所有Block是否可以生成，有则返回ID，无则返回null
    private BlockID? RandomFindBlockAbleToSpawn(Vector2Int inputPosition)
    {
        //将所有的ID转为一个列表
        List<int> allSelectionIndex = Enumerable.Range(0, _blockCompare.Length - 1).ToList();
        do
        {
            int count = 0;
            //在ID列表随机一个值
            int randomInt = UnityEngine.Random.Range(0, allSelectionIndex.Count);
            //从数据库中取相对的 Vector2Int[]
            Vector2Int[] blockPositions = _blockCompare[allSelectionIndex[randomInt]];
            //遍历这个Vector2Int[]
            foreach (Vector2Int position in blockPositions)
            {
                //判断这个位置在不在可以判断的_map中
                Vector2Int forCompare = inputPosition + position;
                if (forCompare.x < blocknums &&
                    forCompare.x >= 0 &&
                    forCompare.y < blocknums &&
                    forCompare.y >= 0
                    )//在范围内
                {
                    if (_map[forCompare.x, forCompare.y] is not null)
                    {
                        break;
                    }//但是不为空
                }
                else
                {
                    break;
                }
                count++;
            }
            //遍历结束后，看统计的符合的位置数是否与数据库中的方块数相同，相同则返回，不相同则判断下一个随机值
            if (count == blockPositions.Length)
            {
                return (BlockID)allSelectionIndex[randomInt];
            }
            else
            {
                allSelectionIndex.RemoveAt(randomInt);
            }
        }
        while (allSelectionIndex.Count > 0);
        return null;
    }

    //创建多个相互关联的俄罗斯方块
    private void AddGroup(BlockID blockId, Vector2Int positionInt)
    {
        GameObject[] parents = new GameObject[_blockCompare[(int)blockId].Length];
        int parentsIndex = 0;
        Color color = GetColor();
        foreach (Vector2Int delayValue in _blockCompare[(int)blockId])
        {
            parents[parentsIndex] = AddOne(positionInt + delayValue, color);
            parentsIndex++;
        }
        setParents(parents);
        return;
    }

    //通用创建俄罗斯单个方块
    private GameObject AddOne(Vector2Int positionInt, Color inputColor)
    {
        //视觉层面，使用GameObject实际产生对象
        var position = new Vector2(-_delayLength + _delayLength * 2 / (blocknums - 1) * positionInt.x, -_delayLength + _delayLength * 2 / (blocknums - 1) * positionInt.y);
        GameObject theGameObject = Instantiate(GameObject.Find("Block"), position, Quaternion.Euler(0, 0, 0));
        theGameObject.transform.localScale = Vector3.one * _delayLength * 2 / (blocknums - 1);
        theGameObject.GetComponent<SpriteRenderer>().color = inputColor;
        //系统层面
        _map[positionInt.x, positionInt.y] = theGameObject;
        theGameObject.GetComponent<Block_Data>().intLocation = positionInt;
        return theGameObject;
    }

    //为单个方块设置相关的方块
    private void setParents(GameObject[] gameObjects)
    {
        foreach (GameObject gameObject in gameObjects)
        {
            foreach (GameObject parent in gameObjects)
            {
                if (gameObject != parent)
                {
                    gameObject.GetComponent<Block_Data>().parents.Add(parent);
                }
            }
        }
    }

    //获取所有_map中可用于生成的空位置
    private List<Vector2Int> GetAllAblePoint()
    {
        List<Vector2Int> returnValue = new List<Vector2Int>();
        for (int x = 0; x < blocknums; x++)
            for (int y = 0; y < blocknums; y++)
            {
                if (_map[x, y] is null)
                {
                    returnValue.Add(new Vector2Int(x, y));
                }
            }
        return returnValue;
    }

    //移动<------------------------------------------
    private void Move(Direction direction)
    {
        if (direction == Direction.down)
        {
            for (int y = 0; y < blocknums; y++)
                for (int x = 0; x < blocknums; x++)
                {
                    if (_map[x, y] is not null)
                    {
                        Move_OneGroup(new Vector2Int(x, y), direction);
                    }
                }
        }
        else if (direction == Direction.up)
        {
            for (int y = blocknums - 1; y >= 0; y--)
                for (int x = 0; x < blocknums; x++)
                {
                    if (_map[x, y] is not null)
                    {
                        Move_OneGroup(new Vector2Int(x, y), direction);
                    }
                }
        }
        else if (direction == Direction.left)
        {
            for (int x = 0; x < blocknums; x++)
                for (int y = 0; y < blocknums; y++)
                {
                    if (_map[x, y] is not null)
                    {
                        Move_OneGroup(new Vector2Int(x, y), direction);
                    }
                }
        }
        else if (direction == Direction.right)
        {
            for (int x = blocknums - 1; x >= 0; x--)
                for (int y = 0; y < blocknums; y++)
                {
                    if (_map[x, y] is not null)
                    {
                        Move_OneGroup(new Vector2Int(x, y), direction);
                    }
                }
        }
        return;
    }

    //以组移动
    private void Move_OneGroup(Vector2Int mapIndex, Direction direction)
    {
        int step = judgeMovestep(_map[mapIndex.x, mapIndex.y], direction);
        if (step == 0)
        {
            return;
        }
        List<GameObject> gameObjects = new List<GameObject>();
        List<Vector2Int> positions = new List<Vector2Int>();

        gameObjects.Add(_map[mapIndex.x, mapIndex.y]);
        positions.Add(new Vector2Int(mapIndex.x, mapIndex.y));
        foreach (GameObject parent in _map[mapIndex.x, mapIndex.y].GetComponent<Block_Data>().parents)
        {
            gameObjects.Add(parent);
            positions.Add(parent.GetComponent<Block_Data>().intLocation);
        }

        //主体_map擦除
        foreach (Vector2Int position in positions)
        {
            _map[position.x, position.y] = null;
        }

        //修改Block_Data中的数据 和 _map 中的数据
        for (int i = 0; i < gameObjects.Count; i++)
        {
            if (direction == Direction.left)
            {
                gameObjects[i].GetComponent<Block_Data>().intLocation = positions[i] + new Vector2Int(-step, 0);
                Move_Visual(gameObjects[i], new Vector2Int(-step, 0));
            }
            else if (direction == Direction.right)
            {
                gameObjects[i].GetComponent<Block_Data>().intLocation = positions[i] + new Vector2Int(step, 0);
                Move_Visual(gameObjects[i], new Vector2Int(step, 0));
            }
            else if (direction == Direction.up)
            {
                gameObjects[i].GetComponent<Block_Data>().intLocation = positions[i] + new Vector2Int(0, step);
                Move_Visual(gameObjects[i], new Vector2Int(0, step));
            }
            else if (direction == Direction.down)
            {
                gameObjects[i].GetComponent<Block_Data>().intLocation = positions[i] + new Vector2Int(0, -step);
                Move_Visual(gameObjects[i], new Vector2Int(0, -step));
            }
            _map[gameObjects[i].GetComponent<Block_Data>().intLocation.x,
                gameObjects[i].GetComponent<Block_Data>().intLocation.y]
                = gameObjects[i];
        }
    }

    //视觉移动
    private void Move_Visual(GameObject gameObject, Vector2Int transValue)
    {
        /*
          gameObject.transform.Translate(transValue.x * _delayLength*2/(blocknums-1)
            , transValue.y * _delayLength * 2 / (blocknums - 1)
            , 0);
        */
        //新版本
        gameObject.GetComponent<Move>().move(
            new Vector2(
            transValue.x * _delayLength * 2 / (blocknums - 1)
            , transValue.y * _delayLength * 2 / (blocknums - 1)
            )
            , waitTime);
    }

    //判断输入的GameObject可以移动几个位置
    private int judgeMovestep(GameObject _block, Direction direction)
    {
        List<int> spaceNum = new List<int>();
        List<GameObject> gameObjects = new List<GameObject>();

        gameObjects.Add(_block);
        foreach (GameObject relationedAllblock in _block.GetComponent<Block_Data>().parents)
        {
            gameObjects.Add(relationedAllblock);
        }
        foreach (GameObject gameObject in gameObjects)
        {
            Tuple<int, GameObject> result = judgeMoveStep_one(gameObject, direction);
            if (!gameObjects.Contains(result.Item2))
            {
                spaceNum.Add(result.Item1);
            }
        }

        return spaceNum.Min();
    }

    //对一个方块进行判断，目标方向的空位
    private Tuple<int, GameObject> judgeMoveStep_one(GameObject _block, Direction direction)
    {
        Vector2Int intPostion = _block.GetComponent<Block_Data>().intLocation;
        int count = 0;
        if (direction == Direction.down)
        {
            int x = intPostion.x;
            for (int y = intPostion.y - 1; y >= 0; y--)
            {
                if (_map[x, y] is null)
                {
                    count++;
                }
                else
                {
                    return Tuple.Create<int, GameObject>(count, _map[x, y]);
                }
            }
            return Tuple.Create<int, GameObject>(count, null);
        }

        if (direction == Direction.up)
        {
            int x = intPostion.x;
            for (int y = intPostion.y + 1; y < blocknums; y++)
            {
                if (_map[x, y] is null)
                {
                    count++;
                }
                else
                {
                    return Tuple.Create<int, GameObject>(count, _map[x, y]);
                }
            }
            return Tuple.Create<int, GameObject>(count, null);
        }
        if (direction == Direction.left)
        {
            int y = intPostion.y;
            for (int x = intPostion.x - 1; x >= 0; x--)
            {
                if (_map[x, y] is null)
                {
                    count++;
                }
                else
                {
                    return Tuple.Create<int, GameObject>(count, _map[x, y]);
                }
            }
            return Tuple.Create<int, GameObject>(count, null);
        }

        if (direction == Direction.right)
        {
            int y = intPostion.y;
            for (int x = intPostion.x + 1; x < blocknums; x++)
            {
                if (_map[x, y] is null)
                {
                    count++;
                }
                else
                {
                    return Tuple.Create<int, GameObject>(count, _map[x, y]);
                }
            }
            return Tuple.Create<int, GameObject>(count, null);

        }
        Debug.LogWarning("ERROR : direction is not get right value!");
        return Tuple.Create<int, GameObject>(count, null);
    }

    //摧毁方块
    private bool DestoryComplete(Direction direction)
    {
        Stack<GameObject> blocks = judgeComplete(direction);
        if (blocks.Count > 0)
        {
            DestroyBlock(blocks);
            return true;
        }
        return false;
    }

    //从给定的一个方向上，该方向情况下，底部是否已经有堆满的方块，有则返回所有满足条件方块的GameObject的Stack，没有则为Null
    private Stack<GameObject> judgeComplete(Direction direction)
    {
        Stack<GameObject> _returnValue = new Stack<GameObject>();
        switch (direction)
        {
            case Direction.up:
                for (int y = blocknums - 1; y >= 0; y--)
                {
                    bool hadNull = false;
                    for (int x = 0; x < blocknums; x++)
                    {
                        if (_map[x, y] is null)
                        {
                            hadNull = true;
                            break;
                        }
                    }
                    if (hadNull)
                    {
                        return _returnValue;
                    }
                    for (int x = 0; x < blocknums; x++)
                    {
                        _returnValue.Push(_map[x, y]);
                    }
                }
                return _returnValue;
            case Direction.down:
                for (int y = 0; y < blocknums; y++)
                {
                    bool hadNull = false;
                    for (int x = 0; x < blocknums; x++)
                    {
                        if (_map[x, y] is null)
                        {
                            hadNull = true;
                            break;
                        }
                    }
                    if (hadNull)
                    {
                        return _returnValue;
                    }
                    for (int x = 0; x < blocknums; x++)
                    {
                        _returnValue.Push(_map[x, y]);
                    }
                }
                return _returnValue;
            case Direction.left:
                for (int x = 0; x < blocknums; x++)
                {
                    bool hadNull = false;
                    for (int y = 0; y < blocknums; y++)
                    {
                        if (_map[x, y] is null)
                        {
                            hadNull = true;
                            break;
                        }
                    }
                    if (hadNull)
                    {
                        return _returnValue;
                    }
                    for (int y = 0; y < blocknums; y++)
                    {
                        _returnValue.Push(_map[x, y]);
                    }
                }
                return _returnValue;
            case Direction.right:
                for (int x = blocknums - 1; x > 0; x--)
                {
                    bool hadNull = false;
                    for (int y = 0; y < blocknums; y++)
                    {
                        if (_map[x, y] is null)
                        {
                            hadNull = true;
                            break;
                        }
                    }
                    if (hadNull)
                    {
                        return _returnValue;
                    }
                    for (int y = 0; y < blocknums; y++)
                    {
                        _returnValue.Push(_map[x, y]);
                    }
                }
                return _returnValue;
        }
        return null;
    }

    //摧毁方块
    private void DestroyBlock(Stack<GameObject> Blocks)
    {
        Scored(Blocks);
        foreach (GameObject gameObject in Blocks)
        {
            _map[gameObject.GetComponent<Block_Data>().intLocation.x,
                gameObject.GetComponent<Block_Data>().intLocation.y]
                = null;
            //去除关系
            foreach (GameObject parent in gameObject.GetComponent<Block_Data>().parents)
            {
                parent.GetComponent<Block_Data>().parents.Remove(gameObject);
            }
            Destroy(gameObject);
        }

    }
    //Debug.Log打印测试内部数据
    public void Print()
    {
        StringBuilder returnValue = new StringBuilder("\n");
        for (int y = blocknums - 1; y >= 0; y--)
        {
            for (int x = 0; x < blocknums; x++)
            {
                if (_map[x, y] is not null)
                {
                    returnValue.Append("■");
                }
                else
                {
                    returnValue.Append("□");
                }

            }
            returnValue.Append("\n");
        }

        Debug.Log(returnValue);
    }

    private void Scored(Stack<GameObject> gameObjects)
    {
        _score += gameObjects.Count;
        UI.text = "Score : " + _score;
    }
    private void GameOver()
    {
        Debug.Log("GAMEOVER");
        _gameOver = true;
        UIGameOver.active = true;
    }

    //创建背景小点点
    private void BuildBackGroundDot(GameObject BackGroundDot)
    {
        Debug.Log("START");
        for (int x = 0; x < blocknums; x++)
            for (int y = 0; y < blocknums; y++)
            {
                Instantiate(BackGroundDot,
                    new Vector2(-_delayLength + _delayLength * 2 / (blocknums - 1) * x, _delayLength - _delayLength * 2 / (blocknums - 1) * y),
                    Quaternion.Euler(0, 0, 0));
            }
        Destroy(BackGroundDot);
    }
}