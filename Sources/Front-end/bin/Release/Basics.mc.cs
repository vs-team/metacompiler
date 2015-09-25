using System.Collections.Immutable;
using System;
using System;
using UnityEngine;
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
public interface Utility {}
public interface Test {}



public class _opDollar : ID  {
public string P1;

public _opDollar(string P1) {this.P1 = P1;}
public static _opDollar Create(string P1) { return new _opDollar(P1); }

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

 res += " $first "; res += P1.ToString(); 

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

 res += " $s "; res += P1.ToString(); 

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

 res += " $second "; res += P1.ToString(); 

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

 res += " $t "; res += P1.ToString(); 

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

public class _opDollarwrapper : Value  {
public WrapperTest P1;

public _opDollarwrapper(WrapperTest P1) {this.P1 = P1;}
public static _opDollarwrapper Create(WrapperTest P1) { return new _opDollarwrapper(P1); }

public override string ToString() {
 var res = "("; 

 res += " $wrapper "; res += P1.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _opDollarwrapper;
 if(tmp != null) return this.P1.Equals(tmp.P1); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class _opDollarwrapperSet : Value  {
public WrapperTest P1;
public Vector3 P2;

public _opDollarwrapperSet(WrapperTest P1, Vector3 P2) {this.P1 = P1; this.P2 = P2;}
public static _opDollarwrapperSet Create(WrapperTest P1, Vector3 P2) { return new _opDollarwrapperSet(P1, P2); }

public override string ToString() {
 var res = "("; 

 res += " $wrapperSet "; res += P1.ToString(); 
res += P2.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _opDollarwrapperSet;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
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

public class _opAddition_opAddition : Expr  {
public Expr P1;
public Expr P2;

public _opAddition_opAddition(Expr P1, Expr P2) {this.P1 = P1; this.P2 = P2;}
public static _opAddition_opAddition Create(Expr P1, Expr P2) { return new _opAddition_opAddition(P1, P2); }

public override string ToString() {
 var res = "("; 
res += P1.ToString(); 

 res += " ++ "; res += P2.ToString(); 

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
res += P1.ToString(); 

 res += " , "; res += P2.ToString(); 

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

public class _opDivision : Expr  {
public Expr P1;
public Expr P2;

public _opDivision(Expr P1, Expr P2) {this.P1 = P1; this.P2 = P2;}
public static _opDivision Create(Expr P1, Expr P2) { return new _opDivision(P1, P2); }

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

public class _Colon_Colon<a> : List<a>  {
public a P1;
public List<a> P2;

public _Colon_Colon(a P1, List<a> P2) {this.P1 = P1; this.P2 = P2;}
public static _Colon_Colon<a> Create(a P1, List<a> P2) { return new _Colon_Colon<a>(P1, P2); }

public override string ToString() {
 var res = "("; 
res += P1.ToString(); 

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
res += P1.ToString(); 

 res += " @ "; res += P2.ToString(); 

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

public class createArgString : Utility  {
public List<Expr> P1;

public createArgString(List<Expr> P1) {this.P1 = P1;}
public static createArgString Create(List<Expr> P1) { return new createArgString(P1); }


public static Value StaticRun(List<Expr> P1) { 
throw new System.Exception("Error evaluating: createArgString no result returned."); }
public Value Run(){ return StaticRun(P1); }

public override string ToString() {
 var res = "("; 

 res += " createArgString "; if (P1 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P1.ToString(); } 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as createArgString;
 if(tmp != null) return this.P1.Equals(tmp.P1); 
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
 #line 54 "../../../Content/Content\CNV3/Basics.mc"
var tmp_0 = P1 as _opDollar; 
 #line 54 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var a = tmp_0.P1; var tmp_1 = P2 as Context; 
 #line 54 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_1 != null) { 
 var locals = tmp_1.P1; var entity = tmp_1.P2; var world = tmp_1.P3; 
 #line 54 "../../../Content/Content\CNV3/Basics.mc"
if((locals.ContainsKey(a)).Equals(true)) { var res = (locals.GetKey(a)); 
 #line 54 "../../../Content/Content\CNV3/Basics.mc"
var result = res;
 #line 54 "../../../Content/Content\CNV3/Basics.mc"
 return result;  } } }
 } 

  
 { 
 #line 59 "../../../Content/Content\CNV3/Basics.mc"
var tmp_0 = P1 as _opDollar; 
 #line 59 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var a = tmp_0.P1; var tmp_1 = P2 as Context; 
 #line 59 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_1 != null) { 
 var locals = tmp_1.P1; var entity = tmp_1.P2; var world = tmp_1.P3; var res = (entity.GetKey(a)); 
 #line 59 "../../../Content/Content\CNV3/Basics.mc"
var result = res;
 #line 59 "../../../Content/Content\CNV3/Basics.mc"
 return result;  } }
 } 

  
 { 
 #line 63 "../../../Content/Content\CNV3/Basics.mc"
var tmp_0 = P1 as _opDollarb; 
 #line 63 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var a = tmp_0.P1; var m = P2; 
 #line 63 "../../../Content/Content\CNV3/Basics.mc"
var result = _opDollarb.Create(a);
 #line 63 "../../../Content/Content\CNV3/Basics.mc"
 return result;  }
 } 

  
 { 
 #line 66 "../../../Content/Content\CNV3/Basics.mc"
var tmp_0 = P1 as _opDollari; 
 #line 66 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var a = tmp_0.P1; var m = P2; 
 #line 66 "../../../Content/Content\CNV3/Basics.mc"
var result = _opDollari.Create(a);
 #line 66 "../../../Content/Content\CNV3/Basics.mc"
 return result;  }
 } 

  
 { 
 #line 69 "../../../Content/Content\CNV3/Basics.mc"
var tmp_0 = P1 as _opDollars; 
 #line 69 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var s = tmp_0.P1; var m = P2; 
 #line 69 "../../../Content/Content\CNV3/Basics.mc"
var result = _opDollars.Create(s);
 #line 69 "../../../Content/Content\CNV3/Basics.mc"
 return result;  }
 } 

  
 { 
 #line 72 "../../../Content/Content\CNV3/Basics.mc"
var tmp_0 = P1 as _opDollarf; 
 #line 72 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var f = tmp_0.P1; var m = P2; 
 #line 72 "../../../Content/Content\CNV3/Basics.mc"
var result = _opDollarf.Create(f);
 #line 72 "../../../Content/Content\CNV3/Basics.mc"
 return result;  }
 } 

  
 { 
 #line 75 "../../../Content/Content\CNV3/Basics.mc"
var tmp_0 = P1 as _opDollart; 
 #line 75 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var t = tmp_0.P1; var m = P2; 
 #line 75 "../../../Content/Content\CNV3/Basics.mc"
var result = _opDollart.Create(t);
 #line 75 "../../../Content/Content\CNV3/Basics.mc"
 return result;  }
 } 

  
 { 
 #line 78 "../../../Content/Content\CNV3/Basics.mc"
var tmp_0 = P1 as _opDollarVector3; 
 #line 78 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var v = tmp_0.P1; var m = P2; 
 #line 78 "../../../Content/Content\CNV3/Basics.mc"
var result = _opDollarVector3.Create(v);
 #line 78 "../../../Content/Content\CNV3/Basics.mc"
 return result;  }
 } 

  
 { 
 #line 82 "../../../Content/Content\CNV3/Basics.mc"
var tmp_0 = P1 as _opAddition; 
 #line 82 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
 #line 82 "../../../Content/Content\CNV3/Basics.mc"
var tmp_2 = eval.StaticRun(a, m);
 #line 82 "../../../Content/Content\CNV3/Basics.mc"

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollarVector3; 
 #line 82 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_3 != null) { 
 var v1 = tmp_3.P1; 
 #line 82 "../../../Content/Content\CNV3/Basics.mc"
var tmp_5 = eval.StaticRun(b, m);
 #line 82 "../../../Content/Content\CNV3/Basics.mc"

var tmp_4 = tmp_5;

var tmp_6 = tmp_4 as _opDollarVector3; 
 #line 82 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_6 != null) { 
 var v2 = tmp_6.P1; var res = (v1+v2); 
 #line 82 "../../../Content/Content\CNV3/Basics.mc"
var result = _opDollarVector3.Create(res);
 #line 82 "../../../Content/Content\CNV3/Basics.mc"
 return result;  } } }
 } 

  
 { 
 #line 88 "../../../Content/Content\CNV3/Basics.mc"
var tmp_0 = P1 as _opSubtraction; 
 #line 88 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
 #line 88 "../../../Content/Content\CNV3/Basics.mc"
var tmp_2 = eval.StaticRun(a, m);
 #line 88 "../../../Content/Content\CNV3/Basics.mc"

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollarVector3; 
 #line 88 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_3 != null) { 
 var v1 = tmp_3.P1; 
 #line 88 "../../../Content/Content\CNV3/Basics.mc"
var tmp_5 = eval.StaticRun(b, m);
 #line 88 "../../../Content/Content\CNV3/Basics.mc"

var tmp_4 = tmp_5;

var tmp_6 = tmp_4 as _opDollarVector3; 
 #line 88 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_6 != null) { 
 var v2 = tmp_6.P1; var res = (v1-v2); 
 #line 88 "../../../Content/Content\CNV3/Basics.mc"
var result = _opDollarVector3.Create(res);
 #line 88 "../../../Content/Content\CNV3/Basics.mc"
 return result;  } } }
 } 

  
 { 
 #line 94 "../../../Content/Content\CNV3/Basics.mc"
var tmp_0 = P1 as _opMultiplication; 
 #line 94 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var v = tmp_0.P1; var s = tmp_0.P2; var m = P2; 
 #line 94 "../../../Content/Content\CNV3/Basics.mc"
var tmp_2 = eval.StaticRun(v, m);
 #line 94 "../../../Content/Content\CNV3/Basics.mc"

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollarVector3; 
 #line 94 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_3 != null) { 
 var v1 = tmp_3.P1; 
 #line 94 "../../../Content/Content\CNV3/Basics.mc"
var tmp_5 = eval.StaticRun(s, m);
 #line 94 "../../../Content/Content\CNV3/Basics.mc"

var tmp_4 = tmp_5;

var tmp_6 = tmp_4 as _opDollarf; 
 #line 94 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_6 != null) { 
 var s1 = tmp_6.P1; var res = (v1*s1); 
 #line 94 "../../../Content/Content\CNV3/Basics.mc"
var result = _opDollarVector3.Create(res);
 #line 94 "../../../Content/Content\CNV3/Basics.mc"
 return result;  } } }
 } 

  
 { 
 #line 100 "../../../Content/Content\CNV3/Basics.mc"
var tmp_0 = P1 as _opDivision; 
 #line 100 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var v = tmp_0.P1; var s = tmp_0.P2; var m = P2; 
 #line 100 "../../../Content/Content\CNV3/Basics.mc"
var tmp_2 = eval.StaticRun(v, m);
 #line 100 "../../../Content/Content\CNV3/Basics.mc"

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollarVector3; 
 #line 100 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_3 != null) { 
 var v1 = tmp_3.P1; 
 #line 100 "../../../Content/Content\CNV3/Basics.mc"
var tmp_5 = eval.StaticRun(s, m);
 #line 100 "../../../Content/Content\CNV3/Basics.mc"

var tmp_4 = tmp_5;

var tmp_6 = tmp_4 as _opDollarf; 
 #line 100 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_6 != null) { 
 var s1 = tmp_6.P1; var res = (v1/s1); 
 #line 100 "../../../Content/Content\CNV3/Basics.mc"
var result = _opDollarVector3.Create(res);
 #line 100 "../../../Content/Content\CNV3/Basics.mc"
 return result;  } } }
 } 

  
 { 
 #line 107 "../../../Content/Content\CNV3/Basics.mc"
var tmp_0 = P1 as _opAddition; 
 #line 107 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
 #line 107 "../../../Content/Content\CNV3/Basics.mc"
var tmp_2 = eval.StaticRun(a, m);
 #line 107 "../../../Content/Content\CNV3/Basics.mc"

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollari; 
 #line 107 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_3 != null) { 
 var c = tmp_3.P1; 
 #line 107 "../../../Content/Content\CNV3/Basics.mc"
var tmp_5 = eval.StaticRun(b, m);
 #line 107 "../../../Content/Content\CNV3/Basics.mc"

var tmp_4 = tmp_5;

var tmp_6 = tmp_4 as _opDollari; 
 #line 107 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_6 != null) { 
 var d = tmp_6.P1; var res = (c+d); 
 #line 107 "../../../Content/Content\CNV3/Basics.mc"
var result = _opDollari.Create(res);
 #line 107 "../../../Content/Content\CNV3/Basics.mc"
 return result;  } } }
 } 

  
 { 
 #line 113 "../../../Content/Content\CNV3/Basics.mc"
var tmp_0 = P1 as _opSubtraction; 
 #line 113 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
 #line 113 "../../../Content/Content\CNV3/Basics.mc"
var tmp_2 = eval.StaticRun(a, m);
 #line 113 "../../../Content/Content\CNV3/Basics.mc"

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollari; 
 #line 113 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_3 != null) { 
 var c = tmp_3.P1; 
 #line 113 "../../../Content/Content\CNV3/Basics.mc"
var tmp_5 = eval.StaticRun(b, m);
 #line 113 "../../../Content/Content\CNV3/Basics.mc"

var tmp_4 = tmp_5;

var tmp_6 = tmp_4 as _opDollari; 
 #line 113 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_6 != null) { 
 var d = tmp_6.P1; var res = (c-d); 
 #line 113 "../../../Content/Content\CNV3/Basics.mc"
var result = _opDollari.Create(res);
 #line 113 "../../../Content/Content\CNV3/Basics.mc"
 return result;  } } }
 } 

  
 { 
 #line 120 "../../../Content/Content\CNV3/Basics.mc"
var tmp_0 = P1 as _opMultiplication; 
 #line 120 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
 #line 120 "../../../Content/Content\CNV3/Basics.mc"
var tmp_2 = eval.StaticRun(a, m);
 #line 120 "../../../Content/Content\CNV3/Basics.mc"

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollari; 
 #line 120 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_3 != null) { 
 var c = tmp_3.P1; 
 #line 120 "../../../Content/Content\CNV3/Basics.mc"
var tmp_5 = eval.StaticRun(b, m);
 #line 120 "../../../Content/Content\CNV3/Basics.mc"

var tmp_4 = tmp_5;

var tmp_6 = tmp_4 as _opDollari; 
 #line 120 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_6 != null) { 
 var d = tmp_6.P1; var res = (c*d); 
 #line 120 "../../../Content/Content\CNV3/Basics.mc"
var result = _opDollari.Create(res);
 #line 120 "../../../Content/Content\CNV3/Basics.mc"
 return result;  } } }
 } 

  
 { 
 #line 126 "../../../Content/Content\CNV3/Basics.mc"
var tmp_0 = P1 as _opDivision; 
 #line 126 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
 #line 126 "../../../Content/Content\CNV3/Basics.mc"
var tmp_2 = eval.StaticRun(a, m);
 #line 126 "../../../Content/Content\CNV3/Basics.mc"

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollari; 
 #line 126 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_3 != null) { 
 var c = tmp_3.P1; 
 #line 126 "../../../Content/Content\CNV3/Basics.mc"
var tmp_5 = eval.StaticRun(b, m);
 #line 126 "../../../Content/Content\CNV3/Basics.mc"

var tmp_4 = tmp_5;

var tmp_6 = tmp_4 as _opDollari; 
 #line 126 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_6 != null) { 
 var d = tmp_6.P1; var res = (c/d); 
 #line 126 "../../../Content/Content\CNV3/Basics.mc"
var result = _opDollari.Create(res);
 #line 126 "../../../Content/Content\CNV3/Basics.mc"
 return result;  } } }
 } 

  
 { 
 #line 132 "../../../Content/Content\CNV3/Basics.mc"
var tmp_0 = P1 as _opAddition; 
 #line 132 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
 #line 132 "../../../Content/Content\CNV3/Basics.mc"
var tmp_2 = eval.StaticRun(a, m);
 #line 132 "../../../Content/Content\CNV3/Basics.mc"

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollarf; 
 #line 132 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_3 != null) { 
 var c = tmp_3.P1; 
 #line 132 "../../../Content/Content\CNV3/Basics.mc"
var tmp_5 = eval.StaticRun(b, m);
 #line 132 "../../../Content/Content\CNV3/Basics.mc"

var tmp_4 = tmp_5;

var tmp_6 = tmp_4 as _opDollarf; 
 #line 132 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_6 != null) { 
 var d = tmp_6.P1; var res = (c+d); 
 #line 132 "../../../Content/Content\CNV3/Basics.mc"
var result = _opDollarf.Create(res);
 #line 132 "../../../Content/Content\CNV3/Basics.mc"
 return result;  } } }
 } 

  
 { 
 #line 138 "../../../Content/Content\CNV3/Basics.mc"
var tmp_0 = P1 as _opMultiplication; 
 #line 138 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
 #line 138 "../../../Content/Content\CNV3/Basics.mc"
var tmp_2 = eval.StaticRun(a, m);
 #line 138 "../../../Content/Content\CNV3/Basics.mc"

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollarf; 
 #line 138 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_3 != null) { 
 var c = tmp_3.P1; 
 #line 138 "../../../Content/Content\CNV3/Basics.mc"
var tmp_5 = eval.StaticRun(b, m);
 #line 138 "../../../Content/Content\CNV3/Basics.mc"

var tmp_4 = tmp_5;

var tmp_6 = tmp_4 as _opDollarf; 
 #line 138 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_6 != null) { 
 var d = tmp_6.P1; var res = (c*d); 
 #line 138 "../../../Content/Content\CNV3/Basics.mc"
var result = _opDollarf.Create(res);
 #line 138 "../../../Content/Content\CNV3/Basics.mc"
 return result;  } } }
 } 

  
 { 
 #line 144 "../../../Content/Content\CNV3/Basics.mc"
var tmp_0 = P1 as _opDivision; 
 #line 144 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
 #line 144 "../../../Content/Content\CNV3/Basics.mc"
var tmp_2 = eval.StaticRun(a, m);
 #line 144 "../../../Content/Content\CNV3/Basics.mc"

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollarf; 
 #line 144 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_3 != null) { 
 var c = tmp_3.P1; 
 #line 144 "../../../Content/Content\CNV3/Basics.mc"
var tmp_5 = eval.StaticRun(b, m);
 #line 144 "../../../Content/Content\CNV3/Basics.mc"

var tmp_4 = tmp_5;

var tmp_6 = tmp_4 as _opDollarf; 
 #line 144 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_6 != null) { 
 var d = tmp_6.P1; var res = (c/d); 
 #line 144 "../../../Content/Content\CNV3/Basics.mc"
var result = _opDollarf.Create(res);
 #line 144 "../../../Content/Content\CNV3/Basics.mc"
 return result;  } } }
 } 

  
 { 
 #line 150 "../../../Content/Content\CNV3/Basics.mc"
var tmp_0 = P1 as _opSubtraction; 
 #line 150 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
 #line 150 "../../../Content/Content\CNV3/Basics.mc"
var tmp_2 = eval.StaticRun(a, m);
 #line 150 "../../../Content/Content\CNV3/Basics.mc"

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollarf; 
 #line 150 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_3 != null) { 
 var c = tmp_3.P1; 
 #line 150 "../../../Content/Content\CNV3/Basics.mc"
var tmp_5 = eval.StaticRun(b, m);
 #line 150 "../../../Content/Content\CNV3/Basics.mc"

var tmp_4 = tmp_5;

var tmp_6 = tmp_4 as _opDollarf; 
 #line 150 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_6 != null) { 
 var d = tmp_6.P1; var res = (c-d); 
 #line 150 "../../../Content/Content\CNV3/Basics.mc"
var result = _opDollarf.Create(res);
 #line 150 "../../../Content/Content\CNV3/Basics.mc"
 return result;  } } }
 } 

  
 { 
 #line 156 "../../../Content/Content\CNV3/Basics.mc"
var tmp_0 = P1 as _opOr; 
 #line 156 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
 #line 156 "../../../Content/Content\CNV3/Basics.mc"
var tmp_2 = eval.StaticRun(a, m);
 #line 156 "../../../Content/Content\CNV3/Basics.mc"

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollarb; 
 #line 156 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_3 != null) { 
 var c = tmp_3.P1; 
 #line 156 "../../../Content/Content\CNV3/Basics.mc"
if(c.Equals(true)) { 
 #line 156 "../../../Content/Content\CNV3/Basics.mc"
var result = _opDollarb.Create(c);
 #line 156 "../../../Content/Content\CNV3/Basics.mc"
 return result;  } } }
 } 

  
 { 
 #line 161 "../../../Content/Content\CNV3/Basics.mc"
var tmp_0 = P1 as _opOr; 
 #line 161 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
 #line 161 "../../../Content/Content\CNV3/Basics.mc"
var tmp_2 = eval.StaticRun(a, m);
 #line 161 "../../../Content/Content\CNV3/Basics.mc"

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollarb; 
 #line 161 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_3 != null) { 
 var c = tmp_3.P1; 
 #line 161 "../../../Content/Content\CNV3/Basics.mc"
if(c.Equals(false)) { 
 #line 161 "../../../Content/Content\CNV3/Basics.mc"
var tmp_5 = eval.StaticRun(b, m);
 #line 161 "../../../Content/Content\CNV3/Basics.mc"

var tmp_4 = tmp_5;

var tmp_6 = tmp_4 as _opDollarb; 
 #line 161 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_6 != null) { 
 var d = tmp_6.P1; 
 #line 161 "../../../Content/Content\CNV3/Basics.mc"
var result = _opDollarb.Create(d);
 #line 161 "../../../Content/Content\CNV3/Basics.mc"
 return result;  } } } }
 } 

  
 { 
 #line 167 "../../../Content/Content\CNV3/Basics.mc"
var tmp_0 = P1 as _opAnd; 
 #line 167 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
 #line 167 "../../../Content/Content\CNV3/Basics.mc"
var tmp_2 = eval.StaticRun(a, m);
 #line 167 "../../../Content/Content\CNV3/Basics.mc"

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollarb; 
 #line 167 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_3 != null) { 
 var c = tmp_3.P1; 
 #line 167 "../../../Content/Content\CNV3/Basics.mc"
if(c.Equals(true)) { 
 #line 167 "../../../Content/Content\CNV3/Basics.mc"
var tmp_5 = eval.StaticRun(b, m);
 #line 167 "../../../Content/Content\CNV3/Basics.mc"

var tmp_4 = tmp_5;

var tmp_6 = tmp_4 as _opDollarb; 
 #line 167 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_6 != null) { 
 var d = tmp_6.P1; 
 #line 167 "../../../Content/Content\CNV3/Basics.mc"
var result = _opDollarb.Create(d);
 #line 167 "../../../Content/Content\CNV3/Basics.mc"
 return result;  } } } }
 } 

  
 { 
 #line 173 "../../../Content/Content\CNV3/Basics.mc"
var tmp_0 = P1 as _opAnd; 
 #line 173 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
 #line 173 "../../../Content/Content\CNV3/Basics.mc"
var tmp_2 = eval.StaticRun(a, m);
 #line 173 "../../../Content/Content\CNV3/Basics.mc"

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollarb; 
 #line 173 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_3 != null) { 
 var c = tmp_3.P1; 
 #line 173 "../../../Content/Content\CNV3/Basics.mc"
if(c.Equals(false)) { 
 #line 173 "../../../Content/Content\CNV3/Basics.mc"
var result = _opDollarb.Create(c);
 #line 173 "../../../Content/Content\CNV3/Basics.mc"
 return result;  } } }
 } 

  
 { 
 #line 178 "../../../Content/Content\CNV3/Basics.mc"
var tmp_0 = P1 as lt; 
 #line 178 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
 #line 178 "../../../Content/Content\CNV3/Basics.mc"
var tmp_2 = eval.StaticRun(a, m);
 #line 178 "../../../Content/Content\CNV3/Basics.mc"

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollari; 
 #line 178 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_3 != null) { 
 var x = tmp_3.P1; 
 #line 178 "../../../Content/Content\CNV3/Basics.mc"
var tmp_5 = eval.StaticRun(b, m);
 #line 178 "../../../Content/Content\CNV3/Basics.mc"

var tmp_4 = tmp_5;

var tmp_6 = tmp_4 as _opDollari; 
 #line 178 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_6 != null) { 
 var y = tmp_6.P1; 
 #line 178 "../../../Content/Content\CNV3/Basics.mc"
var result = _opDollarb.Create(x<y);
 #line 178 "../../../Content/Content\CNV3/Basics.mc"
 return result;  } } }
 } 

  
 { 
 #line 183 "../../../Content/Content\CNV3/Basics.mc"
var tmp_0 = P1 as leq; 
 #line 183 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
 #line 183 "../../../Content/Content\CNV3/Basics.mc"
var tmp_2 = eval.StaticRun(a, m);
 #line 183 "../../../Content/Content\CNV3/Basics.mc"

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollari; 
 #line 183 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_3 != null) { 
 var x = tmp_3.P1; 
 #line 183 "../../../Content/Content\CNV3/Basics.mc"
var tmp_5 = eval.StaticRun(b, m);
 #line 183 "../../../Content/Content\CNV3/Basics.mc"

var tmp_4 = tmp_5;

var tmp_6 = tmp_4 as _opDollari; 
 #line 183 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_6 != null) { 
 var y = tmp_6.P1; 
 #line 183 "../../../Content/Content\CNV3/Basics.mc"
var result = _opDollarb.Create(x<=y);
 #line 183 "../../../Content/Content\CNV3/Basics.mc"
 return result;  } } }
 } 

  
 { 
 #line 188 "../../../Content/Content\CNV3/Basics.mc"
var tmp_0 = P1 as gt; 
 #line 188 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
 #line 188 "../../../Content/Content\CNV3/Basics.mc"
var tmp_2 = eval.StaticRun(a, m);
 #line 188 "../../../Content/Content\CNV3/Basics.mc"

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollari; 
 #line 188 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_3 != null) { 
 var x = tmp_3.P1; 
 #line 188 "../../../Content/Content\CNV3/Basics.mc"
var tmp_5 = eval.StaticRun(b, m);
 #line 188 "../../../Content/Content\CNV3/Basics.mc"

var tmp_4 = tmp_5;

var tmp_6 = tmp_4 as _opDollari; 
 #line 188 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_6 != null) { 
 var y = tmp_6.P1; 
 #line 188 "../../../Content/Content\CNV3/Basics.mc"
var result = _opDollarb.Create(x>y);
 #line 188 "../../../Content/Content\CNV3/Basics.mc"
 return result;  } } }
 } 

  
 { 
 #line 193 "../../../Content/Content\CNV3/Basics.mc"
var tmp_0 = P1 as geq; 
 #line 193 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
 #line 193 "../../../Content/Content\CNV3/Basics.mc"
var tmp_2 = eval.StaticRun(a, m);
 #line 193 "../../../Content/Content\CNV3/Basics.mc"

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollari; 
 #line 193 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_3 != null) { 
 var x = tmp_3.P1; 
 #line 193 "../../../Content/Content\CNV3/Basics.mc"
var tmp_5 = eval.StaticRun(b, m);
 #line 193 "../../../Content/Content\CNV3/Basics.mc"

var tmp_4 = tmp_5;

var tmp_6 = tmp_4 as _opDollari; 
 #line 193 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_6 != null) { 
 var y = tmp_6.P1; 
 #line 193 "../../../Content/Content\CNV3/Basics.mc"
var result = _opDollarb.Create(x>=y);
 #line 193 "../../../Content/Content\CNV3/Basics.mc"
 return result;  } } }
 } 

  
 { 
 #line 198 "../../../Content/Content\CNV3/Basics.mc"
var tmp_0 = P1 as lt; 
 #line 198 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
 #line 198 "../../../Content/Content\CNV3/Basics.mc"
var tmp_2 = eval.StaticRun(a, m);
 #line 198 "../../../Content/Content\CNV3/Basics.mc"

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollarf; 
 #line 198 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_3 != null) { 
 var x = tmp_3.P1; 
 #line 198 "../../../Content/Content\CNV3/Basics.mc"
var tmp_5 = eval.StaticRun(b, m);
 #line 198 "../../../Content/Content\CNV3/Basics.mc"

var tmp_4 = tmp_5;

var tmp_6 = tmp_4 as _opDollarf; 
 #line 198 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_6 != null) { 
 var y = tmp_6.P1; 
 #line 198 "../../../Content/Content\CNV3/Basics.mc"
var result = _opDollarb.Create(x<y);
 #line 198 "../../../Content/Content\CNV3/Basics.mc"
 return result;  } } }
 } 

  
 { 
 #line 203 "../../../Content/Content\CNV3/Basics.mc"
var tmp_0 = P1 as leq; 
 #line 203 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
 #line 203 "../../../Content/Content\CNV3/Basics.mc"
var tmp_2 = eval.StaticRun(a, m);
 #line 203 "../../../Content/Content\CNV3/Basics.mc"

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollarf; 
 #line 203 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_3 != null) { 
 var x = tmp_3.P1; 
 #line 203 "../../../Content/Content\CNV3/Basics.mc"
var tmp_5 = eval.StaticRun(b, m);
 #line 203 "../../../Content/Content\CNV3/Basics.mc"

var tmp_4 = tmp_5;

var tmp_6 = tmp_4 as _opDollarf; 
 #line 203 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_6 != null) { 
 var y = tmp_6.P1; 
 #line 203 "../../../Content/Content\CNV3/Basics.mc"
var result = _opDollarb.Create(x<=y);
 #line 203 "../../../Content/Content\CNV3/Basics.mc"
 return result;  } } }
 } 

  
 { 
 #line 208 "../../../Content/Content\CNV3/Basics.mc"
var tmp_0 = P1 as gt; 
 #line 208 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
 #line 208 "../../../Content/Content\CNV3/Basics.mc"
var tmp_2 = eval.StaticRun(a, m);
 #line 208 "../../../Content/Content\CNV3/Basics.mc"

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollarf; 
 #line 208 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_3 != null) { 
 var x = tmp_3.P1; 
 #line 208 "../../../Content/Content\CNV3/Basics.mc"
var tmp_5 = eval.StaticRun(b, m);
 #line 208 "../../../Content/Content\CNV3/Basics.mc"

var tmp_4 = tmp_5;

var tmp_6 = tmp_4 as _opDollarf; 
 #line 208 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_6 != null) { 
 var y = tmp_6.P1; 
 #line 208 "../../../Content/Content\CNV3/Basics.mc"
var result = _opDollarb.Create(x>y);
 #line 208 "../../../Content/Content\CNV3/Basics.mc"
 return result;  } } }
 } 

  
 { 
 #line 213 "../../../Content/Content\CNV3/Basics.mc"
var tmp_0 = P1 as geq; 
 #line 213 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; var m = P2; 
 #line 213 "../../../Content/Content\CNV3/Basics.mc"
var tmp_2 = eval.StaticRun(a, m);
 #line 213 "../../../Content/Content\CNV3/Basics.mc"

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollarf; 
 #line 213 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_3 != null) { 
 var x = tmp_3.P1; 
 #line 213 "../../../Content/Content\CNV3/Basics.mc"
var tmp_5 = eval.StaticRun(b, m);
 #line 213 "../../../Content/Content\CNV3/Basics.mc"

var tmp_4 = tmp_5;

var tmp_6 = tmp_4 as _opDollarf; 
 #line 213 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_6 != null) { 
 var y = tmp_6.P1; 
 #line 213 "../../../Content/Content\CNV3/Basics.mc"
var result = _opDollarb.Create(x>=y);
 #line 213 "../../../Content/Content\CNV3/Basics.mc"
 return result;  } } }
 } 

  
 { 
 #line 219 "../../../Content/Content\CNV3/Basics.mc"
var tmp_0 = P1 as _opDollarl; 
 #line 219 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var li = tmp_0.P1; var m = P2; 
 #line 219 "../../../Content/Content\CNV3/Basics.mc"
var result = _opDollarl.Create(li);
 #line 219 "../../../Content/Content\CNV3/Basics.mc"
 return result;  }
 } 

  
 { 
 #line 222 "../../../Content/Content\CNV3/Basics.mc"
var tmp_0 = P1 as _opAddition_opAddition; 
 #line 222 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var e = tmp_0.P1; var el = tmp_0.P2; var m = P2; 
 #line 222 "../../../Content/Content\CNV3/Basics.mc"
var tmp_2 = eval.StaticRun(e, m);
 #line 222 "../../../Content/Content\CNV3/Basics.mc"

var tmp_1 = tmp_2;

var v = tmp_1; 
 #line 222 "../../../Content/Content\CNV3/Basics.mc"
var tmp_4 = eval.StaticRun(el, m);
 #line 222 "../../../Content/Content\CNV3/Basics.mc"

var tmp_3 = tmp_4;

var tmp_5 = tmp_3 as _opDollarl; 
 #line 222 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_5 != null) { 
 var li = tmp_5.P1; 
 #line 222 "../../../Content/Content\CNV3/Basics.mc"
var result = _opDollarl.Create(_Colon_Colon<Value>.Create(v, li));
 #line 222 "../../../Content/Content\CNV3/Basics.mc"
 return result;  } }
 } 

  
 { 
 #line 227 "../../../Content/Content\CNV3/Basics.mc"
var tmp_0 = P1 as _opAt; 
 #line 227 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var ex = tmp_0.P1; var ey = tmp_0.P2; var m = P2; 
 #line 227 "../../../Content/Content\CNV3/Basics.mc"
var tmp_2 = eval.StaticRun(ex, m);
 #line 227 "../../../Content/Content\CNV3/Basics.mc"

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollarl; 
 #line 227 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_3 != null) { 
 var xs = tmp_3.P1; 
 #line 227 "../../../Content/Content\CNV3/Basics.mc"
var tmp_5 = eval.StaticRun(ey, m);
 #line 227 "../../../Content/Content\CNV3/Basics.mc"

var tmp_4 = tmp_5;

var tmp_6 = tmp_4 as _opDollarl; 
 #line 227 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_6 != null) { 
 var ys = tmp_6.P1; 
 #line 227 "../../../Content/Content\CNV3/Basics.mc"
var tmp_8 = append<Value>.StaticRun(xs, ys);
 #line 227 "../../../Content/Content\CNV3/Basics.mc"

var tmp_7 = tmp_8;

var zs = tmp_7; 
 #line 227 "../../../Content/Content\CNV3/Basics.mc"
var result = _opDollarl.Create(zs);
 #line 227 "../../../Content/Content\CNV3/Basics.mc"
 return result;  } } }
 } 

  
 { 
 #line 233 "../../../Content/Content\CNV3/Basics.mc"
var tmp_0 = P1 as _opDollarfirst; 
 #line 233 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var expr = tmp_0.P1; var m = P2; 
 #line 233 "../../../Content/Content\CNV3/Basics.mc"
var tmp_2 = eval.StaticRun(expr, m);
 #line 233 "../../../Content/Content\CNV3/Basics.mc"

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollart; 
 #line 233 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_3 != null) { 
 var t = tmp_3.P1; 
 #line 233 "../../../Content/Content\CNV3/Basics.mc"
var tmp_5 = fst<Value, Value>.StaticRun(t);
 #line 233 "../../../Content/Content\CNV3/Basics.mc"

var tmp_4 = tmp_5;

var res = tmp_4; 
 #line 233 "../../../Content/Content\CNV3/Basics.mc"
var result = res;
 #line 233 "../../../Content/Content\CNV3/Basics.mc"
 return result;  } }
 } 

  
 { 
 #line 238 "../../../Content/Content\CNV3/Basics.mc"
var tmp_0 = P1 as _opDollarsecond; 
 #line 238 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var expr = tmp_0.P1; var m = P2; 
 #line 238 "../../../Content/Content\CNV3/Basics.mc"
var tmp_2 = eval.StaticRun(expr, m);
 #line 238 "../../../Content/Content\CNV3/Basics.mc"

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollart; 
 #line 238 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_3 != null) { 
 var t = tmp_3.P1; 
 #line 238 "../../../Content/Content\CNV3/Basics.mc"
var tmp_5 = snd<Value, Value>.StaticRun(t);
 #line 238 "../../../Content/Content\CNV3/Basics.mc"

var tmp_4 = tmp_5;

var res = tmp_4; 
 #line 238 "../../../Content/Content\CNV3/Basics.mc"
var result = res;
 #line 238 "../../../Content/Content\CNV3/Basics.mc"
 return result;  } }
 } 

  
 { 
 #line 243 "../../../Content/Content\CNV3/Basics.mc"
var tmp_0 = P1 as vectorx; 
 #line 243 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var v = tmp_0.P1; var m = P2; 
 #line 243 "../../../Content/Content\CNV3/Basics.mc"
var tmp_2 = eval.StaticRun(v, m);
 #line 243 "../../../Content/Content\CNV3/Basics.mc"

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollarVector3; 
 #line 243 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_3 != null) { 
 var v1 = tmp_3.P1; var res = (v1.x); 
 #line 243 "../../../Content/Content\CNV3/Basics.mc"
var result = _opDollarf.Create(res);
 #line 243 "../../../Content/Content\CNV3/Basics.mc"
 return result;  } }
 } 

  
 { 
 #line 248 "../../../Content/Content\CNV3/Basics.mc"
var tmp_0 = P1 as vectory; 
 #line 248 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var v = tmp_0.P1; var m = P2; 
 #line 248 "../../../Content/Content\CNV3/Basics.mc"
var tmp_2 = eval.StaticRun(v, m);
 #line 248 "../../../Content/Content\CNV3/Basics.mc"

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollarVector3; 
 #line 248 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_3 != null) { 
 var v1 = tmp_3.P1; var res = (v1.y); 
 #line 248 "../../../Content/Content\CNV3/Basics.mc"
var result = _opDollarf.Create(res);
 #line 248 "../../../Content/Content\CNV3/Basics.mc"
 return result;  } }
 } 

  
 { 
 #line 253 "../../../Content/Content\CNV3/Basics.mc"
var tmp_0 = P1 as vectorz; 
 #line 253 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var v = tmp_0.P1; var m = P2; 
 #line 253 "../../../Content/Content\CNV3/Basics.mc"
var tmp_2 = eval.StaticRun(v, m);
 #line 253 "../../../Content/Content\CNV3/Basics.mc"

var tmp_1 = tmp_2;

var tmp_3 = tmp_1 as _opDollarVector3; 
 #line 253 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_3 != null) { 
 var v1 = tmp_3.P1; var res = (v1.z); 
 #line 253 "../../../Content/Content\CNV3/Basics.mc"
var result = _opDollarf.Create(res);
 #line 253 "../../../Content/Content\CNV3/Basics.mc"
 return result;  } }
 } 

  
 { 
 #line 258 "../../../Content/Content\CNV3/Basics.mc"
var tmp_0 = P1 as _opDollarwrapper; 
 #line 258 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var x = tmp_0.P1; var m = P2; var v = (x.Position); 
 #line 258 "../../../Content/Content\CNV3/Basics.mc"
var result = _opDollarVector3.Create(v);
 #line 258 "../../../Content/Content\CNV3/Basics.mc"
 return result;  }
 } 

  
 { 
 #line 262 "../../../Content/Content\CNV3/Basics.mc"
var tmp_0 = P1 as _opDollarwrapperSet; 
 #line 262 "../../../Content/Content\CNV3/Basics.mc"
if (tmp_0 != null) { 
 var x = tmp_0.P1; var v = tmp_0.P2; var m = P2; 
 #line 262 "../../../Content/Content\CNV3/Basics.mc"
var result = _opDollarwrapperSet.Create(x, v);
 #line 262 "../../../Content/Content\CNV3/Basics.mc"
 return result;  }
 } 

  
throw new System.Exception("Error evaluating: eval. No result returned."); }
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

 res += " fst "; res += P1.ToString(); 

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
res += P1.ToString(); 

 res += " geq "; res += P2.ToString(); 

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
res += P1.ToString(); 

 res += " gt "; res += P2.ToString(); 

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
res += P1.ToString(); 

 res += " leq "; res += P2.ToString(); 

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
res += P1.ToString(); 

 res += " lt "; res += P2.ToString(); 

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

 res += " snd "; res += P1.ToString(); 

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
 #line 267 "../../../Content/Content\CNV3/Basics.mc"
var vx = (new Vector3(1.000000f,-3.000000f,0.000000f)); var p = (new Vector3(0.000000f,1.000000f,0.000000f)); var wt = (WrapperTest.Instantiate(p)); var m = Context.Create(ImmutableDictionary<string,Value>.Empty, ImmutableDictionary<string,Value>.Empty, ImmutableDictionary<string,Value>.Empty); 
 #line 267 "../../../Content/Content\CNV3/Basics.mc"
var tmp_1 = eval.StaticRun(_opAddition.Create(_opDollarwrapper.Create(wt), _opDollarVector3.Create(vx)), m);
 #line 267 "../../../Content/Content\CNV3/Basics.mc"

var tmp_0 = tmp_1;

var res = tmp_0; 
 #line 267 "../../../Content/Content\CNV3/Basics.mc"
var result = res;
 #line 267 "../../../Content/Content\CNV3/Basics.mc"
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

public class vectorx : Expr  {
public Expr P1;

public vectorx(Expr P1) {this.P1 = P1;}
public static vectorx Create(Expr P1) { return new vectorx(P1); }

public override string ToString() {
 var res = "("; 

 res += " vectorx "; res += P1.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as vectorx;
 if(tmp != null) return this.P1.Equals(tmp.P1); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class vectory : Expr  {
public Expr P1;

public vectory(Expr P1) {this.P1 = P1;}
public static vectory Create(Expr P1) { return new vectory(P1); }

public override string ToString() {
 var res = "("; 

 res += " vectory "; res += P1.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as vectory;
 if(tmp != null) return this.P1.Equals(tmp.P1); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class vectorz : Expr  {
public Expr P1;

public vectorz(Expr P1) {this.P1 = P1;}
public static vectorz Create(Expr P1) { return new vectorz(P1); }

public override string ToString() {
 var res = "("; 

 res += " vectorz "; res += P1.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as vectorz;
 if(tmp != null) return this.P1.Equals(tmp.P1); 
 else return false; }

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
