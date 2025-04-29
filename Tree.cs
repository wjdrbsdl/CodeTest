using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class Tree
{
    //자료구조의 구현 방식 - 1. 순차(배열) 2. 연결리스트(노드 같은거)

    //노드란 무엇일까 
    #region 트리그리기

    static void PaintTree()
    {
        //주어지는 노드 연결을 보고, 누가 부모인지 캐치해서 트리 그리는문제 
        //1. 그래프로 먼저 구성해야함. 
        int nodeCount = int.Parse(Console.ReadLine());
        List<int>[] treeGraph = new List<int>[nodeCount + 1];

        int lineCount = nodeCount - 1;
        for (int i = 1; i < nodeCount + 1; i++)
        {
            treeGraph[i] = new List<int>();
        }

        for (int i = 1; i <= lineCount; i++)
        {
            string[] lineStr = Console.ReadLine().Split();
            int from = int.Parse(lineStr[0]);
            int to = int.Parse(lineStr[1]);

            treeGraph[from].Add(to);
            treeGraph[to].Add(from);
        } //그래프 완성

        Queue<int> childNode = new();
        childNode.Enqueue(1); //1번 부터 자식 주입 시작
        int[] parentInfo = new int[nodeCount + 1];
        parentInfo[1] = -1; //루트노드 지정.
        while (childNode.Count > 0)
        {
            int curNode = childNode.Dequeue();
            List<int> childs = treeGraph[curNode];
            for (int i = 0; i < childs.Count; i++)
            {
                int leafNode = childs[i];
                if (parentInfo[leafNode] == 0)
                {
                    //타겟의 부모가 없으면 날 부모로 지정
                    parentInfo[leafNode] = curNode;
                    childNode.Enqueue(leafNode);
                }

            }
        }

        StringBuilder sb = new StringBuilder();
        for (int i = 2; i < parentInfo.Length; i++)
        {
            sb.AppendLine(parentInfo[i].ToString());
        }

        Console.WriteLine(sb);
    }
    #endregion

    #region 힙 구현하기

    static void TestMinHeap()
    {
        int commandCount = int.Parse(Console.ReadLine());
        MinHeap minHeap = new();
        StringBuilder sb = new();
        for (int i = 0; i < commandCount; i++)
        {
            int command = int.Parse(Console.ReadLine());
            if (command == 0)
            {
                sb.AppendLine(minHeap.Dequeue().ToString());
            }
            else
            {
                minHeap.Enqueue(command);
            }

        }
        Console.WriteLine(sb);
    }

    class MinHeap
    {


        //트리를 배열로 구현 - 모든 노드 정보가 배열에 표현(인덱스가 보유) 
        // 내 자식을 찾아가려면 인덱스가 필요
        //인덱스 규칙만으로 자식, 부모가 산출되어야 한다. 
        // + 중간 노드가 비는 경우가 발생할 수 있는데, 힙은 완전트리로서 중간에 비는게 없어서 배열로 해도 낭비가 없다.
        private List<int> _tree = new();

        //Peek
        //입력 : 없음
        //출력 : 최소 원소 -> 루트 노드 값 -> 0번
        public int Peek()
        {
            return _tree[0];
        }

        //Enqueue
        public void Enqueue(int newValue)
        {
            //1. 맨 끝에 넣는다
            _tree.Add(newValue);

            //2. 부모랑 비교해서 자식 크기가 작으면, 부모랑 바꾼다. 
            // -> 위를 반복한다. 
            int childIdx = _tree.Count - 1;
            while (childIdx != 0)
            {
                int parentIdx = ((childIdx - 1) / 2);

                if (_tree[parentIdx] <= _tree[childIdx])
                {
                    //부모가 자식보다 작으면 아무일 없음
                    return;
                }

                //자식이 더 작으면 부모와 위치를 바꾼다
                int tempValue = _tree[parentIdx];
                _tree[parentIdx] = _tree[childIdx];
                _tree[childIdx] = tempValue;

                childIdx = parentIdx; //자식 인덱스를 부모 인덱스로 바꾼뒤 계속 진행한다
            }

        }

        //Dequeue
        public int Dequeue()
        {
            if (_tree.Count == 0)
            {
                return 0;
            }
            int minValue = _tree[0];
            int lastIdx = _tree.Count - 1;
            int lastValue = _tree[lastIdx];
            //1. 맨 마지막 원소를 루트노드로 이동하기
            _tree[0] = lastValue; //루트노드에 마지막 값 넣고
            _tree.RemoveAt(lastIdx); //마지막 원소 제거

            //2. 힙불변성 유지 위해 부모로부터 자식과 비교시작
            int parentIdx = 0;
            while (parentIdx < _tree.Count)
            {
                int parentValue = _tree[parentIdx];
                int leftIdx = (parentIdx + 1) * 2 - 1;
                int rightIdx = (parentIdx + 1) * 2;

                //3. 자식 없으면 끝
                if (_tree.Count <= leftIdx)
                {
                    return minValue;
                }
                int leftValue = _tree[leftIdx];
                if (_tree.Count <= rightIdx)
                {
                    //오른쪽 자식 없으면 왼쪽자식하고만 비교
                    if (leftValue < parentValue)
                    {
                        _tree[leftIdx] = parentValue;
                        _tree[parentIdx] = leftValue;
                        parentIdx = leftIdx;
                        //그리고 탐색 진행
                        continue;
                    }
                    //부모가 더 작으면 종료
                    return minValue;
                }

                int rightValue = _tree[rightIdx];
                if (leftValue < rightValue)
                {
                    //왼쪽 자식이 작으면 왼쪽 자식하고 부모 비교
                    if (leftValue < parentValue)
                    {
                        _tree[leftIdx] = parentValue;
                        _tree[parentIdx] = leftValue;
                        parentIdx = leftIdx;
                        //그리고 탐색 진행
                        continue;
                    }
                    //부모가 더 작으면 종료
                    return minValue;
                }
                //오른쪽 자식이 작으면 오른쪽 자식하고 부모 비교
                if (rightValue < parentValue)
                {
                    _tree[rightIdx] = parentValue;
                    _tree[parentIdx] = rightValue;
                    parentIdx = rightIdx;
                    //그리고 탐색 진행
                    continue;
                }
                return minValue;
            }

            //부모가 더 작으면 종료
            return minValue;
        }
    }

    #endregion

    #region 다익스트라
    const int INF = 987654321; //해당 값이 나오면 연결 안 된 것. 
    const int NoWay = -1;
    private static int[][] graph;
    static void Dijkstra()
    {
        //그래프 구성
        graph = new int[7][];

        graph[0] = new int[] { 0, 7, INF, INF, 3, 10, INF };
        graph[1] = new int[] { 7, 0, 4, 10, 2, 6, INF };
        graph[2] = new int[] { INF, 4, 0, 2, INF, INF, INF };
        graph[3] = new int[] { INF, 10, 2, 0, 11, 9, 4 };
        graph[4] = new int[] { 3, 2, INF, 11, 0, INF, 5 };
        graph[5] = new int[] { 10, 6, INF, 9, INF, 0, INF };
        graph[6] = new int[] { INF, INF, INF, 4, 5, INF, 0 };

    }

    static int GetDistance(int start, int goal)
    {
        int distance = 0;
        //start로부터 다른 모든 정점까지 거리를 저장할 배열
        int[] distanceRecord = new int[graph.Length];
        for (int i = 0; i < distanceRecord.Length; i++)
        {
            distanceRecord[i] = INF;
        }
        distanceRecord[start] = 0;
        int[] path = new int[graph.Length];
        for (int i = 0; i < path.Length; i++)
        {
            path[i] = NoWay;
        }
        path[start] = 0;

        //가장 짧은 루트로만 계속 쭉쭉 진행
        PriorityQueue<int, int> pq = new();
        pq.Enqueue(start, distanceRecord[start]);
        while (pq.Count > 0)
        {
            int current = pq.Dequeue(); //어느 지점의 최소 경로 값이 나온다. 

            //dist를 갱신한다
            for (int v = 0; v < graph[current].Length; v++)
            {
                int viaNext = distanceRecord[current] + graph[current][v]; //v 까지의 경유한 거리 계산 

                //최단 거리 비교한다
                if (viaNext < distanceRecord[v])
                {
                    //경유 경로가 더 짧으면 새로 갱신할것
                    distanceRecord[v] = viaNext;
                    pq.Enqueue(v, distanceRecord[v]); //해당 current 기준으로 짧아졌던 거리로 목적지와 그 거리가 들어가게 됨
                    path[v] = current;
                    //만약 갱신되었다면 같은 v에 향해 갈수 있다는 record[v] 값이 여러개로 있을 수 있겠다.
                }
            }
        }

        Stack<int> pathStack = new();
        int cur = goal;
        pathStack.Push(goal);
        while (cur != start)
        {
            int pre = path[cur];
            pathStack.Push(pre);
            cur = pre;
        }
        int stackCount = pathStack.Count;
        for (int i = 0; i < stackCount; i++)
        {
            Console.Write(pathStack.Pop().ToString() + " ");
        }

        return distanceRecord[goal];

    }

    #endregion

    #region A*
    const int MAX_Y = 10;
    const int MAX_X = 10;
    static char[][] map = new char[MAX_Y][];


    // 맵을 구성한다.
    static void ConstructMap()
    {
        map[0] = "          ".ToCharArray();
        map[1] = "          ".ToCharArray();
        map[2] = "          ".ToCharArray();
        map[3] = "    #     ".ToCharArray();
        map[4] = " S  #  G  ".ToCharArray();
        map[5] = "    #     ".ToCharArray();
        map[6] = "          ".ToCharArray();
        map[7] = "          ".ToCharArray();
        map[8] = "          ".ToCharArray();
        map[9] = "          ".ToCharArray();
    }

    static int startX, startY, endX, endY;
    static void AStar()
    {

        for (int y = 0; y < map.Length; y++)
        {
            char[] inMap = map[y];
            for (int x = 0; x < inMap.Length; x++)
            {
                char charactor = map[y][x];
                if (charactor == 'S')
                {
                    startX = x;
                    startY = y;
                }
                else if (charactor == 'G')
                {
                    endX = x;
                    endY = y;
                }
            }
        }

    }

    class AstarNode
    {
        //좌표값
        public int X;
        public int Y;

        //f(x) = h(x)+ g(x)에서 f(x)
        public int F;

        public AstarNode Path; //나 오기전 녀석
    }

    //휴리스틱 거리 필요 h(x) 맨해튼 or 유클리드
    static int GetHeuristic(int _x1, int _y1, int _x2, int _y2)
    {
        //맨해튼 방식
        return Math.Abs(_x1 - _x2) + Math.Abs(_y2 - _y1);
    }

    static void SetPath()
    {
        //경로 생성
        //맵과 대응되게
        AstarNode[,] pathMap = new AstarNode[MAX_Y, MAX_X];
        for (int y = 0; y < MAX_Y; y++)
        {
            for (int x = 0; x < MAX_X; x++)
            {
                pathMap[y, x] = new AstarNode() { X = x, Y = y };
            }
        }

        //우선순위 큐
        PriorityQueue<AstarNode, int> priority = new(); //8방향탐색
        priority.Enqueue(pathMap[startY, startX], 0);
        //  
        int[] dy = { -1, -1, -1, 0, 1, 1, 1, 0 };
        int[] dx = { -1, 0, 1, 1, 1, 0, -1, -1 };
        int[] dg = { 14, 10, 14, 10, 14, 10, 14, 10 }; //g(x) 8방향에 가기위한 가중치 1, 1.4 에 10을 곱한것

        while (priority.Count > 0)
        {
            AstarNode current = priority.Dequeue();
            for (int i = 0; i < dy.Length; i++)
            {
                int ny = current.Y + dy[i];
                int nx = current.X + dx[i];

                //유효성 검사
                if ((0 <= nx && nx < MAX_X && 0 <= ny && ny < MAX_Y) == false)
                {
                    //범위 벗어난건 패쓰
                    continue;
                }
                if (map[ny][nx] == '#')
                {
                    //벽이면 패쓰
                    continue;
                }
                int f = dg[i] + GetHeuristic(nx, ny, endX, endY) * 10; //4각의 대각으로 이동하는 가중치는 14로 동일한데
                                                                       //nx,ny와 목적지의 휴리스틱(맨해튼 거리 계산법)으로 계산했을때 그게 더 작은건 목적지 방향으로 향하는거
                                                                       //f -> 가 곧 가중치가 됨. 가중치 10,14,10만 넣는게 아닌 목적지와의 근접함(h(x)를 추가해서 f 생성

                //다음 갈것 가져오기
                AstarNode nextNode = pathMap[ny, nx];
                if (nextNode.F > f)
                {
                    nextNode.F = f;
                    if (nextNode.X == endX && nextNode.Y == endY)
                    {

                    }
                    nextNode.Path = current;
                    priority.Enqueue(nextNode, f);
                }
            }
        }

        Stack<AstarNode> pathStack = new();
        AstarNode curNode = pathMap[endY, endX];
        while (curNode.X != startX && curNode.Y != startY)
        {
            AstarNode pre = curNode.Path;
            Console.WriteLine(pre.X + " " + pre.Y);
            curNode = pre;
        }
    }

    #endregion

    #region 최단경로1753
    static void FindFastPath()
    {
        string[] veInfo = Console.ReadLine().Split();
        int vCount = int.Parse(veInfo[0]);
        int eCount = int.Parse(veInfo[1]);
        int startV = int.Parse(Console.ReadLine());

        //그래프 그리기
        Dictionary<int, int>[] graph = new Dictionary<int, int>[vCount + 1];
        for (int i = 0; i < eCount; i++)
        {
            string[] eInfo = Console.ReadLine().Split();
            int start = int.Parse(eInfo[0]);
            int end = int.Parse(eInfo[1]);
            int weight = int.Parse(eInfo[2]);
            InsertWeight(start, end, weight, graph);
        }

        DiSearch(startV, vCount, graph);
    }

    static void InsertWeight(int _startV, int _endV, int _weight, Dictionary<int, int>[] _graph)
    {
        Dictionary<int, int> startDic = _graph[_startV];
        if (startDic == null)
        {
            startDic = new();
            _graph[_startV] = startDic;
        }
        if (startDic.ContainsKey(_endV))
        {
            startDic[_endV] = Math.Min(startDic[_endV], _weight);
        }
        else
        {
            startDic.Add(_endV, _weight);
        }

        Dictionary<int, int> endDic = _graph[_endV];
        if (endDic == null)
        {
            endDic = new();
            _graph[_endV] = endDic;
        }
        if (startDic.ContainsKey(_startV))
        {
            startDic[_startV] = Math.Min(startDic[_startV], _weight);
        }
        else
        {
            startDic.Add(_startV, _weight);
        }
    }

    static void DiSearch(int _start, int _vCount, Dictionary<int, int>[] _graph)
    {
        int[] distance = new int[_vCount + 1];
        for (int i = 0; i < distance.Length; i++)
        {
            distance[i] = int.MaxValue; //최대거리로 초기화하고
        }
        distance[_start] = 0; //시작은 0으로하고

        PriorityQueue<int, int> que = new();
        que.Enqueue(_start, 0); //시작정점과 여기까지의 거리 0을 넣고

        while (que.Count > 0)
        {
            int currentV = que.Dequeue(); //거리가 제일 짧은 녀석을 뽑고
            Dictionary<int, int> currentPath = _graph[currentV];
            foreach (KeyValuePair<int, int> item in currentPath)
            {
                int nextV = item.Key; //갈 수 있는 정점
                int weight = item.Value; //그 정점까지의 가중치

                int newWeight = distance[currentV] + weight; //현재위치까지 왔던 값에 가중치를 더함

                if (newWeight < distance[nextV])
                {
                    //새로 갱신된 경로가 기존 경로보다 짧으면
                    distance[nextV] = newWeight; //거기까지 가는 경로 길이를 수정하고
                    que.Enqueue(nextV, newWeight); //nextV 까지 가는 거리로 추가 
                }
            }
        }

        StringBuilder sb = new StringBuilder();
        for (int i = 1; i < distance.Length; i++)
        {
            int value = distance[i];
            if (value == int.MaxValue)
            {
                sb.AppendLine("INF");
            }
            else
            {
                sb.AppendLine(value.ToString());
            }
        }
        Console.WriteLine(sb);
    }
    #endregion

    #region 최단경로1504
    static void SpecialRoute()
    {
        string[] veInfo = Console.ReadLine().Split();
        int vCount = int.Parse(veInfo[0]);
        int eCount = int.Parse(veInfo[1]);

        //그래프 그리기
        Dictionary<int, int>[] graph = new Dictionary<int, int>[vCount + 1];
        for (int i = 0; i < eCount; i++)
        {
            string[] eInfo = Console.ReadLine().Split();
            int start = int.Parse(eInfo[0]);
            int end = int.Parse(eInfo[1]);
            int weight = int.Parse(eInfo[2]);
            SInsertWeight(start, end, weight, graph);
        }

        string[] specialInfo = Console.ReadLine().Split();
        int routeA = int.Parse(specialInfo[0]);
        int routeB = int.Parse(specialInfo[1]);

        int a1 = SDiSearch(1, vCount, graph, routeA);
        int a2 = SDiSearch(routeA, vCount, graph, routeB);
        int a3 = SDiSearch(routeB, vCount, graph, vCount);
        int b1 = SDiSearch(1, vCount, graph, routeB);
        int b2 = SDiSearch(routeB, vCount, graph, routeA);
        int b3 = SDiSearch(routeA, vCount, graph, vCount);

        long A = a1 + a2 + a3;
        long B = b1 + b2 + b3;

        long answer = Math.Min(A, B);
        if (a1 == -1 || a2 == -1 || a3 == -1)
        {
            answer = -1;
        }

        Console.WriteLine(answer);
    }

    static void SInsertWeight(int _startV, int _endV, int _weight, Dictionary<int, int>[] _graph)
    {
        Dictionary<int, int> startDic = _graph[_startV];
        if (startDic == null)
        {
            startDic = new();
            _graph[_startV] = startDic;
        }
        if (startDic.ContainsKey(_endV))
        {
            startDic[_endV] = Math.Min(startDic[_endV], _weight);
        }
        else
        {
            startDic.Add(_endV, _weight);
        }

        Dictionary<int, int> endDic = _graph[_endV];
        if (endDic == null)
        {
            endDic = new();
            _graph[_endV] = endDic;
        }
        if (endDic.ContainsKey(_startV))
        {
            endDic[_startV] = Math.Min(endDic[_startV], _weight);
        }
        else
        {
            endDic.Add(_startV, _weight);
        }
    }

    static int SDiSearch(int _start, int _vCount, Dictionary<int, int>[] _graph, int _end)
    {
        int[] distance = new int[_vCount + 1];
        for (int i = 0; i < distance.Length; i++)
        {
            distance[i] = int.MaxValue; //최대거리로 초기화하고
        }
        distance[_start] = 0; //시작은 0으로하고

        PriorityQueue<int, int> que = new();
        que.Enqueue(_start, 0); //시작정점과 여기까지의 거리 0을 넣고

        while (que.Count > 0)
        {
            int currentV = que.Dequeue(); //거리가 제일 짧은 녀석을 뽑고
            Dictionary<int, int> currentPath = _graph[currentV];
            if (currentPath == null)
            {
                continue;
            }
            foreach (KeyValuePair<int, int> item in currentPath)
            {
                int nextV = item.Key; //갈 수 있는 정점
                int weight = item.Value; //그 정점까지의 가중치

                int newWeight = distance[currentV] + weight; //현재위치까지 왔던 값에 가중치를 더함

                if (newWeight < distance[nextV])
                {
                    //새로 갱신된 경로가 기존 경로보다 짧으면
                    distance[nextV] = newWeight; //거기까지 가는 경로 길이를 수정하고
                    que.Enqueue(nextV, newWeight); //nextV 까지 가는 거리로 추가 
                }
            }
        }

        int minDistance = distance[_end];
        if (minDistance == int.MaxValue)
        {
            minDistance = -1;
        }
        return minDistance;
    }
    #endregion

    #region 이진검색트리
    class BSTree
    {
        private BsTreeNode _root;

        public void Insert(int data)
        {
            //루트가 null 인가?
            // null이면 루트 노드를 만든다.
            BsTreeNode node = new BsTreeNode(data, null, this);
            if (_root == null)
            {
                _root = node;
                return;
            }

            _root.Insert(node);
        }

        public List<int> PreOrder()
        {
            List<int> dataList = new();
            if (_root == null)
            {
                return dataList;
            }

            _root.PreOrder(dataList);
            return dataList;
        }

        public List<int> InOrderSearch()
        {
            List<int> dataList = new();
            if (_root == null)
            {
                //null 객체? 
                return dataList;
            }

            _root.LeftOrder(dataList);
            return dataList;
        }

        public List<int> LevelOrderSearch()
        {
            List<int> dataList = new();
            if (_root == null)
            {
                //null 객체? 
                return dataList;
            }

            _root.LevelOrder(dataList);
            return dataList;
        }

        public bool Contains(int findData)
        {
            if (_root == null)
            {
                return false;
            }

            return _root.FindValue(findData);
        }
    }

    class BsTreeNode
    {
        private int _data;
        private BsTreeNode? _left;
        private BsTreeNode? _right;
        public BsTreeNode Parent { get; private set; }
        private BSTree? _tree;

        public BsTreeNode(int data, BsTreeNode parent, BSTree tree)
        {
            _data = data;
            Parent = parent;
            _tree = tree;
        }

        public void Insert(BsTreeNode node)
        {
            int nodeValue = node._data;
            if (nodeValue <= _data)
            {
                if (_left == null)
                {
                    _left = node;
                    node.Parent = this;
                }
                else
                {
                    _left.Insert(node);
                }
            }
            else
            {
                if (_right == null)
                {
                    _right = node;
                    node.Parent = this;
                }
                else
                {
                    _right.Insert(node);
                }
            }
        }

        public void PreOrder(List<int> dataList)
        {
            dataList.Add(_data);
            if (_left != null)
            {
                _left.PreOrder(dataList);
            }
            if (_right != null)
            {
                _right.PreOrder(dataList);
            }
        }

        public void LeftOrder(List<int> dataList)
        {

            if (_left != null)
            {
                _left.LeftOrder(dataList);
            }
            dataList.Add(_data);
            if (_right != null)
            {
                _right.LeftOrder(dataList);
            }
        }

        public void LastOrder(List<int> dataList)
        {
            if (_left != null)
            {
                _left.LastOrder(dataList);
            }
            if (_right != null)
            {
                _right.LastOrder(dataList);
            }
            dataList.Add(_data);
        }

        public void LevelOrder(List<int> dataList)
        {
            Queue<BsTreeNode> nodeQu = new();
            nodeQu.Enqueue(this); //루트 노드 집어넣기
            dataList.Add(_data);
            while (nodeQu.Count > 0)
            {
                BsTreeNode node = nodeQu.Dequeue();

                if (node._left != null)
                {
                    nodeQu.Enqueue(node._left);
                    dataList.Add(node._left._data);
                }
                if (node._right != null)
                {
                    nodeQu.Enqueue(node._right);
                    dataList.Add(node._right._data);
                }
            }

        }

        public bool FindValue(int findValue)
        {
            if (findValue == _data)
            {
                return true;
            }

            //자식 둘 중 있으면 트루 
            bool leftFind = false;
            if (_left != null && findValue <= _data)
            {
                leftFind = _left.FindValue(findValue);
            }
            bool rightFind = false;
            if (_right != null && _data < findValue)
            {
                rightFind = _right.FindValue(findValue);
            }

            if (leftFind || rightFind)
            {
                return true;
            }

            return false;
        }
    }
    #endregion

    static void Main()
    {
        
    }

    #region 동생찾기
    static void FindBrother()
    {
        string[] command = Console.ReadLine().Split();
        int subin = int.Parse(command[0]);
        int young = int.Parse(command[1]);

    }
    #endregion

}

