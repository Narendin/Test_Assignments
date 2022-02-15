using System;
using System.Collections.Generic;

namespace NmarketTestTask.Models
{
    public class House : IComparable<House>
    {
        public string Name { get; set; }
        public List<Flat> Flats { get; set; }

        public int CompareTo(House other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return int.Parse(Name).CompareTo(int.Parse(other.Name));
        }
    }
}