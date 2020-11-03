using System;

namespace nuce.web.model
{
   public class RandomInt: IEquatable<RandomInt>, IComparable<RandomInt>
    {
        public int Value { get; set; }
        public Guid Key { get; set; }

        public bool Equals(RandomInt other)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(RandomInt other)
        {
            throw new NotImplementedException();
        }
    }
}
