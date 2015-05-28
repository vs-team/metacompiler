using System.Collections.Generic;
using System.Linq;
namespace Binary_trees {
 public static class Extensions { public static V GetKey<T, V>(this System.Collections.Immutable.ImmutableDictionary<T, V> self, T key) { return self[key]; } }



public interface BinTreeInt : Expr {}
public interface Expr {}



public class _Comma : Expr  {
public Expr P1;
public bool P2;

public _Comma(Expr P1, bool P2) {this.P1 = P1; this.P2 = P2;}
public static _Comma Create(Expr P1, bool P2) { return new _Comma(P1, P2); }

public override string ToString() {
 var res = "("; 
if (P1 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P1.ToString(); } 

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

  public static BinTreeInt StaticRun0_(BinTreeInt P1, int P2) {    
 { 
 #line 24 "Content\Binary trees\transform.mc"
var tmp_0 = P1 as nil; 
 #line 24 "Content\Binary trees\transform.mc"
if (tmp_0 != null) { 
 var k = P2; 
 #line 24 "Content\Binary trees\transform.mc"
var result = node.Create(nil.Create(), k, nil.Create());
 #line 24 "Content\Binary trees\transform.mc"
 return result;  }
 } 

  
 { 
 #line 27 "Content\Binary trees\transform.mc"
var tmp_0 = P1 as node; 
 #line 27 "Content\Binary trees\transform.mc"
if (tmp_0 != null) { 
 var l = tmp_0.P1; var k = tmp_0.P2; var r = tmp_0.P3; var x = P2; 
 #line 27 "Content\Binary trees\transform.mc"
if(x.Equals(k)) { 
 #line 27 "Content\Binary trees\transform.mc"
var result = node.Create(l, k, r);
 #line 27 "Content\Binary trees\transform.mc"
 return result;  } }
 } 

  
 { 
 #line 31 "Content\Binary trees\transform.mc"
var tmp_0 = P1 as node; 
 #line 31 "Content\Binary trees\transform.mc"
if (tmp_0 != null) { 
 var l = tmp_0.P1; var k = tmp_0.P2; var r = tmp_0.P3; var x = P2; 
 #line 31 "Content\Binary trees\transform.mc"
if(x<k) { 
 #line 31 "Content\Binary trees\transform.mc"
var tmp_2 = add.Create(l, x);
 #line 31 "Content\Binary trees\transform.mc"

var tmp_1 = tmp_2.Run0_();

var l_Prime = tmp_1; 
 #line 31 "Content\Binary trees\transform.mc"
var result = node.Create(l_Prime, k, r);
 #line 31 "Content\Binary trees\transform.mc"
 return result;  } }
 } 

  
 { 
 #line 36 "Content\Binary trees\transform.mc"
var tmp_0 = P1 as node; 
 #line 36 "Content\Binary trees\transform.mc"
if (tmp_0 != null) { 
 var l = tmp_0.P1; var k = tmp_0.P2; var r = tmp_0.P3; var x = P2; 
 #line 36 "Content\Binary trees\transform.mc"
if(x>k) { 
 #line 36 "Content\Binary trees\transform.mc"
var tmp_2 = add.Create(r, x);
 #line 36 "Content\Binary trees\transform.mc"

var tmp_1 = tmp_2.Run0_();

var r_Prime = tmp_1; 
 #line 36 "Content\Binary trees\transform.mc"
var result = node.Create(l, k, r_Prime);
 #line 36 "Content\Binary trees\transform.mc"
 return result;  } }
 } 

 var p = StaticRun(P1, P2); return p; 
throw new System.Exception("Error evaluating: " + new add(P1, P2).ToString() + " no result returned."); }
public BinTreeInt Run0_() { return StaticRun0_(P1, P2); }

public static BinTreeInt StaticRun(BinTreeInt P1, int P2) { 
throw new System.Exception("Error evaluating: " + new add(P1, P2).ToString() + " no result returned."); }
public BinTreeInt Run(){ return StaticRun(P1, P2); }

public override string ToString() {
 var res = "("; 
if (P1 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P1.ToString(); } 

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

  public static bool StaticRun0_(BinTreeInt P1, int P2) {    
 { 
 #line 42 "Content\Binary trees\transform.mc"
var tmp_0 = P1 as nil; 
 #line 42 "Content\Binary trees\transform.mc"
if (tmp_0 != null) { 
 var k = P2; 
 #line 42 "Content\Binary trees\transform.mc"
var result = false;
 #line 42 "Content\Binary trees\transform.mc"
 return result;  }
 } 

  
 { 
 #line 45 "Content\Binary trees\transform.mc"
var tmp_0 = P1 as node; 
 #line 45 "Content\Binary trees\transform.mc"
if (tmp_0 != null) { 
 var l = tmp_0.P1; var k = tmp_0.P2; var r = tmp_0.P3; var x = P2; 
 #line 45 "Content\Binary trees\transform.mc"
if(x.Equals(k)) { 
 #line 45 "Content\Binary trees\transform.mc"
var result = true;
 #line 45 "Content\Binary trees\transform.mc"
 return result;  } }
 } 

  
 { 
 #line 49 "Content\Binary trees\transform.mc"
var tmp_0 = P1 as node; 
 #line 49 "Content\Binary trees\transform.mc"
if (tmp_0 != null) { 
 var l = tmp_0.P1; var k = tmp_0.P2; var r = tmp_0.P3; var x = P2; 
 #line 49 "Content\Binary trees\transform.mc"
if(x<k) { 
 #line 49 "Content\Binary trees\transform.mc"
var tmp_2 = contains.Create(l, x);
 #line 49 "Content\Binary trees\transform.mc"

var tmp_1 = tmp_2.Run0_();

var res = tmp_1; 
 #line 49 "Content\Binary trees\transform.mc"
var result = res;
 #line 49 "Content\Binary trees\transform.mc"
 return result;  } }
 } 

  
 { 
 #line 54 "Content\Binary trees\transform.mc"
var tmp_0 = P1 as node; 
 #line 54 "Content\Binary trees\transform.mc"
if (tmp_0 != null) { 
 var l = tmp_0.P1; var k = tmp_0.P2; var r = tmp_0.P3; var x = P2; 
 #line 54 "Content\Binary trees\transform.mc"
if(x>k) { 
 #line 54 "Content\Binary trees\transform.mc"
var tmp_2 = contains.Create(r, x);
 #line 54 "Content\Binary trees\transform.mc"

var tmp_1 = tmp_2.Run0_();

var res = tmp_1; 
 #line 54 "Content\Binary trees\transform.mc"
var result = res;
 #line 54 "Content\Binary trees\transform.mc"
 return result;  } }
 } 

 var p = StaticRun(P1, P2); return p; 
throw new System.Exception("Error evaluating: " + new contains(P1, P2).ToString() + " no result returned."); }
public bool Run0_() { return StaticRun0_(P1, P2); }

public static bool StaticRun(BinTreeInt P1, int P2) { 
throw new System.Exception("Error evaluating: " + new contains(P1, P2).ToString() + " no result returned."); }
public bool Run(){ return StaticRun(P1, P2); }

public override string ToString() {
 var res = "("; 
if (P1 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P1.ToString(); } 

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

public override string ToString() {
 var res = "("; 

 res += " node "; if (P1 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P1.ToString(); } 
res += P2.ToString(); 
if (P3 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P3 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P3.ToString(); } 

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

  public static Expr StaticRun() {    
 { 
 #line 13 "Content\Binary trees\transform.mc"

 #line 13 "Content\Binary trees\transform.mc"
var tmp_1 = add.Create(nil.Create(), 10);
 #line 13 "Content\Binary trees\transform.mc"

var tmp_0 = tmp_1.Run0_();

var t1 = tmp_0; 
 #line 13 "Content\Binary trees\transform.mc"
var tmp_3 = add.Create(t1, 5);
 #line 13 "Content\Binary trees\transform.mc"

var tmp_2 = tmp_3.Run0_();

var t2 = tmp_2; 
 #line 13 "Content\Binary trees\transform.mc"
var tmp_5 = add.Create(t2, 7);
 #line 13 "Content\Binary trees\transform.mc"

var tmp_4 = tmp_5.Run0_();

var t2b = tmp_4; 
 #line 13 "Content\Binary trees\transform.mc"
var tmp_7 = add.Create(t2b, 15);
 #line 13 "Content\Binary trees\transform.mc"

var tmp_6 = tmp_7.Run0_();

var t3 = tmp_6; 
 #line 13 "Content\Binary trees\transform.mc"
var tmp_9 = add.Create(t3, 1);
 #line 13 "Content\Binary trees\transform.mc"

var tmp_8 = tmp_9.Run0_();

var t4 = tmp_8; 
 #line 13 "Content\Binary trees\transform.mc"
var tmp_11 = add.Create(t4, 16);
 #line 13 "Content\Binary trees\transform.mc"

var tmp_10 = tmp_11.Run0_();

var t = tmp_10; 
 #line 13 "Content\Binary trees\transform.mc"
var tmp_13 = contains.Create(t, 7);
 #line 13 "Content\Binary trees\transform.mc"

var tmp_12 = tmp_13.Run0_();

var res = tmp_12; var arg = contains.Create(t, 7); 
 #line 13 "Content\Binary trees\transform.mc"
var result = _Comma.Create(arg, res);
 #line 13 "Content\Binary trees\transform.mc"
 return result; 
 } 

  
throw new System.Exception("Error evaluating: " + new run().ToString() + " no result returned."); }
public Expr Run() { return StaticRun(); }

public static Expr StaticRun0_() { return StaticRun(); }
public Expr Run0_(){ return StaticRun0_(); }

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
