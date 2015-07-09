using System.Collections.Immutable;
using System;
using System;
using System.Collections.Immutable;
using System.Collections.Generic;
using System.Linq;
namespace Basics.mc {
 public static class Extensions { public static V GetKey<T, V>(this System.Collections.Immutable.ImmutableDictionary<T, V> self, T key) { return self[key]; } }



public interface Tuple<a, b> {}
public interface TupleOperator {}
public interface List<a> {}
public interface ListOperator {}
public interface Value : Expr {}
public interface Expr {}
public interface ID : Expr {}
public interface ctxt {}
public interface Evaluator {}
public interface Test {}



public class _opDollar : ID  {
public string P1;

public _opDollar(string P1) {this.P1 = P1;}
public static _opDollar Create(string P1) { return new _opDollar(P1); }

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

public class _opDollarb : Value  {
public bool P1;

public _opDollarb(bool P1) {this.P1 = P1;}
public static _opDollarb Create(bool P1) { return new _opDollarb(P1); }

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

public class _opDollarf : Value  {
public float P1;

public _opDollarf(float P1) {this.P1 = P1;}
public static _opDollarf Create(float P1) { return new _opDollarf(P1); }

public override string ToString() {
 var res = "("; 

 res += " $f "; res += P1.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _opDollarf;
 if(tmp != null) return this.P1.Equals(tmp.P1); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class _opDollarfirst : Expr  {
public Expr P1;

public _opDollarfirst(Expr P1) {this.P1 = P1;}
public static _opDollarfirst Create(Expr P1) { return new _opDollarfirst(P1); }

public override string ToString() {
 var res = "("; 

 res += " $first "; if (P1 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P1.ToString(); } 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _opDollarfirst;
 if(tmp != null) return this.P1.Equals(tmp.P1); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class _opDollari : Value  {
public int P1;

public _opDollari(int P1) {this.P1 = P1;}
public static _opDollari Create(int P1) { return new _opDollari(P1); }

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

public class _opDollarl : Value  {
public List<Value> P1;

public _opDollarl(List<Value> P1) {this.P1 = P1;}
public static _opDollarl Create(List<Value> P1) { return new _opDollarl(P1); }

public override string ToString() {
 var res = "("; 

 res += " $l "; if (P1 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P1.ToString(); } 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _opDollarl;
 if(tmp != null) return this.P1.Equals(tmp.P1); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class _opDollars : Value  {
public string P1;

public _opDollars(string P1) {this.P1 = P1;}
public static _opDollars Create(string P1) { return new _opDollars(P1); }

public override string ToString() {
 var res = "("; 

 res += " $s "; if (P1 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P1.ToString(); } 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _opDollars;
 if(tmp != null) return this.P1.Equals(tmp.P1); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class _opDollarsecond : Expr  {
public Expr P1;

public _opDollarsecond(Expr P1) {this.P1 = P1;}
public static _opDollarsecond Create(Expr P1) { return new _opDollarsecond(P1); }

public override string ToString() {
 var res = "("; 

 res += " $second "; if (P1 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P1.ToString(); } 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _opDollarsecond;
 if(tmp != null) return this.P1.Equals(tmp.P1); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class _opDollart : Value  {
public Tuple<Value, Value> P1;

public _opDollart(Tuple<Value, Value> P1) {this.P1 = P1;}
public static _opDollart Create(Tuple<Value, Value> P1) { return new _opDollart(P1); }

public override string ToString() {
 var res = "("; 

 res += " $t "; if (P1 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P1.ToString(); } 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _opDollart;
 if(tmp != null) return this.P1.Equals(tmp.P1); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class _opAnd : Expr  {
public Expr P1;
public Expr P2;

public _opAnd(Expr P1, Expr P2) {this.P1 = P1; this.P2 = P2;}
public static _opAnd Create(Expr P1, Expr P2) { return new _opAnd(P1, P2); }

public override string ToString() {
 var res = "("; 
if (P1 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P1.ToString(); } 

 res += " && "; if (P2 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P2 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P2.ToString(); } 

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

public class _opMultiplication : Expr  {
public Expr P1;
public Expr P2;

public _opMultiplication(Expr P1, Expr P2) {this.P1 = P1; this.P2 = P2;}
public static _opMultiplication Create(Expr P1, Expr P2) { return new _opMultiplication(P1, P2); }

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

public class _opAddition_opAddition : Expr  {
public Expr P1;
public Expr P2;

public _opAddition_opAddition(Expr P1, Expr P2) {this.P1 = P1; this.P2 = P2;}
public static _opAddition_opAddition Create(Expr P1, Expr P2) { return new _opAddition_opAddition(P1, P2); }

public override string ToString() {
 var res = "("; 
if (P1 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P1.ToString(); } 

 res += " ++ "; if (P2 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P2 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P2.ToString(); } 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _opAddition_opAddition;
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

public class _opSubtraction : Expr  {
public Expr P1;
public Expr P2;

public _opSubtraction(Expr P1, Expr P2) {this.P1 = P1; this.P2 = P2;}
public static _opSubtraction Create(Expr P1, Expr P2) { return new _opSubtraction(P1, P2); }

public override string ToString() {
 var res = "("; 
if (P1 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P1.ToString(); } 

 res += " - "; if (P2 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P2 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P2.ToString(); } 

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

public class _opDivision : Expr  {
public Expr P1;
public Expr P2;

public _opDivision(Expr P1, Expr P2) {this.P1 = P1; this.P2 = P2;}
public static _opDivision Create(Expr P1, Expr P2) { return new _opDivision(P1, P2); }

public override string ToString() {
 var res = "("; 
if (P1 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P1.ToString(); } 

 res += " / "; if (P2 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P2 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P2.ToString(); } 

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

public class _opAt : Expr  {
public Expr P1;
public Expr P2;

public _opAt(Expr P1, Expr P2) {this.P1 = P1; this.P2 = P2;}
public static _opAt Create(Expr P1, Expr P2) { return new _opAt(P1, P2); }

public override string ToString() {
 var res = "("; 
if (P1 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P1.ToString(); } 

 res += " @ "; if (P2 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P2 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P2.ToString(); } 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _opAt;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class Context : ctxt  {
public ImmutableDictionary<string, Value> P1;
public ImmutableDictionary<string, Value> P2;
public ImmutableDictionary<string, Value> P3;

public Context(ImmutableDictionary<string, Value> P1, ImmutableDictionary<string, Value> P2, ImmutableDictionary<string, Value> P3) {this.P1 = P1; this.P2 = P2; this.P3 = P3;}
public static Context Create(ImmutableDictionary<string, Value> P1, ImmutableDictionary<string, Value> P2, ImmutableDictionary<string, Value> P3) { return new Context(P1, P2, P3); }

public override string ToString() {
 var res = "("; 

 res += " Context "; if (P1 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P1.ToString(); } 
if (P2 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P2 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P2.ToString(); } 
if (P3 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P3 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P3.ToString(); } 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as Context;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2) && this.P3.Equals(tmp.P3); 
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

public class eval : Evaluator  {
public Expr P1;
public ctxt P2;

public eval(Expr P1, ctxt P2) {this.P1 = P1; this.P2 = P2;}
public static eval Create(Expr P1, ctxt P2) { return new eval(P1, P2); }

  public static Value StaticRun(Expr P1, ctxt P2) {    
 { 
 var tmp_0 = P1 as _opDollar; 
if (tmp_0 != null) { 
 var a = tmp_0.P1; var tmp_1 = P2 as Context; 
if (tmp_1 != null) { 
 var locals = tmp_1.P1; var entity = tmp_1.P2; var world = tmp_1.P3; 
if((locals.ContainsKey(a)).Equals(true)) { var res = (locals.GetKey(a)); 
var result = res;
 return result;  } } }
 } 

  
 { 
 var tmp_0 = P1 as _opDollar; 
if (tmp_0 != null) { 
 var a = tmp_0.P1; var tmp_1 = P2 as Context; 
if (tmp_1 != null) { 
 var locals = tmp_1.P1; var entity = tmp_1.P2; var world = tmp_1.P3; var res = (entity.GetKey(a)); 
var result = res;
 return result;  } }
 } 

  
 { 
 var tmp_0 = P1 as _opDollarb; 
if (tmp_0 != null) { 
 var a = tmp_0.P1; var m = P2; 
var result = _opDollarb.Create(a);
 return result;  }
 } 

  
 { 
 var tmp_0 = P1 as _opDollari; 
if (tmp_0 != null) { 
 var a = tmp_0.P1; var m = P2; 
var result = _opDollari.Create(a);
 return result;  }
 } 

  
 { 
 var tmp_0 = P1 as _opDollars; 
if (tmp_0 != null) { 
 var s = tmp_0.P1; var m = P2; 
var result = _opDollars.Create(s);
 return result;  }
 } 

  
 { 
 var tmp_0 = P1 as _opDollarf; 
if (tmp_0 != null) { 
 var f = tmp_0.P1; var m = P2; 
var result = _opDollarf.Create(f);
 return result;  }
 } 

  
 { 
 var tmp_0 = P1 as _opDollart; 
if (tmp_0 != null) { 
 var t = tmp_0.P1; var m = P2; 
var result = _opDollart.Create(t);
 return result;  }
 } 

  
 { 
 var tmp_0 = P1 as _opAddition; 
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
var tmp_2 = eval.StaticRun(a, m);

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollari; 
if (tmp_3 != null) { 
 var c = tmp_3.P1; 
var tmp_5 = eval.StaticRun(b, m);

var tmp_4 = tmp_5;

var tmp_6 = tmp_4 as _opDollari; 
if (tmp_6 != null) { 
 var d = tmp_6.P1; var res = (c+d); 
var result = _opDollari.Create(res);
 return result;  } } }
 } 

  
 { 
 var tmp_0 = P1 as _opSubtraction; 
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
var tmp_2 = eval.StaticRun(a, m);

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollari; 
if (tmp_3 != null) { 
 var c = tmp_3.P1; 
var tmp_5 = eval.StaticRun(b, m);

var tmp_4 = tmp_5;

var tmp_6 = tmp_4 as _opDollari; 
if (tmp_6 != null) { 
 var d = tmp_6.P1; var res = (c-d); 
var result = _opDollari.Create(res);
 return result;  } } }
 } 

  
 { 
 var tmp_0 = P1 as _opMultiplication; 
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
var tmp_2 = eval.StaticRun(a, m);

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollari; 
if (tmp_3 != null) { 
 var c = tmp_3.P1; 
var tmp_5 = eval.StaticRun(b, m);

var tmp_4 = tmp_5;

var tmp_6 = tmp_4 as _opDollari; 
if (tmp_6 != null) { 
 var d = tmp_6.P1; var res = (c*d); 
var result = _opDollari.Create(res);
 return result;  } } }
 } 

  
 { 
 var tmp_0 = P1 as _opDivision; 
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
var tmp_2 = eval.StaticRun(a, m);

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollari; 
if (tmp_3 != null) { 
 var c = tmp_3.P1; 
var tmp_5 = eval.StaticRun(b, m);

var tmp_4 = tmp_5;

var tmp_6 = tmp_4 as _opDollari; 
if (tmp_6 != null) { 
 var d = tmp_6.P1; var res = (c/d); 
var result = _opDollari.Create(res);
 return result;  } } }
 } 

  
 { 
 var tmp_0 = P1 as _opAddition; 
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
var tmp_2 = eval.StaticRun(a, m);

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollarf; 
if (tmp_3 != null) { 
 var c = tmp_3.P1; 
var tmp_5 = eval.StaticRun(b, m);

var tmp_4 = tmp_5;

var tmp_6 = tmp_4 as _opDollarf; 
if (tmp_6 != null) { 
 var d = tmp_6.P1; var res = (c+d); 
var result = _opDollarf.Create(res);
 return result;  } } }
 } 

  
 { 
 var tmp_0 = P1 as _opMultiplication; 
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
var tmp_2 = eval.StaticRun(a, m);

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollarf; 
if (tmp_3 != null) { 
 var c = tmp_3.P1; 
var tmp_5 = eval.StaticRun(b, m);

var tmp_4 = tmp_5;

var tmp_6 = tmp_4 as _opDollarf; 
if (tmp_6 != null) { 
 var d = tmp_6.P1; var res = (c*d); 
var result = _opDollarf.Create(res);
 return result;  } } }
 } 

  
 { 
 var tmp_0 = P1 as _opDivision; 
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
var tmp_2 = eval.StaticRun(a, m);

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollarf; 
if (tmp_3 != null) { 
 var c = tmp_3.P1; 
var tmp_5 = eval.StaticRun(b, m);

var tmp_4 = tmp_5;

var tmp_6 = tmp_4 as _opDollarf; 
if (tmp_6 != null) { 
 var d = tmp_6.P1; var res = (c/d); 
var result = _opDollarf.Create(res);
 return result;  } } }
 } 

  
 { 
 var tmp_0 = P1 as _opSubtraction; 
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
var tmp_2 = eval.StaticRun(a, m);

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollarf; 
if (tmp_3 != null) { 
 var c = tmp_3.P1; 
var tmp_5 = eval.StaticRun(b, m);

var tmp_4 = tmp_5;

var tmp_6 = tmp_4 as _opDollarf; 
if (tmp_6 != null) { 
 var d = tmp_6.P1; var res = (c-d); 
var result = _opDollarf.Create(res);
 return result;  } } }
 } 

  
 { 
 var tmp_0 = P1 as _opOr; 
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
var tmp_2 = eval.StaticRun(a, m);

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollarb; 
if (tmp_3 != null) { 
 var c = tmp_3.P1; 
if(c.Equals(true)) { 
var result = _opDollarb.Create(c);
 return result;  } } }
 } 

  
 { 
 var tmp_0 = P1 as _opOr; 
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
var tmp_2 = eval.StaticRun(a, m);

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollarb; 
if (tmp_3 != null) { 
 var c = tmp_3.P1; 
if(c.Equals(false)) { 
var tmp_5 = eval.StaticRun(b, m);

var tmp_4 = tmp_5;

var tmp_6 = tmp_4 as _opDollarb; 
if (tmp_6 != null) { 
 var d = tmp_6.P1; 
var result = _opDollarb.Create(d);
 return result;  } } } }
 } 

  
 { 
 var tmp_0 = P1 as _opAnd; 
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
var tmp_2 = eval.StaticRun(a, m);

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollarb; 
if (tmp_3 != null) { 
 var c = tmp_3.P1; 
if(c.Equals(true)) { 
var tmp_5 = eval.StaticRun(b, m);

var tmp_4 = tmp_5;

var tmp_6 = tmp_4 as _opDollarb; 
if (tmp_6 != null) { 
 var d = tmp_6.P1; 
var result = _opDollarb.Create(d);
 return result;  } } } }
 } 

  
 { 
 var tmp_0 = P1 as _opAnd; 
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
var tmp_2 = eval.StaticRun(a, m);

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollarb; 
if (tmp_3 != null) { 
 var c = tmp_3.P1; 
if(c.Equals(false)) { 
var result = _opDollarb.Create(c);
 return result;  } } }
 } 

  
 { 
 var tmp_0 = P1 as lt; 
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
var tmp_2 = eval.StaticRun(a, m);

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollari; 
if (tmp_3 != null) { 
 var x = tmp_3.P1; 
var tmp_5 = eval.StaticRun(b, m);

var tmp_4 = tmp_5;

var tmp_6 = tmp_4 as _opDollari; 
if (tmp_6 != null) { 
 var y = tmp_6.P1; 
var result = _opDollarb.Create(x<y);
 return result;  } } }
 } 

  
 { 
 var tmp_0 = P1 as leq; 
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
var tmp_2 = eval.StaticRun(a, m);

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollari; 
if (tmp_3 != null) { 
 var x = tmp_3.P1; 
var tmp_5 = eval.StaticRun(b, m);

var tmp_4 = tmp_5;

var tmp_6 = tmp_4 as _opDollari; 
if (tmp_6 != null) { 
 var y = tmp_6.P1; 
var result = _opDollarb.Create(x<=y);
 return result;  } } }
 } 

  
 { 
 var tmp_0 = P1 as gt; 
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
var tmp_2 = eval.StaticRun(a, m);

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollari; 
if (tmp_3 != null) { 
 var x = tmp_3.P1; 
var tmp_5 = eval.StaticRun(b, m);

var tmp_4 = tmp_5;

var tmp_6 = tmp_4 as _opDollari; 
if (tmp_6 != null) { 
 var y = tmp_6.P1; 
var result = _opDollarb.Create(x>y);
 return result;  } } }
 } 

  
 { 
 var tmp_0 = P1 as geq; 
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
var tmp_2 = eval.StaticRun(a, m);

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollari; 
if (tmp_3 != null) { 
 var x = tmp_3.P1; 
var tmp_5 = eval.StaticRun(b, m);

var tmp_4 = tmp_5;

var tmp_6 = tmp_4 as _opDollari; 
if (tmp_6 != null) { 
 var y = tmp_6.P1; 
var result = _opDollarb.Create(x>=y);
 return result;  } } }
 } 

  
 { 
 var tmp_0 = P1 as lt; 
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
var tmp_2 = eval.StaticRun(a, m);

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollarf; 
if (tmp_3 != null) { 
 var x = tmp_3.P1; 
var tmp_5 = eval.StaticRun(b, m);

var tmp_4 = tmp_5;

var tmp_6 = tmp_4 as _opDollarf; 
if (tmp_6 != null) { 
 var y = tmp_6.P1; 
var result = _opDollarb.Create(x<y);
 return result;  } } }
 } 

  
 { 
 var tmp_0 = P1 as leq; 
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
var tmp_2 = eval.StaticRun(a, m);

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollarf; 
if (tmp_3 != null) { 
 var x = tmp_3.P1; 
var tmp_5 = eval.StaticRun(b, m);

var tmp_4 = tmp_5;

var tmp_6 = tmp_4 as _opDollarf; 
if (tmp_6 != null) { 
 var y = tmp_6.P1; 
var result = _opDollarb.Create(x<=y);
 return result;  } } }
 } 

  
 { 
 var tmp_0 = P1 as gt; 
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
var tmp_2 = eval.StaticRun(a, m);

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollarf; 
if (tmp_3 != null) { 
 var x = tmp_3.P1; 
var tmp_5 = eval.StaticRun(b, m);

var tmp_4 = tmp_5;

var tmp_6 = tmp_4 as _opDollarf; 
if (tmp_6 != null) { 
 var y = tmp_6.P1; 
var result = _opDollarb.Create(x>y);
 return result;  } } }
 } 

  
 { 
 var tmp_0 = P1 as geq; 
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
var tmp_2 = eval.StaticRun(a, m);

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollarf; 
if (tmp_3 != null) { 
 var x = tmp_3.P1; 
var tmp_5 = eval.StaticRun(b, m);

var tmp_4 = tmp_5;

var tmp_6 = tmp_4 as _opDollarf; 
if (tmp_6 != null) { 
 var y = tmp_6.P1; 
var result = _opDollarb.Create(x>=y);
 return result;  } } }
 } 

  
 { 
 var tmp_0 = P1 as _opDollarl; 
if (tmp_0 != null) { 
 var li = tmp_0.P1; var m = P2; 
var result = _opDollarl.Create(li);
 return result;  }
 } 

  
 { 
 var tmp_0 = P1 as _opAddition_opAddition; 
if (tmp_0 != null) { 
 var e = tmp_0.P1; var el = tmp_0.P2; var m = P2; 
var tmp_2 = eval.StaticRun(e, m);

var tmp_1 = tmp_2;

var v = tmp_1; 
var tmp_4 = eval.StaticRun(el, m);

var tmp_3 = tmp_4;

var tmp_5 = tmp_3 as _opDollarl; 
if (tmp_5 != null) { 
 var li = tmp_5.P1; 
var result = _opDollarl.Create(_Colon_Colon<Value>.Create(v, li));
 return result;  } }
 } 

  
 { 
 var tmp_0 = P1 as _opAt; 
if (tmp_0 != null) { 
 var ex = tmp_0.P1; var ey = tmp_0.P2; var m = P2; 
var tmp_2 = eval.StaticRun(ex, m);

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollarl; 
if (tmp_3 != null) { 
 var xs = tmp_3.P1; 
var tmp_5 = eval.StaticRun(ey, m);

var tmp_4 = tmp_5;

var tmp_6 = tmp_4 as _opDollarl; 
if (tmp_6 != null) { 
 var ys = tmp_6.P1; 
var tmp_8 = append<Value>.StaticRun(xs, ys);

var tmp_7 = tmp_8;

var zs = tmp_7; 
var result = _opDollarl.Create(zs);
 return result;  } } }
 } 

  
 { 
 var tmp_0 = P1 as _opDollarfirst; 
if (tmp_0 != null) { 
 var expr = tmp_0.P1; var m = P2; 
var tmp_2 = eval.StaticRun(expr, m);

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollart; 
if (tmp_3 != null) { 
 var t = tmp_3.P1; 
Console.WriteLine(t);

var tmp_5 = fst<Value, Value>.StaticRun(t);

var tmp_4 = tmp_5;

var res = tmp_4; 
var result = res;
 return result;  } }
 } 

  
 { 
 var tmp_0 = P1 as _opDollarsecond; 
if (tmp_0 != null) { 
 var expr = tmp_0.P1; var m = P2; 
var tmp_2 = eval.StaticRun(expr, m);

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollart; 
if (tmp_3 != null) { 
 var t = tmp_3.P1; 
var tmp_5 = snd<Value, Value>.StaticRun(t);

var tmp_4 = tmp_5;

var res = tmp_4; 
var result = res;
 return result;  } }
 } 

  
throw new System.Exception("Error evaluating: eval. No result returned."); }
public Value Run() { return StaticRun(P1, P2); }


public override string ToString() {
 var res = "("; 

 res += " eval "; if (P1 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P1.ToString(); } 
if (P2 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P2 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P2.ToString(); } 

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

public class fst<a, b> : TupleOperator  {
public Tuple<a, b> P1;

public fst(Tuple<a, b> P1) {this.P1 = P1;}
public static fst<a, b> Create(Tuple<a, b> P1) { return new fst<a, b>(P1); }


public static a StaticRun(Tuple<a, b> P1) { 
throw new System.Exception("Error evaluating: fst no result returned."); }
public a Run(){ return StaticRun(P1); }

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

public class geq : Expr  {
public Expr P1;
public Expr P2;

public geq(Expr P1, Expr P2) {this.P1 = P1; this.P2 = P2;}
public static geq Create(Expr P1, Expr P2) { return new geq(P1, P2); }

public override string ToString() {
 var res = "("; 
if (P1 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P1.ToString(); } 

 res += " geq "; if (P2 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P2 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P2.ToString(); } 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as geq;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class gt : Expr  {
public Expr P1;
public Expr P2;

public gt(Expr P1, Expr P2) {this.P1 = P1; this.P2 = P2;}
public static gt Create(Expr P1, Expr P2) { return new gt(P1, P2); }

public override string ToString() {
 var res = "("; 
if (P1 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P1.ToString(); } 

 res += " gt "; if (P2 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P2 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P2.ToString(); } 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as gt;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
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

public class leq : Expr  {
public Expr P1;
public Expr P2;

public leq(Expr P1, Expr P2) {this.P1 = P1; this.P2 = P2;}
public static leq Create(Expr P1, Expr P2) { return new leq(P1, P2); }

public override string ToString() {
 var res = "("; 
if (P1 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P1.ToString(); } 

 res += " leq "; if (P2 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P2 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P2.ToString(); } 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as leq;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class lt : Expr  {
public Expr P1;
public Expr P2;

public lt(Expr P1, Expr P2) {this.P1 = P1; this.P2 = P2;}
public static lt Create(Expr P1, Expr P2) { return new lt(P1, P2); }

public override string ToString() {
 var res = "("; 
if (P1 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P1.ToString(); } 

 res += " lt "; if (P2 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P2 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P2.ToString(); } 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as lt;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
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

public class snd<a, b> : TupleOperator  {
public Tuple<a, b> P1;

public snd(Tuple<a, b> P1) {this.P1 = P1;}
public static snd<a, b> Create(Tuple<a, b> P1) { return new snd<a, b>(P1); }


public static b StaticRun(Tuple<a, b> P1) { 
throw new System.Exception("Error evaluating: snd no result returned."); }
public b Run(){ return StaticRun(P1); }

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

public class test : Test  {

public test() {}
public static test Create() { return new test(); }

  public static Value StaticRun() {    
 { 
 var t = _opDollart.Create(_Comma<Value, Value>.Create(_opDollarf.Create(1.000000f), _opDollart.Create(_Comma<Value, Value>.Create(_opDollarf.Create(2.000000f), _opDollarf.Create(3.000000f))))); var m = Context.Create(ImmutableDictionary<string,Value>.Empty, ImmutableDictionary<string,Value>.Empty, ImmutableDictionary<string,Value>.Empty); 
var tmp_1 = eval.StaticRun(_opDollarfirst.Create(t), m);

var tmp_0 = tmp_1;

var res = tmp_0; 
var result = res;
 return result; 
 } 

  
throw new System.Exception("Error evaluating: test. No result returned."); }
public Value Run() { return StaticRun(); }


public override string ToString() {
return "test";
}

public override bool Equals(object other) {
 return other is test; 
}

public override int GetHashCode() {
 return 0; 
}

}

public class _opOr : Expr  {
public Expr P1;
public Expr P2;

public _opOr(Expr P1, Expr P2) {this.P1 = P1; this.P2 = P2;}
public static _opOr Create(Expr P1, Expr P2) { return new _opOr(P1, P2); }

public override string ToString() {
 var res = "("; 
if (P1 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P1.ToString(); } 

 res += " || "; if (P2 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P2 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P2.ToString(); } 

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
 public static bool Print(string s) {System.Console.WriteLine(s); return true;}
   
static public object Run(bool printInput)
{
 #line 1 "input"
 var p = test.Create();
if(printInput) System.Console.WriteLine(p.ToString());
 
 var result = p.Run(); 

return result;
}
}

}
