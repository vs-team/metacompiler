using System;
using System.Collections.Generic;
using System.Linq;
namespace Boolean_expressions {
 public static class Extensions { public static V GetKey<T, V>(this System.Collections.Immutable.ImmutableDictionary<T, V> self, T key) { return self[key]; } }



public interface Expr {}
public interface Value : Expr {}



public class _opBang : Expr  {
public Expr P1;

public _opBang(Expr P1) {this.P1 = P1;}
public static _opBang Create(Expr P1) { return new _opBang(P1); }

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

public class _opBitwiseAnd : Expr  {
public Expr P1;
public Expr P2;

public _opBitwiseAnd(Expr P1, Expr P2) {this.P1 = P1; this.P2 = P2;}
public static _opBitwiseAnd Create(Expr P1, Expr P2) { return new _opBitwiseAnd(P1, P2); }

public override string ToString() {
 var res = "("; 
if (P1 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P1.ToString(); } 

 res += " & "; if (P2 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P2 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P2.ToString(); } 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _opBitwiseAnd;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class FALSE : Value  {

public FALSE() {}
public static FALSE Create() { return new FALSE(); }

public override string ToString() {
return "FALSE";
}

public override bool Equals(object other) {
 return other is FALSE; 
}

public override int GetHashCode() {
 return 0; 
}

}

public class TRUE : Value  {

public TRUE() {}
public static TRUE Create() { return new TRUE(); }

public override string ToString() {
return "TRUE";
}

public override bool Equals(object other) {
 return other is TRUE; 
}

public override int GetHashCode() {
 return 0; 
}

}

public class eval : Expr  {
public Expr P1;

public eval(Expr P1) {this.P1 = P1;}
public static eval Create(Expr P1) { return new eval(P1); }

  public static Value StaticRun0_(Expr P1) {    
 { 
 #line 21 "Content\Boolean expressions\transform.mc"
var tmp_0 = P1 as TRUE; 
 #line 21 "Content\Boolean expressions\transform.mc"
if (tmp_0 != null) { 
 
 #line 21 "Content\Boolean expressions\transform.mc"
var result = TRUE.Create();
 #line 21 "Content\Boolean expressions\transform.mc"
 return result;  }
 } 

  
 { 
 #line 24 "Content\Boolean expressions\transform.mc"
var tmp_0 = P1 as FALSE; 
 #line 24 "Content\Boolean expressions\transform.mc"
if (tmp_0 != null) { 
 
 #line 24 "Content\Boolean expressions\transform.mc"
var result = FALSE.Create();
 #line 24 "Content\Boolean expressions\transform.mc"
 return result;  }
 } 

  
 { 
 #line 28 "Content\Boolean expressions\transform.mc"
var tmp_0 = P1 as _opBang; 
 #line 28 "Content\Boolean expressions\transform.mc"
if (tmp_0 != null) { 
 var a = tmp_0.P1; 
 #line 28 "Content\Boolean expressions\transform.mc"
var tmp_2 = eval.Create(a);
 #line 28 "Content\Boolean expressions\transform.mc"

var tmp_1 = tmp_2.Run0_();

var tmp_3 = tmp_1 as TRUE; 
 #line 28 "Content\Boolean expressions\transform.mc"
if (tmp_3 != null) { 
 
 #line 28 "Content\Boolean expressions\transform.mc"
var result = FALSE.Create();
 #line 28 "Content\Boolean expressions\transform.mc"
 return result;  } }
 } 

  
 { 
 #line 32 "Content\Boolean expressions\transform.mc"
var tmp_0 = P1 as _opBang; 
 #line 32 "Content\Boolean expressions\transform.mc"
if (tmp_0 != null) { 
 var a = tmp_0.P1; 
 #line 32 "Content\Boolean expressions\transform.mc"
var tmp_2 = eval.Create(a);
 #line 32 "Content\Boolean expressions\transform.mc"

var tmp_1 = tmp_2.Run0_();

var tmp_3 = tmp_1 as FALSE; 
 #line 32 "Content\Boolean expressions\transform.mc"
if (tmp_3 != null) { 
 
 #line 32 "Content\Boolean expressions\transform.mc"
var result = TRUE.Create();
 #line 32 "Content\Boolean expressions\transform.mc"
 return result;  } }
 } 

  
 { 
 #line 37 "Content\Boolean expressions\transform.mc"
var tmp_0 = P1 as _opBitwiseOr; 
 #line 37 "Content\Boolean expressions\transform.mc"
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; 
 #line 37 "Content\Boolean expressions\transform.mc"
var tmp_2 = eval.Create(a);
 #line 37 "Content\Boolean expressions\transform.mc"

var tmp_1 = tmp_2.Run0_();

var tmp_3 = tmp_1 as TRUE; 
 #line 37 "Content\Boolean expressions\transform.mc"
if (tmp_3 != null) { 
 
 #line 37 "Content\Boolean expressions\transform.mc"
var result = TRUE.Create();
 #line 37 "Content\Boolean expressions\transform.mc"
 return result;  } }
 } 

  
 { 
 #line 41 "Content\Boolean expressions\transform.mc"
var tmp_0 = P1 as _opBitwiseOr; 
 #line 41 "Content\Boolean expressions\transform.mc"
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; 
 #line 41 "Content\Boolean expressions\transform.mc"
var tmp_2 = eval.Create(a);
 #line 41 "Content\Boolean expressions\transform.mc"

var tmp_1 = tmp_2.Run0_();

var tmp_3 = tmp_1 as FALSE; 
 #line 41 "Content\Boolean expressions\transform.mc"
if (tmp_3 != null) { 
 
 #line 41 "Content\Boolean expressions\transform.mc"
var tmp_5 = eval.Create(b);
 #line 41 "Content\Boolean expressions\transform.mc"

var tmp_4 = tmp_5.Run0_();

var y = tmp_4; 
 #line 41 "Content\Boolean expressions\transform.mc"
var result = y;
 #line 41 "Content\Boolean expressions\transform.mc"
 return result;  } }
 } 

  
 { 
 #line 47 "Content\Boolean expressions\transform.mc"
var tmp_0 = P1 as _opBitwiseAnd; 
 #line 47 "Content\Boolean expressions\transform.mc"
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; 
 #line 47 "Content\Boolean expressions\transform.mc"
var tmp_2 = eval.Create(a);
 #line 47 "Content\Boolean expressions\transform.mc"

var tmp_1 = tmp_2.Run0_();

var tmp_3 = tmp_1 as FALSE; 
 #line 47 "Content\Boolean expressions\transform.mc"
if (tmp_3 != null) { 
 
 #line 47 "Content\Boolean expressions\transform.mc"
var result = FALSE.Create();
 #line 47 "Content\Boolean expressions\transform.mc"
 return result;  } }
 } 

  
 { 
 #line 51 "Content\Boolean expressions\transform.mc"
var tmp_0 = P1 as _opBitwiseAnd; 
 #line 51 "Content\Boolean expressions\transform.mc"
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; 
 #line 51 "Content\Boolean expressions\transform.mc"
var tmp_2 = eval.Create(a);
 #line 51 "Content\Boolean expressions\transform.mc"

var tmp_1 = tmp_2.Run0_();

var tmp_3 = tmp_1 as TRUE; 
 #line 51 "Content\Boolean expressions\transform.mc"
if (tmp_3 != null) { 
 
 #line 51 "Content\Boolean expressions\transform.mc"
var tmp_5 = eval.Create(b);
 #line 51 "Content\Boolean expressions\transform.mc"

var tmp_4 = tmp_5.Run0_();

var y = tmp_4; 
 #line 51 "Content\Boolean expressions\transform.mc"
var result = y;
 #line 51 "Content\Boolean expressions\transform.mc"
 return result;  } }
 } 

 var p = StaticRun(P1); return p; 
throw new System.Exception("Error evaluating: " + new eval(P1).ToString() + " no result returned."); }
public Value Run0_() { return StaticRun0_(P1); }

public static Value StaticRun(Expr P1) { 
throw new System.Exception("Error evaluating: " + new eval(P1).ToString() + " no result returned."); }
public Value Run(){ return StaticRun(P1); }

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

public class run : Expr  {

public run() {}
public static run Create() { return new run(); }

  public static Value StaticRun() {    
 { 
 #line 17 "Content\Boolean expressions\transform.mc"

 #line 17 "Content\Boolean expressions\transform.mc"
var tmp_1 = eval.Create(_opBitwiseOr.Create(FALSE.Create(), _opBitwiseAnd.Create(TRUE.Create(), _opBang.Create(FALSE.Create()))));
 #line 17 "Content\Boolean expressions\transform.mc"

var tmp_0 = tmp_1.Run0_();

var res = tmp_0; 
 #line 17 "Content\Boolean expressions\transform.mc"
var result = res;
 #line 17 "Content\Boolean expressions\transform.mc"
 return result; 
 } 

  
throw new System.Exception("Error evaluating: " + new run().ToString() + " no result returned."); }
public Value Run() { return StaticRun(); }

public static Value StaticRun0_() { return StaticRun(); }
public Value Run0_(){ return StaticRun0_(); }

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

public class _opBitwiseOr : Expr  {
public Expr P1;
public Expr P2;

public _opBitwiseOr(Expr P1, Expr P2) {this.P1 = P1; this.P2 = P2;}
public static _opBitwiseOr Create(Expr P1, Expr P2) { return new _opBitwiseOr(P1, P2); }

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
