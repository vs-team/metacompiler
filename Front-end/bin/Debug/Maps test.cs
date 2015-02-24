using System.Collections.Generic;
using System.Linq;
namespace Maps_test {
 public static class Extensions { public static V GetKey<T, V>(this System.Collections.Immutable.ImmutableDictionary<T, V> self, T key) { return self[key]; } }
 public interface IRunnable { IEnumerable<IRunnable> Run();
 }


public interface Expr : IRunnable {}
public interface MapIntString : Expr {}



public class _opDollar : MapIntString  {
public System.Collections.Immutable.ImmutableDictionary<int, string> P1;

public _opDollar(System.Collections.Immutable.ImmutableDictionary<int, string> P1) {this.P1 = P1;}
public static _opDollar Create(System.Collections.Immutable.ImmutableDictionary<int, string> P1) { return new _opDollar(P1); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }

public override string ToString() {
 var res = "("; 

 res += "$"; if (P1 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P1.ToString(); } 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _opDollar;
 if(tmp != null) return this.P1.Equals(tmp.P1); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class add : Expr  {
public MapIntString P1;
public int P2;
public string P3;

public add(MapIntString P1, int P2, string P3) {this.P1 = P1; this.P2 = P2; this.P3 = P3;}
public static add Create(MapIntString P1, int P2, string P3) { return new add(P1, P2, P3); }

  public IEnumerable<IRunnable> Run() {   
 { 
 #line 7 "Content\Maps test\transform.mc"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opDollar; 
 #line 7 "Content\Maps test\transform.mc"
if (tmp_1 != null) { var M = tmp_1.P1; var k = tmp_0.P2; var v = tmp_0.P3; var M_Prime = (M.Add (k,v)); 
 #line 7 "Content\Maps test\transform.mc"
if(M_Prime is System.Collections.Immutable.ImmutableDictionary<int, string>) { 
 #line 7 "Content\Maps test\transform.mc"
var result = _opDollar.Create(M_Prime as System.Collections.Immutable.ImmutableDictionary<int, string>);
 #line 7 "Content\Maps test\transform.mc"
yield return result;  } }
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

public override int GetHashCode() {
 return 0; 
}

}

public class run : Expr  {
public MapIntString P1;

public run(MapIntString P1) {this.P1 = P1;}
public static run Create(MapIntString P1) { return new run(P1); }

  public IEnumerable<IRunnable> Run() {   
 { 
 #line 11 "Content\Maps test\transform.mc"
var tmp_0 = this; var M = tmp_0.P1; 
 #line 11 "Content\Maps test\transform.mc"
if(M is MapIntString) { 
 #line 11 "Content\Maps test\transform.mc"
var tmp_2 = add.Create(M as MapIntString, 1, "one");
 #line 11 "Content\Maps test\transform.mc"
foreach (var tmp_1 in tmp_2.Run()) { var M1 = tmp_1; 
 #line 11 "Content\Maps test\transform.mc"
if(M1 is MapIntString) { 
 #line 11 "Content\Maps test\transform.mc"
var tmp_4 = add.Create(M1 as MapIntString, 2, "two");
 #line 11 "Content\Maps test\transform.mc"
foreach (var tmp_3 in tmp_4.Run()) { var M2 = tmp_3; 
 #line 11 "Content\Maps test\transform.mc"
if(M2 is MapIntString) { 
 #line 11 "Content\Maps test\transform.mc"
var tmp_6 = add.Create(M2 as MapIntString, 3, "three");
 #line 11 "Content\Maps test\transform.mc"
foreach (var tmp_5 in tmp_6.Run()) { var M3 = tmp_5; 
 #line 11 "Content\Maps test\transform.mc"
var result = M3;
 #line 11 "Content\Maps test\transform.mc"
yield return result;  } } } } } }
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

public override int GetHashCode() {
 return 0; 
}

}




public class EntryPoint {
 static public int Print(string s) { System.Console.WriteLine(s); return 0; } 
static public IEnumerable<IRunnable> Run(bool printInput)
{
 #line 1 "input"
 var p = run.Create(_opDollar.Create(System.Collections.Immutable.ImmutableDictionary<int,string >.Empty));
if(printInput) System.Console.WriteLine(p.ToString());
foreach(var x in p.Run())
yield return x;
}
}

}
