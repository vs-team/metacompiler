using System.Collections.Generic;
using System.Linq;
namespace Casanova_semantics {
 public static class Extensions { public static V GetKey<T, V>(this System.Collections.Immutable.ImmutableDictionary<T, V> self, T key) { return self[key]; } }
 public interface IRunnable { IEnumerable<IRunnable> Run();
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



public class _Dollar : Id {
public System.String P1;

public _Dollar(System.String P1) {this.P1 = P1;}


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

public class _Dollarb : BoolConst {
public System.Boolean P1;

public _Dollarb(System.Boolean P1) {this.P1 = P1;}


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }

public override string ToString() {
 var res = "("; 

 res += "$b"; res += P1.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _Dollarb;
 if(tmp != null) return this.P1.Equals(tmp.P1); 
 else return false; }
}

public class _Dollari : IntConst {
public System.Int32 P1;

public _Dollari(System.Int32 P1) {this.P1 = P1;}


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }

public override string ToString() {
 var res = "("; 

 res += "$i"; res += P1.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _Dollari;
 if(tmp != null) return this.P1.Equals(tmp.P1); 
 else return false; }
}

public class _Dollarm : Locals {
public System.Collections.Immutable.ImmutableDictionary<string, Expr> P1;

public _Dollarm(System.Collections.Immutable.ImmutableDictionary<string, Expr> P1) {this.P1 = P1;}


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }

public override string ToString() {
 var res = "("; 

 res += "$m"; if (P1 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P1.ToString(); } 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _Dollarm;
 if(tmp != null) return this.P1.Equals(tmp.P1); 
 else return false; }
}

public class _opAnd : BoolExpr {
public BoolExpr P1;
public BoolExpr P2;

public _opAnd(BoolExpr P1, BoolExpr P2) {this.P1 = P1; this.P2 = P2;}


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }

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

public class _opAddition : IntExpr {
public IntExpr P1;
public IntExpr P2;

public _opAddition(IntExpr P1, IntExpr P2) {this.P1 = P1; this.P2 = P2;}


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }

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

public class _opEquals : BoolExpr {
public IntExpr P1;
public IntExpr P2;

public _opEquals(IntExpr P1, IntExpr P2) {this.P1 = P1; this.P2 = P2;}


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }

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
 #line 35 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _Dollarm; 
 #line 35 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var M = tmp_1.P1; var k = tmp_0.P2; var v = tmp_0.P3; var M_Prime = (M.Add (k,v)); 
 #line 35 "Content\Casanova semantics\transform.mc"
if(M_Prime is System.Collections.Immutable.ImmutableDictionary<string, Expr>) { 
 #line 35 "Content\Casanova semantics\transform.mc"
var result = new _Dollarm(M_Prime as System.Collections.Immutable.ImmutableDictionary<string, Expr>);
 #line 35 "Content\Casanova semantics\transform.mc"
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
}

public class _Else : Else {

public _Else() {}


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }

public override string ToString() {
return "else";
}

public override bool Equals(object other) {
 return other is _Else; 
}
}

public class eval : Expr {
public Locals P1;
public Expr P2;

public eval(Locals P1, Expr P2) {this.P1 = P1; this.P2 = P2;}

  public IEnumerable<IRunnable> Run() {   
 { 
 #line 39 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var M = tmp_0.P1; var tmp_1 = tmp_0.P2 as _If; 
 #line 39 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var c = tmp_1.P1; var tmp_2 = tmp_1.P2 as _Then; 
 #line 39 "Content\Casanova semantics\transform.mc"
if (tmp_2 != null) { var t = tmp_1.P3; var tmp_3 = tmp_1.P4 as _Else; 
 #line 39 "Content\Casanova semantics\transform.mc"
if (tmp_3 != null) { var e = tmp_1.P5; 
 #line 39 "Content\Casanova semantics\transform.mc"
if(!c.Equals(new _Dollarb(true))) { 
 #line 39 "Content\Casanova semantics\transform.mc"
if(!c.Equals(new _Dollarb(false))) { 
 #line 39 "Content\Casanova semantics\transform.mc"
if(M is Locals && c is Expr) { 
 #line 39 "Content\Casanova semantics\transform.mc"
var tmp_5 = new eval(M as Locals, c as Expr);
 #line 39 "Content\Casanova semantics\transform.mc"
foreach (var tmp_4 in tmp_5.Run()) { var c_Prime = tmp_4; 
 #line 39 "Content\Casanova semantics\transform.mc"
if(M is Locals && c_Prime is BoolExpr && t is Expr && e is Expr) { 
 #line 39 "Content\Casanova semantics\transform.mc"
var tmp_7 = new eval(M as Locals, new _If(c_Prime as BoolExpr, new _Then(), t as Expr, new _Else(), e as Expr));
 #line 39 "Content\Casanova semantics\transform.mc"
foreach (var tmp_6 in tmp_7.Run()) { var res = tmp_6; 
 #line 39 "Content\Casanova semantics\transform.mc"
var result = res;
 #line 39 "Content\Casanova semantics\transform.mc"
yield return result;  } } } } } } } } }
 } 

  
 { 
 #line 46 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var M = tmp_0.P1; var tmp_1 = tmp_0.P2 as _If; 
 #line 46 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var tmp_2 = tmp_1.P1 as _Dollarb; 
 #line 46 "Content\Casanova semantics\transform.mc"
if (tmp_2 != null) { 
 #line 46 "Content\Casanova semantics\transform.mc"
if (tmp_2.P1 == true) { var tmp_3 = tmp_1.P2 as _Then; 
 #line 46 "Content\Casanova semantics\transform.mc"
if (tmp_3 != null) { var t = tmp_1.P3; var tmp_4 = tmp_1.P4 as _Else; 
 #line 46 "Content\Casanova semantics\transform.mc"
if (tmp_4 != null) { var e = tmp_1.P5; 
 #line 46 "Content\Casanova semantics\transform.mc"
if(M is Locals && t is Expr) { 
 #line 46 "Content\Casanova semantics\transform.mc"
var tmp_6 = new eval(M as Locals, t as Expr);
 #line 46 "Content\Casanova semantics\transform.mc"
foreach (var tmp_5 in tmp_6.Run()) { var res = tmp_5; 
 #line 46 "Content\Casanova semantics\transform.mc"
var result = res;
 #line 46 "Content\Casanova semantics\transform.mc"
yield return result;  } } } } } } }
 } 

  
 { 
 #line 50 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var M = tmp_0.P1; var tmp_1 = tmp_0.P2 as _If; 
 #line 50 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var tmp_2 = tmp_1.P1 as _Dollarb; 
 #line 50 "Content\Casanova semantics\transform.mc"
if (tmp_2 != null) { 
 #line 50 "Content\Casanova semantics\transform.mc"
if (tmp_2.P1 == false) { var tmp_3 = tmp_1.P2 as _Then; 
 #line 50 "Content\Casanova semantics\transform.mc"
if (tmp_3 != null) { var t = tmp_1.P3; var tmp_4 = tmp_1.P4 as _Else; 
 #line 50 "Content\Casanova semantics\transform.mc"
if (tmp_4 != null) { var e = tmp_1.P5; 
 #line 50 "Content\Casanova semantics\transform.mc"
if(M is Locals && e is Expr) { 
 #line 50 "Content\Casanova semantics\transform.mc"
var tmp_6 = new eval(M as Locals, e as Expr);
 #line 50 "Content\Casanova semantics\transform.mc"
foreach (var tmp_5 in tmp_6.Run()) { var res = tmp_5; 
 #line 50 "Content\Casanova semantics\transform.mc"
var result = res;
 #line 50 "Content\Casanova semantics\transform.mc"
yield return result;  } } } } } } }
 } 

  
 { 
 #line 54 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var M = tmp_0.P1; var tmp_1 = tmp_0.P2 as _Dollar; 
 #line 54 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var v = tmp_1.P1; 
 #line 54 "Content\Casanova semantics\transform.mc"
if(M is Locals && v is string) { 
 #line 54 "Content\Casanova semantics\transform.mc"
var tmp_3 = new lookup(M as Locals, v as string);
 #line 54 "Content\Casanova semantics\transform.mc"
foreach (var tmp_2 in tmp_3.Run()) { var res = tmp_2; 
 #line 54 "Content\Casanova semantics\transform.mc"
var result = res;
 #line 54 "Content\Casanova semantics\transform.mc"
yield return result;  } } }
 } 

  }


public override string ToString() {
 var res = "("; 

 res += "eval"; res += P1.ToString(); 
res += P2.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as eval;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
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
 #line 31 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _Dollarm; 
 #line 31 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var M = tmp_1.P1; var k = tmp_0.P2; var v = (M.GetKey (k)); 
 #line 31 "Content\Casanova semantics\transform.mc"
var result = v;
 #line 31 "Content\Casanova semantics\transform.mc"
yield return result;  }
 } 

  }


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
 #line 58 "Content\Casanova semantics\transform.mc"
var tmp_0 = this as runTest1; var M0 = new _Dollarm(System.Collections.Immutable.ImmutableDictionary <string,Expr>.Empty); 
 #line 58 "Content\Casanova semantics\transform.mc"
if(M0 is Locals) { 
 #line 58 "Content\Casanova semantics\transform.mc"
var tmp_2 = new add(M0 as Locals, "x", new _Dollari(100));
 #line 58 "Content\Casanova semantics\transform.mc"
foreach (var tmp_1 in tmp_2.Run()) { var M1 = tmp_1; 
 #line 58 "Content\Casanova semantics\transform.mc"
if(M1 is Locals) { 
 #line 58 "Content\Casanova semantics\transform.mc"
var tmp_4 = new add(M1 as Locals, "y", new _Dollari(1000));
 #line 58 "Content\Casanova semantics\transform.mc"
foreach (var tmp_3 in tmp_4.Run()) { var M2 = tmp_3; 
 #line 58 "Content\Casanova semantics\transform.mc"
if(M2 is Locals) { 
 #line 58 "Content\Casanova semantics\transform.mc"
var tmp_6 = new eval(M2 as Locals, new _If(new _Dollarb(true), new _Then(), new _Dollar("x"), new _Else(), new _Dollar("y")));
 #line 58 "Content\Casanova semantics\transform.mc"
foreach (var tmp_5 in tmp_6.Run()) { var res = tmp_5; 
 #line 58 "Content\Casanova semantics\transform.mc"
var result = res;
 #line 58 "Content\Casanova semantics\transform.mc"
yield return result;  } } } } } }
 } 

  }


public override string ToString() {
return "runTest1";
}

public override bool Equals(object other) {
 return other is runTest1; 
}
}

public class _Then : Then {

public _Then() {}


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }

public override string ToString() {
return "then";
}

public override bool Equals(object other) {
 return other is _Then; 
}
}

public class _opOr : BoolExpr {
public BoolExpr P1;
public BoolExpr P2;

public _opOr(BoolExpr P1, BoolExpr P2) {this.P1 = P1; this.P2 = P2;}


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }

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
var p = new runTest1();
if(printInput) System.Console.WriteLine(p.ToString());
foreach(var x in p.Run())
yield return x;
}
}

}
