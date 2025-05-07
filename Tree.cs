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

    #region 전위입력후위출력
    static void FirstSearch()
    {
        int[,] binaryTree = new int[100001, 3];
        int root = int.Parse(Console.ReadLine());
        binaryTree[root, 0] = int.MaxValue;
        string command;
        while ((command = Console.ReadLine()) != null)
        {
            if (command == "")
            {
                break;
            }
            int node = int.Parse(command);

            Insert(node, binaryTree, root);

        }

        SayLayter(root, binaryTree);
    }

    static void Insert(int _value, int[,] _graph, int _parent)
    {
        //부모보다 낮으면 왼쪽에
        if (_value < _parent)
        {
            //이미 왼쪽에 있으면 왼쪽 리프에서 재진행
            if (_graph[_parent, 1] == 0)
            {
                _graph[_parent, 1] = _value;
                _graph[_value, 0] = _parent;
            }
            else
            {
                Insert(_value, _graph, _graph[_parent, 1]);
            }
            return;
        }

        if (_graph[_parent, 2] == 0)
        {
            _graph[_parent, 2] = _value;
            _graph[_value, 0] = _parent;
            return;
        }
        Insert(_value, _graph, _graph[_parent, 2]);
    }

    static void SayLayter(int _start, int[,] _graph)
    {
        if (_graph[_start, 1] != 0)
        {
            SayLayter(_graph[_start, 1], _graph);
        }
        if (_graph[_start, 2] != 0)
        {
            SayLayter(_graph[_start, 2], _graph);
        }

        Console.WriteLine(_start.ToString());
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

    #region 최대힙
    public class MaxHeap
    {
        List<int> list;
        public MaxHeap()
        {
            list = new();
            list.Add(-1);
        }

        public void Enque(int _value)
        {
            list.Add(_value);
            int childIdx = list.Count - 1;
            while (childIdx != 1)
            {
                int parentIdx = childIdx / 2;
                int parentValue = list[parentIdx];
                int childValue = list[childIdx];
                //부모보다 내가 크면 위로 올라갈것
                if (childValue < parentValue)
                {
                    break;
                }
                list[childIdx] = parentValue;
                list[parentIdx] = childValue;
                childIdx = parentIdx;
            }
        }

        public int Deque()
        {
            if (list.Count == 1)
            {
                return 0;
            }
            int maxValue = list[1];
            int lastIdx = list.Count - 1;
            list[1] = list[lastIdx]; //맨 끝값을 앞으로
            list.RemoveAt(lastIdx); //맨끝 잘라내고
            int parentIdx = 1;
            if (list.Count == 1)
            {
                return maxValue;
            }
            int parentValue = list[parentIdx];

            while (true)
            {
                int rightChildIdx = parentIdx * 2 + 1;
                int leftChildIdx = rightChildIdx - 1;
                if (rightChildIdx < list.Count)
                {
                    //오른쪽 유효하다면
                    int rightValue = list[rightChildIdx];
                    int leftValues = list[leftChildIdx];
                    //둘중에 큰거랑 비교
                    if (leftValues < rightValue)
                    {
                        if (parentValue < rightValue)
                        {
                            list[parentIdx] = rightValue;
                            list[rightChildIdx] = parentValue;
                            parentIdx = rightChildIdx;
                            continue;
                        }
                        else
                        {
                            //부모가 더크면 종료
                            break;
                        }
                    }
                    else
                    {
                        if (parentValue < leftValues)
                        {
                            list[parentIdx] = leftValues;
                            list[leftChildIdx] = parentValue;
                            parentIdx = leftChildIdx;
                            continue;
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                if (leftChildIdx < list.Count)
                {
                    int leftValue = list[leftChildIdx];
                    if (parentValue < leftValue)
                    {
                        list[parentIdx] = leftValue;
                        list[leftChildIdx] = parentValue;
                        parentIdx = leftChildIdx;
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
                break; //자식이 없으면 종료
            }

            return maxValue;
        }
    }

    static void TestMaxHeap()
    {
        int commandCount = int.Parse(Console.ReadLine());
        MaxHeap heap = new();
        for (int i = 0; i < commandCount; i++)
        {
            int command = int.Parse(Console.ReadLine());
            if (command == 0)
            {
                Console.WriteLine(heap.Deque());
                continue;
            }
            heap.Enque(command);
        }
    }
    #endregion

    #region 힙 구현하기

    static void TestAbsHeap()
    {
        int commandCount = int.Parse(Console.ReadLine());
        AbsHeap minHeap = new();
        StringBuilder sb = new();
        for (int i = 0; i < commandCount; i++)
        {
            long command = long.Parse(Console.ReadLine());
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

    class AbsHeap
    {


        //트리를 배열로 구현 - 모든 노드 정보가 배열에 표현(인덱스가 보유) 
        // 내 자식을 찾아가려면 인덱스가 필요
        //인덱스 규칙만으로 자식, 부모가 산출되어야 한다. 
        // + 중간 노드가 비는 경우가 발생할 수 있는데, 힙은 완전트리로서 중간에 비는게 없어서 배열로 해도 낭비가 없다.
        private List<long> _tree = new();

        //Peek
        //입력 : 없음
        //출력 : 최소 원소 -> 루트 노드 값 -> 0번
        public long Peek()
        {
            return _tree[0];
        }

        //Enqueue
        public void Enqueue(long newValue)
        {
            //1. 맨 끝에 넣는다
            _tree.Add(newValue);

            //2. 부모랑 비교해서 자식 크기가 작으면, 부모랑 바꾼다. 
            // -> 위를 반복한다. 
            int childIdx = _tree.Count - 1;
            while (childIdx != 0)
            {
                int parentIdx = ((childIdx - 1) / 2);
                long parentValue = _tree[parentIdx];
                long childValue = _tree[childIdx];


                if (IsLeftSmall(parentValue, childValue) == true)
                {
                    return;
                }

                //자식이 더 작으면 부모와 위치를 바꾼다
                _tree[parentIdx] = childValue;
                _tree[childIdx] = parentValue;

                childIdx = parentIdx; //자식 인덱스를 부모 인덱스로 바꾼뒤 계속 진행한다
            }

        }

        //Dequeue
        public long Dequeue()
        {
            if (_tree.Count == 0)
            {
                return 0;
            }
            long minValue = _tree[0];
            int lastIdx = _tree.Count - 1;
            long lastValue = _tree[lastIdx];
            //1. 맨 마지막 원소를 루트노드로 이동하기
            _tree[0] = lastValue; //루트노드에 마지막 값 넣고
            _tree.RemoveAt(lastIdx); //마지막 원소 제거

            //2. 힙불변성 유지 위해 부모로부터 자식과 비교시작
            int parentIdx = 0;
            while (parentIdx < _tree.Count)
            {
                long parentValue = _tree[parentIdx];
                int leftIdx = (parentIdx + 1) * 2 - 1;
                int rightIdx = (parentIdx + 1) * 2;

                //3. 자식 없으면 끝
                if (_tree.Count <= leftIdx)
                {
                    return minValue;
                }
                long leftValue = _tree[leftIdx];
                if (_tree.Count <= rightIdx)
                {
                    //오른쪽 자식 없으면 왼쪽자식하고만 비교
                    if (IsLeftSmall(leftValue, parentValue) == true)
                    {
                        //절댓값으로 먼저 따지고 
                        _tree[leftIdx] = parentValue;
                        _tree[parentIdx] = leftValue;
                        parentIdx = leftIdx;
                        //그리고 탐색 진행
                        continue;
                    }
                    //부모가 더 작으면 종료
                    return minValue;
                }

                long rightValue = _tree[rightIdx];

                if (IsLeftSmall(leftValue, rightValue)) //왼쪽이 더 작은 경우
                {
                    //오른쪽 자식 없으면 왼쪽자식하고만 비교
                    if (IsLeftSmall(leftValue, parentValue) == true)
                    {
                        //절댓값으로 먼저 따지고 
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
                if (IsLeftSmall(rightValue, parentValue) == true)
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

        bool IsLeftSmall(long _left, long _right)
        {
            //절댓값으로 왼쪽이 작으면 왼쪽이 작은거
            if (Math.Abs(_left) < Math.Abs(_right))
            {
                return true;
            }
            //절댓값으론 같다면 실제로 왼쪽값이 작을때 
            else if (Math.Abs(_left) == Math.Abs(_right) && _left < _right)
            {
                return true;
            }
            //절댓값이 왼쪽이 더 크면 false 인거지.
            return false;
        }
    }

    #endregion

    #region 숨바3 13549
    static void RunBro()
    {
        string[] command = Console.ReadLine().Split();
        int subin = int.Parse(command[0]);
        int bro = int.Parse(command[1]);


        PriorityQueue<int, int> pq = new();
        int[] visited = new int[100_005];
        for (int i = 0; i < visited.Length; i++)
        {
            visited[i] = int.MaxValue;
        }
        visited[subin] = 0;
        int f = visited[subin];
        pq.Enqueue(subin, f);
        int[] path = new int[100_005];
        while (pq.Count > 0)
        {
            int current = pq.Dequeue();

            if (current == bro)
            {
                break;
            }

            int[] dx = { -1, 1, current };
            int[] dW = { 1, 1, 0 };
            for (int i = 0; i < 3; i++)
            {
                int nx = current + dx[i];
                if (0 <= nx && nx <= 100000)
                {
                    int weight = visited[current] + dW[i];
                    if (weight < visited[nx])
                    {
                        visited[nx] = weight;
                        path[nx] = current;
                        pq.Enqueue(nx, weight);
                    }
                }
            }
        }
        Console.WriteLine(visited[bro]);
        //int cur = bro;
        //while (true)
        //{
        //    int pre = path[cur];
        //    Console.WriteLine(pre);
        //    if(pre == 0)
        //    {
        //        break;
        //    }
        //    cur = pre;
        //}
    }

    static int BroHue(int _cur, int _bro)
    {
        return Math.Abs(_bro - _cur);
    }
    #endregion

    #region 요원 9370
    static void Hidden()
    {
        int testCase = int.Parse(Console.ReadLine());
        for (int z = 0; z < testCase; z++)
        {
            string[] commandStr = Console.ReadLine().Split();
            int nodeCount = int.Parse(commandStr[0]); // 노드 수
            int lineCount = int.Parse(commandStr[1]); //간선 수
            int guessCount = int.Parse(commandStr[2]); //후보 수
            string[] infoStr = Console.ReadLine().Split();
            int startNode = int.Parse(infoStr[0]); //예술가 출발지
            int passNode1 = int.Parse(infoStr[1]); //건너간 노드
            int passNode2 = int.Parse(infoStr[2]); //건너간 노드
            List<int[]>[] graph = new List<int[]>[nodeCount + 1]; //각 노드에서 다른노드까지 간선, 비중 저장
            for (int x = 0; x < nodeCount + 1; x++)
            {
                graph[x] = new List<int[]>();
            }

            for (int lineInfoIdx = 0; lineInfoIdx < lineCount; lineInfoIdx++)
            {
                string[] lineStr = Console.ReadLine().Split();
                int lineStartNode = int.Parse(lineStr[0]);
                int lineEndNode = int.Parse(lineStr[1]);
                int lineWeight = int.Parse(lineStr[2]);

                //양방향 정보 입력
                int[] goal = { lineEndNode, lineWeight };
                graph[lineStartNode].Add(goal);
                int[] back = { lineStartNode, lineWeight };
                graph[lineEndNode].Add(back);
            }
            List<int> guessList = new List<int>();
            for (int guessInfo = 0; guessInfo < guessCount; guessInfo++)
            {
                int guessNode = int.Parse(Console.ReadLine());
                guessList.Add(guessNode);
            }

            //출발에서 먼저 가야하는 노드들 
            //후보까지 가는 루트에서 pass1과 2를 포함하는 후보들

            //int[] route = MinRoute(graph, startNode);
            //PriorityQueue<int, int> rank = new();
            //for (int i = 0; i < guessList.Count; i++)
            //{
            //    int goal = guessList[i];
            //    int back = route[goal];
            //    bool[] find = { false, false};
            //    int[] haveTo = { passNode1, passNode2 };
            //    while (back != 0)
            //    {
            //        if(back == haveTo[0])
            //        {
            //            find[0] = true;
            //        }
            //        else if(back == haveTo[1])
            //        {
            //            find[1] = true;
            //        }
            //        if (find[0] == true && find[1] == true)
            //        {
            //            //2경로를 지나쳤으면
            //            rank.Enqueue(goal, goal);
            //            break;
            //        }
            //        back = route[back];
            //    }
            //}

            int[] minDisA = MinDistance(graph, startNode);
            int[] minDis1 = MinDistance(graph, passNode1);
            int[] minDis2 = MinDistance(graph, passNode2);
            List<int[]> pass = graph[passNode1];
            int passDistance = 0;
            PriorityQueue<int, int> rank = new();
            for (int i = 0; i < pass.Count; i++)
            {
                if (pass[i][0] == passNode2)
                {
                    passDistance = pass[i][1];
                    break;
                }
            }
            for (int i = 0; i < guessList.Count; i++)
            {
                int goal = guessList[i];
                int passDis = passDistance;
                int route1 = minDisA[passNode1] + minDis2[goal] + passDis;
                int route2 = minDisA[passNode2] + minDis1[goal] + passDis;
                if (route1 == minDisA[goal] || route2 == minDisA[goal])
                {
                    rank.Enqueue(goal, goal);
                }
            }


            StringBuilder sb = new();
            while (rank.Count > 0)
            {
                sb.Append(rank.Dequeue().ToString() + " ");
            }
            sb.Remove(sb.Length - 1, 1);
            Console.WriteLine(sb);
        }
    }

    static int[] MinDistance(List<int[]>[] _graph, int _start)
    {
        int[] distanceInfo = new int[_graph.Length];
        int[] routeInfo = new int[_graph.Length];
        //시작점으로부터 다른 노드까지의 모든 거리를 반환
        for (int i = 0; i < distanceInfo.Length; i++)
        {
            distanceInfo[i] = int.MaxValue;
        }
        distanceInfo[_start] = 0;
        PriorityQueue<int, int> pq = new();
        pq.Enqueue(_start, 0);
        while (pq.Count > 0)
        {
            int current = pq.Dequeue();

            List<int[]> nextNodeList = _graph[current];//현재 노드에서 갈수있는 다른 노드
            for (int i = 0; i < nextNodeList.Count; i++)
            {
                int nextNode = nextNodeList[i][0]; // [][] [0]은 노드 번호 [1]은 거리
                int nextWeight = distanceInfo[current] + nextNodeList[i][1]; //현재 루트를 지날때 다음노드의 가중치
                if (nextWeight < distanceInfo[nextNode])
                {
                    //현재 노드를 지나는게 더 짧은 거리라면
                    distanceInfo[nextNode] = nextWeight;
                    routeInfo[nextNode] = current; //지나온 족적 기록
                    pq.Enqueue(nextNode, nextWeight);
                }
            }
        }
        return distanceInfo;
    }
    #endregion

    #region babyShark
    static void BabyShark()
    {
        int mapSize = int.Parse(Console.ReadLine());
        int[,] map = new int[mapSize, mapSize];
        int sharkY = 0;
        int sharkX = 0;
        for (int i = 0; i < mapSize; i++)
        {
            string[] mapInfo = Console.ReadLine().Split();
            for (int x = 0; x < mapSize; x++)
            {
                int info = int.Parse(mapInfo[x]);
                map[i, x] = info;
                if (info == 9)
                {
                    sharkY = i;
                    sharkX = x;
                }
            }
        }

        int sharkSize = 2;

        int findFish = 1;
        int passTime = 0;
        int needFood = sharkSize;
        while (findFish != 0)
        {
            List<(int, int, int)> fishiList = EatBfs(map, sharkX, sharkY, sharkSize, mapSize);
            if (fishiList.Count == 0)
            {
                //먹을 물고기 없으면 끝
                break;
            }

            fishiList.Sort((a, b) =>
            {
                //거리순 정렬
                int compare = a.Item3.CompareTo(b.Item3);
                //높이순 정렬
                if (compare == 0)
                {
                    compare = a.Item1.CompareTo(b.Item1);
                }
                //했는데도 0이면
                if (compare == 0)
                {
                    //왼쪽순 정렬
                    compare = a.Item2.CompareTo(b.Item2);
                }
                return compare;
            });
            //0번째 물고기를 먹을거야
            int distance = fishiList[0].Item3;
            int y = fishiList[0].Item1;
            int x = fishiList[0].Item2;
            passTime += distance;
            needFood -= 1;
            if (needFood == 0)
            {
                sharkSize += 1;
                needFood = sharkSize;
            }
            map[y, x] = 9;
            map[sharkY, sharkX] = 0;
            sharkY = y;
            sharkX = x;
        }
        Console.WriteLine(passTime);
    }


    static List<(int, int, int)> EatBfs(int[,] _map, int _startX, int _startY, int _sharkSize, int _mapSize)
    {
        int[,] visited = new int[_mapSize, _mapSize];
        Queue<(int, int, int)> sq = new();
        sq.Enqueue((_startY, _startX, 0));
        int sharkSize = _sharkSize;
        int eatPoint = _sharkSize;
        visited[_startY, _startX] = 1;
        List<(int, int, int)> fishList = new(); //먹을수 있는 목록
        while (sq.Count > 0)
        {

            (int, int, int) curInfo = sq.Dequeue();

            int curY = curInfo.Item1;
            int curX = curInfo.Item2;
            int dis = curInfo.Item3;
            int[] dy = { -1, 0, 0, 1 };
            int[] dx = { 0, -1, 1, 0 };
            for (int i = 0; i < 4; i++)
            {
                int ny = curY + dy[i];
                int nx = curX + dx[i];
                //Console.WriteLine("다음 위치"+ny + " : "+nx);
                if (ny < 0 || nx < 0 || _mapSize <= ny || _mapSize <= nx)
                {
                    // Console.WriteLine("논외");
                    continue; //벗어난 위치
                }

                if (visited[ny, nx] != 0)
                {
                    // Console.WriteLine("방문지");
                    continue;
                }

                if (sharkSize < _map[ny, nx])
                {
                    //Console.WriteLine("나보다 큰 물고기");
                    continue;
                }

                if (0 < _map[ny, nx] && _map[ny, nx] < sharkSize)
                {
                    (int, int, int) fishInfo = (ny, nx, dis + 1);
                    fishList.Add(fishInfo);
                }

                visited[ny, nx] = 1; //방문했다 체크하고
                sq.Enqueue((ny, nx, dis + 1));

            }



        }

        return fishList;
    }
    #endregion

    #region 경로찾기 11403
    static void FindOnePath()
    {
        //일방향 간선

        int nodeCount = int.Parse(Console.ReadLine());
        List<int>[] graph = new List<int>[nodeCount];
        for (int i = 0; i < nodeCount; i++)
        {
            List<int> able = new();
            string[] command = Console.ReadLine().Split();
            // able.Add(i);//자기자신은 언제나 갈수있음
            for (int c = 0; c < command.Length; c++)
            {
                if (int.Parse(command[c]) == 1)
                {
                    able.Add(c); //갈수있는경로로 추가
                }
            }
            graph[i] = able;
        }
        //그래프 그려놨으니 이제 갈수있는거 판단해서 하면됨.
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < nodeCount; i++)
        {
            sb.Clear();
            int[] havePath = BfsOnePath(graph, i);
            for (int x = 0; x < havePath.Length; x++)
            {
                sb.Append(havePath[x] + " ");
            }
            sb.Remove(sb.Length - 1, 1);
            Console.WriteLine(sb);
        }

    }

    static int[] BfsOnePath(List<int>[] graph, int start)
    {
        int[] pathRecord = new int[graph.Length];
        Queue<int> nodeQ = new();
        nodeQ.Enqueue(start);
        while (nodeQ.Count > 0)
        {
            int current = nodeQ.Dequeue();
            List<int> pathList = graph[current];
            for (int i = 0; i < pathList.Count; i++)
            {
                int next = pathList[i];
                if (pathRecord[next] == 0)
                {
                    pathRecord[next] = 1;
                    nodeQ.Enqueue(next);
                }
            }
        }

        return pathRecord;
    }
    #endregion

    #region MinBus 11404
    static void FindCheapBus()
    {
        int cityCount = int.Parse(Console.ReadLine());
        int busCount = int.Parse(Console.ReadLine());
        Dictionary<int, int>[] busGraph = new Dictionary<int, int>[cityCount];
        for (int i = 0; i < cityCount; i++)
        {
            busGraph[i] = new Dictionary<int, int>();
        }

        for (int i = 0; i < busCount; i++)
        {
            string[] busInfo = Console.ReadLine().Split();
            int startCity = int.Parse(busInfo[0]) - 1;
            int endCity = int.Parse(busInfo[1]) - 1;
            int pay = int.Parse(busInfo[2]);

            if (busGraph[startCity].ContainsKey(endCity))
            {
                //있으면 낮은값으로
                busGraph[startCity][endCity] = Math.Min(busGraph[startCity][endCity], pay);
                continue;
            }
            busGraph[startCity].Add(endCity, pay);
            //시작과 도시가 같은 버스는 없다. 
        }

        string[] questStr = Console.ReadLine().Split();
        int start = int.Parse(questStr[0]);
        int end = int.Parse(questStr[1]);

        int[] payinfo = FindBfs(busGraph, start - 1);
        Console.WriteLine(payinfo[end - 1]);

    }

    static int[] FindBfs(Dictionary<int, int>[] _graph, int _start)
    {
        int[] payInfo = new int[_graph.Length];
        for (int i = 0; i < payInfo.Length; i++)
        {
            payInfo[i] = int.MaxValue; //최대 요금으로 설정
        }
        payInfo[_start] = 0; //현재 위치는 0원
        //갈수 있는 경로중 제일 싼걸로 골라서 선택
        PriorityQueue<int, int> nextQ = new();
        nextQ.Enqueue(_start, 0);
        while (nextQ.Count > 0)
        {
            int current = nextQ.Dequeue();
            Dictionary<int, int> nextPathList = _graph[current];
            foreach (KeyValuePair<int, int> item in nextPathList)
            {
                int nextNode = item.Key;
                int nextPay = payInfo[current] + item.Value;
                if (nextPay < payInfo[nextNode])
                {
                    payInfo[nextNode] = nextPay; //제일 싼 비용으로 갱신
                    nextQ.Enqueue(nextNode, nextPay); //갈 노드와, 그 값을 넣는다.
                }
            }

        }

        return payInfo;
    }
    #endregion

    #region 돈뽑기
    static void Withdraw()
    {
        int count = int.Parse(Console.ReadLine());
        string[] command = Console.ReadLine().Split();
        int[] pTime = new int[count];
        PriorityQueue<int, int> pq = new();
        for (int i = 0; i < pTime.Length; i++)
        {
            pTime[i] = int.Parse(command[i]);
            pq.Enqueue(pTime[i], pTime[i]);

        }

        int totalTime = 0;
        int preTime = 0;
        while (pq.Count > 0)
        {
            int thisTime = pq.Dequeue();
            preTime += thisTime;
            totalTime += preTime;
            /* 0
             * 1 = 1
             * 1 + 2 
             * 1 + 2 + 3
             * 1 + 2 + 3 + 3
             * 1 + 2 + 3 + 3 + 4
             */

        }
        Console.WriteLine(totalTime);
        //기다리는 시간이 가장 낮게
        // 1 2 3 4 5 
        // 1은 1시간
        // 2는 1 + 2 시간
        // 3은 1 + 2 + 3 시간
        // 즉 앞에 짧은걸 넣을수록 기다리는 시간이 줄어드는듯?

    }
    #endregion
    #region n번째영화 1436
    static void MovieName()
    {
        //666이 들어가야한다 
        int n = int.Parse(Console.ReadLine());
        int find = 0;
        int start = 1;
        while (find < n)
        {
            //숫자에 666이 들어가는건 어떻게 알 수 있나
            string name = start.ToString();

            if (name.Length >= 3)
            {
                int count = 0;
                for (int i = 0; i < name.Length; i++)
                {
                    if (name[i] == '6')
                    {
                        count += 1;
                        if (count == 3)
                        {

                            find += 1;
                            if (find == n)
                            {
                                Console.WriteLine(name);
                            }
                        }
                    }
                    else
                    {
                        count = 0;
                    }
                }
            }
            start += 1;
        }
    }
    #endregion
    #region N개의수업 11000
    static void ArrangeRoom()
    {
        int classCount = int.Parse(Console.ReadLine());
        List<int[]> classTime = new List<int[]>();
        for (int i = 0; i < classCount; i++)
        {
            string[] timeStr = Console.ReadLine().Split();
            int start = int.Parse(timeStr[0]);
            int end = int.Parse(timeStr[1]);
            classTime.Add(new int[2] { start, end });
        }
        //위 수업을 모두 배치하기 위해 필요한 강의실의 최소 수는?
        //시작 시간으로 모두 정렬해야되지 않을까
        classTime.Sort((a, b) => a[0].CompareTo(b[0]));
        //오브젝트 풀 처럼 해야겠따
        PriorityQueue<int, int> backTimeQ = new(); //강의 종료시간 제일 빨리끝나는걸로 담아놓기
        int makeCount = 0;
        for (int i = 0; i < classTime.Count; i++)
        {
            int startTime = classTime[i][0]; //강의 시작시간
            int endTime = classTime[i][1];
            if (backTimeQ.TryPeek(out int resetTime, out int prior))
            {
                if (resetTime <= startTime)
                {
                    //금방 끝나는애가 시작하려는 시간보다 앞서면 해당 강의실 쓰면됨
                    backTimeQ.Dequeue(); //빼고
                    backTimeQ.Enqueue(endTime, endTime); //새로 집어넣고
                    continue;// 다음넘어가고
                }
                else
                {
                    //젤먼저 끝나는애가 시작시간보다 뒤면
                    //꺼낼게 없으면
                    makeCount += 1; //만든횟수 넣고
                    backTimeQ.Enqueue(endTime, endTime); //종료시간으로 큐집어넣기
                    continue;
                }
            }
            else
            {
                //꺼낼게 없으면
                makeCount += 1; //만든횟수 넣고
                backTimeQ.Enqueue(endTime, endTime); //종료시간으로 큐집어넣기
            }
        }
        Console.WriteLine(makeCount);
    }
    #endregion
    #region 파티 1238
    static void PartyPath()
    {
        string[] command = Console.ReadLine().Split();
        int nodeCount = int.Parse(command[0]);
        int lineCount = int.Parse(command[1]);
        int place = int.Parse(command[2]);
        List<(int, int)>[] graph = new List<(int, int)>[nodeCount + 1];
        List<(int, int)>[] reverseGraph = new List<(int, int)>[nodeCount + 1];
        for (int i = 0; i < nodeCount + 1; i++)
        {
            graph[i] = new List<(int, int)>();
            reverseGraph[i] = new List<(int, int)>();
        }
        for (int i = 0; i < lineCount; i++)
        {
            string[] lineStr = Console.ReadLine().Split();
            int depart = int.Parse(lineStr[0]);
            int arrive = int.Parse(lineStr[1]);
            int weight = int.Parse(lineStr[2]);
            graph[depart].Add((arrive, weight));
            reverseGraph[arrive].Add((depart, weight));
        }
        //그래프 그려놓음.
        int[] max = new int[nodeCount + 1];
        //for (int i = 1; i <= nodeCount; i++)
        //{
        //    int distance = (DajkiPartyPath(graph, i))[place];
        //    max[i] += distance; //i마을에서 출발하면서 최단거리구함
        //}
        max = DajkiPartyPath(reverseGraph, place);
        int[] back = DajkiPartyPath(graph, place);
        int answer = 0;
        for (int i = 1; i <= nodeCount; i++)
        {
            int final = back[i] + max[i];
            answer = Math.Max(answer, final);
        }
        Console.WriteLine(answer);
    }

    static int[] DajkiPartyPath(List<(int, int)>[] graph, int _start)
    {
        int[] distance = new int[graph.Length];
        for (int i = 0; i < distance.Length; i++)
        {
            distance[i] = int.MaxValue;
        }
        distance[_start] = 0;
        PriorityQueue<int, int> pq = new();
        pq.Enqueue(_start, 0);
        while (pq.Count > 0)
        {
            int current = pq.Dequeue();
            List<(int, int)> info = graph[current];
            for (int i = 0; i < info.Count; i++)
            {
                int next = info[i].Item1;
                int weight = distance[current] + info[i].Item2;
                if (weight < distance[next])
                {
                    distance[next] = weight;
                    pq.Enqueue(next, weight);
                }
            }
        }
        return distance;
    }
    #endregion

    #region N퀸문제
    static void IamQuenn()
    {
        int quest = int.Parse(Console.ReadLine());
        //quest 너비의 판에서 quest개의 퀸을 공격할 수 없게 놓을 수 있는 방법 
        //직선, 대각모두 - 모든 라인에 하나씩만 존재가능 첫째줄에 놓았으면 n+1부터는 앞에줄 다 패스해서 놓고
        List<(int, int)> chessPos = new();
        RecursionChess(0, quest, chessPos);
        Console.WriteLine(find);
    }

    static int find = 0;
    static void RecursionChess(int _line, int _length, List<(int, int)> chess)
    {
        if (_line == _length)
        {
            //만약 최종까지 넘어왔으면 해당 로직은 된거
            find += 1;

            return;
        }

        for (int i = 0; i < _length; i++)
        {
            if (AbleQueen(_line, i, chess))
            {
                chess.Add((_line, i));
                RecursionChess(_line + 1, _length, chess); //다음줄에 가서도 확인하도록 진행
                //만약 위 위치를 돌고 나왔으면 이제 마지막 자기가 넣은건 빼기
                chess.RemoveAt(chess.Count - 1);
            }

        }
    }
    /*
     * [1][0][0][0][0][0][0][0]
     * [0][0][1][0][0][0][0][0]
     * [0][0][0][0][1][0][0][0]
     * [0][1][0][0][0][0][0][0]
     * [1][0][0][0][0][0][0][0] 
     * [1][0][0][0][0][0][0][0]
     * [1][0][0][0][0][0][0][0]
     * [1][0][0][0][0][0][0][0]
     */
    static bool AbleQueen(int _checkY, int _checkX, List<(int, int)> _chessList)
    {

        for (int i = 0; i < _chessList.Count; i++)
        {
            int preY = _chessList[i].Item1; // 놓여있는 줄 y
            int preX = _chessList[i].Item2; //놓여있는 x
            //체크하려는 위치가 이전 위치와 걸리면 false 반환
            if (preY == _checkY || preX == _checkX)
            {
                return false;
            }

            //대각으로도 같으면 
            if (Math.Abs(preY - _checkY) == Math.Abs(preX - _checkX))
            {
                //기울기가 1이면 같은 대각라인
                return false;
            }

        }


        return true;
    }
    #endregion

    #region K번째 1854
    static void KList()
    {
        string[] command = Console.ReadLine().Split();

        int nodeCount = int.Parse(command[0]);
        int lineCount = int.Parse(command[1]);
        int k = int.Parse(command[2]);
        List<(int, int)>[] graph = new List<(int, int)>[nodeCount + 1];
        for (int i = 0; i < graph.Length; i++)
        {
            graph[i] = new();
        }
        for (int i = 0; i < lineCount; i++)
        {
            string[] lineStr = Console.ReadLine().Split();
            int start = int.Parse(lineStr[0]);
            int end = int.Parse(lineStr[1]);
            int weight = int.Parse(lineStr[2]);
            graph[start].Add((end, weight));
        }

        PriorityQueue<int, int>[] info = KSearch(graph, 1, k);
        for (int i = 1; i < info.Length; i++)
        {
            PriorityQueue<int, int> pq = info[i];
            if (pq.TryDequeue(out int value, out _))
            {
                Console.WriteLine(value);
            }
            else
            {
                Console.WriteLine(-1);
            }
        }
    }

    static PriorityQueue<int, int>[] KSearch(List<(int, int)>[] _graph, int _start, int _k)
    {
        PriorityQueue<int, int>[] priVisited = new PriorityQueue<int, int>[_graph.Length];
        for (int i = 0; i < priVisited.Length; i++)
        {
            priVisited[i] = new PriorityQueue<int, int>();
        }
        priVisited[1].Enqueue(0, 0);
        Queue<(int, int)> q = new();
        q.Enqueue((_start, 0));
        while (q.Count > 0)
        {
            (int, int) curK = q.Dequeue();
            int curN = curK.Item1;
            int curD = curK.Item2;

            List<(int, int)> nextList = _graph[curN];
            for (int i = 0; i < nextList.Count; i++)
            {
                int nextN = nextList[i].Item1;
                int nextWeight = nextList[i].Item2;
                int newWeight = nextWeight + curD;
                if (priVisited[nextN].Count < _k)
                {
                    priVisited[nextN].Enqueue((newWeight), -(newWeight));
                    q.Enqueue((nextN, newWeight));
                }
                else if (priVisited[nextN].Peek() > newWeight)
                {

                    priVisited[nextN].Dequeue();
                    priVisited[nextN].Enqueue((newWeight), -(newWeight));
                    q.Enqueue((nextN, newWeight));
                }
            }
        }
        return priVisited;
    }
    #endregion


    #region 스도쿠2580
    static void Sdoku()
    {
        int[,] map = new int[9, 9];
        for (int i = 0; i < 9; i++)
        {
            string[] command = Console.ReadLine().Split();
            for (int x = 0; x < 9; x++)
            {
                map[i, x] = int.Parse(command[x]);
            }
        }

        //0번인 칸에 들어갈 수 있는 조합을 적어놓고, 
        //n칸에 숫자를 하나씩 넣으면서 다른조합이 되는지 봐서 
        List<SdokuRecord> questList = new();
        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                if (map[y, x] == 0)
                {
                    List<int> need = new();
                    SdokuRecord st = new SdokuRecord(y, x, need);
                    questList.Add(st);
                    CheckAble(map, y, x, st.ableList);
                }
            }
        }

        bool isDone = false;
        SdokuRecursion(map, questList, 0, ref isDone);


    }

    struct SdokuRecord
    {
        public int y;
        public int x;
        public List<int> ableList;

        public SdokuRecord(int _y, int _x, List<int> _list)
        {
            y = _y; x = _x;
            ableList = _list;
        }

    }

    static void SdokuRecursion(int[,] _map, List<SdokuRecord> _needList, int _curIdx, ref bool _isDone)
    {
        //해당번째 아이템을 넣어서 아이템이 진행되는지 - 마지막 경우까지 들어갔다면 완료 된거
        if (_needList.Count == _curIdx && _isDone == false)
        {
            _isDone = true; //접근한 최초 한번만 그림그리도록
            //모든 리스트의 값들을 다 넣을 수 있었다. 답이 채워졌다.
            StringBuilder sb = new();
            for (int i = 0; i < 9; i++)
            {
                for (int x = 0; x < 9; x++)
                {
                    sb.Append(_map[i, x] + " ");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.AppendLine();
            }
            sb.Remove(sb.Length - 1, 1);

            Console.WriteLine(sb);
            return;
        }

        SdokuRecord curRecord = _needList[_curIdx];
        List<int> checkNum = curRecord.ableList;
        for (int i = 0; i < checkNum.Count; i++)
        {
            int inputNum = checkNum[i];
            int posY = curRecord.y;
            int posX = curRecord.x;
            //그 빈칸에 이제 인풋넘버를 넣을꺼야 넣었을때 다른거랑 만족하는지 보는거야 
            if (CheckInput(_map, posY, posX, inputNum))
            {
                //넣을 수 있으면 넣고
                _map[posY, posX] = inputNum;
                //다음 진행
                SdokuRecursion(_map, _needList, _curIdx + 1, ref _isDone);
            }
            //넣었던 답 넣엇던 말던 다시 되돌리고 다른거 체크하기
            _map[posY, posX] = 0;

        }
    }

    static bool CheckInput(int[,] _map, int _y, int _x, int _inputNum)
    {
        //해당 위치에 해당 넘버를 넣었을때 다른거랑 겹치는지만 체크
        //가로줄 체크
        for (int i = 0; i < 9; i++)
        {
            int haveNum = _map[_y, i];
            if (haveNum == _inputNum)
            {
                return false;
            }
        }
        //세로줄 체크
        for (int i = 0; i < 9; i++)
        {
            int haveNum = _map[i, _x];
            if (haveNum == _inputNum)
            {
                return false;
            }
        }
        //구역 체크
        int startY = _y / 3;
        startY *= 3;
        int startX = _x / 3;
        startX *= 3;
        for (int y = startY; y < startY + 3; y++)
        {
            for (int x = startX; x < startX + 3; x++)
            {
                int haveNum = _map[y, x];
                if (haveNum == _inputNum)
                {
                    return false;
                }
            }
        }
        return true;
    }

    static void CheckAble(int[,] _map, int _y, int _x, List<int> _record)
    {
        bool[] ableGaro = new bool[10];
        //가로줄 확인
        bool[] have = new bool[10];
        for (int i = 0; i < 9; i++)
        {
            int haveNum = _map[_y, i];
            have[haveNum] = true;
        }
        for (int i = 1; i <= 9; i++)
        {
            if (have[i] == false)
            {
                ableGaro[i] = true;
            }
        }

        //세로줄 확인
        bool[] haveSero = new bool[10];
        bool[] ableSero = new bool[10];
        for (int i = 0; i < 9; i++)
        {
            int haveNum = _map[i, _x];
            haveSero[haveNum] = true;
        }
        for (int i = 1; i <= 9; i++)
        {
            if (haveSero[i] == false)
            {
                ableSero[i] = true;
            }
        }

        //구역 확인
        bool[] haveQuack = new bool[10];
        bool[] ableQuack = new bool[10];
        //0~8이고 시작은 0, 3, 6
        int startY = _y / 3;
        startY *= 3;
        int startX = _x / 3;
        startX *= 3;
        for (int y = startY; y < startY + 3; y++)
        {
            for (int x = startX; x < startX + 3; x++)
            {
                int haveNum = _map[y, x];
                haveQuack[haveNum] = true;
            }
        }
        for (int i = 1; i <= 9; i++)
        {
            if (haveQuack[i] == false)
            {
                ableQuack[i] = true;
            }
        }


        for (int i = 1; i <= 9; i++)
        {
            if (ableGaro[i] == true && ableSero[i] == true && ableQuack[i] == true)
            {
                _record.Add(i);
            }
        }
    }

    #endregion

    #region 백팩 12865
    static void BackPack()
    {
        string[] command = Console.ReadLine().Split();
        int count = int.Parse(command[0]);
        int k = int.Parse(command[1]);
        int[][] info = new int[count][];
        for (int i = 0; i < count; i++)
        {
            string[] infoStr = Console.ReadLine().Split();
            int weight = int.Parse(infoStr[0]);
            int value = int.Parse(infoStr[1]);
            info[i] = new int[] { weight, value };
        }
        //정보 입력은 되엇고
        //이제 k값 이내에 저걸해야하는데
        // Array.Sort(info, (a, b) => b[1].CompareTo(a[1]));

        int curValue = 0;
        int maxValue = 0;
        int[] sumInfo = new int[count]; //해당 품목부터 뒤의 품목의 모든 합 -> 만약 뒤에껄 다 넣는다 해도 못이기면 탐색안하게
        sumInfo[count - 1] = info[count - 1][1]; //맨 끝에 넣어넣고
        int[] weightInfo = new int[count];
        weightInfo[count - 1] = info[count - 1][0];
        for (int i = count - 2; i >= 0; i--)
        {
            sumInfo[i] = sumInfo[i + 1] + info[i][1];
            weightInfo[i] = weightInfo[i + 1] + info[i][0];
        }
        //Put(info, sumInfo, weightInfo, k, 0, ref curValue, ref maxValue);
        PutDp(info, k);
    }

    static void PutDp(int[][] _info, int _k)
    {
        //k만큼 넣을거야
        int[] kRecord = new int[_k + 1];
        for (int i = 0; i < _info.GetLength(0); i++)
        {
            int weight = _info[i][0];
            int value = _info[i][1];
            //이제 넣을거야
            //공간 kRecord에 하나씩 넣은걸 기록해갈것
            for (int idx = _k; idx >= weight; idx--)
            {
                //해당 무게에 weight만큼 넣을테니 이전기록 [_k-weight]의 최대무게에 내껄 더하면된다.
                kRecord[idx] = Math.Max(kRecord[idx], kRecord[idx - weight] + value);
                //그러면 처음에 넣은애의 무게는 무게만큼 공간이 채워지고. 
                //그다음 넣을 애의 무게도 넣을 수있는 테이블에서 계속 더해지면서 진행
            }
        }

        int maxValue = 0;
        for (int i = 0; i < kRecord.Length; i++)
        {
            maxValue = Math.Max(maxValue, kRecord[i]);
        }
        Console.WriteLine(maxValue);
    }

    static void Put(int[][] _info, int[] _sumInfo, int[] _weightInfo, int _rest, int _curIdx, ref int _curValue, ref int _maxValue)
    {
        if (_info.GetLength(0) == _curIdx)
        {
            //인덱스 밖이면 넘기고
            return;
        }

        int weight = _info[_curIdx][0];
        int value = _info[_curIdx][1];

        if (_maxValue >= _curValue + _sumInfo[_curIdx])
        {
            // 이후의모든걸 넣어도 최댓값을 못넘거나
            return;
        }
        if (_rest >= _weightInfo[_curIdx])
        {
            //남은거 다 넣을만큼 공간이 있으면
            _maxValue = Math.Max(_maxValue, (_curValue + _sumInfo[_curIdx]));  //현재부터 다더한값 넣고 계산뒤 패스

            return;
        }

        //자기를 넣거나
        if (_rest >= weight)
        {
            _curValue += value;
            _rest -= weight;
            Put(_info, _sumInfo, _weightInfo, _rest, _curIdx + 1, ref _curValue, ref _maxValue);
            //다 넣고 나면
            _maxValue = Math.Max(_curValue, _maxValue);
            //자기 정보 복구
            _curValue -= value;
            _rest += weight;
        }
        //자기를 빼고 가거나
        Put(_info, _sumInfo, _weightInfo, _rest, _curIdx + 1, ref _curValue, ref _maxValue);
        _maxValue = Math.Max(_curValue, _maxValue);
    }
    #endregion

    #region 전기선라인 2565
    static void SparkLine()
    {
        int count = int.Parse(Console.ReadLine());
        List<(int, int)> lineList = new();
        for (int i = 0; i < count; i++)
        {
            string[] lineStr = Console.ReadLine().Split();
            int left = int.Parse(lineStr[0]);
            int right = int.Parse(lineStr[1]);
            lineList.Add((left, right));
        }

        //왼쪽 오른쪽 정보가
        //lineList에 들어가있고. 
        //하나씩 하면서 엉키는거 빼면되려나? 
        //다른선을 많이 가리는애를 빼면될것같은데
        lineList.Sort((a, b) => a.Item1.CompareTo(b.Item1));
        //이제 정렬된 item2의 값이 오름차순으로 되게 뽑기만하면되거든. 
        //길이가 1,2,3일때 최댓값을 갱신해준다? 그리고 현재 값을 갱신?
        //갱신되면 최대 길이가 되고 
        int maxCount = 1;
        List<int> answerList = new();
        answerList.Add(lineList[0].Item2); //초기값 넣고
        for (int i = 1; i < lineList.Count; i++)
        {
            int lineValue = lineList[i].Item2; //연결될 녀석
            bool isChange = false;
            for (int x = 0; x < answerList.Count; x++)
            {
                if (answerList[x] < lineValue)
                {
                    //기존값보다 크면 
                    continue;//넘어가고
                }
                else
                {
                    //기존값보다 작으면
                    //해당 위치의 최솟값을 이놈으로 대체 
                    answerList[x] = lineValue;
                    isChange = true;
                    break;
                }
            }
            if (isChange == false)
            {
                //교체가 일어나지 않았단건 얘가 제일 큰수라는거
                answerList.Add(lineValue); //줄 연결
            }
        }
        Console.WriteLine(lineList.Count - answerList.Count);
    }
    #endregion

    #region 바이토닉 11054
    static void Bitonic()
    {
        int count = int.Parse(Console.ReadLine());
        string[] question = Console.ReadLine().Split();
        List<int> questList = new();
        for (int i = 0; i < question.Length; i++)
        {
            questList.Add(int.Parse(question[i]));
        } //문제 완성

        //쭈욱 올라가다가 쭈욱 내려가는 가장 큰 길이

        int maxBio = 0;
        for (int g = 0; g < questList.Count; g++)
        {
            int goalIdx = g;
            int goalValue = questList[goalIdx];

            List<int> upList = new();
            List<int> downList = new();
            for (int i = 0; i < goalIdx; i++)
            {
                int addValue = questList[i];
                bool isChange = false;
                if (goalValue <= addValue)
                {

                    continue;
                }
                for (int x = 0; x < upList.Count; x++)
                {

                    if (upList[x] < addValue)
                    {
                        continue;
                    }
                    else
                    {
                        upList[x] = addValue;
                        isChange = true;
                        break;
                    }
                }
                if (isChange == false)
                {
                    upList.Add(addValue);
                }
            }
            int upCount = upList.Count;
            for (int i = goalIdx + 1; i < question.Length; i++)
            {
                int addValue = questList[i];
                bool isChange = false;
                if (goalValue <= addValue)
                {

                    continue;
                }
                for (int x = 0; x < downList.Count; x++)
                {

                    if (downList[x] > addValue)
                    {
                        continue;
                    }
                    else
                    {
                        downList[x] = addValue;
                        isChange = true;
                        break;
                    }
                }
                if (isChange == false)
                {
                    downList.Add(addValue);
                }
            }
            int downCount = downList.Count;

            maxBio = Math.Max(maxBio, downCount + upCount + 1);
        }

        Console.WriteLine(maxBio);
    }
    #endregion


    #region 낮게이동하기
    static void MoveLower()
    {
        string[] command = Console.ReadLine().Split();
        int row = int.Parse(command[0]);
        int col = int.Parse(command[1]);
        int[,] map = new int[row, col];
        int[,] record = new int[row, col];
        for (int i = 0; i < row; i++)
        {
            string[] lineStr = Console.ReadLine().Split();
            for (int c = 0; c < col; c++)
            {
                map[i, c] = int.Parse(lineStr[c]);
                record[i, c] = -1;
            }
        } //맵핑

        //0,0 에서부터 해당 위치의 숫자보다 낮은 위치로 이동해서, row-1, c-1 위치에 도달하는 경우의 수 구하기

        record[0, 0] = 0;
        LowSearch(map, record, 0, 0, row, col);
        Console.WriteLine(record[0, 0]);

    }


    static int LowSearch(int[,] _map, int[,] _record, int _curR, int _curC, int _maxR, int _maxC)
    {
        int curR = _curR;
        int curC = _curC;
        int curValue = _map[curR, curC];

        int[] dc = { -1, 1, 0, 0 };
        int[] dr = { 0, 0, -1, 1 };
        for (int i = 0; i < 4; i++)
        {
            int nR = curR + dr[i];
            int nC = curC + dc[i];

            if (nR < 0 || _maxR <= nR || nC < 0 || _maxC <= nC)
            {
                //벗어나는 범위는 탐색 안함
                continue;
            }

            if (curValue <= _map[nR, nC])
            {
                //다음 위치가 현재 위치이상이면 진행안함
                continue;
            }

            if (nR == _maxR - 1 && nC == _maxC - 1)
            {
                _record[curR, curC] += 1;
                continue;
            }

            if (_record[nR, nC] != -1)
            {
                //이미 지나간곳
                _record[curR, curC] += _record[nR, nC];
                continue;
            }

            _record[nR, nC] = 0; //이동하려는곳 기록값 0 으로 초기화하고 
            _record[curR, curC] += LowSearch(_map, _record, nR, nC, _maxR, _maxC);
        }
        return _record[curR, curC];
    }
    #endregion

    #region 동전 보자기
    static void NoDupliDraw()
    {
        string[] command = Console.ReadLine().Split();
        int diceCount = int.Parse(command[0]);
        int goal = int.Parse(command[1]);
        int[] dice = new int[diceCount];
        for (int i = 0; i < diceCount; i++)
        {
            dice[i] = int.Parse(Console.ReadLine());
        }
        //이걸 n개로 만들때의 거시기
        //Array.Sort(dice);
        //int count = goal / dice[0];
        //int answerCount = 0;
        //Draw(dice, 0, count, 0, goal, ref answerCount);
        //Console.WriteLine(answerCount);
        DrawDp(dice, goal);

    }

    static void Draw(int[] _memeber, int _startIdx, int _curDeapth, int _sum, int _goal, ref int _answerCount)
    {
        if (_goal == _sum)
        {
            _answerCount += 1;
            return;
        }
        if (_curDeapth == 0)
        {
            return;
        }
        if (_goal < _sum)
        {
            return;
        }

        for (int i = _startIdx; i < _memeber.Length; i++)
        {
            int drawMemeber = _memeber[i];
            int sum = _sum + drawMemeber;
            Draw(_memeber, i, _curDeapth - 1, sum, _goal, ref _answerCount);
        }
    }

    static void DrawDp(int[] coinValues, int _goal)
    {
        //goal이 될때까지 합을 더한다?
        int[] muchValues = new int[_goal + 1]; //골까지 합산
        muchValues[0] = 1; //첫번째값은 1로 해놔야하나?
        for (int i = 1; i <= _goal; i++)
        {
            int curGoal = i; //현재 만들려는 숫자
            for (int x = 0; x < coinValues.Length; x++)
            {
                int curValue = coinValues[x]; //현재 더하려는 숫자
                if (curGoal - curValue < 0)
                {
                    //인덱스 벗어나면 얘는 못넣는거
                    continue;
                }
                //인덱스 안벗어나면
                muchValues[i] += muchValues[curGoal - curValue]; //얘보다 부족한 값을 만드는 경우의수를 더하기
                //위의 짓을 반복? //1을 만들때 만드는 가짓수는 1이 될꺼고
                //2를 만들때는 2는 [0] + 2가 될거고1은 [1] +1 이니까 그값을 더하면될거고 이래저래?
                //근데 128이떠 이건 같은 조합일때를 제외를 못한거거든..
                //1 + 2 와 2 + 1은 같으니까.
            }
        }
        Console.WriteLine(muchValues[_goal]); //목표값 만드는 경우의수 보면 되기?
    }
    #endregion

    static void Main()
    {
        IsBiGraph();
    }


    static void IsBiGraph()
    {
        int testCase = int.Parse(Console.ReadLine());
        for (int i = 0; i < testCase; i++)
        {
            string[] command = Console.ReadLine().Split();
            int nodeCount = int.Parse(command[0]);
            int lineCount = int.Parse(command[1]);
            List<int>[] graph = new List<int>[nodeCount + 1];

            for (int c = 0; c < graph.Length; c++)
            {
                graph[c] = new List<int>();
            }

            for (int lineIdx = 0; lineIdx < lineCount; lineIdx++)
            {
                string[] lineStr = Console.ReadLine().Split();
                int start = int.Parse(lineStr[0]);
                int end = int.Parse(lineStr[1]);

                graph[start].Add(end);
                graph[end].Add(start); //무방향 그래프
            }

            bool isBi = SearchBi(graph);

            Console.WriteLine(isBi ? "YES" : "NO");
        }
    }
    static bool SearchBi(List<int>[] _graph)
    {
        int[] visited = new int[_graph.Length];

        Queue<int> nextQ = new();
        nextQ.Enqueue(1);
        while (nextQ.Count >0)
        {
            int curNode = nextQ.Dequeue();

            if (visited[curNode] == 0)
            {
                visited[curNode] = 2; //1로 시작
            }

            List<int> nextList = _graph[curNode]; //내가갈곳들
            for (int i = 0; i < nextList.Count; i++)
            {
                int nextNode = nextList[i];
                if (visited[curNode] == visited[nextNode])
                {
                    //가려는 곳이 현재의 값 이상이면 (다른 데서 가기로 찜한곳이므로 순환이됨)
                    //홀수 순환이라면 
                    return false;
                }
                else if (visited[curNode] < visited[nextNode])
                {
                    //짝수 순환이라면 일단 넘김
                    continue;
                }
                if (visited[nextNode]== visited[curNode] - 1)
                {
                    //내가 왔던 곳이면
                    continue; //패쓰
                }
                visited[nextNode] = visited[curNode] + 1;
                nextQ.Enqueue(nextNode);
            }

            if(nextQ.Count == 0)
            {
                //만약 진행중이던 노드의 여정이 끝났으면
                for (int i = 1; i < visited.Length; i++)
                {
                    //방문안한곳을 찾아서 넣기
                    if (visited[i] == 0)
                    {
                        nextQ.Enqueue(i);
                        break;
                    }
                }
            }
        }

        return true;
    }

}


