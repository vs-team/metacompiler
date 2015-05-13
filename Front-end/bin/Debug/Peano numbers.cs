using System.Collections.Generic;
using System.Linq;
namespace Peano_numbers {
 public static class Extensions { public static V GetKey<T, V>(this System.Collections.Immutable.ImmutableDictionary<T, V> self, T key) { return self[key]; } }
 public interface IRunnable { IEnumerable<IRunnable> Run();
 }


public interface IntValue : Expr {}
public interface Num : Expr {}
public interface Expr : IRunnable {}



public class _opDollar : IntValue  {
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

public class _opMultiplication : Expr , Num  {
public Expr P1;
public Expr P2;

public _opMultiplication(Expr P1, Expr P2) {this.P1 = P1; this.P2 = P2;}
public static _opMultiplication Create(Expr P1, Expr P2) { return new _opMultiplication(P1, P2); }

  public IEnumerable<IRunnable> Run() {   
 { 
 var tmp_0 = this; var tmp_1 = tmp_0.P1 as z; 
if (tmp_1 != null) { var a = tmp_0.P2; 
var result = z.Create();
yield return result;  }
 } 

  
 { 
 var tmp_0 = this; var tmp_1 = tmp_0.P1 as s; 
if (tmp_1 != null) { var a = tmp_1.P1; var b = tmp_0.P2; 
if(a is Expr && b is Expr) { 
var tmp_3 = _opMultiplication.Create(a as Expr, b as Expr);
foreach (var tmp_2 in tmp_3.Run()) { var c = tmp_2; 
if(c is Expr && b is Expr) { 
var tmp_5 = _opAddition.Create(c as Expr, b as Expr);
foreach (var tmp_4 in tmp_5.Run()) { var d = tmp_4; 
var result = d;
yield return result;  } } } } }
 } 

  }


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

public class _opAddition : Expr , Num  {
public Expr P1;
public Expr P2;

public _opAddition(Expr P1, Expr P2) {this.P1 = P1; this.P2 = P2;}
public static _opAddition Create(Expr P1, Expr P2) { return new _opAddition(P1, P2); }

  public IEnumerable<IRunnable> Run() {   
 { 
 var tmp_0 = this; var tmp_1 = tmp_0.P1 as z; 
if (tmp_1 != null) { var a = tmp_0.P2; 
var result = a;
yield return result;  }
 } 

  
 { 
 var tmp_0 = this; var tmp_1 = tmp_0.P1 as s; 
if (tmp_1 != null) { var a = tmp_1.P1; var b = tmp_0.P2; 
if(a is Expr && b is Expr) { 
var tmp_3 = _opAddition.Create(a as Expr, b as Expr);
foreach (var tmp_2 in tmp_3.Run()) { var c = tmp_2; 
if(c is Num) { 
var result = s.Create(c as Num);
yield return result;  } } } }
 } 

  }


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

public class eval : Expr , Num  {
public Expr P1;

public eval(Expr P1) {this.P1 = P1;}
public static eval Create(Expr P1) { return new eval(P1); }

  public IEnumerable<IRunnable> Run() {   
 { 
 var tmp_0 = this; var tmp_1 = tmp_0.P1 as z; 
if (tmp_1 != null) { 
var result = z.Create();
yield return result;  }
 } 

  
 { 
 var tmp_0 = this; var tmp_1 = tmp_0.P1 as s; 
if (tmp_1 != null) { var a = tmp_1.P1; 
if(a is Num) { 
var result = s.Create(a as Num);
yield return result;  } }
 } 

  
 { 
 var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opAddition; 
if (tmp_1 != null) { var a = tmp_1.P1; var b = tmp_1.P2; 
if(a is Expr) { 
var tmp_3 = eval.Create(a as Expr);
foreach (var tmp_2 in tmp_3.Run()) { var a_Prime = tmp_2; 
if(b is Expr) { 
var tmp_5 = eval.Create(b as Expr);
foreach (var tmp_4 in tmp_5.Run()) { var b_Prime = tmp_4; 
if(a_Prime is Expr && b_Prime is Expr) { 
var tmp_7 = _opAddition.Create(a_Prime as Expr, b_Prime as Expr);
foreach (var tmp_6 in tmp_7.Run()) { var c = tmp_6; 
var result = c;
yield return result;  } } } } } } }
 } 

  
 { 
 var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opMultiplication; 
if (tmp_1 != null) { var a = tmp_1.P1; var b = tmp_1.P2; 
if(a is Expr) { 
var tmp_3 = eval.Create(a as Expr);
foreach (var tmp_2 in tmp_3.Run()) { var a_Prime = tmp_2; 
if(b is Expr) { 
var tmp_5 = eval.Create(b as Expr);
foreach (var tmp_4 in tmp_5.Run()) { var b_Prime = tmp_4; 
if(a_Prime is Expr && b_Prime is Expr) { 
var tmp_7 = _opMultiplication.Create(a_Prime as Expr, b_Prime as Expr);
foreach (var tmp_6 in tmp_7.Run()) { var c = tmp_6; 
var result = c;
yield return result;  } } } } } } }
 } 

  }


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

  public IEnumerable<IRunnable> Run() {   
 { 
 var tmp_0 = this as run; 
var tmp_2 = eval.Create(_opAddition.Create(_opMultiplication.Create(s.Create(s.Create(z.Create())), _opMultiplication.Create(s.Create(s.Create(z.Create())), s.Create(s.Create(z.Create())))), s.Create(z.Create())));
foreach (var tmp_1 in tmp_2.Run()) { var n = tmp_1; 
if(n is Num) { 
var tmp_4 = toNum.Create(n as Num);
foreach (var tmp_3 in tmp_4.Run()) { var res = tmp_3; 
var result = res;
yield return result;  } } }
 } 

  }


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


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }

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

public class toNum : IntValue  {
public Num P1;

public toNum(Num P1) {this.P1 = P1;}
public static toNum Create(Num P1) { return new toNum(P1); }

  public IEnumerable<IRunnable> Run() {   
 { 
 var tmp_0 = this; var tmp_1 = tmp_0.P1 as s; 
if (tmp_1 != null) { var a = tmp_1.P1; 
if(a is Num) { 
var tmp_3 = toNum.Create(a as Num);
foreach (var tmp_2 in tmp_3.Run()) { var tmp_4 = tmp_2 as _opDollar; 
if (tmp_4 != null) { var res = tmp_4.P1; 
var result = _opDollar.Create(res+1);
yield return result;  } } } }
 } 

  
 { 
 var tmp_0 = this; var tmp_1 = tmp_0.P1 as z; 
if (tmp_1 != null) { 
var result = _opDollar.Create(0);
yield return result;  }
 } 

  }


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


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }

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
static public IEnumerable<IRunnable> Run(bool printInput)
{
 #line 1 "input"
 var p = run.Create();
if(printInput) System.Console.WriteLine(p.ToString());
foreach(var x in p.Run())
yield return x;
}
}

}
