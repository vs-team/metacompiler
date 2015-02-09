using System.Collections.Generic;
using System.Linq;
namespace Lambda_calculus {
 public static class Extensions { public static V GetKey<T, V>(this System.Collections.Immutable.ImmutableDictionary<T, V> self, T key) { return self[key]; } }
 public interface IRunnable { IEnumerable<IRunnable> Run();
IEnumerable<IRunnable> Run4_();
 }


public interface Dot : IRunnable {}
public interface Id : Term {}
public interface Term : IRunnable {}
public interface Where : IRunnable {}
public interface With : IRunnable {}



public class _Dollar : Id {
public System.String P1;

public _Dollar(System.String P1) {this.P1 = P1;}

  public IEnumerable<IRunnable> Run() {   
 { 
 #line 11 "Content\Lambda calculus\transform.mc"
var tmp_0 = this; var x = tmp_0.P1; 
 #line 11 "Content\Lambda calculus\transform.mc"
var result = new _Dollar(x);
 #line 11 "Content\Lambda calculus\transform.mc"
yield return result; 
 } 

  }

public IEnumerable<IRunnable> Run4_() { foreach(var p in Run()) yield return p; }

public override string ToString() {
 var res = "("; 

 res += "$"; res += P1.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _Dollar;
 if(tmp != null) return this.P1.Equals(tmp.P1); 
 else return false; }
}

public class _Arrow : Where {
public Term P1;
public Term P2;

public _Arrow(Term P1, Term P2) {this.P1 = P1; this.P2 = P2;}


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run4_() { foreach(var p in Run()) yield return p; }

public override string ToString() {
 var res = "("; 
res += P1.ToString(); 

 res += "->"; res += P2.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _Arrow;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
 else return false; }
}

public class _opDot : Dot {

public _opDot() {}


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run4_() { foreach(var p in Run()) yield return p; }

public override string ToString() {
return ".";
}

public override bool Equals(object other) {
 return other is _opDot; 
}
}

public class _opSlash : Term {
public Id P1;
public Dot P2;
public Term P3;

public _opSlash(Id P1, Dot P2, Term P3) {this.P1 = P1; this.P2 = P2; this.P3 = P3;}

  public IEnumerable<IRunnable> Run() {   
 { 
 #line 14 "Content\Lambda calculus\transform.mc"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _Dollar; 
 #line 14 "Content\Lambda calculus\transform.mc"
if (tmp_1 != null) { var x = tmp_1.P1; var tmp_2 = tmp_0.P2 as _opDot; 
 #line 14 "Content\Lambda calculus\transform.mc"
if (tmp_2 != null) { var t = tmp_0.P3; 
 #line 14 "Content\Lambda calculus\transform.mc"
if(t is Term) { 
 #line 14 "Content\Lambda calculus\transform.mc"
var result = new _opSlash(new _Dollar(x), new _opDot(), t as Term);
 #line 14 "Content\Lambda calculus\transform.mc"
yield return result;  } } }
 } 

  }

public IEnumerable<IRunnable> Run4_() { foreach(var p in Run()) yield return p; }

public override string ToString() {
 var res = "("; 

 res += "\\"; res += P1.ToString(); 
res += P2.ToString(); 
res += P3.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _opSlash;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2) && this.P3.Equals(tmp.P3); 
 else return false; }
}

public class _With : With {
public Term P1;
public Where P2;

public _With(Term P1, Where P2) {this.P1 = P1; this.P2 = P2;}

  public IEnumerable<IRunnable> Run4_() {   
 { 
 #line 30 "Content\Lambda calculus\transform.mc"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _Dollar; 
 #line 30 "Content\Lambda calculus\transform.mc"
if (tmp_1 != null) { var y = tmp_1.P1; var tmp_2 = tmp_0.P2 as _Arrow; 
 #line 30 "Content\Lambda calculus\transform.mc"
if (tmp_2 != null) { var tmp_3 = tmp_2.P1 as _Dollar; 
 #line 30 "Content\Lambda calculus\transform.mc"
if (tmp_3 != null) { var x = tmp_3.P1; var u = tmp_2.P2; 
 #line 30 "Content\Lambda calculus\transform.mc"
if(x.Equals(y)) { 
 #line 30 "Content\Lambda calculus\transform.mc"
var result = u;
 #line 30 "Content\Lambda calculus\transform.mc"
yield return result;  } } } }
 } 

  
 { 
 #line 34 "Content\Lambda calculus\transform.mc"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _Dollar; 
 #line 34 "Content\Lambda calculus\transform.mc"
if (tmp_1 != null) { var y = tmp_1.P1; var tmp_2 = tmp_0.P2 as _Arrow; 
 #line 34 "Content\Lambda calculus\transform.mc"
if (tmp_2 != null) { var tmp_3 = tmp_2.P1 as _Dollar; 
 #line 34 "Content\Lambda calculus\transform.mc"
if (tmp_3 != null) { var x = tmp_3.P1; var u = tmp_2.P2; 
 #line 34 "Content\Lambda calculus\transform.mc"
if(!x.Equals(y)) { 
 #line 34 "Content\Lambda calculus\transform.mc"
var result = new _Dollar(y);
 #line 34 "Content\Lambda calculus\transform.mc"
yield return result;  } } } }
 } 

  
 { 
 #line 38 "Content\Lambda calculus\transform.mc"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opSlash; 
 #line 38 "Content\Lambda calculus\transform.mc"
if (tmp_1 != null) { var tmp_2 = tmp_1.P1 as _Dollar; 
 #line 38 "Content\Lambda calculus\transform.mc"
if (tmp_2 != null) { var x = tmp_2.P1; var tmp_3 = tmp_1.P2 as _opDot; 
 #line 38 "Content\Lambda calculus\transform.mc"
if (tmp_3 != null) { var t = tmp_1.P3; var tmp_4 = tmp_0.P2 as _Arrow; 
 #line 38 "Content\Lambda calculus\transform.mc"
if (tmp_4 != null) { var tmp_5 = tmp_4.P1 as _Dollar; 
 #line 38 "Content\Lambda calculus\transform.mc"
if (tmp_5 != null) { var y = tmp_5.P1; var u = tmp_4.P2; 
 #line 38 "Content\Lambda calculus\transform.mc"
if(x.Equals(y)) { 
 #line 38 "Content\Lambda calculus\transform.mc"
if(t is Term) { 
 #line 38 "Content\Lambda calculus\transform.mc"
var result = new _opSlash(new _Dollar(x), new _opDot(), t as Term);
 #line 38 "Content\Lambda calculus\transform.mc"
yield return result;  } } } } } } }
 } 

  
 { 
 #line 42 "Content\Lambda calculus\transform.mc"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opSlash; 
 #line 42 "Content\Lambda calculus\transform.mc"
if (tmp_1 != null) { var tmp_2 = tmp_1.P1 as _Dollar; 
 #line 42 "Content\Lambda calculus\transform.mc"
if (tmp_2 != null) { var x = tmp_2.P1; var tmp_3 = tmp_1.P2 as _opDot; 
 #line 42 "Content\Lambda calculus\transform.mc"
if (tmp_3 != null) { var t = tmp_1.P3; var tmp_4 = tmp_0.P2 as _Arrow; 
 #line 42 "Content\Lambda calculus\transform.mc"
if (tmp_4 != null) { var tmp_5 = tmp_4.P1 as _Dollar; 
 #line 42 "Content\Lambda calculus\transform.mc"
if (tmp_5 != null) { var y = tmp_5.P1; var u = tmp_4.P2; 
 #line 42 "Content\Lambda calculus\transform.mc"
if(!x.Equals(y)) { 
 #line 42 "Content\Lambda calculus\transform.mc"
if(t is Term && u is Term) { 
 #line 42 "Content\Lambda calculus\transform.mc"
var tmp_7 = new _With(t as Term, new _Arrow(new _Dollar(y), u as Term));
 #line 42 "Content\Lambda calculus\transform.mc"
foreach (var tmp_6 in tmp_7.Run4_()) { var t_Prime = tmp_6; 
 #line 42 "Content\Lambda calculus\transform.mc"
if(t_Prime is Term) { 
 #line 42 "Content\Lambda calculus\transform.mc"
var result = new _opSlash(new _Dollar(x), new _opDot(), t_Prime as Term);
 #line 42 "Content\Lambda calculus\transform.mc"
yield return result;  } } } } } } } } }
 } 

  
 { 
 #line 47 "Content\Lambda calculus\transform.mc"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opVBar; 
 #line 47 "Content\Lambda calculus\transform.mc"
if (tmp_1 != null) { var t = tmp_1.P1; var u = tmp_1.P2; var tmp_2 = tmp_0.P2 as _Arrow; 
 #line 47 "Content\Lambda calculus\transform.mc"
if (tmp_2 != null) { var tmp_3 = tmp_2.P1 as _Dollar; 
 #line 47 "Content\Lambda calculus\transform.mc"
if (tmp_3 != null) { var x = tmp_3.P1; var v = tmp_2.P2; 
 #line 47 "Content\Lambda calculus\transform.mc"
if(t is Term && v is Term) { 
 #line 47 "Content\Lambda calculus\transform.mc"
var tmp_5 = new _With(t as Term, new _Arrow(new _Dollar(x), v as Term));
 #line 47 "Content\Lambda calculus\transform.mc"
foreach (var tmp_4 in tmp_5.Run4_()) { var t_Prime = tmp_4; 
 #line 47 "Content\Lambda calculus\transform.mc"
if(u is Term && v is Term) { 
 #line 47 "Content\Lambda calculus\transform.mc"
var tmp_7 = new _With(u as Term, new _Arrow(new _Dollar(x), v as Term));
 #line 47 "Content\Lambda calculus\transform.mc"
foreach (var tmp_6 in tmp_7.Run4_()) { var u_Prime = tmp_6; 
 #line 47 "Content\Lambda calculus\transform.mc"
if(t_Prime is Term && u_Prime is Term) { 
 #line 47 "Content\Lambda calculus\transform.mc"
var result = new _opVBar(t_Prime as Term, u_Prime as Term);
 #line 47 "Content\Lambda calculus\transform.mc"
yield return result;  } } } } } } } }
 } 

 foreach(var p in Run()) yield return p; }

public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }

public override string ToString() {
 var res = "("; 
res += P1.ToString(); 

 res += "with"; res += P2.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _With;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
 else return false; }
}

public class _opVBar : Term {
public Term P1;
public Term P2;

public _opVBar(Term P1, Term P2) {this.P1 = P1; this.P2 = P2;}

  public IEnumerable<IRunnable> Run() {   
 { 
 #line 17 "Content\Lambda calculus\transform.mc"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _Dollar; 
 #line 17 "Content\Lambda calculus\transform.mc"
if (tmp_1 != null) { var x = tmp_1.P1; var u = tmp_0.P2; 
 #line 17 "Content\Lambda calculus\transform.mc"
if(u is Term) { 
 #line 17 "Content\Lambda calculus\transform.mc"
var result = new _opVBar(new _Dollar(x), u as Term);
 #line 17 "Content\Lambda calculus\transform.mc"
yield return result;  } }
 } 

  
 { 
 #line 20 "Content\Lambda calculus\transform.mc"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opVBar; 
 #line 20 "Content\Lambda calculus\transform.mc"
if (tmp_1 != null) { var u = tmp_1.P1; var v = tmp_1.P2; var w = tmp_0.P2; 
 #line 20 "Content\Lambda calculus\transform.mc"
if(u is Term && v is Term) { 
 #line 20 "Content\Lambda calculus\transform.mc"
var tmp_3 = new _opVBar(u as Term, v as Term);
 #line 20 "Content\Lambda calculus\transform.mc"
foreach (var tmp_2 in tmp_3.Run()) { var v_Prime = tmp_2; 
 #line 20 "Content\Lambda calculus\transform.mc"
if(v_Prime is Term && w is Term) { 
 #line 20 "Content\Lambda calculus\transform.mc"
var tmp_5 = new _opVBar(v_Prime as Term, w as Term);
 #line 20 "Content\Lambda calculus\transform.mc"
foreach (var tmp_4 in tmp_5.Run()) { var res = tmp_4; 
 #line 20 "Content\Lambda calculus\transform.mc"
var result = res;
 #line 20 "Content\Lambda calculus\transform.mc"
yield return result;  } } } } }
 } 

  
 { 
 #line 25 "Content\Lambda calculus\transform.mc"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opSlash; 
 #line 25 "Content\Lambda calculus\transform.mc"
if (tmp_1 != null) { var tmp_2 = tmp_1.P1 as _Dollar; 
 #line 25 "Content\Lambda calculus\transform.mc"
if (tmp_2 != null) { var x = tmp_2.P1; var tmp_3 = tmp_1.P2 as _opDot; 
 #line 25 "Content\Lambda calculus\transform.mc"
if (tmp_3 != null) { var t = tmp_1.P3; var u = tmp_0.P2; 
 #line 25 "Content\Lambda calculus\transform.mc"
if (t is Term && u is Term) { var term = new _With(t as Term, new _Arrow(new _Dollar(x), u as Term)); 
 #line 25 "Content\Lambda calculus\transform.mc"
var tmp_5 = term;
 #line 25 "Content\Lambda calculus\transform.mc"
foreach (var tmp_4 in tmp_5.Run4_()) { var t_Prime = tmp_4; 
 #line 25 "Content\Lambda calculus\transform.mc"
var result = t_Prime;
 #line 25 "Content\Lambda calculus\transform.mc"
yield return result;  } } } } }
 } 

  }

public IEnumerable<IRunnable> Run4_() { foreach(var p in Run()) yield return p; }

public override string ToString() {
 var res = "("; 
res += P1.ToString(); 

 res += "|"; res += P2.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _opVBar;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
 else return false; }
}




public class EntryPoint {
 static public IEnumerable<IRunnable> Run(bool printInput)
{
var p = new _opVBar(new _opVBar(new _opSlash(new _Dollar("y"), new _opDot(), new _Dollar("y")), new _opSlash(new _Dollar("y"), new _opDot(), new _Dollar("y"))), new _opVBar(new _Dollar("x"), new _Dollar("z")));
if(printInput) System.Console.WriteLine(p.ToString());
foreach(var x in p.Run())
yield return x;
}
}

}
