using System.Collections.Generic;
using System.Linq;
namespace Lambda_calculus {
 public static class Extensions { public static V GetKey<T, V>(this System.Collections.Immutable.ImmutableDictionary<T, V> self, T key) { return self[key]; } }



public interface Id : Term {}
public interface Term {}
public interface Dot {}
public interface Where {}
public interface With {}
public interface Expr {}



public class _opDollar : Id  {
public string P1;

public _opDollar(string P1) {this.P1 = P1;}
public static _opDollar Create(string P1) { return new _opDollar(P1); }

  public static Term StaticRun(string P1) {    
 { 
 #line 18 "Content\Lambda calculus\transform.mc"
var x = P1; 
 #line 18 "Content\Lambda calculus\transform.mc"
var result = _opDollar.Create(x);
 #line 18 "Content\Lambda calculus\transform.mc"
 return result; 
 } 

  
throw new System.Exception("Error evaluating: " + new _opDollar(P1).ToString() + " no result returned."); }
public Term Run() { return StaticRun(P1); }

public static Term StaticRun5_(string P1) { return StaticRun(P1); }
public Term Run5_(){ return StaticRun5_(P1); }

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

public class _Arrow : Dot  {

public _Arrow() {}
public static _Arrow Create() { return new _Arrow(); }

public override string ToString() {
return "->";
}

public override bool Equals(object other) {
 return other is _Arrow; 
}

public override int GetHashCode() {
 return 0; 
}

}

public class _opSlash : Term  {
public Id P1;
public Dot P2;
public Term P3;

public _opSlash(Id P1, Dot P2, Term P3) {this.P1 = P1; this.P2 = P2; this.P3 = P3;}
public static _opSlash Create(Id P1, Dot P2, Term P3) { return new _opSlash(P1, P2, P3); }

  public static Term StaticRun(Id P1, Dot P2, Term P3) {    
 { 
 #line 21 "Content\Lambda calculus\transform.mc"
var tmp_0 = P1 as _opDollar; 
 #line 21 "Content\Lambda calculus\transform.mc"
if (tmp_0 != null) { 
 var x = tmp_0.P1; var tmp_1 = P2 as _Arrow; 
 #line 21 "Content\Lambda calculus\transform.mc"
if (tmp_1 != null) { 
 var t = P3; 
 #line 21 "Content\Lambda calculus\transform.mc"
var result = _opSlash.Create(_opDollar.Create(x), _Arrow.Create(), t);
 #line 21 "Content\Lambda calculus\transform.mc"
 return result;  } }
 } 

  
throw new System.Exception("Error evaluating: " + new _opSlash(P1, P2, P3).ToString() + " no result returned."); }
public Term Run() { return StaticRun(P1, P2, P3); }

public static Term StaticRun5_(Id P1, Dot P2, Term P3) { return StaticRun(P1, P2, P3); }
public Term Run5_(){ return StaticRun5_(P1, P2, P3); }

public override string ToString() {
 var res = "("; 

 res += " \\ "; if (P1 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P1.ToString(); } 
if (P2 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P2 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P2.ToString(); } 
if (P3 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P3 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P3.ToString(); } 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _opSlash;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2) && this.P3.Equals(tmp.P3); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class _As : Where  {
public Term P1;
public Term P2;

public _As(Term P1, Term P2) {this.P1 = P1; this.P2 = P2;}
public static _As Create(Term P1, Term P2) { return new _As(P1, P2); }

public override string ToString() {
 var res = "("; 
if (P1 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P1.ToString(); } 

 res += " as "; if (P2 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P2 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P2.ToString(); } 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _As;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class run : Expr  {

public run() {}
public static run Create() { return new run(); }

  public static Term StaticRun() {    
 { 
 #line 14 "Content\Lambda calculus\transform.mc"

 #line 14 "Content\Lambda calculus\transform.mc"
var tmp_1 = _opBitwiseOr.Create(_opSlash.Create(_opDollar.Create("x"), _Arrow.Create(), _opDollar.Create("x")), _opDollar.Create("y"));
 #line 14 "Content\Lambda calculus\transform.mc"

var tmp_0 = tmp_1.Run();

var res = tmp_0; 
 #line 14 "Content\Lambda calculus\transform.mc"
var result = res;
 #line 14 "Content\Lambda calculus\transform.mc"
 return result; 
 } 

  
throw new System.Exception("Error evaluating: " + new run().ToString() + " no result returned."); }
public Term Run() { return StaticRun(); }

public static Term StaticRun5_() { return StaticRun(); }
public Term Run5_(){ return StaticRun5_(); }

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

public class _With : With  {
public Term P1;
public Where P2;

public _With(Term P1, Where P2) {this.P1 = P1; this.P2 = P2;}
public static _With Create(Term P1, Where P2) { return new _With(P1, P2); }

  public static Term StaticRun5_(Term P1, Where P2) {    
 { 
 #line 36 "Content\Lambda calculus\transform.mc"
var tmp_0 = P1 as _opDollar; 
 #line 36 "Content\Lambda calculus\transform.mc"
if (tmp_0 != null) { 
 var y = tmp_0.P1; var tmp_1 = P2 as _As; 
 #line 36 "Content\Lambda calculus\transform.mc"
if (tmp_1 != null) { 
 var tmp_2 = tmp_1.P1 as _opDollar; 
 #line 36 "Content\Lambda calculus\transform.mc"
if (tmp_2 != null) { 
 var x = tmp_2.P1; var u = tmp_1.P2; 
 #line 36 "Content\Lambda calculus\transform.mc"
if(x.Equals(y)) { 
 #line 36 "Content\Lambda calculus\transform.mc"
var result = u;
 #line 36 "Content\Lambda calculus\transform.mc"
 return result;  } } } }
 } 

  
 { 
 #line 40 "Content\Lambda calculus\transform.mc"
var tmp_0 = P1 as _opDollar; 
 #line 40 "Content\Lambda calculus\transform.mc"
if (tmp_0 != null) { 
 var y = tmp_0.P1; var tmp_1 = P2 as _As; 
 #line 40 "Content\Lambda calculus\transform.mc"
if (tmp_1 != null) { 
 var tmp_2 = tmp_1.P1 as _opDollar; 
 #line 40 "Content\Lambda calculus\transform.mc"
if (tmp_2 != null) { 
 var x = tmp_2.P1; var u = tmp_1.P2; 
 #line 40 "Content\Lambda calculus\transform.mc"
if(!x.Equals(y)) { 
 #line 40 "Content\Lambda calculus\transform.mc"
var result = _opDollar.Create(y);
 #line 40 "Content\Lambda calculus\transform.mc"
 return result;  } } } }
 } 

  
 { 
 #line 44 "Content\Lambda calculus\transform.mc"
var tmp_0 = P1 as _opSlash; 
 #line 44 "Content\Lambda calculus\transform.mc"
if (tmp_0 != null) { 
 var tmp_1 = tmp_0.P1 as _opDollar; 
 #line 44 "Content\Lambda calculus\transform.mc"
if (tmp_1 != null) { 
 var x = tmp_1.P1; var tmp_2 = tmp_0.P2 as _Arrow; 
 #line 44 "Content\Lambda calculus\transform.mc"
if (tmp_2 != null) { 
 var t = tmp_0.P3; var tmp_3 = P2 as _As; 
 #line 44 "Content\Lambda calculus\transform.mc"
if (tmp_3 != null) { 
 var tmp_4 = tmp_3.P1 as _opDollar; 
 #line 44 "Content\Lambda calculus\transform.mc"
if (tmp_4 != null) { 
 var y = tmp_4.P1; var u = tmp_3.P2; 
 #line 44 "Content\Lambda calculus\transform.mc"
if(x.Equals(y)) { 
 #line 44 "Content\Lambda calculus\transform.mc"
var result = _opSlash.Create(_opDollar.Create(x), _Arrow.Create(), t);
 #line 44 "Content\Lambda calculus\transform.mc"
 return result;  } } } } } }
 } 

  
 { 
 #line 48 "Content\Lambda calculus\transform.mc"
var tmp_0 = P1 as _opSlash; 
 #line 48 "Content\Lambda calculus\transform.mc"
if (tmp_0 != null) { 
 var tmp_1 = tmp_0.P1 as _opDollar; 
 #line 48 "Content\Lambda calculus\transform.mc"
if (tmp_1 != null) { 
 var x = tmp_1.P1; var tmp_2 = tmp_0.P2 as _Arrow; 
 #line 48 "Content\Lambda calculus\transform.mc"
if (tmp_2 != null) { 
 var t = tmp_0.P3; var tmp_3 = P2 as _As; 
 #line 48 "Content\Lambda calculus\transform.mc"
if (tmp_3 != null) { 
 var tmp_4 = tmp_3.P1 as _opDollar; 
 #line 48 "Content\Lambda calculus\transform.mc"
if (tmp_4 != null) { 
 var y = tmp_4.P1; var u = tmp_3.P2; 
 #line 48 "Content\Lambda calculus\transform.mc"
if(!x.Equals(y)) { 
 #line 48 "Content\Lambda calculus\transform.mc"
var tmp_6 = _With.Create(t, _As.Create(_opDollar.Create(y), u));
 #line 48 "Content\Lambda calculus\transform.mc"

var tmp_5 = tmp_6.Run5_();

var t_Prime = tmp_5; 
 #line 48 "Content\Lambda calculus\transform.mc"
var result = _opSlash.Create(_opDollar.Create(x), _Arrow.Create(), t_Prime);
 #line 48 "Content\Lambda calculus\transform.mc"
 return result;  } } } } } }
 } 

  
 { 
 #line 53 "Content\Lambda calculus\transform.mc"
var tmp_0 = P1 as _opBitwiseOr; 
 #line 53 "Content\Lambda calculus\transform.mc"
if (tmp_0 != null) { 
 var t = tmp_0.P1; var u = tmp_0.P2; var tmp_1 = P2 as _As; 
 #line 53 "Content\Lambda calculus\transform.mc"
if (tmp_1 != null) { 
 var tmp_2 = tmp_1.P1 as _opDollar; 
 #line 53 "Content\Lambda calculus\transform.mc"
if (tmp_2 != null) { 
 var x = tmp_2.P1; var v = tmp_1.P2; 
 #line 53 "Content\Lambda calculus\transform.mc"
var tmp_4 = _With.Create(t, _As.Create(_opDollar.Create(x), v));
 #line 53 "Content\Lambda calculus\transform.mc"

var tmp_3 = tmp_4.Run5_();

var t_Prime = tmp_3; 
 #line 53 "Content\Lambda calculus\transform.mc"
var tmp_6 = _With.Create(u, _As.Create(_opDollar.Create(x), v));
 #line 53 "Content\Lambda calculus\transform.mc"

var tmp_5 = tmp_6.Run5_();

var u_Prime = tmp_5; 
 #line 53 "Content\Lambda calculus\transform.mc"
var result = _opBitwiseOr.Create(t_Prime, u_Prime);
 #line 53 "Content\Lambda calculus\transform.mc"
 return result;  } } }
 } 

 var p = StaticRun(P1, P2); return p; 
throw new System.Exception("Error evaluating: " + new _With(P1, P2).ToString() + " no result returned."); }
public Term Run5_() { return StaticRun5_(P1, P2); }

public static Term StaticRun(Term P1, Where P2) { 
throw new System.Exception("Error evaluating: " + new _With(P1, P2).ToString() + " no result returned."); }
public Term Run(){ return StaticRun(P1, P2); }

public override string ToString() {
 var res = "("; 
if (P1 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P1.ToString(); } 

 res += " with "; if (P2 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P2 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P2.ToString(); } 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _With;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class _opBitwiseOr : Term  {
public Term P1;
public Term P2;

public _opBitwiseOr(Term P1, Term P2) {this.P1 = P1; this.P2 = P2;}
public static _opBitwiseOr Create(Term P1, Term P2) { return new _opBitwiseOr(P1, P2); }

  public static Term StaticRun(Term P1, Term P2) {    
 { 
 #line 24 "Content\Lambda calculus\transform.mc"
var tmp_0 = P1 as _opDollar; 
 #line 24 "Content\Lambda calculus\transform.mc"
if (tmp_0 != null) { 
 var x = tmp_0.P1; var u = P2; 
 #line 24 "Content\Lambda calculus\transform.mc"
var result = _opBitwiseOr.Create(_opDollar.Create(x), u);
 #line 24 "Content\Lambda calculus\transform.mc"
 return result;  }
 } 

  
 { 
 #line 27 "Content\Lambda calculus\transform.mc"
var tmp_0 = P1 as _opBitwiseOr; 
 #line 27 "Content\Lambda calculus\transform.mc"
if (tmp_0 != null) { 
 var u = tmp_0.P1; var v = tmp_0.P2; var w = P2; 
 #line 27 "Content\Lambda calculus\transform.mc"
var tmp_2 = _opBitwiseOr.Create(u, v);
 #line 27 "Content\Lambda calculus\transform.mc"

var tmp_1 = tmp_2.Run();

var v_Prime = tmp_1; 
 #line 27 "Content\Lambda calculus\transform.mc"
var tmp_4 = _opBitwiseOr.Create(v_Prime, w);
 #line 27 "Content\Lambda calculus\transform.mc"

var tmp_3 = tmp_4.Run();

var res = tmp_3; 
 #line 27 "Content\Lambda calculus\transform.mc"
var result = res;
 #line 27 "Content\Lambda calculus\transform.mc"
 return result;  }
 } 

  
 { 
 #line 32 "Content\Lambda calculus\transform.mc"
var tmp_0 = P1 as _opSlash; 
 #line 32 "Content\Lambda calculus\transform.mc"
if (tmp_0 != null) { 
 var tmp_1 = tmp_0.P1 as _opDollar; 
 #line 32 "Content\Lambda calculus\transform.mc"
if (tmp_1 != null) { 
 var x = tmp_1.P1; var tmp_2 = tmp_0.P2 as _Arrow; 
 #line 32 "Content\Lambda calculus\transform.mc"
if (tmp_2 != null) { 
 var t = tmp_0.P3; var u = P2; 
 #line 32 "Content\Lambda calculus\transform.mc"
var tmp_4 = _With.Create(t, _As.Create(_opDollar.Create(x), u));
 #line 32 "Content\Lambda calculus\transform.mc"

var tmp_3 = tmp_4.Run5_();

var t_Prime = tmp_3; 
 #line 32 "Content\Lambda calculus\transform.mc"
var result = t_Prime;
 #line 32 "Content\Lambda calculus\transform.mc"
 return result;  } } }
 } 

  
throw new System.Exception("Error evaluating: " + new _opBitwiseOr(P1, P2).ToString() + " no result returned."); }
public Term Run() { return StaticRun(P1, P2); }

public static Term StaticRun5_(Term P1, Term P2) { return StaticRun(P1, P2); }
public Term Run5_(){ return StaticRun5_(P1, P2); }

public override string ToString() {
 var res = "("; 
if (P1 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P1.ToString(); } 

 res += " | "; if (P2 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P2 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P2.ToString(); } 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _opBitwiseOr;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}




public class EntryPoint {
static public object Run(bool printInput)
{
 #line 1 "input"
 var p = run.Create();
if(printInput) System.Console.WriteLine(p.ToString());
 
 var result = p.Run(); 

return result;
}
}

}
