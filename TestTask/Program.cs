using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace TestTask
{
    class Program
    {
        static void Main(string[] args)
        {
            const int MaxCount = 10;
            int previous = -1;
            int current = 0;
            State state = new State() { Count = MaxCount, Current = 0 };

            // 呼び出し方法 その1
            // Taskのインスタンスを作ってからStartする。
            //
            //var task = new Task<int>(o => CountUpMethod((State)o), state);
            //task.Start();

            // 呼び出し方法 その2
            // Taskのインスタンスを作ってからRunするが、
            // stateはTaskのインスタンスに渡していない為、
            // task.AsyncStateがnullになり、NullReferenceExceptionが発生する。
            //
            //var task = Task.Run(() => CountUpMethod(state));

            // 呼び出し方法 その3
            // TaskのFactoryのStartNewでインスタンスの作成とスタートを同時に実行する。
            // また、stateをTaskのインスタンスに渡している(インスタンス内にstateが保存される) 
            var task = Task.Factory.StartNew(o => CountUpMethod((State)o), state);

            while (current < MaxCount)
            {
                current = ((State)task.AsyncState).Current;
                if(current != previous )
                {
                    Console.Write($"{current} ");
                    previous = current;
                }
#if DEBUG
                Debug.WriteLine($"{current}");
#endif
            }

            Console.WriteLine("\n\nPush any key!");
            Console.ReadKey();
        }

        static int CountUpMethod(State state)
        {
            int count = 0;

            for (int i = 0; i < state.Count; i++)
            {
                count++;
                state.Current = count;
                Thread.Sleep(1000); // 1秒
            }

            return count;
        }
        
    }

    class State
    {
        public int Current { get; set; }
        public int Count { get; set; }
    }
}
