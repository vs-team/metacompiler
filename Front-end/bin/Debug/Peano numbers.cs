using System.Collections.Generic;
using System.Linq;
namespace Peano_numbers {
 public static class Extensions { public static V GetKey<T, V>(this System.Collections.Immutable.ImmutableDictionary<T, V> self, T key) { return self[key]; } }
 public interface IRunnable { IEnumerable<IRunnable> Run();
 }


public interface Expr : IRunnable {}
public interface Num : Expr {}



public class _opBang : Expr  {
public Expr P1;

public _opBang(Expr P1) {this.P1 = P1;}
public static _opBang Create(Expr P1) { return new _opBang(P1); }

  public IEnumerable<IRunnable> Run() {   
 { 
 #line 26 ""
var tmp_0 = this; var tmp_1 = tmp_0.P1 as z; 
 #line 26 ""
if (tmp_1 != null) { 
 #line 26 ""
var result = z.Create();
 #line 26 ""
yield return result;  }
 } 

  
 { 
 #line 29 ""
var tmp_0 = this; var tmp_1 = tmp_0.P1 as s; 
 #line 29 ""
if (tmp_1 != null) { var a = tmp_1.P1; 
 #line 29 ""
if(a is Num) { 
 #line 29 ""
var result = s.Create(a as Num);
 #line 29 ""
yield return result;  } }
 } 

  
 { 
 #line 32 ""
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opAddition; 
 #line 32 ""
if (tmp_1 != null) { var a = tmp_1.P1; var b = tmp_1.P2; 
 #line 32 ""
if(a is Expr) { 
 #line 32 ""
var tmp_3 = _opBang.Create(a as Expr);
 #line 32 ""
foreach (var tmp_2 in tmp_3.Run()) { var a_Prime = tmp_2; 
 #line 32 ""
if(b is Expr) { 
 #line 32 ""
var tmp_5 = _opBang.Create(b as Expr);
 #line 32 ""
foreach (var tmp_4 in tmp_5.Run()) { var b_Prime = tmp_4; 
 #line 32 ""
if(a_Prime is Expr && b_Prime is Expr) { 
 #line 32 ""
var tmp_7 = _opAddition.Create(a_Prime as Expr, b_Prime as Expr);
 #line 32 ""
foreach (var tmp_6 in tmp_7.Run()) { var c = tmp_6; 
 #line 32 ""
var result = c;
 #line 32 ""
yield return result;  } } } } } } }
 } 

  
 { 
 #line 38 ""
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opMultiplication; 
 #line 38 ""
if (tmp_1 != null) { var a = tmp_1.P1; var b = tmp_1.P2; 
 #line 38 ""
if(a is Expr) { 
 #line 38 ""
var tmp_3 = _opBang.Create(a as Expr);
 #line 38 ""
foreach (var tmp_2 in tmp_3.Run()) { var a_Prime = tmp_2; 
 #line 38 ""
if(b is Expr) { 
 #line 38 ""
var tmp_5 = _opBang.Create(b as Expr);
 #line 38 ""
foreach (var tmp_4 in tmp_5.Run()) { var b_Prime = tmp_4; 
 #line 38 ""
if(a_Prime is Expr && b_Prime is Expr) { 
 #line 38 ""
var tmp_7 = _opMultiplication.Create(a_Prime as Expr, b_Prime as Expr);
 #line 38 ""
foreach (var tmp_6 in tmp_7.Run()) { var c = tmp_6; 
 #line 38 ""
var result = c;
 #line 38 ""
yield return result;  } } } } } } }
 } 

  }


public override string ToString() {
 var res = "("; 

 res += " ! "; res += P1.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _opBang;
 if(tmp != null) return this.P1.Equals(tmp.P1); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class _opMultiplication : Expr  {
public Expr P1;
public Expr P2;

public _opMultiplication(Expr P1, Expr P2) {this.P1 = P1; this.P2 = P2;}
public static _opMultiplication Create(Expr P1, Expr P2) { return new _opMultiplication(P1, P2); }

  public IEnumerable<IRunnable> Run() {   
 { 
 #line 17 ""
var tmp_0 = this; var tmp_1 = tmp_0.P1 as z; 
 #line 17 ""
if (tmp_1 != null) { var a = tmp_0.P2; 
 #line 17 ""
var result = z.Create();
 #line 17 ""
yield return result;  }
 } 

  
 { 
 #line 20 ""
var tmp_0 = this; var tmp_1 = tmp_0.P1 as s; 
 #line 20 ""
if (tmp_1 != null) { var a = tmp_1.P1; var b = tmp_0.P2; 
 #line 20 ""
if(a is Expr && b is Expr) { 
 #line 20 ""
var tmp_3 = _opMultiplication.Create(a as Expr, b as Expr);
 #line 20 ""
foreach (var tmp_2 in tmp_3.Run()) { var c = tmp_2; 
 #line 20 ""
if(c is Expr && b is Expr) { 
 #line 20 ""
var tmp_5 = _opAddition.Create(c as Expr, b as Expr);
 #line 20 ""
foreach (var tmp_4 in tmp_5.Run()) { var d = tmp_4; 
 #line 20 ""
var result = d;
 #line 20 ""
yield return result;  } } } } }
 } 

  }


public override string ToString() {
 var res = "("; 
res += P1.ToString(); 

 res += " * "; res += P2.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _opMultiplication;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class _opAddition : Expr  {
public Expr P1;
public Expr P2;

public _opAddition(Expr P1, Expr P2) {this.P1 = P1; this.P2 = P2;}
public static _opAddition Create(Expr P1, Expr P2) { return new _opAddition(P1, P2); }

  public IEnumerable<IRunnable> Run() {   
 { 
 #line 10 ""
var tmp_0 = this; var tmp_1 = tmp_0.P1 as z; 
 #line 10 ""
if (tmp_1 != null) { var a = tmp_0.P2; 
 #line 10 ""
var result = a;
 #line 10 ""
yield return result;  }
 } 

  
 { 
 #line 13 ""
var tmp_0 = this; var tmp_1 = tmp_0.P1 as s; 
 #line 13 ""
if (tmp_1 != null) { var a = tmp_1.P1; var b = tmp_0.P2; 
 #line 13 ""
if(a is Expr && b is Expr) { 
 #line 13 ""
var tmp_3 = _opAddition.Create(a as Expr, b as Expr);
 #line 13 ""
foreach (var tmp_2 in tmp_3.Run()) { var c = tmp_2; 
 #line 13 ""
if(c is Num) { 
 #line 13 ""
var result = s.Create(c as Num);
 #line 13 ""
yield return result;  } } } }
 } 

  }


public override string ToString() {
 var res = "("; 
res += P1.ToString(); 

 res += " + "; res += P2.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _opAddition;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class s : Num  {
public Num P1;

public s(Num P1) {this.P1 = P1;}
public static s Create(Num P1) { return new s(P1); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }

public override string ToString() {
 var res = "("; 

 res += " s "; res += P1.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as s;
 if(tmp != null) return this.P1.Equals(tmp.P1); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class z : Num  {

public z() {}
public static z Create() { return new z(); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }

public override string ToString() {
return "z";
}

public override bool Equals(object other) {
 return other is z; 
}

public override int GetHashCode() {
 return 0; 
}

}




public class EntryPoint {
 static public int Print(object s) { System.Console.WriteLine(s.ToString()); return 0; } 
 static public int Sleep(float s) { int t = (int)(s * 1000.0f); System.Threading.Thread.Sleep(t); return 0; } 
static public IEnumerable<IRunnable> Run(bool printInput)
{
 #line 1 "input"
 var p = _opBang.Create(_opMultiplication.Create(_opMultiplication.Create(s.Create(s.Create(z.Create())), s.Create(s.Create(z.Create()))), _opAddition.Create(s.Create(s.Create(z.Create())), s.Create(z.Create()))));
if(printInput) System.Console.WriteLine(p.ToString());
foreach(var x in p.Run())
yield return x;
}
}

}
