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

public class contains : Expr  {
public ListInt P1;
public int P2;

public contains(ListInt P1, int P2) {this.P1 = P1; this.P2 = P2;}
public static contains Create(ListInt P1, int P2) { return new contains(P1, P2); }

  public IEnumerable<IRunnable> Run() {   
 { 
 #line 12 "Content\Lists\transform.mc"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as nil; 
 #line 12 "Content\Lists\transform.mc"
if (tmp_1 != null) { var k = tmp_0.P2; 
 #line 12 "Content\Lists\transform.mc"
var result = no.Create();
 #line 12 "Content\Lists\transform.mc"
yield return result;  }
 } 

  
 { 
 #line 15 "Content\Lists\transform.mc"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _Semicolon; 
 #line 15 "Content\Lists\transform.mc"
if (tmp_1 != null) { var tmp_2 = tmp_1.P1 as _opDollar; 
 #line 15 "Content\Lists\transform.mc"
if (tmp_2 != null) { var x = tmp_2.P1; var xs = tmp_1.P2; var k = tmp_0.P2; 
 #line 15 "Content\Lists\transform.mc"
if(x.Equals(k)) { 
 #line 15 "Content\Lists\transform.mc"
var result = yes.Create();
 #line 15 "Content\Lists\transform.mc"
yield return result;  } } }
 } 

  
 { 
 #line 19 "Content\Lists\transform.mc"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _Semicolon; 
 #line 19 "Content\Lists\transform.mc"
if (tmp_1 != null) { var tmp_2 = tmp_1.P1 as _opDollar; 
 #line 19 "Content\Lists\transform.mc"
if (tmp_2 != null) { var x = tmp_2.P1; var xs = tmp_1.P2; var k = tmp_0.P2; 
 #line 19 "Content\Lists\transform.mc"
if(!x.Equals(k)) { 
 #line 19 "Content\Lists\transform.mc"
if(xs is ListInt) { 
 #line 19 "Content\Lists\transform.mc"
var tmp_4 = contains.Create(xs as ListInt, k);
 #line 19 "Content\Lists\transform.mc"
foreach (var tmp_3 in tmp_4.Run()) { var res = tmp_3; 
 #line 19 "Content\Lists\transform.mc"
var result = res;
 #line 19 "Content\Lists\transform.mc"
yield return result;  } } } } }
 } 

  
 { 
 #line 25 "Content\Lists\transform.mc"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _Semicolon; 
 #line 25 "Content\Lists\transform.mc"
if (tmp_1 != null) { var tmp_2 = tmp_1.P1 as _opDollar; 
 #line 25 "Content\Lists\transform.mc"
if (tmp_2 != null) { var x = tmp_2.P1; var xs = tmp_1.P2; var k = tmp_0.P2; 
 #line 25 "Content\Lists\transform.mc"
if(x.Equals(k)) { 
 #line 25 "Content\Lists\transform.mc"
var result = yes.Create();
 #line 25 "Content\Lists\transform.mc"
yield return result;  } } }
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
 #line 30 "Content\Lists\transform.mc"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as nil; 
 #line 30 "Content\Lists\transform.mc"
if (tmp_1 != null) { 
 #line 30 "Content\Lists\transform.mc"
var result = nil.Create();
 #line 30 "Content\Lists\transform.mc"
yield return result;  }
 } 

  
 { 
 #line 33 "Content\Lists\transform.mc"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _Semicolon; 
 #line 33 "Content\Lists\transform.mc"
if (tmp_1 != null) { var tmp_2 = tmp_1.P1 as _opDollar; 
 #line 33 "Content\Lists\transform.mc"
if (tmp_2 != null) { var x = tmp_2.P1; var xs = tmp_1.P2; 
 #line 33 "Content\Lists\transform.mc"
if(x%2== 0) { 
 #line 33 "Content\Lists\transform.mc"
if(xs is ListInt) { 
 #line 33 "Content\Lists\transform.mc"
var tmp_4 = removeOdd.Create(xs as ListInt);
 #line 33 "Content\Lists\transform.mc"
foreach (var tmp_3 in tmp_4.Run()) { var xs_Prime = tmp_3; 
 #line 33 "Content\Lists\transform.mc"
var result = xs_Prime;
 #line 33 "Content\Lists\transform.mc"
yield return result;  } } } } }
 } 

  
 { 
 #line 38 "Content\Lists\transform.mc"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _Semicolon; 
 #line 38 "Content\Lists\transform.mc"
if (tmp_1 != null) { var tmp_2 = tmp_1.P1 as _opDollar; 
 #line 38 "Content\Lists\transform.mc"
if (tmp_2 != null) { var x = tmp_2.P1; var xs = tmp_1.P2; 
 #line 38 "Content\Lists\transform.mc"
if(x%2!= 0) { 
 #line 38 "Content\Lists\transform.mc"
if(xs is ListInt) { 
 #line 38 "Content\Lists\transform.mc"
var tmp_4 = removeOdd.Create(xs as ListInt);
 #line 38 "Content\Lists\transform.mc"
foreach (var tmp_3 in tmp_4.Run()) { var xs_Prime = tmp_3; 
 #line 38 "Content\Lists\transform.mc"
if(xs_Prime is ListInt) { 
 #line 38 "Content\Lists\transform.mc"
var result = _Semicolon.Create(_opDollar.Create(x), xs_Prime as ListInt);
 #line 38 "Content\Lists\transform.mc"
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
 var p = removeOdd.Create(_Semicolon.Create(_opDollar.Create(0), _Semicolon.Create(_opDollar.Create(1), _Semicolon.Create(_opDollar.Create(2), _Semicolon.Create(_opDollar.Create(3), nil.Create())))));
if(printInput) System.Console.WriteLine(p.ToString());
foreach(var x in p.Run())
yield return x;
}
}

}
