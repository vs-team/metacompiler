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



public class _opDollar : Id {
public System.String P1;

public _opDollar(System.String P1) {this.P1 = P1;}
public static _opDollar Create(System.String P1) { return new _opDollar(P1); }

  public IEnumerable<IRunnable> Run() {   
 { 
 var tmp_0 = this; var x = tmp_0.P1; 
var result = _opDollar.Create(x);
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
 var tmp = other as _opDollar;
 if(tmp != null) return this.P1.Equals(tmp.P1); 
 else return false; }
}

public class _Arrow : Where {
public Term P1;
public Term P2;

public _Arrow(Term P1, Term P2) {this.P1 = P1; this.P2 = P2;}
public static _Arrow Create(Term P1, Term P2) { return new _Arrow(P1, P2); }


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
public static _opDot Create() { return new _opDot(); }


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
public static _opSlash Create(Id P1, Dot P2, Term P3) { return new _opSlash(P1, P2, P3); }

  public IEnumerable<IRunnable> Run() {   
 { 
 var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opDollar; 
if (tmp_1 != null) { var x = tmp_1.P1; var tmp_2 = tmp_0.P2 as _opDot; 
if (tmp_2 != null) { var t = tmp_0.P3; 
if(t is Term) { 
var result = _opSlash.Create(_opDollar.Create(x), _opDot.Create(), t as Term);
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
public static _With Create(Term P1, Where P2) { return new _With(P1, P2); }

  public IEnumerable<IRunnable> Run4_() {   
 { 
 var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opDollar; 
if (tmp_1 != null) { var y = tmp_1.P1; var tmp_2 = tmp_0.P2 as _Arrow; 
if (tmp_2 != null) { var tmp_3 = tmp_2.P1 as _opDollar; 
if (tmp_3 != null) { var x = tmp_3.P1; var u = tmp_2.P2; 
if(x.Equals(y)) { 
var result = u;
yield return result;  } } } }
 } 

  
 { 
 var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opDollar; 
if (tmp_1 != null) { var y = tmp_1.P1; var tmp_2 = tmp_0.P2 as _Arrow; 
if (tmp_2 != null) { var tmp_3 = tmp_2.P1 as _opDollar; 
if (tmp_3 != null) { var x = tmp_3.P1; var u = tmp_2.P2; 
if(!x.Equals(y)) { 
var result = _opDollar.Create(y);
yield return result;  } } } }
 } 

  
 { 
 var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opSlash; 
if (tmp_1 != null) { var tmp_2 = tmp_1.P1 as _opDollar; 
if (tmp_2 != null) { var x = tmp_2.P1; var tmp_3 = tmp_1.P2 as _opDot; 
if (tmp_3 != null) { var t = tmp_1.P3; var tmp_4 = tmp_0.P2 as _Arrow; 
if (tmp_4 != null) { var tmp_5 = tmp_4.P1 as _opDollar; 
if (tmp_5 != null) { var y = tmp_5.P1; var u = tmp_4.P2; 
if(x.Equals(y)) { 
if(t is Term) { 
var result = _opSlash.Create(_opDollar.Create(x), _opDot.Create(), t as Term);
yield return result;  } } } } } } }
 } 

  
 { 
 var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opSlash; 
if (tmp_1 != null) { var tmp_2 = tmp_1.P1 as _opDollar; 
if (tmp_2 != null) { var x = tmp_2.P1; var tmp_3 = tmp_1.P2 as _opDot; 
if (tmp_3 != null) { var t = tmp_1.P3; var tmp_4 = tmp_0.P2 as _Arrow; 
if (tmp_4 != null) { var tmp_5 = tmp_4.P1 as _opDollar; 
if (tmp_5 != null) { var y = tmp_5.P1; var u = tmp_4.P2; 
if(!x.Equals(y)) { 
if(t is Term && u is Term) { 
var tmp_7 = _With.Create(t as Term, _Arrow.Create(_opDollar.Create(y), u as Term));
foreach (var tmp_6 in tmp_7.Run4_()) { var t_Prime = tmp_6; 
if(t_Prime is Term) { 
var result = _opSlash.Create(_opDollar.Create(x), _opDot.Create(), t_Prime as Term);
yield return result;  } } } } } } } } }
 } 

  
 { 
 var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opVBar; 
if (tmp_1 != null) { var t = tmp_1.P1; var u = tmp_1.P2; var tmp_2 = tmp_0.P2 as _Arrow; 
if (tmp_2 != null) { var tmp_3 = tmp_2.P1 as _opDollar; 
if (tmp_3 != null) { var x = tmp_3.P1; var v = tmp_2.P2; 
if(t is Term && v is Term) { 
var tmp_5 = _With.Create(t as Term, _Arrow.Create(_opDollar.Create(x), v as Term));
foreach (var tmp_4 in tmp_5.Run4_()) { var t_Prime = tmp_4; 
if(u is Term && v is Term) { 
var tmp_7 = _With.Create(u as Term, _Arrow.Create(_opDollar.Create(x), v as Term));
foreach (var tmp_6 in tmp_7.Run4_()) { var u_Prime = tmp_6; 
if(t_Prime is Term && u_Prime is Term) { 
var result = _opVBar.Create(t_Prime as Term, u_Prime as Term);
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
public static _opVBar Create(Term P1, Term P2) { return new _opVBar(P1, P2); }

  public IEnumerable<IRunnable> Run() {   
 { 
 var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opDollar; 
if (tmp_1 != null) { var x = tmp_1.P1; var u = tmp_0.P2; 
if(u is Term) { 
var result = _opVBar.Create(_opDollar.Create(x), u as Term);
yield return result;  } }
 } 

  
 { 
 var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opVBar; 
if (tmp_1 != null) { var u = tmp_1.P1; var v = tmp_1.P2; var w = tmp_0.P2; 
if(u is Term && v is Term) { 
var tmp_3 = _opVBar.Create(u as Term, v as Term);
foreach (var tmp_2 in tmp_3.Run()) { var v_Prime = tmp_2; 
if(v_Prime is Term && w is Term) { 
var tmp_5 = _opVBar.Create(v_Prime as Term, w as Term);
foreach (var tmp_4 in tmp_5.Run()) { var res = tmp_4; 
var result = res;
yield return result;  } } } } }
 } 

  
 { 
 var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opSlash; 
if (tmp_1 != null) { var tmp_2 = tmp_1.P1 as _opDollar; 
if (tmp_2 != null) { var x = tmp_2.P1; var tmp_3 = tmp_1.P2 as _opDot; 
if (tmp_3 != null) { var t = tmp_1.P3; var u = tmp_0.P2; 
if (t is Term && u is Term) { var term = _With.Create(t as Term, _Arrow.Create(_opDollar.Create(x), u as Term)); 
var tmp_5 = term;
foreach (var tmp_4 in tmp_5.Run4_()) { var t_Prime = tmp_4; 
var result = t_Prime;
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
 #line 1 "input"
 var p = _opVBar.Create(_opVBar.Create(_opSlash.Create(_opDollar.Create("y"), _opDot.Create(), _opDollar.Create("y")), _opSlash.Create(_opDollar.Create("y"), _opDot.Create(), _opDollar.Create("y"))), _opVBar.Create(_opDollar.Create("x"), _opDollar.Create("z")));
if(printInput) System.Console.WriteLine(p.ToString());
foreach(var x in p.Run())
yield return x;
}
}

}
