using System.Collections.Generic;
using System.Linq;
namespace Peano_numbers {
 public static class Extensions { public static V GetKey<T, V>(this System.Collections.Immutable.ImmutableDictionary<T, V> self, T key) { return self[key]; } }
 public interface IRunnable { IEnumerable<IRunnable> Run();
 }


public interface Expr : IRunnable {}
public interface Num : Expr {}



public class _opBang : Expr {
public Expr P1;

public _opBang(Expr P1) {this.P1 = P1;}

  public IEnumerable<IRunnable> Run() {   
 { 
 #line 28 ".\Content\Peano numbers\transform.mc"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as z; 
 #line 28 ".\Content\Peano numbers\transform.mc"
if (tmp_1 != null) { 
 #line 28 ".\Content\Peano numbers\transform.mc"
var result = new z();
 #line 28 ".\Content\Peano numbers\transform.mc"
yield return result;  }
 } 

  
 { 
 #line 31 ".\Content\Peano numbers\transform.mc"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as s; 
 #line 31 ".\Content\Peano numbers\transform.mc"
if (tmp_1 != null) { var a = tmp_1.P1; 
 #line 31 ".\Content\Peano numbers\transform.mc"
if(a is Num) { 
 #line 31 ".\Content\Peano numbers\transform.mc"
var result = new s(a as Num);
 #line 31 ".\Content\Peano numbers\transform.mc"
yield return result;  } }
 } 

  
 { 
 #line 34 ".\Content\Peano numbers\transform.mc"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opAddition; 
 #line 34 ".\Content\Peano numbers\transform.mc"
if (tmp_1 != null) { var a = tmp_1.P1; var b = tmp_1.P2; 
 #line 34 ".\Content\Peano numbers\transform.mc"
if(a is Expr) { 
 #line 34 ".\Content\Peano numbers\transform.mc"
var tmp_3 = new _opBang(a as Expr);
 #line 34 ".\Content\Peano numbers\transform.mc"
foreach (var tmp_2 in tmp_3.Run()) { var a_Prime = tmp_2; 
 #line 34 ".\Content\Peano numbers\transform.mc"
if(b is Expr) { 
 #line 34 ".\Content\Peano numbers\transform.mc"
var tmp_5 = new _opBang(b as Expr);
 #line 34 ".\Content\Peano numbers\transform.mc"
foreach (var tmp_4 in tmp_5.Run()) { var b_Prime = tmp_4; 
 #line 34 ".\Content\Peano numbers\transform.mc"
if(a_Prime is Expr && b_Prime is Expr) { 
 #line 34 ".\Content\Peano numbers\transform.mc"
var tmp_7 = new _opAddition(a_Prime as Expr, b_Prime as Expr);
 #line 34 ".\Content\Peano numbers\transform.mc"
foreach (var tmp_6 in tmp_7.Run()) { var c = tmp_6; 
 #line 34 ".\Content\Peano numbers\transform.mc"
var result = c;
 #line 34 ".\Content\Peano numbers\transform.mc"
yield return result;  } } } } } } }
 } 

  
 { 
 #line 40 ".\Content\Peano numbers\transform.mc"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opMultiplication; 
 #line 40 ".\Content\Peano numbers\transform.mc"
if (tmp_1 != null) { var a = tmp_1.P1; var b = tmp_1.P2; 
 #line 40 ".\Content\Peano numbers\transform.mc"
if(a is Expr) { 
 #line 40 ".\Content\Peano numbers\transform.mc"
var tmp_3 = new _opBang(a as Expr);
 #line 40 ".\Content\Peano numbers\transform.mc"
foreach (var tmp_2 in tmp_3.Run()) { var a_Prime = tmp_2; 
 #line 40 ".\Content\Peano numbers\transform.mc"
if(b is Expr) { 
 #line 40 ".\Content\Peano numbers\transform.mc"
var tmp_5 = new _opBang(b as Expr);
 #line 40 ".\Content\Peano numbers\transform.mc"
foreach (var tmp_4 in tmp_5.Run()) { var b_Prime = tmp_4; 
 #line 40 ".\Content\Peano numbers\transform.mc"
if(a_Prime is Expr && b_Prime is Expr) { 
 #line 40 ".\Content\Peano numbers\transform.mc"
var tmp_7 = new _opMultiplication(a_Prime as Expr, b_Prime as Expr);
 #line 40 ".\Content\Peano numbers\transform.mc"
foreach (var tmp_6 in tmp_7.Run()) { var c = tmp_6; 
 #line 40 ".\Content\Peano numbers\transform.mc"
var result = c;
 #line 40 ".\Content\Peano numbers\transform.mc"
yield return result;  } } } } } } }
 } 

  }


public override string ToString() {
 var res = "("; 

 res += "!"; res += P1.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _opBang;
 if(tmp != null) return this.P1.Equals(tmp.P1); 
 else return false; }
}

public class _opMultiplication : Expr {
public Expr P1;
public Expr P2;

public _opMultiplication(Expr P1, Expr P2) {this.P1 = P1; this.P2 = P2;}

  public IEnumerable<IRunnable> Run() {   
 { 
 #line 19 ".\Content\Peano numbers\transform.mc"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as z; 
 #line 19 ".\Content\Peano numbers\transform.mc"
if (tmp_1 != null) { var a = tmp_0.P2; 
 #line 19 ".\Content\Peano numbers\transform.mc"
var result = new z();
 #line 19 ".\Content\Peano numbers\transform.mc"
yield return result;  }
 } 

  
 { 
 #line 22 ".\Content\Peano numbers\transform.mc"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as s; 
 #line 22 ".\Content\Peano numbers\transform.mc"
if (tmp_1 != null) { var a = tmp_1.P1; var b = tmp_0.P2; 
 #line 22 ".\Content\Peano numbers\transform.mc"
if(a is Expr && b is Expr) { 
 #line 22 ".\Content\Peano numbers\transform.mc"
var tmp_3 = new _opMultiplication(a as Expr, b as Expr);
 #line 22 ".\Content\Peano numbers\transform.mc"
foreach (var tmp_2 in tmp_3.Run()) { var c = tmp_2; 
 #line 22 ".\Content\Peano numbers\transform.mc"
if(c is Expr && b is Expr) { 
 #line 22 ".\Content\Peano numbers\transform.mc"
var tmp_5 = new _opAddition(c as Expr, b as Expr);
 #line 22 ".\Content\Peano numbers\transform.mc"
foreach (var tmp_4 in tmp_5.Run()) { var d = tmp_4; 
 #line 22 ".\Content\Peano numbers\transform.mc"
var result = d;
 #line 22 ".\Content\Peano numbers\transform.mc"
yield return result;  } } } } }
 } 

  }


public override string ToString() {
 var res = "("; 
res += P1.ToString(); 

 res += "*"; res += P2.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _opMultiplication;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
 else return false; }
}

public class _opAddition : Expr {
public Expr P1;
public Expr P2;

public _opAddition(Expr P1, Expr P2) {this.P1 = P1; this.P2 = P2;}

  public IEnumerable<IRunnable> Run() {   
 { 
 #line 12 ".\Content\Peano numbers\transform.mc"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as z; 
 #line 12 ".\Content\Peano numbers\transform.mc"
if (tmp_1 != null) { var a = tmp_0.P2; 
 #line 12 ".\Content\Peano numbers\transform.mc"
var result = a;
 #line 12 ".\Content\Peano numbers\transform.mc"
yield return result;  }
 } 

  
 { 
 #line 15 ".\Content\Peano numbers\transform.mc"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as s; 
 #line 15 ".\Content\Peano numbers\transform.mc"
if (tmp_1 != null) { var a = tmp_1.P1; var b = tmp_0.P2; 
 #line 15 ".\Content\Peano numbers\transform.mc"
if(a is Expr && b is Expr) { 
 #line 15 ".\Content\Peano numbers\transform.mc"
var tmp_3 = new _opAddition(a as Expr, b as Expr);
 #line 15 ".\Content\Peano numbers\transform.mc"
foreach (var tmp_2 in tmp_3.Run()) { var c = tmp_2; 
 #line 15 ".\Content\Peano numbers\transform.mc"
if(c is Num) { 
 #line 15 ".\Content\Peano numbers\transform.mc"
var result = new s(c as Num);
 #line 15 ".\Content\Peano numbers\transform.mc"
yield return result;  } } } }
 } 

  }


public override string ToString() {
 var res = "("; 
res += P1.ToString(); 

 res += "+"; res += P2.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _opAddition;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
 else return false; }
}

public class s : Num {
public Num P1;

public s(Num P1) {this.P1 = P1;}


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }

public override string ToString() {
 var res = "("; 

 res += "s"; res += P1.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as s;
 if(tmp != null) return this.P1.Equals(tmp.P1); 
 else return false; }
}

public class z : Num {

public z() {}


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }

public override string ToString() {
return "z";
}

public override bool Equals(object other) {
 return other is z; 
}
}




public class EntryPoint {
 static public IEnumerable<IRunnable> Run(bool printInput)
{
var p = new _opBang(new _opMultiplication(new _opMultiplication(new s(new s(new z())), new s(new s(new z()))), new _opAddition(new s(new s(new z())), new s(new z()))));
if(printInput) System.Console.WriteLine(p.ToString());
foreach(var x in p.Run())
yield return x;
}
}

}
