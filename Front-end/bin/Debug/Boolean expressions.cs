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


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run0_() { foreach(var p in Run()) yield return p; }

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


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run0_() { foreach(var p in Run()) yield return p; }

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


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run0_() { foreach(var p in Run()) yield return p; }

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


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run0_() { foreach(var p in Run()) yield return p; }

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

  public IEnumerable<IRunnable> Run0_() {   
 { 
 #line 30 "Content\Boolean expressions"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as TRUE; 
 #line 30 "Content\Boolean expressions"
if (tmp_1 != null) { 
 #line 30 "Content\Boolean expressions"
var result = TRUE.Create();
 #line 30 "Content\Boolean expressions"
yield return result;  }
 } 

  
 { 
 #line 35 "Content\Boolean expressions"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as FALSE; 
 #line 35 "Content\Boolean expressions"
if (tmp_1 != null) { 
 #line 35 "Content\Boolean expressions"
var result = FALSE.Create();
 #line 35 "Content\Boolean expressions"
yield return result;  }
 } 

  
 { 
 #line 41 "Content\Boolean expressions"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opBang; 
 #line 41 "Content\Boolean expressions"
if (tmp_1 != null) { var a = tmp_1.P1; 
 #line 41 "Content\Boolean expressions"
if(a is Expr) { 
 #line 41 "Content\Boolean expressions"
var tmp_3 = eval.Create(a as Expr);
 #line 41 "Content\Boolean expressions"
foreach (var tmp_2 in tmp_3.Run0_()) { var tmp_4 = tmp_2 as TRUE; 
 #line 41 "Content\Boolean expressions"
if (tmp_4 != null) { 
 #line 41 "Content\Boolean expressions"
var result = FALSE.Create();
 #line 41 "Content\Boolean expressions"
yield return result;  } } } }
 } 

  
 { 
 #line 48 "Content\Boolean expressions"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opBang; 
 #line 48 "Content\Boolean expressions"
if (tmp_1 != null) { var a = tmp_1.P1; 
 #line 48 "Content\Boolean expressions"
if(a is Expr) { 
 #line 48 "Content\Boolean expressions"
var tmp_3 = eval.Create(a as Expr);
 #line 48 "Content\Boolean expressions"
foreach (var tmp_2 in tmp_3.Run0_()) { var tmp_4 = tmp_2 as FALSE; 
 #line 48 "Content\Boolean expressions"
if (tmp_4 != null) { 
 #line 48 "Content\Boolean expressions"
var result = TRUE.Create();
 #line 48 "Content\Boolean expressions"
yield return result;  } } } }
 } 

  
 { 
 #line 56 "Content\Boolean expressions"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opBitwiseOr; 
 #line 56 "Content\Boolean expressions"
if (tmp_1 != null) { var a = tmp_1.P1; var b = tmp_1.P2; 
 #line 56 "Content\Boolean expressions"
if(a is Expr) { 
 #line 56 "Content\Boolean expressions"
var tmp_3 = eval.Create(a as Expr);
 #line 56 "Content\Boolean expressions"
foreach (var tmp_2 in tmp_3.Run0_()) { var tmp_4 = tmp_2 as TRUE; 
 #line 56 "Content\Boolean expressions"
if (tmp_4 != null) { 
 #line 56 "Content\Boolean expressions"
var result = TRUE.Create();
 #line 56 "Content\Boolean expressions"
yield return result;  } } } }
 } 

  
 { 
 #line 63 "Content\Boolean expressions"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opBitwiseOr; 
 #line 63 "Content\Boolean expressions"
if (tmp_1 != null) { var a = tmp_1.P1; var b = tmp_1.P2; 
 #line 63 "Content\Boolean expressions"
if(a is Expr) { 
 #line 63 "Content\Boolean expressions"
var tmp_3 = eval.Create(a as Expr);
 #line 63 "Content\Boolean expressions"
foreach (var tmp_2 in tmp_3.Run0_()) { var tmp_4 = tmp_2 as FALSE; 
 #line 63 "Content\Boolean expressions"
if (tmp_4 != null) { 
 #line 63 "Content\Boolean expressions"
if(b is Expr) { 
 #line 63 "Content\Boolean expressions"
var tmp_6 = eval.Create(b as Expr);
 #line 63 "Content\Boolean expressions"
foreach (var tmp_5 in tmp_6.Run0_()) { var y = tmp_5; 
 #line 63 "Content\Boolean expressions"
var result = y;
 #line 63 "Content\Boolean expressions"
yield return result;  } } } } } }
 } 

  
 { 
 #line 73 "Content\Boolean expressions"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opBitwiseAnd; 
 #line 73 "Content\Boolean expressions"
if (tmp_1 != null) { var a = tmp_1.P1; var b = tmp_1.P2; 
 #line 73 "Content\Boolean expressions"
if(a is Expr) { 
 #line 73 "Content\Boolean expressions"
var tmp_3 = eval.Create(a as Expr);
 #line 73 "Content\Boolean expressions"
foreach (var tmp_2 in tmp_3.Run0_()) { var tmp_4 = tmp_2 as FALSE; 
 #line 73 "Content\Boolean expressions"
if (tmp_4 != null) { 
 #line 73 "Content\Boolean expressions"
var result = FALSE.Create();
 #line 73 "Content\Boolean expressions"
yield return result;  } } } }
 } 

  
 { 
 #line 80 "Content\Boolean expressions"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opBitwiseAnd; 
 #line 80 "Content\Boolean expressions"
if (tmp_1 != null) { var a = tmp_1.P1; var b = tmp_1.P2; 
 #line 80 "Content\Boolean expressions"
if(a is Expr) { 
 #line 80 "Content\Boolean expressions"
var tmp_3 = eval.Create(a as Expr);
 #line 80 "Content\Boolean expressions"
foreach (var tmp_2 in tmp_3.Run0_()) { var tmp_4 = tmp_2 as TRUE; 
 #line 80 "Content\Boolean expressions"
if (tmp_4 != null) { 
 #line 80 "Content\Boolean expressions"
if(b is Expr) { 
 #line 80 "Content\Boolean expressions"
var tmp_6 = eval.Create(b as Expr);
 #line 80 "Content\Boolean expressions"
foreach (var tmp_5 in tmp_6.Run0_()) { var y = tmp_5; 
 #line 80 "Content\Boolean expressions"
var result = y;
 #line 80 "Content\Boolean expressions"
yield return result;  } } } } } }
 } 

 foreach(var p in Run()) yield return p; }

public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }

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

  public IEnumerable<IRunnable> Run() {   
 { 
 #line 23 "Content\Boolean expressions"
var tmp_0 = this as run; 
 #line 23 "Content\Boolean expressions"
var tmp_2 = eval.Create(_opBitwiseOr.Create(FALSE.Create(), _opBitwiseAnd.Create(TRUE.Create(), _opBang.Create(FALSE.Create()))));
 #line 23 "Content\Boolean expressions"
foreach (var tmp_1 in tmp_2.Run0_()) { var res = tmp_1; 
 #line 23 "Content\Boolean expressions"
var result = res;
 #line 23 "Content\Boolean expressions"
yield return result;  }
 } 

  }

public IEnumerable<IRunnable> Run0_() { foreach(var p in Run()) yield return p; }

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


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run0_() { foreach(var p in Run()) yield return p; }

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
