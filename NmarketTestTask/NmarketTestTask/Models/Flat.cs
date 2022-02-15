using System;

namespace NmarketTestTask.Models
{
    public class Flat : IComparable<Flat>
    {
        public string Number { get; set; }
        public string Price { get; set; }

        public int CompareTo(Flat other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return int.Parse(Number).CompareTo(int.Parse(other.Number));
        }
    }
}