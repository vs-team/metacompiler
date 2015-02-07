using System.Collections.Generic;
using System.Linq;
namespace Maps_test {
 public interface IRunnable { IEnumerable<IRunnable> Run();
 }


public interface Expr : IRunnable {}
public interface MapIntString : IRunnable {}



public class _Dollar : MapIntString {
public System.Collections.Immutable.ImmutableDictionary<int, string> P1;

public _Dollar(System.Collections.Immutable.ImmutableDictionary<int, string> P1) {this.P1 = P1;}


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }

public override string ToString() {
 var res = "("; 

 res += "$"; res += P1.ToString();

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _Dollar;
 if(tmp != null) return this.P1.Equals(tmp.P1); 
 else return false; }
}

public class add : Expr {
public MapIntString P1;
public int P2;
public string P3;

public add(MapIntString P1, int P2, string P3) {this.P1 = P1; this.P2 = P2; this.P3 = P3;}

  public IEnumerable<IRunnable> Run() {   
 { 
 var tmp_0 = this; var tmp_1 = tmp_0.P1 as _Dollar; if (tmp_1 != null) { var M = tmp_1.P1; var k = tmp_0.P2; var v = tmp_0.P3; var M_Prime = M.Add(k,v); if(M_Prime is System.Collections.Immutable.ImmutableDictionary<int, string>) { var result = new _Dollar(M_Prime as System.Collections.Immutable.ImmutableDictionary<int, string>);yield return result;  } }
 } 

  }


public override string ToString() {
 var res = "("; 
res += P1.ToString();

 res += "add"; res += P2.ToString();
res += P3.ToString();

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as add;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2) && this.P3.Equals(tmp.P3); 
 else return false; }
}

public class run : Expr {
public MapIntString P1;

public run(MapIntString P1) {this.P1 = P1;}

  public IEnumerable<IRunnable> Run() {   
 { 
 var tmp_0 = this; var M = tmp_0.P1; if(M is MapIntString) { var tmp_2 = new add(M as MapIntString, 1, "one");foreach (var tmp_1 in tmp_2.Run()) { var M1 = tmp_1; if(M1 is MapIntString) { var tmp_4 = new add(M1 as MapIntString, 2, "two");foreach (var tmp_3 in tmp_4.Run()) { var M2 = tmp_3; if(M2 is MapIntString) { var tmp_6 = new add(M2 as MapIntString, 3, "three");foreach (var tmp_5 in tmp_6.Run()) { var M3 = tmp_5; var result = M3;yield return result;  } } } } } }
 } 

  }


public override string ToString() {
 var res = "("; 

 res += "run"; res += P1.ToString();

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as run;
 if(tmp != null) return this.P1.Equals(tmp.P1); 
 else return false; }
}




public class EntryPoint {
 static public IEnumerable<IRunnable> Run(bool printInput)
{
var p = new run(new _Dollar(System.Collections.Immutable.ImmutableDictionary <int,string>.Empty));
if(printInput) System.Console.WriteLine(p.ToString());
foreach(var x in p.Run())
yield return x;
}
}

}
