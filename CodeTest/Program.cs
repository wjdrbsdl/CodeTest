using System;
using System.Reflection.Emit;
using System.Text;


internal class Program
{

    #region 하노이
    static StringBuilder sb = new StringBuilder(); //스트링빌더 쓰면 빠르다.
    static int GetAnotherGoal(int _goal, int _cur)
    {
        if (_goal != 1 && _cur != 1)
        {
            return 1;
        }
        else if (_goal != 2 && _cur != 2)
        {
            return 2;
        }
        else if (_goal != 3 && _cur != 3)
        {
            return 3;
        }

        return -1;
    }

    static void DebugMove(int _level, int _curPos, int _goal)
    {
        Console.WriteLine(_level + "을" + _curPos + "에서 " + _goal + "로 가는법");
    }

    static Dictionary<string, string> routeDic = new Dictionary<string, string>();

    class Cup
    {
        public int level = 1;
        public int curPos = 1;
        public int goalPos = 1;
        public Cup? smallCup = null;
        public Cup? bigCup = null;

        public Cup(int _level, Cup _smallCup)
        {
            level = _level;
            if (_smallCup != null)
            {
                smallCup = _smallCup;
                _smallCup.bigCup = this;
            }
        }


        public void SetPos(int _goal)
        {
            curPos = _goal;
            if (smallCup != null)
            {
                smallCup.SetPos(_goal);
            }
        }

        public string OrderMove(int _goal, string _record)
        {
            int tempCurPos = curPos;
            string tempRecord = _record;
            // Console.WriteLine(level +"이"+ tempCurPos + "에서"+_goal+"로 이동하기 시작");
            //if (routeDic.ContainsKey(level + "_" + tempCurPos + "_" + _goal))
            //{
            //    SetPos(_goal);
            //    return routeDic[level + "_" + tempCurPos + "_" + _goal]+"\n";
            //}
            goalPos = _goal;

            if (smallCup != null)
            {
                int upCupGoal = GetAnotherGoal(_goal, curPos);
                // DebugMove(smallCup.level, smallCup.curPos, upCupGoal);
                tempRecord += smallCup.OrderMove(upCupGoal, tempRecord);
            }

            // Console.WriteLine(curPos.ToString() + " " + goalPos.ToString());
            tempRecord += curPos.ToString() + " " + goalPos.ToString() + "\n";
            sb.Append(curPos.ToString() + " " + goalPos.ToString() + "\n");
            //log.Add(curPos.ToString() + " " + goalPos.ToString());
            curPos = goalPos;
            // Console.WriteLine($"{level}컵 {goalPos}로 이동");
            if (smallCup != null)
            {
                // DebugMove(smallCup.level, smallCup.curPos, curPos);
                string newOrder = "";
                tempRecord += smallCup.OrderMove(curPos, newOrder);
            }
            string remove = tempRecord.TrimEnd('\n');
            //Console.WriteLine(level + "이" + tempCurPos + "에서" + _goal + "로 이동하기 종료 경로는\n"+ remove);
            //  routeDic.Add(level + "_" + tempCurPos + "_" + _goal, remove);
            return tempRecord;
        }

        public void OrderMove(int _goal)
        {
            int tempCurPos = curPos;

            goalPos = _goal;

            if (smallCup != null)
            {
                int upCupGoal = GetAnotherGoal(_goal, curPos);
                smallCup.OrderMove(upCupGoal);
            }

            sb.Append(curPos.ToString() + " " + goalPos.ToString() + "\n");
            curPos = goalPos;
            // Console.WriteLine($"{level}컵 {goalPos}로 이동");
            if (smallCup != null)
            {
                // DebugMove(smallCup.level, smallCup.curPos, curPos);
                string newOrder = "";
                smallCup.OrderMove(curPos);
            }
        }
    }

    static void Hanoi()
    {
        routeDic = new();
        int cupCount = int.Parse(Console.ReadLine());
        Cup[] cups = new Cup[cupCount + 1];
        for (int i = 1; i <= cupCount; i++)
        {
            Cup newCup = new Cup(i, cups[i - 1]);
            cups[i] = newCup;
        }
        int count = 1;
        for (int i = 2; i <= cupCount; i++)
        {
            count = count * 2 + 1;
        }
        Console.WriteLine(count);
        sb = new StringBuilder();


        // string remove = cups[cupCount].OrderMove(3, "").TrimEnd('\n');
        cups[cupCount].OrderMove(3);

        Console.WriteLine(sb);
        //Console.WriteLine(remove);
    }
    #endregion

    #region 별그리기

    static void PrintStar(int _n, StringBuilder sb)
    {
        //n은 3의 배수고 3칸 그리고 1칸 띄우고, 3칸 그리고를 한다 
        string text = sb.ToString();
        string[] textLine = text.Split('\n');
        int colum = textLine[0].Length;
        sb = new StringBuilder();
        for (int i = 0; i < textLine.Length * 3; i++)
        {
            //1칸씩 채워나가기 그려야할 줄은
            string origin = textLine[i % textLine.Length]; //세로줄 마다 반복

            for (int x = 0; x < colum * 3; x++)
            {
                if (i >= textLine.Length && i <= textLine.Length * 2 - 1)
                {
                    if (x >= colum && x <= colum * 2 - 1)
                    {
                        sb.Append(" ");
                        continue;
                    }
                }
                sb.Append(origin[x % colum]);
            }
            sb.Append("\n");
        }
        sb.Remove(sb.Length - 1, 1);

        if (_n == 3)
        {
            Console.Write(sb);
        }
        else
        {
            PrintStar(_n / 3, sb);
        }

    }


    #endregion

    #region 동적 횟수 비교
    static int FibCount = 1;
    static int FibnaCount = 0;
    static void CodeCompare(int _count)
    {
        Fib(_count);
        fiboArray = new int[_count + 1];
        Fibo(_count);

        Console.WriteLine(FibCount + " " + FibnaCount);
    }

    static int Fib(int _n)
    {
        if (_n == 1 || _n == 2)
        {
            return 1;
        }
        FibCount++;
        return Fib(_n - 1) + Fib(_n - 2);
    }

    static int[] fiboArray;
    static int Fibo(int _n)
    {
        if (_n == 1 || _n == 2)
        {
            return 1;
        }

        if (fiboArray[_n] != 0)
        {
            return fiboArray[_n];
        }
        FibnaCount++;
        return fiboArray[_n] = Fibo(_n - 1) + Fibo(_n - 2);
    }
    #endregion

    #region 의사코드

    static void AskWBC()
    {
        record = new int[50, 50, 50];
        record[0, 0, 0] = 1;
        while (true)
        {

            string code = Console.ReadLine();
            string[] codeSplit = code.Split(" ");
            int[] codeNum = new int[3];
            for (int i = 0; i < 3; i++)
            {
                codeNum[i] = int.Parse(codeSplit[i]);
            }

            if (codeNum[0] == -1 && codeNum[1] == -1 && codeNum[2] == -1)
            {
                return;
            }
            Console.WriteLine("w(" + codeNum[0] + ", " + codeNum[1] + ", " + codeNum[2] + ") = " + WBC(codeNum[0], codeNum[1], codeNum[2]).ToString());

        }

    }

    static int[,,] record;
    static int WBC(int _a, int _b, int _c)
    {
        if (_a <= 0 || _b <= 0 || _c <= 0)
        {
            return 1;
        }

        if (_a > 20 || _b > 20 || _c > 20)
        {
            return WBC(20, 20, 20);
        }

        if (record[_a, _b, _c] != 0)
        {
            return record[_a, _b, _c];
        }

        if (_a < _b && _b < _c)
        {
            return record[_a, _b, _c] = WBC(_a, _b, _c - 1) + WBC(_a, _b - 1, _c - 1) - WBC(_a, _b - 1, _c);
        }
        return record[_a, _b, _c] = WBC(_a - 1, _b, _c) + WBC(_a - 1, _b - 1, _c) + WBC(_a - 1, _b, _c - 1) - WBC(_a - 1, _b - 1, _c - 1);
    }
    #endregion

    #region 에디터
    static int curCusorPos = 0;
    static List<char> curInputList = new List<char>();
    static void Editor()
    {
        string input = Console.ReadLine();
        for (int i = 0; i < input.Length; i++)
        {
            curInputList.Add(input[i]);
        }
        curCusorPos = curInputList.Count;

        int commandCount = int.Parse(Console.ReadLine());
        for (int i = 0; i < commandCount; i++)
        {
            string command = Console.ReadLine();
            DoCommand(command);
            //Console.WriteLine(ConsoleList() + "현재 커서위치 "+curCusorPos);
            Console.WriteLine(ConsoleList());
        }
    }

    static void DoCommand(string command)
    {
        char first = command[0];
        switch (first)
        {
            case 'L':
                Console.WriteLine("커서 왼쪽으로 옮기기");
                if (curCusorPos != 0)
                {
                    curCusorPos -= 1;
                }
                break;
            case 'D':
                Console.WriteLine("커서 오른쪽으로 옮기기");
                if (curCusorPos != curInputList.Count)
                {
                    curCusorPos += 1;
                }
                break;
            case 'B':
                Console.WriteLine("커서 왼쪽 문자 지우기");
                if (curCusorPos != 0)
                {
                    curInputList.RemoveAt(curCusorPos - 1);
                    curCusorPos -= 1;
                }
                break;
            case 'P':
                Console.WriteLine("콘솔 위치에 문자 삽입하기");

                for (int i = 2; i < command.Length; i++)
                {
                    curInputList.Insert(curCusorPos, command[i]);
                    curCusorPos += 1;
                }
                break;
        }
    }

    static string ConsoleList()
    {
        StringBuilder sb = new();
        for (int i = 0; i < curInputList.Count; i++)
        {
            sb.Append(curInputList[i]);
        }
        return sb.ToString();
    }

    #endregion

    #region KeyLog

    static int curKCusorPos = 0;
    static List<char> curKInputList = new List<char>();
    static void KeyLog()
    {
        int commandCount = int.Parse(Console.ReadLine());

        for (int i = 0; i < commandCount; i++)
        {
            string input = Console.ReadLine();
            curKInputList = new();
            curKCusorPos = 0;
            for (int x = 0; x < input.Length; x++)
            {
                KeyLogDoCommand(input[x]);
                // Console.WriteLine(KeyLogConsoleList());
            }
            Console.WriteLine(KeyLogConsoleList());
        }

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
                //    Console.WriteLine("커서 왼쪽 문자 지우기");
                if (curKCusorPos != 0)
                {
                    curKInputList.RemoveAt(curKCusorPos - 1);
                    curKCusorPos -= 1;
                }
                break;
            default:
                //  Console.WriteLine("콘솔 위치에 문자 삽입하기");

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

    #region N,M
    static void NAndM()
    {
        string[] readCommand = Console.ReadLine().Split(" ");
        int n = int.Parse(readCommand[0]);
        int m = int.Parse(readCommand[1]);

    }

    static void PrintNAndM(int _n, int _m, int _cur)
    {

    }

    #endregion

    #region 재귀함수가 뭐임
    static int goalLevel;
    static void WhatIsRecursion(int _level)
    {
        for (int i = 0; i < _level * 4; i++)
        {
            Console.Write("_");
        }
        Console.WriteLine("재귀함수가 뭔가요?");

        if (_level == goalLevel)
        {
            for (int i = 0; i < _level * 4; i++)
            {
                Console.Write("_");
            }
            Console.WriteLine("재귀함수는 자기 자신을 호출하는 함수라네");
            for (int i = 0; i < _level * 4; i++)
            {
                Console.Write("_");
            }
            Console.WriteLine("라고 답변하였지");
            return;
        }

        for (int i = 0; i < _level * 4; i++)
        {
            Console.Write("_");
        }
        Console.WriteLine("잘 들어보게. 옛날옛날 한 산 꼭대기에 이세상 모든 지식을 통달한 선인이 있었어.");
        for (int i = 0; i < _level * 4; i++)
        {
            Console.Write("_");
        }
        Console.WriteLine("마을 사람들은 모두 그 선인에게 수많은 질문을 했고, 모두 지혜롭게 대답해 주었지.");
        for (int i = 0; i < _level * 4; i++)
        {
            Console.Write("_");
        }
        Console.WriteLine("의 답은 대부분 옳았다고 하네.그런데 어느 날, 그 선인에게 한 선비가 찾아와서 물었어.");

        WhatIsRecursion(_level + 1);

        for (int i = 0; i < _level * 4; i++)
        {
            Console.Write("_");
        }
        Console.WriteLine("라고 답변하였지");

    }

    #endregion

    #region 전선만들기
    static void MakeCamp()
    {
        //숫자를 뽑을거고 그 숫자가 해에 해당하는지 체크만하면됨. 

    }
    #endregion

    #region 랜선자르기
    static void CutLine()
    {
        string[] inputs = input.ReadLine().Split();
        long K = int.Parse(inputs[0]);
        long N = int.Parse(inputs[1]);
        long minX = int.MinValue;

        //선받기
        int[] lines = new int[K];
        for (int i = 0; i < K; i++)
        {
            lines[i] = int.Parse(input.ReadLine());
            minX = Math.Max(minX, lines[i]); //최소 선 구하기
        }

        //선 검색 시작
        long start = 1;
        long end = minX;
        long answer = 0;
        while (start <= end)
        {
            long middleX = start + (end - start) / 2;

            long sum = 0; //선 합
            for (int i = 0; i < K; i++)
            {
                sum += lines[i] / middleX; //현재 테스트중인 길이로 나눈 몫을 더함
            }

            if (sum >= N)
            {
                //목표 이상치로 나왔으면 위에꺼 더 찾아봐야하니까
                answer = Math.Max(answer, middleX); //답 저장해놓고
                start = middleX + 1; //저장 위치를 옮기고
            }
            else
            {
                //답이 안나왔으면
                end = middleX - 1; //범위 좁히고
            }
        }
        output.WriteLine(answer);
        output.Flush();
    }
    #endregion

    public static readonly StreamReader input = new(new
BufferedStream(Console.OpenStandardInput()));
    public static readonly StreamWriter output = new(new
        BufferedStream(Console.OpenStandardOutput()));

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
        while (true)
        {
            findCount += 1;
            middleIdx = rangeStart + (rangeEnd - rangeStart) / 2;
            // A + B /2  //rangeStart + (rangeEnd - rangeStart) / 2
            middleValue = line[middleIdx];
            if (key <= middleValue)
            {
                //만약 중간그값이 키 면 얘를 범위에 포함해두면 될것같은데 
                //그밖에 오른쪽 범위는 날리고
                rangeEnd = middleIdx;
                findIndex = Math.Min(findIndex, middleIdx);
                if (rangeEnd == rangeStart)
                {
                    break;
                }
            }
            else if (middleValue < key)
            {
                //키값 보다 작은것들은 관심없으니까 날리고 
                rangeStart = middleIdx + 1;

            }

            //다둘러보는 조건은?
            if (rangeEnd < rangeStart)
            {
                //범위 다찾음
                break;
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
        while (rangeStart < rangeEnd)
        {
            findCount += 1;
            middleIdx = rangeStart + (rangeEnd - rangeStart) / 2;
            // A + B /2  //rangeStart + (rangeEnd - rangeStart) / 2
            middleValue = line[middleIdx];
            if (key < middleValue)
            {
                //만약 중간그값이 키 면 얘를 범위에 포함해두면 될것같은데 
                //그밖에 오른쪽 범위는 날리고
                rangeEnd = middleIdx;
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

    #region 수 찾기
    static int[] A = new int[15];
    static void Search()
    {
        StringBuilder sb = new StringBuilder();
        int count = int.Parse(Console.ReadLine());
        A = Array.ConvertAll(Console.ReadLine().Split(), int.Parse);
        Array.Sort(A);

        int questCount = int.Parse(Console.ReadLine());
        int[] questArray = Array.ConvertAll(Console.ReadLine().Split(), int.Parse);

        for (int i = 0; i < questCount; i++)
        {
            sb.AppendLine(SerachBinary(questArray[i]).ToString());
        }
        output.WriteLine(sb);
        output.Flush();
        return;
    }

    static int SerachBinary(int _findKey)
    {
        int searchIndex = 0;

        int startIdx = 0;
        int endIdx = A.Length - 1;

        while (startIdx <= endIdx)
        {
            int midIdx = startIdx + (endIdx - startIdx) / 2;
            int midValue = A[midIdx];

            if (midValue == _findKey)
            {
                searchIndex = 1;
                break;
            }
            else if (midValue < _findKey)
            {
                startIdx = midIdx + 1;
            }
            else
            {

                endIdx = midIdx - 1;
            }
        }


        return searchIndex;
    }
    #endregion

    #region 이진 탐색
    public static bool LinearSearch(List<int> linearList, int key)
    {
        for (int i = 0; i < linearList.Count; i++)
        {
            if (linearList[i] == key)
            {
                return true;
            }
        }
        return false;
    }

    public static bool BinarySearch(List<int> line, int key)
    {
        //중간 인덱스를 고른다
        //중간 값을 보고 키와 크기 비교
        //키와의 크기 비교에 따라 다음 검색범위를 결정 
        //중간값이 키보다 크면 검색범위를 왼쪽으로 좁히고, 같으면 좋은거고, 키보다 작으면 검색범위를 오른쪽으로 좁히고

        //검색범위
        int rangeStart = 0;
        int rangeEnd = line.Count - 1;
        int middleIdx = 0;
        int middleValue = 0;
        int findCount = 0;
        //범위 방식
        /*
         * ( ) 초과, 미만
         * [ ) -> 이게 편하다?
         * ( ]
         * [ ] 이상, 이하
         */
        while (true)
        {
            findCount += 1;
            middleIdx = rangeStart + (rangeEnd - rangeStart) / 2;
            // A + B /2  //rangeStart + (rangeEnd - rangeStart) / 2
            middleValue = line[middleIdx];
            if (key < middleValue)
            {
                //오른쪽 범위 날리기
                rangeEnd = middleIdx - 1;
            }
            else if (middleValue == key)
            {
                Console.WriteLine(findCount + "만에 찾앗다" + middleIdx + "자리에 위치");
                return true;
            }
            else if (middleValue < key)
            {
                rangeStart = middleIdx + 1;
            }

            if (rangeEnd < rangeStart)
            {
                //범위 다찾음
                break;
            }
        }

        Console.WriteLine("없다");
        return false;
    }

    public static int LowerBound(List<int> line, int key)
    {
        //중간 인덱스를 고른다
        //중간 값을 보고 키와 크기 비교
        //키와의 크기 비교에 따라 다음 검색범위를 결정 
        //중간값이 키보다 크면 검색범위를 왼쪽으로 좁히고, 같으면 좋은거고, 키보다 작으면 검색범위를 오른쪽으로 좁히고

        //검색범위
        int rangeStart = 0;
        int rangeEnd = line.Count - 1;
        int middleIdx = 0;
        int middleValue = 0;
        int findCount = 0;
        int findIndex = -1;
        //범위 방식
        /*
         * ( ) 초과, 미만
         * [ ) -> 이게 편하다?
         * ( ]
         * [ ] 이상, 이하
         */
        while (true)
        {
            findCount += 1;
            middleIdx = rangeStart + (rangeEnd - rangeStart) / 2;
            // A + B /2  //rangeStart + (rangeEnd - rangeStart) / 2
            middleValue = line[middleIdx];
            if (key <= middleValue)
            {
                //만약 중간그값이 키 면 얘를 범위에 포함해두면 될것같은데 
                //그밖에 오른쪽 범위는 날리고
                rangeEnd = middleIdx;
                findIndex = middleIdx;
                if (rangeEnd == rangeStart)
                {
                    break;
                }
            }
            else if (middleValue < key)
            {
                //키값 보다 작은것들은 관심없으니까 날리고 
                rangeStart = middleIdx + 1;

            }

            //다둘러보는 조건은?
            if (rangeEnd < rangeStart)
            {
                //범위 다찾음
                break;
            }
        }

        Console.WriteLine("그값은" + findIndex);
        return findIndex;
    }

    public static int UpperBound(List<int> line, int key)
    {
        //중간 인덱스를 고른다
        //중간 값을 보고 키와 크기 비교
        //키와의 크기 비교에 따라 다음 검색범위를 결정 
        //중간값이 키보다 크면 검색범위를 왼쪽으로 좁히고, 같으면 좋은거고, 키보다 작으면 검색범위를 오른쪽으로 좁히고

        //검색범위
        int rangeStart = 0;
        int rangeEnd = line.Count - 1;
        int middleIdx = 0;
        int middleValue = 0;
        int findCount = 0;
        int findIndex = -1;
        //범위 방식
        /*
         * ( ) 초과, 미만
         * [ ) -> 이게 편하다?
         * ( ]
         * [ ] 이상, 이하
         */
        while (rangeStart < rangeEnd)
        {
            findCount += 1;
            middleIdx = rangeStart + (rangeEnd - rangeStart) / 2;
            // A + B /2  //rangeStart + (rangeEnd - rangeStart) / 2
            middleValue = line[middleIdx];
            if (key < middleValue)
            {
                //만약 중간그값이 키 면 얘를 범위에 포함해두면 될것같은데 
                //그밖에 오른쪽 범위는 날리고
                rangeEnd = middleIdx;
                findIndex = middleIdx;

            }
            else if (middleValue <= key)
            {
                rangeStart = middleIdx + 1;
            }

        }

        Console.WriteLine("그값은" + findIndex);
        return findIndex;
    }
    #endregion

    #region 공유기실패

    class WifiDistance
    {
        public int distance = 0;
        public WifiDistance leftDistance;
        public WifiDistance rightDistance;

        public WifiDistance(int _leftPos, int _rightPos, WifiDistance _leftWifi)
        {
            distance = _rightPos - _leftPos;
            leftDistance = _leftWifi;
            if (_leftWifi != null)
            {
                _leftWifi.rightDistance = this;
            }

        }

        public void ExpandWifi()
        {
            //해당 녀석의 좌우 길이를 비교해서 더 짧은 애랑 합치면됨 
            if (leftDistance == null)
            {
                CombineWifi(rightDistance, false); //오른쪽이랑 결합
                return;
            }
            if (rightDistance == null)
            {
                CombineWifi(leftDistance, true);
                return;
            }

            if (leftDistance.distance < rightDistance.distance)
            {
                CombineWifi(leftDistance, true);
                return;
            }
            else
            {
                CombineWifi(rightDistance, false);//오른쪽이랑 결합
                return;
            }
        }

        public void CombineWifi(WifiDistance _target, bool _isLeft)
        {
            distanceList.Remove(_target);
            //타겟된 와이파이를 흡수 
            if (_isLeft)
            {
                //왼쪽 와이파이 흡수
                leftDistance = _target.leftDistance;
                if (leftDistance != null)
                {
                    leftDistance.rightDistance = this;
                }
            }
            else
            {
                rightDistance = _target.rightDistance;
                if (rightDistance != null)
                {
                    rightDistance.leftDistance = this;
                }
            }
            distance += _target.distance; //거리 통합
        }
    }

    class WifiHeap
    {
        public WifiDistance[] heap;

        public WifiHeap(int _listCount)
        {
            heap = new WifiDistance[_listCount];
        }

        public void Add(WifiDistance _wifi)
        {

        }

        public void Remove(WifiDistance _wifi)
        {

        }

        public WifiDistance Get()
        {
            return heap[1];
        }
    }

    static List<WifiDistance> distanceList = new();
    static WifiDistance[] distanceHeap;
    static void SetWifi()
    {
        string[] inputs = Console.ReadLine().Split();
        int homeCount = int.Parse(inputs[0]);
        int wifiCount = int.Parse(inputs[1]);

        WifiHeap heap = new WifiHeap(homeCount);
        List<int> housePos = new();
        for (int i = 0; i < homeCount; i++)
        {
            housePos.Add(int.Parse(Console.ReadLine()));
        }
        housePos.Sort();
        //거리정보 담아둘 클래스
        WifiDistance firstWifi = new WifiDistance(housePos[0], housePos[1], null);
        distanceList.Add(firstWifi);
        for (int i = 1; i < housePos.Count - 1; i++)
        {
            WifiDistance wifi = new WifiDistance(housePos[i], housePos[i + 1], distanceList[i - 1]);
            distanceList.Add(wifi);
        }

        // Console.WriteLine("와이파이 거리별로 나왔다");
        distanceList.Sort((a, b) => a.distance.CompareTo(b.distance));
        for (int i = 0; i < homeCount - wifiCount; i++)
        {
            distanceList[0].ExpandWifi();
            distanceList.Sort((a, b) => a.distance.CompareTo(b.distance));
        }
        Console.WriteLine(distanceList[0].distance);
        //모든 집에 공유기가 있다고 진행
        //그 사이 거리를 클래스로 생성
        //거리가 가장 짧은 클래스를 왼쪽이나 오른쪽으로 병합하면서 갱신 
        //병합할때 마다 wifi 하나씩 제거 
    }
    #endregion

    #region 공유기설치
    static void SetWifiBinary()
    {
        string[] inputs = Console.ReadLine().Split();
        int homeCount = int.Parse(inputs[0]);
        int wifiCount = int.Parse(inputs[1]);

        List<int> housePos = new();
        for (int i = 0; i < homeCount; i++)
        {
            housePos.Add(int.Parse(Console.ReadLine()));
        }
        housePos.Sort(); //집 위치 정렬되었고

        int start = 1;
        int end = housePos[homeCount - 1];
        int maxDistance = int.MinValue;
        while (start <= end)
        {
            int distance = (start + end) / 2;

            int curWifiCount = 1; //기본 0번집에 설치 
            int prePos = housePos[0];
            for (int i = 1; i < homeCount; i++)
            {
                int homeDistance = housePos[i] - prePos;
                if (homeDistance >= distance)
                {
                    //와이파이 설치했을때만 이전집 위치 갱신
                    prePos = housePos[i];
                    curWifiCount++;
                }
            }

            //목표한량 이상이면 거리를 더 넓혀보기
            if (curWifiCount >= wifiCount)
            {
                start = distance + 1;
                maxDistance = Math.Max(distance, maxDistance);
            }
            else
            {
                //아니면 거리를 좁히기
                end = distance - 1;
            }

        }
        Console.WriteLine(maxDistance);
    }
    #endregion

    #region 힙힙힙

    static int Pop(List<int> _heap)
    {
        int rootValue = 0;
        if (_heap.Count == 1)
        {
            return rootValue;
        }
        rootValue = _heap[1];

        Swap(_heap, 1, _heap.Count - 1); //맨끝이랑 루트노드랑 바꾸고
        _heap.RemoveAt(_heap.Count - 1);//맨끝에 지우고

        if (_heap.Count == 1)
        {
            return rootValue;
        }
        Compare(_heap, 1);

        return rootValue;
    }

    static void Compare(List<int> _heap, int _parentIdx)
    {
        //부모가 자식보다 높은지 비교후 바꾸기 작업
        //이제 내려가기 시작
        int validIdx = _heap.Count - 1;
        int parentIdx = _parentIdx;
        int leftChildIdx = parentIdx * 2;
        int rightChildIdx = parentIdx * 2 + 1;
        int parentValue = _heap[parentIdx];

        if (validIdx < leftChildIdx)
        {
            return;
        }
        int leftChildValue = _heap[leftChildIdx];

        if (validIdx < rightChildIdx)
        {
            //오른쪽 자식이 없는 경우엔 왼쪽하고만 비교해서 끝

            if (leftChildValue < parentValue)
            {
                //부모가 크면 끝
                return;
            }
            else
            {
                Swap(_heap, parentIdx, leftChildIdx); //왼쪽자식이 더크면 위치 바꾸고
                Compare(_heap, leftChildIdx); //아래 들어간 자식이 그 자식과 비교를 더 진행
                return;
            }

        }

        int rightChildValue = _heap[rightChildIdx];

        //오른쪽거까지 유효하면 둘이 비교해서 큰거랑 부모랑 비교
        if (rightChildValue < leftChildValue)
        {
            if (leftChildValue < parentValue)
            {
                //부모가 크면 끝
                return;
            }
            else
            {
                Swap(_heap, parentIdx, leftChildIdx); //왼쪽자식이 더크면 위치 바꾸고
                Compare(_heap, leftChildIdx); //아래 들어간 자식이 그 자식과 비교를 더 진행
                return;
            }
        }

        if (parentValue < rightChildValue)
        {
            Swap(_heap, parentIdx, rightChildIdx);
            Compare(_heap, rightChildIdx);
        }


    }

    static void Add(List<int> heapList, int _value)
    {
        heapList.Add(_value); //값을 넣고

        int childValue = _value;
        int childIdx = heapList.Count - 1;
        int parentIdx = childIdx / 2;
        int parentValue = heapList[parentIdx];
        while (parentValue < childValue && parentIdx != 0)
        {
            Swap(heapList, parentIdx, childIdx);
            childIdx = parentIdx;
            parentIdx = childIdx / 2;
            parentValue = heapList[parentIdx];
            childValue = heapList[childIdx];
        }


    }

    static void Swap(List<int> _list, int _pIdx, int _cIdx)
    {
        int temp = _list[_pIdx];
        _list[_pIdx] = _list[_cIdx];
        _list[_cIdx] = temp;
    }
    #endregion

    #region 팩토리얼
    static long[] saveFac;
    static void CalFactorial()
    {
        int N = int.Parse(Console.ReadLine());
        saveFac = new long[N + 1];
        saveFac[0] = 1;
        Cal(N);
        Console.WriteLine(saveFac[N]);
    }

    static long Cal(int _curValue)
    {
        if (_curValue == 0)
        {
            return 1;
        }

        if (saveFac[_curValue] != 0)
        {
            return saveFac[_curValue];
        }

        return saveFac[_curValue] = _curValue * Cal(_curValue - 1);
    }
    #endregion

    #region 펠린드롬

    static void 펠린드롬호출()
    {
        int N = int.Parse(Console.ReadLine());
        for (int i = 0; i < N; i++)
        {
            CheckPalin(Console.ReadLine());
        }
    }

    static bool CheckPalin(string _str)
    {
        int isTrue = RecursionCheckPalin(_str, 0, _str.Length - 1, 1);

        if (isTrue == 1)
            return true;

        return false;
    }

    static int RecursionCheckPalin(string _str, int _leftIdx, int _rightIdx, int _count)
    {
        //해당 문자의, 좌우를 한칸씩 줄여나가면서
        //같은지 보고 
        //중앙에 도착했으면 성공
        if (_rightIdx <= _leftIdx)
        {
            //인덱스가 서로 교차 되었거나(짝수길이)
            //같은 위치일때(홀수 길이)
            Console.WriteLine(1 + " " + _count);
            return 1;
        }
        if (_str[_leftIdx] != _str[_rightIdx])
        {
            //도중에 다르면 펄스
            Console.WriteLine(0 + " " + _count);
            return 0;
        }

        return RecursionCheckPalin(_str, _leftIdx + 1, _rightIdx - 1, _count + 1); //좌우 인덱스 범위를 좁혀서 재진행
    }
    #endregion

    #region 칸토어
    static void Kanto()
    {
        while (true)
        {
            string questStr = Console.ReadLine();
            if (questStr == null)
            {
                return;
            }
            int quest = int.Parse(questStr);
            StringBuilder sb = new StringBuilder();
            sb.Append("-");
            Print(sb, 0, quest);
        }

    }
    static void Print(StringBuilder sb, int _n, int _goal)
    {
        if (_n == _goal)
        {
            Console.WriteLine(sb);
            return;
        }
        StringBuilder newSb = new StringBuilder();
        string curText = sb.ToString();
        int textCount = curText.Length;
        int drawCount = curText.Length * 3;
        int middleStart = curText.Length;
        int middleEnd = curText.Length * 2;
        for (int i = 0; i < drawCount; i++)
        {
            if (middleStart <= i && i < middleEnd)
            {
                newSb.Append(" ");
                continue;
            }
            int index = i % textCount;
            newSb.Append(curText[index]);
        }
        Print(newSb, _n + 1, _goal);
    }
    #endregion

    #region 01타일
    static void Tile01()
    {
        int N = int.Parse(Console.ReadLine());
        int[] nArray = new int[N + 1];
        nArray[0] = 1;
        nArray[1] = 1;
        if (N == 1)
        {
            Console.WriteLine(1);
            return;
        }

        for (int i = 2; i <= N; i++)
        {
            nArray[i] = (nArray[i - 1] + nArray[i - 2]) % 15746;
        }
        Console.WriteLine(nArray[N]);
    }
    #endregion

    #region 계단수
    static void Stair(int _n)
    {
        //해당 단계에서 0~9로 끝난 수를 기록하고
        //다음 단계에선 전 단계의 0~9의 수에 따라 기록해나가면되는군!
        int N = _n;
        long[,] value = new long[N + 1, 10];
        value[1, 0] = 0;
        value[1, 1] = 1;
        value[1, 2] = 1;
        value[1, 3] = 1;
        value[1, 4] = 1;
        value[1, 5] = 1;
        value[1, 6] = 1;
        value[1, 7] = 1;
        value[1, 8] = 1;
        value[1, 9] = 1;
        for (int i = 2; i <= N; i++)
        {
            for (int x = 0; x < 10; x++)
            {
                long xCount = value[i - 1, x]; //전단계의 0을 끝으로 가진 숫자의 수
                if (x == 0)
                {
                    value[i, x + 1] = (value[i, x + 1] + xCount) % 1_000_000_000;
                    continue;
                }
                if (x == 9)
                {
                    value[i, x - 1] = (value[i, x - 1] + xCount) % 1_000_000_000;
                    continue;
                }
                //그외는
                value[i, x + 1] = (value[i, x + 1] + xCount) % 1_000_000_000;
                value[i, x - 1] = (value[i, x - 1] + xCount) % 1_000_000_000;
                //플러스 마이너스로 넣어주기
            }

        }
        long answerN = 0;
        for (int i = 0; i < 10; i++)
        {
            answerN = (answerN + value[N, i]) % 1_000_000_000;
        }
        Console.WriteLine(answerN);
    }
    #endregion

    #region 삼각나선
    static void Triangle(int _n)
    {
        int n = _n;
        int[] value = new int[n + 1];

        if (n == 1 || n == 2 || n == 3)
        {
            Console.WriteLine(1);
            return;
        }
        if (n == 4 || n == 5)
        {
            Console.WriteLine(2);
            return;
        }
        value[1] = 1;
        value[2] = 1;
        value[3] = 1;
        value[4] = 2;
        value[5] = 2;

        for (int i = 6; i <= n; i++)
        {
            value[i] = value[i - 1] + value[i - 5];
        }
        Console.WriteLine(value[n]);
    }
    #endregion

    #region 스택만들기
    static int[] stackArray;
    static int topIdx = 0;
    static int size = 0;
    static void StackPractie()
    {

        stackArray = new int[10];

        while (true)
        {
            string[] commandArray = Console.ReadLine().Split();
            string command = commandArray[0];
            Command(command);
        }

    }

    static void Command(string _str)
    {
        string[] commandArray = _str.Split();
        string command = commandArray[0];
        switch (command)
        {
            case "push":
                int pushValue = int.Parse(commandArray[1]);
                Push(pushValue);
                break;
            case "pop":
                Console.WriteLine(Pop());
                break;
            case "size":
                Console.WriteLine(Size());
                break;
            case "empty":
                Console.WriteLine(Empty().ToString());
                break;
            case "top":
                Console.WriteLine(Top());
                break;

        }
    }

    static void Push(int _x)
    {
        if (topIdx == stackArray.Length)
        {
            //공간 늘리기
            int[] newStack = new int[topIdx * 2];
            for (int i = 0; i < stackArray.Length; i++)
            {
                newStack[i] = stackArray[i];
            }
            stackArray = newStack;
        }
        stackArray[topIdx] = _x;
        topIdx += 1;
    }

    static int Pop()
    {
        int popValue = -1;
        if (topIdx == 0)
        {
            return popValue;
        }

        topIdx -= 1;
        popValue = stackArray[topIdx];

        return popValue;
    }

    static int Size()
    {
        int size = topIdx;

        return size;
    }

    static int Empty()
    {
        if (topIdx == 0)
            return 1;

        return 0;
    }

    static int Top()
    {
        int peekValue = -1;
        if (topIdx == 0)
        {
            return peekValue;
        }

        peekValue = stackArray[topIdx - 1];

        return peekValue;
    }
    #endregion

    #region 스택수열
    static void StackArray()
    {
        int intCount = int.Parse(Console.ReadLine());
        List<int> intList = new();
        List<int> sortList = new();
        for (int i = 0; i < intCount; i++)
        {
            int input = int.Parse(Console.ReadLine());
            intList.Add(input);
            sortList.Add(input);
        }
        sortList.Sort();

        int totalCount = intCount;
        int wantIndex = 0;
        int stackIndex = 0;
        Stack<int> stack = new();
        StringBuilder sb = new StringBuilder();
        while (true)
        {
            int wantNum = intList[wantIndex];
            if (stack.TryPeek(out int result))
            {
                if (result == wantNum)
                {
                    stack.Pop();
                    wantIndex += 1;
                    sb.AppendLine("-");
                    if (wantIndex == totalCount)
                    {
                        break;
                    }
                    continue;
                }
            }

            if (sortList.Count == stackIndex)
            {
                Console.WriteLine("NO");
                return;
            }
            stack.Push(sortList[stackIndex]);
            stackIndex += 1;
            sb.AppendLine("+");
        }

        Console.WriteLine(sb);
    }
    #endregion

    #region 배열큐
    class ArrayQueue
    {
        public int[] queueArray = new int[10];
        public int head = 0; //삭제할 위치
        public int tail = 0; //삽입할 위치
        public int size = 0;

        public void QueueTest()
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

        public void QueuePush(int _value)
        {
            //if (tail == queueArray.Length)
            //{
            //    int[] newValue = new int[queueArray.Length * 2];
            //    for (int i = head; i < tail; i++)
            //    {
            //        newValue[i - head] = queueArray[i];

            //    }
            //    tail = tail - head;
            //    head = 0;

            //    queueArray = newValue;
            //}
            if (QueueSize() == queueArray.Length)
            {
                return;
            }

            if (tail == queueArray.Length)
            {
                //맨 끝에 도달했을때
                //사용중인 사이즈가 용량보다 낮으면

                tail = 0; //삽입할 곳을 0으로 되돌리기


            }
            queueArray[tail] = _value;
            tail += 1;
            size += 1;
        }

        public int QueuePop()
        {
            int dequeueValue = -1;
            if (QueueSize() == 0)
            {
                return dequeueValue;
            }

            if (head == queueArray.Length)
            {
                head = 0;
            }

            dequeueValue = queueArray[head];
            head += 1;
            size -= 1;
            return dequeueValue;
        }

        public int QueueSize()
        {
            //폐구간 - 개구간으로 빼서 범위를 만들 수 있다.
            return size;
        }

        public int QueueEmpty()
        {
            int size = QueueSize();

            return size == 0 ? 1 : 0;
        }
        public int QueueFront()
        {
            if (QueueSize() == 0)
            {
                return -1;
            }

            return queueArray[head];
        }
        public int QueueBack()
        {
            if (QueueSize() == 0)
            {
                return -1;
            }
            return queueArray[tail - 1];
        }
    }

    #endregion

    #region 연결리스트 큐
    public class QueNode
    {
        public int value = 0;
        public int index;
        public QueNode next;

        public QueNode(int _value)
        {
            value = _value;
        }

        public QueNode Next()
        {
            return next;
        }
    }

    class LinkQueue
    {
        public QueNode headNode = null;
        public QueNode endNode = null;

        public void LinkQueueTest()
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
                        LinkQueuePush(pushValue);
                        break;
                    case "pop":
                        output.WriteLine(LinkQueuePop().ToString());
                        break;
                    case "size":
                        output.WriteLine(LinkQueueSize().ToString());
                        break;
                    case "empty":
                        output.WriteLine(LinkQueueEmpty().ToString());
                        break;
                    case "front":
                        output.WriteLine(LinkQueueFront().ToString());
                        break;
                    case "back":
                        output.WriteLine(LinkQueueBack().ToString());
                        break;

                }
            }
            output.Flush();
        }

        public void LinkQueuePush(int _value)
        {
            QueNode newNode = new QueNode(_value);
            if (endNode == null)
            {
                newNode.index = 0;
                headNode = newNode;
                endNode = newNode;
                return;
            }
            endNode.next = newNode;
            newNode.index = endNode.index + 1;
            endNode = newNode;


        }

        public int LinkQueuePop()
        {
            if (headNode == null)
            {
                return -1;
            }

            int value = headNode.value;
            headNode = headNode.Next();
            if (headNode == null)
            {
                endNode = null;
            }
            return value;
        }

        public int LinkQueueSize()
        {
            if (endNode == null)
            {
                return 0;
            }

            return endNode.index - headNode.index + 1; //인덱스 이상 이하이므로 +1
        }

        public int LinkQueueEmpty()
        {
            return LinkQueueSize() == 0 ? 1 : 0;
        }

        public int LinkQueueFront()
        {
            if (headNode == null)
            {
                return -1;
            }

            return headNode.value;
        }

        public int LinkQueueBack()
        {
            if (endNode == null)
            {
                return -1;
            }

            return endNode.value;
        }
    }

    #endregion

    #region 스택클래스로 구현해보기
    class HandMadeStack
    {
        int[] stackArray = new int[10];
        int topIdx;

        public HandMadeStack()
        {
            topIdx = 0;
        }

        ~HandMadeStack()
        {

        }

        public void Push(int _value)
        {
            if (topIdx == stackArray.Length)
            {
                //확장진행
                int[] newArray = new int[stackArray.Length * 2];
                for (int i = 0; i < stackArray.Length; i++)
                {
                    newArray[i] = stackArray[i];
                }
                stackArray = newArray;
            }

            stackArray[topIdx] = _value;
            topIdx += 1;
        }

        public int Pop()
        {
            if (topIdx == 0)
            {
                return -1;
            }
            topIdx--;
            int popValue = stackArray[topIdx];


            return popValue;
        }

        public int Size()
        {
            int size = topIdx + 1;
            return size;
        }
    }
    #endregion




}
