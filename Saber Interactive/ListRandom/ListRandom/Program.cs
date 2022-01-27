using System;
using System.IO;

namespace ListRandom
{
    internal class Program
    {
        private static void Main()
        {
            try
            {
                ListRandomEl listRandomEl = GetListRandom(10);

                FileStream fs = new FileStream("Result.txt", FileMode.OpenOrCreate);
                listRandomEl.Serialize(fs);

                ListRandomEl newListRandomEl = new ListRandomEl();

                fs = new FileStream("Result.txt", FileMode.Open);
                newListRandomEl.Deserialize(fs);

                fs = new FileStream("NewResult.txt", FileMode.OpenOrCreate);
                newListRandomEl.Serialize(fs);
                FileCompare("Result.txt", "NewResult.txt");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                Console.WriteLine("Для выхода из программы нажмите любую клавищу...");
                Console.ReadKey();
            }
        }

        private static ListRandomEl GetListRandom(int count)
        {
            ListNode head = new ListNode();
            head.Data = GetRandomString();

            ListNode tail = head;
            for (int i = 1; i < count; i++)
            {
                tail = AddNode(tail);
            }

            ListNode temp = head;
            for (int i = 1; i < count; i++)
            {
                temp.Random = GetRandNode(head, count);
                temp = temp.Next;
            }

            ListRandomEl listRandomEl = new ListRandomEl();
            listRandomEl.Head = head;
            listRandomEl.Tail = tail;
            listRandomEl.Count = count;

            return listRandomEl;
        }

        private static ListNode AddNode(ListNode node)
        {
            ListNode next = new ListNode();
            next.Previous = node;
            next.Data = GetRandomString();
            node.Next = next;
            return next;
        }

        private static string GetRandomString()
        {
            string path = Path.GetRandomFileName();
            path = path.Replace(".", "");
            return path;
        }

        private static ListNode GetRandNode(ListNode head, int length)
        {
            Random rand = new Random();
            int randNodeNum = rand.Next(0, length);

            ListNode randNode = head;

            while (randNodeNum > 0)
            {
                randNode = randNode.Next;
                randNodeNum--;
            }

            return randNode;
        }

        private static void FileCompare(string path1, string path2)
        {
            StreamReader s1 = new StreamReader(path1);
            StreamReader s2 = new StreamReader(path2);

            string file1 = s1.ReadToEnd();
            string file2 = s2.ReadToEnd();

            if (file1 == file2)
            {
                Console.WriteLine("Файлы одинаковые, сериализация и десериализация прошли успешно.");
            }
            else
            {
                Console.WriteLine("Файлы разные. Где-то ошибка.");
            }
        }
    }
}