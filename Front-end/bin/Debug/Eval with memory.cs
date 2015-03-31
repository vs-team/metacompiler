using System;
using System.Collections.Immutable;
using System.Collections.Generic;
using System.Linq;
namespace Eval_with_memory {
 public static class Extensions { public static V GetKey<T, V>(this System.Collections.Immutable.ImmutableDictionary<T, V> self, T key) { return self[key]; } }
 public interface IRunnable { IEnumerable<IRunnable> Run();
 }


public interface Expr : IRunnable {}
public interface MapIntString : Expr {}
public interface Value : Expr {}
public interface Variable : Expr {}



public class _opBang : Variable  {
public string P1;

public _opBang(string P1) {this.P1 = P1;}
public static _opBang Create(string P1) { return new _opBang(P1); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }

public override string ToString() {
 var res = "("; 

 res += " ! "; if (P1 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P1.ToString(); } 

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

public class _opDollar : Value  {
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

public class _opMultiplication : Expr  {
public Expr P1;
public Expr P2;

public _opMultiplication(Expr P1, Expr P2) {this.P1 = P1; this.P2 = P2;}
public static _opMultiplication Create(Expr P1, Expr P2) { return new _opMultiplication(P1, P2); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }

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


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }

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

public class _Comma : Expr  {
public Value P1;
public MapIntString P2;

public _Comma(Value P1, MapIntString P2) {this.P1 = P1; this.P2 = P2;}
public static _Comma Create(Value P1, MapIntString P2) { return new _Comma(P1, P2); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }

public override string ToString() {
 var res = "("; 
res += P1.ToString(); 

 res += " , "; res += P2.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _Comma;
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

public class add : Expr  {
public MapIntString P1;
public string P2;
public int P3;

public add(MapIntString P1, string P2, int P3) {this.P1 = P1; this.P2 = P2; this.P3 = P3;}
public static add Create(MapIntString P1, string P2, int P3) { return new add(P1, P2, P3); }

  public IEnumerable<IRunnable> Run() {   
 { 
 #line 24 ""
var tmp_0 = this; var tmp_1 = tmp_0.P1 as map; 
 #line 24 ""
if (tmp_1 != null) { var M = tmp_1.P1; var k = tmp_0.P2; var v = tmp_0.P3; var M1 = (M.Remove(k)); var M2 = (M1.Add(k,v)); 
 #line 24 ""
var result = map.Create(M2);
 #line 24 ""
yield return result;  }
 } 

  }


public override string ToString() {
 var res = "("; 
res += P1.ToString(); 

 res += " add "; if (P2 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P2 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P2.ToString(); } 
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

public class assign : Expr  {
public Variable P1;
public Expr P2;

public assign(Variable P1, Expr P2) {this.P1 = P1; this.P2 = P2;}
public static assign Create(Variable P1, Expr P2) { return new assign(P1, P2); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }

public override string ToString() {
 var res = "("; 
res += P1.ToString(); 

 res += " assign "; res += P2.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as assign;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class eval : Expr  {
public Expr P1;
public MapIntString P2;

public eval(Expr P1, MapIntString P2) {this.P1 = P1; this.P2 = P2;}
public static eval Create(Expr P1, MapIntString P2) { return new eval(P1, P2); }

  public IEnumerable<IRunnable> Run() {   
 { 
 #line 42 ""
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opDollar; 
 #line 42 ""
if (tmp_1 != null) { var i = tmp_1.P1; var m = tmp_0.P2; 
 #line 42 ""
if(m is MapIntString) { 
 #line 42 ""
var result = _Comma.Create(_opDollar.Create(i), m as MapIntString);
 #line 42 ""
yield return result;  } }
 } 

  
 { 
 #line 45 ""
var tmp_0 = this; var tmp_1 = tmp_0.P1 as nil; 
 #line 45 ""
if (tmp_1 != null) { var m = tmp_0.P2; 
 #line 45 ""
if(m is MapIntString) { 
 #line 45 ""
var result = _Comma.Create(nil.Create(), m as MapIntString);
 #line 45 ""
yield return result;  } }
 } 

  
 { 
 #line 48 ""
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opBang; 
 #line 48 ""
if (tmp_1 != null) { var v = tmp_1.P1; var m = tmp_0.P2; 
 #line 48 ""
if(m is MapIntString) { 
 #line 48 ""
var tmp_3 = lookup.Create(m as MapIntString, v);
 #line 48 ""
foreach (var tmp_2 in tmp_3.Run()) { var res = tmp_2; 
 #line 48 ""
if(res is Value && m is MapIntString) { 
 #line 48 ""
var result = _Comma.Create(res as Value, m as MapIntString);
 #line 48 ""
yield return result;  } } } }
 } 

  
 { 
 #line 52 ""
var tmp_0 = this; var tmp_1 = tmp_0.P1 as assign; 
 #line 52 ""
if (tmp_1 != null) { var tmp_2 = tmp_1.P1 as _opBang; 
 #line 52 ""
if (tmp_2 != null) { var v = tmp_2.P1; var e = tmp_1.P2; var m = tmp_0.P2; 
 #line 52 ""
if(e is Expr && m is MapIntString) { 
 #line 52 ""
var tmp_4 = eval.Create(e as Expr, m as MapIntString);
 #line 52 ""
foreach (var tmp_3 in tmp_4.Run()) { var tmp_5 = tmp_3 as _Comma; 
 #line 52 ""
if (tmp_5 != null) { var tmp_6 = tmp_5.P1 as _opDollar; 
 #line 52 ""
if (tmp_6 != null) { var res = tmp_6.P1; var m1 = tmp_5.P2; 
 #line 52 ""
if(m1 is MapIntString) { 
 #line 52 ""
var tmp_8 = add.Create(m1 as MapIntString, v, res);
 #line 52 ""
foreach (var tmp_7 in tmp_8.Run()) { var m2 = tmp_7; 
 #line 52 ""
if(m2 is MapIntString) { 
 #line 52 ""
var result = _Comma.Create(nil.Create(), m2 as MapIntString);
 #line 52 ""
yield return result;  } } } } } } } } }
 } 

  
 { 
 #line 57 ""
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _Semicolon; 
 #line 57 ""
if (tmp_1 != null) { var a = tmp_1.P1; var b = tmp_1.P2; var m0 = tmp_0.P2; 
 #line 57 ""
if(a is Expr && m0 is MapIntString) { 
 #line 57 ""
var tmp_3 = eval.Create(a as Expr, m0 as MapIntString);
 #line 57 ""
foreach (var tmp_2 in tmp_3.Run()) { var tmp_4 = tmp_2 as _Comma; 
 #line 57 ""
if (tmp_4 != null) { var tmp_5 = tmp_4.P1 as nil; 
 #line 57 ""
if (tmp_5 != null) { var m1 = tmp_4.P2; 
 #line 57 ""
if(b is Expr && m1 is MapIntString) { 
 #line 57 ""
var tmp_7 = eval.Create(b as Expr, m1 as MapIntString);
 #line 57 ""
foreach (var tmp_6 in tmp_7.Run()) { var tmp_8 = tmp_6 as _Comma; 
 #line 57 ""
if (tmp_8 != null) { var res = tmp_8.P1; var m2 = tmp_8.P2; 
 #line 57 ""
if(res is Value && m2 is MapIntString) { 
 #line 57 ""
var result = _Comma.Create(res as Value, m2 as MapIntString);
 #line 57 ""
yield return result;  } } } } } } } } }
 } 

  
 { 
 #line 62 ""
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opAddition; 
 #line 62 ""
if (tmp_1 != null) { var a = tmp_1.P1; var b = tmp_1.P2; var m0 = tmp_0.P2; 
 #line 62 ""
if(a is Expr && m0 is MapIntString) { 
 #line 62 ""
var tmp_3 = eval.Create(a as Expr, m0 as MapIntString);
 #line 62 ""
foreach (var tmp_2 in tmp_3.Run()) { var tmp_4 = tmp_2 as _Comma; 
 #line 62 ""
if (tmp_4 != null) { var tmp_5 = tmp_4.P1 as _opDollar; 
 #line 62 ""
if (tmp_5 != null) { var x = tmp_5.P1; var m1 = tmp_4.P2; 
 #line 62 ""
if(b is Expr && m1 is MapIntString) { 
 #line 62 ""
var tmp_7 = eval.Create(b as Expr, m1 as MapIntString);
 #line 62 ""
foreach (var tmp_6 in tmp_7.Run()) { var tmp_8 = tmp_6 as _Comma; 
 #line 62 ""
if (tmp_8 != null) { var tmp_9 = tmp_8.P1 as _opDollar; 
 #line 62 ""
if (tmp_9 != null) { var y = tmp_9.P1; var m2 = tmp_8.P2; var res = (x+y); 
 #line 62 ""
if(m2 is MapIntString) { 
 #line 62 ""
var result = _Comma.Create(_opDollar.Create(res), m2 as MapIntString);
 #line 62 ""
yield return result;  } } } } } } } } } }
 } 

  
 { 
 #line 68 ""
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opMultiplication; 
 #line 68 ""
if (tmp_1 != null) { var a = tmp_1.P1; var b = tmp_1.P2; var m0 = tmp_0.P2; 
 #line 68 ""
if(a is Expr && m0 is MapIntString) { 
 #line 68 ""
var tmp_3 = eval.Create(a as Expr, m0 as MapIntString);
 #line 68 ""
foreach (var tmp_2 in tmp_3.Run()) { var tmp_4 = tmp_2 as _Comma; 
 #line 68 ""
if (tmp_4 != null) { var tmp_5 = tmp_4.P1 as _opDollar; 
 #line 68 ""
if (tmp_5 != null) { var x = tmp_5.P1; var m1 = tmp_4.P2; 
 #line 68 ""
if(b is Expr && m1 is MapIntString) { 
 #line 68 ""
var tmp_7 = eval.Create(b as Expr, m1 as MapIntString);
 #line 68 ""
foreach (var tmp_6 in tmp_7.Run()) { var tmp_8 = tmp_6 as _Comma; 
 #line 68 ""
if (tmp_8 != null) { var tmp_9 = tmp_8.P1 as _opDollar; 
 #line 68 ""
if (tmp_9 != null) { var y = tmp_9.P1; var m2 = tmp_8.P2; var res = (x*y); 
 #line 68 ""
if(m2 is MapIntString) { 
 #line 68 ""
var result = _Comma.Create(_opDollar.Create(res), m2 as MapIntString);
 #line 68 ""
yield return result;  } } } } } } } } } }
 } 

  }


public override string ToString() {
 var res = "("; 

 res += " eval "; res += P1.ToString(); 
res += P2.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as eval;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class lookup : Expr  {
public MapIntString P1;
public string P2;

public lookup(MapIntString P1, string P2) {this.P1 = P1; this.P2 = P2;}
public static lookup Create(MapIntString P1, string P2) { return new lookup(P1, P2); }

  public IEnumerable<IRunnable> Run() {   
 { 
 #line 29 ""
var tmp_0 = this; var tmp_1 = tmp_0.P1 as map; 
 #line 29 ""
if (tmp_1 != null) { var M = tmp_1.P1; var k = tmp_0.P2; var v = (M.GetKey(k)); 
 #line 29 ""
var result = _opDollar.Create(v);
 #line 29 ""
yield return result;  }
 } 

  }


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

public class map : MapIntString  {
public ImmutableDictionary<string,int> P1;

public map(ImmutableDictionary<string,int> P1) {this.P1 = P1;}
public static map Create(ImmutableDictionary<string,int> P1) { return new map(P1); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }

public override string ToString() {
 var res = "("; 

 res += " map "; if (P1 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P1.ToString(); } 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as map;
 if(tmp != null) return this.P1.Equals(tmp.P1); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class nil : Value  {

public nil() {}
public static nil Create() { return new nil(); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }

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

public class run : Expr  {
public MapIntString P1;

public run(MapIntString P1) {this.P1 = P1;}
public static run Create(MapIntString P1) { return new run(P1); }

  public IEnumerable<IRunnable> Run() {   
 { 
 #line 34 ""
var tmp_0 = this; var M = tmp_0.P1; 
 #line 34 ""
if(M is MapIntString) { 
 #line 34 ""
var tmp_2 = add.Create(M as MapIntString, "x", 10);
 #line 34 ""
foreach (var tmp_1 in tmp_2.Run()) { var M1 = tmp_1; 
 #line 34 ""
if(M1 is MapIntString) { 
 #line 34 ""
var tmp_4 = add.Create(M1 as MapIntString, "y", 20);
 #line 34 ""
foreach (var tmp_3 in tmp_4.Run()) { var M2 = tmp_3; 
 #line 34 ""
if(M2 is MapIntString) { 
 #line 34 ""
var tmp_6 = add.Create(M2 as MapIntString, "z", -30);
 #line 34 ""
foreach (var tmp_5 in tmp_6.Run()) { var M3 = tmp_5; 
 #line 34 ""
if(M3 is MapIntString) { 
 #line 34 ""
var tmp_8 = eval.Create(_Semicolon.Create(assign.Create(_opBang.Create("x"), _opAddition.Create(_opBang.Create("x"), _opMultiplication.Create(_opBang.Create("y"), _opDollar.Create(2)))), _opAddition.Create(_opBang.Create("x"), _opBang.Create("z"))), M3 as MapIntString);
 #line 34 ""
foreach (var tmp_7 in tmp_8.Run()) { var res = tmp_7; 
 #line 34 ""
var result = res;
 #line 34 ""
yield return result;  } } } } } } } }
 } 

  }


public override string ToString() {
 var res = "("; 

 res += " run "; res += P1.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as run;
 if(tmp != null) return this.P1.Equals(tmp.P1); 
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
 var p = run.Create(map.Create(ImmutableDictionary<string,int>.Empty));
if(printInput) System.Console.WriteLine(p.ToString());
foreach(var x in p.Run())
yield return x;
}
}

}
