using System.Collections.Generic;
using System.Linq;
namespace Generic_lists {
 public static class Extensions { public static V GetKey<T, V>(this System.Collections.Immutable.ImmutableDictionary<T, V> self, T key) { return self[key]; } }
 public interface IRunnable { IEnumerable<IRunnable> Run();
 }


public interface Bool : Expr {}
public interface Expr : IRunnable {}
public interface Id : Expr {}
public interface List<t> : Expr, Id {}



public class _opDollar : Id  {
public int P1;

public _opDollar(int P1) {this.P1 = P1;}
public static _opDollar Create(int P1) { return new _opDollar(P1); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }

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

public override int GetHashCode() {
 return 0; 
}

}

public class _Semicolon<t> : List<t> where t : class {
public t P1;
public List<t> P2;

public _Semicolon(t P1, List<t> P2) {this.P1 = P1; this.P2 = P2;}
public static _Semicolon<t> Create(t P1, List<t> P2) { return new _Semicolon<t>(P1, P2); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }

public override string ToString() {
 var res = "("; 
res += P1.ToString(); 

 res += ";"; if (P2 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P2 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P2.ToString(); } 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _Semicolon<t>;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class contains<a> : Expr where a : class {
public List<a> P1;
public a P2;

public contains(List<a> P1, a P2) {this.P1 = P1; this.P2 = P2;}
public static contains<a> Create(List<a> P1, a P2) { return new contains<a>(P1, P2); }

  public IEnumerable<IRunnable> Run() {   
 { 
 #line 25 "Content\Generic lists\transform.mc"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _Semicolon<a>; 
 #line 25 "Content\Generic lists\transform.mc"
if (tmp_1 != null) { var x = tmp_1.P1; var xs = tmp_1.P2; var k = tmp_0.P2; 
 #line 25 "Content\Generic lists\transform.mc"
if(x.Equals(k)) { 
 #line 25 "Content\Generic lists\transform.mc"
var result = yes.Create();
 #line 25 "Content\Generic lists\transform.mc"
yield return result;  } }
 } 

  
 { 
 #line 29 "Content\Generic lists\transform.mc"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as nil<a>; 
 #line 29 "Content\Generic lists\transform.mc"
if (tmp_1 != null) { var k = tmp_0.P2; 
 #line 29 "Content\Generic lists\transform.mc"
var result = no.Create();
 #line 29 "Content\Generic lists\transform.mc"
yield return result;  }
 } 

  
 { 
 #line 32 "Content\Generic lists\transform.mc"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _Semicolon<a>; 
 #line 32 "Content\Generic lists\transform.mc"
if (tmp_1 != null) { var x = tmp_1.P1; var xs = tmp_1.P2; var k = tmp_0.P2; 
 #line 32 "Content\Generic lists\transform.mc"
if(!x.Equals(k)) { 
 #line 32 "Content\Generic lists\transform.mc"
if(xs is List<a> && k is a) { 
 #line 32 "Content\Generic lists\transform.mc"
var tmp_3 = contains<a>.Create(xs as List<a>, k as a);
 #line 32 "Content\Generic lists\transform.mc"
foreach (var tmp_2 in tmp_3.Run()) { var res = tmp_2; 
 #line 32 "Content\Generic lists\transform.mc"
var result = res;
 #line 32 "Content\Generic lists\transform.mc"
yield return result;  } } } }
 } 

  }


public override string ToString() {
 var res = "("; 
if (P1 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P1.ToString(); } 

 res += "contains"; res += P2.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as contains<a>;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class nil<t> : List<t> where t : class {

public nil() {}
public static nil<t> Create() { return new nil<t>(); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }

public override string ToString() {
return "nil";
}

public override bool Equals(object other) {
 return other is nil<t>; 
}

public override int GetHashCode() {
 return 0; 
}

}

public class no : Bool  {

public no() {}
public static no Create() { return new no(); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }

public override string ToString() {
return "no";
}

public override bool Equals(object other) {
 return other is no; 
}

public override int GetHashCode() {
 return 0; 
}

}

public class runTest1 : Expr  {

public runTest1() {}
public static runTest1 Create() { return new runTest1(); }

  public IEnumerable<IRunnable> Run() {   
 { 
 #line 19 "Content\Generic lists\transform.mc"
var tmp_0 = this as runTest1; var l = _Semicolon<_opDollar>.Create(_opDollar.Create(1), _Semicolon<_opDollar>.Create(_opDollar.Create(2), _Semicolon<_opDollar>.Create(_opDollar.Create(3), nil<_opDollar>.Create()))); 
 #line 19 "Content\Generic lists\transform.mc"
if (l is List<_opDollar>) { var p = contains<_opDollar>.Create(l as List<_opDollar>, _opDollar.Create(3)); 
 #line 19 "Content\Generic lists\transform.mc"
var tmp_2 = p;
 #line 19 "Content\Generic lists\transform.mc"
foreach (var tmp_1 in tmp_2.Run()) { var res = tmp_1; 
 #line 19 "Content\Generic lists\transform.mc"
if(p is Expr && res is Expr) { 
 #line 19 "Content\Generic lists\transform.mc"
var result = _Comma.Create(p as Expr, res as Expr);
 #line 19 "Content\Generic lists\transform.mc"
yield return result;  } } }
 } 

  }


public override string ToString() {
return "runTest1";
}

public override bool Equals(object other) {
 return other is runTest1; 
}

public override int GetHashCode() {
 return 0; 
}

}

public class yes : Bool  {

public yes() {}
public static yes Create() { return new yes(); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }

public override string ToString() {
return "yes";
}

public override bool Equals(object other) {
 return other is yes; 
}

public override int GetHashCode() {
 return 0; 
}

}




public class EntryPoint {
 static public int Print(string s) { System.Console.WriteLine(s); return 0; } 
static public IEnumerable<IRunnable> Run(bool printInput)
{
 #line 1 "input"
 var p = runTest1.Create();
if(printInput) System.Console.WriteLine(p.ToString());
foreach(var x in p.Run())
yield return x;
}
}

}
