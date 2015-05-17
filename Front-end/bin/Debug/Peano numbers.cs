using System.Collections.Generic;
using System.Linq;
namespace Peano_numbers {
 public static class Extensions { public static V GetKey<T, V>(this System.Collections.Immutable.ImmutableDictionary<T, V> self, T key) { return self[key]; } }



public interface IntValue : Expr {}
public interface Num : Expr {}
public interface Expr {}



public class _opDollar : IntValue  {
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

public class _opMultiplication : Expr,Num  {
public Expr P1;
public Expr P2;

public _opMultiplication(Expr P1, Expr P2) {this.P1 = P1; this.P2 = P2;}
public static _opMultiplication Create(Expr P1, Expr P2) { return new _opMultiplication(P1, P2); }

  public static IEnumerable<Num> StaticRun(Expr P1, Expr P2) {   
 { 
 var tmp_0 = P1 as z; 
if (tmp_0 != null) { var a = P2; 
var result = z.Create();
yield return result;  }
 } 

  
 { 
 var tmp_0 = P1 as s; 
if (tmp_0 != null) { var a = tmp_0.P1; var b = P2; 
var tmp_2 = _opMultiplication.Create(a, b);
foreach (var tmp_1 in tmp_2.Run()) { var c = tmp_1; 
var tmp_4 = _opAddition.Create(c, b);
foreach (var tmp_3 in tmp_4.Run()) { var d = tmp_3; 
var result = d;
yield return result;  } } }
 } 

  }
public IEnumerable<Num> Run() { return StaticRun(P1, P2); }


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

public class _opAddition : Expr,Num  {
public Expr P1;
public Expr P2;

public _opAddition(Expr P1, Expr P2) {this.P1 = P1; this.P2 = P2;}
public static _opAddition Create(Expr P1, Expr P2) { return new _opAddition(P1, P2); }

  public static IEnumerable<Num> StaticRun(Expr P1, Expr P2) {   
 { 
 var tmp_0 = P1 as z; 
if (tmp_0 != null) { var a = P2; 
var tmp_2 = eval.Create(a);
foreach (var tmp_1 in tmp_2.Run()) { var res = tmp_1; 
var result = res;
yield return result;  } }
 } 

  
 { 
 var tmp_0 = P1 as s; 
if (tmp_0 != null) { var a = tmp_0.P1; var b = P2; 
var tmp_2 = _opAddition.Create(a, b);
foreach (var tmp_1 in tmp_2.Run()) { var c = tmp_1; 
var result = s.Create(c);
yield return result;  } }
 } 

  }
public IEnumerable<Num> Run() { return StaticRun(P1, P2); }


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

public class eval : Expr,Num  {
public Expr P1;

public eval(Expr P1) {this.P1 = P1;}
public static eval Create(Expr P1) { return new eval(P1); }

  public static IEnumerable<Num> StaticRun(Expr P1) {   
 { 
 var tmp_0 = P1 as z; 
if (tmp_0 != null) { 
var result = z.Create();
yield return result;  }
 } 

  
 { 
 var tmp_0 = P1 as s; 
if (tmp_0 != null) { var a = tmp_0.P1; 
var result = s.Create(a);
yield return result;  }
 } 

  
 { 
 var tmp_0 = P1 as _opAddition; 
if (tmp_0 != null) { var a = tmp_0.P1; var b = tmp_0.P2; 
var tmp_2 = eval.Create(a);
foreach (var tmp_1 in tmp_2.Run()) { var a_Prime = tmp_1; 
var tmp_4 = eval.Create(b);
foreach (var tmp_3 in tmp_4.Run()) { var b_Prime = tmp_3; 
var tmp_6 = _opAddition.Create(a_Prime, b_Prime);
foreach (var tmp_5 in tmp_6.Run()) { var c = tmp_5; 
var result = c;
yield return result;  } } } }
 } 

  
 { 
 var tmp_0 = P1 as _opMultiplication; 
if (tmp_0 != null) { var a = tmp_0.P1; var b = tmp_0.P2; 
var tmp_2 = eval.Create(a);
foreach (var tmp_1 in tmp_2.Run()) { var a_Prime = tmp_1; 
var tmp_4 = eval.Create(b);
foreach (var tmp_3 in tmp_4.Run()) { var b_Prime = tmp_3; 
var tmp_6 = _opMultiplication.Create(a_Prime, b_Prime);
foreach (var tmp_5 in tmp_6.Run()) { var c = tmp_5; 
var result = c;
yield return result;  } } } }
 } 

  }
public IEnumerable<Num> Run() { return StaticRun(P1); }


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

public class run : Expr,IntValue  {

public run() {}
public static run Create() { return new run(); }

  public static IEnumerable<IntValue> StaticRun() {   
 { 
 
var tmp_1 = eval.Create(_opAddition.Create(_opMultiplication.Create(s.Create(s.Create(z.Create())), _opMultiplication.Create(s.Create(s.Create(z.Create())), s.Create(s.Create(z.Create())))), s.Create(z.Create())));
foreach (var tmp_0 in tmp_1.Run()) { var n = tmp_0; 
var tmp_3 = toNum.Create(n);
foreach (var tmp_2 in tmp_3.Run()) { var res = tmp_2; 
var result = res;
yield return result;  } }
 } 

  }
public IEnumerable<IntValue> Run() { return StaticRun(); }


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

 res += " s "; res += P1.ToString(); 

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

public class toNum : Expr,IntValue  {
public Num P1;

public toNum(Num P1) {this.P1 = P1;}
public static toNum Create(Num P1) { return new toNum(P1); }

  public static IEnumerable<IntValue> StaticRun(Num P1) {   
 { 
 var tmp_0 = P1 as s; 
if (tmp_0 != null) { var a = tmp_0.P1; 
var tmp_2 = toNum.Create(a);
foreach (var tmp_1 in tmp_2.Run()) { var tmp_3 = tmp_1 as _opDollar; 
if (tmp_3 != null) { var res = tmp_3.P1; 
var result = _opDollar.Create(res+1);
yield return result;  } } }
 } 

  
 { 
 var tmp_0 = P1 as z; 
if (tmp_0 != null) { 
var result = _opDollar.Create(0);
yield return result;  }
 } 

  }
public IEnumerable<IntValue> Run() { return StaticRun(P1); }


public override string ToString() {
 var res = "("; 

 res += " toNum "; res += P1.ToString(); 

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
 static public int Sleep(float s) { int t = (int)(s * 1000.0f); ; return 0; } 
static public IEnumerable<object> Run(bool printInput)
{
 #line 1 "input"
 var p = run.Create();
if(printInput) System.Console.WriteLine(p.ToString());
foreach(var x in p.Run())
yield return x;
}
}

}
