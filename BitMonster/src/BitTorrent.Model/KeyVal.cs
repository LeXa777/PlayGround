using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitTorrent.Model
{
   public class KeyVal<TKey, TVal>
   {
      public TKey Key { get; set; }
      public TVal Value { get; set; }

      public KeyVal(TKey key, TVal value)
      {
         Key = key;
         Value = value;
      }
   }
}
