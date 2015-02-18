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



public class _opDollar : BoolExpr  {
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

public class _opAddition_opSubtraction : BinTreeIntLeft  {
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

public class _Comma : Expr  {
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

public class _opSubtraction_opAddition : BinTreeInt  {
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

public class add : Expr  {
public BinTreeInt P1;
public int P2;

public add(BinTreeInt P1, int P2) {this.P1 = P1; this.P2 = P2;}
public static add Create(BinTreeInt P1, int P2) { return new add(P1, P2); }

  public IEnumerable<IRunnable> Run0_() {   
 { 
 var tmp_0 = this; var tmp_1 = tmp_0.P1 as nil; 
if (tmp_1 != null) { var k = tmp_0.P2; 
var result = _opSubtraction_opAddition.Create(_opAddition_opSubtraction.Create(nil.Create(), k), nil.Create());
yield return result;  }
 } 

  
 { 
 var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opSubtraction_opAddition; 
if (tmp_1 != null) { var tmp_2 = tmp_1.P1 as _opAddition_opSubtraction; 
if (tmp_2 != null) { var l = tmp_2.P1; var k = tmp_2.P2; var r = tmp_1.P2; var x = tmp_0.P2; 
if(x.Equals(k)) { 
if(l is BinTreeInt && r is BinTreeInt) { 
var result = _opSubtraction_opAddition.Create(_opAddition_opSubtraction.Create(l as BinTreeInt, k), r as BinTreeInt);
yield return result;  } } } }
 } 

  
 { 
 var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opSubtraction_opAddition; 
if (tmp_1 != null) { var tmp_2 = tmp_1.P1 as _opAddition_opSubtraction; 
if (tmp_2 != null) { var l = tmp_2.P1; var k = tmp_2.P2; var r = tmp_1.P2; var x = tmp_0.P2; 
if(x<k) { 
if(l is BinTreeInt) { 
var tmp_4 = add.Create(l as BinTreeInt, x);
foreach (var tmp_3 in tmp_4.Run0_()) { var l_Prime = tmp_3; 
if(l_Prime is BinTreeInt && r is BinTreeInt) { 
var result = _opSubtraction_opAddition.Create(_opAddition_opSubtraction.Create(l_Prime as BinTreeInt, k), r as BinTreeInt);
yield return result;  } } } } } }
 } 

  
 { 
 var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opSubtraction_opAddition; 
if (tmp_1 != null) { var tmp_2 = tmp_1.P1 as _opAddition_opSubtraction; 
if (tmp_2 != null) { var l = tmp_2.P1; var k = tmp_2.P2; var r = tmp_1.P2; var x = tmp_0.P2; 
if(x>k) { 
if(r is BinTreeInt) { 
var tmp_4 = add.Create(r as BinTreeInt, x);
foreach (var tmp_3 in tmp_4.Run0_()) { var r_Prime = tmp_3; 
if(l is BinTreeInt && r_Prime is BinTreeInt) { 
var result = _opSubtraction_opAddition.Create(_opAddition_opSubtraction.Create(l as BinTreeInt, k), r_Prime as BinTreeInt);
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

public class contains : Expr  {
public BinTreeInt P1;
public int P2;

public contains(BinTreeInt P1, int P2) {this.P1 = P1; this.P2 = P2;}
public static contains Create(BinTreeInt P1, int P2) { return new contains(P1, P2); }

  public IEnumerable<IRunnable> Run0_() {   
 { 
 var tmp_0 = this; var tmp_1 = tmp_0.P1 as nil; 
if (tmp_1 != null) { var k = tmp_0.P2; 
var result = _opDollar.Create(false);
yield return result;  }
 } 

  
 { 
 var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opSubtraction_opAddition; 
if (tmp_1 != null) { var tmp_2 = tmp_1.P1 as _opAddition_opSubtraction; 
if (tmp_2 != null) { var l = tmp_2.P1; var k = tmp_2.P2; var r = tmp_1.P2; var x = tmp_0.P2; 
if(x.Equals(k)) { 
var result = _opDollar.Create(true);
yield return result;  } } }
 } 

  
 { 
 var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opSubtraction_opAddition; 
if (tmp_1 != null) { var tmp_2 = tmp_1.P1 as _opAddition_opSubtraction; 
if (tmp_2 != null) { var l = tmp_2.P1; var k = tmp_2.P2; var r = tmp_1.P2; var x = tmp_0.P2; 
if(x<k) { 
if(l is BinTreeInt) { 
var tmp_4 = contains.Create(l as BinTreeInt, x);
foreach (var tmp_3 in tmp_4.Run0_()) { var res = tmp_3; 
var result = res;
yield return result;  } } } } }
 } 

  
 { 
 var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opSubtraction_opAddition; 
if (tmp_1 != null) { var tmp_2 = tmp_1.P1 as _opAddition_opSubtraction; 
if (tmp_2 != null) { var l = tmp_2.P1; var k = tmp_2.P2; var r = tmp_1.P2; var x = tmp_0.P2; 
if(x>k) { 
if(r is BinTreeInt) { 
var tmp_4 = contains.Create(r as BinTreeInt, x);
foreach (var tmp_3 in tmp_4.Run0_()) { var res = tmp_3; 
var result = res;
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

public class nil : BinTreeInt  {

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

public class run : Expr  {

public run() {}
public static run Create() { return new run(); }

  public IEnumerable<IRunnable> Run() {   
 { 
 var tmp_0 = this as run; 
var tmp_2 = add.Create(nil.Create(), 10);
foreach (var tmp_1 in tmp_2.Run0_()) { var t1 = tmp_1; 
if(t1 is BinTreeInt) { 
var tmp_4 = add.Create(t1 as BinTreeInt, 5);
foreach (var tmp_3 in tmp_4.Run0_()) { var t2 = tmp_3; 
if(t2 is BinTreeInt) { 
var tmp_6 = add.Create(t2 as BinTreeInt, 7);
foreach (var tmp_5 in tmp_6.Run0_()) { var t2b = tmp_5; 
if(t2b is BinTreeInt) { 
var tmp_8 = add.Create(t2b as BinTreeInt, 15);
foreach (var tmp_7 in tmp_8.Run0_()) { var t3 = tmp_7; 
if(t3 is BinTreeInt) { 
var tmp_10 = add.Create(t3 as BinTreeInt, 1);
foreach (var tmp_9 in tmp_10.Run0_()) { var t4 = tmp_9; 
if(t4 is BinTreeInt) { 
var tmp_12 = add.Create(t4 as BinTreeInt, 16);
foreach (var tmp_11 in tmp_12.Run0_()) { var t = tmp_11; 
if(t is BinTreeInt) { 
var tmp_14 = contains.Create(t as BinTreeInt, 7);
foreach (var tmp_13 in tmp_14.Run0_()) { var res = tmp_13; 
if(t is BinTreeInt && res is Expr) { 
var result = _Comma.Create(contains.Create(t as BinTreeInt, 7), res as Expr);
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
