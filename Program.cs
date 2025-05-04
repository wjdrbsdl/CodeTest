using System.Text;
using System.Collections.Generic;
using System.IO;
using System;

class Pro
{

    #region KeyLog

    class KeyNode
    {
        public char text;
        public KeyNode preKey;
        public KeyNode nextKey;

        public KeyNode(char _text)
        {
            text = _text;
        }
    }

    static int curKCusorPos = 0;
    static KeyNode cursorNode;
    static KeyNode startNode;
    static List<char> curKInputList = new List<char>();
    static void KeyLog()
    {


        //cursorNode = cursor;
        //startNode = cursor;
        //while (true)
        //{
        //    string input = Console.ReadLine();
        //    for (int x = 0; x < input.Length; x++)
        //    {
        //        KeyLodeCommand(input[x]);
        //        Console.WriteLine(KeyNodeConsole());
        //    }
        //}

        int commandCount = int.Parse(Console.ReadLine());
        for (int i = 0; i < commandCount; i++)
        {
            string input = Console.ReadLine();
            KeyNode cursor = new(' ');
            cursorNode = cursor;
            startNode = cursor;
            for (int x = 0; x < input.Length; x++)
            {
                KeyLodeCommand(input[x]);
                //   Console.WriteLine(KeyLogConsoleList());
            }
            Console.WriteLine(KeyNodeConsole());
        }

    }

    static void KeyLodeCommand(char command)
    {
        switch (command)
        {
            case '<':
                //  Console.WriteLine("커서 왼쪽으로 옮기기");
                if (cursorNode.preKey != null)
                {
                    //앞에 있던 노드랑 커서노드랑 자리 바꾸기

                    KeyNode preNode1 = cursorNode.preKey; //커서 앞에 노드
                    KeyNode prepreNode = preNode1.preKey; //앞에 앞에 노드
                    KeyNode backNode = cursorNode.nextKey; //커서 뒤의 노드
                    cursorNode.preKey = null;
                    cursorNode.nextKey = preNode1; //기존 앞에 있던애를 커서의 뒤로 이동
                    if (prepreNode != null)
                    {
                        //앞에있던 노드가 맨 앞이 아니였으면 
                        cursorNode.preKey = prepreNode; //커서노드의 앞에 전달해주고
                        prepreNode.nextKey = cursorNode; //재연결하고
                    }
                    else
                    {
                        //만약 맨 앞의 노드였다면
                        startNode = preNode1; //걔가 스타트 노드였던것. 
                    }

                    //앞에 있던 노드의 앞은 커서노드로 갱신해주고
                    preNode1.preKey = cursorNode;
                    preNode1.nextKey = null; //뒤도 연결해줄건데
                    if (backNode != null)
                    {
                        //만약 null 이 아니면
                        preNode1.nextKey = backNode;
                        backNode.preKey = preNode1; //서로 연결
                    }
                }
                break;
            case '>':
                //   Console.WriteLine("커서 오른쪽으로 옮기기");
                if (cursorNode.nextKey != null)
                {
                    //뒤에 애가 잇으면 뒤애 애랑 바꿀껀데
                    KeyNode preNode2 = cursorNode.preKey; //커서 앞에 노드
                    KeyNode backNode = cursorNode.nextKey; //커서 뒤의 노드 무조건 존재.
                    KeyNode backBack = backNode.nextKey; //널일지 아닐지 모름
                    cursorNode.preKey = backNode; //뒤에 있던 애가 앞이 되고
                    cursorNode.nextKey = backBack; //뒤에 뒤에 있던 애가 커서 뒤가 되고
                    if (backBack != null)
                    {
                        backBack.preKey = cursorNode; //뒤에 뒤에 있던 애가 널이 아니면 연결해주고
                    }

                    backNode.preKey = preNode2; //커서 앞으로 간 애의 pre는 커서의 앞에 있던 애가 되고
                    backNode.nextKey = cursorNode; //next는 커서가 된다. 
                    if (backNode.preKey == null)
                    {
                        //근데 맨 앞이라면
                        startNode = backNode; //스타트 노드가 갱신된다. 
                    }
                    else
                    {
                        preNode2.nextKey = backNode;
                    }
                }
                break;
            case '-':
                //  Console.WriteLine("현재 키노드를 지울건데");
                if (cursorNode.preKey != null)
                {
                    //앞에 있는 애를 지울 수 있고
                    KeyNode preNode3 = cursorNode.preKey;
                    KeyNode prepreNode = preNode3.preKey;
                    cursorNode.preKey = prepreNode;
                    if (prepreNode != null)
                    {
                        prepreNode.nextKey = cursorNode;
                    }
                    else
                    {
                        //만약 맨 앞의 애를 지운거라면
                        startNode = cursorNode.nextKey; //스타트 갱신
                    }
                }

                break;
            default:
                //삽입은 현재 커서노드 에 집어넣을거
                KeyNode insertNode = new(command);

                KeyNode preNode = cursorNode.preKey;
                if (preNode != null)
                {
                    //앞에 문자가 있었으면 연결해주고
                    preNode.nextKey = insertNode;
                    insertNode.preKey = preNode;
                    insertNode.nextKey = cursorNode;//뒤에 커서 연결하고
                    cursorNode.preKey = insertNode; //커서 앞에 얘를 연결하고
                }
                else
                {
                    //맨앞에 등장한거면
                    insertNode.nextKey = cursorNode;//뒤에 커서 연결하고
                    cursorNode.preKey = insertNode; //커서 앞에 얘를 연결하고
                    startNode = insertNode; //시작을 얘로 갱신하고 
                }

                break;
        }
    }

    static string KeyNodeConsole()
    {
        StringBuilder sb = new();

        KeyNode curNode = startNode;
        while (true)
        {
            if (curNode.text != ' ')
            {
                sb.Append(curNode.text);
            }

            if (curNode.nextKey != null)
            {
                curNode = curNode.nextKey;
            }
            else
            {
                break;
            }
        }

        return sb.ToString();
    }

    static void KeyLogDoCommand(char command)
    {
        switch (command)
        {
            case '<':
                //  Console.WriteLine("커서 왼쪽으로 옮기기");
                if (curKCusorPos != 0)
                {
                    curKCusorPos -= 1;
                }
                break;
            case '>':
                //   Console.WriteLine("커서 오른쪽으로 옮기기");
                if (curKCusorPos != curKInputList.Count)
                {
                    curKCusorPos += 1;
                }
                break;
            case '-':
                //  Console.WriteLine("커서 왼쪽 문자 지우기");
                if (curKCusorPos != 0)
                {
                    curKInputList.RemoveAt(curKCusorPos - 1);
                    curKCusorPos -= 1;
                }
                break;
            default:
                //   Console.WriteLine("콘솔 위치에 문자 삽입하기");

                curKInputList.Insert(curKCusorPos, command);
                curKCusorPos += 1;

                break;
        }
    }

    static string KeyLogConsoleList()
    {
        StringBuilder sb = new();
        for (int i = 0; i < curKInputList.Count; i++)
        {
            sb.Append(curKInputList[i]);
        }
        return sb.ToString();
    }

    #endregion

    #region 숫자카드2
    static void NumberCard()
    {
        int cardCount = int.Parse(Console.ReadLine());
        int[] cardArray = Array.ConvertAll(Console.ReadLine().Split(), int.Parse);
        int askCount = int.Parse(Console.ReadLine());
        int[] askArray = Array.ConvertAll(Console.ReadLine().Split(), int.Parse);

        Array.Sort(cardArray);
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < askArray.Length; i++)
        {
            int find = askArray[i];
            int haveIndex = LowerBound(cardArray, find);
            int haveCard = 0;
            if (haveIndex != -1)
            {
                if (cardArray[haveIndex] == find)
                {
                    int upperIdx = UpperBound(cardArray, find);

                    if (upperIdx == -1)
                    {
                        //만약 그 초과되는 얘가 없단건 lower때부터 끝 배열까지 해당 수로 도배되었다는 의미
                        upperIdx = cardArray.Length;
                    }
                    haveCard = upperIdx - haveIndex;
                }
            }
            sb.Append(haveCard + " ");
        }
        sb.Remove(sb.Length - 1, 1);
        Console.WriteLine(sb);
    }

    public static int LowerBound(int[] line, int key)
    {
        //중간 인덱스를 고른다
        //중간 값을 보고 키와 크기 비교
        //키와의 크기 비교에 따라 다음 검색범위를 결정 
        //중간값이 키보다 크면 검색범위를 왼쪽으로 좁히고, 같으면 좋은거고, 키보다 작으면 검색범위를 오른쪽으로 좁히고

        //검색범위
        int rangeStart = 0;
        int rangeEnd = line.Length - 1;
        int middleIdx = 0;
        int middleValue = 0;
        int findCount = 0;
        int findIndex = int.MaxValue;
        //범위 방식
        /*
         * ( ) 초과, 미만
         * [ ) -> 이게 편하다?
         * ( ]
         * [ ] 이상, 이하
         */
        while (rangeStart <= rangeEnd)
        {
            findCount += 1;
            middleIdx = rangeStart + (rangeEnd - rangeStart) / 2;
            // A + B /2  //rangeStart + (rangeEnd - rangeStart) / 2
            middleValue = line[middleIdx];
            if (key <= middleValue)
            {
                //만약 중간그값이 키 면 얘를 범위에 포함해두면 될것같은데 
                //그밖에 오른쪽 범위는 날리고
                rangeEnd = middleIdx - 1;
                findIndex = Math.Min(findIndex, middleIdx);
            }
            else if (middleValue < key)
            {
                //키값 보다 작은것들은 관심없으니까 날리고 
                rangeStart = middleIdx + 1;

            }

        }

        // Console.WriteLine("그값은" + findIndex);
        if (findIndex == int.MaxValue)
        {
            findIndex = -1;
        }
        return findIndex;
    }

    public static int UpperBound(int[] line, int key)
    {
        //중간 인덱스를 고른다
        //중간 값을 보고 키와 크기 비교
        //키와의 크기 비교에 따라 다음 검색범위를 결정 
        //중간값이 키보다 크면 검색범위를 왼쪽으로 좁히고, 같으면 좋은거고, 키보다 작으면 검색범위를 오른쪽으로 좁히고

        //검색범위
        int rangeStart = 0;
        int rangeEnd = line.Length - 1;
        int middleIdx = 0;
        int middleValue = 0;
        int findCount = 0;
        int findIndex = int.MaxValue;
        //범위 방식
        /*
         * ( ) 초과, 미만
         * [ ) -> 이게 편하다?
         * ( ]
         * [ ] 이상, 이하
         */
        while (rangeStart <= rangeEnd)
        {
            findCount += 1;
            middleIdx = rangeStart + (rangeEnd - rangeStart) / 2;
            // A + B /2  //rangeStart + (rangeEnd - rangeStart) / 2
            middleValue = line[middleIdx];
            if (key < middleValue)
            {
                //만약 중간그값이 키 면 얘를 범위에 포함해두면 될것같은데 
                //그밖에 오른쪽 범위는 날리고
                rangeEnd = middleIdx - 1;
                findIndex = Math.Min(findIndex, middleIdx);

            }
            else if (middleValue <= key)
            {
                rangeStart = middleIdx + 1;
            }

        }

        //Console.WriteLine("그값은" + findIndex);
        if (findIndex == int.MaxValue)
        {
            findIndex = -1;
        }
        return findIndex;
    }
    #endregion

    #region 나무자르기
    static void CutTree()
    {
        string[] inputs = Console.ReadLine().Split();
        int treeCount = int.Parse(inputs[0]);
        int needLength = int.Parse(inputs[1]);

        string[] treeLengthStr = Console.ReadLine().Split(); //나무길이 입력
        int[] treeLength = new int[treeCount];
        for (int i = 0; i < treeCount; i++)
        {
            treeLength[i] = int.Parse(treeLengthStr[i]); // 나무 인트 
        }

        //나무를 자를건데 최대한 높이를 길게 해서 남는 나무가 길게 - 대신 필요한 수만큼은 챙길 수 있게 

        int answerValue = int.MinValue;
        int startHeight = 0;
        int endHeight = int.MaxValue;
        while (startHeight <= endHeight)
        {
            long needSum = 0;
            int cutValue = startHeight + (endHeight - startHeight) / 2;
            for (int i = 0; i < treeCount; i++)
            {
                int cutSum = treeLength[i] - cutValue;
                if (cutSum > 0)
                {
                    needSum += cutSum; //자르고 남은 것만 더한다
                }
            }
            if (needSum >= needLength)
            {
                //필요한 이상만큼 나왔으면 높이를 좀더 높여본다.
                answerValue = Math.Max(answerValue, cutValue);
                startHeight = cutValue + 1;
            }
            else
            {
                endHeight = cutValue - 1;
            }
        }
        Console.WriteLine(answerValue);
    }
    #endregion
    #region 힙구현
    static void Heap()
    {
        //집 위치들이 있을 때
        //몇개의집에 공유기를 설치하였을 때 
        //공유기간의 차이의 최솟값이 가장크도록 -> 아주 고르게 분배한 경우를 찾아라. 
        //거리를 
        List<int> heap = new();
        heap.Add(0); //0인덱스 채울 아무값
        Add(heap, 12);
        Add(heap, 15);
        Add(heap, 1);
        Add(heap, -5);
        Add(heap, 17);
        Console.WriteLine(Remove(heap));
        Add(heap, 32);
        Console.WriteLine(Remove(heap));
        Add(heap, 16);
        Console.WriteLine(Remove(heap));
        Console.WriteLine(Remove(heap));
        Console.WriteLine(Remove(heap));
        Console.WriteLine(Remove(heap));
        Console.WriteLine(Remove(heap));
        Console.WriteLine(Remove(heap));

    }

    static void Add(List<int> _heap, int _value)
    {
        _heap.Add(_value); //맨 끝에 넣고

        int childIdx = _heap.Count - 1;
        int parentIdx = childIdx / 2;

        while (parentIdx != 0)
        {
            int childValue = _heap[childIdx];
            int parentValue = _heap[parentIdx];
            if (childValue <= parentValue)
            {
                return;
            }
            Swap(_heap, childIdx, parentIdx); //자식이 더크면 자식을 부모노드로 올리고
            childIdx = parentIdx;
            parentIdx = childIdx / 2;
        }
    }

    static int Remove(List<int> _heap)
    {
        if (_heap.Count == 1)
        {
            return -1;
        }

        int max = _heap[1];

        int last = _heap[_heap.Count - 1];
        _heap[1] = last;
        _heap.RemoveAt(_heap.Count - 1); //마지막 지우고
        //새로 노드된 애를 자식이랑 비교시작

        if (_heap.Count == 1)
        {
            //부모 빼면서 비었으면 그냥 끝
            return max;
        }

        //아니면 자식과 비교 진행
        ChangeChild(_heap, 1);
        return max;
    }

    static void ChangeChild(List<int> _heap, int _parent)
    {
        int parentIdx = _parent;
        int leftCIdx = parentIdx * 2;
        int rightCIdx = parentIdx * 2 + 1;
        int validIdx = _heap.Count - 1;
        int parentValue = _heap[parentIdx];

        //1 왼쪽이 없는경우엔 끝 (오른쪽도 없음)
        if (validIdx < leftCIdx)
        {
            return;
        }

        //2 왼쪽이 있으면 왼쪽값 구하기
        int leftCValue = _heap[leftCIdx]; //왼쪽자식놈값 구하고

        //3 오른쪽 있는지 체크 
        if (validIdx < rightCIdx)
        {
            //오른쪽 없으면 왼쪽하고만 비교
            if (leftCValue < parentValue)
            {
                //부모랑 비교해서 왼쪽이 작으면 끝
                return;
            }
            else
            {
                //아니면 부모랑 왼쪽 자식 교환
                Swap(_heap, leftCIdx, parentIdx);
                ChangeChild(_heap, leftCIdx); //자식 위치를 부모위치로 삼아 재검색
                return;
            }
        }
        //4. 좌우 둘다 있으면 둘이 먼저 비교 후 부모와 비교
        int rightValue = _heap[rightCIdx];

        //5. 좌우 중 왼쪽이 더큰 경우
        if (rightValue < leftCValue)
        {
            //왼쪽을 부모와 비교
            if (leftCValue < parentValue)
            {
                //부모랑 비교해서 왼쪽이 작으면 끝
                return;
            }
            else
            {
                //아니면 부모랑 왼쪽 자식 교환
                Swap(_heap, leftCIdx, parentIdx);
                ChangeChild(_heap, leftCIdx); //자식 위치를 부모위치로 삼아 재검색
                return;
            }
        }

        //6. 좌우 중 오른쪽이 더크면
        if (rightValue < parentValue)
        {
            //오른쪽과 비교해서 부모가 더크면 끝
            return;
        }

        Swap(_heap, rightCIdx, parentIdx);//오른쪽이 더크면 부모랑 바꾸고
        ChangeChild(_heap, rightCIdx);//재탐색
    }

    static void Swap(List<int> _heap, int _aIdx, int _bIdx)
    {
        int temp = _heap[_aIdx];
        _heap[_aIdx] = _heap[_bIdx];
        _heap[_bIdx] = temp;
    }
    #endregion
    #region CardQueue
    static void CardQueue()
    {
        Queue<int> queue = new();

        int quest = int.Parse(Console.ReadLine());
        for (int i = 1; i <= quest; i++)
        {
            QueuePush(i);
        }

        while (QueueSize() > 1)
        {
            QueuePop();
            int second = QueuePop();
            QueuePush(second);
        }

        Console.WriteLine(QueueFront());
    }
    #endregion

    #region queue
    public static readonly StreamReader input = new(new
BufferedStream(Console.OpenStandardInput()));
    public static readonly StreamWriter output = new(new
        BufferedStream(Console.OpenStandardOutput()));

    static int[] queueArray = new int[10];
    static int head = 0;
    static int tail = 0;

    static void QueueTest()
    {
        int questCount = int.Parse(input.ReadLine());
        for (int i = 0; i < questCount; i++)
        {
            string str = input.ReadLine();
            string[] commandArray = str.Split();
            string command = commandArray[0];
            switch (command)
            {
                case "push":
                    int pushValue = int.Parse(commandArray[1]);
                    QueuePush(pushValue);
                    break;
                case "pop":
                    output.WriteLine(QueuePop().ToString());
                    break;
                case "size":
                    output.WriteLine(QueueSize().ToString());
                    break;
                case "empty":
                    output.WriteLine(QueueEmpty().ToString());
                    break;
                case "front":
                    output.WriteLine(QueueFront().ToString());
                    break;
                case "back":
                    output.WriteLine(QueueBack().ToString());
                    break;

            }
        }
    }

    static void QueuePush(int _value)
    {
        if (tail == queueArray.Length)
        {
            int[] newValue = new int[queueArray.Length * 2];
            for (int i = head; i < tail; i++)
            {
                newValue[i - head] = queueArray[i];

            }
            tail = tail - head;
            head = 0;
            queueArray = newValue;
        }
        queueArray[tail] = _value;
        tail += 1;
    }

    static int QueuePop()
    {
        int dequeueValue = -1;
        if (head == tail)
        {
            return dequeueValue;
        }

        dequeueValue = queueArray[head];
        head += 1;
        return dequeueValue;
    }

    static int QueueSize()
    {
        return tail - head;
    }

    static int QueueEmpty()
    {
        int size = QueueSize();

        return size == 0 ? 1 : 0;
    }

    static int QueueFront()
    {
        if (QueueSize() == 0)
        {
            return -1;
        }

        return queueArray[head];
    }

    static int QueueBack()
    {
        if (QueueSize() == 0)
        {
            return -1;
        }
        return queueArray[tail - 1];
    }
    #endregion

    #region 짝맞는괄호
    static void MatchFence()
    {
        while (true)
        {
            string fenceStr = Console.ReadLine();
            Stack<char> fenceQue = new(); //괄호만 넣을것들
            char first = fenceStr[0];
            if (first == '.')
            {
                return; //끝
            }
            bool isMatch = true;
            for (int i = 0; i < fenceStr.Length; i++)
            {
                char charactor = fenceStr[i];
                if (charactor == '(' || charactor == '[')
                {
                    fenceQue.Push(charactor);
                    continue;
                }
                if (charactor == ')')
                {
                    if (fenceQue.TryPop(out char result) == false)
                    {
                        isMatch = false;
                        break;
                    }
                    if (result != '(')
                    {
                        isMatch = false;
                        break;
                    }
                    continue;
                }
                if (charactor == ']')
                {
                    if (fenceQue.TryPop(out char result) == false)
                    {
                        isMatch = false;
                        break;
                    }
                    if (result != '[')
                    {
                        isMatch = false;
                        break;
                    }
                    continue;
                }
            }
            if (fenceQue.Count == 0 && isMatch == true)
            {
                Console.WriteLine("yes");
            }
            else
            {
                Console.WriteLine("no");
            }

        }

    }
    #endregion
    #region 오이푸스
    static void Oipus()
    {
        string[] command = Console.ReadLine().Split();
        int manCount = int.Parse(command[0]);
        int term = int.Parse(command[1]);
        List<int> list = new List<int>();
        for (int i = 1; i <= manCount; i++)
        {
            list.Add(i);
        }

        int index = 0;
        StringBuilder sb = new StringBuilder();
        sb.Append("<");
        while (list.Count > 0)
        {
            index = (index + term - 1) % list.Count;
            sb.Append(list[index] + ", ");
            list.RemoveAt(index);
        }
        sb.Remove(sb.Length - 2, 2);
        sb.Append(">");
        Console.WriteLine(sb);



    }
    #endregion

    #region 덱
    class Deck
    {

        int[] deckArray = new int[20004];
        int frontIdx = 10000;
        int backIdx = 10001;
        int size = 0;
        /*
        push_front X: 정수 X를 덱의 앞에 넣는다.
        push_back X: 정수 X를 덱의 뒤에 넣는다.
        pop_front: 덱의 가장 앞에 있는 수를 빼고, 그 수를 출력한다.만약, 덱에 들어있는 정수가 없는 경우에는 -1을 출력한다.
        pop_back: 덱의 가장 뒤에 있는 수를 빼고, 그 수를 출력한다.만약, 덱에 들어있는 정수가 없는 경우에는 -1을 출력한다.
        size: 덱에 들어있는 정수의 개수를 출력한다.
        empty: 덱이 비어있으면 1을, 아니면 0을 출력한다.
        front: 덱의 가장 앞에 있는 정수를 출력한다. 만약 덱에 들어있는 정수가 없는 경우에는 -1을 출력한다.
        back: 덱의 가장 뒤에 있는 정수를 출력한다. 만약 덱에 들어있는 정수가 없는 경우에는 -1을 출력한다.
            */
        public void Command()
        {
            int count = int.Parse(Console.ReadLine());
            for (int i = 0; i < count; i++)
            {
                string[] command = Console.ReadLine().Split();
                switch (command[0])
                {
                    case "push_front":
                        PushFront(int.Parse(command[1]));
                        break;
                    case "push_back":
                        PushBack(int.Parse(command[1]));
                        break;
                    case "pop_front":
                        Console.WriteLine(PopFront().ToString());
                        break;
                    case "pop_back":
                        Console.WriteLine(PopBack().ToString());
                        break;
                    case "size":
                        Console.WriteLine(size.ToString());
                        break;
                    case "empty":
                        int empty = size == 0 ? 1 : 0;
                        Console.WriteLine(empty.ToString());
                        break;
                    case "front":
                        Console.WriteLine(Front().ToString());
                        break;
                    case "back":
                        Console.WriteLine(Back().ToString());
                        break;

                }
            }
        }

        public void PushFront(int _x)
        {
            deckArray[frontIdx] = _x;
            frontIdx--;
            size++;
        }
        public void PushBack(int _x)
        {
            deckArray[backIdx] = _x;
            backIdx++;
            size++;
        }

        public int PopFront()
        {
            if (size == 0)
            {
                return -1;
            }
            frontIdx += 1;
            size--;
            return deckArray[frontIdx];
        }

        public int PopBack()
        {
            if (size == 0)
            {
                return -1;
            }
            backIdx -= 1;
            size--;
            return deckArray[backIdx];
        }

        public int Front()
        {
            if (size == 0)
            {
                return -1;
            }
            return deckArray[frontIdx + 1];
        }

        public int Back()
        {
            if (size == 0)
            {
                return -1;
            }
            return deckArray[backIdx - 1];
        }
    }

    #endregion
    #region 프린터큐
    class PrintNode
    {
        public int weight = 0;
        public int index = 0;

        public PrintNode(int _weight, int _index)
        {
            weight = _weight;
            index = _index;
        }
    }

    static void PrintQue()
    {
        int count = int.Parse(Console.ReadLine());
        for (int i = 0; i < count; i++)
        {
            string[] command = Console.ReadLine().Split();
            int orderCount = int.Parse(command[0]);
            int orderIdx = int.Parse(command[1]);

            Queue<PrintNode> printQueue = new();
            string[] orderStr = Console.ReadLine().Split();
            int[] values = new int[10];
            for (int x = 0; x < orderCount; x++)
            {
                int weight = int.Parse(orderStr[x]);
                values[weight] += 1;
                PrintNode node = new PrintNode(weight, x);
                printQueue.Enqueue(node);
            }

            int maxValue = GetMax(values);
            int deCount = 0;
            while (true)
            {
                PrintNode node = printQueue.Dequeue();
                if (node.weight >= maxValue)
                {
                    deCount += 1; //빼낸 횟수
                    values[maxValue] -= 1;
                    maxValue = GetMax(values);
                    if (node.index == orderIdx)
                    {
                        Console.WriteLine(deCount);
                        break;
                    }
                }
                else
                {
                    printQueue.Enqueue(node);
                }
            }
        }
    }
    static int GetMax(int[] _values)
    {
        for (int i = 9; i >= 0; i--)
        {
            if (_values[i] != 0)
            {
                return i;
            }
        }
        return 0;
    }
    #endregion

    public static readonly StreamReader inputReader = new(new
BufferedStream(Console.OpenStandardInput()));
    public static readonly StreamWriter outputWriter = new(new
        BufferedStream(Console.OpenStandardOutput()));
    #region 디피에스
    static void DFSTest()
    {
        string[] command = Console.ReadLine().Split();
        int nodeCount = int.Parse(command[0]);
        int lineCount = int.Parse(command[1]);
        int start = int.Parse(command[2]);
        List<int>[] graph = new List<int>[nodeCount + 1];
        int[] isVisit = new int[nodeCount + 1];

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

        for (int i = 1; i < nodeCount + 1; i++)
        {
            graph[i].Sort();
        }

        //DfsSerch(graph, isVisit, start);
        //for (int i = 1; i < isVisit.Length; i++)
        //{
        //    Console.WriteLine(isVisit[i].ToString());
        //}

        BfsSerch(graph, isVisit, start);
    }

    static int visitCount = 1;
    static void DfsSerch(List<int>[] _graph, int[] _isVisit, int _visitNode)
    {
        if (_isVisit[_visitNode] != 0)
        { //방문한곳이면 끝
            return;
        }

        //Console.WriteLine(_visitNode.ToString()); //방문한곳 출력
        _isVisit[_visitNode] = visitCount; //방문했음
        visitCount++;
        List<int> ableNode = _graph[_visitNode]; //방문가능지 뽀ㅃ고
        for (int i = 0; i < ableNode.Count; i++)
        {
            if (_isVisit[ableNode[i]] == 0)
            {
                //방문한적없으면
                DfsSerch(_graph, _isVisit, ableNode[i]); //방문합니다.
            }
        }
    }

    static void BfsSerch(List<int>[] _graph, int[] _isVisit, int _startNode)
    {

        Queue<int> visitQ = new();
        visitQ.Enqueue(_startNode);
        int visitCount = 1;
        while (visitQ.Count > 0)
        {
            int visit = visitQ.Dequeue();
            if (_isVisit[visit] != 0)
            {
                continue;
            }
            //방문한적이 없는곳이면
            _isVisit[visit] = visitCount; //방문한번째 기록
            visitCount++;//방문번째 상승

            List<int> ableVisit = _graph[visit];//현재노드에서 방문가능한 리스트 뽑고
            for (int i = 0; i < ableVisit.Count; i++)
            {
                int nextVisit = ableVisit[i];
                if (_isVisit[nextVisit] == 0)
                {
                    //방문한적없으면
                    visitQ.Enqueue(nextVisit); //큐에넣는다.
                }
            }
        }
        for (int i = 1; i < _isVisit.Length; i++)
        {
            Console.WriteLine(_isVisit[i].ToString());
        }
    }
    #endregion

    #region 컴퓨터 바이러스
    static void VirusTest()
    {
        int nodeCount = int.Parse(Console.ReadLine());
        int lineCount = int.Parse(Console.ReadLine());
        int start = 1;
        List<int>[] graph = new List<int>[nodeCount + 1];
        int[] isVisit = new int[nodeCount + 1];

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

        VirusSerch(graph, isVisit, 1);
    }

    static void VirusSerch(List<int>[] _graph, int[] _isVisit, int _startNode)
    {

        Queue<int> visitQ = new();
        visitQ.Enqueue(_startNode);
        int visitCount = 0;
        while (visitQ.Count > 0)
        {
            int visit = visitQ.Dequeue();
            if (_isVisit[visit] != 0)
            {
                continue;
            }
            //방문한적이 없는곳이면
            visitCount++;//방문번째 상승
            _isVisit[visit] = visitCount; //방문한번째 기록


            List<int> ableVisit = _graph[visit];//현재노드에서 방문가능한 리스트 뽑고
            for (int i = 0; i < ableVisit.Count; i++)
            {
                int nextVisit = ableVisit[i];
                if (_isVisit[nextVisit] == 0)
                {
                    //방문한적없으면
                    visitQ.Enqueue(nextVisit); //큐에넣는다.
                }
            }
        }
        Console.WriteLine(visitCount - 1);
    }
    #endregion

    #region 아파트
    static void Apart()
    {

    }
    #endregion

    #region 알파벳 트리 순회
    static int AValue = 65 - 1; //보정값


    static void AlphaTree()
    {
        int alphaCount = int.Parse(Console.ReadLine());
        int[,] alphaTree = new int[alphaCount + 1, 3];
        for (int i = 0; i < alphaCount; i++)
        {
            string alphaStr = Console.ReadLine();
            int aIndex = AValue; //1번부터 넣기 위해 -1
            int alphabetIndex = alphaStr[0] - aIndex;
            int leftChild = alphaStr[2] - aIndex;
            if (leftChild > 0)
            {
                //자식이 있을경우에만
                alphaTree[alphabetIndex, 1] = leftChild; //왼쪽 자식에 자식 입력
                alphaTree[leftChild, 0] = alphabetIndex; //왼쪽 자식의 부모를 자기로
            }
            int rightChild = alphaStr[4] - aIndex;
            if (rightChild > 0)
            {
                //자식이 있을경우에만
                alphaTree[alphabetIndex, 2] = rightChild; //오른쪽 자식에 입력
                alphaTree[rightChild, 0] = alphabetIndex; //오른쪽 자식의 부모를 자기로
            }
        }//트리 완성


        //말하기 진행

        StringBuilder sb = new();
        sb = MeFirst(1, sb, alphaTree);
        Console.WriteLine(sb);
        sb.Clear();

        sb = MeMiddle(1, sb, alphaTree);
        Console.WriteLine(sb);
        sb.Clear();

        sb = MeLate(1, sb, alphaTree);
        Console.WriteLine(sb);
        sb.Clear();
    }

    static StringBuilder MeFirst(int _node, StringBuilder _sb, int[,] _alphaTree)
    {
        _sb.Append(((char)(_node + AValue))); //자기 넣고
        if (_alphaTree[_node, 1] != 0)
        {
            MeFirst(_alphaTree[_node, 1], _sb, _alphaTree);
        }
        if (_alphaTree[_node, 2] != 0)
        {
            MeFirst(_alphaTree[_node, 2], _sb, _alphaTree);
        }
        return _sb;
    }

    static StringBuilder MeMiddle(int _node, StringBuilder _sb, int[,] _alphaTree)
    {

        if (_alphaTree[_node, 1] != 0)
        {
            MeMiddle(_alphaTree[_node, 1], _sb, _alphaTree);
        }
        _sb.Append(((char)(_node + AValue))); //자기 넣고
        if (_alphaTree[_node, 2] != 0)
        {
            MeMiddle(_alphaTree[_node, 2], _sb, _alphaTree);
        }
        return _sb;
    }

    static StringBuilder MeLate(int _node, StringBuilder _sb, int[,] _alphaTree)
    {

        if (_alphaTree[_node, 1] != 0)
        {
            MeLate(_alphaTree[_node, 1], _sb, _alphaTree);
        }
        if (_alphaTree[_node, 2] != 0)
        {
            MeLate(_alphaTree[_node, 2], _sb, _alphaTree);

        }
        _sb.Append(((char)(_node + AValue))); //자기 넣고
        return _sb;
    }
    #endregion

    #region 트리지름
    static void TreeNubi()
    {
        int nodeCount = int.Parse(Console.ReadLine());
        int lineCount = nodeCount - 1;
        List<int[]>[] numberTree = new List<int[]>[nodeCount + 1]; // ,0 부모 ,1 왼쪽자식 ,2 왼쪽가중치, 3 오른쪽자식, 4 오른쪽가중치
        List<int>[] numberWeight = new List<int>[nodeCount+1]; //자식들 가중치
        for (int i = 0; i < numberTree.Length; i++)
        {
            numberTree[i] = new List<int[]>();
            numberWeight[i] = new List<int>();
        }

        for (int i = 0; i < lineCount; i++)
        {
            string[] lineStr = Console.ReadLine().Split();
            int parentNum = int.Parse( lineStr[0]);
            int childNum = int.Parse(lineStr[1]);
            int lineWeight= int.Parse(lineStr[2]);
            int[] value = { childNum, lineWeight };
            numberTree[parentNum].Add(value);
            numberWeight[parentNum].Add(0); //가중치 0으로 추가
        }//트리 완성

        int maxValue = 0;
        LateSearch(1, 0, numberTree, numberWeight, ref maxValue);
        Console.WriteLine(maxValue);
     }

    static int LateSearch(int _node, int _lineWeight, List<int[]>[] _tree, List<int>[] _weight, ref int _maxValue)
    {
        List<int[]> childList = _tree[_node];
        for (int i = 0; i < childList.Count; i++)
        {
            int childNum = childList[i][0];
            int lineWeight = childList[i][1];

            _weight[_node][i] = LateSearch(childNum, lineWeight, _tree, _weight, ref _maxValue);
           // Console.WriteLine(_node + "의 "+ i + "번째 자식의 비중은" + _weight[_node][i]);
        }


        //해당 노드에서 왼쪽 오른쪽 라인을 당겼을때의 맥스값
        List<int> weightList = _weight[_node];
        int weight = 0;
        for (int i = 0; i < weightList.Count; i++)
        {
            weight = Math.Max(weight, weightList[i]);
          //  Console.WriteLine(_node+"의 최고 존엄은"+weight);
            for (int j = i+1; j < weightList.Count; j++)
            {
                _maxValue = Math.Max(_maxValue, (weightList[i] + weightList[j]));
                
            }
        }
       
        //부모까지의 가중치를 더해서 반환
        return weight + _lineWeight;
    }
    #endregion

    #region 트리지름2

    static void MakeTree(List<int[]>[] _graph, int _start, List<int[]>[] _tree, List<int>[] _treeWeight)
    {
        bool[] _isVisited = new bool[_graph.Length];
        _isVisited[_start] = true;
        Queue<int> nodeQu = new();
        nodeQu.Enqueue(_start);

        while (nodeQu.Count > 0)
        {
            int current = nodeQu.Dequeue();

            //자식 리스트
            List<int[]> childList = _graph[current];
            //현재 노드가 부모가 됨
            for (int i = 0; i < childList.Count; i++)
            {
                int childNum = childList[i][0]; //연결된 번호
                int weight = childList[i][1]; //연결된 가중치
                if (_isVisited[childNum] == false)
                {
                    //방문한적없으면 얘를 자식으로 삼기
                    _tree[current].Add(new int[]{childNum, weight});
                    _treeWeight[current].Add(0);
                    nodeQu.Enqueue(childNum); //자식도 자식 만들기 진행 
                    _isVisited[childNum] = true;
                }
            }
        }
    }

    static void TreeNubi2()
    {
        int nodeCount = int.Parse(Console.ReadLine());
        int lineCount = nodeCount ;

        List<int[]>[] graph = new List<int[]>[nodeCount + 1];
        for (int i = 1; i < graph.Length; i++)
        {
            graph[i] = new List<int[]>();
        }

        int start = 0;
        for (int i = 0; i < lineCount; i++)
        {
            string[] lineStr = Console.ReadLine().Split();
            int parentNum = int.Parse(lineStr[0]);
          
            for (int j = 1; j < lineStr.Length; j += 2)
            {
                int childNum = int.Parse(lineStr[j]);

                if (childNum == -1)
                {
                    break;
                }

                int lineWeight = int.Parse(lineStr[j + 1]);
                int[] value = { childNum, lineWeight };
                graph[parentNum].Add(value);
                start = parentNum;
            }

        }//그래프 완성

        List<int[]>[] numberTree = new List<int[]>[nodeCount + 1]; // ,0 부모 ,1 왼쪽자식 ,2 왼쪽가중치, 3 오른쪽자식, 4 오른쪽가중치
        List<int>[] numberWeight = new List<int>[nodeCount + 1]; //자식들 가중치
        for (int i = 0; i < numberTree.Length; i++)
        {
            numberTree[i] = new List<int[]>();
            numberWeight[i] = new List<int>();
        }

        MakeTree(graph, start, numberTree, numberWeight);

        int maxValue = 0;
        LateSearch2(start, 0, numberTree, numberWeight, ref maxValue);
        Console.WriteLine(maxValue);
    }

    static int LateSearch2(int _node, int _lineWeight, List<int[]>[] _tree, List<int>[] _weight, ref int _maxValue)
    {
        List<int[]> childList = _tree[_node];
        for (int i = 0; i < childList.Count; i++)
        {
            int childNum = childList[i][0];
            int lineWeight = childList[i][1];

            _weight[_node][i] = LateSearch2(childNum, lineWeight, _tree, _weight, ref _maxValue);
            // Console.WriteLine(_node + "의 "+ i + "번째 자식의 비중은" + _weight[_node][i]);
        }


        //해당 노드에서 왼쪽 오른쪽 라인을 당겼을때의 맥스값
        List<int> weightList = _weight[_node];
        int weight = 0;
        int best = 0;
        int second = 0;
        for (int i = 0; i < weightList.Count; i++)
        {
            int value = weightList[i];
            if (best < value)
            {
                second = best;
                best = value;
                continue;
            }
            else if(second < value)
            {
                second = value;
            }
        }
        weight = best;
        _maxValue = Math.Max(_maxValue, best+second);


        //부모까지의 가중치를 더해서 반환
        return weight + _lineWeight;
    }
    #endregion



    #region 포도주나열
    static void DrinkWine()
    {
        int wineCount = int.Parse(Console.ReadLine());
        int[] wineAmounts = new int[wineCount];
        for (int i = 0; i < wineCount; i++)
        {
            wineAmounts[i] = int.Parse(Console.ReadLine());
        }

        //연속으로 3번을 마시지 못할 때, 가장 많이 마시는 방법
        //n 까지 마셨을때 다음 기회를 쉬어야하나, 1잔까지 가능하냐, 2잔까지 가능하냐 버전 
    }
    #endregion
}
