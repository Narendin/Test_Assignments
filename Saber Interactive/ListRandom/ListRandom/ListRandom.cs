using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ListRandom
{
    public class ListRandom
    {
        public ListNode Head;
        public ListNode Tail;
        public int Count;

        public void Serialize(Stream s)
        {
            try
            {
                List<ListNode> nodes = new List<ListNode>();
                ListNode temp;
                temp = Head;

                while (temp != null)
                {
                    nodes.Add(temp);
                    temp = temp.Next;
                }

                if (!nodes.Any())
                {
                    Console.WriteLine("Нечего сериализировать.");
                    return;
                }

                using (StreamWriter sw = new StreamWriter(s))
                {
                    foreach (var node in nodes)
                    {
                        sw.WriteLine($"{node.Data}:{nodes.IndexOf(node.Random)}");
                    }
                }

                Console.WriteLine($"Сериализация завершена.");
            }
            catch (Exception e)
            {
                // т.к. нет логирования, то ошибку выведу в консоль
                Console.WriteLine(e);
                throw;
            }
        }

        public void Deserialize(Stream s)
        {
            try
            {
                List<ListNode> nodes = new List<ListNode>();
                ListNode temp = new ListNode();
                Head = temp;
                Count = 0;
                string nodeLine;

                using (StreamReader sr = new StreamReader(s))
                {
                    while ((nodeLine = sr.ReadLine()?.Trim()) != null)
                    {
                        if (nodeLine == "") continue;

                        nodes.Add(temp);

                        ListNode nextNode = new ListNode();
                        temp.Data = nodeLine;
                        temp.Next = nextNode;

                        nextNode.Previous = temp;
                        temp = nextNode;

                        Count++;
                    }
                }

                Tail = temp.Previous;
                Tail.Next = null;

                if (!nodes.Any())
                {
                    Console.WriteLine("Нечего десериализировать.");
                    return;
                }

                foreach (var node in nodes)
                {
                    int randNodeNum = int.Parse(node.Data.Split(':')[1]);
                    node.Random = randNodeNum >= 0 ? nodes[randNodeNum] : null;
                    node.Data = node.Data.Split(':')[0];
                    /*
                     тут оставил допущение, что файл для десериализации был сформирован этой же программой, а значит ссылка на элемент массива всегда число
                     а так же предполагаю, что в поле Data узлов списка будет отсутствовать символ двоеточния.
                     если предполагать, что символ все же будет, то надо либо прыдумывать иной разделитель, либо проверять,
                     что сплит выдает всего два элемента в массив, и если больше - выдавать в логирование ошибку.
                    */
                }
            }
            catch (Exception e)
            {
                // т.к. нет логирования, то ошибку выведу в консоль
                Console.WriteLine(e);
                throw;
            }
        }
    }
}