using System;
using System.Collections.Generic;
using System.Linq;
namespace Lists {
 public static class Extensions { public static V GetKey<T, V>(this System.Collections.Immutable.ImmutableDictionary<T, V> self, T key) { return self[key]; } }
 public interface IRunnable { IEnumerable<IRunnable> Run();
 }


public interface Bool : IRunnable {}
public interface Expr : IRunnable {}
public interface IntValue : IRunnable {}
public interface ListInt : IRunnable {}



public class _opDollar : IntValue  {
public int P1;

public _opDollar(int P1) {this.P1 = P1;}
public static _opDollar Create(int P1) { return new _opDollar(P1); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }

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

public class _Semicolon : ListInt  {
public IntValue P1;
public ListInt P2;

public _Semicolon(IntValue P1, ListInt P2) {this.P1 = P1; this.P2 = P2;}
public static _Semicolon Create(IntValue P1, ListInt P2) { return new _Semicolon(P1, P2); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }

public override string ToString() {
 var res = "("; 
res += P1.ToString(); 

 res += " ; "; res += P2.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _Semicolon;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class add : Expr  {
public ListInt P1;

public add(ListInt P1) {this.P1 = P1;}
public static add Create(ListInt P1) { return new add(P1); }

  public IEnumerable<IRunnable> Run() {   
 { 
 #line 16 ""
var tmp_0 = this; var tmp_1 = tmp_0.P1 as nil; 
 #line 16 ""
if (tmp_1 != null) { 
 #line 16 ""
var result = _opDollar.Create(0);
 #line 16 ""
yield return result;  }
 } 

  
 { 
 #line 19 ""
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _Semicolon; 
 #line 19 ""
if (tmp_1 != null) { var tmp_2 = tmp_1.P1 as _opDollar; 
 #line 19 ""
if (tmp_2 != null) { var x = tmp_2.P1; var xs = tmp_1.P2; 
 #line 19 ""
if(xs is ListInt) { 
 #line 19 ""
var tmp_4 = add.Create(xs as ListInt);
 #line 19 ""
foreach (var tmp_3 in tmp_4.Run()) { var tmp_5 = tmp_3 as _opDollar; 
 #line 19 ""
if (tmp_5 != null) { var res = tmp_5.P1; var res_Prime = (x+res); 
 #line 19 ""
var result = _opDollar.Create(res_Prime);
 #line 19 ""
yield return result;  } } } } }
 } 

  }


public override string ToString() {
 var res = "("; 

 res += " add "; res += P1.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as add;
 if(tmp != null) return this.P1.Equals(tmp.P1); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class contains : Expr  {
public ListInt P1;
public int P2;

public contains(ListInt P1, int P2) {this.P1 = P1; this.P2 = P2;}
public static contains Create(ListInt P1, int P2) { return new contains(P1, P2); }

  public IEnumerable<IRunnable> Run() {   
 { 
 #line 25 ""
var tmp_0 = this; var tmp_1 = tmp_0.P1 as nil; 
 #line 25 ""
if (tmp_1 != null) { var k = tmp_0.P2; 
 #line 25 ""
var result = no.Create();
 #line 25 ""
yield return result;  }
 } 

  
 { 
 #line 28 ""
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _Semicolon; 
 #line 28 ""
if (tmp_1 != null) { var tmp_2 = tmp_1.P1 as _opDollar; 
 #line 28 ""
if (tmp_2 != null) { var x = tmp_2.P1; var xs = tmp_1.P2; var k = tmp_0.P2; 
 #line 28 ""
if(x.Equals(k)) { 
 #line 28 ""
var result = yes.Create();
 #line 28 ""
yield return result;  } } }
 } 

  
 { 
 #line 32 ""
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _Semicolon; 
 #line 32 ""
if (tmp_1 != null) { var tmp_2 = tmp_1.P1 as _opDollar; 
 #line 32 ""
if (tmp_2 != null) { var x = tmp_2.P1; var xs = tmp_1.P2; var k = tmp_0.P2; 
 #line 32 ""
if(!x.Equals(k)) { 
 #line 32 ""
if(xs is ListInt) { 
 #line 32 ""
var tmp_4 = contains.Create(xs as ListInt, k);
 #line 32 ""
foreach (var tmp_3 in tmp_4.Run()) { var res = tmp_3; 
 #line 32 ""
var result = res;
 #line 32 ""
yield return result;  } } } } }
 } 

  }


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

public class nil : ListInt  {

public nil() {}
public static nil Create() { return new nil(); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }

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

public class removeOdd : Expr  {
public ListInt P1;

public removeOdd(ListInt P1) {this.P1 = P1;}
public static removeOdd Create(ListInt P1) { return new removeOdd(P1); }

  public IEnumerable<IRunnable> Run() {   
 { 
 #line 38 ""
var tmp_0 = this; var tmp_1 = tmp_0.P1 as nil; 
 #line 38 ""
if (tmp_1 != null) { 
 #line 38 ""
var result = nil.Create();
 #line 38 ""
yield return result;  }
 } 

  
 { 
 #line 41 ""
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _Semicolon; 
 #line 41 ""
if (tmp_1 != null) { var tmp_2 = tmp_1.P1 as _opDollar; 
 #line 41 ""
if (tmp_2 != null) { var x = tmp_2.P1; var xs = tmp_1.P2; 
 #line 41 ""
if((x%2).Equals(0)) { 
 #line 41 ""
if(xs is ListInt) { 
 #line 41 ""
var tmp_4 = removeOdd.Create(xs as ListInt);
 #line 41 ""
foreach (var tmp_3 in tmp_4.Run()) { var xs_Prime = tmp_3; 
 #line 41 ""
var result = xs_Prime;
 #line 41 ""
yield return result;  } } } } }
 } 

  
 { 
 #line 46 ""
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _Semicolon; 
 #line 46 ""
if (tmp_1 != null) { var tmp_2 = tmp_1.P1 as _opDollar; 
 #line 46 ""
if (tmp_2 != null) { var x = tmp_2.P1; var xs = tmp_1.P2; 
 #line 46 ""
if((x%2).Equals(1)) { 
 #line 46 ""
if(xs is ListInt) { 
 #line 46 ""
var tmp_4 = removeOdd.Create(xs as ListInt);
 #line 46 ""
foreach (var tmp_3 in tmp_4.Run()) { var xs_Prime = tmp_3; 
 #line 46 ""
if(xs_Prime is ListInt) { 
 #line 46 ""
var result = _Semicolon.Create(_opDollar.Create(x), xs_Prime as ListInt);
 #line 46 ""
yield return result;  } } } } } }
 } 

  }


public override string ToString() {
 var res = "("; 

 res += " removeOdd "; res += P1.ToString(); 

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
 static public int Print(object s) { System.Console.WriteLine(s.ToString()); return 0; } 
 static public int Sleep(float s) { int t = (int)(s * 1000.0f); System.Threading.Thread.Sleep(t); return 0; } 
static public IEnumerable<IRunnable> Run(bool printInput)
{
 #line 1 "input"
 var p = add.Create(_Semicolon.Create(_opDollar.Create(0), _Semicolon.Create(_opDollar.Create(1), _Semicolon.Create(_opDollar.Create(2), _Semicolon.Create(_opDollar.Create(3), nil.Create())))));
if(printInput) System.Console.WriteLine(p.ToString());
foreach(var x in p.Run())
yield return x;
}
}

}
