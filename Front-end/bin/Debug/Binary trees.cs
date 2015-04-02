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


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run0_() { foreach(var p in Run()) yield return p; }

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


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run0_() { foreach(var p in Run()) yield return p; }

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

  public IEnumerable<IRunnable> Run0_() {   
 { 
 #line 40 "Content\Binary trees"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as nil; 
 #line 40 "Content\Binary trees"
if (tmp_1 != null) { var k = tmp_0.P2; 
 #line 40 "Content\Binary trees"
var result = node.Create(nil.Create(), k, nil.Create());
 #line 40 "Content\Binary trees"
yield return result;  }
 } 

  
 { 
 #line 45 "Content\Binary trees"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as node; 
 #line 45 "Content\Binary trees"
if (tmp_1 != null) { var l = tmp_1.P1; var k = tmp_1.P2; var r = tmp_1.P3; var x = tmp_0.P2; 
 #line 45 "Content\Binary trees"
if(x.Equals(k)) { 
 #line 45 "Content\Binary trees"
if(l is BinTreeInt && r is BinTreeInt) { 
 #line 45 "Content\Binary trees"
var result = node.Create(l as BinTreeInt, k, r as BinTreeInt);
 #line 45 "Content\Binary trees"
yield return result;  } } }
 } 

  
 { 
 #line 52 "Content\Binary trees"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as node; 
 #line 52 "Content\Binary trees"
if (tmp_1 != null) { var l = tmp_1.P1; var k = tmp_1.P2; var r = tmp_1.P3; var x = tmp_0.P2; 
 #line 52 "Content\Binary trees"
if(x<k) { 
 #line 52 "Content\Binary trees"
if(l is BinTreeInt) { 
 #line 52 "Content\Binary trees"
var tmp_3 = add.Create(l as BinTreeInt, x);
 #line 52 "Content\Binary trees"
foreach (var tmp_2 in tmp_3.Run0_()) { var l_Prime = tmp_2; 
 #line 52 "Content\Binary trees"
if(l_Prime is BinTreeInt && r is BinTreeInt) { 
 #line 52 "Content\Binary trees"
var result = node.Create(l_Prime as BinTreeInt, k, r as BinTreeInt);
 #line 52 "Content\Binary trees"
yield return result;  } } } } }
 } 

  
 { 
 #line 61 "Content\Binary trees"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as node; 
 #line 61 "Content\Binary trees"
if (tmp_1 != null) { var l = tmp_1.P1; var k = tmp_1.P2; var r = tmp_1.P3; var x = tmp_0.P2; 
 #line 61 "Content\Binary trees"
if(x>k) { 
 #line 61 "Content\Binary trees"
if(r is BinTreeInt) { 
 #line 61 "Content\Binary trees"
var tmp_3 = add.Create(r as BinTreeInt, x);
 #line 61 "Content\Binary trees"
foreach (var tmp_2 in tmp_3.Run0_()) { var r_Prime = tmp_2; 
 #line 61 "Content\Binary trees"
if(l is BinTreeInt && r_Prime is BinTreeInt) { 
 #line 61 "Content\Binary trees"
var result = node.Create(l as BinTreeInt, k, r_Prime as BinTreeInt);
 #line 61 "Content\Binary trees"
yield return result;  } } } } }
 } 

 foreach(var p in Run()) yield return p; }

public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }

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

  public IEnumerable<IRunnable> Run0_() {   
 { 
 #line 71 "Content\Binary trees"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as nil; 
 #line 71 "Content\Binary trees"
if (tmp_1 != null) { var k = tmp_0.P2; 
 #line 71 "Content\Binary trees"
var result = _opDollar.Create(false);
 #line 71 "Content\Binary trees"
yield return result;  }
 } 

  
 { 
 #line 76 "Content\Binary trees"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as node; 
 #line 76 "Content\Binary trees"
if (tmp_1 != null) { var l = tmp_1.P1; var k = tmp_1.P2; var r = tmp_1.P3; var x = tmp_0.P2; 
 #line 76 "Content\Binary trees"
if(x.Equals(k)) { 
 #line 76 "Content\Binary trees"
var result = _opDollar.Create(true);
 #line 76 "Content\Binary trees"
yield return result;  } }
 } 

  
 { 
 #line 83 "Content\Binary trees"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as node; 
 #line 83 "Content\Binary trees"
if (tmp_1 != null) { var l = tmp_1.P1; var k = tmp_1.P2; var r = tmp_1.P3; var x = tmp_0.P2; 
 #line 83 "Content\Binary trees"
if(x<k) { 
 #line 83 "Content\Binary trees"
if(l is BinTreeInt) { 
 #line 83 "Content\Binary trees"
var tmp_3 = contains.Create(l as BinTreeInt, x);
 #line 83 "Content\Binary trees"
foreach (var tmp_2 in tmp_3.Run0_()) { var res = tmp_2; 
 #line 83 "Content\Binary trees"
var result = res;
 #line 83 "Content\Binary trees"
yield return result;  } } } }
 } 

  
 { 
 #line 92 "Content\Binary trees"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as node; 
 #line 92 "Content\Binary trees"
if (tmp_1 != null) { var l = tmp_1.P1; var k = tmp_1.P2; var r = tmp_1.P3; var x = tmp_0.P2; 
 #line 92 "Content\Binary trees"
if(x>k) { 
 #line 92 "Content\Binary trees"
if(r is BinTreeInt) { 
 #line 92 "Content\Binary trees"
var tmp_3 = contains.Create(r as BinTreeInt, x);
 #line 92 "Content\Binary trees"
foreach (var tmp_2 in tmp_3.Run0_()) { var res = tmp_2; 
 #line 92 "Content\Binary trees"
var result = res;
 #line 92 "Content\Binary trees"
yield return result;  } } } }
 } 

 foreach(var p in Run()) yield return p; }

public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }

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


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run0_() { foreach(var p in Run()) yield return p; }

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


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run0_() { foreach(var p in Run()) yield return p; }

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

  public IEnumerable<IRunnable> Run() {   
 { 
 #line 21 "Content\Binary trees"
var tmp_0 = this as run; 
 #line 21 "Content\Binary trees"
var tmp_2 = add.Create(nil.Create(), 10);
 #line 21 "Content\Binary trees"
foreach (var tmp_1 in tmp_2.Run0_()) { var t1 = tmp_1; 
 #line 21 "Content\Binary trees"
if(t1 is BinTreeInt) { 
 #line 21 "Content\Binary trees"
var tmp_4 = add.Create(t1 as BinTreeInt, 5);
 #line 21 "Content\Binary trees"
foreach (var tmp_3 in tmp_4.Run0_()) { var t2 = tmp_3; 
 #line 21 "Content\Binary trees"
if(t2 is BinTreeInt) { 
 #line 21 "Content\Binary trees"
var tmp_6 = add.Create(t2 as BinTreeInt, 7);
 #line 21 "Content\Binary trees"
foreach (var tmp_5 in tmp_6.Run0_()) { var t2b = tmp_5; 
 #line 21 "Content\Binary trees"
if(t2b is BinTreeInt) { 
 #line 21 "Content\Binary trees"
var tmp_8 = add.Create(t2b as BinTreeInt, 15);
 #line 21 "Content\Binary trees"
foreach (var tmp_7 in tmp_8.Run0_()) { var t3 = tmp_7; 
 #line 21 "Content\Binary trees"
if(t3 is BinTreeInt) { 
 #line 21 "Content\Binary trees"
var tmp_10 = add.Create(t3 as BinTreeInt, 1);
 #line 21 "Content\Binary trees"
foreach (var tmp_9 in tmp_10.Run0_()) { var t4 = tmp_9; 
 #line 21 "Content\Binary trees"
if(t4 is BinTreeInt) { 
 #line 21 "Content\Binary trees"
var tmp_12 = add.Create(t4 as BinTreeInt, 16);
 #line 21 "Content\Binary trees"
foreach (var tmp_11 in tmp_12.Run0_()) { var t = tmp_11; 
 #line 21 "Content\Binary trees"
if(t is BinTreeInt) { 
 #line 21 "Content\Binary trees"
var tmp_14 = contains.Create(t as BinTreeInt, 7);
 #line 21 "Content\Binary trees"
foreach (var tmp_13 in tmp_14.Run0_()) { var res = tmp_13; 
 #line 21 "Content\Binary trees"
if(t is BinTreeInt && res is Expr) { 
 #line 21 "Content\Binary trees"
var result = _Comma.Create(contains.Create(t as BinTreeInt, 7), res as Expr);
 #line 21 "Content\Binary trees"
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
