using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mxp.Win
{
    public class Group<Tkey, TElement> : List<TElement>
    {
        public Tkey Key { get; set; }
        public Group(IGrouping<Tkey, TElement> group)
            : base(group)
        {
            this.Key = group.Key;
        }
        public override bool Equals(object obj)
        {
            Group<Tkey, TElement> that = obj as Group<Tkey, TElement>;
            return (that != null) && (this.Key.Equals(that.Key));
        }
        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }
    }
}