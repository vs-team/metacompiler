using System.Collections.Generic;
using System.Linq;
namespace Casanova_semantics {
 public static class Extensions { public static V GetKey<T, V>(this System.Collections.Immutable.ImmutableDictionary<T, V> self, T key) { return self[key]; } }
 public interface IRunnable { IEnumerable<IRunnable> Run();
IEnumerable<IRunnable> Run3_();
IEnumerable<IRunnable> Run3_6_();
IEnumerable<IRunnable> Run3_7_();
 }


public interface BoolConst : BoolExpr, Expr, ExprResult {}
public interface BoolExpr : Expr {}
public interface Else : IRunnable {}
public interface Entity : IRunnable {}
public interface Expr : IRunnable {}
public interface ExprResult : Expr {}
public interface Id : Expr {}
public interface IntConst : Expr, ExprResult, IntExpr {}
public interface IntExpr : Expr {}
public interface Locals : IRunnable {}
public interface Test : IRunnable {}
public interface Then : IRunnable {}
public interface List<t> : IRunnable {}



public class _opDollar : Id  {
public string P1;

public _opDollar(string P1) {this.P1 = P1;}
public static _opDollar Create(string P1) { return new _opDollar(P1); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run3_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run3_6_() { foreach(var p in Run3_()) yield return p; }
public IEnumerable<IRunnable> Run3_7_() { foreach(var p in Run3_()) yield return p; }

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

public override int GetHashCode() {
 return 0; 
}

}

public class _opDollarb : BoolConst  {
public bool P1;

public _opDollarb(bool P1) {this.P1 = P1;}
public static _opDollarb Create(bool P1) { return new _opDollarb(P1); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run3_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run3_6_() { foreach(var p in Run3_()) yield return p; }
public IEnumerable<IRunnable> Run3_7_() { foreach(var p in Run3_()) yield return p; }

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

public override int GetHashCode() {
 return 0; 
}

}

public class _opDollari : IntConst  {
public int P1;

public _opDollari(int P1) {this.P1 = P1;}
public static _opDollari Create(int P1) { return new _opDollari(P1); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run3_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run3_6_() { foreach(var p in Run3_()) yield return p; }
public IEnumerable<IRunnable> Run3_7_() { foreach(var p in Run3_()) yield return p; }

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

public override int GetHashCode() {
 return 0; 
}

}

public class _opDollarm : Locals  {
public System.Collections.Immutable.ImmutableDictionary<string, Expr> P1;

public _opDollarm(System.Collections.Immutable.ImmutableDictionary<string, Expr> P1) {this.P1 = P1;}
public static _opDollarm Create(System.Collections.Immutable.ImmutableDictionary<string, Expr> P1) { return new _opDollarm(P1); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run3_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run3_6_() { foreach(var p in Run3_()) yield return p; }
public IEnumerable<IRunnable> Run3_7_() { foreach(var p in Run3_()) yield return p; }

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

public override int GetHashCode() {
 return 0; 
}

}

public class _opAnd : BoolExpr  {
public BoolExpr P1;
public BoolExpr P2;

public _opAnd(BoolExpr P1, BoolExpr P2) {this.P1 = P1; this.P2 = P2;}
public static _opAnd Create(BoolExpr P1, BoolExpr P2) { return new _opAnd(P1, P2); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run3_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run3_6_() { foreach(var p in Run3_()) yield return p; }
public IEnumerable<IRunnable> Run3_7_() { foreach(var p in Run3_()) yield return p; }

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

public override int GetHashCode() {
 return 0; 
}

}

public class _opMultiplication : IntExpr  {
public IntExpr P1;
public IntExpr P2;

public _opMultiplication(IntExpr P1, IntExpr P2) {this.P1 = P1; this.P2 = P2;}
public static _opMultiplication Create(IntExpr P1, IntExpr P2) { return new _opMultiplication(P1, P2); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run3_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run3_6_() { foreach(var p in Run3_()) yield return p; }
public IEnumerable<IRunnable> Run3_7_() { foreach(var p in Run3_()) yield return p; }

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

public override int GetHashCode() {
 return 0; 
}

}

public class _opAddition : IntExpr  {
public IntExpr P1;
public IntExpr P2;

public _opAddition(IntExpr P1, IntExpr P2) {this.P1 = P1; this.P2 = P2;}
public static _opAddition Create(IntExpr P1, IntExpr P2) { return new _opAddition(P1, P2); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run3_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run3_6_() { foreach(var p in Run3_()) yield return p; }
public IEnumerable<IRunnable> Run3_7_() { foreach(var p in Run3_()) yield return p; }

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

public override int GetHashCode() {
 return 0; 
}

}

public class _Comma<t> : List<t> where t : class {
public t P1;
public List<t> P2;

public _Comma(t P1, List<t> P2) {this.P1 = P1; this.P2 = P2;}
public static _Comma<t> Create(t P1, List<t> P2) { return new _Comma<t>(P1, P2); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run3_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run3_6_() { foreach(var p in Run3_()) yield return p; }
public IEnumerable<IRunnable> Run3_7_() { foreach(var p in Run3_()) yield return p; }

public override string ToString() {
 var res = "("; 
res += P1.ToString(); 

 res += ","; if (P2 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P2 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P2.ToString(); } 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _Comma<t>;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class _opSubtraction : IntExpr  {
public IntExpr P1;
public IntExpr P2;

public _opSubtraction(IntExpr P1, IntExpr P2) {this.P1 = P1; this.P2 = P2;}
public static _opSubtraction Create(IntExpr P1, IntExpr P2) { return new _opSubtraction(P1, P2); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run3_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run3_6_() { foreach(var p in Run3_()) yield return p; }
public IEnumerable<IRunnable> Run3_7_() { foreach(var p in Run3_()) yield return p; }

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

public override int GetHashCode() {
 return 0; 
}

}

public class _opDivision : IntExpr  {
public IntExpr P1;
public IntExpr P2;

public _opDivision(IntExpr P1, IntExpr P2) {this.P1 = P1; this.P2 = P2;}
public static _opDivision Create(IntExpr P1, IntExpr P2) { return new _opDivision(P1, P2); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run3_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run3_6_() { foreach(var p in Run3_()) yield return p; }
public IEnumerable<IRunnable> Run3_7_() { foreach(var p in Run3_()) yield return p; }

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

public override int GetHashCode() {
 return 0; 
}

}

public class _Semicolon : Expr  {
public Expr P1;
public Expr P2;

public _Semicolon(Expr P1, Expr P2) {this.P1 = P1; this.P2 = P2;}
public static _Semicolon Create(Expr P1, Expr P2) { return new _Semicolon(P1, P2); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run3_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run3_6_() { foreach(var p in Run3_()) yield return p; }
public IEnumerable<IRunnable> Run3_7_() { foreach(var p in Run3_()) yield return p; }

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

public override int GetHashCode() {
 return 0; 
}

}

public class _Semicolon_Prime : ExprResult  {
public ExprResult P1;
public Expr P2;

public _Semicolon_Prime(ExprResult P1, Expr P2) {this.P1 = P1; this.P2 = P2;}
public static _Semicolon_Prime Create(ExprResult P1, Expr P2) { return new _Semicolon_Prime(P1, P2); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run3_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run3_6_() { foreach(var p in Run3_()) yield return p; }
public IEnumerable<IRunnable> Run3_7_() { foreach(var p in Run3_()) yield return p; }

public override string ToString() {
 var res = "("; 
res += P1.ToString(); 

 res += ";'"; res += P2.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _Semicolon_Prime;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class _opEquals : BoolExpr  {
public IntExpr P1;
public IntExpr P2;

public _opEquals(IntExpr P1, IntExpr P2) {this.P1 = P1; this.P2 = P2;}
public static _opEquals Create(IntExpr P1, IntExpr P2) { return new _opEquals(P1, P2); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run3_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run3_6_() { foreach(var p in Run3_()) yield return p; }
public IEnumerable<IRunnable> Run3_7_() { foreach(var p in Run3_()) yield return p; }

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

public override int GetHashCode() {
 return 0; 
}

}

public class _opGreaterThan : BoolExpr  {
public IntExpr P1;
public IntExpr P2;

public _opGreaterThan(IntExpr P1, IntExpr P2) {this.P1 = P1; this.P2 = P2;}
public static _opGreaterThan Create(IntExpr P1, IntExpr P2) { return new _opGreaterThan(P1, P2); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run3_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run3_6_() { foreach(var p in Run3_()) yield return p; }
public IEnumerable<IRunnable> Run3_7_() { foreach(var p in Run3_()) yield return p; }

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

public override int GetHashCode() {
 return 0; 
}

}

public class add : Locals  {
public Locals P1;
public string P2;
public Expr P3;

public add(Locals P1, string P2, Expr P3) {this.P1 = P1; this.P2 = P2; this.P3 = P3;}
public static add Create(Locals P1, string P2, Expr P3) { return new add(P1, P2, P3); }

  public IEnumerable<IRunnable> Run() {   
 { 
 #line 67 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opDollarm; 
 #line 67 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var M = tmp_1.P1; var k = tmp_0.P2; var v = tmp_0.P3; var M_Prime = (M.Add (k,v)); 
 #line 67 "Content\Casanova semantics\transform.mc"
if(M_Prime is System.Collections.Immutable.ImmutableDictionary<string, Expr>) { 
 #line 67 "Content\Casanova semantics\transform.mc"
var result = _opDollarm.Create(M_Prime as System.Collections.Immutable.ImmutableDictionary<string, Expr>);
 #line 67 "Content\Casanova semantics\transform.mc"
yield return result;  } }
 } 

  }

public IEnumerable<IRunnable> Run3_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run3_6_() { foreach(var p in Run3_()) yield return p; }
public IEnumerable<IRunnable> Run3_7_() { foreach(var p in Run3_()) yield return p; }

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

public class _Else : Else  {

public _Else() {}
public static _Else Create() { return new _Else(); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run3_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run3_6_() { foreach(var p in Run3_()) yield return p; }
public IEnumerable<IRunnable> Run3_7_() { foreach(var p in Run3_()) yield return p; }

public override string ToString() {
return "else";
}

public override bool Equals(object other) {
 return other is _Else; 
}

public override int GetHashCode() {
 return 0; 
}

}

public class eval : Expr  {
public float P1;
public Locals P2;
public Expr P3;

public eval(float P1, Locals P2, Expr P3) {this.P1 = P1; this.P2 = P2; this.P3 = P3;}
public static eval Create(float P1, Locals P2, Expr P3) { return new eval(P1, P2, P3); }

  public IEnumerable<IRunnable> Run() {   
 { 
 #line 56 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var tmp_1 = tmp_0.P3 as _If; 
 #line 56 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var c = tmp_1.P1; var tmp_2 = tmp_1.P2 as _Then; 
 #line 56 "Content\Casanova semantics\transform.mc"
if (tmp_2 != null) { var t = tmp_1.P3; var tmp_3 = tmp_1.P4 as _Else; 
 #line 56 "Content\Casanova semantics\transform.mc"
if (tmp_3 != null) { var e = tmp_1.P5; 
 #line 56 "Content\Casanova semantics\transform.mc"
if(!c.Equals(_opDollarb.Create(true))) { 
 #line 56 "Content\Casanova semantics\transform.mc"
if(!c.Equals(_opDollarb.Create(false))) { 
 #line 56 "Content\Casanova semantics\transform.mc"
if(M is Locals && c is Expr) { 
 #line 56 "Content\Casanova semantics\transform.mc"
var tmp_5 = eval.Create(dt, M as Locals, c as Expr);
 #line 56 "Content\Casanova semantics\transform.mc"
foreach (var tmp_4 in tmp_5.Run()) { var c_Prime = tmp_4; 
 #line 56 "Content\Casanova semantics\transform.mc"
if(M is Locals && c_Prime is BoolExpr && t is Expr && e is Expr) { 
 #line 56 "Content\Casanova semantics\transform.mc"
var tmp_7 = eval.Create(dt, M as Locals, _If.Create(c_Prime as BoolExpr, _Then.Create(), t as Expr, _Else.Create(), e as Expr));
 #line 56 "Content\Casanova semantics\transform.mc"
foreach (var tmp_6 in tmp_7.Run()) { var res = tmp_6; 
 #line 56 "Content\Casanova semantics\transform.mc"
var result = res;
 #line 56 "Content\Casanova semantics\transform.mc"
yield return result;  } } } } } } } } }
 } 

  }
  public IEnumerable<IRunnable> Run3_() {   
 { 
 #line 78 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var tmp_1 = tmp_0.P3 as _If; 
 #line 78 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var tmp_2 = tmp_1.P1 as _opDollarb; 
 #line 78 "Content\Casanova semantics\transform.mc"
if (tmp_2 != null) { 
 #line 78 "Content\Casanova semantics\transform.mc"
if (tmp_2.P1 == true) { var tmp_3 = tmp_1.P2 as _Then; 
 #line 78 "Content\Casanova semantics\transform.mc"
if (tmp_3 != null) { var t = tmp_1.P3; var tmp_4 = tmp_1.P4 as _Else; 
 #line 78 "Content\Casanova semantics\transform.mc"
if (tmp_4 != null) { var e = tmp_1.P5; 
 #line 78 "Content\Casanova semantics\transform.mc"
if(M is Locals && t is Expr) { 
 #line 78 "Content\Casanova semantics\transform.mc"
var tmp_6 = eval.Create(dt, M as Locals, t as Expr);
 #line 78 "Content\Casanova semantics\transform.mc"
foreach (var tmp_5 in tmp_6.Run3_()) { var res = tmp_5; 
 #line 78 "Content\Casanova semantics\transform.mc"
var result = res;
 #line 78 "Content\Casanova semantics\transform.mc"
yield return result;  } } } } } } }
 } 

  
 { 
 #line 82 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var tmp_1 = tmp_0.P3 as _If; 
 #line 82 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var tmp_2 = tmp_1.P1 as _opDollarb; 
 #line 82 "Content\Casanova semantics\transform.mc"
if (tmp_2 != null) { 
 #line 82 "Content\Casanova semantics\transform.mc"
if (tmp_2.P1 == false) { var tmp_3 = tmp_1.P2 as _Then; 
 #line 82 "Content\Casanova semantics\transform.mc"
if (tmp_3 != null) { var t = tmp_1.P3; var tmp_4 = tmp_1.P4 as _Else; 
 #line 82 "Content\Casanova semantics\transform.mc"
if (tmp_4 != null) { var e = tmp_1.P5; 
 #line 82 "Content\Casanova semantics\transform.mc"
if(M is Locals && e is Expr) { 
 #line 82 "Content\Casanova semantics\transform.mc"
var tmp_6 = eval.Create(dt, M as Locals, e as Expr);
 #line 82 "Content\Casanova semantics\transform.mc"
foreach (var tmp_5 in tmp_6.Run3_()) { var res = tmp_5; 
 #line 82 "Content\Casanova semantics\transform.mc"
var result = res;
 #line 82 "Content\Casanova semantics\transform.mc"
yield return result;  } } } } } } }
 } 

  
 { 
 #line 86 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var tmp_1 = tmp_0.P3 as _opDollar; 
 #line 86 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var v = tmp_1.P1; 
 #line 86 "Content\Casanova semantics\transform.mc"
if(M is Locals) { 
 #line 86 "Content\Casanova semantics\transform.mc"
var tmp_3 = lookup.Create(M as Locals, v);
 #line 86 "Content\Casanova semantics\transform.mc"
foreach (var tmp_2 in tmp_3.Run3_()) { var res = tmp_2; 
 #line 86 "Content\Casanova semantics\transform.mc"
var result = res;
 #line 86 "Content\Casanova semantics\transform.mc"
yield return result;  } } }
 } 

  
 { 
 #line 90 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var tmp_1 = tmp_0.P3 as wait; 
 #line 90 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var t = tmp_1.P1; 
 #line 90 "Content\Casanova semantics\transform.mc"
if((dt>=t).Equals(true)) { var dt_Prime = (dt-t); 
 #line 90 "Content\Casanova semantics\transform.mc"
var result = setDt.Create(dt_Prime);
 #line 90 "Content\Casanova semantics\transform.mc"
yield return result;  } }
 } 

  
 { 
 #line 95 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var tmp_1 = tmp_0.P3 as wait; 
 #line 95 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var t = tmp_1.P1; 
 #line 95 "Content\Casanova semantics\transform.mc"
if((dt<t).Equals(true)) { var t_Prime = (t-dt); 
 #line 95 "Content\Casanova semantics\transform.mc"
var result = waitResult.Create(t_Prime);
 #line 95 "Content\Casanova semantics\transform.mc"
yield return result;  } }
 } 

  
 { 
 #line 100 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var tmp_1 = tmp_0.P3 as _opDollari; 
 #line 100 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var val = tmp_1.P1; 
 #line 100 "Content\Casanova semantics\transform.mc"
var result = _opDollari.Create(val);
 #line 100 "Content\Casanova semantics\transform.mc"
yield return result;  }
 } 

  
 { 
 #line 103 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var tmp_1 = tmp_0.P3 as yield; 
 #line 103 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var tmp_2 = tmp_1.P1 as _Comma<Expr>; 
 #line 103 "Content\Casanova semantics\transform.mc"
if (tmp_2 != null) { var e = tmp_2.P1; var exprs = tmp_2.P2; 
 #line 103 "Content\Casanova semantics\transform.mc"
if(M is Locals && e is Expr && exprs is List<Expr>) { 
 #line 103 "Content\Casanova semantics\transform.mc"
var tmp_4 = evalMany.Create(dt, M as Locals, _Comma<Expr>.Create(e as Expr, exprs as List<Expr>));
 #line 103 "Content\Casanova semantics\transform.mc"
foreach (var tmp_3 in tmp_4.Run3_6_()) { var vals = tmp_3; 
 #line 103 "Content\Casanova semantics\transform.mc"
if(vals is List<ExprResult>) { 
 #line 103 "Content\Casanova semantics\transform.mc"
var result = yieldResult.Create(vals as List<ExprResult>);
 #line 103 "Content\Casanova semantics\transform.mc"
yield return result;  } } } } }
 } 

  
 { 
 #line 119 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var tmp_1 = tmp_0.P3 as _Semicolon; 
 #line 119 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var a = tmp_1.P1; var b = tmp_1.P2; var debug = (EntryPoint.Print ("---------")); var debug1 = (EntryPoint.Print (a)); 
 #line 119 "Content\Casanova semantics\transform.mc"
if(M is Locals && a is Expr) { 
 #line 119 "Content\Casanova semantics\transform.mc"
var tmp_3 = eval.Create(dt, M as Locals, a as Expr);
 #line 119 "Content\Casanova semantics\transform.mc"
foreach (var tmp_2 in tmp_3.Run3_7_()) { var a1 = tmp_2; var debug2 = (EntryPoint.Print (a1)); 
 #line 119 "Content\Casanova semantics\transform.mc"
if(M is Locals && a1 is ExprResult && b is Expr) { 
 #line 119 "Content\Casanova semantics\transform.mc"
var tmp_5 = stepOrSuspend .Create(dt, M as Locals, a1 as ExprResult, b as Expr);
 #line 119 "Content\Casanova semantics\transform.mc"
foreach (var tmp_4 in tmp_5.Run3_7_()) { var res = tmp_4; var debug3 = (EntryPoint.Print (b)); var debug1000 = (EntryPoint.Print ("++++++++")); 
 #line 119 "Content\Casanova semantics\transform.mc"
var result = res;
 #line 119 "Content\Casanova semantics\transform.mc"
yield return result;  } } } } }
 } 

 foreach(var p in Run()) yield return p; }

public IEnumerable<IRunnable> Run3_6_() { foreach(var p in Run3_()) yield return p; }
public IEnumerable<IRunnable> Run3_7_() { foreach(var p in Run3_()) yield return p; }

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

public override int GetHashCode() {
 return 0; 
}

}

public class evalMany : Expr  {
public float P1;
public Locals P2;
public List<Expr> P3;

public evalMany(float P1, Locals P2, List<Expr> P3) {this.P1 = P1; this.P2 = P2; this.P3 = P3;}
public static evalMany Create(float P1, Locals P2, List<Expr> P3) { return new evalMany(P1, P2, P3); }

  public IEnumerable<IRunnable> Run3_6_() {   
 { 
 #line 108 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var tmp_1 = tmp_0.P3 as _Comma<Expr>; 
 #line 108 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var e = tmp_1.P1; var exprs = tmp_1.P2; 
 #line 108 "Content\Casanova semantics\transform.mc"
if (e is Expr && exprs is List<Expr>) { var es = _Comma<Expr>.Create(e as Expr, exprs as List<Expr>); 
 #line 108 "Content\Casanova semantics\transform.mc"
if(M is Locals && e is Expr) { 
 #line 108 "Content\Casanova semantics\transform.mc"
var tmp_3 = eval.Create(dt, M as Locals, e as Expr);
 #line 108 "Content\Casanova semantics\transform.mc"
foreach (var tmp_2 in tmp_3.Run3_6_()) { var val = tmp_2; 
 #line 108 "Content\Casanova semantics\transform.mc"
if(M is Locals && exprs is List<Expr>) { 
 #line 108 "Content\Casanova semantics\transform.mc"
var tmp_5 = evalMany.Create(dt, M as Locals, exprs as List<Expr>);
 #line 108 "Content\Casanova semantics\transform.mc"
foreach (var tmp_4 in tmp_5.Run3_6_()) { var vals = tmp_4; 
 #line 108 "Content\Casanova semantics\transform.mc"
if (val is Expr && vals is List<Expr>) { var res = _Comma<Expr>.Create(val as Expr, vals as List<Expr>); 
 #line 108 "Content\Casanova semantics\transform.mc"
if(val is Expr && vals is List<Expr>) { 
 #line 108 "Content\Casanova semantics\transform.mc"
var result = _Comma<Expr>.Create(val as Expr, vals as List<Expr>);
 #line 108 "Content\Casanova semantics\transform.mc"
yield return result;  } } } } } } } }
 } 

  
 { 
 #line 115 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var tmp_1 = tmp_0.P3 as nil<Expr>; 
 #line 115 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { 
 #line 115 "Content\Casanova semantics\transform.mc"
var result = nil<Expr>.Create();
 #line 115 "Content\Casanova semantics\transform.mc"
yield return result;  }
 } 

 foreach(var p in Run3_()) yield return p; }

public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run3_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run3_7_() { foreach(var p in Run3_()) yield return p; }

public override string ToString() {
 var res = "("; 

 res += "evalMany"; res += P1.ToString(); 
res += P2.ToString(); 
if (P3 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P3 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P3.ToString(); } 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as evalMany;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2) && this.P3.Equals(tmp.P3); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class evalRule : Entity  {
public float P1;
public Locals P2;
public string P3;
public Expr P4;

public evalRule(float P1, Locals P2, string P3, Expr P4) {this.P1 = P1; this.P2 = P2; this.P3 = P3; this.P4 = P4;}
public static evalRule Create(float P1, Locals P2, string P3, Expr P4) { return new evalRule(P1, P2, P3, P4); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run3_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run3_6_() { foreach(var p in Run3_()) yield return p; }
public IEnumerable<IRunnable> Run3_7_() { foreach(var p in Run3_()) yield return p; }

public override string ToString() {
 var res = "("; 

 res += "evalRule"; res += P1.ToString(); 
res += P2.ToString(); 
res += P3.ToString(); 
res += P4.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as evalRule;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2) && this.P3.Equals(tmp.P3) && this.P4.Equals(tmp.P4); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class _If : Expr  {
public BoolExpr P1;
public Then P2;
public Expr P3;
public Else P4;
public Expr P5;

public _If(BoolExpr P1, Then P2, Expr P3, Else P4, Expr P5) {this.P1 = P1; this.P2 = P2; this.P3 = P3; this.P4 = P4; this.P5 = P5;}
public static _If Create(BoolExpr P1, Then P2, Expr P3, Else P4, Expr P5) { return new _If(P1, P2, P3, P4, P5); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run3_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run3_6_() { foreach(var p in Run3_()) yield return p; }
public IEnumerable<IRunnable> Run3_7_() { foreach(var p in Run3_()) yield return p; }

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

public override int GetHashCode() {
 return 0; 
}

}

public class lookup : Expr  {
public Locals P1;
public string P2;

public lookup(Locals P1, string P2) {this.P1 = P1; this.P2 = P2;}
public static lookup Create(Locals P1, string P2) { return new lookup(P1, P2); }

  public IEnumerable<IRunnable> Run() {   
 { 
 #line 63 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opDollarm; 
 #line 63 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var M = tmp_1.P1; var k = tmp_0.P2; var v = (M.GetKey (k)); 
 #line 63 "Content\Casanova semantics\transform.mc"
var result = v;
 #line 63 "Content\Casanova semantics\transform.mc"
yield return result;  }
 } 

  }

public IEnumerable<IRunnable> Run3_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run3_6_() { foreach(var p in Run3_()) yield return p; }
public IEnumerable<IRunnable> Run3_7_() { foreach(var p in Run3_()) yield return p; }

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

public override int GetHashCode() {
 return 0; 
}

}

public class nil<t> : List<t> where t : class {

public nil() {}
public static nil<t> Create() { return new nil<t>(); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run3_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run3_6_() { foreach(var p in Run3_()) yield return p; }
public IEnumerable<IRunnable> Run3_7_() { foreach(var p in Run3_()) yield return p; }

public override string ToString() {
return "nil";
}

public override bool Equals(object other) {
 return other is nil<t>; 
}

public override int GetHashCode() {
 return 0; 
}

}

public class runTest1 : Test  {

public runTest1() {}
public static runTest1 Create() { return new runTest1(); }

  public IEnumerable<IRunnable> Run() {   
 { 
 #line 72 "Content\Casanova semantics\transform.mc"
var tmp_0 = this as runTest1; var dt = 0.020000f; var M = _opDollarm.Create(System.Collections.Immutable.ImmutableDictionary<string,Expr >.Empty); 
 #line 72 "Content\Casanova semantics\transform.mc"
if(M is Locals) { 
 #line 72 "Content\Casanova semantics\transform.mc"
var tmp_2 = eval.Create(dt, M as Locals, _Semicolon.Create(wait.Create(0.010000f), yield.Create(_Comma<Expr>.Create(_opDollari.Create(1), nil<Expr>.Create()))));
 #line 72 "Content\Casanova semantics\transform.mc"
foreach (var tmp_1 in tmp_2.Run3_()) { var res = tmp_1; 
 #line 72 "Content\Casanova semantics\transform.mc"
var result = res;
 #line 72 "Content\Casanova semantics\transform.mc"
yield return result;  } }
 } 

  }

public IEnumerable<IRunnable> Run3_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run3_6_() { foreach(var p in Run3_()) yield return p; }
public IEnumerable<IRunnable> Run3_7_() { foreach(var p in Run3_()) yield return p; }

public override string ToString() {
return "runTest1";
}

public override bool Equals(object other) {
 return other is runTest1; 
}

public override int GetHashCode() {
 return 0; 
}

}

public class setDt : ExprResult  {
public float P1;

public setDt(float P1) {this.P1 = P1;}
public static setDt Create(float P1) { return new setDt(P1); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run3_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run3_6_() { foreach(var p in Run3_()) yield return p; }
public IEnumerable<IRunnable> Run3_7_() { foreach(var p in Run3_()) yield return p; }

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

public override int GetHashCode() {
 return 0; 
}

}

public class stepOrSuspend  : Expr  {
public float P1;
public Locals P2;
public ExprResult P3;
public Expr P4;

public stepOrSuspend (float P1, Locals P2, ExprResult P3, Expr P4) {this.P1 = P1; this.P2 = P2; this.P3 = P3; this.P4 = P4;}
public static stepOrSuspend  Create(float P1, Locals P2, ExprResult P3, Expr P4) { return new stepOrSuspend (P1, P2, P3, P4); }

  public IEnumerable<IRunnable> Run3_7_() {   
 { 
 #line 129 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var tmp_1 = tmp_0.P3 as setDt; 
 #line 129 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var dt_Prime = tmp_1.P1; var b = tmp_0.P4; 
 #line 129 "Content\Casanova semantics\transform.mc"
if(M is Locals && b is Expr) { 
 #line 129 "Content\Casanova semantics\transform.mc"
var tmp_3 = eval.Create(dt_Prime, M as Locals, b as Expr);
 #line 129 "Content\Casanova semantics\transform.mc"
foreach (var tmp_2 in tmp_3.Run3_7_()) { var res = tmp_2; 
 #line 129 "Content\Casanova semantics\transform.mc"
var result = res;
 #line 129 "Content\Casanova semantics\transform.mc"
yield return result;  } } }
 } 

  
 { 
 #line 133 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var tmp_1 = tmp_0.P3 as waitResult; 
 #line 133 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var t = tmp_1.P1; var b = tmp_0.P4; 
 #line 133 "Content\Casanova semantics\transform.mc"
if(b is Expr) { 
 #line 133 "Content\Casanova semantics\transform.mc"
var result = _Semicolon_Prime.Create(waitResult.Create(t), b as Expr);
 #line 133 "Content\Casanova semantics\transform.mc"
yield return result;  } }
 } 

  
 { 
 #line 136 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var tmp_1 = tmp_0.P3 as yieldResult; 
 #line 136 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var vals = tmp_1.P1; var b = tmp_0.P4; 
 #line 136 "Content\Casanova semantics\transform.mc"
if(vals is List<ExprResult> && b is Expr) { 
 #line 136 "Content\Casanova semantics\transform.mc"
var result = _Semicolon_Prime.Create(yieldResult.Create(vals as List<ExprResult>), b as Expr);
 #line 136 "Content\Casanova semantics\transform.mc"
yield return result;  } }
 } 

 foreach(var p in Run3_()) yield return p; }

public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run3_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run3_6_() { foreach(var p in Run3_()) yield return p; }

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

public override int GetHashCode() {
 return 0; 
}

}

public class _Then : Then  {

public _Then() {}
public static _Then Create() { return new _Then(); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run3_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run3_6_() { foreach(var p in Run3_()) yield return p; }
public IEnumerable<IRunnable> Run3_7_() { foreach(var p in Run3_()) yield return p; }

public override string ToString() {
return "then";
}

public override bool Equals(object other) {
 return other is _Then; 
}

public override int GetHashCode() {
 return 0; 
}

}

public class wait : Expr  {
public float P1;

public wait(float P1) {this.P1 = P1;}
public static wait Create(float P1) { return new wait(P1); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run3_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run3_6_() { foreach(var p in Run3_()) yield return p; }
public IEnumerable<IRunnable> Run3_7_() { foreach(var p in Run3_()) yield return p; }

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

public override int GetHashCode() {
 return 0; 
}

}

public class waitResult : ExprResult  {
public float P1;

public waitResult(float P1) {this.P1 = P1;}
public static waitResult Create(float P1) { return new waitResult(P1); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run3_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run3_6_() { foreach(var p in Run3_()) yield return p; }
public IEnumerable<IRunnable> Run3_7_() { foreach(var p in Run3_()) yield return p; }

public override string ToString() {
 var res = "("; 

 res += "waitResult"; res += P1.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as waitResult;
 if(tmp != null) return this.P1.Equals(tmp.P1); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class yield : Expr  {
public List<Expr> P1;

public yield(List<Expr> P1) {this.P1 = P1;}
public static yield Create(List<Expr> P1) { return new yield(P1); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run3_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run3_6_() { foreach(var p in Run3_()) yield return p; }
public IEnumerable<IRunnable> Run3_7_() { foreach(var p in Run3_()) yield return p; }

public override string ToString() {
 var res = "("; 

 res += "yield"; if (P1 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P1.ToString(); } 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as yield;
 if(tmp != null) return this.P1.Equals(tmp.P1); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class yieldResult : ExprResult  {
public List<ExprResult> P1;

public yieldResult(List<ExprResult> P1) {this.P1 = P1;}
public static yieldResult Create(List<ExprResult> P1) { return new yieldResult(P1); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run3_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run3_6_() { foreach(var p in Run3_()) yield return p; }
public IEnumerable<IRunnable> Run3_7_() { foreach(var p in Run3_()) yield return p; }

public override string ToString() {
 var res = "("; 

 res += "yieldResult"; if (P1 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P1.ToString(); } 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as yieldResult;
 if(tmp != null) return this.P1.Equals(tmp.P1); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class _opOr : BoolExpr  {
public BoolExpr P1;
public BoolExpr P2;

public _opOr(BoolExpr P1, BoolExpr P2) {this.P1 = P1; this.P2 = P2;}
public static _opOr Create(BoolExpr P1, BoolExpr P2) { return new _opOr(P1, P2); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run3_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run3_6_() { foreach(var p in Run3_()) yield return p; }
public IEnumerable<IRunnable> Run3_7_() { foreach(var p in Run3_()) yield return p; }

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

public override int GetHashCode() {
 return 0; 
}

}




public class EntryPoint {
 static public int Print(object s) { System.Console.WriteLine(s.ToString()); return 0; } 
static public IEnumerable<IRunnable> Run(bool printInput)
{
 #line 1 "input"
 var p = runTest1.Create();
if(printInput) System.Console.WriteLine(p.ToString());
foreach(var x in p.Run())
yield return x;
}
}

}
