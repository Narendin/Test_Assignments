using ListRandom;
using System;
using System.IO;
using Xunit;

namespace TestListRandom
{
    public class Tests
    {
        [Fact]
        public void NullStreamSerialize()
        {
            Assert.Throws<ArgumentNullException>(() => new ListRandomEl().Serialize(null));
        }

        [Fact]
        public void NullListSerialize()
        {
            var s = new FileStream("Result.txt", FileMode.OpenOrCreate);
            Assert.Throws<ArgumentNullException>(() => new ListRandomEl().Serialize(s));
        }

        [Fact]
        public void NullStreamDeserialize()
        {
            Assert.Throws<ArgumentNullException>(() => new ListRandomEl().Deserialize(null));
        }

        [Fact]
        public void NullListDeserialize()
        {
            var s = new FileStream("ClearFile.txt", FileMode.OpenOrCreate);
            Assert.Throws<ArgumentNullException>(() => new ListRandomEl().Deserialize(s));
        }
    }
}