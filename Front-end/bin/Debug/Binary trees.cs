using System.Collections.Generic;
using System.Linq;
namespace Binary_trees {
 public static class Extensions { public static V GetKey<T, V>(this System.Collections.Immutable.ImmutableDictionary<T, V> self, T key) { return self[key]; } }
 public interface IRunnable { IEnumerable<IRunnable> Run();
IEnumerable<IRunnable> Run0_();
 }


public interface BinTreeInt : IRunnable {}
public interface BoolExpr : Expr {}
public interface Expr : IRunnable {}



public class _opDollar : BoolExpr  {
public bool P1;

public _opDollar(bool P1) {this.P1 = P1;}
public static _opDollar Create(bool P1) { return new _opDollar(P1); }


public static IEnumerable<IRunnable> StaticRun(bool P1) { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run(){ return StaticRun(P1); }
public static IEnumerable<IRunnable> StaticRun0_(bool P1) { return StaticRun(P1); }
public IEnumerable<IRunnable> Run0_(){ return StaticRun0_(P1); }

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

public class _Comma : Expr  {
public Expr P1;
public Expr P2;

public _Comma(Expr P1, Expr P2) {this.P1 = P1; this.P2 = P2;}
public static _Comma Create(Expr P1, Expr P2) { return new _Comma(P1, P2); }


public static IEnumerable<IRunnable> StaticRun(Expr P1, Expr P2) { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run(){ return StaticRun(P1, P2); }
public static IEnumerable<IRunnable> StaticRun0_(Expr P1, Expr P2) { return StaticRun(P1, P2); }
public IEnumerable<IRunnable> Run0_(){ return StaticRun0_(P1, P2); }

public override string ToString() {
 var res = "("; 
res += P1.ToString(); 

 res += " , "; res += P2.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _Comma;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class add : Expr  {
public BinTreeInt P1;
public int P2;

public add(BinTreeInt P1, int P2) {this.P1 = P1; this.P2 = P2;}
public static add Create(BinTreeInt P1, int P2) { return new add(P1, P2); }

  public static IEnumerable<IRunnable> StaticRun0_(BinTreeInt P1, int P2) {   
 { 
 var tmp_0 = P1 as nil; 
if (tmp_0 != null) { var k = P2; 
var result = node.Create(nil.Create(), k, nil.Create());
yield return result;  }
 } 

  
 { 
 var tmp_0 = P1 as node; 
if (tmp_0 != null) { var l = tmp_0.P1; var k = tmp_0.P2; var r = tmp_0.P3; var x = P2; 
if(x.Equals(k)) { 
if(l is BinTreeInt && r is BinTreeInt) { 
var result = node.Create(l as BinTreeInt, k, r as BinTreeInt);
yield return result;  } } }
 } 

  
 { 
 var tmp_0 = P1 as node; 
if (tmp_0 != null) { var l = tmp_0.P1; var k = tmp_0.P2; var r = tmp_0.P3; var x = P2; 
if(x<k) { 
if(l is BinTreeInt) { 
var tmp_2 = add.Create(l as BinTreeInt, x);
foreach (var tmp_1 in tmp_2.Run0_()) { var l_Prime = tmp_1; 
if(l_Prime is BinTreeInt && r is BinTreeInt) { 
var result = node.Create(l_Prime as BinTreeInt, k, r as BinTreeInt);
yield return result;  } } } } }
 } 

  
 { 
 var tmp_0 = P1 as node; 
if (tmp_0 != null) { var l = tmp_0.P1; var k = tmp_0.P2; var r = tmp_0.P3; var x = P2; 
if(x>k) { 
if(r is BinTreeInt) { 
var tmp_2 = add.Create(r as BinTreeInt, x);
foreach (var tmp_1 in tmp_2.Run0_()) { var r_Prime = tmp_1; 
if(l is BinTreeInt && r_Prime is BinTreeInt) { 
var result = node.Create(l as BinTreeInt, k, r_Prime as BinTreeInt);
yield return result;  } } } } }
 } 

 foreach(var p in StaticRun(P1, P2)) yield return p; }
public IEnumerable<IRunnable> Run0_() { return StaticRun0_(P1, P2); }

public static IEnumerable<IRunnable> StaticRun(BinTreeInt P1, int P2) { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run(){ return StaticRun(P1, P2); }

public override string ToString() {
 var res = "("; 
res += P1.ToString(); 

 res += " add "; res += P2.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as add;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class contains : Expr  {
public BinTreeInt P1;
public int P2;

public contains(BinTreeInt P1, int P2) {this.P1 = P1; this.P2 = P2;}
public static contains Create(BinTreeInt P1, int P2) { return new contains(P1, P2); }

  public static IEnumerable<IRunnable> StaticRun0_(BinTreeInt P1, int P2) {   
 { 
 var tmp_0 = P1 as nil; 
if (tmp_0 != null) { var k = P2; 
var result = _opDollar.Create(false);
yield return result;  }
 } 

  
 { 
 var tmp_0 = P1 as node; 
if (tmp_0 != null) { var l = tmp_0.P1; var k = tmp_0.P2; var r = tmp_0.P3; var x = P2; 
if(x.Equals(k)) { 
var result = _opDollar.Create(true);
yield return result;  } }
 } 

  
 { 
 var tmp_0 = P1 as node; 
if (tmp_0 != null) { var l = tmp_0.P1; var k = tmp_0.P2; var r = tmp_0.P3; var x = P2; 
if(x<k) { 
if(l is BinTreeInt) { 
var tmp_2 = contains.Create(l as BinTreeInt, x);
foreach (var tmp_1 in tmp_2.Run0_()) { var res = tmp_1; 
var result = res;
yield return result;  } } } }
 } 

  
 { 
 var tmp_0 = P1 as node; 
if (tmp_0 != null) { var l = tmp_0.P1; var k = tmp_0.P2; var r = tmp_0.P3; var x = P2; 
if(x>k) { 
if(r is BinTreeInt) { 
var tmp_2 = contains.Create(r as BinTreeInt, x);
foreach (var tmp_1 in tmp_2.Run0_()) { var res = tmp_1; 
var result = res;
yield return result;  } } } }
 } 

 foreach(var p in StaticRun(P1, P2)) yield return p; }
public IEnumerable<IRunnable> Run0_() { return StaticRun0_(P1, P2); }

public static IEnumerable<IRunnable> StaticRun(BinTreeInt P1, int P2) { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run(){ return StaticRun(P1, P2); }

public override string ToString() {
 var res = "("; 
res += P1.ToString(); 

 res += " contains "; res += P2.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as contains;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class nil : BinTreeInt  {

public nil() {}
public static nil Create() { return new nil(); }


public static IEnumerable<IRunnable> StaticRun() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run(){ return StaticRun(); }
public static IEnumerable<IRunnable> StaticRun0_() { return StaticRun(); }
public IEnumerable<IRunnable> Run0_(){ return StaticRun0_(); }

public override string ToString() {
return "nil";
}

public override bool Equals(object other) {
 return other is nil; 
}

public override int GetHashCode() {
 return 0; 
}

}

public class node : BinTreeInt  {
public BinTreeInt P1;
public int P2;
public BinTreeInt P3;

public node(BinTreeInt P1, int P2, BinTreeInt P3) {this.P1 = P1; this.P2 = P2; this.P3 = P3;}
public static node Create(BinTreeInt P1, int P2, BinTreeInt P3) { return new node(P1, P2, P3); }


public static IEnumerable<IRunnable> StaticRun(BinTreeInt P1, int P2, BinTreeInt P3) { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run(){ return StaticRun(P1, P2, P3); }
public static IEnumerable<IRunnable> StaticRun0_(BinTreeInt P1, int P2, BinTreeInt P3) { return StaticRun(P1, P2, P3); }
public IEnumerable<IRunnable> Run0_(){ return StaticRun0_(P1, P2, P3); }

public override string ToString() {
 var res = "("; 

 res += " node "; res += P1.ToString(); 
res += P2.ToString(); 
res += P3.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as node;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2) && this.P3.Equals(tmp.P3); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class run : Expr  {

public run() {}
public static run Create() { return new run(); }

  public static IEnumerable<IRunnable> StaticRun() {   
 { 
 
var tmp_1 = add.Create(nil.Create(), 10);
foreach (var tmp_0 in tmp_1.Run0_()) { var t1 = tmp_0; 
if(t1 is BinTreeInt) { 
var tmp_3 = add.Create(t1 as BinTreeInt, 5);
foreach (var tmp_2 in tmp_3.Run0_()) { var t2 = tmp_2; 
if(t2 is BinTreeInt) { 
var tmp_5 = add.Create(t2 as BinTreeInt, 7);
foreach (var tmp_4 in tmp_5.Run0_()) { var t2b = tmp_4; 
if(t2b is BinTreeInt) { 
var tmp_7 = add.Create(t2b as BinTreeInt, 15);
foreach (var tmp_6 in tmp_7.Run0_()) { var t3 = tmp_6; 
if(t3 is BinTreeInt) { 
var tmp_9 = add.Create(t3 as BinTreeInt, 1);
foreach (var tmp_8 in tmp_9.Run0_()) { var t4 = tmp_8; 
if(t4 is BinTreeInt) { 
var tmp_11 = add.Create(t4 as BinTreeInt, 16);
foreach (var tmp_10 in tmp_11.Run0_()) { var t = tmp_10; 
if(t is BinTreeInt) { 
var tmp_13 = contains.Create(t as BinTreeInt, 7);
foreach (var tmp_12 in tmp_13.Run0_()) { var res = tmp_12; 
if(t is BinTreeInt && res is Expr) { 
var result = _Comma.Create(contains.Create(t as BinTreeInt, 7), res as Expr);
yield return result;  } } } } } } } } } } } } } }
 } 

  }
public IEnumerable<IRunnable> Run() { return StaticRun(); }

public static IEnumerable<IRunnable> StaticRun0_() { return StaticRun(); }
public IEnumerable<IRunnable> Run0_(){ return StaticRun0_(); }

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
