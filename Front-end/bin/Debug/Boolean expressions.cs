using System;
using System.Collections.Generic;
using System.Linq;
namespace Boolean_expressions {
 public static class Extensions { public static V GetKey<T, V>(this System.Collections.Immutable.ImmutableDictionary<T, V> self, T key) { return self[key]; } }
 public interface IRunnable { IEnumerable<IRunnable> Run();
IEnumerable<IRunnable> Run0_();
 }


public interface Expr : IRunnable {}
public interface Value : Expr {}



public class _opBang : Expr  {
public Expr P1;

public _opBang(Expr P1) {this.P1 = P1;}
public static _opBang Create(Expr P1) { return new _opBang(P1); }


public static IEnumerable<IRunnable> StaticRun(Expr P1) { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run(){ return StaticRun(P1); }
public static IEnumerable<IRunnable> StaticRun0_(Expr P1) { return StaticRun(P1); }
public IEnumerable<IRunnable> Run0_(){ return StaticRun0_(P1); }

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

public class _opBitwiseAnd : Expr  {
public Expr P1;
public Expr P2;

public _opBitwiseAnd(Expr P1, Expr P2) {this.P1 = P1; this.P2 = P2;}
public static _opBitwiseAnd Create(Expr P1, Expr P2) { return new _opBitwiseAnd(P1, P2); }


public static IEnumerable<IRunnable> StaticRun(Expr P1, Expr P2) { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run(){ return StaticRun(P1, P2); }
public static IEnumerable<IRunnable> StaticRun0_(Expr P1, Expr P2) { return StaticRun(P1, P2); }
public IEnumerable<IRunnable> Run0_(){ return StaticRun0_(P1, P2); }

public override string ToString() {
 var res = "("; 
res += P1.ToString(); 

 res += " & "; res += P2.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _opBitwiseAnd;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class FALSE : Value  {

public FALSE() {}
public static FALSE Create() { return new FALSE(); }


public static IEnumerable<IRunnable> StaticRun() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run(){ return StaticRun(); }
public static IEnumerable<IRunnable> StaticRun0_() { return StaticRun(); }
public IEnumerable<IRunnable> Run0_(){ return StaticRun0_(); }

public override string ToString() {
return "FALSE";
}

public override bool Equals(object other) {
 return other is FALSE; 
}

public override int GetHashCode() {
 return 0; 
}

}

public class TRUE : Value  {

public TRUE() {}
public static TRUE Create() { return new TRUE(); }


public static IEnumerable<IRunnable> StaticRun() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run(){ return StaticRun(); }
public static IEnumerable<IRunnable> StaticRun0_() { return StaticRun(); }
public IEnumerable<IRunnable> Run0_(){ return StaticRun0_(); }

public override string ToString() {
return "TRUE";
}

public override bool Equals(object other) {
 return other is TRUE; 
}

public override int GetHashCode() {
 return 0; 
}

}

public class eval : Value  {
public Expr P1;

public eval(Expr P1) {this.P1 = P1;}
public static eval Create(Expr P1) { return new eval(P1); }

  public static IEnumerable<IRunnable> StaticRun0_(Expr P1) {   
 { 
 var tmp_0 = P1 as TRUE; 
if (tmp_0 != null) { 
var result = TRUE.Create();
yield return result;  }
 } 

  
 { 
 var tmp_0 = P1 as FALSE; 
if (tmp_0 != null) { 
var result = FALSE.Create();
yield return result;  }
 } 

  
 { 
 var tmp_0 = P1 as _opBang; 
if (tmp_0 != null) { var a = tmp_0.P1; 
if(a is Expr) { 
var tmp_2 = eval.Create(a as Expr);
foreach (var tmp_1 in tmp_2.Run0_()) { var tmp_3 = tmp_1 as TRUE; 
if (tmp_3 != null) { 
var result = FALSE.Create();
yield return result;  } } } }
 } 

  
 { 
 var tmp_0 = P1 as _opBang; 
if (tmp_0 != null) { var a = tmp_0.P1; 
if(a is Expr) { 
var tmp_2 = eval.Create(a as Expr);
foreach (var tmp_1 in tmp_2.Run0_()) { var tmp_3 = tmp_1 as FALSE; 
if (tmp_3 != null) { 
var result = TRUE.Create();
yield return result;  } } } }
 } 

  
 { 
 var tmp_0 = P1 as _opBitwiseOr; 
if (tmp_0 != null) { var a = tmp_0.P1; var b = tmp_0.P2; 
if(a is Expr) { 
var tmp_2 = eval.Create(a as Expr);
foreach (var tmp_1 in tmp_2.Run0_()) { var tmp_3 = tmp_1 as TRUE; 
if (tmp_3 != null) { 
var result = TRUE.Create();
yield return result;  } } } }
 } 

  
 { 
 var tmp_0 = P1 as _opBitwiseOr; 
if (tmp_0 != null) { var a = tmp_0.P1; var b = tmp_0.P2; 
if(a is Expr) { 
var tmp_2 = eval.Create(a as Expr);
foreach (var tmp_1 in tmp_2.Run0_()) { var tmp_3 = tmp_1 as FALSE; 
if (tmp_3 != null) { 
if(b is Expr) { 
var tmp_5 = eval.Create(b as Expr);
foreach (var tmp_4 in tmp_5.Run0_()) { var y = tmp_4; 
var result = y;
yield return result;  } } } } } }
 } 

  
 { 
 var tmp_0 = P1 as _opBitwiseAnd; 
if (tmp_0 != null) { var a = tmp_0.P1; var b = tmp_0.P2; 
if(a is Expr) { 
var tmp_2 = eval.Create(a as Expr);
foreach (var tmp_1 in tmp_2.Run0_()) { var tmp_3 = tmp_1 as FALSE; 
if (tmp_3 != null) { 
var result = FALSE.Create();
yield return result;  } } } }
 } 

  
 { 
 var tmp_0 = P1 as _opBitwiseAnd; 
if (tmp_0 != null) { var a = tmp_0.P1; var b = tmp_0.P2; 
if(a is Expr) { 
var tmp_2 = eval.Create(a as Expr);
foreach (var tmp_1 in tmp_2.Run0_()) { var tmp_3 = tmp_1 as TRUE; 
if (tmp_3 != null) { 
if(b is Expr) { 
var tmp_5 = eval.Create(b as Expr);
foreach (var tmp_4 in tmp_5.Run0_()) { var y = tmp_4; 
var result = y;
yield return result;  } } } } } }
 } 

 foreach(var p in StaticRun(P1)) yield return p; }
public IEnumerable<IRunnable> Run0_() { return StaticRun0_(P1); }

public static IEnumerable<IRunnable> StaticRun(Expr P1) { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run(){ return StaticRun(P1); }

public override string ToString() {
 var res = "("; 

 res += " eval "; res += P1.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as eval;
 if(tmp != null) return this.P1.Equals(tmp.P1); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class run : Value  {

public run() {}
public static run Create() { return new run(); }

  public static IEnumerable<IRunnable> StaticRun() {   
 { 
 
var tmp_1 = eval.Create(_opBitwiseOr.Create(FALSE.Create(), _opBitwiseAnd.Create(TRUE.Create(), _opBang.Create(FALSE.Create()))));
foreach (var tmp_0 in tmp_1.Run0_()) { var res = tmp_0; 
var result = res;
yield return result;  }
 } 

  }
public IEnumerable<IRunnable> Run() { return StaticRun(); }

public static IEnumerable<IRunnable> StaticRun0_() { return StaticRun(); }
public IEnumerable<IRunnable> Run0_(){ return StaticRun0_(); }

public override string ToString() {
return "run";
}

public override bool Equals(object other) {
 return other is run; 
}

public override int GetHashCode() {
 return 0; 
}

}

public class _opBitwiseOr : Expr  {
public Expr P1;
public Expr P2;

public _opBitwiseOr(Expr P1, Expr P2) {this.P1 = P1; this.P2 = P2;}
public static _opBitwiseOr Create(Expr P1, Expr P2) { return new _opBitwiseOr(P1, P2); }


public static IEnumerable<IRunnable> StaticRun(Expr P1, Expr P2) { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run(){ return StaticRun(P1, P2); }
public static IEnumerable<IRunnable> StaticRun0_(Expr P1, Expr P2) { return StaticRun(P1, P2); }
public IEnumerable<IRunnable> Run0_(){ return StaticRun0_(P1, P2); }

public override string ToString() {
 var res = "("; 
res += P1.ToString(); 

 res += " | "; res += P2.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _opBitwiseOr;
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
 var p = run.Create();
if(printInput) System.Console.WriteLine(p.ToString());
foreach(var x in p.Run())
yield return x;
}
}

}
