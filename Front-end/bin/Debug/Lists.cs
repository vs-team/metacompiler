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
public int P1;
public ListInt P2;

public _Semicolon(int P1, ListInt P2) {this.P1 = P1; this.P2 = P2;}
public static _Semicolon Create(int P1, ListInt P2) { return new _Semicolon(P1, P2); }


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
 #line 24 "Content\Lists"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as nil; 
 #line 24 "Content\Lists"
if (tmp_1 != null) { 
 #line 24 "Content\Lists"
var result = _opDollar.Create(0);
 #line 24 "Content\Lists"
yield return result;  }
 } 

  
 { 
 #line 29 "Content\Lists"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _Semicolon; 
 #line 29 "Content\Lists"
if (tmp_1 != null) { var x = tmp_1.P1; var xs = tmp_1.P2; 
 #line 29 "Content\Lists"
if(xs is ListInt) { 
 #line 29 "Content\Lists"
var tmp_3 = add.Create(xs as ListInt);
 #line 29 "Content\Lists"
foreach (var tmp_2 in tmp_3.Run()) { var tmp_4 = tmp_2 as _opDollar; 
 #line 29 "Content\Lists"
if (tmp_4 != null) { var res = tmp_4.P1; 
 #line 29 "Content\Lists"
var result = _opDollar.Create(x+res);
 #line 29 "Content\Lists"
yield return result;  } } } }
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
 #line 37 "Content\Lists"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as nil; 
 #line 37 "Content\Lists"
if (tmp_1 != null) { var k = tmp_0.P2; 
 #line 37 "Content\Lists"
var result = no.Create();
 #line 37 "Content\Lists"
yield return result;  }
 } 

  
 { 
 #line 42 "Content\Lists"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _Semicolon; 
 #line 42 "Content\Lists"
if (tmp_1 != null) { var x = tmp_1.P1; var xs = tmp_1.P2; var k = tmp_0.P2; 
 #line 42 "Content\Lists"
if(x.Equals(k)) { 
 #line 42 "Content\Lists"
var result = yes.Create();
 #line 42 "Content\Lists"
yield return result;  } }
 } 

  
 { 
 #line 49 "Content\Lists"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _Semicolon; 
 #line 49 "Content\Lists"
if (tmp_1 != null) { var x = tmp_1.P1; var xs = tmp_1.P2; var k = tmp_0.P2; 
 #line 49 "Content\Lists"
if(!x.Equals(k)) { 
 #line 49 "Content\Lists"
if(xs is ListInt) { 
 #line 49 "Content\Lists"
var tmp_3 = contains.Create(xs as ListInt, k);
 #line 49 "Content\Lists"
foreach (var tmp_2 in tmp_3.Run()) { var res = tmp_2; 
 #line 49 "Content\Lists"
var result = res;
 #line 49 "Content\Lists"
yield return result;  } } } }
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
 #line 59 "Content\Lists"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as nil; 
 #line 59 "Content\Lists"
if (tmp_1 != null) { 
 #line 59 "Content\Lists"
var result = nil.Create();
 #line 59 "Content\Lists"
yield return result;  }
 } 

  
 { 
 #line 64 "Content\Lists"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _Semicolon; 
 #line 64 "Content\Lists"
if (tmp_1 != null) { var x = tmp_1.P1; var xs = tmp_1.P2; 
 #line 64 "Content\Lists"
if((x%2).Equals(0)) { 
 #line 64 "Content\Lists"
if(xs is ListInt) { 
 #line 64 "Content\Lists"
var tmp_3 = removeOdd.Create(xs as ListInt);
 #line 64 "Content\Lists"
foreach (var tmp_2 in tmp_3.Run()) { var xs_Prime = tmp_2; 
 #line 64 "Content\Lists"
var result = xs_Prime;
 #line 64 "Content\Lists"
yield return result;  } } } }
 } 

  
 { 
 #line 73 "Content\Lists"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _Semicolon; 
 #line 73 "Content\Lists"
if (tmp_1 != null) { var x = tmp_1.P1; var xs = tmp_1.P2; 
 #line 73 "Content\Lists"
if((x%2).Equals(1)) { 
 #line 73 "Content\Lists"
if(xs is ListInt) { 
 #line 73 "Content\Lists"
var tmp_3 = removeOdd.Create(xs as ListInt);
 #line 73 "Content\Lists"
foreach (var tmp_2 in tmp_3.Run()) { var xs_Prime = tmp_2; 
 #line 73 "Content\Lists"
if(xs_Prime is ListInt) { 
 #line 73 "Content\Lists"
var result = _Semicolon.Create(x, xs_Prime as ListInt);
 #line 73 "Content\Lists"
yield return result;  } } } } }
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
 static public int Sleep(float s) { int t = (int)(s * 1000.0f); ; return 0; } 
static public IEnumerable<IRunnable> Run(bool printInput)
{
 #line 1 "input"
 var p = add.Create(_Semicolon.Create(0, _Semicolon.Create(1, _Semicolon.Create(2, _Semicolon.Create(3, nil.Create())))));
if(printInput) System.Console.WriteLine(p.ToString());
foreach(var x in p.Run())
yield return x;
}
}

}
