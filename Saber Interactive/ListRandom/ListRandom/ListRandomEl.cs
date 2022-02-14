using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("TestListRandom")]

namespace ListRandom
{
    public class ListRandomEl
    {
        public ListNode Head;
        public ListNode Tail;
        public int Count;

        public void Serialize(Stream s)
        {
            CheckStream(s);
            var nodes = GetNodeListBySerialize();
            CheckNodes(nodes);

            using var sw = new StreamWriter(s);
            foreach (var node in nodes)
            {
                sw.WriteLine($"{node.Data}:{nodes.IndexOf(node.Random)}");
            }
        }

        public void Deserialize(Stream s)
        {
            CheckStream(s);

            using var sr = new StreamReader(s);
            var strArray = sr.ReadToEnd().Trim().Split(
                new[] { "\r\n", "\r", "\n" },
                StringSplitOptions.None);

            var nodes = GetNodeListByDeserialize(strArray);
            CheckNodes(nodes);

            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i] = ParseNode(nodes[i], nodes); // тут подумать, как-то кривовато
            }
        }

        private ListNode ParseNode(ListNode node, List<ListNode> nodes)
        {
            var randNodeNum = int.Parse(node.Data.Split(':')[1]);
            node.Random = randNodeNum >= 0 ? nodes[randNodeNum] : null;
            node.Data = node.Data.Split(':')[0];
            return node;
        }

        private void CheckStream(Stream s)
        {
            if (s == null) throw new ArgumentNullException("Пустой поток");
        }

        private void CheckNodes(List<ListNode> nodes)
        {
            if (nodes == null) throw new ArgumentNullException("Нет данных");
        }

        private List<ListNode> GetNodeListBySerialize()
        {
            var nodes = new List<ListNode>();
            ListNode temp = Head;

            while (temp != null)
            {
                nodes.Add(temp);
                temp = temp.Next;
            }

            return !nodes.Any() ? null : nodes;
        }

        private List<ListNode> GetNodeListByDeserialize(string[] arr)
        {
            if (arr[0] == "") return null;

            var nodes = new List<ListNode>();
            ListNode temp = new ListNode();
            Head = temp;
            Count = 0;

            foreach (var nodeLine in arr)
            {
                nodes.Add(temp);

                ListNode nextNode = new ListNode();
                temp.Data = nodeLine;
                temp.Next = nextNode;
                nextNode.Previous = temp;

                temp = nextNode;

                Count++;
            }

            Tail = temp.Previous;
            Tail.Next = null;

            return nodes;
        }
    }
}