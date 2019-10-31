using UnityEngine;
using System.Collections;
public class LocalChess : MonoBehaviour
{
    //四个锚点位置，用于计算棋子落点,绑定到棋盘的四个角处
    public GameObject LeftTop;
    public GameObject RightTop;
    public GameObject LeftBottom;
    public GameObject RightBottom;
    //主摄像机
    public Camera cam;
    //锚点在屏幕上的映射位置
    Vector3 LTPos;
    Vector3 RTPos;
    Vector3 LBPos;
    Vector3 RBPos;
    //当前点选的位置
    Vector3 PointPos;  
    //棋盘网格宽度
    float gridWidth =1;
    //棋盘网格高度
    float gridHeight =1;
    //网格宽和高中较小的一个
    float minGridDis;
    //存储棋盘上所有可以落子的位置
    Vector2[,] chessPos;
    //存储棋盘位置上的落子状态
    int[,] chessState;

    //落子顺序，使用定义的类型turn来表示，也可以用bool但是这样可读性更高
    enum turn {black, white } ;
    turn chessTurn;
    //白棋子的图片
    public Texture2D white;
    //黑棋子的图片
    public Texture2D black;
    //黑子获胜提示图
    public Texture2D blackWin;
    //白子获胜提示图
    public Texture2D whiteWin;
    //暂停图片
    public Texture2D Pause;
    //获胜方，1为黑子，-1为白子
    int winner = 0;
    //是否处于对弈状态
    bool isPlaying = true;
    bool isPause = false;
    bool isEnd = false;
    
    void Start () {
        //生成棋盘，并按照五子棋规则将先手赋予黑棋
        chessPos = new Vector2[15, 15];
        chessState =new int[15,15];
        chessTurn = turn.black;
    } 	void Update () {
        //计算锚点位置
        LTPos = cam.WorldToScreenPoint(LeftTop.transform.position);
        RTPos = cam.WorldToScreenPoint(RightTop.transform.position);
        LBPos = cam.WorldToScreenPoint(LeftBottom.transform.position);
        RBPos = cam.WorldToScreenPoint(RightBottom.transform.position);
        //计算网格宽度
        gridWidth = (RTPos.x - LTPos.x) / 14;
        gridHeight = (LTPos.y - LBPos.y) / 14;
        minGridDis = gridWidth < gridHeight ? gridWidth : gridHeight;
        //计算落子点位置	
        for (int i = 0; i < 15; i++)
        {
            for (int j = 0; j < 15; j++)
            {
                chessPos[i, j] = new Vector2(LBPos.x + gridWidth * i, LBPos.y + gridHeight * j);
            }
        }
        //检测鼠标输入并确定落子状态	
        if (isPlaying && Input.GetMouseButtonDown(0))
        {
            PointPos = Input.mousePosition;
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    //找到最接近鼠标点击位置的落子点，如果空则落子	
                    if (Dis(PointPos, chessPos[i, j]) < minGridDis / 2 && chessState[i,j]==0)
                    {
                        //根据下棋顺序确定落子颜色(判断是否为黑棋顺序，是则为1即黑色，不是则为-1即白色)		
                        chessState[i, j] = chessTurn == turn.black ? 1 : -1;
                        //落子成功，更换下棋顺序	
                        chessTurn = chessTurn == turn.black ? turn.white : turn.black;
                    }
                }
            }
            //调用判断函数，确定是否有获胜方
            int re = Result();
            if (re == 1)
            {
                Debug.Log("黑棋胜");
                winner = 1;
                isPlaying = false;
                isEnd = true;
            }
            else if(re==-1)
            {
                Debug.Log("白棋胜");
                winner = -1;
                isPlaying = false;
                isEnd = true;
            }
        }
        //按下空格,若游戏中为暂停，暂停时为开始，结束则为重新开始	
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isPlaying == true)
            {
                isPause = true;
                isPlaying = false;
            }
            else if (isPause == true)
            {
                isPlaying = true;
                isPause = false;
            }
            else if (isEnd == true)
            {
                for (int i = 0; i < 15; i++)
                {
                    for (int j = 0; j < 15; j++)
                    {
                        chessState[i, j] = 0;
                    }
                }
                isPlaying = true;
                chessTurn = turn.black;
                winner = 0;
            }
            else { }
        }
    }
    //计算平面距离函数	
    float Dis(Vector3 mPos, Vector2 gridPos)
    {		
    return Mathf.Sqrt(Mathf.Pow(mPos.x - gridPos.x, 2)+ Mathf.Pow(mPos.y - gridPos.y, 2));
	}
void OnGUI()
{
    //绘制棋子	
    for (int i=0;i<15;i++)
    {
        for (int j = 0; j < 15; j++)
            {
                if (chessState[i, j] == 1)//落棋位置状态为黑棋时
                {
                    GUI.DrawTexture(new Rect(chessPos[i,j].x-gridWidth/2, Screen.height-chessPos[i,j].y-gridHeight/2, gridWidth,gridHeight),black);
                }
                if (chessState[i, j] == -1)//白棋时
                {
                    GUI.DrawTexture(new Rect(chessPos[i, j].x - gridWidth / 2, Screen.height - chessPos[i, j].y - gridHeight / 2, gridWidth, gridHeight), white);
                }
            }
        }
        //根据暂停状态弹出暂停图片
        if (isPause == true)
            //Rect,前两个变量为图片位置，后两个变量为图片大小
            GUI.DrawTexture(new Rect(Screen.width * 0.25f, Screen.height * 0.25f, Screen.width * 0.5f, Screen.height * 0.6f), Pause);
        //根据获胜状态，弹出相应的胜利图片	
        if (winner ==  1)
            GUI.DrawTexture(new Rect(Screen.width * 0.25f, Screen.height * 0.25f, Screen.width * 0.5f, Screen.height * 0.25f), blackWin);
        if (winner == -1)
            GUI.DrawTexture(new Rect(Screen.width * 0.25f, Screen.height * 0.25f, Screen.width * 0.5f, Screen.height * 0.25f), whiteWin);
    }
    //检测是够获胜的函数，不含黑棋禁手检测	
    int Result()
    {
        //用来返回胜利状态的变量
        int flag = 0;
        //如果当前该白棋落子，标定黑棋刚刚下完一步，此时应该判断黑棋是否获胜
        //因为该程序的判定胜利实在易手之后进行，如果在Update中将result函数的顺序提前则要更改判定条件
        //(即判定当前下棋方是否胜利)
        //原文中冗余判定被注释掉，修改时将其删除
        if (chessTurn == turn.white)
        {
            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 15; j++)
                    //该判定为整体棋盘的判定，所以列数只需要向下判定11列即可，11列之后的落棋点不用判定纵向
                {
                    if (j <= 4)//小于4行的斜线判定右斜线
                    {
                        //横向		
                        if (chessState[i, j] == 1 && chessState[i, j + 1] == 1 && chessState[i, j + 2] == 1 && chessState[i, j + 3] == 1 && chessState[i, j + 4] == 1)
                        {
                            flag = 1;
                            return flag;
                        }
                        //纵向
                        if (chessState[i, j] == 1 && chessState[i + 1, j] == 1 && chessState[i + 2, j] == 1 && chessState[i + 3, j] == 1 && chessState[i + 4, j] == 1)
                        {
                            flag = 1;
                            return flag;
                        }
                        //右斜线
                        if (chessState[i, j] == 1 && chessState[i + 1, j + 1] == 1 && chessState[i + 2, j + 2] == 1 && chessState[i + 3, j + 3] == 1 && chessState[i + 4, j + 4] == 1)
                        {
                            flag = 1;
                            return flag;
                        }				
                       
                    }
                    else if (j > 4 && j < 11)//4行到11行之间的斜线判定左斜线和右斜线
                    {
                        //横向		
                        if (chessState[i, j] == 1 && chessState[i, j + 1] == 1 && chessState[i, j + 2] == 1 && chessState[i, j + 3] == 1 && chessState[i, j + 4] == 1)
                        {
                            flag = 1;
                            return flag;
                        }
                        //纵向		
                        if (chessState[i, j] == 1 && chessState[i + 1, j] == 1 && chessState[i + 2, j] == 1 && chessState[i + 3, j] == 1 && chessState[i + 4, j] == 1)
                        {
                            flag = 1;
                            return flag;
                        }
                        //右斜线		
                        if (chessState[i, j] == 1 && chessState[i + 1, j + 1] == 1 && chessState[i + 2, j + 2] == 1 && chessState[i + 3, j + 3] == 1 && chessState[i + 4, j + 4] == 1)
                        {
                            flag = 1;
                            return flag;
                        }
                        //左斜线	
                        if (chessState[i, j] == 1 && chessState[i + 1, j - 1] == 1 && chessState[i + 2, j - 2] == 1 && chessState[i + 3, j - 3] == 1 && chessState[i + 4, j - 4] == 1)
                        {
                            flag = 1;
                            return flag;
                        }
                    }
                    else//大于11行的斜线判定左斜线
                    {
                        //横向		
                        //
                        if (chessState[i, j] == 1 && chessState[i, j + 1] == 1 && chessState[i, j + 2] == 1 && chessState[i, j + 3] == 1 && chessState[i, j + 4] == 1)
                        {
                            flag = 1;  
                            return flag;                        //
                        }
                        //纵向						
                        if (chessState[i, j] == 1 && chessState[i + 1, j] == 1 && chessState[i + 2, j] == 1 && chessState[i + 3, j] == 1 && chessState[i + 4, j] == 1)
                        {
                            flag = 1;
                            return flag;
                        }
                        //左斜线			
                        if (chessState[i, j] == 1 && chessState[i + 1, j - 1] == 1 && chessState[i + 2, j - 2] == 1 && chessState[i + 3, j - 3] == 1 && chessState[i + 4, j - 4] == 1)
                        {
                            flag = 1;
                            return flag;
                        }
                    }
                }
            }
            for (int i = 11; i < 15; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    //其他判定已经全部完成，只需要判定横向  	
                    if (chessState[i, j] == 1 && chessState[i, j + 1] == 1 && chessState[i, j + 2] == 1 && chessState[i, j + 3] == 1 && chessState[i, j + 4] == 1)
                    {
                        flag = 1;
                        return flag;
                    }  				}
            }
        }
        //如果当前该黑棋落子，标定白棋刚刚下完一步，此时应该判断白棋是否获胜
        //整体判定和黑色一致
        else if(chessTurn == turn.black)
        {
            for (int i = 0; i < 11; i++)
            {
                for (int j = 0; j < 15; j++)
                {					if (j < 4)
                    {
                        //横向		
                        if (chessState[i, j] == -1 && chessState[i, j + 1] == -1 && chessState[i, j + 2] == -1 && chessState[i, j + 3] == -1 && chessState[i, j + 4] == -1)
                        {
                            flag = -1;
                            return flag;
                        }
                        //纵向	
                        if (chessState[i, j] == -1 && chessState[i + 1, j] == -1 && chessState[i + 2, j] == -1 && chessState[i + 3, j] == -1 && chessState[i + 4, j] == -1)
                        {
                            flag = -1;
                            return flag;
                        }                      
                        //右斜线	
                        if (chessState[i, j] == -1 && chessState[i + 1, j + 1] == -1 && chessState[i + 2, j + 2] == -1 && chessState[i + 3, j + 3] == -1 && chessState[i + 4, j + 4] == -1)
                        {
                            flag = -1;
                            return flag;
                        }
                    }
                    else if (j >= 4 && j < 11)
                    {
                        //横向			
                        if (chessState[i, j] == -1 && chessState[i, j + 1] == -1 && chessState[i, j + 2] == -1 && chessState[i, j + 3] == -1 && chessState[i, j + 4] ==- 1)
                        {
                            flag = -1;
                            return flag;
                        }
                        //纵向				
                        if (chessState[i, j] == -1 && chessState[i + 1, j] == -1 && chessState[i + 2, j] == -1 && chessState[i + 3, j] == -1 && chessState[i + 4, j] == -1)
                        {
                            flag = -1;
                            return flag;
                        }
                        //右斜线
                        if (chessState[i, j] == -1 && chessState[i + 1, j + 1] == -1 && chessState[i + 2, j + 2] == -1 && chessState[i + 3, j + 3] == -1 && chessState[i + 4, j + 4] == -1)
                        {
                            flag = -1;
                            return flag;
                        }
                        //左斜线	
                        if (chessState[i, j] == -1 && chessState[i + 1, j - 1] == -1 && chessState[i + 2, j - 2] == -1 && chessState[i + 3, j - 3] == -1 && chessState[i + 4, j - 4] == -1)
                        {
                            flag = -1;
                            return flag;
                        }
                    }
                    else
                    {
                        //横向		
                        //
                        if (chessState[i, j] == -1 && chessState[i, j + 1] ==- 1 && chessState[i, j + 2] == -1 && chessState[i, j + 3] == -1 && chessState[i, j + 4] == -1)
                        //
                        {
                            // 
                            flag = -1;
                            //   
                            return flag;
                            //
                        }
                        //纵向		
                        if (chessState[i, j] == -1 && chessState[i + 1, j] ==- 1 && chessState[i + 2, j] ==- 1 && chessState[i + 3, j] ==- 1 && chessState[i + 4, j] == -1)
                        {
                            flag = -1;
                            return flag;
                        }
                        //左斜线
                        if (chessState[i, j] == -1 && chessState[i + 1, j - 1] == -1 && chessState[i + 2, j - 2] == -1 && chessState[i + 3, j - 3] == -1 && chessState[i + 4, j - 4] == -1)
                        {
                            flag = -1;
                            return flag;
                        }
                    }
                }
            }
            for (int i = 11; i < 15; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    //只需要判断横向  	
                    if (chessState[i, j] == -1 && chessState[i, j + 1] == -1 && chessState[i, j + 2] == -1 && chessState[i, j + 3] == -1 && chessState[i, j + 4] == -1)
                    {
                        flag = -1;
                        return flag;
                    }
                }
            }
        }
        return flag;
    }
}

