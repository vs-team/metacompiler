using System.Collections.Generic;
using System.Linq;
namespace Casanova_semantics {
 public static class Extensions { public static V GetKey<T, V>(this System.Collections.Immutable.ImmutableDictionary<T, V> self, T key) { return self[key]; } }
 public interface IRunnable { IEnumerable<IRunnable> Run();
IEnumerable<IRunnable> Run3_();
IEnumerable<IRunnable> Run3_7_();
IEnumerable<IRunnable> Run3_8_();
 }


public interface BoolConst : BoolExpr, Expr, ExprResult {}
public interface BoolExpr : Expr {}
public interface Else : IRunnable {}
public interface Expr : IRunnable {}
public interface ExprList : Expr {}
public interface ExprResult : Expr {}
public interface ExprResultList : IRunnable {}
public interface Id : Expr {}
public interface IntConst : Expr, ExprResult, IntExpr {}
public interface IntExpr : Expr {}
public interface Locals : IRunnable {}
public interface MemoryOp : IRunnable {}
public interface Test : IRunnable {}
public interface Then : IRunnable {}



public class _opDollar : Id  {
public string P1;

public _opDollar(string P1) {this.P1 = P1;}
public static _opDollar Create(string P1) { return new _opDollar(P1); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run3_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run3_7_() { foreach(var p in Run3_()) yield return p; }
public IEnumerable<IRunnable> Run3_8_() { foreach(var p in Run3_()) yield return p; }

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

public class _opDollarb : BoolConst  {
public bool P1;

public _opDollarb(bool P1) {this.P1 = P1;}
public static _opDollarb Create(bool P1) { return new _opDollarb(P1); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run3_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run3_7_() { foreach(var p in Run3_()) yield return p; }
public IEnumerable<IRunnable> Run3_8_() { foreach(var p in Run3_()) yield return p; }

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
public IEnumerable<IRunnable> Run3_7_() { foreach(var p in Run3_()) yield return p; }
public IEnumerable<IRunnable> Run3_8_() { foreach(var p in Run3_()) yield return p; }

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
public System.Collections.Immutable.ImmutableDictionary<string,ExprResult> P1;

public _opDollarm(System.Collections.Immutable.ImmutableDictionary<string,ExprResult> P1) {this.P1 = P1;}
public static _opDollarm Create(System.Collections.Immutable.ImmutableDictionary<string,ExprResult> P1) { return new _opDollarm(P1); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run3_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run3_7_() { foreach(var p in Run3_()) yield return p; }
public IEnumerable<IRunnable> Run3_8_() { foreach(var p in Run3_()) yield return p; }

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
public IEnumerable<IRunnable> Run3_7_() { foreach(var p in Run3_()) yield return p; }
public IEnumerable<IRunnable> Run3_8_() { foreach(var p in Run3_()) yield return p; }

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
public IEnumerable<IRunnable> Run3_7_() { foreach(var p in Run3_()) yield return p; }
public IEnumerable<IRunnable> Run3_8_() { foreach(var p in Run3_()) yield return p; }

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
public IEnumerable<IRunnable> Run3_7_() { foreach(var p in Run3_()) yield return p; }
public IEnumerable<IRunnable> Run3_8_() { foreach(var p in Run3_()) yield return p; }

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

public class _opSubtraction : IntExpr  {
public IntExpr P1;
public IntExpr P2;

public _opSubtraction(IntExpr P1, IntExpr P2) {this.P1 = P1; this.P2 = P2;}
public static _opSubtraction Create(IntExpr P1, IntExpr P2) { return new _opSubtraction(P1, P2); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run3_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run3_7_() { foreach(var p in Run3_()) yield return p; }
public IEnumerable<IRunnable> Run3_8_() { foreach(var p in Run3_()) yield return p; }

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
public IEnumerable<IRunnable> Run3_7_() { foreach(var p in Run3_()) yield return p; }
public IEnumerable<IRunnable> Run3_8_() { foreach(var p in Run3_()) yield return p; }

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

public class _Colon : ExprResultList  {
public ExprResult P1;
public ExprResultList P2;

public _Colon(ExprResult P1, ExprResultList P2) {this.P1 = P1; this.P2 = P2;}
public static _Colon Create(ExprResult P1, ExprResultList P2) { return new _Colon(P1, P2); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run3_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run3_7_() { foreach(var p in Run3_()) yield return p; }
public IEnumerable<IRunnable> Run3_8_() { foreach(var p in Run3_()) yield return p; }

public override string ToString() {
 var res = "("; 
res += P1.ToString(); 

 res += ":"; res += P2.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _Colon;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class _Semicolon : ExprList  {
public Expr P1;
public ExprList P2;

public _Semicolon(Expr P1, ExprList P2) {this.P1 = P1; this.P2 = P2;}
public static _Semicolon Create(Expr P1, ExprList P2) { return new _Semicolon(P1, P2); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run3_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run3_7_() { foreach(var p in Run3_()) yield return p; }
public IEnumerable<IRunnable> Run3_8_() { foreach(var p in Run3_()) yield return p; }

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

public class _opEquals : BoolExpr  {
public IntExpr P1;
public IntExpr P2;

public _opEquals(IntExpr P1, IntExpr P2) {this.P1 = P1; this.P2 = P2;}
public static _opEquals Create(IntExpr P1, IntExpr P2) { return new _opEquals(P1, P2); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run3_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run3_7_() { foreach(var p in Run3_()) yield return p; }
public IEnumerable<IRunnable> Run3_8_() { foreach(var p in Run3_()) yield return p; }

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
public IEnumerable<IRunnable> Run3_7_() { foreach(var p in Run3_()) yield return p; }
public IEnumerable<IRunnable> Run3_8_() { foreach(var p in Run3_()) yield return p; }

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
public ExprResult P3;

public add(Locals P1, string P2, ExprResult P3) {this.P1 = P1; this.P2 = P2; this.P3 = P3;}
public static add Create(Locals P1, string P2, ExprResult P3) { return new add(P1, P2, P3); }

  public IEnumerable<IRunnable> Run() {   
 { 
 #line 71 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opDollarm; 
 #line 71 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var M = tmp_1.P1; var k = tmp_0.P2; var v = tmp_0.P3; var M_Prime = (M.Add(k,v)); 
 #line 71 "Content\Casanova semantics\transform.mc"
var result = _opDollarm.Create(M_Prime);
 #line 71 "Content\Casanova semantics\transform.mc"
yield return result;  }
 } 

  }

public IEnumerable<IRunnable> Run3_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run3_7_() { foreach(var p in Run3_()) yield return p; }
public IEnumerable<IRunnable> Run3_8_() { foreach(var p in Run3_()) yield return p; }

public override string ToString() {
 var res = "("; 
res += P1.ToString(); 

 res += "add"; if (P2 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P2 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P2.ToString(); } 
if (P3 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P3 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P3.ToString(); } 

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

public class continueWith : ExprResult  {
public ExprList P1;

public continueWith(ExprList P1) {this.P1 = P1;}
public static continueWith Create(ExprList P1) { return new continueWith(P1); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run3_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run3_7_() { foreach(var p in Run3_()) yield return p; }
public IEnumerable<IRunnable> Run3_8_() { foreach(var p in Run3_()) yield return p; }

public override string ToString() {
 var res = "("; 

 res += "continueWith"; res += P1.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as continueWith;
 if(tmp != null) return this.P1.Equals(tmp.P1); 
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
public IEnumerable<IRunnable> Run3_7_() { foreach(var p in Run3_()) yield return p; }
public IEnumerable<IRunnable> Run3_8_() { foreach(var p in Run3_()) yield return p; }

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
 #line 60 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var tmp_1 = tmp_0.P3 as _If; 
 #line 60 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var c = tmp_1.P1; var tmp_2 = tmp_1.P2 as _Then; 
 #line 60 "Content\Casanova semantics\transform.mc"
if (tmp_2 != null) { var t = tmp_1.P3; var tmp_3 = tmp_1.P4 as _Else; 
 #line 60 "Content\Casanova semantics\transform.mc"
if (tmp_3 != null) { var e = tmp_1.P5; 
 #line 60 "Content\Casanova semantics\transform.mc"
if(!c.Equals(_opDollarb.Create(true))) { 
 #line 60 "Content\Casanova semantics\transform.mc"
if(!c.Equals(_opDollarb.Create(false))) { 
 #line 60 "Content\Casanova semantics\transform.mc"
if(M is Locals && c is Expr) { 
 #line 60 "Content\Casanova semantics\transform.mc"
var tmp_5 = eval.Create(dt, M as Locals, c as Expr);
 #line 60 "Content\Casanova semantics\transform.mc"
foreach (var tmp_4 in tmp_5.Run()) { var c_Prime = tmp_4; 
 #line 60 "Content\Casanova semantics\transform.mc"
if(M is Locals && c_Prime is BoolExpr && t is ExprList && e is ExprList) { 
 #line 60 "Content\Casanova semantics\transform.mc"
var tmp_7 = eval.Create(dt, M as Locals, _If.Create(c_Prime as BoolExpr, _Then.Create(), t as ExprList, _Else.Create(), e as ExprList));
 #line 60 "Content\Casanova semantics\transform.mc"
foreach (var tmp_6 in tmp_7.Run()) { var res = tmp_6; 
 #line 60 "Content\Casanova semantics\transform.mc"
var result = res;
 #line 60 "Content\Casanova semantics\transform.mc"
yield return result;  } } } } } } } } }
 } 

  }
  public IEnumerable<IRunnable> Run3_() {   
 { 
 #line 83 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var tmp_1 = tmp_0.P3 as _If; 
 #line 83 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var tmp_2 = tmp_1.P1 as _opDollarb; 
 #line 83 "Content\Casanova semantics\transform.mc"
if (tmp_2 != null) { 
 #line 83 "Content\Casanova semantics\transform.mc"
if (tmp_2.P1 == true) { var tmp_3 = tmp_1.P2 as _Then; 
 #line 83 "Content\Casanova semantics\transform.mc"
if (tmp_3 != null) { var t = tmp_1.P3; var tmp_4 = tmp_1.P4 as _Else; 
 #line 83 "Content\Casanova semantics\transform.mc"
if (tmp_4 != null) { var e = tmp_1.P5; 
 #line 83 "Content\Casanova semantics\transform.mc"
if(M is Locals && t is Expr) { 
 #line 83 "Content\Casanova semantics\transform.mc"
var tmp_6 = eval.Create(dt, M as Locals, t as Expr);
 #line 83 "Content\Casanova semantics\transform.mc"
foreach (var tmp_5 in tmp_6.Run3_()) { var res = tmp_5; 
 #line 83 "Content\Casanova semantics\transform.mc"
var result = res;
 #line 83 "Content\Casanova semantics\transform.mc"
yield return result;  } } } } } } }
 } 

  
 { 
 #line 87 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var tmp_1 = tmp_0.P3 as _If; 
 #line 87 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var tmp_2 = tmp_1.P1 as _opDollarb; 
 #line 87 "Content\Casanova semantics\transform.mc"
if (tmp_2 != null) { 
 #line 87 "Content\Casanova semantics\transform.mc"
if (tmp_2.P1 == false) { var tmp_3 = tmp_1.P2 as _Then; 
 #line 87 "Content\Casanova semantics\transform.mc"
if (tmp_3 != null) { var t = tmp_1.P3; var tmp_4 = tmp_1.P4 as _Else; 
 #line 87 "Content\Casanova semantics\transform.mc"
if (tmp_4 != null) { var e = tmp_1.P5; 
 #line 87 "Content\Casanova semantics\transform.mc"
if(M is Locals && e is Expr) { 
 #line 87 "Content\Casanova semantics\transform.mc"
var tmp_6 = eval.Create(dt, M as Locals, e as Expr);
 #line 87 "Content\Casanova semantics\transform.mc"
foreach (var tmp_5 in tmp_6.Run3_()) { var res = tmp_5; 
 #line 87 "Content\Casanova semantics\transform.mc"
var result = res;
 #line 87 "Content\Casanova semantics\transform.mc"
yield return result;  } } } } } } }
 } 

  
 { 
 #line 91 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var tmp_1 = tmp_0.P3 as _opDollar; 
 #line 91 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var v = tmp_1.P1; 
 #line 91 "Content\Casanova semantics\transform.mc"
if(M is Locals) { 
 #line 91 "Content\Casanova semantics\transform.mc"
var tmp_3 = lookup.Create(M as Locals, v);
 #line 91 "Content\Casanova semantics\transform.mc"
foreach (var tmp_2 in tmp_3.Run3_()) { var res = tmp_2; 
 #line 91 "Content\Casanova semantics\transform.mc"
var result = res;
 #line 91 "Content\Casanova semantics\transform.mc"
yield return result;  } } }
 } 

  
 { 
 #line 95 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var tmp_1 = tmp_0.P3 as wait; 
 #line 95 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var t = tmp_1.P1; 
 #line 95 "Content\Casanova semantics\transform.mc"
if((dt>=t).Equals(true)) { var dt_Prime = (dt-t); 
 #line 95 "Content\Casanova semantics\transform.mc"
var result = setDt.Create(dt_Prime);
 #line 95 "Content\Casanova semantics\transform.mc"
yield return result;  } }
 } 

  
 { 
 #line 100 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var tmp_1 = tmp_0.P3 as wait; 
 #line 100 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var t = tmp_1.P1; 
 #line 100 "Content\Casanova semantics\transform.mc"
if((dt<t).Equals(true)) { var t_Prime = (t-dt); 
 #line 100 "Content\Casanova semantics\transform.mc"
var result = waitResult.Create(t_Prime);
 #line 100 "Content\Casanova semantics\transform.mc"
yield return result;  } }
 } 

  
 { 
 #line 105 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var tmp_1 = tmp_0.P3 as _opDollari; 
 #line 105 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var val = tmp_1.P1; 
 #line 105 "Content\Casanova semantics\transform.mc"
var result = _opDollari.Create(val);
 #line 105 "Content\Casanova semantics\transform.mc"
yield return result;  }
 } 

  
 { 
 #line 108 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var tmp_1 = tmp_0.P3 as yield; 
 #line 108 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var tmp_2 = tmp_1.P1 as _Semicolon; 
 #line 108 "Content\Casanova semantics\transform.mc"
if (tmp_2 != null) { var e = tmp_2.P1; var exprs = tmp_2.P2; 
 #line 108 "Content\Casanova semantics\transform.mc"
if(M is Locals && e is Expr && exprs is ExprList) { 
 #line 108 "Content\Casanova semantics\transform.mc"
var tmp_4 = evalMany.Create(dt, M as Locals, _Semicolon.Create(e as Expr, exprs as ExprList));
 #line 108 "Content\Casanova semantics\transform.mc"
foreach (var tmp_3 in tmp_4.Run3_()) { var vals = tmp_3; 
 #line 108 "Content\Casanova semantics\transform.mc"
if(vals is ExprResultList) { 
 #line 108 "Content\Casanova semantics\transform.mc"
var result = yieldResult.Create(vals as ExprResultList);
 #line 108 "Content\Casanova semantics\transform.mc"
yield return result;  } } } } }
 } 

  
 { 
 #line 112 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var tmp_1 = tmp_0.P3 as nil; 
 #line 112 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { 
 #line 112 "Content\Casanova semantics\transform.mc"
var result = nilResult.Create();
 #line 112 "Content\Casanova semantics\transform.mc"
yield return result;  }
 } 

  
 { 
 #line 126 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var tmp_1 = tmp_0.P3 as _Semicolon; 
 #line 126 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var a = tmp_1.P1; var b = tmp_1.P2; 
 #line 126 "Content\Casanova semantics\transform.mc"
if(M is Locals && a is Expr) { 
 #line 126 "Content\Casanova semantics\transform.mc"
var tmp_3 = eval.Create(dt, M as Locals, a as Expr);
 #line 126 "Content\Casanova semantics\transform.mc"
foreach (var tmp_2 in tmp_3.Run3_8_()) { var a_Prime = tmp_2; 
 #line 126 "Content\Casanova semantics\transform.mc"
if(M is Locals && a_Prime is ExprResult && b is Expr) { 
 #line 126 "Content\Casanova semantics\transform.mc"
var tmp_5 = stepOrSuspend.Create(dt, M as Locals, a_Prime as ExprResult, b as Expr);
 #line 126 "Content\Casanova semantics\transform.mc"
foreach (var tmp_4 in tmp_5.Run3_8_()) { var res = tmp_4; 
 #line 126 "Content\Casanova semantics\transform.mc"
var result = res;
 #line 126 "Content\Casanova semantics\transform.mc"
yield return result;  } } } } }
 } 

 foreach(var p in Run()) yield return p; }

public IEnumerable<IRunnable> Run3_7_() { foreach(var p in Run3_()) yield return p; }
public IEnumerable<IRunnable> Run3_8_() { foreach(var p in Run3_()) yield return p; }

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
public ExprList P3;

public evalMany(float P1, Locals P2, ExprList P3) {this.P1 = P1; this.P2 = P2; this.P3 = P3;}
public static evalMany Create(float P1, Locals P2, ExprList P3) { return new evalMany(P1, P2, P3); }

  public IEnumerable<IRunnable> Run3_7_() {   
 { 
 #line 116 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var tmp_1 = tmp_0.P3 as _Semicolon; 
 #line 116 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var e = tmp_1.P1; var exprs = tmp_1.P2; 
 #line 116 "Content\Casanova semantics\transform.mc"
if(M is Locals && e is Expr) { 
 #line 116 "Content\Casanova semantics\transform.mc"
var tmp_3 = eval.Create(dt, M as Locals, e as Expr);
 #line 116 "Content\Casanova semantics\transform.mc"
foreach (var tmp_2 in tmp_3.Run3_7_()) { var val = tmp_2; 
 #line 116 "Content\Casanova semantics\transform.mc"
if(M is Locals && exprs is ExprList) { 
 #line 116 "Content\Casanova semantics\transform.mc"
var tmp_5 = evalMany.Create(dt, M as Locals, exprs as ExprList);
 #line 116 "Content\Casanova semantics\transform.mc"
foreach (var tmp_4 in tmp_5.Run3_7_()) { var vals = tmp_4; 
 #line 116 "Content\Casanova semantics\transform.mc"
if (val is ExprResult && vals is ExprResultList) { var res = _Colon.Create(val as ExprResult, vals as ExprResultList); 
 #line 116 "Content\Casanova semantics\transform.mc"
if(val is ExprResult && vals is ExprResultList) { 
 #line 116 "Content\Casanova semantics\transform.mc"
var result = _Colon.Create(val as ExprResult, vals as ExprResultList);
 #line 116 "Content\Casanova semantics\transform.mc"
yield return result;  } } } } } } }
 } 

  
 { 
 #line 122 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var tmp_1 = tmp_0.P3 as nil; 
 #line 122 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { 
 #line 122 "Content\Casanova semantics\transform.mc"
var result = nilResult.Create();
 #line 122 "Content\Casanova semantics\transform.mc"
yield return result;  }
 } 

 foreach(var p in Run3_()) yield return p; }

public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run3_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run3_8_() { foreach(var p in Run3_()) yield return p; }

public override string ToString() {
 var res = "("; 

 res += "evalMany"; res += P1.ToString(); 
res += P2.ToString(); 
res += P3.ToString(); 

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

public class _If : Expr  {
public BoolExpr P1;
public Then P2;
public ExprList P3;
public Else P4;
public ExprList P5;

public _If(BoolExpr P1, Then P2, ExprList P3, Else P4, ExprList P5) {this.P1 = P1; this.P2 = P2; this.P3 = P3; this.P4 = P4; this.P5 = P5;}
public static _If Create(BoolExpr P1, Then P2, ExprList P3, Else P4, ExprList P5) { return new _If(P1, P2, P3, P4, P5); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run3_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run3_7_() { foreach(var p in Run3_()) yield return p; }
public IEnumerable<IRunnable> Run3_8_() { foreach(var p in Run3_()) yield return p; }

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

public class lookup : MemoryOp  {
public Locals P1;
public string P2;

public lookup(Locals P1, string P2) {this.P1 = P1; this.P2 = P2;}
public static lookup Create(Locals P1, string P2) { return new lookup(P1, P2); }

  public IEnumerable<IRunnable> Run() {   
 { 
 #line 67 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opDollarm; 
 #line 67 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var M = tmp_1.P1; var k = tmp_0.P2; var v = (M.GetKey(k)); 
 #line 67 "Content\Casanova semantics\transform.mc"
var result = v;
 #line 67 "Content\Casanova semantics\transform.mc"
yield return result;  }
 } 

  }

public IEnumerable<IRunnable> Run3_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run3_7_() { foreach(var p in Run3_()) yield return p; }
public IEnumerable<IRunnable> Run3_8_() { foreach(var p in Run3_()) yield return p; }

public override string ToString() {
 var res = "("; 
res += P1.ToString(); 

 res += "lookup"; if (P2 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P2 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P2.ToString(); } 

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

public class nil : ExprList  {

public nil() {}
public static nil Create() { return new nil(); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run3_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run3_7_() { foreach(var p in Run3_()) yield return p; }
public IEnumerable<IRunnable> Run3_8_() { foreach(var p in Run3_()) yield return p; }

public override string ToString() {
return "nil";
}

public override bool Equals(object other) {
 return other is nil; 
}

public override int GetHashCode() {
 return 0; 
}

}

public class nilResult : ExprResultList  {

public nilResult() {}
public static nilResult Create() { return new nilResult(); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run3_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run3_7_() { foreach(var p in Run3_()) yield return p; }
public IEnumerable<IRunnable> Run3_8_() { foreach(var p in Run3_()) yield return p; }

public override string ToString() {
return "nilResult";
}

public override bool Equals(object other) {
 return other is nilResult; 
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
 #line 75 "Content\Casanova semantics\transform.mc"
var tmp_0 = this as runTest1; var dt = 0.020000f; var M = _opDollarm.Create(System.Collections.Immutable.ImmutableDictionary<string,ExprResult>.Empty); var y1 = yield.Create(_Semicolon.Create(_opDollari.Create(1), nil.Create())); var y2 = yield.Create(_Semicolon.Create(_opDollari.Create(2), nil.Create())); 
 #line 75 "Content\Casanova semantics\transform.mc"
if(M is Locals && y1 is Expr && y2 is Expr) { 
 #line 75 "Content\Casanova semantics\transform.mc"
var tmp_2 = eval.Create(dt, M as Locals, _Semicolon.Create(y1 as Expr, _Semicolon.Create(y2 as Expr, nil.Create())));
 #line 75 "Content\Casanova semantics\transform.mc"
foreach (var tmp_1 in tmp_2.Run3_()) { var res = tmp_1; 
 #line 75 "Content\Casanova semantics\transform.mc"
var result = res;
 #line 75 "Content\Casanova semantics\transform.mc"
yield return result;  } }
 } 

  }

public IEnumerable<IRunnable> Run3_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run3_7_() { foreach(var p in Run3_()) yield return p; }
public IEnumerable<IRunnable> Run3_8_() { foreach(var p in Run3_()) yield return p; }

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
public IEnumerable<IRunnable> Run3_7_() { foreach(var p in Run3_()) yield return p; }
public IEnumerable<IRunnable> Run3_8_() { foreach(var p in Run3_()) yield return p; }

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

public class stepOrSuspend : Expr  {
public float P1;
public Locals P2;
public ExprResult P3;
public Expr P4;

public stepOrSuspend(float P1, Locals P2, ExprResult P3, Expr P4) {this.P1 = P1; this.P2 = P2; this.P3 = P3; this.P4 = P4;}
public static stepOrSuspend Create(float P1, Locals P2, ExprResult P3, Expr P4) { return new stepOrSuspend(P1, P2, P3, P4); }

  public IEnumerable<IRunnable> Run3_8_() {   
 { 
 #line 131 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var tmp_1 = tmp_0.P3 as setDt; 
 #line 131 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var dt_Prime = tmp_1.P1; var b = tmp_0.P4; 
 #line 131 "Content\Casanova semantics\transform.mc"
if(M is Locals && b is Expr) { 
 #line 131 "Content\Casanova semantics\transform.mc"
var tmp_3 = eval.Create(dt_Prime, M as Locals, b as Expr);
 #line 131 "Content\Casanova semantics\transform.mc"
foreach (var tmp_2 in tmp_3.Run3_8_()) { var res = tmp_2; 
 #line 131 "Content\Casanova semantics\transform.mc"
var result = res;
 #line 131 "Content\Casanova semantics\transform.mc"
yield return result;  } } }
 } 

  
 { 
 #line 136 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var tmp_1 = tmp_0.P3 as waitResult; 
 #line 136 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var t = tmp_1.P1; var b = tmp_0.P4; 
 #line 136 "Content\Casanova semantics\transform.mc"
if (b is ExprList) { var res = continueWith.Create(_Semicolon.Create(wait.Create(t), b as ExprList)); 
 #line 136 "Content\Casanova semantics\transform.mc"
var result = res;
 #line 136 "Content\Casanova semantics\transform.mc"
yield return result;  } }
 } 

  
 { 
 #line 140 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var tmp_1 = tmp_0.P3 as yieldResult; 
 #line 140 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var vals = tmp_1.P1; var b = tmp_0.P4; 
 #line 140 "Content\Casanova semantics\transform.mc"
if (vals is ExprResultList && b is ExprList) { var res = updateFieldsAndContinueWith.Create(vals as ExprResultList, b as ExprList); 
 #line 140 "Content\Casanova semantics\transform.mc"
var result = res;
 #line 140 "Content\Casanova semantics\transform.mc"
yield return result;  } }
 } 

 foreach(var p in Run3_()) yield return p; }

public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run3_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run3_7_() { foreach(var p in Run3_()) yield return p; }

public override string ToString() {
 var res = "("; 

 res += "stepOrSuspend"; res += P1.ToString(); 
res += P2.ToString(); 
res += P3.ToString(); 
res += P4.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as stepOrSuspend;
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
public IEnumerable<IRunnable> Run3_7_() { foreach(var p in Run3_()) yield return p; }
public IEnumerable<IRunnable> Run3_8_() { foreach(var p in Run3_()) yield return p; }

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

public class updateFieldsAndContinueWith : ExprResult  {
public ExprResultList P1;
public ExprList P2;

public updateFieldsAndContinueWith(ExprResultList P1, ExprList P2) {this.P1 = P1; this.P2 = P2;}
public static updateFieldsAndContinueWith Create(ExprResultList P1, ExprList P2) { return new updateFieldsAndContinueWith(P1, P2); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run3_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run3_7_() { foreach(var p in Run3_()) yield return p; }
public IEnumerable<IRunnable> Run3_8_() { foreach(var p in Run3_()) yield return p; }

public override string ToString() {
 var res = "("; 

 res += "updateFieldsAndContinueWith"; res += P1.ToString(); 
res += P2.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as updateFieldsAndContinueWith;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
 else return false; }

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
public IEnumerable<IRunnable> Run3_7_() { foreach(var p in Run3_()) yield return p; }
public IEnumerable<IRunnable> Run3_8_() { foreach(var p in Run3_()) yield return p; }

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
public IEnumerable<IRunnable> Run3_7_() { foreach(var p in Run3_()) yield return p; }
public IEnumerable<IRunnable> Run3_8_() { foreach(var p in Run3_()) yield return p; }

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
public ExprList P1;

public yield(ExprList P1) {this.P1 = P1;}
public static yield Create(ExprList P1) { return new yield(P1); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run3_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run3_7_() { foreach(var p in Run3_()) yield return p; }
public IEnumerable<IRunnable> Run3_8_() { foreach(var p in Run3_()) yield return p; }

public override string ToString() {
 var res = "("; 

 res += "yield"; res += P1.ToString(); 

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
public ExprResultList P1;

public yieldResult(ExprResultList P1) {this.P1 = P1;}
public static yieldResult Create(ExprResultList P1) { return new yieldResult(P1); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run3_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run3_7_() { foreach(var p in Run3_()) yield return p; }
public IEnumerable<IRunnable> Run3_8_() { foreach(var p in Run3_()) yield return p; }

public override string ToString() {
 var res = "("; 

 res += "yieldResult"; res += P1.ToString(); 

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
public IEnumerable<IRunnable> Run3_7_() { foreach(var p in Run3_()) yield return p; }
public IEnumerable<IRunnable> Run3_8_() { foreach(var p in Run3_()) yield return p; }

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
