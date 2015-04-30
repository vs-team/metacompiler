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

  public static IEnumerable<IRunnable> StaticRun(Expr P1) {   
 { 
 var tmp_0 = P1 as z; 
if (tmp_0 != null) { 
var result = z.Create();
yield return result;  }
 } 

  
 { 
 var tmp_0 = P1 as s; 
if (tmp_0 != null) { var a = tmp_0.P1; 
if(a is Num) { 
var result = s.Create(a as Num);
yield return result;  } }
 } 

  
 { 
 var tmp_0 = P1 as _opAddition; 
if (tmp_0 != null) { var a = tmp_0.P1; var b = tmp_0.P2; 
if(a is Expr) { 
var tmp_2 = _opBang.Create(a as Expr);
foreach (var tmp_1 in tmp_2.Run()) { var a_Prime = tmp_1; 
if(b is Expr) { 
var tmp_4 = _opBang.Create(b as Expr);
foreach (var tmp_3 in tmp_4.Run()) { var b_Prime = tmp_3; 
if(a_Prime is Expr && b_Prime is Expr) { 
var tmp_6 = _opAddition.Create(a_Prime as Expr, b_Prime as Expr);
foreach (var tmp_5 in tmp_6.Run()) { var c = tmp_5; 
var result = c;
yield return result;  } } } } } } }
 } 

  
 { 
 var tmp_0 = P1 as _opMultiplication; 
if (tmp_0 != null) { var a = tmp_0.P1; var b = tmp_0.P2; 
if(a is Expr) { 
var tmp_2 = _opBang.Create(a as Expr);
foreach (var tmp_1 in tmp_2.Run()) { var a_Prime = tmp_1; 
if(b is Expr) { 
var tmp_4 = _opBang.Create(b as Expr);
foreach (var tmp_3 in tmp_4.Run()) { var b_Prime = tmp_3; 
if(a_Prime is Expr && b_Prime is Expr) { 
var tmp_6 = _opMultiplication.Create(a_Prime as Expr, b_Prime as Expr);
foreach (var tmp_5 in tmp_6.Run()) { var c = tmp_5; 
var result = c;
yield return result;  } } } } } } }
 } 

  }
public IEnumerable<IRunnable> Run() { return StaticRun(P1); }


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

  public static IEnumerable<IRunnable> StaticRun(Expr P1, Expr P2) {   
 { 
 var tmp_0 = P1 as z; 
if (tmp_0 != null) { var a = P2; 
var result = z.Create();
yield return result;  }
 } 

  
 { 
 var tmp_0 = P1 as s; 
if (tmp_0 != null) { var a = tmp_0.P1; var b = P2; 
if(a is Expr && b is Expr) { 
var tmp_2 = _opMultiplication.Create(a as Expr, b as Expr);
foreach (var tmp_1 in tmp_2.Run()) { var c = tmp_1; 
if(c is Expr && b is Expr) { 
var tmp_4 = _opAddition.Create(c as Expr, b as Expr);
foreach (var tmp_3 in tmp_4.Run()) { var d = tmp_3; 
var result = d;
yield return result;  } } } } }
 } 

  }
public IEnumerable<IRunnable> Run() { return StaticRun(P1, P2); }


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

  public static IEnumerable<IRunnable> StaticRun(Expr P1, Expr P2) {   
 { 
 var tmp_0 = P1 as z; 
if (tmp_0 != null) { var a = P2; 
var result = a;
yield return result;  }
 } 

  
 { 
 var tmp_0 = P1 as s; 
if (tmp_0 != null) { var a = tmp_0.P1; var b = P2; 
if(a is Expr && b is Expr) { 
var tmp_2 = _opAddition.Create(a as Expr, b as Expr);
foreach (var tmp_1 in tmp_2.Run()) { var c = tmp_1; 
if(c is Num) { 
var result = s.Create(c as Num);
yield return result;  } } } }
 } 

  }
public IEnumerable<IRunnable> Run() { return StaticRun(P1, P2); }


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


public static IEnumerable<IRunnable> StaticRun(Num P1) { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run(){ return StaticRun(P1); }

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


public static IEnumerable<IRunnable> StaticRun() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run(){ return StaticRun(); }

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
 static public int Sleep(float s) { int t = (int)(s * 1000.0f); ; return 0; } 
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
