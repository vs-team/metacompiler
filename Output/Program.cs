using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Output
{
  public static class Program
  {
    public static void Main()
    {
      var result = new EntryPoint().Run().ToList();
      foreach (var x in result)
      {
        Console.WriteLine(x.ToString());
      }
      Console.ReadLine();
    }
  }
}
