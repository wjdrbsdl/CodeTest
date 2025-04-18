﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static GraphSample;

public class GraphSample
{
    #region 그래프표현 인접행렬방식
    public void Test()
    {
        int[,] aa = new int[7, 7];
        int[][] bb = new int[2][];

        aa[3, 5] = 4;
        aa[5, 3] = 4;

        aa[5, 2] = 6;
        aa[2, 5] = 6;

        aa[2, 6] = 3;
        aa[6, 2] = 3;

        aa[1, 2] = 2;
        aa[2, 1] = 2;

        aa[1, 3] = 2;
        aa[3, 1] = 2;

        aa[3, 4] = 1;
        aa[4, 3] = 1;

        aa[0, 1] = 1;
        aa[1, 0] = 1;
    }
    #endregion

    #region 인접리스트방식
    struct Edge
    {
        public int Next { get; init; }
        public int Weight { get; init; }

        public Edge(int next, int weight)
        {
            Next = next;
            Weight = weight;
        }
    }
    public void CloseList()
    {
        List<Edge>[] edgeList = new List<Edge>[7];
        edgeList[0].Add(new Edge(1, 1)); //1번 정점에 1 가중치

        edgeList[1].Add(new Edge(0, 1));
        edgeList[1].Add(new Edge(2, 2));
        edgeList[1].Add(new Edge(3, 2));

        edgeList[2].Add(new Edge(1, 2));
        edgeList[2].Add(new Edge(5, 6));
        edgeList[2].Add(new Edge(6, 3));

        edgeList[3].Add(new Edge(1, 2));
        edgeList[3].Add(new Edge(4, 1));
        edgeList[3].Add(new Edge(5, 4));

        edgeList[4].Add(new Edge(3, 1));

        edgeList[5].Add(new Edge(3, 4));
        edgeList[5].Add(new Edge(2, 6));

        edgeList[6].Add(new Edge(2, 3));
    }

    static public List<int>[] SimpleCloseList()
    {
        List<int>[] neighbors = new List<int>[7];
        for (int i = 0; i < neighbors.Length; i++)
        {
            neighbors[i] = new List<int>();
        }
        neighbors[0].Add(1);

        neighbors[1].Add(0);
        neighbors[1].Add(2);
        neighbors[1].Add(3);

        neighbors[2].Add(1);
        neighbors[2].Add(5);
        neighbors[2].Add(6);

        neighbors[3].Add(1);
        neighbors[3].Add(4);
        neighbors[3].Add(5);

        neighbors[4].Add(3);

        neighbors[5].Add(2);
        neighbors[5].Add(3);

        neighbors[6].Add(2);

        return neighbors;
    }
    #endregion

    #region 깊이우선탐색

    static void DeepFirstSearch(List<int>[] neighborList)
    {
        //1.정점의 방문했는지 여부를 기록할 집합을 만들어야함
        bool[] isVisited = new bool[neighborList.Length];

        //2.스택(dfs) 이나 큐(bfs)를 생성
        Stack<int> nextNodeSt = new(); //nodeGraph의 정점을 int로 받았으므로 int 스택으로

        //3. 최초 정점 넣기
        nextNodeSt.Push(0);

        //4. 순회 시작
        while (nextNodeSt.Count > 0)
        {
            //4-1.방문할집 찾음
            int nextNodeNum = nextNodeSt.Pop();

            if (isVisited[nextNodeNum] == true)
            {
                continue;
            }
            //Console.WriteLine(nextNodeNum +"에 방문");
            Console.Write($"{nextNodeNum} ->");
            //4-2. 방문여부 본다
            isVisited[nextNodeNum] = true;

            //4-3. 방문장소 넣는다.
            List<int> visitList = neighborList[nextNodeNum];
            for (int i = visitList.Count - 1; i >= 0; i--)
            {
                //4-4. 방문하지 않았던것만
                if (isVisited[visitList[i]] == false)
                {
                    nextNodeSt.Push(visitList[i]);
                }

            }
        }
    }
    #endregion

    #region 깊이우선탐색

    static void BroadFirstSearch(List<int>[] neighborList)
    {
        //1.정점의 방문했는지 여부를 기록할 집합을 만들어야함
        bool[] isVisited = new bool[neighborList.Length];

        //2.스택(dfs) 이나 큐(bfs)를 생성
        Queue<int> nextNodeSt = new(); //nodeGraph의 정점을 int로 받았으므로 int 스택으로

        //3. 최초 정점 넣기
        nextNodeSt.Enqueue(0);

        //4. 순회 시작
        while (nextNodeSt.Count > 0)
        {
            //4-1.방문할집 찾음
            int nextNodeNum = nextNodeSt.Dequeue();

            if (isVisited[nextNodeNum] == true)
            {
                continue;
            }
            //Console.WriteLine(nextNodeNum +"에 방문");
            Console.Write($"{nextNodeNum} ->");
            //4-2. 방문여부 본다
            isVisited[nextNodeNum] = true;

            //4-3. 방문장소 넣는다.
            List<int> visitList = neighborList[nextNodeNum];
            for (int i = 0; i < visitList.Count; i++)
            {
                //4-4. 방문하지 않았던것만
                if (isVisited[visitList[i]] == false)
                {
                    nextNodeSt.Enqueue(visitList[i]);
                }

            }
        }
    }
    #endregion
    static List<int>[] MakeGraph(int _nodeCount, int _lineCount)
    {
        List<int>[] neighbors = new List<int>[_nodeCount + 1];
        for (int i = 0; i < neighbors.Length; i++)
        {
            neighbors[i] = new List<int>();
        }

        for (int i = 0; i < _lineCount; i++)
        {
            string[] lineStr = Console.ReadLine().Split();
            int from = int.Parse(lineStr[0]);
            int to = int.Parse(lineStr[1]);
            neighbors[from].Add(to);
            neighbors[to].Add(from);
        }

        for (int i = 0; i < neighbors.Length; i++)
        {
            neighbors[i].Sort();
        }
        return neighbors;
    }

    #region 단지구역2667
    class StringNode
    {
        public int total = 0;
    }

    static void MakeArrayGraph()
    {
        int lineCount = int.Parse(Console.ReadLine());

        string[] graphStr = new string[lineCount];
        for (int i = 0; i < lineCount; i++)
        {
            graphStr[i] = Console.ReadLine();
        }

        int[,] graph = new int[lineCount, lineCount];
        List<StringNode> strLink = new();
        for (int y = 0; y < lineCount; y++)
        {
            for (int x = 0; x < lineCount; x++)
            {
                graph[y, x] = int.Parse(graphStr[y][x].ToString());
            }
        }
        for (int y = 0; y < lineCount; y++)
        {
            for (int x = 0; x < lineCount; x++)
            {
                if (graph[y, x] == 1)
                {
                    StringNode node = new();
                    strLink.Add(node);
                    StrDfs(graph, y, x, lineCount, node);
                }

            }
        }
        strLink.Sort((a, b) => a.total.CompareTo(b.total));
        Console.WriteLine(strLink.Count);
        for (int i = 0; i < strLink.Count; i++)
        {
            Console.WriteLine(strLink[i].total);
        }
    }

    static void StrDfs(int[,] _graphStr, int _y, int _x, int _length, StringNode _node)
    {
        if (_graphStr[_y, _x] != 1)
        {
            return;
        }
        _node.total += 1;
        _graphStr[_y, _x] = 2;
        //상하좌우 확인하기 시작

        if (0 < _y)
        {
            StrDfs(_graphStr, _y - 1, _x, _length, _node);
        }
        if (0 < _x)
        {
            StrDfs(_graphStr, _y, _x - 1, _length, _node);
        }
        if (_y < _length - 1)
        {
            StrDfs(_graphStr, _y + 1, _x, _length, _node);
        }
        if (_x < _length - 1)
        {
            StrDfs(_graphStr, _y, _x + 1, _length, _node);
        }
    }
    #endregion

    #region 깊이우선탐색 재귀

    static void Deeprecursion(List<int>[] neighborList, bool[] visit, int _nextNode)
    {
        if (visit[_nextNode] == true)
        {
            return;
        }
        Console.Write(_nextNode + "->");
        visit[_nextNode] = true;
        List<int> visitList = neighborList[_nextNode];
        for (int i = 0; i < visitList.Count; i++)
        {
            //4-4. 방문하지 않았던것만
            if (visit[visitList[i]] == false)
            {
                Deeprecursion(neighborList, visit, visitList[i]);
            }

        }
    }
    #endregion

    #region 숨바꼭질 dfs
    static int staticTime = int.MaxValue;

    static void Find(int cur, int goal, int _time)
    {
        if (cur == goal)
        {
            staticTime = Math.Min(staticTime, _time);
            return;
        }
        if (_time > staticTime)
        {
            return;
        }
        if (goal < cur)
        {
            _time += (cur - goal);
            staticTime = Math.Min(staticTime, _time);

            return;
        }

        _time += 1;
        if (0 < cur)
            Find(2 * cur, goal, _time);

        Find(cur + 1, goal, _time);

        if (0 < cur)
            Find(cur - 1, goal, _time);


    }
    #endregion

    #region 숨바꼭질BFS
    class FindNode
    {
        public int cur;
        public int goal;
        public int time;

        public FindNode()
        {

        }

        public FindNode(int _cur, int _goal, int _time)
        {
            cur = _cur;
            goal = _goal;
            time = _time;
        }

        public FindNode(FindNode _copy)
        {
            cur = _copy.cur;
            goal = _copy.goal;
            time = _copy.time;
        }

        public void DoubleMove()
        {
            cur *= 2;
            time += 1;
        }

        public void Left()
        {
            cur -= 1;
            time += 1;
        }

        public void Right()
        {
            cur += 1;
            time += 1;
        }

    }

    static void FindQueue(int cur, int goal)
    {
        int[] visit = new int[1000001];
        //컬 부터 목표지점까지 찾아보기 - bfs 탐색
        //어느 한지점에서 순서대로 2배, +1, -1 그 순으로 진행하기? 이때 가져야할게 각 함수가 진행될때 그 컬인데
        FindNode first = new FindNode(cur, goal, 0);
        Queue<FindNode> findQu = new();
        findQu.Enqueue(first);
        FindNode firstArrive = new();
        while (findQu.Count > 0)
        {
            FindNode find = findQu.Dequeue();
            if (find.cur < 0 || 100000 < find.cur)
            {
                continue;
            }
            if (visit[find.cur] != 0)
            {
                //방문했던곳인데
                if (visit[find.cur] < find.time)
                {
                    //다시들어온애가 더 오래걸려서 온거면 끝.
                    continue;
                }
            }
            //아니라면 처음 방문하는곳
            if (find.cur == find.goal)
            {
                //목적지면 최초 도달자가 최초 타임
                firstArrive = find;
                break;
            }
            //도착지 아니면
            visit[find.cur] = find.time; //찾은 노드의 찾은 횟수를 기록

            FindNode doubleNode = new FindNode(find);
            doubleNode.DoubleMove(); //2배이동한거

            FindNode leftNode = new FindNode(find);
            leftNode.Left();  //왼쪽 이동
            FindNode rightNode = new FindNode(find);
            rightNode.Right(); //오른편 이동

            //너비 상태로 큐 추가
            findQu.Enqueue(doubleNode);
            findQu.Enqueue(leftNode);
            findQu.Enqueue(rightNode);
        }
        Console.WriteLine(firstArrive.time);
    }
    #endregion

    #region 미로탐색 2178
    static void MakeMageGraph()
    {
        string[] lineCountStr = Console.ReadLine().Split();

        int yy = int.Parse(lineCountStr[0]); //행
        int xx = int.Parse(lineCountStr[1]); //열

        int[,] graph = new int[yy, xx];
        for (int y = 0; y < yy; y++)
        {
            string yyStr = Console.ReadLine();
            for (int x = 0; x < xx; x++)
            {
                int num = yyStr[x] - 48;
                graph[y, x] = num;
            }
        }
        bool[,] visit = new bool[yy, xx];
        MageBfs(graph, visit, (byte)yy, (byte)xx);

    }


    static void MageBfs(int[,] _graph, bool[,] _visit, byte _yLength, byte _xLength)
    {
        //bfs로 상하좌우 갈거
        (int, int) start = new((_yLength - 1), (_xLength - 1));
        Queue<(int, int)> mageQue = new();
        _visit[(_yLength - 1), (_xLength - 1)] = true;
        mageQue.Enqueue(start);

        while (mageQue.Count > 0)
        {
            (int, int) node = mageQue.Dequeue();

            int y = node.Item1;
            int x = node.Item2;

            //그게 아니라면 몇번째에 왔다고 발도장

            if (y == 0 && x == 0)
            {
                break;
            }
            // 상하좌우 이동 방향
            int[] dy = { -1, 1, 0, 0 };
            int[] dx = { 0, 0, -1, 1 };

            for (int i = 0; i < 4; i++)
            {
                //4방향

                int ny = y + dy[i];
                int nx = x + dx[i];

                if (0 <= ny && ny <= _yLength - 1 && 0 <= nx && nx <= _xLength - 1)
                {
                    if (_graph[ny, nx] == 1 && _visit[ny, nx] == false)
                    {
                        _graph[ny, nx] = _graph[y, x] + 1;
                        _visit[ny, nx] = true;
                        mageQue.Enqueue((ny, nx));
                    }
                }
            }

        }

        Console.WriteLine(_graph[0, 0]);
    }

    static bool PreCheck(int[,] _graph, int y, int x, int time)
    {
        if (_graph[y, x] == 0)
        {
            return false;
        }

        if (_graph[y, x] < time)
        {
            //더 돌아서 온거면 넘어가
            return false;
        }

        return true;
    }

    public enum Direc
    {
        None, Right, Left, Up, Down
    }

    static void MageDfs(int[,] _graph, int _y, int _x, int _yLength, int _xLength, int _curStep, Direc _direc)
    {
        if (_graph[_y, _x] == 0)
        {
            return;//갈수 없는 땅
        }
        if (_graph[_y, _x] < _curStep)
        {
            //더 멀리 돌아온 거리라면 빠꾸
            return;
        }

        _graph[_y, _x] = _curStep; //아니면 해당 노드를 컬스텝만에 밟았다고 인정
        _curStep++;

        if (_y == _yLength - 1 && _x == _xLength - 1)
        {
            //다왔으면 종료
            return;
        }

        if (0 < _y && _direc != Direc.Left)
        {
            MageDfs(_graph, _y - 1, _x, _yLength, _xLength, _curStep, Direc.Right);
        }
        if (0 < _x && _direc != Direc.Up)
        {
            MageDfs(_graph, _y, _x - 1, _yLength, _xLength, _curStep, Direc.Down);
        }
        if (_y < _yLength - 1 && _direc != Direc.Right)
        {
            MageDfs(_graph, _y + 1, _x, _yLength, _xLength, _curStep, Direc.Left);
        }
        if (_x < _xLength - 1 && _direc != Direc.Down)
        {
            MageDfs(_graph, _y, _x + 1, _yLength, _xLength, _curStep, Direc.Up);
        }
    }
    #endregion

    #region 체스판
    static void GoChess()
    {
        int caseCount = int.Parse(Console.ReadLine());

        for (int i = 0; i < caseCount; i++)
        {
            int lineCount = int.Parse(Console.ReadLine());
            int[,] graph = new int[lineCount, lineCount];
            bool[,] visit = new bool[lineCount, lineCount];
            string[] curPosStr = Console.ReadLine().Split();
            (int, int) curPos = (int.Parse(curPosStr[0]), int.Parse(curPosStr[1]));
            string[] goalPosStr = Console.ReadLine().Split();
            (int, int) goalPos = (int.Parse(goalPosStr[0]), int.Parse(goalPosStr[1]));
            ChessBfs(graph, visit, curPos.Item1, curPos.Item2, goalPos.Item1, goalPos.Item2, lineCount);
        }


    }

    static void ChessBfs(int[,] _graph, bool[,] _isVisit, int _startX, int _startY, int _goalX, int _goalY, int _length)
    {
        Queue<(int, int)> chessQue = new();
        chessQue.Enqueue((_startX, _startY));

        while (chessQue.Count > 0)
        {
            (int, int) curPos = chessQue.Dequeue();

            int x = curPos.Item1;
            int y = curPos.Item2;
            if (x == _goalX && y == _goalY)
            {
                break;
            }

            //8방향으로 다음 nx, ny를 구해야함 

            int[] dx = { -2, -1, 1, 2, -2, -1, 1, 2 }; //x 방향
            int[] dy = { 1, 2, 2, 1, -1, -2, -2, -1 }; //y 방향

            for (int i = 0; i < 8; i++)
            {
                int nx = x + dx[i];
                int ny = y + dy[i];

                if (0 <= nx && nx < _length && 0 <= ny && ny < _length)
                {
                    //유효한 이동이면
                    if (_graph[nx, ny] == 0) //그 위치에 아무도 간적이 없으면
                    {
                        _graph[nx, ny] = _graph[x, y] + 1; //현재 위치값에서 1을 더한거
                        chessQue.Enqueue((nx, ny)); //큐 삽입
                    }
                }
            }
        }
        Console.WriteLine(_graph[_goalX, _goalY]);
    }
    #endregion

    #region 토매토
    static void Tomato()
    {
        string[] lineCountStr = Console.ReadLine().Split();

        int yy = int.Parse(lineCountStr[1]); //행
        int xx = int.Parse(lineCountStr[0]); //열

        int[,] graph = new int[yy, xx];
        List<(int, int)> welldonList = new();
        int rareCount = 0;
        for (int y = 0; y < yy; y++)
        {
            string[] yyStr = Console.ReadLine().Split();
            for (int x = 0; x < xx; x++)
            {
                int num = int.Parse(yyStr[x]);
                graph[y, x] = num;
                if (num == 1)
                {
                    welldonList.Add((y, x));
                }
                else if (num == 0)
                {
                    rareCount += 1;
                }
            }
        }
        TomatoBfs(graph, rareCount, welldonList, xx, yy);
    }

    static void TomatoBfs(int[,] _graph, int _rareCount, List<(int, int)> _wellDone, int _xLength, int _yLength)
    {
        //한 큐마다 익은거 하나당 확장하고, 익히고, 확장한건 다음 큐에 넣는다. 
        //그렇게 큐에 넣을게 없으면 탐색이 끝나고, 
        //익은 수가 rareCount보다 낮으면 다 못 익은거고 
        //같다면, 카운팅된 날짜만큼 일자 소요

        Queue<(int, int)> tomatoQue = new();
        for (int i = 0; i < _wellDone.Count; i++)
        {
            tomatoQue.Enqueue(_wellDone[i]); //잘익은거 집어넣고
        }

        int day = 0;
        while (tomatoQue.Count > 0)
        {
            if (_rareCount == 0)
            {
                break;
            }

            Queue<(int, int)> nextQue = new();
            day += 1; //하루가 지나고
            int queCount = tomatoQue.Count;
            for (int i = 0; i < queCount; i++)
            {
                (int, int) place = tomatoQue.Dequeue();

                int[] dx = { -1, 1, 0, 0 };
                int[] dy = { 0, 0, 1, -1 };

                for (int direc = 0; direc < 4; direc++)
                {
                    int ny = place.Item1 + dy[direc];
                    int nx = place.Item2 + dx[direc];
                    if (0 <= nx && nx < _xLength && 0 <= ny && ny < _yLength)
                    {
                        if (_graph[ny, nx] == 0)
                        {
                            _rareCount -= 1;
                            _graph[ny, nx] = 1;
                            nextQue.Enqueue((ny, nx));
                        }
                    }
                }
            }

            tomatoQue = nextQue;
        }

        int answer = -1;
        if (_rareCount == 0)
        {
            answer = day;
        }
        Console.WriteLine(answer);
    }
    #endregion

    #region fff
    static void Serach()
    {
        string[] command = Console.ReadLine().Split();
        int nodeCount = int.Parse(command[0]);
        int lineCount = int.Parse(command[1]);
        int start = int.Parse(command[2]);

        List<int>[] graph = new List<int>[nodeCount + 1];

        for (int i = 0; i < nodeCount + 1; i++)
        {
            graph[i] = new List<int>();
        }

        for (int i = 0; i < lineCount; i++)
        {
            string[] line = Console.ReadLine().Split();
            int from = int.Parse(line[0]);
            int to = int.Parse(line[1]);
            graph[from].Add(to);
            graph[to].Add(from);
        }

        for (int i = 0; i < nodeCount + 1; i++)
        {
            graph[i].Sort();
        }

        Dfs(graph, start, new bool[nodeCount + 1]);
        Bfs(graph, start, new bool[nodeCount + 1]);
    }

    static void Dfs(List<int>[] _graph, int _start, bool[] _visit)
    {
        Stack<int> nodeSt = new Stack<int>();
        nodeSt.Push(_start);
        StringBuilder sb = new StringBuilder();
        while (nodeSt.Count > 0)
        {
            int curNode = nodeSt.Pop();

            if (_visit[curNode] == true)
            {
                continue;
            }
            _visit[curNode] = true;
            sb.Append(curNode + " ");
            List<int> abeNode = _graph[curNode];
            for (int i = abeNode.Count - 1; i >= 0; i--)
            {
                if (_visit[abeNode[i]] == false)
                {
                    nodeSt.Push(abeNode[i]);
                }
            }
        }
        sb.Remove(sb.Length - 1, 1);
        Console.WriteLine(sb);
    }

    static void Bfs(List<int>[] _graph, int _start, bool[] _visit)
    {
        Queue<int> nodeSt = new Queue<int>();
        nodeSt.Enqueue(_start);
        StringBuilder sb = new StringBuilder();
        while (nodeSt.Count > 0)
        {
            int curNode = nodeSt.Dequeue();

            if (_visit[curNode] == true)
            {
                continue;
            }
            _visit[curNode] = true;
            sb.Append(curNode + " ");
            List<int> abeNode = _graph[curNode];
            for (int i = 0; i < abeNode.Count; i++)
            {
                if (_visit[abeNode[i]] == false)
                {
                    nodeSt.Enqueue(abeNode[i]);
                }
            }
        }
        sb.Remove(sb.Length - 1, 1);
        Console.WriteLine(sb);
    }

    #endregion

    #region 뱀과사다리
    static void SankeLadder()
    {
        //그래프를 짯다고 치고, 
        //병렬로 수행하며, 사다리를 다 타본다 - 뱀은 다 피한다. 
        //사다리를 타고, 타고 탄 그 루트로 최종컷을 낸다. 
        // 그럼 1~100 까지가 있고 노드는 뱀과, 사다리만 놓고 본다? 사다리를 타고 갔다가 뱀을 타고 돌아와서 가는것도 방법일수도
        int[] visit = new int[101];
        string[] command = Console.ReadLine().Split();
        int ladderCount = int.Parse(command[0]);
        int sankeCount = int.Parse(command[1]);
        int[] map = new int[101];
        for (int i = 0; i < ladderCount; i++)
        {
            string[] ladderStr = Console.ReadLine().Split();
            int from = int.Parse(ladderStr[0]);
            int to = int.Parse(ladderStr[1]);
            map[from] = to;
        }
        for (int i = 0; i < sankeCount; i++)
        {
            string[] sankeStr = Console.ReadLine().Split();
            int from = int.Parse(sankeStr[0]);
            int to = int.Parse(sankeStr[1]);
            map[from] = to;
        }
        SankeBfs(map, visit, 1, 100);
    }

    static void SankeBfs(int[] _map, int[] _visit, int _start, int _end)
    {
        Queue<int> nextQue = new();
        nextQue.Enqueue(_start);
        _visit[_start] = 0;
        while (nextQue.Count > 0)
        {
            int curPos = nextQue.Dequeue();
            if (curPos == 100)
            {
                break;
            }

            for (int i = 1; i <= 6; i++)
            {
                int nextPos = curPos + i;
                if (nextPos <= 100)
                {

                    if (_map[nextPos] != 0)
                    {
                        //다음 이동하려는곳이 만약 0 이 아니면 이동해야하는곳
                        nextPos = _map[nextPos]; //nextPos로 바꾸고
                    }

                    if (_visit[nextPos] == 0)
                    {
                        _visit[nextPos] = _visit[curPos] + 1; //다녀간 도장 찍고
                        nextQue.Enqueue(nextPos);
                    }
                }
            }
        }

        Console.WriteLine(_visit[100]);
    }
    #endregion

    static void Main()
    {
        BreakWall();
    }

    static void BreakWall()
    {
        string[] lineCountStr = Console.ReadLine().Split();

        int yy = int.Parse(lineCountStr[0]); //행
        int xx = int.Parse(lineCountStr[1]); //열

        int[,] graph = new int[yy, xx];
        for (int y = 0; y < yy; y++)
        {
            string yyStr = Console.ReadLine();
            for (int x = 0; x < xx; x++)
            {
                int num = (yyStr[x] - 48);
                graph[y, x] = num;
            }
        }
    }
  
}

#region 덱
public class DeckNode
{
    public DeckNode pre;
    public DeckNode next;
}

public class Deck
{
    // push_front X: 정수 X를 덱의 앞에 넣는다." 
    //push_back X: 정수 X를 덱의 뒤에 넣는다." +
    //pop_front: 덱의 가장 앞에 있는 수를 빼고, 그 수를 출력한다.
    //만약, 덱에 들어있는 정수가 없는 경우에는 -1을 출력한다." +
    //pop_back: 덱의 가장 뒤에 있는 수를 빼고, 그 수를 출력한다.
    //만약, 덱에 들어있는 정수가 없는 경우에는 -1을 출력한다." +

    //size: 덱에 들어있는 정수의 개수를 출력한다.
    //empty: 덱이 비어있으면 1을, 아니면 0을 출력한다." +
    //front: 덱의 가장 앞에 있는 정수를 출력한다. 만약 덱에 들어있는 정수가 없는 경우에는 -1을 출력한다." +
    //back: 덱의 가장 뒤에 있는 정수를 출력한다. 만약 덱에 들어있는 정수가 없는 경우에는 -1을 출력한다."

}
#endregion