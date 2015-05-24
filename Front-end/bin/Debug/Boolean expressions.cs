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

 res += " ! "; res += P1.ToString(); 

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
res += P1.ToString(); 

 res += " & "; res += P2.ToString(); 

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
 var tmp_0 = P1 as TRUE; 
if (tmp_0 != null) { 
 
var result = TRUE.Create();
 return result;  }
 } 

  
 { 
 var tmp_0 = P1 as FALSE; 
if (tmp_0 != null) { 
 
var result = FALSE.Create();
 return result;  }
 } 

  
 { 
 var tmp_0 = P1 as _opBang; 
if (tmp_0 != null) { 
 var a = tmp_0.P1; 
var tmp_2 = eval.Create(a);

var tmp_1 = tmp_2.Run0_();

var tmp_3 = tmp_1 as TRUE; 
if (tmp_3 != null) { 
 
var result = FALSE.Create();
 return result;  } }
 } 

  
 { 
 var tmp_0 = P1 as _opBang; 
if (tmp_0 != null) { 
 var a = tmp_0.P1; 
var tmp_2 = eval.Create(a);

var tmp_1 = tmp_2.Run0_();

var tmp_3 = tmp_1 as FALSE; 
if (tmp_3 != null) { 
 
var result = TRUE.Create();
 return result;  } }
 } 

  
 { 
 var tmp_0 = P1 as _opBitwiseOr; 
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; 
var tmp_2 = eval.Create(a);

var tmp_1 = tmp_2.Run0_();

var tmp_3 = tmp_1 as TRUE; 
if (tmp_3 != null) { 
 
var result = TRUE.Create();
 return result;  } }
 } 

  
 { 
 var tmp_0 = P1 as _opBitwiseOr; 
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; 
var tmp_2 = eval.Create(a);

var tmp_1 = tmp_2.Run0_();

var tmp_3 = tmp_1 as FALSE; 
if (tmp_3 != null) { 
 
var tmp_5 = eval.Create(b);

var tmp_4 = tmp_5.Run0_();

var y = tmp_4; 
var result = y;
 return result;  } }
 } 

  
 { 
 var tmp_0 = P1 as _opBitwiseAnd; 
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; 
var tmp_2 = eval.Create(a);

var tmp_1 = tmp_2.Run0_();

var tmp_3 = tmp_1 as FALSE; 
if (tmp_3 != null) { 
 
var result = FALSE.Create();
 return result;  } }
 } 

  
 { 
 var tmp_0 = P1 as _opBitwiseAnd; 
if (tmp_0 != null) { 
 var a = tmp_0.P1; var b = tmp_0.P2; 
var tmp_2 = eval.Create(a);

var tmp_1 = tmp_2.Run0_();

var tmp_3 = tmp_1 as TRUE; 
if (tmp_3 != null) { 
 
var tmp_5 = eval.Create(b);

var tmp_4 = tmp_5.Run0_();

var y = tmp_4; 
var result = y;
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

 res += " eval "; res += P1.ToString(); 

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
 
var tmp_1 = eval.Create(_opBitwiseOr.Create(FALSE.Create(), _opBitwiseAnd.Create(TRUE.Create(), _opBang.Create(FALSE.Create()))));

var tmp_0 = tmp_1.Run0_();

var res = tmp_0; 
var result = res;
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
res += P1.ToString(); 

 res += " | "; res += P2.ToString(); 

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
