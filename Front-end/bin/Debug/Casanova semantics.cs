using System.Collections.Generic;
using System.Linq;
namespace Casanova_semantics {
 public static class Extensions { public static V GetKey<T, V>(this System.Collections.Immutable.ImmutableDictionary<T, V> self, T key) { return self[key]; } }
 public interface IRunnable { IEnumerable<IRunnable> Run();
IEnumerable<IRunnable> Run2_();
IEnumerable<IRunnable> Run2_6_();
 }


public interface BoolConst : BoolExpr {}
public interface BoolExpr : Expr {}
public interface Else : IRunnable {}
public interface Expr : IRunnable {}
public interface Id : Expr {}
public interface IntConst : IntExpr {}
public interface IntExpr : Expr {}
public interface Locals : IRunnable {}
public interface Test : IRunnable {}
public interface Then : IRunnable {}



public class _opDollar : Id {
public string P1;

public _opDollar(string P1) {this.P1 = P1;}


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run2_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run2_6_() { foreach(var p in Run2_()) yield return p; }

public override string ToString() {
 var res = "("; 

 res += "$"; res += P1.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _opDollar;
 if(tmp != null) return this.P1.Equals(tmp.P1); 
 else return false; }
}

public class _opDollarb : BoolConst {
public bool P1;

public _opDollarb(bool P1) {this.P1 = P1;}


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run2_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run2_6_() { foreach(var p in Run2_()) yield return p; }

public override string ToString() {
 var res = "("; 

 res += "$b"; res += P1.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _opDollarb;
 if(tmp != null) return this.P1.Equals(tmp.P1); 
 else return false; }
}

public class _opDollari : IntConst {
public int P1;

public _opDollari(int P1) {this.P1 = P1;}


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run2_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run2_6_() { foreach(var p in Run2_()) yield return p; }

public override string ToString() {
 var res = "("; 

 res += "$i"; res += P1.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _opDollari;
 if(tmp != null) return this.P1.Equals(tmp.P1); 
 else return false; }
}

public class _opDollarm : Locals {
public System.Collections.Immutable.ImmutableDictionary<string, Expr> P1;

public _opDollarm(System.Collections.Immutable.ImmutableDictionary<string, Expr> P1) {this.P1 = P1;}


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run2_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run2_6_() { foreach(var p in Run2_()) yield return p; }

public override string ToString() {
 var res = "("; 

 res += "$m"; if (P1 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P1.ToString(); } 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _opDollarm;
 if(tmp != null) return this.P1.Equals(tmp.P1); 
 else return false; }
}

public class _opAnd : BoolExpr {
public BoolExpr P1;
public BoolExpr P2;

public _opAnd(BoolExpr P1, BoolExpr P2) {this.P1 = P1; this.P2 = P2;}


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run2_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run2_6_() { foreach(var p in Run2_()) yield return p; }

public override string ToString() {
 var res = "("; 
res += P1.ToString(); 

 res += "&&"; res += P2.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _opAnd;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
 else return false; }
}

public class _opMultiplication : IntExpr {
public IntExpr P1;
public IntExpr P2;

public _opMultiplication(IntExpr P1, IntExpr P2) {this.P1 = P1; this.P2 = P2;}


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run2_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run2_6_() { foreach(var p in Run2_()) yield return p; }

public override string ToString() {
 var res = "("; 
res += P1.ToString(); 

 res += "*"; res += P2.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _opMultiplication;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
 else return false; }
}

public class _opAddition : IntExpr {
public IntExpr P1;
public IntExpr P2;

public _opAddition(IntExpr P1, IntExpr P2) {this.P1 = P1; this.P2 = P2;}


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run2_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run2_6_() { foreach(var p in Run2_()) yield return p; }

public override string ToString() {
 var res = "("; 
res += P1.ToString(); 

 res += "+"; res += P2.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _opAddition;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
 else return false; }
}

public class _opSubtraction : IntExpr {
public IntExpr P1;
public IntExpr P2;

public _opSubtraction(IntExpr P1, IntExpr P2) {this.P1 = P1; this.P2 = P2;}


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run2_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run2_6_() { foreach(var p in Run2_()) yield return p; }

public override string ToString() {
 var res = "("; 
res += P1.ToString(); 

 res += "-"; res += P2.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _opSubtraction;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
 else return false; }
}

public class _opDivision : IntExpr {
public IntExpr P1;
public IntExpr P2;

public _opDivision(IntExpr P1, IntExpr P2) {this.P1 = P1; this.P2 = P2;}


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run2_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run2_6_() { foreach(var p in Run2_()) yield return p; }

public override string ToString() {
 var res = "("; 
res += P1.ToString(); 

 res += "/"; res += P2.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _opDivision;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
 else return false; }
}

public class _Semicolon : Expr {
public Expr P1;
public Expr P2;

public _Semicolon(Expr P1, Expr P2) {this.P1 = P1; this.P2 = P2;}


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run2_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run2_6_() { foreach(var p in Run2_()) yield return p; }

public override string ToString() {
 var res = "("; 
res += P1.ToString(); 

 res += ";"; res += P2.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _Semicolon;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
 else return false; }
}

public class _opEquals : BoolExpr {
public IntExpr P1;
public IntExpr P2;

public _opEquals(IntExpr P1, IntExpr P2) {this.P1 = P1; this.P2 = P2;}


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run2_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run2_6_() { foreach(var p in Run2_()) yield return p; }

public override string ToString() {
 var res = "("; 
res += P1.ToString(); 

 res += "="; res += P2.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _opEquals;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
 else return false; }
}

public class _opGreaterThan : BoolExpr {
public IntExpr P1;
public IntExpr P2;

public _opGreaterThan(IntExpr P1, IntExpr P2) {this.P1 = P1; this.P2 = P2;}


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run2_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run2_6_() { foreach(var p in Run2_()) yield return p; }

public override string ToString() {
 var res = "("; 
res += P1.ToString(); 

 res += ">"; res += P2.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _opGreaterThan;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
 else return false; }
}

public class add : Locals {
public Locals P1;
public string P2;
public Expr P3;

public add(Locals P1, string P2, Expr P3) {this.P1 = P1; this.P2 = P2; this.P3 = P3;}

  public IEnumerable<IRunnable> Run() {   
 { 
 #line 44 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opDollarm; 
 #line 44 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var M = tmp_1.P1; var k = tmp_0.P2; var v = tmp_0.P3; var M_Prime = (M.Add (k,v)); 
 #line 44 "Content\Casanova semantics\transform.mc"
if(M_Prime is System.Collections.Immutable.ImmutableDictionary<string, Expr>) { 
 #line 44 "Content\Casanova semantics\transform.mc"
var result = new _opDollarm(M_Prime as System.Collections.Immutable.ImmutableDictionary<string, Expr>);
 #line 44 "Content\Casanova semantics\transform.mc"
yield return result;  } }
 } 

  }

public IEnumerable<IRunnable> Run2_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run2_6_() { foreach(var p in Run2_()) yield return p; }

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

public class _Else : Else {

public _Else() {}


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run2_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run2_6_() { foreach(var p in Run2_()) yield return p; }

public override string ToString() {
return "else";
}

public override bool Equals(object other) {
 return other is _Else; 
}
}

public class eval : Expr {
public float P1;
public Locals P2;
public Expr P3;

public eval(float P1, Locals P2, Expr P3) {this.P1 = P1; this.P2 = P2; this.P3 = P3;}

  public IEnumerable<IRunnable> Run2_() {   
 { 
 #line 54 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var tmp_1 = tmp_0.P3 as _If; 
 #line 54 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var c = tmp_1.P1; var tmp_2 = tmp_1.P2 as _Then; 
 #line 54 "Content\Casanova semantics\transform.mc"
if (tmp_2 != null) { var t = tmp_1.P3; var tmp_3 = tmp_1.P4 as _Else; 
 #line 54 "Content\Casanova semantics\transform.mc"
if (tmp_3 != null) { var e = tmp_1.P5; 
 #line 54 "Content\Casanova semantics\transform.mc"
if(!c.Equals(new _opDollarb(true))) { 
 #line 54 "Content\Casanova semantics\transform.mc"
if(!c.Equals(new _opDollarb(false))) { 
 #line 54 "Content\Casanova semantics\transform.mc"
if(M is Locals && c is Expr) { 
 #line 54 "Content\Casanova semantics\transform.mc"
var tmp_5 = new eval(dt, M as Locals, c as Expr);
 #line 54 "Content\Casanova semantics\transform.mc"
foreach (var tmp_4 in tmp_5.Run2_()) { var c_Prime = tmp_4; 
 #line 54 "Content\Casanova semantics\transform.mc"
if(M is Locals && c_Prime is BoolExpr && t is Expr && e is Expr) { 
 #line 54 "Content\Casanova semantics\transform.mc"
var tmp_7 = new eval(dt, M as Locals, new _If(c_Prime as BoolExpr, new _Then(), t as Expr, new _Else(), e as Expr));
 #line 54 "Content\Casanova semantics\transform.mc"
foreach (var tmp_6 in tmp_7.Run2_()) { var res = tmp_6; 
 #line 54 "Content\Casanova semantics\transform.mc"
var result = res;
 #line 54 "Content\Casanova semantics\transform.mc"
yield return result;  } } } } } } } } }
 } 

  
 { 
 #line 61 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var tmp_1 = tmp_0.P3 as _If; 
 #line 61 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var tmp_2 = tmp_1.P1 as _opDollarb; 
 #line 61 "Content\Casanova semantics\transform.mc"
if (tmp_2 != null) { 
 #line 61 "Content\Casanova semantics\transform.mc"
if (tmp_2.P1 == true) { var tmp_3 = tmp_1.P2 as _Then; 
 #line 61 "Content\Casanova semantics\transform.mc"
if (tmp_3 != null) { var t = tmp_1.P3; var tmp_4 = tmp_1.P4 as _Else; 
 #line 61 "Content\Casanova semantics\transform.mc"
if (tmp_4 != null) { var e = tmp_1.P5; 
 #line 61 "Content\Casanova semantics\transform.mc"
if(M is Locals && t is Expr) { 
 #line 61 "Content\Casanova semantics\transform.mc"
var tmp_6 = new eval(dt, M as Locals, t as Expr);
 #line 61 "Content\Casanova semantics\transform.mc"
foreach (var tmp_5 in tmp_6.Run2_()) { var res = tmp_5; 
 #line 61 "Content\Casanova semantics\transform.mc"
var result = res;
 #line 61 "Content\Casanova semantics\transform.mc"
yield return result;  } } } } } } }
 } 

  
 { 
 #line 65 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var tmp_1 = tmp_0.P3 as _If; 
 #line 65 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var tmp_2 = tmp_1.P1 as _opDollarb; 
 #line 65 "Content\Casanova semantics\transform.mc"
if (tmp_2 != null) { 
 #line 65 "Content\Casanova semantics\transform.mc"
if (tmp_2.P1 == false) { var tmp_3 = tmp_1.P2 as _Then; 
 #line 65 "Content\Casanova semantics\transform.mc"
if (tmp_3 != null) { var t = tmp_1.P3; var tmp_4 = tmp_1.P4 as _Else; 
 #line 65 "Content\Casanova semantics\transform.mc"
if (tmp_4 != null) { var e = tmp_1.P5; 
 #line 65 "Content\Casanova semantics\transform.mc"
if(M is Locals && e is Expr) { 
 #line 65 "Content\Casanova semantics\transform.mc"
var tmp_6 = new eval(dt, M as Locals, e as Expr);
 #line 65 "Content\Casanova semantics\transform.mc"
foreach (var tmp_5 in tmp_6.Run2_()) { var res = tmp_5; 
 #line 65 "Content\Casanova semantics\transform.mc"
var result = res;
 #line 65 "Content\Casanova semantics\transform.mc"
yield return result;  } } } } } } }
 } 

  
 { 
 #line 69 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var tmp_1 = tmp_0.P3 as _opDollar; 
 #line 69 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var v = tmp_1.P1; 
 #line 69 "Content\Casanova semantics\transform.mc"
if(M is Locals) { 
 #line 69 "Content\Casanova semantics\transform.mc"
var tmp_3 = new lookup(M as Locals, v);
 #line 69 "Content\Casanova semantics\transform.mc"
foreach (var tmp_2 in tmp_3.Run2_()) { var res = tmp_2; 
 #line 69 "Content\Casanova semantics\transform.mc"
var result = res;
 #line 69 "Content\Casanova semantics\transform.mc"
yield return result;  } } }
 } 

  
 { 
 #line 73 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var tmp_1 = tmp_0.P3 as wait; 
 #line 73 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var t = tmp_1.P1; 
 #line 73 "Content\Casanova semantics\transform.mc"
if((dt>=t).Equals(true)) { var dt_Prime = (dt-t); 
 #line 73 "Content\Casanova semantics\transform.mc"
var result = new setDt(dt_Prime);
 #line 73 "Content\Casanova semantics\transform.mc"
yield return result;  } }
 } 

  
 { 
 #line 78 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var tmp_1 = tmp_0.P3 as wait; 
 #line 78 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var t = tmp_1.P1; 
 #line 78 "Content\Casanova semantics\transform.mc"
if((dt<t).Equals(true)) { var t_Prime = (t-dt); 
 #line 78 "Content\Casanova semantics\transform.mc"
var result = new wait(t_Prime);
 #line 78 "Content\Casanova semantics\transform.mc"
yield return result;  } }
 } 

  
 { 
 #line 83 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var tmp_1 = tmp_0.P3 as _Semicolon; 
 #line 83 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var a = tmp_1.P1; var b = tmp_1.P2; 
 #line 83 "Content\Casanova semantics\transform.mc"
if(M is Locals && a is Expr) { 
 #line 83 "Content\Casanova semantics\transform.mc"
var tmp_3 = new eval(dt, M as Locals, a as Expr);
 #line 83 "Content\Casanova semantics\transform.mc"
foreach (var tmp_2 in tmp_3.Run2_6_()) { var a_Prime = tmp_2; 
 #line 83 "Content\Casanova semantics\transform.mc"
if(M is Locals && a_Prime is Expr && b is Expr) { 
 #line 83 "Content\Casanova semantics\transform.mc"
var tmp_5 = new stepOrSuspend (dt, M as Locals, a_Prime as Expr, b as Expr);
 #line 83 "Content\Casanova semantics\transform.mc"
foreach (var tmp_4 in tmp_5.Run2_6_()) { var res = tmp_4; 
 #line 83 "Content\Casanova semantics\transform.mc"
var result = res;
 #line 83 "Content\Casanova semantics\transform.mc"
yield return result;  } } } } }
 } 

 foreach(var p in Run()) yield return p; }

public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run2_6_() { foreach(var p in Run2_()) yield return p; }

public override string ToString() {
 var res = "("; 

 res += "eval"; res += P1.ToString(); 
res += P2.ToString(); 
res += P3.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as eval;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2) && this.P3.Equals(tmp.P3); 
 else return false; }
}

public class _If : Expr {
public BoolExpr P1;
public Then P2;
public Expr P3;
public Else P4;
public Expr P5;

public _If(BoolExpr P1, Then P2, Expr P3, Else P4, Expr P5) {this.P1 = P1; this.P2 = P2; this.P3 = P3; this.P4 = P4; this.P5 = P5;}


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run2_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run2_6_() { foreach(var p in Run2_()) yield return p; }

public override string ToString() {
 var res = "("; 

 res += "if"; res += P1.ToString(); 
res += P2.ToString(); 
res += P3.ToString(); 
res += P4.ToString(); 
res += P5.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _If;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2) && this.P3.Equals(tmp.P3) && this.P4.Equals(tmp.P4) && this.P5.Equals(tmp.P5); 
 else return false; }
}

public class lookup : Expr {
public Locals P1;
public string P2;

public lookup(Locals P1, string P2) {this.P1 = P1; this.P2 = P2;}

  public IEnumerable<IRunnable> Run() {   
 { 
 #line 40 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opDollarm; 
 #line 40 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var M = tmp_1.P1; var k = tmp_0.P2; var v = (M.GetKey (k)); 
 #line 40 "Content\Casanova semantics\transform.mc"
var result = v;
 #line 40 "Content\Casanova semantics\transform.mc"
yield return result;  }
 } 

  }

public IEnumerable<IRunnable> Run2_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run2_6_() { foreach(var p in Run2_()) yield return p; }

public override string ToString() {
 var res = "("; 
res += P1.ToString(); 

 res += "lookup"; res += P2.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as lookup;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
 else return false; }
}

public class runTest1 : Test {

public runTest1() {}

  public IEnumerable<IRunnable> Run() {   
 { 
 #line 48 "Content\Casanova semantics\transform.mc"
var tmp_0 = this as runTest1; var dt = 0.020000f; var M = new _opDollarm(System.Collections.Immutable.ImmutableDictionary<string,Expr >.Empty); 
 #line 48 "Content\Casanova semantics\transform.mc"
if(M is Locals) { 
 #line 48 "Content\Casanova semantics\transform.mc"
var tmp_2 = new eval(dt, M as Locals, new _Semicolon(new _Semicolon(new wait(0.010000f), new wait(0.020000f)), new wait(0.020000f)));
 #line 48 "Content\Casanova semantics\transform.mc"
foreach (var tmp_1 in tmp_2.Run2_()) { var res = tmp_1; 
 #line 48 "Content\Casanova semantics\transform.mc"
var result = res;
 #line 48 "Content\Casanova semantics\transform.mc"
yield return result;  } }
 } 

  }

public IEnumerable<IRunnable> Run2_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run2_6_() { foreach(var p in Run2_()) yield return p; }

public override string ToString() {
return "runTest1";
}

public override bool Equals(object other) {
 return other is runTest1; 
}
}

public class setDt : Expr {
public float P1;

public setDt(float P1) {this.P1 = P1;}


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run2_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run2_6_() { foreach(var p in Run2_()) yield return p; }

public override string ToString() {
 var res = "("; 

 res += "setDt"; res += P1.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as setDt;
 if(tmp != null) return this.P1.Equals(tmp.P1); 
 else return false; }
}

public class stepOrSuspend  : Expr {
public float P1;
public Locals P2;
public Expr P3;
public Expr P4;

public stepOrSuspend (float P1, Locals P2, Expr P3, Expr P4) {this.P1 = P1; this.P2 = P2; this.P3 = P3; this.P4 = P4;}

  public IEnumerable<IRunnable> Run2_6_() {   
 { 
 #line 88 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var tmp_1 = tmp_0.P3 as setDt; 
 #line 88 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var dt_Prime = tmp_1.P1; var b = tmp_0.P4; 
 #line 88 "Content\Casanova semantics\transform.mc"
if(M is Locals && b is Expr) { 
 #line 88 "Content\Casanova semantics\transform.mc"
var tmp_3 = new eval(dt_Prime, M as Locals, b as Expr);
 #line 88 "Content\Casanova semantics\transform.mc"
foreach (var tmp_2 in tmp_3.Run2_6_()) { var res = tmp_2; 
 #line 88 "Content\Casanova semantics\transform.mc"
var result = res;
 #line 88 "Content\Casanova semantics\transform.mc"
yield return result;  } } }
 } 

  
 { 
 #line 92 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var tmp_1 = tmp_0.P3 as wait; 
 #line 92 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var t = tmp_1.P1; var b = tmp_0.P4; 
 #line 92 "Content\Casanova semantics\transform.mc"
if(b is Expr) { 
 #line 92 "Content\Casanova semantics\transform.mc"
var result = new _Semicolon(new wait(t), b as Expr);
 #line 92 "Content\Casanova semantics\transform.mc"
yield return result;  } }
 } 

 foreach(var p in Run2_()) yield return p; }

public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run2_() { foreach(var p in Run()) yield return p; }

public override string ToString() {
 var res = "("; 

 res += "stepOrSuspend "; res += P1.ToString(); 
res += P2.ToString(); 
res += P3.ToString(); 
res += P4.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as stepOrSuspend ;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2) && this.P3.Equals(tmp.P3) && this.P4.Equals(tmp.P4); 
 else return false; }
}

public class _Then : Then {

public _Then() {}


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run2_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run2_6_() { foreach(var p in Run2_()) yield return p; }

public override string ToString() {
return "then";
}

public override bool Equals(object other) {
 return other is _Then; 
}
}

public class wait : Expr {
public float P1;

public wait(float P1) {this.P1 = P1;}


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run2_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run2_6_() { foreach(var p in Run2_()) yield return p; }

public override string ToString() {
 var res = "("; 

 res += "wait"; res += P1.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as wait;
 if(tmp != null) return this.P1.Equals(tmp.P1); 
 else return false; }
}

public class _opOr : BoolExpr {
public BoolExpr P1;
public BoolExpr P2;

public _opOr(BoolExpr P1, BoolExpr P2) {this.P1 = P1; this.P2 = P2;}


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run2_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run2_6_() { foreach(var p in Run2_()) yield return p; }

public override string ToString() {
 var res = "("; 
res += P1.ToString(); 

 res += "||"; res += P2.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _opOr;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
 else return false; }
}




public class EntryPoint {
 static public IEnumerable<IRunnable> Run(bool printInput)
{
 #line 1 "input"
 var p = new runTest1();
if(printInput) System.Console.WriteLine(p.ToString());
foreach(var x in p.Run())
yield return x;
}
}

}
