using System;
using System.Collections.Generic;
using System.Linq;
namespace stsil {
 public static class Extensions { public static V GetKey<T, V>(this System.Collections.Immutable.ImmutableDictionary<T, V> self, T key) { return self[key]; } }
 public interface IRunnable { IEnumerable<IRunnable> Run();
 }


public interface Expr : IRunnable {}
public interface IntValue : IRunnable {}
public interface ListInt : IRunnable {}



public class _opDollar : IntValue  {
public int P1;

public _opDollar(int P1) {this.P1 = P1;}
public static _opDollar Create(int P1) { return new _opDollar(P1); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }

public override string ToString() {
 var res = "("; 

 res += " $ "; res += P1.ToString(); 

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

public class dda : Expr  {
public ListInt P1;

public dda(ListInt P1) {this.P1 = P1;}
public static dda Create(ListInt P1) { return new dda(P1); }

  public IEnumerable<IRunnable> Run() {   
 { 
 #line 14 "Content\stsil"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as lin; 
 #line 14 "Content\stsil"
if (tmp_1 != null) { 
 #line 14 "Content\stsil"
var result = _opDollar.Create(0);
 #line 14 "Content\stsil"
yield return result;  }
 } 

  
 { 
 #line 19 "Content\stsil"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as snoc; 
 #line 19 "Content\stsil"
if (tmp_1 != null) { var xs = tmp_1.P1; var x = tmp_1.P2; 
 #line 19 "Content\stsil"
if(xs is ListInt) { 
 #line 19 "Content\stsil"
var tmp_3 = dda.Create(xs as ListInt);
 #line 19 "Content\stsil"
foreach (var tmp_2 in tmp_3.Run()) { var tmp_4 = tmp_2 as _opDollar; 
 #line 19 "Content\stsil"
if (tmp_4 != null) { var res = tmp_4.P1; 
 #line 19 "Content\stsil"
var result = _opDollar.Create(x+res);
 #line 19 "Content\stsil"
yield return result;  } } } }
 } 

  }


public override string ToString() {
 var res = "("; 

 res += " dda "; res += P1.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as dda;
 if(tmp != null) return this.P1.Equals(tmp.P1); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class lin : ListInt  {

public lin() {}
public static lin Create() { return new lin(); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }

public override string ToString() {
return "lin";
}

public override bool Equals(object other) {
 return other is lin; 
}

public override int GetHashCode() {
 return 0; 
}

}

public class snoc : ListInt  {
public ListInt P1;
public int P2;

public snoc(ListInt P1, int P2) {this.P1 = P1; this.P2 = P2;}
public static snoc Create(ListInt P1, int P2) { return new snoc(P1, P2); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }

public override string ToString() {
 var res = "("; 
res += P1.ToString(); 

 res += " snoc "; res += P2.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as snoc;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}




public class EntryPoint {
 static public int Sleep(float s) { int t = (int)(s * 1000.0f); ; return 0; } 
static public IEnumerable<IRunnable> Run(bool printInput)
{
 #line 1 "input"
 var p = dda.Create(snoc.Create(snoc.Create(snoc.Create(lin.Create(), 3), 2), 1));
if(printInput) System.Console.WriteLine(p.ToString());
foreach(var x in p.Run())
yield return x;
}
}

}
