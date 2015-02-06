using System.Collections.Generic;
using System.Linq;
namespace PeanoNumbers {
 public interface IRunnable { IEnumerable<IRunnable> Run();
 }


public interface Expr : IRunnable {}



public class _opMultiplication : Expr {
public Expr P1;
public Expr P2;

public _opMultiplication(Expr P1, Expr P2) {this.P1 = P1; this.P2 = P2;}

  public IEnumerable<IRunnable> Run() {   {
var tmp_0 = this; var tmp_1 = tmp_0.P1 as z; 
if (tmp_1 != null) { var a = tmp_0.P2; 
var result = new z();
yield return result;  }
}
  {
var tmp_0 = this; var tmp_1 = tmp_0.P1 as s; 
if (tmp_1 != null) { var a = tmp_1.P1; var b = tmp_0.P2; 
var tmp_3 = a;
foreach (var tmp_2 in tmp_3.Run()) { var a_Prime = tmp_2; 
var tmp_5 = b;
foreach (var tmp_4 in tmp_5.Run()) { var b_Prime = tmp_4; 
if(a_Prime is Expr && b_Prime is Expr) { 
var tmp_7 = new _opMultiplication(a_Prime as Expr, b_Prime as Expr);
foreach (var tmp_6 in tmp_7.Run()) { var c = tmp_6; 
var tmp_9 = c;
foreach (var tmp_8 in tmp_9.Run()) { var c_Prime = tmp_8; 
if(c_Prime is Expr && b_Prime is Expr) { 
var tmp_11 = new _opAddition(c_Prime as Expr, b_Prime as Expr);
foreach (var tmp_10 in tmp_11.Run()) { var d = tmp_10; 
var result = d;
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
}

public class _opAddition : Expr {
public Expr P1;
public Expr P2;

public _opAddition(Expr P1, Expr P2) {this.P1 = P1; this.P2 = P2;}

  public IEnumerable<IRunnable> Run() {   {
var tmp_0 = this; var tmp_1 = tmp_0.P1 as z; 
if (tmp_1 != null) { var a = tmp_0.P2; 
var tmp_3 = a;
foreach (var tmp_2 in tmp_3.Run()) { var a_Prime = tmp_2; 
var result = a_Prime;
yield return result;  } }
}
  {
var tmp_0 = this; var tmp_1 = tmp_0.P1 as s; 
if (tmp_1 != null) { var a = tmp_1.P1; var b = tmp_0.P2; 
var tmp_3 = a;
foreach (var tmp_2 in tmp_3.Run()) { var a_Prime = tmp_2; 
var tmp_5 = b;
foreach (var tmp_4 in tmp_5.Run()) { var b_Prime = tmp_4; 
if(a_Prime is Expr && b_Prime is Expr) { 
var tmp_7 = new _opAddition(a_Prime as Expr, b_Prime as Expr);
foreach (var tmp_6 in tmp_7.Run()) { var c = tmp_6; 
if(c is Expr) { 
var result = new s(c as Expr);
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
}

public class s : Expr {
public Expr P1;

public s(Expr P1) {this.P1 = P1;}

  public IEnumerable<IRunnable> Run() {   {
var tmp_0 = this; var a = tmp_0.P1; 
var tmp_2 = a;
foreach (var tmp_1 in tmp_2.Run()) { var a_Prime = tmp_1; 
if(a_Prime is Expr) { 
var result = new s(a_Prime as Expr);
yield return result;  } }
}
  }


public override string ToString() {
 var res = "("; 

 res += "s"; res += P1.ToString();

 res += ")";
 return res;
}
}

public class z : Expr {

public z() {}

  public IEnumerable<IRunnable> Run() {   {
var tmp_0 = this as z; 
var result = new z();
yield return result; 
}
  }


public override string ToString() {
return "z";
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
