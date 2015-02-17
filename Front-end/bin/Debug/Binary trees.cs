using System.Collections.Generic;
using System.Linq;
namespace Binary_trees {
 public static class Extensions { public static V GetKey<T, V>(this System.Collections.Immutable.ImmutableDictionary<T, V> self, T key) { return self[key]; } }
 public interface IRunnable { IEnumerable<IRunnable> Run();
IEnumerable<IRunnable> Run0_();
 }


public interface BinTreeInt : IRunnable {}
public interface BinTreeIntLeft : IRunnable {}
public interface BoolExpr : Expr {}
public interface Expr : IRunnable {}



public class _opDollar : BoolExpr {
public bool P1;

public _opDollar(bool P1) {this.P1 = P1;}
public static _opDollar Create(bool P1) { return new _opDollar(P1); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run0_() { foreach(var p in Run()) yield return p; }

public override string ToString() {
 var res = "("; 

 res += "$"; res += P1.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _opDollar;
 if(tmp != null) return this.P1.Equals(tmp.P1); 
 else return false; }
}

public class _opAddition_opSubtraction : BinTreeIntLeft {
public BinTreeInt P1;
public int P2;

public _opAddition_opSubtraction(BinTreeInt P1, int P2) {this.P1 = P1; this.P2 = P2;}
public static _opAddition_opSubtraction Create(BinTreeInt P1, int P2) { return new _opAddition_opSubtraction(P1, P2); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run0_() { foreach(var p in Run()) yield return p; }

public override string ToString() {
 var res = "("; 
res += P1.ToString(); 

 res += "+-"; res += P2.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _opAddition_opSubtraction;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
 else return false; }
}

public class _Comma : Expr {
public Expr P1;
public Expr P2;

public _Comma(Expr P1, Expr P2) {this.P1 = P1; this.P2 = P2;}
public static _Comma Create(Expr P1, Expr P2) { return new _Comma(P1, P2); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run0_() { foreach(var p in Run()) yield return p; }

public override string ToString() {
 var res = "("; 
res += P1.ToString(); 

 res += ","; res += P2.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _Comma;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
 else return false; }
}

public class _opSubtraction_opAddition : BinTreeInt {
public BinTreeIntLeft P1;
public BinTreeInt P2;

public _opSubtraction_opAddition(BinTreeIntLeft P1, BinTreeInt P2) {this.P1 = P1; this.P2 = P2;}
public static _opSubtraction_opAddition Create(BinTreeIntLeft P1, BinTreeInt P2) { return new _opSubtraction_opAddition(P1, P2); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run0_() { foreach(var p in Run()) yield return p; }

public override string ToString() {
 var res = "("; 
res += P1.ToString(); 

 res += "-+"; res += P2.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _opSubtraction_opAddition;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
 else return false; }
}

public class add : Expr {
public BinTreeInt P1;
public int P2;

public add(BinTreeInt P1, int P2) {this.P1 = P1; this.P2 = P2;}
public static add Create(BinTreeInt P1, int P2) { return new add(P1, P2); }

  public IEnumerable<IRunnable> Run0_() {   
 { 
 #line 25 "Content\Binary trees\transform.mc"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as nil; 
 #line 25 "Content\Binary trees\transform.mc"
if (tmp_1 != null) { var k = tmp_0.P2; 
 #line 25 "Content\Binary trees\transform.mc"
var result = _opSubtraction_opAddition.Create(_opAddition_opSubtraction.Create(nil.Create(), k), nil.Create());
 #line 25 "Content\Binary trees\transform.mc"
yield return result;  }
 } 

  
 { 
 #line 28 "Content\Binary trees\transform.mc"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opSubtraction_opAddition; 
 #line 28 "Content\Binary trees\transform.mc"
if (tmp_1 != null) { var tmp_2 = tmp_1.P1 as _opAddition_opSubtraction; 
 #line 28 "Content\Binary trees\transform.mc"
if (tmp_2 != null) { var l = tmp_2.P1; var k = tmp_2.P2; var r = tmp_1.P2; var x = tmp_0.P2; 
 #line 28 "Content\Binary trees\transform.mc"
if(x.Equals(k)) { 
 #line 28 "Content\Binary trees\transform.mc"
if(l is BinTreeInt && r is BinTreeInt) { 
 #line 28 "Content\Binary trees\transform.mc"
var result = _opSubtraction_opAddition.Create(_opAddition_opSubtraction.Create(l as BinTreeInt, k), r as BinTreeInt);
 #line 28 "Content\Binary trees\transform.mc"
yield return result;  } } } }
 } 

  
 { 
 #line 32 "Content\Binary trees\transform.mc"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opSubtraction_opAddition; 
 #line 32 "Content\Binary trees\transform.mc"
if (tmp_1 != null) { var tmp_2 = tmp_1.P1 as _opAddition_opSubtraction; 
 #line 32 "Content\Binary trees\transform.mc"
if (tmp_2 != null) { var l = tmp_2.P1; var k = tmp_2.P2; var r = tmp_1.P2; var x = tmp_0.P2; 
 #line 32 "Content\Binary trees\transform.mc"
if(x<k) { 
 #line 32 "Content\Binary trees\transform.mc"
if(l is BinTreeInt) { 
 #line 32 "Content\Binary trees\transform.mc"
var tmp_4 = add.Create(l as BinTreeInt, x);
 #line 32 "Content\Binary trees\transform.mc"
foreach (var tmp_3 in tmp_4.Run0_()) { var l_Prime = tmp_3; 
 #line 32 "Content\Binary trees\transform.mc"
if(l_Prime is BinTreeInt && r is BinTreeInt) { 
 #line 32 "Content\Binary trees\transform.mc"
var result = _opSubtraction_opAddition.Create(_opAddition_opSubtraction.Create(l_Prime as BinTreeInt, k), r as BinTreeInt);
 #line 32 "Content\Binary trees\transform.mc"
yield return result;  } } } } } }
 } 

  
 { 
 #line 37 "Content\Binary trees\transform.mc"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opSubtraction_opAddition; 
 #line 37 "Content\Binary trees\transform.mc"
if (tmp_1 != null) { var tmp_2 = tmp_1.P1 as _opAddition_opSubtraction; 
 #line 37 "Content\Binary trees\transform.mc"
if (tmp_2 != null) { var l = tmp_2.P1; var k = tmp_2.P2; var r = tmp_1.P2; var x = tmp_0.P2; 
 #line 37 "Content\Binary trees\transform.mc"
if(x>k) { 
 #line 37 "Content\Binary trees\transform.mc"
if(r is BinTreeInt) { 
 #line 37 "Content\Binary trees\transform.mc"
var tmp_4 = add.Create(r as BinTreeInt, x);
 #line 37 "Content\Binary trees\transform.mc"
foreach (var tmp_3 in tmp_4.Run0_()) { var r_Prime = tmp_3; 
 #line 37 "Content\Binary trees\transform.mc"
if(l is BinTreeInt && r_Prime is BinTreeInt) { 
 #line 37 "Content\Binary trees\transform.mc"
var result = _opSubtraction_opAddition.Create(_opAddition_opSubtraction.Create(l as BinTreeInt, k), r_Prime as BinTreeInt);
 #line 37 "Content\Binary trees\transform.mc"
yield return result;  } } } } } }
 } 

 foreach(var p in Run()) yield return p; }

public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }

public override string ToString() {
 var res = "("; 
res += P1.ToString(); 

 res += "add"; res += P2.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as add;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
 else return false; }
}

public class contains : Expr {
public BinTreeInt P1;
public int P2;

public contains(BinTreeInt P1, int P2) {this.P1 = P1; this.P2 = P2;}
public static contains Create(BinTreeInt P1, int P2) { return new contains(P1, P2); }

  public IEnumerable<IRunnable> Run0_() {   
 { 
 #line 43 "Content\Binary trees\transform.mc"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as nil; 
 #line 43 "Content\Binary trees\transform.mc"
if (tmp_1 != null) { var k = tmp_0.P2; 
 #line 43 "Content\Binary trees\transform.mc"
var result = _opDollar.Create(false);
 #line 43 "Content\Binary trees\transform.mc"
yield return result;  }
 } 

  
 { 
 #line 46 "Content\Binary trees\transform.mc"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opSubtraction_opAddition; 
 #line 46 "Content\Binary trees\transform.mc"
if (tmp_1 != null) { var tmp_2 = tmp_1.P1 as _opAddition_opSubtraction; 
 #line 46 "Content\Binary trees\transform.mc"
if (tmp_2 != null) { var l = tmp_2.P1; var k = tmp_2.P2; var r = tmp_1.P2; var x = tmp_0.P2; 
 #line 46 "Content\Binary trees\transform.mc"
if(x.Equals(k)) { 
 #line 46 "Content\Binary trees\transform.mc"
var result = _opDollar.Create(true);
 #line 46 "Content\Binary trees\transform.mc"
yield return result;  } } }
 } 

  
 { 
 #line 50 "Content\Binary trees\transform.mc"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opSubtraction_opAddition; 
 #line 50 "Content\Binary trees\transform.mc"
if (tmp_1 != null) { var tmp_2 = tmp_1.P1 as _opAddition_opSubtraction; 
 #line 50 "Content\Binary trees\transform.mc"
if (tmp_2 != null) { var l = tmp_2.P1; var k = tmp_2.P2; var r = tmp_1.P2; var x = tmp_0.P2; 
 #line 50 "Content\Binary trees\transform.mc"
if(x<k) { 
 #line 50 "Content\Binary trees\transform.mc"
if(l is BinTreeInt) { 
 #line 50 "Content\Binary trees\transform.mc"
var tmp_4 = contains.Create(l as BinTreeInt, x);
 #line 50 "Content\Binary trees\transform.mc"
foreach (var tmp_3 in tmp_4.Run0_()) { var res = tmp_3; 
 #line 50 "Content\Binary trees\transform.mc"
var result = res;
 #line 50 "Content\Binary trees\transform.mc"
yield return result;  } } } } }
 } 

  
 { 
 #line 55 "Content\Binary trees\transform.mc"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opSubtraction_opAddition; 
 #line 55 "Content\Binary trees\transform.mc"
if (tmp_1 != null) { var tmp_2 = tmp_1.P1 as _opAddition_opSubtraction; 
 #line 55 "Content\Binary trees\transform.mc"
if (tmp_2 != null) { var l = tmp_2.P1; var k = tmp_2.P2; var r = tmp_1.P2; var x = tmp_0.P2; 
 #line 55 "Content\Binary trees\transform.mc"
if(x>k) { 
 #line 55 "Content\Binary trees\transform.mc"
if(r is BinTreeInt) { 
 #line 55 "Content\Binary trees\transform.mc"
var tmp_4 = contains.Create(r as BinTreeInt, x);
 #line 55 "Content\Binary trees\transform.mc"
foreach (var tmp_3 in tmp_4.Run0_()) { var res = tmp_3; 
 #line 55 "Content\Binary trees\transform.mc"
var result = res;
 #line 55 "Content\Binary trees\transform.mc"
yield return result;  } } } } }
 } 

 foreach(var p in Run()) yield return p; }

public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }

public override string ToString() {
 var res = "("; 
res += P1.ToString(); 

 res += "contains"; res += P2.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as contains;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
 else return false; }
}

public class nil : BinTreeInt {

public nil() {}
public static nil Create() { return new nil(); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run0_() { foreach(var p in Run()) yield return p; }

public override string ToString() {
return "nil";
}

public override bool Equals(object other) {
 return other is nil; 
}
}

public class run : Expr {

public run() {}
public static run Create() { return new run(); }

  public IEnumerable<IRunnable> Run() {   
 { 
 #line 15 "Content\Binary trees\transform.mc"
var tmp_0 = this as run; 
 #line 15 "Content\Binary trees\transform.mc"
var tmp_2 = add.Create(nil.Create(), 10);
 #line 15 "Content\Binary trees\transform.mc"
foreach (var tmp_1 in tmp_2.Run0_()) { var t1 = tmp_1; 
 #line 15 "Content\Binary trees\transform.mc"
if(t1 is BinTreeInt) { 
 #line 15 "Content\Binary trees\transform.mc"
var tmp_4 = add.Create(t1 as BinTreeInt, 5);
 #line 15 "Content\Binary trees\transform.mc"
foreach (var tmp_3 in tmp_4.Run0_()) { var t2 = tmp_3; 
 #line 15 "Content\Binary trees\transform.mc"
if(t2 is BinTreeInt) { 
 #line 15 "Content\Binary trees\transform.mc"
var tmp_6 = add.Create(t2 as BinTreeInt, 7);
 #line 15 "Content\Binary trees\transform.mc"
foreach (var tmp_5 in tmp_6.Run0_()) { var t2b = tmp_5; 
 #line 15 "Content\Binary trees\transform.mc"
if(t2b is BinTreeInt) { 
 #line 15 "Content\Binary trees\transform.mc"
var tmp_8 = add.Create(t2b as BinTreeInt, 15);
 #line 15 "Content\Binary trees\transform.mc"
foreach (var tmp_7 in tmp_8.Run0_()) { var t3 = tmp_7; 
 #line 15 "Content\Binary trees\transform.mc"
if(t3 is BinTreeInt) { 
 #line 15 "Content\Binary trees\transform.mc"
var tmp_10 = add.Create(t3 as BinTreeInt, 1);
 #line 15 "Content\Binary trees\transform.mc"
foreach (var tmp_9 in tmp_10.Run0_()) { var t4 = tmp_9; 
 #line 15 "Content\Binary trees\transform.mc"
if(t4 is BinTreeInt) { 
 #line 15 "Content\Binary trees\transform.mc"
var tmp_12 = add.Create(t4 as BinTreeInt, 16);
 #line 15 "Content\Binary trees\transform.mc"
foreach (var tmp_11 in tmp_12.Run0_()) { var t = tmp_11; 
 #line 15 "Content\Binary trees\transform.mc"
if(t is BinTreeInt) { 
 #line 15 "Content\Binary trees\transform.mc"
var tmp_14 = contains.Create(t as BinTreeInt, 7);
 #line 15 "Content\Binary trees\transform.mc"
foreach (var tmp_13 in tmp_14.Run0_()) { var res = tmp_13; 
 #line 15 "Content\Binary trees\transform.mc"
if(t is BinTreeInt && res is Expr) { 
 #line 15 "Content\Binary trees\transform.mc"
var result = _Comma.Create(contains.Create(t as BinTreeInt, 7), res as Expr);
 #line 15 "Content\Binary trees\transform.mc"
yield return result;  } } } } } } } } } } } } } }
 } 

  }

public IEnumerable<IRunnable> Run0_() { foreach(var p in Run()) yield return p; }

public override string ToString() {
return "run";
}

public override bool Equals(object other) {
 return other is run; 
}
}




public class EntryPoint {
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
