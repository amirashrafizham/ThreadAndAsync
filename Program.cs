// See https://aka.ms/new-console-template for more information
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

using System.Net.Http;

namespace ThreadAndAsync
{
    class Program
    {
        public static async Task Main()
        {
            //RunningSynchronously();
            //RunningMultiThreadWithJoin();
            //ThreadPoolExample();
            //ThreadVsThreadPoolExample();
            TaskExample1_usingStart();
            //TaskExample2_usingFactory();
            //TaskExample3_usingRun();
            //TaskExample4_MainThreadWait();
            //TaskExample5_TaskReturnValue(10000);
            //TaskExample6_TaskReturnClass();
            //AsyncExample1_SynchronousExample();
            //await AsyncExample2_AsyncExample();
        }

        static void RunningSynchronously()
        {
            var sw = Stopwatch.StartNew();
            Method1();
            Method2();
            Method3();
            Console.WriteLine($"Total elapsed time :  {sw.ElapsedMilliseconds * 0.001:N2} seconds");
        }

        static void RunningMultiThread()
        {
            var sw = Stopwatch.StartNew();
            Console.WriteLine("Main Thread started");
            Thread t1 = new Thread(Method1)
            {
                Name = "Thread1"
            };
            Thread t2 = new Thread(Method2)
            {
                Name = "Thread2"
            };
            Thread t3 = new Thread(Method3)
            {
                Name = "Thread3"
            };

            t1.Start();
            t2.Start();
            t3.Start();

            Console.WriteLine("Main Thread ended");
            Console.WriteLine($"Total elapsed time :  {sw.ElapsedMilliseconds * 0.001} seconds");
        }

        static void RunningMultiThreadWithJoin()
        {
            var sw = Stopwatch.StartNew();
            Console.WriteLine("Main Thread started");
            Thread t1 = new Thread(Method1)
            {
                Name = "Thread1"
            };
            Thread t2 = new Thread(Method2)
            {
                Name = "Thread2"
            };
            Thread t3 = new Thread(Method3)
            {
                Name = "Thread3"
            };

            t1.Start();
            t2.Start();
            t3.Start();

            t1.Join();
            t2.Join();
            t3.Join();
            Console.WriteLine("Main Thread ended");
            Console.WriteLine($"Total elapsed time :  {sw.ElapsedMilliseconds * 0.001} seconds");
        }

        static void Method1()
        {
            Console.WriteLine($"Method 1 started. Id : {Thread.CurrentThread.ManagedThreadId}; Thread Pool: {Thread.CurrentThread.IsThreadPoolThread}; Background: {Thread.CurrentThread.IsBackground}");
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("met 1 : " + i);
            }
            Console.WriteLine($"Method 1 started. Id : {Thread.CurrentThread.ManagedThreadId}; Thread Pool: {Thread.CurrentThread.IsThreadPoolThread}; Background: {Thread.CurrentThread.IsBackground}");
        }

        static void Method2()
        {
            Console.WriteLine($"Method 2 started. Id : {Thread.CurrentThread.ManagedThreadId}; Thread Pool: {Thread.CurrentThread.IsThreadPoolThread}; Background: {Thread.CurrentThread.IsBackground}");
            for (int i = 0; i < 10; i++)
            {
                if (i == 3)
                {
                    Console.WriteLine("Querying database transaction");
                    Thread.Sleep(2000);
                    Console.WriteLine("Querying database completed");
                }
                Console.WriteLine("Method 2 : " + i);
            }
            Console.WriteLine($"Method 2 started. Id : {Thread.CurrentThread.ManagedThreadId}; Thread Pool: {Thread.CurrentThread.IsThreadPoolThread}; Background: {Thread.CurrentThread.IsBackground}");
        }

        static void Method3()
        {
            Console.WriteLine($"METH 3 started using thread: {Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine($"Method 3 started. Id : {Thread.CurrentThread.ManagedThreadId}; Thread Pool: {Thread.CurrentThread.IsThreadPoolThread}; Background: {Thread.CurrentThread.IsBackground}");
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("METH 3 : " + i);
            }
            Console.WriteLine($"Method 3 started. Id : {Thread.CurrentThread.ManagedThreadId}; Thread Pool: {Thread.CurrentThread.IsThreadPoolThread}; Background: {Thread.CurrentThread.IsBackground}");
        }


        static void ThreadPoolExample()
        {
            for (int i = 0; i < 10; i++)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(RandomMethod));
            }
            Console.Read();
        }

        static void RandomMethod(object obj)
        {
            Thread thread = Thread.CurrentThread;
            Console.WriteLine($"Background: {thread.IsBackground}; Thread Pool: {thread.IsThreadPoolThread}; Thread ID: {Thread.CurrentThread.ManagedThreadId}");
        }

        static void ThreadVsThreadPoolExample()
        {
            for (int i = 0; i < 10; i++)
            {
                MethodWithThread();
                MethodWithThreadPool();
            }
            Stopwatch stopwatch = new Stopwatch();
            Console.WriteLine("Execution using Thread");
            stopwatch.Start();
            MethodWithThread();
            stopwatch.Stop();
            Console.WriteLine("Time consumed by MethodWithThread is : " +
                                 stopwatch.ElapsedTicks.ToString());

            stopwatch.Reset();
            Console.WriteLine("Execution using Thread Pool");
            stopwatch.Start();
            MethodWithThreadPool();
            stopwatch.Stop();
            Console.WriteLine("Time consumed by MethodWithThreadPool is : " +
                                 stopwatch.ElapsedTicks.ToString());

            Console.Read();

        }

        static void MethodWithThread()
        {
            for (int i = 0; i < 5; i++)
            {
                Thread t = new Thread(RandomMethod);
                t.Start();
            }

        }

        static void MethodWithThreadPool()
        {
            for (int i = 0; i < 5; i++)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(RandomMethod));
            }

        }

        static void PrintCounter()
        {
            Console.WriteLine($"Child Thread Started. Id : {Thread.CurrentThread.ManagedThreadId}; Thread Pool: {Thread.CurrentThread.IsThreadPoolThread}; Background: {Thread.CurrentThread.IsBackground}");
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine($"Count : {i}");
            }
            Console.WriteLine($"Child Thread Ended. Id : {Thread.CurrentThread.ManagedThreadId}; Thread Pool: {Thread.CurrentThread.IsThreadPoolThread}; Background: {Thread.CurrentThread.IsBackground}");
        }

        static void TaskExample1_usingStart()
        {

            Console.WriteLine($"Main Thread Started. Id : {Thread.CurrentThread.ManagedThreadId}; Thread Pool: {Thread.CurrentThread.IsThreadPoolThread}; Background: {Thread.CurrentThread.IsBackground}");

            Task task1 = new Task(PrintCounter);
            task1.Start();

            Console.WriteLine($"Main Thread Ended. Id : {Thread.CurrentThread.ManagedThreadId}; Thread Pool: {Thread.CurrentThread.IsThreadPoolThread}; Background: {Thread.CurrentThread.IsBackground}");
            Console.ReadLine();

        }

        static void TaskExample2_usingFactory()
        {

            Console.WriteLine($"Main Thread Started. Id : {Thread.CurrentThread.ManagedThreadId}; Thread Pool: {Thread.CurrentThread.IsThreadPoolThread}; Background: {Thread.CurrentThread.IsBackground}");

            Task task1 = Task.Factory.StartNew(PrintCounter);

            Console.WriteLine($"Main Thread Ended. Id : {Thread.CurrentThread.ManagedThreadId}; Thread Pool: {Thread.CurrentThread.IsThreadPoolThread}; Background: {Thread.CurrentThread.IsBackground}");
            Console.ReadLine();

        }
        static void TaskExample3_usingRun()
        {

            Console.WriteLine($"Main Thread Started. Id : {Thread.CurrentThread.ManagedThreadId}; Thread Pool: {Thread.CurrentThread.IsThreadPoolThread}; Background: {Thread.CurrentThread.IsBackground}");

            Task task1 = Task.Run(() => PrintCounter());

            Console.WriteLine($"Main Thread Ended. Id : {Thread.CurrentThread.ManagedThreadId}; Thread Pool: {Thread.CurrentThread.IsThreadPoolThread}; Background: {Thread.CurrentThread.IsBackground}");
            Console.ReadLine();

        }
        static void TaskExample4_MainThreadWait()
        {

            Console.WriteLine($"Main Thread Started. Id : {Thread.CurrentThread.ManagedThreadId}; Thread Pool: {Thread.CurrentThread.IsThreadPoolThread}; Background: {Thread.CurrentThread.IsBackground}");

            Task task1 = Task.Factory.StartNew(PrintCounter);
            task1.Wait();

            Console.WriteLine($"Main Thread Ended. Id : {Thread.CurrentThread.ManagedThreadId}; Thread Pool: {Thread.CurrentThread.IsThreadPoolThread}; Background: {Thread.CurrentThread.IsBackground}");
            Console.ReadLine();

        }

        static int CalculateSum(int x)
        {
            Console.WriteLine($"Child Thread Started. Id : {Thread.CurrentThread.ManagedThreadId}; Thread Pool: {Thread.CurrentThread.IsThreadPoolThread}; Background: {Thread.CurrentThread.IsBackground}");
            int sum = 0;
            for (int i = 0; i < x; i++)
            {
                sum = sum + i;
            }
            Console.WriteLine($"Child Thread Ended. Id : {Thread.CurrentThread.ManagedThreadId}; Thread Pool: {Thread.CurrentThread.IsThreadPoolThread}; Background: {Thread.CurrentThread.IsBackground}");

            return sum;
        }

        static void TaskExample5_TaskReturnValue(int x)
        {
            Console.WriteLine($"Main Thread Started. Id : {Thread.CurrentThread.ManagedThreadId}; Thread Pool: {Thread.CurrentThread.IsThreadPoolThread}; Background: {Thread.CurrentThread.IsBackground}");

            Task<int> t1 = Task.Run(() => CalculateSum(x));
            Console.WriteLine($"Sum: {t1.Result}");//Result is blocking the Main Thread to finish.

            Console.WriteLine($"Main Thread Ended. Id : {Thread.CurrentThread.ManagedThreadId}; Thread Pool: {Thread.CurrentThread.IsThreadPoolThread}; Background: {Thread.CurrentThread.IsBackground}");
        }

        static Character GenerateCharacter()
        {
            Console.WriteLine($"Child Thread Started. Id : {Thread.CurrentThread.ManagedThreadId}; Thread Pool: {Thread.CurrentThread.IsThreadPoolThread}; Background: {Thread.CurrentThread.IsBackground}");
            Character newCharacter = new Character()
            {
                Name = "Amir",
                Age = 26
            };
            Console.WriteLine($"Child Thread Ended. Id : {Thread.CurrentThread.ManagedThreadId}; Thread Pool: {Thread.CurrentThread.IsThreadPoolThread}; Background: {Thread.CurrentThread.IsBackground}");

            return newCharacter;
        }

        static void TaskExample6_TaskReturnClass()
        {
            Console.WriteLine($"Main Thread Started. Id : {Thread.CurrentThread.ManagedThreadId}; Thread Pool: {Thread.CurrentThread.IsThreadPoolThread}; Background: {Thread.CurrentThread.IsBackground}");

            Task<Character> characterAsync = Task.Run(() => GenerateCharacter());

            Character newCharacter = characterAsync.Result;

            Console.WriteLine($"Name : {newCharacter.Name}; Age : {newCharacter.Age}");

            Console.WriteLine($"Main Thread Ended. Id : {Thread.CurrentThread.ManagedThreadId}; Thread Pool: {Thread.CurrentThread.IsThreadPoolThread}; Background: {Thread.CurrentThread.IsBackground}");
        }

        static void AsyncExample1_SynchronousExample()
        {
            var sw = new Stopwatch();
            sw.Start();
            f1_sync();
            f2_sync();
            f3_sync();
            Console.WriteLine($"Time elapsed : {sw.ElapsedMilliseconds * 0.001:N2} s ");
        }

        static void f1_sync()
        {

            Console.WriteLine($"Function 1 Child Thread Started. Id : {Thread.CurrentThread.ManagedThreadId}; Thread Pool: {Thread.CurrentThread.IsThreadPoolThread}; Background: {Thread.CurrentThread.IsBackground}");
            Thread.Sleep(4000);
            Console.WriteLine($"Function 1 Child Thread Ended. Id : {Thread.CurrentThread.ManagedThreadId}; Thread Pool: {Thread.CurrentThread.IsThreadPoolThread}; Background: {Thread.CurrentThread.IsBackground}");
        }
        static void f2_sync()
        {

            Console.WriteLine($"Function 2 Child Thread Started. Id : {Thread.CurrentThread.ManagedThreadId}; Thread Pool: {Thread.CurrentThread.IsThreadPoolThread}; Background: {Thread.CurrentThread.IsBackground}");
            Thread.Sleep(7000);
            Console.WriteLine($"Function 2 Child Thread Ended. Id : {Thread.CurrentThread.ManagedThreadId}; Thread Pool: {Thread.CurrentThread.IsThreadPoolThread}; Background: {Thread.CurrentThread.IsBackground}");
        }
        static void f3_sync()
        {

            Console.WriteLine($"Function 3 Child Thread Started. Id : {Thread.CurrentThread.ManagedThreadId}; Thread Pool: {Thread.CurrentThread.IsThreadPoolThread}; Background: {Thread.CurrentThread.IsBackground}");
            Thread.Sleep(2000);
            Console.WriteLine($"Function 3 Child Thread Ended. Id : {Thread.CurrentThread.ManagedThreadId}; Thread Pool: {Thread.CurrentThread.IsThreadPoolThread}; Background: {Thread.CurrentThread.IsBackground}");
        }


        static async Task AsyncExample2_AsyncExample()
        {
            Console.WriteLine($"Main Thread Started. Id : {Thread.CurrentThread.ManagedThreadId}; Thread Pool: {Thread.CurrentThread.IsThreadPoolThread}; Background: {Thread.CurrentThread.IsBackground}");
            var sw = new Stopwatch();
            sw.Start();
            await Task.WhenAll(f1_async(), f2_async(), f3_async());

            Console.WriteLine($"Main Thread Ended. Id : {Thread.CurrentThread.ManagedThreadId}; Thread Pool: {Thread.CurrentThread.IsThreadPoolThread}; Background: {Thread.CurrentThread.IsBackground}");
            Console.WriteLine($"Time elapsed : {sw.ElapsedMilliseconds * 0.001:N2} s ");
        }

        static async Task f1_async()
        {
            Console.WriteLine($"Function 1 Child Thread Started. Id : {Thread.CurrentThread.ManagedThreadId}; Thread Pool: {Thread.CurrentThread.IsThreadPoolThread}; Background: {Thread.CurrentThread.IsBackground}");
            await Task.Delay(2000);
            Console.WriteLine($"Function 1 Child Thread Ended. Id : {Thread.CurrentThread.ManagedThreadId}; Thread Pool: {Thread.CurrentThread.IsThreadPoolThread}; Background: {Thread.CurrentThread.IsBackground}");
        }
        static async Task f2_async()
        {
            Console.WriteLine($"Function 2 Child Thread Started. Id : {Thread.CurrentThread.ManagedThreadId}; Thread Pool: {Thread.CurrentThread.IsThreadPoolThread}; Background: {Thread.CurrentThread.IsBackground}");
            await Task.Delay(7000);
            Console.WriteLine($"Function 2 Child Thread Ended. Id : {Thread.CurrentThread.ManagedThreadId}; Thread Pool: {Thread.CurrentThread.IsThreadPoolThread}; Background: {Thread.CurrentThread.IsBackground}");
        }
        static async Task f3_async()
        {
            Console.WriteLine($"Function 3 Child Thread Started. Id : {Thread.CurrentThread.ManagedThreadId}; Thread Pool: {Thread.CurrentThread.IsThreadPoolThread}; Background: {Thread.CurrentThread.IsBackground}");
            await Task.Delay(2000);
            Console.WriteLine($"Function 3 Child Thread Ended. Id : {Thread.CurrentThread.ManagedThreadId}; Thread Pool: {Thread.CurrentThread.IsThreadPoolThread}; Background: {Thread.CurrentThread.IsBackground}");
        }



    }

    class Character
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}