using System.Collections.Immutable;
using System;
using System;
using Vectors;
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

public class _opDollarVector3 : Value  {
public Vector3 P1;

public _opDollarVector3(Vector3 P1) {this.P1 = P1;}
public static _opDollarVector3 Create(Vector3 P1) { return new _opDollarVector3(P1); }

public override string ToString() {
 var res = "("; 

 res += " $Vector3 "; res += P1.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _opDollarVector3;
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
 #line 18 "Content/GenericLists/transform.mc"
var tmp_0 = P1 as nil<a>; 
 #line 18 "Content/GenericLists/transform.mc"
if (tmp_0 != null) { 
 var ys = P2; 
 #line 18 "Content/GenericLists/transform.mc"
var result = ys;
 #line 18 "Content/GenericLists/transform.mc"
 return result;  }
 } 

  
 { 
 #line 21 "Content/GenericLists/transform.mc"
var tmp_0 = P1 as _Colon_Colon<a>; 
 #line 21 "Content/GenericLists/transform.mc"
if (tmp_0 != null) { 
 var x = tmp_0.P1; var xs = tmp_0.P2; var ys = P2; 
 #line 21 "Content/GenericLists/transform.mc"
var tmp_2 = append<a>.StaticRun(xs, ys);
 #line 21 "Content/GenericLists/transform.mc"

var tmp_1 = tmp_2;

var zs = tmp_1; 
 #line 21 "Content/GenericLists/transform.mc"
var result = _Colon_Colon<a>.Create(x, zs);
 #line 21 "Content/GenericLists/transform.mc"
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
 #line 48 "Content\CNV3/Basics.mc"
var tmp_0 = P1 as _opDollar; 
 #line 48 "Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var a = tmp_0.P1; var tmp_1 = P2 as Context; 
 #line 48 "Content\CNV3/Basics.mc"
if (tmp_1 != null) { 
 var locals = tmp_1.P1; var entity = tmp_1.P2; var world = tmp_1.P3; 
 #line 48 "Content\CNV3/Basics.mc"
if((locals.ContainsKey(a)).Equals(true)) { var res = (locals.GetKey(a)); 
 #line 48 "Content\CNV3/Basics.mc"
var result = res;
 #line 48 "Content\CNV3/Basics.mc"
 return result;  } } }
 } 

  
 { 
 #line 53 "Content\CNV3/Basics.mc"
var tmp_0 = P1 as _opDollar; 
 #line 53 "Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var a = tmp_0.P1; var tmp_1 = P2 as Context; 
 #line 53 "Content\CNV3/Basics.mc"
if (tmp_1 != null) { 
 var locals = tmp_1.P1; var entity = tmp_1.P2; var world = tmp_1.P3; var res = (entity.GetKey(a)); 
 #line 53 "Content\CNV3/Basics.mc"
var result = res;
 #line 53 "Content\CNV3/Basics.mc"
 return result;  } }
 } 

  
 { 
 #line 57 "Content\CNV3/Basics.mc"
var tmp_0 = P1 as _opDollarb; 
 #line 57 "Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var a = tmp_0.P1; var m = P2; 
 #line 57 "Content\CNV3/Basics.mc"
var result = _opDollarb.Create(a);
 #line 57 "Content\CNV3/Basics.mc"
 return result;  }
 } 

  
 { 
 #line 60 "Content\CNV3/Basics.mc"
var tmp_0 = P1 as _opDollari; 
 #line 60 "Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var a = tmp_0.P1; var m = P2; 
 #line 60 "Content\CNV3/Basics.mc"
var result = _opDollari.Create(a);
 #line 60 "Content\CNV3/Basics.mc"
 return result;  }
 } 

  
 { 
 #line 63 "Content\CNV3/Basics.mc"
var tmp_0 = P1 as _opDollars; 
 #line 63 "Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var s = tmp_0.P1; var m = P2; 
 #line 63 "Content\CNV3/Basics.mc"
var result = _opDollars.Create(s);
 #line 63 "Content\CNV3/Basics.mc"
 return result;  }
 } 

  
 { 
 #line 66 "Content\CNV3/Basics.mc"
var tmp_0 = P1 as _opDollarf; 
 #line 66 "Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var f = tmp_0.P1; var m = P2; 
 #line 66 "Content\CNV3/Basics.mc"
var result = _opDollarf.Create(f);
 #line 66 "Content\CNV3/Basics.mc"
 return result;  }
 } 

  
 { 
 #line 69 "Content\CNV3/Basics.mc"
var tmp_0 = P1 as _opDollart; 
 #line 69 "Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var t = tmp_0.P1; var m = P2; 
 #line 69 "Content\CNV3/Basics.mc"
var result = _opDollart.Create(t);
 #line 69 "Content\CNV3/Basics.mc"
 return result;  }
 } 

  
 { 
 #line 72 "Content\CNV3/Basics.mc"
var tmp_0 = P1 as _opDollarVector3; 
 #line 72 "Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var v = tmp_0.P1; var m = P2; 
 #line 72 "Content\CNV3/Basics.mc"
var result = _opDollarVector3.Create(v);
 #line 72 "Content\CNV3/Basics.mc"
 return result;  }
 } 

  
 { 
 #line 76 "Content\CNV3/Basics.mc"
var tmp_0 = P1 as _opAddition; 
 #line 76 "Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
 #line 76 "Content\CNV3/Basics.mc"
var tmp_2 = eval.StaticRun(a, m);
 #line 76 "Content\CNV3/Basics.mc"

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollarVector3; 
 #line 76 "Content\CNV3/Basics.mc"
if (tmp_3 != null) { 
 var v1 = tmp_3.P1; 
 #line 76 "Content\CNV3/Basics.mc"
var tmp_5 = eval.StaticRun(b, m);
 #line 76 "Content\CNV3/Basics.mc"

var tmp_4 = tmp_5;

var tmp_6 = tmp_4 as _opDollarVector3; 
 #line 76 "Content\CNV3/Basics.mc"
if (tmp_6 != null) { 
 var v2 = tmp_6.P1; var res = (v1+v2); 
 #line 76 "Content\CNV3/Basics.mc"
var result = _opDollarVector3.Create(res);
 #line 76 "Content\CNV3/Basics.mc"
 return result;  } } }
 } 

  
 { 
 #line 82 "Content\CNV3/Basics.mc"
var tmp_0 = P1 as _opSubtraction; 
 #line 82 "Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
 #line 82 "Content\CNV3/Basics.mc"
var tmp_2 = eval.StaticRun(a, m);
 #line 82 "Content\CNV3/Basics.mc"

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollarVector3; 
 #line 82 "Content\CNV3/Basics.mc"
if (tmp_3 != null) { 
 var v1 = tmp_3.P1; 
 #line 82 "Content\CNV3/Basics.mc"
var tmp_5 = eval.StaticRun(b, m);
 #line 82 "Content\CNV3/Basics.mc"

var tmp_4 = tmp_5;

var tmp_6 = tmp_4 as _opDollarVector3; 
 #line 82 "Content\CNV3/Basics.mc"
if (tmp_6 != null) { 
 var v2 = tmp_6.P1; var res = (v1-v2); 
 #line 82 "Content\CNV3/Basics.mc"
var result = _opDollarVector3.Create(res);
 #line 82 "Content\CNV3/Basics.mc"
 return result;  } } }
 } 

  
 { 
 #line 89 "Content\CNV3/Basics.mc"
var tmp_0 = P1 as _opAddition; 
 #line 89 "Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
 #line 89 "Content\CNV3/Basics.mc"
var tmp_2 = eval.StaticRun(a, m);
 #line 89 "Content\CNV3/Basics.mc"

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollari; 
 #line 89 "Content\CNV3/Basics.mc"
if (tmp_3 != null) { 
 var c = tmp_3.P1; 
 #line 89 "Content\CNV3/Basics.mc"
var tmp_5 = eval.StaticRun(b, m);
 #line 89 "Content\CNV3/Basics.mc"

var tmp_4 = tmp_5;

var tmp_6 = tmp_4 as _opDollari; 
 #line 89 "Content\CNV3/Basics.mc"
if (tmp_6 != null) { 
 var d = tmp_6.P1; var res = (c+d); 
 #line 89 "Content\CNV3/Basics.mc"
var result = _opDollari.Create(res);
 #line 89 "Content\CNV3/Basics.mc"
 return result;  } } }
 } 

  
 { 
 #line 95 "Content\CNV3/Basics.mc"
var tmp_0 = P1 as _opSubtraction; 
 #line 95 "Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
 #line 95 "Content\CNV3/Basics.mc"
var tmp_2 = eval.StaticRun(a, m);
 #line 95 "Content\CNV3/Basics.mc"

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollari; 
 #line 95 "Content\CNV3/Basics.mc"
if (tmp_3 != null) { 
 var c = tmp_3.P1; 
 #line 95 "Content\CNV3/Basics.mc"
var tmp_5 = eval.StaticRun(b, m);
 #line 95 "Content\CNV3/Basics.mc"

var tmp_4 = tmp_5;

var tmp_6 = tmp_4 as _opDollari; 
 #line 95 "Content\CNV3/Basics.mc"
if (tmp_6 != null) { 
 var d = tmp_6.P1; var res = (c-d); 
 #line 95 "Content\CNV3/Basics.mc"
var result = _opDollari.Create(res);
 #line 95 "Content\CNV3/Basics.mc"
 return result;  } } }
 } 

  
 { 
 #line 102 "Content\CNV3/Basics.mc"
var tmp_0 = P1 as _opMultiplication; 
 #line 102 "Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
 #line 102 "Content\CNV3/Basics.mc"
var tmp_2 = eval.StaticRun(a, m);
 #line 102 "Content\CNV3/Basics.mc"

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollari; 
 #line 102 "Content\CNV3/Basics.mc"
if (tmp_3 != null) { 
 var c = tmp_3.P1; 
 #line 102 "Content\CNV3/Basics.mc"
var tmp_5 = eval.StaticRun(b, m);
 #line 102 "Content\CNV3/Basics.mc"

var tmp_4 = tmp_5;

var tmp_6 = tmp_4 as _opDollari; 
 #line 102 "Content\CNV3/Basics.mc"
if (tmp_6 != null) { 
 var d = tmp_6.P1; var res = (c*d); 
 #line 102 "Content\CNV3/Basics.mc"
var result = _opDollari.Create(res);
 #line 102 "Content\CNV3/Basics.mc"
 return result;  } } }
 } 

  
 { 
 #line 108 "Content\CNV3/Basics.mc"
var tmp_0 = P1 as _opDivision; 
 #line 108 "Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
 #line 108 "Content\CNV3/Basics.mc"
var tmp_2 = eval.StaticRun(a, m);
 #line 108 "Content\CNV3/Basics.mc"

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollari; 
 #line 108 "Content\CNV3/Basics.mc"
if (tmp_3 != null) { 
 var c = tmp_3.P1; 
 #line 108 "Content\CNV3/Basics.mc"
var tmp_5 = eval.StaticRun(b, m);
 #line 108 "Content\CNV3/Basics.mc"

var tmp_4 = tmp_5;

var tmp_6 = tmp_4 as _opDollari; 
 #line 108 "Content\CNV3/Basics.mc"
if (tmp_6 != null) { 
 var d = tmp_6.P1; var res = (c/d); 
 #line 108 "Content\CNV3/Basics.mc"
var result = _opDollari.Create(res);
 #line 108 "Content\CNV3/Basics.mc"
 return result;  } } }
 } 

  
 { 
 #line 114 "Content\CNV3/Basics.mc"
var tmp_0 = P1 as _opAddition; 
 #line 114 "Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
 #line 114 "Content\CNV3/Basics.mc"
var tmp_2 = eval.StaticRun(a, m);
 #line 114 "Content\CNV3/Basics.mc"

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollarf; 
 #line 114 "Content\CNV3/Basics.mc"
if (tmp_3 != null) { 
 var c = tmp_3.P1; 
 #line 114 "Content\CNV3/Basics.mc"
var tmp_5 = eval.StaticRun(b, m);
 #line 114 "Content\CNV3/Basics.mc"

var tmp_4 = tmp_5;

var tmp_6 = tmp_4 as _opDollarf; 
 #line 114 "Content\CNV3/Basics.mc"
if (tmp_6 != null) { 
 var d = tmp_6.P1; var res = (c+d); 
 #line 114 "Content\CNV3/Basics.mc"
var result = _opDollarf.Create(res);
 #line 114 "Content\CNV3/Basics.mc"
 return result;  } } }
 } 

  
 { 
 #line 120 "Content\CNV3/Basics.mc"
var tmp_0 = P1 as _opMultiplication; 
 #line 120 "Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
 #line 120 "Content\CNV3/Basics.mc"
var tmp_2 = eval.StaticRun(a, m);
 #line 120 "Content\CNV3/Basics.mc"

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollarf; 
 #line 120 "Content\CNV3/Basics.mc"
if (tmp_3 != null) { 
 var c = tmp_3.P1; 
 #line 120 "Content\CNV3/Basics.mc"
var tmp_5 = eval.StaticRun(b, m);
 #line 120 "Content\CNV3/Basics.mc"

var tmp_4 = tmp_5;

var tmp_6 = tmp_4 as _opDollarf; 
 #line 120 "Content\CNV3/Basics.mc"
if (tmp_6 != null) { 
 var d = tmp_6.P1; var res = (c*d); 
 #line 120 "Content\CNV3/Basics.mc"
var result = _opDollarf.Create(res);
 #line 120 "Content\CNV3/Basics.mc"
 return result;  } } }
 } 

  
 { 
 #line 126 "Content\CNV3/Basics.mc"
var tmp_0 = P1 as _opDivision; 
 #line 126 "Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
 #line 126 "Content\CNV3/Basics.mc"
var tmp_2 = eval.StaticRun(a, m);
 #line 126 "Content\CNV3/Basics.mc"

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollarf; 
 #line 126 "Content\CNV3/Basics.mc"
if (tmp_3 != null) { 
 var c = tmp_3.P1; 
 #line 126 "Content\CNV3/Basics.mc"
var tmp_5 = eval.StaticRun(b, m);
 #line 126 "Content\CNV3/Basics.mc"

var tmp_4 = tmp_5;

var tmp_6 = tmp_4 as _opDollarf; 
 #line 126 "Content\CNV3/Basics.mc"
if (tmp_6 != null) { 
 var d = tmp_6.P1; var res = (c/d); 
 #line 126 "Content\CNV3/Basics.mc"
var result = _opDollarf.Create(res);
 #line 126 "Content\CNV3/Basics.mc"
 return result;  } } }
 } 

  
 { 
 #line 132 "Content\CNV3/Basics.mc"
var tmp_0 = P1 as _opSubtraction; 
 #line 132 "Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
 #line 132 "Content\CNV3/Basics.mc"
var tmp_2 = eval.StaticRun(a, m);
 #line 132 "Content\CNV3/Basics.mc"

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollarf; 
 #line 132 "Content\CNV3/Basics.mc"
if (tmp_3 != null) { 
 var c = tmp_3.P1; 
 #line 132 "Content\CNV3/Basics.mc"
var tmp_5 = eval.StaticRun(b, m);
 #line 132 "Content\CNV3/Basics.mc"

var tmp_4 = tmp_5;

var tmp_6 = tmp_4 as _opDollarf; 
 #line 132 "Content\CNV3/Basics.mc"
if (tmp_6 != null) { 
 var d = tmp_6.P1; var res = (c-d); 
 #line 132 "Content\CNV3/Basics.mc"
var result = _opDollarf.Create(res);
 #line 132 "Content\CNV3/Basics.mc"
 return result;  } } }
 } 

  
 { 
 #line 138 "Content\CNV3/Basics.mc"
var tmp_0 = P1 as _opOr; 
 #line 138 "Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
 #line 138 "Content\CNV3/Basics.mc"
var tmp_2 = eval.StaticRun(a, m);
 #line 138 "Content\CNV3/Basics.mc"

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollarb; 
 #line 138 "Content\CNV3/Basics.mc"
if (tmp_3 != null) { 
 var c = tmp_3.P1; 
 #line 138 "Content\CNV3/Basics.mc"
if(c.Equals(true)) { 
 #line 138 "Content\CNV3/Basics.mc"
var result = _opDollarb.Create(c);
 #line 138 "Content\CNV3/Basics.mc"
 return result;  } } }
 } 

  
 { 
 #line 143 "Content\CNV3/Basics.mc"
var tmp_0 = P1 as _opOr; 
 #line 143 "Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
 #line 143 "Content\CNV3/Basics.mc"
var tmp_2 = eval.StaticRun(a, m);
 #line 143 "Content\CNV3/Basics.mc"

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollarb; 
 #line 143 "Content\CNV3/Basics.mc"
if (tmp_3 != null) { 
 var c = tmp_3.P1; 
 #line 143 "Content\CNV3/Basics.mc"
if(c.Equals(false)) { 
 #line 143 "Content\CNV3/Basics.mc"
var tmp_5 = eval.StaticRun(b, m);
 #line 143 "Content\CNV3/Basics.mc"

var tmp_4 = tmp_5;

var tmp_6 = tmp_4 as _opDollarb; 
 #line 143 "Content\CNV3/Basics.mc"
if (tmp_6 != null) { 
 var d = tmp_6.P1; 
 #line 143 "Content\CNV3/Basics.mc"
var result = _opDollarb.Create(d);
 #line 143 "Content\CNV3/Basics.mc"
 return result;  } } } }
 } 

  
 { 
 #line 149 "Content\CNV3/Basics.mc"
var tmp_0 = P1 as _opAnd; 
 #line 149 "Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
 #line 149 "Content\CNV3/Basics.mc"
var tmp_2 = eval.StaticRun(a, m);
 #line 149 "Content\CNV3/Basics.mc"

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollarb; 
 #line 149 "Content\CNV3/Basics.mc"
if (tmp_3 != null) { 
 var c = tmp_3.P1; 
 #line 149 "Content\CNV3/Basics.mc"
if(c.Equals(true)) { 
 #line 149 "Content\CNV3/Basics.mc"
var tmp_5 = eval.StaticRun(b, m);
 #line 149 "Content\CNV3/Basics.mc"

var tmp_4 = tmp_5;

var tmp_6 = tmp_4 as _opDollarb; 
 #line 149 "Content\CNV3/Basics.mc"
if (tmp_6 != null) { 
 var d = tmp_6.P1; 
 #line 149 "Content\CNV3/Basics.mc"
var result = _opDollarb.Create(d);
 #line 149 "Content\CNV3/Basics.mc"
 return result;  } } } }
 } 

  
 { 
 #line 155 "Content\CNV3/Basics.mc"
var tmp_0 = P1 as _opAnd; 
 #line 155 "Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
 #line 155 "Content\CNV3/Basics.mc"
var tmp_2 = eval.StaticRun(a, m);
 #line 155 "Content\CNV3/Basics.mc"

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollarb; 
 #line 155 "Content\CNV3/Basics.mc"
if (tmp_3 != null) { 
 var c = tmp_3.P1; 
 #line 155 "Content\CNV3/Basics.mc"
if(c.Equals(false)) { 
 #line 155 "Content\CNV3/Basics.mc"
var result = _opDollarb.Create(c);
 #line 155 "Content\CNV3/Basics.mc"
 return result;  } } }
 } 

  
 { 
 #line 160 "Content\CNV3/Basics.mc"
var tmp_0 = P1 as lt; 
 #line 160 "Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
 #line 160 "Content\CNV3/Basics.mc"
var tmp_2 = eval.StaticRun(a, m);
 #line 160 "Content\CNV3/Basics.mc"

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollari; 
 #line 160 "Content\CNV3/Basics.mc"
if (tmp_3 != null) { 
 var x = tmp_3.P1; 
 #line 160 "Content\CNV3/Basics.mc"
var tmp_5 = eval.StaticRun(b, m);
 #line 160 "Content\CNV3/Basics.mc"

var tmp_4 = tmp_5;

var tmp_6 = tmp_4 as _opDollari; 
 #line 160 "Content\CNV3/Basics.mc"
if (tmp_6 != null) { 
 var y = tmp_6.P1; 
 #line 160 "Content\CNV3/Basics.mc"
var result = _opDollarb.Create(x<y);
 #line 160 "Content\CNV3/Basics.mc"
 return result;  } } }
 } 

  
 { 
 #line 165 "Content\CNV3/Basics.mc"
var tmp_0 = P1 as leq; 
 #line 165 "Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
 #line 165 "Content\CNV3/Basics.mc"
var tmp_2 = eval.StaticRun(a, m);
 #line 165 "Content\CNV3/Basics.mc"

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollari; 
 #line 165 "Content\CNV3/Basics.mc"
if (tmp_3 != null) { 
 var x = tmp_3.P1; 
 #line 165 "Content\CNV3/Basics.mc"
var tmp_5 = eval.StaticRun(b, m);
 #line 165 "Content\CNV3/Basics.mc"

var tmp_4 = tmp_5;

var tmp_6 = tmp_4 as _opDollari; 
 #line 165 "Content\CNV3/Basics.mc"
if (tmp_6 != null) { 
 var y = tmp_6.P1; 
 #line 165 "Content\CNV3/Basics.mc"
var result = _opDollarb.Create(x<=y);
 #line 165 "Content\CNV3/Basics.mc"
 return result;  } } }
 } 

  
 { 
 #line 170 "Content\CNV3/Basics.mc"
var tmp_0 = P1 as gt; 
 #line 170 "Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
 #line 170 "Content\CNV3/Basics.mc"
var tmp_2 = eval.StaticRun(a, m);
 #line 170 "Content\CNV3/Basics.mc"

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollari; 
 #line 170 "Content\CNV3/Basics.mc"
if (tmp_3 != null) { 
 var x = tmp_3.P1; 
 #line 170 "Content\CNV3/Basics.mc"
var tmp_5 = eval.StaticRun(b, m);
 #line 170 "Content\CNV3/Basics.mc"

var tmp_4 = tmp_5;

var tmp_6 = tmp_4 as _opDollari; 
 #line 170 "Content\CNV3/Basics.mc"
if (tmp_6 != null) { 
 var y = tmp_6.P1; 
 #line 170 "Content\CNV3/Basics.mc"
var result = _opDollarb.Create(x>y);
 #line 170 "Content\CNV3/Basics.mc"
 return result;  } } }
 } 

  
 { 
 #line 175 "Content\CNV3/Basics.mc"
var tmp_0 = P1 as geq; 
 #line 175 "Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
 #line 175 "Content\CNV3/Basics.mc"
var tmp_2 = eval.StaticRun(a, m);
 #line 175 "Content\CNV3/Basics.mc"

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollari; 
 #line 175 "Content\CNV3/Basics.mc"
if (tmp_3 != null) { 
 var x = tmp_3.P1; 
 #line 175 "Content\CNV3/Basics.mc"
var tmp_5 = eval.StaticRun(b, m);
 #line 175 "Content\CNV3/Basics.mc"

var tmp_4 = tmp_5;

var tmp_6 = tmp_4 as _opDollari; 
 #line 175 "Content\CNV3/Basics.mc"
if (tmp_6 != null) { 
 var y = tmp_6.P1; 
 #line 175 "Content\CNV3/Basics.mc"
var result = _opDollarb.Create(x>=y);
 #line 175 "Content\CNV3/Basics.mc"
 return result;  } } }
 } 

  
 { 
 #line 180 "Content\CNV3/Basics.mc"
var tmp_0 = P1 as lt; 
 #line 180 "Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
 #line 180 "Content\CNV3/Basics.mc"
var tmp_2 = eval.StaticRun(a, m);
 #line 180 "Content\CNV3/Basics.mc"

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollarf; 
 #line 180 "Content\CNV3/Basics.mc"
if (tmp_3 != null) { 
 var x = tmp_3.P1; 
 #line 180 "Content\CNV3/Basics.mc"
var tmp_5 = eval.StaticRun(b, m);
 #line 180 "Content\CNV3/Basics.mc"

var tmp_4 = tmp_5;

var tmp_6 = tmp_4 as _opDollarf; 
 #line 180 "Content\CNV3/Basics.mc"
if (tmp_6 != null) { 
 var y = tmp_6.P1; 
 #line 180 "Content\CNV3/Basics.mc"
var result = _opDollarb.Create(x<y);
 #line 180 "Content\CNV3/Basics.mc"
 return result;  } } }
 } 

  
 { 
 #line 185 "Content\CNV3/Basics.mc"
var tmp_0 = P1 as leq; 
 #line 185 "Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
 #line 185 "Content\CNV3/Basics.mc"
var tmp_2 = eval.StaticRun(a, m);
 #line 185 "Content\CNV3/Basics.mc"

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollarf; 
 #line 185 "Content\CNV3/Basics.mc"
if (tmp_3 != null) { 
 var x = tmp_3.P1; 
 #line 185 "Content\CNV3/Basics.mc"
var tmp_5 = eval.StaticRun(b, m);
 #line 185 "Content\CNV3/Basics.mc"

var tmp_4 = tmp_5;

var tmp_6 = tmp_4 as _opDollarf; 
 #line 185 "Content\CNV3/Basics.mc"
if (tmp_6 != null) { 
 var y = tmp_6.P1; 
 #line 185 "Content\CNV3/Basics.mc"
var result = _opDollarb.Create(x<=y);
 #line 185 "Content\CNV3/Basics.mc"
 return result;  } } }
 } 

  
 { 
 #line 190 "Content\CNV3/Basics.mc"
var tmp_0 = P1 as gt; 
 #line 190 "Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
 #line 190 "Content\CNV3/Basics.mc"
var tmp_2 = eval.StaticRun(a, m);
 #line 190 "Content\CNV3/Basics.mc"

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollarf; 
 #line 190 "Content\CNV3/Basics.mc"
if (tmp_3 != null) { 
 var x = tmp_3.P1; 
 #line 190 "Content\CNV3/Basics.mc"
var tmp_5 = eval.StaticRun(b, m);
 #line 190 "Content\CNV3/Basics.mc"

var tmp_4 = tmp_5;

var tmp_6 = tmp_4 as _opDollarf; 
 #line 190 "Content\CNV3/Basics.mc"
if (tmp_6 != null) { 
 var y = tmp_6.P1; 
 #line 190 "Content\CNV3/Basics.mc"
var result = _opDollarb.Create(x>y);
 #line 190 "Content\CNV3/Basics.mc"
 return result;  } } }
 } 

  
 { 
 #line 195 "Content\CNV3/Basics.mc"
var tmp_0 = P1 as geq; 
 #line 195 "Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
 #line 195 "Content\CNV3/Basics.mc"
var tmp_2 = eval.StaticRun(a, m);
 #line 195 "Content\CNV3/Basics.mc"

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollarf; 
 #line 195 "Content\CNV3/Basics.mc"
if (tmp_3 != null) { 
 var x = tmp_3.P1; 
 #line 195 "Content\CNV3/Basics.mc"
var tmp_5 = eval.StaticRun(b, m);
 #line 195 "Content\CNV3/Basics.mc"

var tmp_4 = tmp_5;

var tmp_6 = tmp_4 as _opDollarf; 
 #line 195 "Content\CNV3/Basics.mc"
if (tmp_6 != null) { 
 var y = tmp_6.P1; 
 #line 195 "Content\CNV3/Basics.mc"
var result = _opDollarb.Create(x>=y);
 #line 195 "Content\CNV3/Basics.mc"
 return result;  } } }
 } 

  
 { 
 #line 201 "Content\CNV3/Basics.mc"
var tmp_0 = P1 as _opDollarl; 
 #line 201 "Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var li = tmp_0.P1; var m = P2; 
 #line 201 "Content\CNV3/Basics.mc"
var result = _opDollarl.Create(li);
 #line 201 "Content\CNV3/Basics.mc"
 return result;  }
 } 

  
 { 
 #line 204 "Content\CNV3/Basics.mc"
var tmp_0 = P1 as _opAddition_opAddition; 
 #line 204 "Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var e = tmp_0.P1; var el = tmp_0.P2; var m = P2; 
 #line 204 "Content\CNV3/Basics.mc"
var tmp_2 = eval.StaticRun(e, m);
 #line 204 "Content\CNV3/Basics.mc"

var tmp_1 = tmp_2;

var v = tmp_1; 
 #line 204 "Content\CNV3/Basics.mc"
var tmp_4 = eval.StaticRun(el, m);
 #line 204 "Content\CNV3/Basics.mc"

var tmp_3 = tmp_4;

var tmp_5 = tmp_3 as _opDollarl; 
 #line 204 "Content\CNV3/Basics.mc"
if (tmp_5 != null) { 
 var li = tmp_5.P1; 
 #line 204 "Content\CNV3/Basics.mc"
var result = _opDollarl.Create(_Colon_Colon<Value>.Create(v, li));
 #line 204 "Content\CNV3/Basics.mc"
 return result;  } }
 } 

  
 { 
 #line 209 "Content\CNV3/Basics.mc"
var tmp_0 = P1 as _opAt; 
 #line 209 "Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var ex = tmp_0.P1; var ey = tmp_0.P2; var m = P2; 
 #line 209 "Content\CNV3/Basics.mc"
var tmp_2 = eval.StaticRun(ex, m);
 #line 209 "Content\CNV3/Basics.mc"

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollarl; 
 #line 209 "Content\CNV3/Basics.mc"
if (tmp_3 != null) { 
 var xs = tmp_3.P1; 
 #line 209 "Content\CNV3/Basics.mc"
var tmp_5 = eval.StaticRun(ey, m);
 #line 209 "Content\CNV3/Basics.mc"

var tmp_4 = tmp_5;

var tmp_6 = tmp_4 as _opDollarl; 
 #line 209 "Content\CNV3/Basics.mc"
if (tmp_6 != null) { 
 var ys = tmp_6.P1; 
 #line 209 "Content\CNV3/Basics.mc"
var tmp_8 = append<Value>.StaticRun(xs, ys);
 #line 209 "Content\CNV3/Basics.mc"

var tmp_7 = tmp_8;

var zs = tmp_7; 
 #line 209 "Content\CNV3/Basics.mc"
var result = _opDollarl.Create(zs);
 #line 209 "Content\CNV3/Basics.mc"
 return result;  } } }
 } 

  
 { 
 #line 215 "Content\CNV3/Basics.mc"
var tmp_0 = P1 as _opDollarfirst; 
 #line 215 "Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var expr = tmp_0.P1; var m = P2; 
 #line 215 "Content\CNV3/Basics.mc"
var tmp_2 = eval.StaticRun(expr, m);
 #line 215 "Content\CNV3/Basics.mc"

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollart; 
 #line 215 "Content\CNV3/Basics.mc"
if (tmp_3 != null) { 
 var t = tmp_3.P1; 
 #line 215 "Content\CNV3/Basics.mc"
var tmp_5 = fst<Value, Value>.StaticRun(t);
 #line 215 "Content\CNV3/Basics.mc"

var tmp_4 = tmp_5;

var res = tmp_4; 
 #line 215 "Content\CNV3/Basics.mc"
var result = res;
 #line 215 "Content\CNV3/Basics.mc"
 return result;  } }
 } 

  
 { 
 #line 220 "Content\CNV3/Basics.mc"
var tmp_0 = P1 as _opDollarsecond; 
 #line 220 "Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var expr = tmp_0.P1; var m = P2; 
 #line 220 "Content\CNV3/Basics.mc"
var tmp_2 = eval.StaticRun(expr, m);
 #line 220 "Content\CNV3/Basics.mc"

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollart; 
 #line 220 "Content\CNV3/Basics.mc"
if (tmp_3 != null) { 
 var t = tmp_3.P1; 
 #line 220 "Content\CNV3/Basics.mc"
var tmp_5 = snd<Value, Value>.StaticRun(t);
 #line 220 "Content\CNV3/Basics.mc"

var tmp_4 = tmp_5;

var res = tmp_4; 
 #line 220 "Content\CNV3/Basics.mc"
var result = res;
 #line 220 "Content\CNV3/Basics.mc"
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
 { 
 #line 8 "Content/CNV3/Tuples.mc"
var tmp_0 = P1 as _Comma<a, b>; 
 #line 8 "Content/CNV3/Tuples.mc"
if (tmp_0 != null) { 
 var x = tmp_0.P1; var y = tmp_0.P2; 
 #line 8 "Content/CNV3/Tuples.mc"
var result = x;
 #line 8 "Content/CNV3/Tuples.mc"
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
 #line 10 "Content/GenericLists/transform.mc"
var tmp_0 = P1 as nil<a>; 
 #line 10 "Content/GenericLists/transform.mc"
if (tmp_0 != null) { 
 
 #line 10 "Content/GenericLists/transform.mc"
var result = 0;
 #line 10 "Content/GenericLists/transform.mc"
 return result;  }
 } 

  
 { 
 #line 13 "Content/GenericLists/transform.mc"
var tmp_0 = P1 as _Colon_Colon<a>; 
 #line 13 "Content/GenericLists/transform.mc"
if (tmp_0 != null) { 
 var x = tmp_0.P1; var xs = tmp_0.P2; 
 #line 13 "Content/GenericLists/transform.mc"
var tmp_2 = length<a>.StaticRun(xs);
 #line 13 "Content/GenericLists/transform.mc"

var tmp_1 = tmp_2;

var y = tmp_1; 
 #line 13 "Content/GenericLists/transform.mc"
var result = (1+y);
 #line 13 "Content/GenericLists/transform.mc"
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
 #line 26 "Content/GenericLists/transform.mc"
var tmp_0 = P1 as nil<int>; 
 #line 26 "Content/GenericLists/transform.mc"
if (tmp_0 != null) { 
 
 #line 26 "Content/GenericLists/transform.mc"
var result = nil<int>.Create();
 #line 26 "Content/GenericLists/transform.mc"
 return result;  }
 } 

  
 { 
 #line 29 "Content/GenericLists/transform.mc"
var tmp_0 = P1 as _Colon_Colon<int>; 
 #line 29 "Content/GenericLists/transform.mc"
if (tmp_0 != null) { 
 var x = tmp_0.P1; var xs = tmp_0.P2; 
 #line 29 "Content/GenericLists/transform.mc"
if((x%2).Equals(0)) { 
 #line 29 "Content/GenericLists/transform.mc"
var tmp_2 = removeOdd.StaticRun(xs);
 #line 29 "Content/GenericLists/transform.mc"

var tmp_1 = tmp_2;

var xs_Prime = tmp_1; 
 #line 29 "Content/GenericLists/transform.mc"
var result = xs_Prime;
 #line 29 "Content/GenericLists/transform.mc"
 return result;  } }
 } 

  
 { 
 #line 34 "Content/GenericLists/transform.mc"
var tmp_0 = P1 as _Colon_Colon<int>; 
 #line 34 "Content/GenericLists/transform.mc"
if (tmp_0 != null) { 
 var x = tmp_0.P1; var xs = tmp_0.P2; 
 #line 34 "Content/GenericLists/transform.mc"
if((x%2).Equals(1)) { 
 #line 34 "Content/GenericLists/transform.mc"
var tmp_2 = removeOdd.StaticRun(xs);
 #line 34 "Content/GenericLists/transform.mc"

var tmp_1 = tmp_2;

var xs_Prime = tmp_1; 
 #line 34 "Content/GenericLists/transform.mc"
var result = _Colon_Colon<int>.Create(x, xs_Prime);
 #line 34 "Content/GenericLists/transform.mc"
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
 { 
 #line 11 "Content/CNV3/Tuples.mc"
var tmp_0 = P1 as _Comma<a, b>; 
 #line 11 "Content/CNV3/Tuples.mc"
if (tmp_0 != null) { 
 var x = tmp_0.P1; var y = tmp_0.P2; 
 #line 11 "Content/CNV3/Tuples.mc"
var result = y;
 #line 11 "Content/CNV3/Tuples.mc"
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

public class test : Test  {

public test() {}
public static test Create() { return new test(); }

  public static Value StaticRun() {    
 { 
 #line 225 "Content\CNV3/Basics.mc"
var v1 = _opDollarVector3.Create(Vector3.Zero); var v2 = _opDollarVector3.Create(Vector3.One); var m = Context.Create(ImmutableDictionary<string,Value>.Empty, ImmutableDictionary<string,Value>.Empty, ImmutableDictionary<string,Value>.Empty); 
 #line 225 "Content\CNV3/Basics.mc"
var tmp_1 = eval.StaticRun(_opAddition.Create(v1, v2), m);
 #line 225 "Content\CNV3/Basics.mc"

var tmp_0 = tmp_1;

var res = tmp_0; 
 #line 225 "Content\CNV3/Basics.mc"
var result = res;
 #line 225 "Content\CNV3/Basics.mc"
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
