using System.Collections.Generic;
using System.Linq;
namespace Peano_numbers {
 public interface IRunnable { IEnumerable<IRunnable> Run();
 }


public interface Expr : IRunnable {}



public class _opMultiplication : Expr {
public Expr P1;
public Expr P2;

public _opMultiplication(Expr P1, Expr P2) {this.P1 = P1; this.P2 = P2;}

  public IEnumerable<IRunnable> Run() {   
 { 
 #line 23 ".\Content\Peano numbers\transform.mc"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as z; 
 #line 23 ".\Content\Peano numbers\transform.mc"
if (tmp_1 != null) { var a = tmp_0.P2; 
 #line 23 ".\Content\Peano numbers\transform.mc"
var result = new z();
 #line 23 ".\Content\Peano numbers\transform.mc"
yield return result;  }
 } 

  
 { 
 #line 26 ".\Content\Peano numbers\transform.mc"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as s; 
 #line 26 ".\Content\Peano numbers\transform.mc"
if (tmp_1 != null) { var a = tmp_1.P1; var b = tmp_0.P2; 
 #line 26 ".\Content\Peano numbers\transform.mc"
var tmp_3 = a;
 #line 26 ".\Content\Peano numbers\transform.mc"
foreach (var tmp_2 in tmp_3.Run()) { var a_Prime = tmp_2; 
 #line 26 ".\Content\Peano numbers\transform.mc"
var tmp_5 = b;
 #line 26 ".\Content\Peano numbers\transform.mc"
foreach (var tmp_4 in tmp_5.Run()) { var b_Prime = tmp_4; 
 #line 26 ".\Content\Peano numbers\transform.mc"
if(a_Prime is Expr && b_Prime is Expr) { 
 #line 26 ".\Content\Peano numbers\transform.mc"
var tmp_7 = new _opMultiplication(a_Prime as Expr, b_Prime as Expr);
 #line 26 ".\Content\Peano numbers\transform.mc"
foreach (var tmp_6 in tmp_7.Run()) { var c = tmp_6; 
 #line 26 ".\Content\Peano numbers\transform.mc"
var tmp_9 = c;
 #line 26 ".\Content\Peano numbers\transform.mc"
foreach (var tmp_8 in tmp_9.Run()) { var c_Prime = tmp_8; 
 #line 26 ".\Content\Peano numbers\transform.mc"
if(c_Prime is Expr && b_Prime is Expr) { 
 #line 26 ".\Content\Peano numbers\transform.mc"
var tmp_11 = new _opAddition(c_Prime as Expr, b_Prime as Expr);
 #line 26 ".\Content\Peano numbers\transform.mc"
foreach (var tmp_10 in tmp_11.Run()) { var d = tmp_10; 
 #line 26 ".\Content\Peano numbers\transform.mc"
var result = d;
 #line 26 ".\Content\Peano numbers\transform.mc"
yield return result;  } } } } } } } }
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
 #line 13 ".\Content\Peano numbers\transform.mc"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as z; 
 #line 13 ".\Content\Peano numbers\transform.mc"
if (tmp_1 != null) { var a = tmp_0.P2; 
 #line 13 ".\Content\Peano numbers\transform.mc"
var tmp_3 = a;
 #line 13 ".\Content\Peano numbers\transform.mc"
foreach (var tmp_2 in tmp_3.Run()) { var a_Prime = tmp_2; 
 #line 13 ".\Content\Peano numbers\transform.mc"
var result = a_Prime;
 #line 13 ".\Content\Peano numbers\transform.mc"
yield return result;  } }
 } 

  
 { 
 #line 17 ".\Content\Peano numbers\transform.mc"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as s; 
 #line 17 ".\Content\Peano numbers\transform.mc"
if (tmp_1 != null) { var a = tmp_1.P1; var b = tmp_0.P2; 
 #line 17 ".\Content\Peano numbers\transform.mc"
var tmp_3 = a;
 #line 17 ".\Content\Peano numbers\transform.mc"
foreach (var tmp_2 in tmp_3.Run()) { var a_Prime = tmp_2; 
 #line 17 ".\Content\Peano numbers\transform.mc"
var tmp_5 = b;
 #line 17 ".\Content\Peano numbers\transform.mc"
foreach (var tmp_4 in tmp_5.Run()) { var b_Prime = tmp_4; 
 #line 17 ".\Content\Peano numbers\transform.mc"
if(a_Prime is Expr && b_Prime is Expr) { 
 #line 17 ".\Content\Peano numbers\transform.mc"
var tmp_7 = new _opAddition(a_Prime as Expr, b_Prime as Expr);
 #line 17 ".\Content\Peano numbers\transform.mc"
foreach (var tmp_6 in tmp_7.Run()) { var c = tmp_6; 
 #line 17 ".\Content\Peano numbers\transform.mc"
if(c is Expr) { 
 #line 17 ".\Content\Peano numbers\transform.mc"
var result = new s(c as Expr);
 #line 17 ".\Content\Peano numbers\transform.mc"
yield return result;  } } } } } }
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

public class s : Expr {
public Expr P1;

public s(Expr P1) {this.P1 = P1;}

  public IEnumerable<IRunnable> Run() {   
 { 
 #line 9 ".\Content\Peano numbers\transform.mc"
var tmp_0 = this; var a = tmp_0.P1; 
 #line 9 ".\Content\Peano numbers\transform.mc"
var tmp_2 = a;
 #line 9 ".\Content\Peano numbers\transform.mc"
foreach (var tmp_1 in tmp_2.Run()) { var a_Prime = tmp_1; 
 #line 9 ".\Content\Peano numbers\transform.mc"
if(a_Prime is Expr) { 
 #line 9 ".\Content\Peano numbers\transform.mc"
var result = new s(a_Prime as Expr);
 #line 9 ".\Content\Peano numbers\transform.mc"
yield return result;  } }
 } 

  }


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

public class z : Expr {

public z() {}

  public IEnumerable<IRunnable> Run() {   
 { 
 #line 6 ".\Content\Peano numbers\transform.mc"
var tmp_0 = this as z; 
 #line 6 ".\Content\Peano numbers\transform.mc"
var result = new z();
 #line 6 ".\Content\Peano numbers\transform.mc"
yield return result; 
 } 

  }


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
var p = new _opMultiplication(new s(new s(new z())), new s(new s(new z())));
if(printInput) System.Console.WriteLine(p.ToString());
foreach(var x in p.Run())
yield return x;
}
}

}
