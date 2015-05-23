using System;
using System.Collections.Immutable;
using System.Collections.Generic;
using System.Linq;
namespace Eval_with_readonly_memory {
 public static class Extensions { public static V GetKey<T, V>(this System.Collections.Immutable.ImmutableDictionary<T, V> self, T key) { return self[key]; } }



public interface MapIntString : Expr {}
public interface Expr {}
public interface Value : Expr {}
public interface Variable : Expr {}



public class _opBang : Variable  {
public string P1;

public _opBang(string P1) {this.P1 = P1;}
public static _opBang Create(string P1) { return new _opBang(P1); }

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

public class add : Expr  {
public MapIntString P1;
public string P2;
public int P3;

public add(MapIntString P1, string P2, int P3) {this.P1 = P1; this.P2 = P2; this.P3 = P3;}
public static add Create(MapIntString P1, string P2, int P3) { return new add(P1, P2, P3); }

  public static MapIntString StaticRun(MapIntString P1, string P2, int P3) { System.Console.WriteLine("<li>"+new add(P1, P2, P3).ToString()+"</li>");   
 { 
 #line 20 "Content\Eval with readonly memory"
var tmp_0 = P1 as map; 
 #line 20 "Content\Eval with readonly memory"
if (tmp_0 != null) { 
 var M = tmp_0.P1; var k = P2; var v = P3; var M1 = (M.Remove(k)); var M2 = (M1.Add(k,v)); 
 #line 20 "Content\Eval with readonly memory"
var result = map.Create(M2);
 #line 20 "Content\Eval with readonly memory"
 return result;  }
 } 


 throw new System.Exception("Error evaluating: " + new add(P1, P2, P3).ToString() + " no result returned.");  }
public MapIntString Run() { return StaticRun(P1, P2, P3); }


public override string ToString() {
 var res = "("; 

 res += " add "; res += P1.ToString(); 
if (P2 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P2 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P2.ToString(); } 
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

public class eval : Expr  {
public Expr P1;
public MapIntString P2;

public eval(Expr P1, MapIntString P2) {this.P1 = P1; this.P2 = P2;}
public static eval Create(Expr P1, MapIntString P2) { return new eval(P1, P2); }

  public static Value StaticRun(Expr P1, MapIntString P2) { System.Console.WriteLine("<li>"+new eval(P1, P2).ToString()+"</li>");   
 { 
 #line 38 "Content\Eval with readonly memory"
var tmp_0 = P1 as _opDollar; 
 #line 38 "Content\Eval with readonly memory"
if (tmp_0 != null) { 
 var i = tmp_0.P1; var m = P2; 
 #line 38 "Content\Eval with readonly memory"
var result = _opDollar.Create(i);
 #line 38 "Content\Eval with readonly memory"
 return result;  }
 } 

  
 { 
 #line 41 "Content\Eval with readonly memory"
var tmp_0 = P1 as _opBang; 
 #line 41 "Content\Eval with readonly memory"
if (tmp_0 != null) { 
 var v = tmp_0.P1; var m = P2; 
 #line 41 "Content\Eval with readonly memory"
var tmp_2 = lookup.Create(m, v);
 #line 41 "Content\Eval with readonly memory"

var tmp_1 = tmp_2.Run();

var res = tmp_1; 
 #line 41 "Content\Eval with readonly memory"
var result = res;
 #line 41 "Content\Eval with readonly memory"
 return result;  }
 } 

  
 { 
 #line 45 "Content\Eval with readonly memory"
var tmp_0 = P1 as _opAddition; 
 #line 45 "Content\Eval with readonly memory"
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
 #line 45 "Content\Eval with readonly memory"
var tmp_2 = eval.Create(a, m);
 #line 45 "Content\Eval with readonly memory"

var tmp_1 = tmp_2.Run();

var tmp_3 = tmp_1 as _opDollar; 
 #line 45 "Content\Eval with readonly memory"
if (tmp_3 != null) { 
 var x = tmp_3.P1; 
 #line 45 "Content\Eval with readonly memory"
var tmp_5 = eval.Create(b, m);
 #line 45 "Content\Eval with readonly memory"

var tmp_4 = tmp_5.Run();

var tmp_6 = tmp_4 as _opDollar; 
 #line 45 "Content\Eval with readonly memory"
if (tmp_6 != null) { 
 var y = tmp_6.P1; var res = (x+y); 
 #line 45 "Content\Eval with readonly memory"
var result = _opDollar.Create(res);
 #line 45 "Content\Eval with readonly memory"
 return result;  } } }
 } 

  
 { 
 #line 51 "Content\Eval with readonly memory"
var tmp_0 = P1 as _opMultiplication; 
 #line 51 "Content\Eval with readonly memory"
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
 #line 51 "Content\Eval with readonly memory"
var tmp_2 = eval.Create(a, m);
 #line 51 "Content\Eval with readonly memory"

var tmp_1 = tmp_2.Run();

var tmp_3 = tmp_1 as _opDollar; 
 #line 51 "Content\Eval with readonly memory"
if (tmp_3 != null) { 
 var x = tmp_3.P1; 
 #line 51 "Content\Eval with readonly memory"
var tmp_5 = eval.Create(b, m);
 #line 51 "Content\Eval with readonly memory"

var tmp_4 = tmp_5.Run();

var tmp_6 = tmp_4 as _opDollar; 
 #line 51 "Content\Eval with readonly memory"
if (tmp_6 != null) { 
 var y = tmp_6.P1; var res = (x*y); 
 #line 51 "Content\Eval with readonly memory"
var result = _opDollar.Create(res);
 #line 51 "Content\Eval with readonly memory"
 return result;  } } }
 } 


 throw new System.Exception("Error evaluating: " + new eval(P1, P2).ToString() + " no result returned.");  }
public Value Run() { return StaticRun(P1, P2); }


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

  public static Value StaticRun(MapIntString P1, string P2) { System.Console.WriteLine("<li>"+new lookup(P1, P2).ToString()+"</li>");   
 { 
 #line 25 "Content\Eval with readonly memory"
var tmp_0 = P1 as map; 
 #line 25 "Content\Eval with readonly memory"
if (tmp_0 != null) { 
 var M = tmp_0.P1; var k = P2; var v = (M.GetKey(k)); 
 #line 25 "Content\Eval with readonly memory"
var result = _opDollar.Create(v);
 #line 25 "Content\Eval with readonly memory"
 return result;  }
 } 


 throw new System.Exception("Error evaluating: " + new lookup(P1, P2).ToString() + " no result returned.");  }
public Value Run() { return StaticRun(P1, P2); }


public override string ToString() {
 var res = "("; 

 res += " lookup "; res += P1.ToString(); 
if (P2 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P2 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P2.ToString(); } 

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

public class run : Expr  {
public MapIntString P1;

public run(MapIntString P1) {this.P1 = P1;}
public static run Create(MapIntString P1) { return new run(P1); }

  public static Value StaticRun(MapIntString P1) { System.Console.WriteLine("<li>"+new run(P1).ToString()+"</li>");   
 { 
 #line 30 "Content\Eval with readonly memory"
var M = P1; 
 #line 30 "Content\Eval with readonly memory"
var tmp_1 = add.Create(M, "x", 10);
 #line 30 "Content\Eval with readonly memory"

var tmp_0 = tmp_1.Run();

var M1 = tmp_0; 
 #line 30 "Content\Eval with readonly memory"
var tmp_3 = add.Create(M1, "y", 20);
 #line 30 "Content\Eval with readonly memory"

var tmp_2 = tmp_3.Run();

var M2 = tmp_2; 
 #line 30 "Content\Eval with readonly memory"
var tmp_5 = add.Create(M2, "z", -30);
 #line 30 "Content\Eval with readonly memory"

var tmp_4 = tmp_5.Run();

var M3 = tmp_4; 
 #line 30 "Content\Eval with readonly memory"
var tmp_7 = eval.Create(_opAddition.Create(_opBang.Create("x"), _opMultiplication.Create(_opBang.Create("y"), _opDollar.Create(2))), M3);
 #line 30 "Content\Eval with readonly memory"

var tmp_6 = tmp_7.Run();

var res = tmp_6; 
 #line 30 "Content\Eval with readonly memory"
var result = res;
 #line 30 "Content\Eval with readonly memory"
 return result; 
 } 


 throw new System.Exception("Error evaluating: " + new run(P1).ToString() + " no result returned.");  }
public Value Run() { return StaticRun(P1); }


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
static public object Run(bool printInput)
{
 #line 1 "input"
 var p = run.Create(map.Create(ImmutableDictionary<string,int>.Empty));
if(printInput) System.Console.WriteLine(p.ToString());
 
 var result = p.Run(); 

return result;
}
}

}
