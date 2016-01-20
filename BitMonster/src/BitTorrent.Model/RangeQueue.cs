using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitTorrent.Model
{
   public class RangeQueue<T> : Queue<T>
   {
      public void AddRange(T[] bytes)
      {
         foreach (var b in bytes)
         {
            this.Enqueue(b);
         }
      }

      public T[] DequeueRange(int length)
      {
         T[] ret = null;

         if (this.Count >= length)
         {
            ret = new T[length];

            for (int i = 0; i < length; i++)
            {
               ret[i] = this.Dequeue();
            }
         }
         else
         {
            throw new Exception("Wdf");
         }
         
         return ret;
      }

      /// <summary>
      /// Same as DequeueRange but without actually removing the bytes
      /// </summary>
      /// <param name="length"></param>
      /// <returns></returns>
      public T[] DequeueRangePeek(int length)
      {
         T[] ret = null;

         if (this.Count >= length)
         {
            ret = new T[length];
            T[] arr = this.ToArray();

            for (int i = 0; i < length; i++)
            {
               ret[i] = arr[i];
            }
         }
         else
         {
            throw new Exception("Wdf");
         }

         return ret;
      }
   }
}
