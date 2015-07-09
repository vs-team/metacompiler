using System.Collections.Immutable;
using System;
using System;
using System.Collections.Immutable;
using System.Collections.Generic;
using System.Linq;
namespace test.mc {
 public static class Extensions { public static V GetKey<T, V>(this System.Collections.Immutable.ImmutableDictionary<T, V> self, T key) { return self[key]; } }



public interface Tuple<a, b> {}
public interface TupleOperator {}
public interface List<a> {}
public interface ListOperator {}
public interface Num : Expr {}
public interface Expr {}
public interface Debug {}



public class _opMultiplication : Expr  {
public Expr P1;
public Expr P2;

public _opMultiplication(Expr P1, Expr P2) {this.P1 = P1; this.P2 = P2;}
public static _opMultiplication Create(Expr P1, Expr P2) { return new _opMultiplication(P1, P2); }

  public static Num StaticRun(Expr P1, Expr P2) {    
 { 
 var tmp_0 = P1 as z; 
if (tmp_0 != null) { 
 var a = P2; 
var result = z.Create();
 return result;  }
 } 

  
 { 
 var tmp_0 = P1 as s; 
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = P2; 
var tmp_2 = _opMultiplication.StaticRun(a, b);

var tmp_1 = tmp_2;

var c = tmp_1; 
var tmp_4 = _opAddition.StaticRun(c, b);

var tmp_3 = tmp_4;

var d = tmp_3; 
var result = d;
 return result;  }
 } 

  
throw new System.Exception("Error evaluating: _opMultiplication. No result returned."); }
public Num Run() { return StaticRun(P1, P2); }


public override string ToString() {
 var res = "("; 
if (P1 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P1.ToString(); } 

 res += " * "; if (P2 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P2 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P2.ToString(); } 

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

  public static Num StaticRun(Expr P1, Expr P2) {    
 { 
 var tmp_0 = P1 as z; 
if (tmp_0 != null) { 
 var a = P2; 
var tmp_2 = eval.StaticRun(a);

var tmp_1 = tmp_2;

var res = tmp_1; 
var result = res;
 return result;  }
 } 

  
 { 
 var tmp_0 = P1 as s; 
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = P2; 
var tmp_2 = _opAddition.StaticRun(a, b);

var tmp_1 = tmp_2;

var c = tmp_1; 
var result = s.Create(c);
 return result;  }
 } 

  
throw new System.Exception("Error evaluating: _opAddition. No result returned."); }
public Num Run() { return StaticRun(P1, P2); }


public override string ToString() {
 var res = "("; 
if (P1 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P1.ToString(); } 

 res += " + "; if (P2 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P2 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P2.ToString(); } 

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

public class _Comma<a, b> : Tuple<a, b>  {
public a P1;
public b P2;

public _Comma(a P1, b P2) {this.P1 = P1; this.P2 = P2;}
public static _Comma<a, b> Create(a P1, b P2) { return new _Comma<a, b>(P1, P2); }

public override string ToString() {
 var res = "("; 
if (P1 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P1.ToString(); } 

 res += " , "; if (P2 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P2 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P2.ToString(); } 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _Comma<a, b>;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class _Colon_Colon<a> : List<a>  {
public a P1;
public List<a> P2;

public _Colon_Colon(a P1, List<a> P2) {this.P1 = P1; this.P2 = P2;}
public static _Colon_Colon<a> Create(a P1, List<a> P2) { return new _Colon_Colon<a>(P1, P2); }

public override string ToString() {
 var res = "("; 
if (P1 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P1.ToString(); } 

 res += " :: "; if (P2 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P2 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P2.ToString(); } 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _Colon_Colon<a>;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class append<a> : ListOperator  {
public List<a> P1;
public List<a> P2;

public append(List<a> P1, List<a> P2) {this.P1 = P1; this.P2 = P2;}
public static append<a> Create(List<a> P1, List<a> P2) { return new append<a>(P1, P2); }

  public static List<a> StaticRun(List<a> P1, List<a> P2) {    
 { 
 var tmp_0 = P1 as nil<a>; 
if (tmp_0 != null) { 
 var ys = P2; 
var result = ys;
 return result;  }
 } 

  
 { 
 var tmp_0 = P1 as _Colon_Colon<a>; 
if (tmp_0 != null) { 
 var x = tmp_0.P1; var xs = tmp_0.P2; var ys = P2; 
var tmp_2 = append<a>.StaticRun(xs, ys);

var tmp_1 = tmp_2;

var zs = tmp_1; 
var result = _Colon_Colon<a>.Create(x, zs);
 return result;  }
 } 

  
throw new System.Exception("Error evaluating: append. No result returned."); }
public List<a> Run() { return StaticRun(P1, P2); }


public override string ToString() {
 var res = "("; 
if (P1 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P1.ToString(); } 

 res += " append "; if (P2 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P2 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P2.ToString(); } 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as append<a>;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class debug : Debug  {

public debug() {}
public static debug Create() { return new debug(); }

  public static float StaticRun() {    
 { 
 var t = _Comma<float, Tuple<float,float>>.Create(1.000000f, _Comma<float, float>.Create(2.000000f, 3.000000f)); var l = _Colon_Colon<float>.Create(5.000000f, _Colon_Colon<float>.Create(7.000000f, _Colon_Colon<float>.Create(10.000000f, nil<float>.Create()))); var p = s.Create(s.Create(s.Create(s.Create(z.Create())))); 
var tmp_1 = fst<float, Tuple<float,float>>.StaticRun(t);

var tmp_0 = tmp_1;

var f = tmp_0; 
var tmp_3 = length<float>.StaticRun(l);

var tmp_2 = tmp_3;

var lg = tmp_2; 
var tmp_5 = toNum.StaticRun(p);

var tmp_4 = tmp_5;

var n = tmp_4; 
var result = (f+lg+n);
 return result; 
 } 

  
throw new System.Exception("Error evaluating: debug. No result returned."); }
public float Run() { return StaticRun(); }


public override string ToString() {
return "debug";
}

public override bool Equals(object other) {
 return other is debug; 
}

public override int GetHashCode() {
 return 0; 
}

}

public class eval : Expr  {
public Expr P1;

public eval(Expr P1) {this.P1 = P1;}
public static eval Create(Expr P1) { return new eval(P1); }

  public static Num StaticRun(Expr P1) {    
 { 
 var tmp_0 = P1 as z; 
if (tmp_0 != null) { 
 
var result = z.Create();
 return result;  }
 } 

  
 { 
 var tmp_0 = P1 as s; 
if (tmp_0 != null) { 
 var a = tmp_0.P1; 
var result = s.Create(a);
 return result;  }
 } 

  
 { 
 var tmp_0 = P1 as _opAddition; 
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; 
var tmp_2 = eval.StaticRun(a);

var tmp_1 = tmp_2;

var a_Prime = tmp_1; 
var tmp_4 = eval.StaticRun(b);

var tmp_3 = tmp_4;

var b_Prime = tmp_3; 
var tmp_6 = _opAddition.StaticRun(a_Prime, b_Prime);

var tmp_5 = tmp_6;

var c = tmp_5; 
var result = c;
 return result;  }
 } 

  
 { 
 var tmp_0 = P1 as _opMultiplication; 
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; 
var tmp_2 = eval.StaticRun(a);

var tmp_1 = tmp_2;

var a_Prime = tmp_1; 
var tmp_4 = eval.StaticRun(b);

var tmp_3 = tmp_4;

var b_Prime = tmp_3; 
var tmp_6 = _opMultiplication.StaticRun(a_Prime, b_Prime);

var tmp_5 = tmp_6;

var c = tmp_5; 
var result = c;
 return result;  }
 } 

  
throw new System.Exception("Error evaluating: eval. No result returned."); }
public Num Run() { return StaticRun(P1); }


public override string ToString() {
 var res = "("; 

 res += " eval "; if (P1 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P1.ToString(); } 

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

public class fst<a, b> : TupleOperator  {
public Tuple<a, b> P1;

public fst(Tuple<a, b> P1) {this.P1 = P1;}
public static fst<a, b> Create(Tuple<a, b> P1) { return new fst<a, b>(P1); }

  public static a StaticRun(Tuple<a, b> P1) {    
 { 
 var tmp_0 = P1 as _Comma<a, b>; 
if (tmp_0 != null) { 
 var x = tmp_0.P1; var y = tmp_0.P2; 
var result = x;
 return result;  }
 } 

  
throw new System.Exception("Error evaluating: fst. No result returned."); }
public a Run() { return StaticRun(P1); }


public override string ToString() {
 var res = "("; 

 res += " fst "; if (P1 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P1.ToString(); } 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as fst<a, b>;
 if(tmp != null) return this.P1.Equals(tmp.P1); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class length<a> : ListOperator  {
public List<a> P1;

public length(List<a> P1) {this.P1 = P1;}
public static length<a> Create(List<a> P1) { return new length<a>(P1); }

  public static int StaticRun(List<a> P1) {    
 { 
 var tmp_0 = P1 as nil<a>; 
if (tmp_0 != null) { 
 
var result = 0;
 return result;  }
 } 

  
 { 
 var tmp_0 = P1 as _Colon_Colon<a>; 
if (tmp_0 != null) { 
 var x = tmp_0.P1; var xs = tmp_0.P2; 
var tmp_2 = length<a>.StaticRun(xs);

var tmp_1 = tmp_2;

var y = tmp_1; 
var result = (1+y);
 return result;  }
 } 

  
throw new System.Exception("Error evaluating: length. No result returned."); }
public int Run() { return StaticRun(P1); }


public override string ToString() {
 var res = "("; 

 res += " length "; if (P1 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P1.ToString(); } 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as length<a>;
 if(tmp != null) return this.P1.Equals(tmp.P1); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class nil<a> : List<a>  {

public nil() {}
public static nil<a> Create() { return new nil<a>(); }

public override string ToString() {
return "nil";
}

public override bool Equals(object other) {
 return other is nil<a>; 
}

public override int GetHashCode() {
 return 0; 
}

}

public class removeOdd : ListOperator  {
public List<int> P1;

public removeOdd(List<int> P1) {this.P1 = P1;}
public static removeOdd Create(List<int> P1) { return new removeOdd(P1); }

  public static List<int> StaticRun(List<int> P1) {    
 { 
 var tmp_0 = P1 as nil<int>; 
if (tmp_0 != null) { 
 
var result = nil<int>.Create();
 return result;  }
 } 

  
 { 
 var tmp_0 = P1 as _Colon_Colon<int>; 
if (tmp_0 != null) { 
 var x = tmp_0.P1; var xs = tmp_0.P2; 
if((x%2).Equals(0)) { 
var tmp_2 = removeOdd.StaticRun(xs);

var tmp_1 = tmp_2;

var xs_Prime = tmp_1; 
var result = xs_Prime;
 return result;  } }
 } 

  
 { 
 var tmp_0 = P1 as _Colon_Colon<int>; 
if (tmp_0 != null) { 
 var x = tmp_0.P1; var xs = tmp_0.P2; 
if((x%2).Equals(1)) { 
var tmp_2 = removeOdd.StaticRun(xs);

var tmp_1 = tmp_2;

var xs_Prime = tmp_1; 
var result = _Colon_Colon<int>.Create(x, xs_Prime);
 return result;  } }
 } 

  
throw new System.Exception("Error evaluating: removeOdd. No result returned."); }
public List<int> Run() { return StaticRun(P1); }


public override string ToString() {
 var res = "("; 

 res += " removeOdd "; if (P1 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P1.ToString(); } 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as removeOdd;
 if(tmp != null) return this.P1.Equals(tmp.P1); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class run : Expr  {

public run() {}
public static run Create() { return new run(); }

  public static IEnumerable<int> StaticRun() {    
 { 
 
var tmp_1 = eval.StaticRun(_opAddition.Create(_opMultiplication.Create(s.Create(s.Create(z.Create())), _opMultiplication.Create(s.Create(s.Create(z.Create())), s.Create(s.Create(z.Create())))), s.Create(z.Create())));

var tmp_0 = tmp_1;

var n = tmp_0; 
var tmp_3 = toNum.StaticRun(n);

var tmp_2 = tmp_3;

var res = tmp_2; 
var result = res;
yield return result; 
 } 

   }
public IEnumerable<int> Run() { return StaticRun(); }


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

public class s : Num  {
public Num P1;

public s(Num P1) {this.P1 = P1;}
public static s Create(Num P1) { return new s(P1); }

public override string ToString() {
 var res = "("; 

 res += " s "; if (P1 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P1.ToString(); } 

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

public class snd<a, b> : TupleOperator  {
public Tuple<a, b> P1;

public snd(Tuple<a, b> P1) {this.P1 = P1;}
public static snd<a, b> Create(Tuple<a, b> P1) { return new snd<a, b>(P1); }

  public static b StaticRun(Tuple<a, b> P1) {    
 { 
 var tmp_0 = P1 as _Comma<a, b>; 
if (tmp_0 != null) { 
 var x = tmp_0.P1; var y = tmp_0.P2; 
var result = y;
 return result;  }
 } 

  
throw new System.Exception("Error evaluating: snd. No result returned."); }
public b Run() { return StaticRun(P1); }


public override string ToString() {
 var res = "("; 

 res += " snd "; if (P1 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P1.ToString(); } 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as snd<a, b>;
 if(tmp != null) return this.P1.Equals(tmp.P1); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class toNum : Expr  {
public Num P1;

public toNum(Num P1) {this.P1 = P1;}
public static toNum Create(Num P1) { return new toNum(P1); }

  public static int StaticRun(Num P1) {    
 { 
 var tmp_0 = P1 as s; 
if (tmp_0 != null) { 
 var a = tmp_0.P1; 
var tmp_2 = toNum.StaticRun(a);

var tmp_1 = tmp_2;

var res = tmp_1; 
var result = (res+1);
 return result;  }
 } 

  
 { 
 var tmp_0 = P1 as z; 
if (tmp_0 != null) { 
 
var result = 0;
 return result;  }
 } 

  
throw new System.Exception("Error evaluating: toNum. No result returned."); }
public int Run() { return StaticRun(P1); }


public override string ToString() {
 var res = "("; 

 res += " toNum "; if (P1 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P1.ToString(); } 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as toNum;
 if(tmp != null) return this.P1.Equals(tmp.P1); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class z : Num  {

public z() {}
public static z Create() { return new z(); }

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
 public static bool Print(string s) {System.Console.WriteLine(s); return true;}
   
static public object Run(bool printInput)
{
 #line 1 "input"
 var p = debug.Create();
if(printInput) System.Console.WriteLine(p.ToString());
 
 var result = p.Run(); 

return result;
}
}

}
