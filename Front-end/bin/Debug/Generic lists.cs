using System;
using System.Collections.Generic;
using System.Linq;
namespace Generic_lists {
 public static class Extensions { public static V GetKey<T, V>(this System.Collections.Immutable.ImmutableDictionary<T, V> self, T key) { return self[key]; } }



public interface Pair<a, b> {}
public interface List<a> {}
public interface Expr {}



public class _Comma<a, b> : Pair<a, b>  {
public a P1;
public b P2;

public _Comma(a P1, b P2) {this.P1 = P1; this.P2 = P2;}
public static _Comma<a, b> Create(a P1, b P2) { return new _Comma<a, b>(P1, P2); }

public override string ToString() {
 var res = "("; 
res += P1.ToString(); 

 res += " , "; res += P2.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _Comma<a, b>;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class _Semicolon<a> : List<a>  {
public a P1;
public List<a> P2;

public _Semicolon(a P1, List<a> P2) {this.P1 = P1; this.P2 = P2;}
public static _Semicolon<a> Create(a P1, List<a> P2) { return new _Semicolon<a>(P1, P2); }

public override string ToString() {
 var res = "("; 
res += P1.ToString(); 

 res += " ; "; if (P2 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P2 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P2.ToString(); } 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _Semicolon<a>;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class length<a> : Expr  {
public List<a> P1;

public length(List<a> P1) {this.P1 = P1;}
public static length<a> Create(List<a> P1) { return new length<a>(P1); }

  public static int StaticRun(List<a> P1) {    
 { 
 #line 24 "Content\Generic lists\transform.mc"
var tmp_0 = P1 as nil<a>; 
 #line 24 "Content\Generic lists\transform.mc"
if (tmp_0 != null) { 
 
 #line 24 "Content\Generic lists\transform.mc"
var result = 0;
 #line 24 "Content\Generic lists\transform.mc"
 return result;  }
 } 

  
 { 
 #line 27 "Content\Generic lists\transform.mc"
var tmp_0 = P1 as _Semicolon<a>; 
 #line 27 "Content\Generic lists\transform.mc"
if (tmp_0 != null) { 
 var x = tmp_0.P1; var xs = tmp_0.P2; 
 #line 27 "Content\Generic lists\transform.mc"
var tmp_2 = length<a>.StaticRun(xs);
 #line 27 "Content\Generic lists\transform.mc"

var tmp_1 = tmp_2;

var y = tmp_1; 
 #line 27 "Content\Generic lists\transform.mc"
var result = (1+y);
 #line 27 "Content\Generic lists\transform.mc"
 return result;  }
 } 

  
throw new System.Exception("Error evaluating: length. No result returned."); }
public int Run() { return StaticRun(P1); }


public override string ToString() {
 var res = "("; 

 res += " length "; if (P1 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P1.ToString(); } 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as length<a>;
 if(tmp != null) return this.P1.Equals(tmp.P1); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class nil<a> : List<a>  {

public nil() {}
public static nil<a> Create() { return new nil<a>(); }

public override string ToString() {
return "nil";
}

public override bool Equals(object other) {
 return other is nil<a>; 
}

public override int GetHashCode() {
 return 0; 
}

}

public class removeOdd : Expr  {
public List<int> P1;

public removeOdd(List<int> P1) {this.P1 = P1;}
public static removeOdd Create(List<int> P1) { return new removeOdd(P1); }

  public static List<int> StaticRun(List<int> P1) {    
 { 
 #line 32 "Content\Generic lists\transform.mc"
var tmp_0 = P1 as nil<int>; 
 #line 32 "Content\Generic lists\transform.mc"
if (tmp_0 != null) { 
 
 #line 32 "Content\Generic lists\transform.mc"
var result = nil<int>.Create();
 #line 32 "Content\Generic lists\transform.mc"
 return result;  }
 } 

  
 { 
 #line 35 "Content\Generic lists\transform.mc"
var tmp_0 = P1 as _Semicolon<int>; 
 #line 35 "Content\Generic lists\transform.mc"
if (tmp_0 != null) { 
 var x = tmp_0.P1; var xs = tmp_0.P2; 
 #line 35 "Content\Generic lists\transform.mc"
if((x%2).Equals(0)) { 
 #line 35 "Content\Generic lists\transform.mc"
var tmp_2 = removeOdd.StaticRun(xs);
 #line 35 "Content\Generic lists\transform.mc"

var tmp_1 = tmp_2;

var xs_Prime = tmp_1; 
 #line 35 "Content\Generic lists\transform.mc"
var result = xs_Prime;
 #line 35 "Content\Generic lists\transform.mc"
 return result;  } }
 } 

  
 { 
 #line 40 "Content\Generic lists\transform.mc"
var tmp_0 = P1 as _Semicolon<int>; 
 #line 40 "Content\Generic lists\transform.mc"
if (tmp_0 != null) { 
 var x = tmp_0.P1; var xs = tmp_0.P2; 
 #line 40 "Content\Generic lists\transform.mc"
if((x%2).Equals(1)) { 
 #line 40 "Content\Generic lists\transform.mc"
var tmp_2 = removeOdd.StaticRun(xs);
 #line 40 "Content\Generic lists\transform.mc"

var tmp_1 = tmp_2;

var xs_Prime = tmp_1; 
 #line 40 "Content\Generic lists\transform.mc"
var result = _Semicolon<int>.Create(x, xs_Prime);
 #line 40 "Content\Generic lists\transform.mc"
 return result;  } }
 } 

  
throw new System.Exception("Error evaluating: removeOdd. No result returned."); }
public List<int> Run() { return StaticRun(P1); }


public override string ToString() {
 var res = "("; 

 res += " removeOdd "; if (P1 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P1.ToString(); } 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as removeOdd;
 if(tmp != null) return this.P1.Equals(tmp.P1); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class run : Expr  {

public run() {}
public static run Create() { return new run(); }

  public static Pair<Expr, List<int>> StaticRun() {    
 { 
 #line 15 "Content\Generic lists\transform.mc"
var l1 = _Semicolon<string>.Create("x", _Semicolon<string>.Create("y", _Semicolon<string>.Create("z", nil<string>.Create()))); var l2 = _Semicolon<int>.Create(1, _Semicolon<int>.Create(2, _Semicolon<int>.Create(3, _Semicolon<int>.Create(4, _Semicolon<int>.Create(5, nil<int>.Create()))))); 
 #line 15 "Content\Generic lists\transform.mc"
var tmp_1 = length<string>.StaticRun(l1);
 #line 15 "Content\Generic lists\transform.mc"

var tmp_0 = tmp_1;

var x = tmp_0; 
 #line 15 "Content\Generic lists\transform.mc"
var tmp_3 = removeOdd.StaticRun(l2);
 #line 15 "Content\Generic lists\transform.mc"

var tmp_2 = tmp_3;

var y = tmp_2; var len = length<string>.Create(l1); 
 #line 15 "Content\Generic lists\transform.mc"
var result = _Comma<Expr, List<int>>.Create(len, y);
 #line 15 "Content\Generic lists\transform.mc"
 return result; 
 } 

  
throw new System.Exception("Error evaluating: run. No result returned."); }
public Pair<Expr, List<int>> Run() { return StaticRun(); }


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
