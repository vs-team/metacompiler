using System.Collections.Generic;
using System.Linq;
namespace Casanova_semantics {
 public static class Extensions { public static V GetKey<T, V>(this System.Collections.Immutable.ImmutableDictionary<T, V> self, T key) { return self[key]; } }
 public interface IRunnable { IEnumerable<IRunnable> Run();
IEnumerable<IRunnable> Run4_();
IEnumerable<IRunnable> Run4_6_();
IEnumerable<IRunnable> Run4_16_();
IEnumerable<IRunnable> Run4_17_();
 }


public interface BoolConst : BoolExpr, Expr, ExprResult {}
public interface BoolExpr : Expr {}
public interface Domain : IRunnable {}
public interface Else : IRunnable {}
public interface Entity : IRunnable {}
public interface EntityManager : IRunnable {}
public interface Expr : IRunnable {}
public interface ExprList : Expr {}
public interface ExprResult : Expr {}
public interface ExprResultList : IRunnable {}
public interface Id : Expr {}
public interface IntConst : Expr, ExprResult, IntExpr {}
public interface IntExpr : Expr {}
public interface Locals : IRunnable {}
public interface MemoryOp : IRunnable {}
public interface Rule : IRunnable {}
public interface RuleEvaluation : IRunnable {}
public interface RuleList : IRunnable {}
public interface RuleManager : IRunnable {}
public interface RuleResult : IRunnable {}
public interface Test : IRunnable {}
public interface Then : IRunnable {}
public interface UpdateResult : IRunnable {}



public class _opDollar : Id  {
public string P1;

public _opDollar(string P1) {this.P1 = P1;}
public static _opDollar Create(string P1) { return new _opDollar(P1); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run4_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run4_6_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_16_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_17_() { foreach(var p in Run4_()) yield return p; }

public override string ToString() {
 var res = "("; 

 res += " $ "; if (P1 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P1.ToString(); } 

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
public IEnumerable<IRunnable> Run4_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run4_6_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_16_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_17_() { foreach(var p in Run4_()) yield return p; }

public override string ToString() {
 var res = "("; 

 res += " $b "; res += P1.ToString(); 

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
public IEnumerable<IRunnable> Run4_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run4_6_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_16_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_17_() { foreach(var p in Run4_()) yield return p; }

public override string ToString() {
 var res = "("; 

 res += " $i "; res += P1.ToString(); 

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
public IEnumerable<IRunnable> Run4_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run4_6_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_16_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_17_() { foreach(var p in Run4_()) yield return p; }

public override string ToString() {
 var res = "("; 

 res += " $m "; if (P1 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P1.ToString(); } 

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
public IEnumerable<IRunnable> Run4_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run4_6_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_16_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_17_() { foreach(var p in Run4_()) yield return p; }

public override string ToString() {
 var res = "("; 
res += P1.ToString(); 

 res += " && "; res += P2.ToString(); 

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
public IEnumerable<IRunnable> Run4_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run4_6_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_16_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_17_() { foreach(var p in Run4_()) yield return p; }

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

public class _opAddition : IntExpr  {
public IntExpr P1;
public IntExpr P2;

public _opAddition(IntExpr P1, IntExpr P2) {this.P1 = P1; this.P2 = P2;}
public static _opAddition Create(IntExpr P1, IntExpr P2) { return new _opAddition(P1, P2); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run4_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run4_6_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_16_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_17_() { foreach(var p in Run4_()) yield return p; }

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

public class _opSubtraction : IntExpr  {
public IntExpr P1;
public IntExpr P2;

public _opSubtraction(IntExpr P1, IntExpr P2) {this.P1 = P1; this.P2 = P2;}
public static _opSubtraction Create(IntExpr P1, IntExpr P2) { return new _opSubtraction(P1, P2); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run4_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run4_6_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_16_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_17_() { foreach(var p in Run4_()) yield return p; }

public override string ToString() {
 var res = "("; 
res += P1.ToString(); 

 res += " - "; res += P2.ToString(); 

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
public IEnumerable<IRunnable> Run4_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run4_6_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_16_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_17_() { foreach(var p in Run4_()) yield return p; }

public override string ToString() {
 var res = "("; 
res += P1.ToString(); 

 res += " / "; res += P2.ToString(); 

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

public class _Semicolon : ExprList  {
public Expr P1;
public ExprList P2;

public _Semicolon(Expr P1, ExprList P2) {this.P1 = P1; this.P2 = P2;}
public static _Semicolon Create(Expr P1, ExprList P2) { return new _Semicolon(P1, P2); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run4_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run4_6_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_16_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_17_() { foreach(var p in Run4_()) yield return p; }

public override string ToString() {
 var res = "("; 
res += P1.ToString(); 

 res += " ; "; res += P2.ToString(); 

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
public IEnumerable<IRunnable> Run4_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run4_6_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_16_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_17_() { foreach(var p in Run4_()) yield return p; }

public override string ToString() {
 var res = "("; 
res += P1.ToString(); 

 res += " = "; res += P2.ToString(); 

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
public IEnumerable<IRunnable> Run4_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run4_6_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_16_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_17_() { foreach(var p in Run4_()) yield return p; }

public override string ToString() {
 var res = "("; 
res += P1.ToString(); 

 res += " > "; res += P2.ToString(); 

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
 #line 96 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opDollarm; 
 #line 96 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var M = tmp_1.P1; var k = tmp_0.P2; var v = tmp_0.P3; 
 #line 96 "Content\Casanova semantics\transform.mc"
if((M.ContainsKey(k)).Equals(false)) { var M_Prime = (M.Add(k,v)); 
 #line 96 "Content\Casanova semantics\transform.mc"
var result = _opDollarm.Create(M_Prime);
 #line 96 "Content\Casanova semantics\transform.mc"
yield return result;  } }
 } 

  
 { 
 #line 101 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opDollarm; 
 #line 101 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var M = tmp_1.P1; var k = tmp_0.P2; var v = tmp_0.P3; 
 #line 101 "Content\Casanova semantics\transform.mc"
if((M.ContainsKey(k)).Equals(true)) { var M_Prime = (M.SetItem(k,v)); 
 #line 101 "Content\Casanova semantics\transform.mc"
var result = _opDollarm.Create(M_Prime);
 #line 101 "Content\Casanova semantics\transform.mc"
yield return result;  } }
 } 

  }

public IEnumerable<IRunnable> Run4_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run4_6_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_16_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_17_() { foreach(var p in Run4_()) yield return p; }

public override string ToString() {
 var res = "("; 
res += P1.ToString(); 

 res += " add "; if (P2 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P2 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P2.ToString(); } 
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

public class atomic : ExprResult  {

public atomic() {}
public static atomic Create() { return new atomic(); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run4_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run4_6_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_16_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_17_() { foreach(var p in Run4_()) yield return p; }

public override string ToString() {
return "atomic";
}

public override bool Equals(object other) {
 return other is atomic; 
}

public override int GetHashCode() {
 return 0; 
}

}

public class consDomain : Domain  {
public string P1;
public Domain P2;

public consDomain(string P1, Domain P2) {this.P1 = P1; this.P2 = P2;}
public static consDomain Create(string P1, Domain P2) { return new consDomain(P1, P2); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run4_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run4_6_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_16_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_17_() { foreach(var p in Run4_()) yield return p; }

public override string ToString() {
 var res = "("; 
if (P1 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P1.ToString(); } 

 res += " consDomain "; res += P2.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as consDomain;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class consResult : ExprResultList  {
public ExprResult P1;
public ExprResultList P2;

public consResult(ExprResult P1, ExprResultList P2) {this.P1 = P1; this.P2 = P2;}
public static consResult Create(ExprResult P1, ExprResultList P2) { return new consResult(P1, P2); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run4_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run4_6_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_16_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_17_() { foreach(var p in Run4_()) yield return p; }

public override string ToString() {
 var res = "("; 
res += P1.ToString(); 

 res += " consResult "; res += P2.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as consResult;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class consRule : RuleList  {
public Rule P1;
public RuleList P2;

public consRule(Rule P1, RuleList P2) {this.P1 = P1; this.P2 = P2;}
public static consRule Create(Rule P1, RuleList P2) { return new consRule(P1, P2); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run4_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run4_6_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_16_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_17_() { foreach(var p in Run4_()) yield return p; }

public override string ToString() {
 var res = "("; 
res += P1.ToString(); 

 res += " consRule "; res += P2.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as consRule;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
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
public IEnumerable<IRunnable> Run4_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run4_6_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_16_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_17_() { foreach(var p in Run4_()) yield return p; }

public override string ToString() {
 var res = "("; 

 res += " continueWith "; res += P1.ToString(); 

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
public IEnumerable<IRunnable> Run4_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run4_6_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_16_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_17_() { foreach(var p in Run4_()) yield return p; }

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

public class entity : Entity  {
public string P1;
public Locals P2;
public RuleList P3;

public entity(string P1, Locals P2, RuleList P3) {this.P1 = P1; this.P2 = P2; this.P3 = P3;}
public static entity Create(string P1, Locals P2, RuleList P3) { return new entity(P1, P2, P3); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run4_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run4_6_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_16_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_17_() { foreach(var p in Run4_()) yield return p; }

public override string ToString() {
 var res = "("; 

 res += " entity "; if (P1 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P1.ToString(); } 
res += P2.ToString(); 
res += P3.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as entity;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2) && this.P3.Equals(tmp.P3); 
 else return false; }

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
 #line 85 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var tmp_1 = tmp_0.P3 as _If; 
 #line 85 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var c = tmp_1.P1; var tmp_2 = tmp_1.P2 as _Then; 
 #line 85 "Content\Casanova semantics\transform.mc"
if (tmp_2 != null) { var t = tmp_1.P3; var tmp_3 = tmp_1.P4 as _Else; 
 #line 85 "Content\Casanova semantics\transform.mc"
if (tmp_3 != null) { var e = tmp_1.P5; 
 #line 85 "Content\Casanova semantics\transform.mc"
if(!c.Equals(_opDollarb.Create(true))) { 
 #line 85 "Content\Casanova semantics\transform.mc"
if(!c.Equals(_opDollarb.Create(false))) { 
 #line 85 "Content\Casanova semantics\transform.mc"
if(M is Locals && c is Expr) { 
 #line 85 "Content\Casanova semantics\transform.mc"
var tmp_5 = eval.Create(dt, M as Locals, c as Expr);
 #line 85 "Content\Casanova semantics\transform.mc"
foreach (var tmp_4 in tmp_5.Run()) { var c_Prime = tmp_4; 
 #line 85 "Content\Casanova semantics\transform.mc"
if(M is Locals && c_Prime is BoolExpr && t is ExprList && e is ExprList) { 
 #line 85 "Content\Casanova semantics\transform.mc"
var tmp_7 = eval.Create(dt, M as Locals, _If.Create(c_Prime as BoolExpr, _Then.Create(), t as ExprList, _Else.Create(), e as ExprList));
 #line 85 "Content\Casanova semantics\transform.mc"
foreach (var tmp_6 in tmp_7.Run()) { var res = tmp_6; 
 #line 85 "Content\Casanova semantics\transform.mc"
var result = res;
 #line 85 "Content\Casanova semantics\transform.mc"
yield return result;  } } } } } } } } }
 } 

  }
  public IEnumerable<IRunnable> Run4_() {   
 { 
 #line 183 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var tmp_1 = tmp_0.P3 as _If; 
 #line 183 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var tmp_2 = tmp_1.P1 as _opDollarb; 
 #line 183 "Content\Casanova semantics\transform.mc"
if (tmp_2 != null) { 
 #line 183 "Content\Casanova semantics\transform.mc"
if (tmp_2.P1 == true) { var tmp_3 = tmp_1.P2 as _Then; 
 #line 183 "Content\Casanova semantics\transform.mc"
if (tmp_3 != null) { var t = tmp_1.P3; var tmp_4 = tmp_1.P4 as _Else; 
 #line 183 "Content\Casanova semantics\transform.mc"
if (tmp_4 != null) { var e = tmp_1.P5; 
 #line 183 "Content\Casanova semantics\transform.mc"
if(M is Locals && t is Expr) { 
 #line 183 "Content\Casanova semantics\transform.mc"
var tmp_6 = eval.Create(dt, M as Locals, t as Expr);
 #line 183 "Content\Casanova semantics\transform.mc"
foreach (var tmp_5 in tmp_6.Run4_()) { var res = tmp_5; 
 #line 183 "Content\Casanova semantics\transform.mc"
var result = res;
 #line 183 "Content\Casanova semantics\transform.mc"
yield return result;  } } } } } } }
 } 

  
 { 
 #line 187 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var tmp_1 = tmp_0.P3 as _If; 
 #line 187 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var tmp_2 = tmp_1.P1 as _opDollarb; 
 #line 187 "Content\Casanova semantics\transform.mc"
if (tmp_2 != null) { 
 #line 187 "Content\Casanova semantics\transform.mc"
if (tmp_2.P1 == false) { var tmp_3 = tmp_1.P2 as _Then; 
 #line 187 "Content\Casanova semantics\transform.mc"
if (tmp_3 != null) { var t = tmp_1.P3; var tmp_4 = tmp_1.P4 as _Else; 
 #line 187 "Content\Casanova semantics\transform.mc"
if (tmp_4 != null) { var e = tmp_1.P5; 
 #line 187 "Content\Casanova semantics\transform.mc"
if(M is Locals && e is Expr) { 
 #line 187 "Content\Casanova semantics\transform.mc"
var tmp_6 = eval.Create(dt, M as Locals, e as Expr);
 #line 187 "Content\Casanova semantics\transform.mc"
foreach (var tmp_5 in tmp_6.Run4_()) { var res = tmp_5; 
 #line 187 "Content\Casanova semantics\transform.mc"
var result = res;
 #line 187 "Content\Casanova semantics\transform.mc"
yield return result;  } } } } } } }
 } 

  
 { 
 #line 191 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var tmp_1 = tmp_0.P3 as _opDollar; 
 #line 191 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var v = tmp_1.P1; 
 #line 191 "Content\Casanova semantics\transform.mc"
if(M is Locals) { 
 #line 191 "Content\Casanova semantics\transform.mc"
var tmp_3 = lookup.Create(M as Locals, v);
 #line 191 "Content\Casanova semantics\transform.mc"
foreach (var tmp_2 in tmp_3.Run4_()) { var res = tmp_2; 
 #line 191 "Content\Casanova semantics\transform.mc"
var result = res;
 #line 191 "Content\Casanova semantics\transform.mc"
yield return result;  } } }
 } 

  
 { 
 #line 195 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var tmp_1 = tmp_0.P3 as wait; 
 #line 195 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var t = tmp_1.P1; 
 #line 195 "Content\Casanova semantics\transform.mc"
if((dt>=t).Equals(true)) { 
 #line 195 "Content\Casanova semantics\transform.mc"
var result = atomic.Create();
 #line 195 "Content\Casanova semantics\transform.mc"
yield return result;  } }
 } 

  
 { 
 #line 199 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var tmp_1 = tmp_0.P3 as wait; 
 #line 199 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var t = tmp_1.P1; 
 #line 199 "Content\Casanova semantics\transform.mc"
if((dt<t).Equals(true)) { var t_Prime = (t-dt); 
 #line 199 "Content\Casanova semantics\transform.mc"
var result = waitResult.Create(t_Prime);
 #line 199 "Content\Casanova semantics\transform.mc"
yield return result;  } }
 } 

  
 { 
 #line 204 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var tmp_1 = tmp_0.P3 as _opDollari; 
 #line 204 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var val = tmp_1.P1; 
 #line 204 "Content\Casanova semantics\transform.mc"
var result = _opDollari.Create(val);
 #line 204 "Content\Casanova semantics\transform.mc"
yield return result;  }
 } 

  
 { 
 #line 207 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var tmp_1 = tmp_0.P3 as nil; 
 #line 207 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { 
 #line 207 "Content\Casanova semantics\transform.mc"
var result = nilResult.Create();
 #line 207 "Content\Casanova semantics\transform.mc"
yield return result;  }
 } 

  
 { 
 #line 210 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var tmp_1 = tmp_0.P3 as yield; 
 #line 210 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var tmp_2 = tmp_1.P1 as _Semicolon; 
 #line 210 "Content\Casanova semantics\transform.mc"
if (tmp_2 != null) { var e = tmp_2.P1; var exprs = tmp_2.P2; 
 #line 210 "Content\Casanova semantics\transform.mc"
if(M is Locals && e is Expr && exprs is ExprList) { 
 #line 210 "Content\Casanova semantics\transform.mc"
var tmp_4 = evalMany.Create(dt, M as Locals, _Semicolon.Create(e as Expr, exprs as ExprList));
 #line 210 "Content\Casanova semantics\transform.mc"
foreach (var tmp_3 in tmp_4.Run4_16_()) { var vals = tmp_3; 
 #line 210 "Content\Casanova semantics\transform.mc"
if(vals is ExprResultList) { 
 #line 210 "Content\Casanova semantics\transform.mc"
var result = yieldResult.Create(vals as ExprResultList);
 #line 210 "Content\Casanova semantics\transform.mc"
yield return result;  } } } } }
 } 

  
 { 
 #line 224 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var tmp_1 = tmp_0.P3 as _Semicolon; 
 #line 224 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var a = tmp_1.P1; var b = tmp_1.P2; 
 #line 224 "Content\Casanova semantics\transform.mc"
if(M is Locals && a is Expr) { 
 #line 224 "Content\Casanova semantics\transform.mc"
var tmp_3 = eval.Create(dt, M as Locals, a as Expr);
 #line 224 "Content\Casanova semantics\transform.mc"
foreach (var tmp_2 in tmp_3.Run4_17_()) { var a1 = tmp_2; 
 #line 224 "Content\Casanova semantics\transform.mc"
if(M is Locals && a1 is ExprResult && b is Expr) { 
 #line 224 "Content\Casanova semantics\transform.mc"
var tmp_5 = stepOrSuspend.Create(dt, M as Locals, a1 as ExprResult, b as Expr);
 #line 224 "Content\Casanova semantics\transform.mc"
foreach (var tmp_4 in tmp_5.Run4_17_()) { var res = tmp_4; 
 #line 224 "Content\Casanova semantics\transform.mc"
var result = res;
 #line 224 "Content\Casanova semantics\transform.mc"
yield return result;  } } } } }
 } 

 foreach(var p in Run()) yield return p; }

public IEnumerable<IRunnable> Run4_6_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_16_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_17_() { foreach(var p in Run4_()) yield return p; }

public override string ToString() {
 var res = "("; 

 res += " eval "; res += P1.ToString(); 
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

  public IEnumerable<IRunnable> Run4_16_() {   
 { 
 #line 214 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var tmp_1 = tmp_0.P3 as _Semicolon; 
 #line 214 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var e = tmp_1.P1; var exprs = tmp_1.P2; 
 #line 214 "Content\Casanova semantics\transform.mc"
if(M is Locals && e is Expr) { 
 #line 214 "Content\Casanova semantics\transform.mc"
var tmp_3 = eval.Create(dt, M as Locals, e as Expr);
 #line 214 "Content\Casanova semantics\transform.mc"
foreach (var tmp_2 in tmp_3.Run4_16_()) { var val = tmp_2; 
 #line 214 "Content\Casanova semantics\transform.mc"
if(M is Locals && exprs is ExprList) { 
 #line 214 "Content\Casanova semantics\transform.mc"
var tmp_5 = evalMany.Create(dt, M as Locals, exprs as ExprList);
 #line 214 "Content\Casanova semantics\transform.mc"
foreach (var tmp_4 in tmp_5.Run4_16_()) { var vals = tmp_4; 
 #line 214 "Content\Casanova semantics\transform.mc"
if (val is ExprResult && vals is ExprResultList) { var res = consResult.Create(val as ExprResult, vals as ExprResultList); 
 #line 214 "Content\Casanova semantics\transform.mc"
if(val is ExprResult && vals is ExprResultList) { 
 #line 214 "Content\Casanova semantics\transform.mc"
var result = consResult.Create(val as ExprResult, vals as ExprResultList);
 #line 214 "Content\Casanova semantics\transform.mc"
yield return result;  } } } } } } }
 } 

  
 { 
 #line 220 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var tmp_1 = tmp_0.P3 as nil; 
 #line 220 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { 
 #line 220 "Content\Casanova semantics\transform.mc"
var result = nilResult.Create();
 #line 220 "Content\Casanova semantics\transform.mc"
yield return result;  }
 } 

 foreach(var p in Run4_()) yield return p; }

public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run4_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run4_6_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_17_() { foreach(var p in Run4_()) yield return p; }

public override string ToString() {
 var res = "("; 

 res += " evalMany "; res += P1.ToString(); 
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

public class evalRule : RuleEvaluation  {
public float P1;
public Locals P2;
public Domain P3;
public ExprList P4;
public ExprList P5;

public evalRule(float P1, Locals P2, Domain P3, ExprList P4, ExprList P5) {this.P1 = P1; this.P2 = P2; this.P3 = P3; this.P4 = P4; this.P5 = P5;}
public static evalRule Create(float P1, Locals P2, Domain P3, ExprList P4, ExprList P5) { return new evalRule(P1, P2, P3, P4, P5); }

  public IEnumerable<IRunnable> Run4_() {   
 { 
 #line 158 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var domain = tmp_0.P3; var startingBlock = tmp_0.P4; var block = tmp_0.P5; 
 #line 158 "Content\Casanova semantics\transform.mc"
if(M is Locals && block is Expr) { 
 #line 158 "Content\Casanova semantics\transform.mc"
var tmp_2 = eval.Create(dt, M as Locals, block as Expr);
 #line 158 "Content\Casanova semantics\transform.mc"
foreach (var tmp_1 in tmp_2.Run4_()) { var tmp_3 = tmp_1 as continueWith; 
 #line 158 "Content\Casanova semantics\transform.mc"
if (tmp_3 != null) { var tmp_4 = tmp_3.P1 as _Semicolon; 
 #line 158 "Content\Casanova semantics\transform.mc"
if (tmp_4 != null) { var tmp_5 = tmp_4.P1 as wait; 
 #line 158 "Content\Casanova semantics\transform.mc"
if (tmp_5 != null) { var t = tmp_5.P1; var b = tmp_4.P2; 
 #line 158 "Content\Casanova semantics\transform.mc"
if(M is Locals && b is ExprList) { 
 #line 158 "Content\Casanova semantics\transform.mc"
var result = ruleResult.Create(M as Locals, _Semicolon.Create(wait.Create(t), b as ExprList));
 #line 158 "Content\Casanova semantics\transform.mc"
yield return result;  } } } } } }
 } 

  
 { 
 #line 162 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var domain = tmp_0.P3; var startingBlock = tmp_0.P4; var block = tmp_0.P5; 
 #line 162 "Content\Casanova semantics\transform.mc"
if(M is Locals && block is Expr) { 
 #line 162 "Content\Casanova semantics\transform.mc"
var tmp_2 = eval.Create(dt, M as Locals, block as Expr);
 #line 162 "Content\Casanova semantics\transform.mc"
foreach (var tmp_1 in tmp_2.Run4_6_()) { var tmp_3 = tmp_1 as updateFieldsAndContinueWith; 
 #line 162 "Content\Casanova semantics\transform.mc"
if (tmp_3 != null) { var vals = tmp_3.P1; var b = tmp_3.P2; 
 #line 162 "Content\Casanova semantics\transform.mc"
if(M is Locals && domain is Domain && vals is ExprResultList) { 
 #line 162 "Content\Casanova semantics\transform.mc"
var tmp_5 = updateFields.Create(M as Locals, domain as Domain, vals as ExprResultList);
 #line 162 "Content\Casanova semantics\transform.mc"
foreach (var tmp_4 in tmp_5.Run4_6_()) { var M_Prime = tmp_4; 
 #line 162 "Content\Casanova semantics\transform.mc"
if(M_Prime is Locals && b is ExprList) { 
 #line 162 "Content\Casanova semantics\transform.mc"
var result = ruleResult.Create(M_Prime as Locals, b as ExprList);
 #line 162 "Content\Casanova semantics\transform.mc"
yield return result;  } } } } } }
 } 

  
 { 
 #line 175 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var domain = tmp_0.P3; var startingBlock = tmp_0.P4; var block = tmp_0.P5; 
 #line 175 "Content\Casanova semantics\transform.mc"
if(M is Locals && block is Expr) { 
 #line 175 "Content\Casanova semantics\transform.mc"
var tmp_2 = eval.Create(dt, M as Locals, block as Expr);
 #line 175 "Content\Casanova semantics\transform.mc"
foreach (var tmp_1 in tmp_2.Run4_()) { var tmp_3 = tmp_1 as reEvaluate; 
 #line 175 "Content\Casanova semantics\transform.mc"
if (tmp_3 != null) { var b = tmp_3.P1; 
 #line 175 "Content\Casanova semantics\transform.mc"
if(M is Locals && domain is Domain && startingBlock is ExprList && b is ExprList) { 
 #line 175 "Content\Casanova semantics\transform.mc"
var tmp_5 = evalRule.Create(dt, M as Locals, domain as Domain, startingBlock as ExprList, b as ExprList);
 #line 175 "Content\Casanova semantics\transform.mc"
foreach (var tmp_4 in tmp_5.Run4_()) { var res = tmp_4; 
 #line 175 "Content\Casanova semantics\transform.mc"
var result = res;
 #line 175 "Content\Casanova semantics\transform.mc"
yield return result;  } } } } }
 } 

  
 { 
 #line 180 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var domain = tmp_0.P3; var startingBlock = tmp_0.P4; var tmp_1 = tmp_0.P5 as nil; 
 #line 180 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { 
 #line 180 "Content\Casanova semantics\transform.mc"
if(M is Locals && startingBlock is ExprList) { 
 #line 180 "Content\Casanova semantics\transform.mc"
var result = ruleResult.Create(M as Locals, startingBlock as ExprList);
 #line 180 "Content\Casanova semantics\transform.mc"
yield return result;  } }
 } 

 foreach(var p in Run()) yield return p; }

public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run4_6_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_16_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_17_() { foreach(var p in Run4_()) yield return p; }

public override string ToString() {
 var res = "("; 

 res += " evalRule "; res += P1.ToString(); 
res += P2.ToString(); 
res += P3.ToString(); 
res += P4.ToString(); 
res += P5.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as evalRule;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2) && this.P3.Equals(tmp.P3) && this.P4.Equals(tmp.P4) && this.P5.Equals(tmp.P5); 
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
public IEnumerable<IRunnable> Run4_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run4_6_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_16_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_17_() { foreach(var p in Run4_()) yield return p; }

public override string ToString() {
 var res = "("; 

 res += " if "; res += P1.ToString(); 
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
 #line 92 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opDollarm; 
 #line 92 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var M = tmp_1.P1; var k = tmp_0.P2; var v = (M.GetKey(k)); 
 #line 92 "Content\Casanova semantics\transform.mc"
var result = v;
 #line 92 "Content\Casanova semantics\transform.mc"
yield return result;  }
 } 

  }

public IEnumerable<IRunnable> Run4_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run4_6_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_16_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_17_() { foreach(var p in Run4_()) yield return p; }

public override string ToString() {
 var res = "("; 
res += P1.ToString(); 

 res += " lookup "; if (P2 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P2 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P2.ToString(); } 

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

public class loopRules : RuleManager  {
public float P1;
public Locals P2;
public RuleList P3;
public RuleList P4;
public int P5;

public loopRules(float P1, Locals P2, RuleList P3, RuleList P4, int P5) {this.P1 = P1; this.P2 = P2; this.P3 = P3; this.P4 = P4; this.P5 = P5;}
public static loopRules Create(float P1, Locals P2, RuleList P3, RuleList P4, int P5) { return new loopRules(P1, P2, P3, P4, P5); }

  public IEnumerable<IRunnable> Run4_() {   
 { 
 #line 135 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var dt = tmp_0.P1; var fields = tmp_0.P2; var startingRules = tmp_0.P3; var rs = tmp_0.P4; var i = tmp_0.P5; 
 #line 135 "Content\Casanova semantics\transform.mc"
if((i>0).Equals(true)) { var outputString = ("\n----------------\n"+(fields.ToString())+"\n\n"+(rs.ToString())+"\n----------------\n"); var outputUpdate = (EntryPoint.Print(outputString)); var sleeping = (EntryPoint.Sleep(dt)); 
 #line 135 "Content\Casanova semantics\transform.mc"
if(fields is Locals && startingRules is RuleList && rs is RuleList) { 
 #line 135 "Content\Casanova semantics\transform.mc"
var tmp_2 = updateRules.Create(dt, fields as Locals, startingRules as RuleList, rs as RuleList);
 #line 135 "Content\Casanova semantics\transform.mc"
foreach (var tmp_1 in tmp_2.Run4_()) { var tmp_3 = tmp_1 as updateResult; 
 #line 135 "Content\Casanova semantics\transform.mc"
if (tmp_3 != null) { var updatedFields = tmp_3.P1; var updatedRules = tmp_3.P2; var j = (i-1); 
 #line 135 "Content\Casanova semantics\transform.mc"
if(updatedFields is Locals && startingRules is RuleList && updatedRules is RuleList) { 
 #line 135 "Content\Casanova semantics\transform.mc"
var tmp_5 = loopRules.Create(dt, updatedFields as Locals, startingRules as RuleList, updatedRules as RuleList, j);
 #line 135 "Content\Casanova semantics\transform.mc"
foreach (var tmp_4 in tmp_5.Run4_()) { var tmp_6 = tmp_4 as updateResult; 
 #line 135 "Content\Casanova semantics\transform.mc"
if (tmp_6 != null) { var fs_Prime = tmp_6.P1; var rs_Prime = tmp_6.P2; 
 #line 135 "Content\Casanova semantics\transform.mc"
if(fs_Prime is Locals && rs_Prime is RuleList) { 
 #line 135 "Content\Casanova semantics\transform.mc"
var result = updateResult.Create(fs_Prime as Locals, rs_Prime as RuleList);
 #line 135 "Content\Casanova semantics\transform.mc"
yield return result;  } } } } } } } }
 } 

  
 { 
 #line 145 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var dt = tmp_0.P1; var fields = tmp_0.P2; var startingRules = tmp_0.P3; var rs = tmp_0.P4; 
 #line 145 "Content\Casanova semantics\transform.mc"
if (tmp_0.P5 == 0) { 
 #line 145 "Content\Casanova semantics\transform.mc"
if(fields is Locals && rs is RuleList) { 
 #line 145 "Content\Casanova semantics\transform.mc"
var result = updateResult.Create(fields as Locals, rs as RuleList);
 #line 145 "Content\Casanova semantics\transform.mc"
yield return result;  } }
 } 

 foreach(var p in Run()) yield return p; }

public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run4_6_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_16_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_17_() { foreach(var p in Run4_()) yield return p; }

public override string ToString() {
 var res = "("; 

 res += " loopRules "; res += P1.ToString(); 
res += P2.ToString(); 
res += P3.ToString(); 
res += P4.ToString(); 
res += P5.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as loopRules;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2) && this.P3.Equals(tmp.P3) && this.P4.Equals(tmp.P4) && this.P5.Equals(tmp.P5); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class nil : ExprList  {

public nil() {}
public static nil Create() { return new nil(); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run4_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run4_6_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_16_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_17_() { foreach(var p in Run4_()) yield return p; }

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

public class nilDomain : Domain  {

public nilDomain() {}
public static nilDomain Create() { return new nilDomain(); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run4_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run4_6_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_16_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_17_() { foreach(var p in Run4_()) yield return p; }

public override string ToString() {
return "nilDomain";
}

public override bool Equals(object other) {
 return other is nilDomain; 
}

public override int GetHashCode() {
 return 0; 
}

}

public class nilResult : ExprResultList  {

public nilResult() {}
public static nilResult Create() { return new nilResult(); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run4_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run4_6_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_16_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_17_() { foreach(var p in Run4_()) yield return p; }

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

public class nilRule : RuleList  {

public nilRule() {}
public static nilRule Create() { return new nilRule(); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run4_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run4_6_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_16_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_17_() { foreach(var p in Run4_()) yield return p; }

public override string ToString() {
return "nilRule";
}

public override bool Equals(object other) {
 return other is nilRule; 
}

public override int GetHashCode() {
 return 0; 
}

}

public class reEvaluate : ExprResult  {
public ExprList P1;

public reEvaluate(ExprList P1) {this.P1 = P1;}
public static reEvaluate Create(ExprList P1) { return new reEvaluate(P1); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run4_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run4_6_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_16_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_17_() { foreach(var p in Run4_()) yield return p; }

public override string ToString() {
 var res = "("; 

 res += " reEvaluate "; res += P1.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as reEvaluate;
 if(tmp != null) return this.P1.Equals(tmp.P1); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class rule : Rule  {
public Domain P1;
public ExprList P2;

public rule(Domain P1, ExprList P2) {this.P1 = P1; this.P2 = P2;}
public static rule Create(Domain P1, ExprList P2) { return new rule(P1, P2); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run4_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run4_6_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_16_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_17_() { foreach(var p in Run4_()) yield return p; }

public override string ToString() {
 var res = "("; 

 res += " rule "; res += P1.ToString(); 
res += P2.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as rule;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class ruleResult : RuleResult  {
public Locals P1;
public ExprList P2;

public ruleResult(Locals P1, ExprList P2) {this.P1 = P1; this.P2 = P2;}
public static ruleResult Create(Locals P1, ExprList P2) { return new ruleResult(P1, P2); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run4_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run4_6_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_16_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_17_() { foreach(var p in Run4_()) yield return p; }

public override string ToString() {
 var res = "("; 

 res += " ruleResult "; res += P1.ToString(); 
res += P2.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as ruleResult;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class runTest1 : Test  {

public runTest1() {}
public static runTest1 Create() { return new runTest1(); }

  public IEnumerable<IRunnable> Run() {   
 { 
 #line 108 "Content\Casanova semantics\transform.mc"
var tmp_0 = this as runTest1; var dt = 1.000000f; var iterations = 10; var Me = _opDollarm.Create(System.Collections.Immutable.ImmutableDictionary<string,ExprResult>.Empty); 
 #line 108 "Content\Casanova semantics\transform.mc"
if(Me is Locals) { 
 #line 108 "Content\Casanova semantics\transform.mc"
var tmp_2 = add.Create(Me as Locals, "F1", _opDollari.Create(100));
 #line 108 "Content\Casanova semantics\transform.mc"
foreach (var tmp_1 in tmp_2.Run4_()) { var Ml = tmp_1; 
 #line 108 "Content\Casanova semantics\transform.mc"
if(Ml is Locals) { 
 #line 108 "Content\Casanova semantics\transform.mc"
var tmp_4 = add.Create(Ml as Locals, "F2", _opDollari.Create(100));
 #line 108 "Content\Casanova semantics\transform.mc"
foreach (var tmp_3 in tmp_4.Run4_()) { var M = tmp_3; var dom1 = consDomain.Create("F1", nilDomain.Create()); var dom2 = consDomain.Create("F2", nilDomain.Create()); var f1 = _Semicolon.Create(_opDollari.Create(90), _Semicolon.Create(_opDollari.Create(50), nil.Create())); var w1 = wait.Create(3.000000f); var w2 = wait.Create(2.000000f); var y1 = yield.Create(_Semicolon.Create(_opDollari.Create(10), nil.Create())); var y2 = yield.Create(_Semicolon.Create(_opDollari.Create(20), nil.Create())); 
 #line 108 "Content\Casanova semantics\transform.mc"
if (w1 is Expr && y1 is Expr) { var b1 = _Semicolon.Create(w1 as Expr, _Semicolon.Create(y1 as Expr, nil.Create())); 
 #line 108 "Content\Casanova semantics\transform.mc"
if (w2 is Expr && y2 is Expr) { var b2 = _Semicolon.Create(w2 as Expr, _Semicolon.Create(y2 as Expr, nil.Create())); 
 #line 108 "Content\Casanova semantics\transform.mc"
if (dom1 is Domain && b1 is ExprList) { var r1 = rule.Create(dom1 as Domain, b1 as ExprList); 
 #line 108 "Content\Casanova semantics\transform.mc"
if (dom2 is Domain && b2 is ExprList) { var r2 = rule.Create(dom2 as Domain, b2 as ExprList); 
 #line 108 "Content\Casanova semantics\transform.mc"
if (r1 is Rule && r2 is Rule) { var rs = consRule.Create(r1 as Rule, consRule.Create(r2 as Rule, nilRule.Create())); 
 #line 108 "Content\Casanova semantics\transform.mc"
if (M is Locals && rs is RuleList) { var e = entity.Create("E", M as Locals, rs as RuleList); 
 #line 108 "Content\Casanova semantics\transform.mc"
if(e is Entity) { 
 #line 108 "Content\Casanova semantics\transform.mc"
var tmp_6 = updateEntity.Create(dt, e as Entity, iterations);
 #line 108 "Content\Casanova semantics\transform.mc"
foreach (var tmp_5 in tmp_6.Run4_()) { var res = tmp_5; var debug = (EntryPoint.Print("Done!")); 
 #line 108 "Content\Casanova semantics\transform.mc"
var result = res;
 #line 108 "Content\Casanova semantics\transform.mc"
yield return result;  } } } } } } } } } } } }
 } 

  }

public IEnumerable<IRunnable> Run4_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run4_6_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_16_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_17_() { foreach(var p in Run4_()) yield return p; }

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
public IEnumerable<IRunnable> Run4_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run4_6_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_16_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_17_() { foreach(var p in Run4_()) yield return p; }

public override string ToString() {
 var res = "("; 

 res += " setDt "; res += P1.ToString(); 

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

  public IEnumerable<IRunnable> Run4_17_() {   
 { 
 #line 229 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var tmp_1 = tmp_0.P3 as atomic; 
 #line 229 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var b = tmp_0.P4; 
 #line 229 "Content\Casanova semantics\transform.mc"
if (b is ExprList) { var res = reEvaluate.Create(b as ExprList); 
 #line 229 "Content\Casanova semantics\transform.mc"
var result = res;
 #line 229 "Content\Casanova semantics\transform.mc"
yield return result;  } }
 } 

  
 { 
 #line 233 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var tmp_1 = tmp_0.P3 as nilResult; 
 #line 233 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var b = tmp_0.P4; 
 #line 233 "Content\Casanova semantics\transform.mc"
if (b is ExprList) { var res = reEvaluate.Create(b as ExprList); 
 #line 233 "Content\Casanova semantics\transform.mc"
var result = res;
 #line 233 "Content\Casanova semantics\transform.mc"
yield return result;  } }
 } 

  
 { 
 #line 237 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var tmp_1 = tmp_0.P3 as waitResult; 
 #line 237 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var t = tmp_1.P1; var b = tmp_0.P4; 
 #line 237 "Content\Casanova semantics\transform.mc"
if (b is ExprList) { var res = continueWith.Create(_Semicolon.Create(wait.Create(t), b as ExprList)); 
 #line 237 "Content\Casanova semantics\transform.mc"
var result = res;
 #line 237 "Content\Casanova semantics\transform.mc"
yield return result;  } }
 } 

  
 { 
 #line 241 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var tmp_1 = tmp_0.P3 as yieldResult; 
 #line 241 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var vals = tmp_1.P1; var b = tmp_0.P4; 
 #line 241 "Content\Casanova semantics\transform.mc"
if (vals is ExprResultList && b is ExprList) { var res = updateFieldsAndContinueWith.Create(vals as ExprResultList, b as ExprList); 
 #line 241 "Content\Casanova semantics\transform.mc"
var result = res;
 #line 241 "Content\Casanova semantics\transform.mc"
yield return result;  } }
 } 

 foreach(var p in Run4_()) yield return p; }

public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run4_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run4_6_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_16_() { foreach(var p in Run4_()) yield return p; }

public override string ToString() {
 var res = "("; 

 res += " stepOrSuspend "; res += P1.ToString(); 
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

public class suspend : ExprResult  {

public suspend() {}
public static suspend Create() { return new suspend(); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run4_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run4_6_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_16_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_17_() { foreach(var p in Run4_()) yield return p; }

public override string ToString() {
return "suspend";
}

public override bool Equals(object other) {
 return other is suspend; 
}

public override int GetHashCode() {
 return 0; 
}

}

public class suspendResult : ExprResult  {

public suspendResult() {}
public static suspendResult Create() { return new suspendResult(); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run4_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run4_6_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_16_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_17_() { foreach(var p in Run4_()) yield return p; }

public override string ToString() {
return "suspendResult";
}

public override bool Equals(object other) {
 return other is suspendResult; 
}

public override int GetHashCode() {
 return 0; 
}

}

public class _Then : Then  {

public _Then() {}
public static _Then Create() { return new _Then(); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run4_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run4_6_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_16_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_17_() { foreach(var p in Run4_()) yield return p; }

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

public class updateEntity : EntityManager  {
public float P1;
public Entity P2;
public int P3;

public updateEntity(float P1, Entity P2, int P3) {this.P1 = P1; this.P2 = P2; this.P3 = P3;}
public static updateEntity Create(float P1, Entity P2, int P3) { return new updateEntity(P1, P2, P3); }

  public IEnumerable<IRunnable> Run4_() {   
 { 
 #line 131 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var dt = tmp_0.P1; var tmp_1 = tmp_0.P2 as entity; 
 #line 131 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var name = tmp_1.P1; var fields = tmp_1.P2; var rs = tmp_1.P3; var updates = tmp_0.P3; 
 #line 131 "Content\Casanova semantics\transform.mc"
if(fields is Locals && rs is RuleList && rs is RuleList) { 
 #line 131 "Content\Casanova semantics\transform.mc"
var tmp_3 = loopRules.Create(dt, fields as Locals, rs as RuleList, rs as RuleList, updates);
 #line 131 "Content\Casanova semantics\transform.mc"
foreach (var tmp_2 in tmp_3.Run4_()) { var tmp_4 = tmp_2 as updateResult; 
 #line 131 "Content\Casanova semantics\transform.mc"
if (tmp_4 != null) { var fields_Prime = tmp_4.P1; var rs_Prime = tmp_4.P2; 
 #line 131 "Content\Casanova semantics\transform.mc"
if(fields_Prime is Locals && rs_Prime is RuleList) { 
 #line 131 "Content\Casanova semantics\transform.mc"
var result = entity.Create(name, fields_Prime as Locals, rs_Prime as RuleList);
 #line 131 "Content\Casanova semantics\transform.mc"
yield return result;  } } } } }
 } 

 foreach(var p in Run()) yield return p; }

public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run4_6_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_16_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_17_() { foreach(var p in Run4_()) yield return p; }

public override string ToString() {
 var res = "("; 

 res += " updateEntity "; res += P1.ToString(); 
res += P2.ToString(); 
res += P3.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as updateEntity;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2) && this.P3.Equals(tmp.P3); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class updateFields : MemoryOp  {
public Locals P1;
public Domain P2;
public ExprResultList P3;

public updateFields(Locals P1, Domain P2, ExprResultList P3) {this.P1 = P1; this.P2 = P2; this.P3 = P3;}
public static updateFields Create(Locals P1, Domain P2, ExprResultList P3) { return new updateFields(P1, P2, P3); }

  public IEnumerable<IRunnable> Run4_6_() {   
 { 
 #line 167 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var M = tmp_0.P1; var tmp_1 = tmp_0.P2 as consDomain; 
 #line 167 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var field = tmp_1.P1; var fields = tmp_1.P2; var tmp_2 = tmp_0.P3 as consResult; 
 #line 167 "Content\Casanova semantics\transform.mc"
if (tmp_2 != null) { var v = tmp_2.P1; var vals = tmp_2.P2; 
 #line 167 "Content\Casanova semantics\transform.mc"
if(M is Locals) { 
 #line 167 "Content\Casanova semantics\transform.mc"
var tmp_4 = add.Create(M as Locals, field, v);
 #line 167 "Content\Casanova semantics\transform.mc"
foreach (var tmp_3 in tmp_4.Run4_6_()) { var M_Prime = tmp_3; 
 #line 167 "Content\Casanova semantics\transform.mc"
if(M_Prime is Locals && fields is Domain && vals is ExprResultList) { 
 #line 167 "Content\Casanova semantics\transform.mc"
var tmp_6 = updateFields.Create(M_Prime as Locals, fields as Domain, vals as ExprResultList);
 #line 167 "Content\Casanova semantics\transform.mc"
foreach (var tmp_5 in tmp_6.Run4_6_()) { var M_Prime_Prime = tmp_5; 
 #line 167 "Content\Casanova semantics\transform.mc"
var result = M_Prime_Prime;
 #line 167 "Content\Casanova semantics\transform.mc"
yield return result;  } } } } } }
 } 

  
 { 
 #line 172 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var M = tmp_0.P1; var tmp_1 = tmp_0.P2 as nilDomain; 
 #line 172 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var tmp_2 = tmp_0.P3 as nilResult; 
 #line 172 "Content\Casanova semantics\transform.mc"
if (tmp_2 != null) { 
 #line 172 "Content\Casanova semantics\transform.mc"
var result = M;
 #line 172 "Content\Casanova semantics\transform.mc"
yield return result;  } }
 } 

 foreach(var p in Run4_()) yield return p; }

public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run4_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run4_16_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_17_() { foreach(var p in Run4_()) yield return p; }

public override string ToString() {
 var res = "("; 

 res += " updateFields "; res += P1.ToString(); 
res += P2.ToString(); 
res += P3.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as updateFields;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2) && this.P3.Equals(tmp.P3); 
 else return false; }

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
public IEnumerable<IRunnable> Run4_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run4_6_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_16_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_17_() { foreach(var p in Run4_()) yield return p; }

public override string ToString() {
 var res = "("; 

 res += " updateFieldsAndContinueWith "; res += P1.ToString(); 
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

public class updateResult : UpdateResult  {
public Locals P1;
public RuleList P2;

public updateResult(Locals P1, RuleList P2) {this.P1 = P1; this.P2 = P2;}
public static updateResult Create(Locals P1, RuleList P2) { return new updateResult(P1, P2); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run4_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run4_6_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_16_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_17_() { foreach(var p in Run4_()) yield return p; }

public override string ToString() {
 var res = "("; 

 res += " updateResult "; res += P1.ToString(); 
res += P2.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as updateResult;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class updateRules : RuleManager  {
public float P1;
public Locals P2;
public RuleList P3;
public RuleList P4;

public updateRules(float P1, Locals P2, RuleList P3, RuleList P4) {this.P1 = P1; this.P2 = P2; this.P3 = P3; this.P4 = P4;}
public static updateRules Create(float P1, Locals P2, RuleList P3, RuleList P4) { return new updateRules(P1, P2, P3, P4); }

  public IEnumerable<IRunnable> Run4_() {   
 { 
 #line 149 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var tmp_1 = tmp_0.P3 as consRule; 
 #line 149 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { var tmp_2 = tmp_1.P1 as rule; 
 #line 149 "Content\Casanova semantics\transform.mc"
if (tmp_2 != null) { var sd = tmp_2.P1; var sb = tmp_2.P2; var startingRules = tmp_1.P2; var tmp_3 = tmp_0.P4 as consRule; 
 #line 149 "Content\Casanova semantics\transform.mc"
if (tmp_3 != null) { var tmp_4 = tmp_3.P1 as rule; 
 #line 149 "Content\Casanova semantics\transform.mc"
if (tmp_4 != null) { var domain = tmp_4.P1; var block = tmp_4.P2; var rs = tmp_3.P2; 
 #line 149 "Content\Casanova semantics\transform.mc"
if(M is Locals && domain is Domain && sb is ExprList && block is ExprList) { 
 #line 149 "Content\Casanova semantics\transform.mc"
var tmp_6 = evalRule.Create(dt, M as Locals, domain as Domain, sb as ExprList, block as ExprList);
 #line 149 "Content\Casanova semantics\transform.mc"
foreach (var tmp_5 in tmp_6.Run4_()) { var tmp_7 = tmp_5 as ruleResult; 
 #line 149 "Content\Casanova semantics\transform.mc"
if (tmp_7 != null) { var M_Prime = tmp_7.P1; var continuation = tmp_7.P2; 
 #line 149 "Content\Casanova semantics\transform.mc"
if (domain is Domain && continuation is ExprList) { var updatedRule = rule.Create(domain as Domain, continuation as ExprList); 
 #line 149 "Content\Casanova semantics\transform.mc"
if(M_Prime is Locals && startingRules is RuleList && rs is RuleList) { 
 #line 149 "Content\Casanova semantics\transform.mc"
var tmp_9 = updateRules.Create(dt, M_Prime as Locals, startingRules as RuleList, rs as RuleList);
 #line 149 "Content\Casanova semantics\transform.mc"
foreach (var tmp_8 in tmp_9.Run4_()) { var tmp_10 = tmp_8 as updateResult; 
 #line 149 "Content\Casanova semantics\transform.mc"
if (tmp_10 != null) { var M_Prime_Prime = tmp_10.P1; var rs_Prime = tmp_10.P2; 
 #line 149 "Content\Casanova semantics\transform.mc"
if(M_Prime_Prime is Locals && updatedRule is Rule && rs_Prime is RuleList) { 
 #line 149 "Content\Casanova semantics\transform.mc"
var result = updateResult.Create(M_Prime_Prime as Locals, consRule.Create(updatedRule as Rule, rs_Prime as RuleList));
 #line 149 "Content\Casanova semantics\transform.mc"
yield return result;  } } } } } } } } } } } }
 } 

  
 { 
 #line 155 "Content\Casanova semantics\transform.mc"
var tmp_0 = this; var dt = tmp_0.P1; var M = tmp_0.P2; var rs = tmp_0.P3; var tmp_1 = tmp_0.P4 as nilRule; 
 #line 155 "Content\Casanova semantics\transform.mc"
if (tmp_1 != null) { 
 #line 155 "Content\Casanova semantics\transform.mc"
if(M is Locals) { 
 #line 155 "Content\Casanova semantics\transform.mc"
var result = updateResult.Create(M as Locals, nilRule.Create());
 #line 155 "Content\Casanova semantics\transform.mc"
yield return result;  } }
 } 

 foreach(var p in Run()) yield return p; }

public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run4_6_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_16_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_17_() { foreach(var p in Run4_()) yield return p; }

public override string ToString() {
 var res = "("; 

 res += " updateRules "; res += P1.ToString(); 
res += P2.ToString(); 
res += P3.ToString(); 
res += P4.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as updateRules;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2) && this.P3.Equals(tmp.P3) && this.P4.Equals(tmp.P4); 
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
public IEnumerable<IRunnable> Run4_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run4_6_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_16_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_17_() { foreach(var p in Run4_()) yield return p; }

public override string ToString() {
 var res = "("; 

 res += " wait "; res += P1.ToString(); 

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
public IEnumerable<IRunnable> Run4_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run4_6_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_16_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_17_() { foreach(var p in Run4_()) yield return p; }

public override string ToString() {
 var res = "("; 

 res += " waitResult "; res += P1.ToString(); 

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
public IEnumerable<IRunnable> Run4_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run4_6_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_16_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_17_() { foreach(var p in Run4_()) yield return p; }

public override string ToString() {
 var res = "("; 

 res += " yield "; res += P1.ToString(); 

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
public IEnumerable<IRunnable> Run4_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run4_6_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_16_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_17_() { foreach(var p in Run4_()) yield return p; }

public override string ToString() {
 var res = "("; 

 res += " yieldResult "; res += P1.ToString(); 

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
public IEnumerable<IRunnable> Run4_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run4_6_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_16_() { foreach(var p in Run4_()) yield return p; }
public IEnumerable<IRunnable> Run4_17_() { foreach(var p in Run4_()) yield return p; }

public override string ToString() {
 var res = "("; 
res += P1.ToString(); 

 res += " || "; res += P2.ToString(); 

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
 static public int Sleep(float s) { int t = (int)(s * 1000.0f); System.Threading.Thread.Sleep(t); return 0; } 
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
