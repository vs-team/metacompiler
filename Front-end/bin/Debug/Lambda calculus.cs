using System.Collections.Generic;
using System.Linq;
namespace Lambda_calculus {
 public static class Extensions { public static V GetKey<T, V>(this System.Collections.Immutable.ImmutableDictionary<T, V> self, T key) { return self[key]; } }
 public interface IRunnable { IEnumerable<IRunnable> Run();
IEnumerable<IRunnable> Run5_();
 }


public interface Dot : IRunnable {}
public interface Id : Term {}
public interface Term : IRunnable {}
public interface Where : IRunnable {}
public interface With : IRunnable {}



public class _opDollar : Id  {
public string P1;

public _opDollar(string P1) {this.P1 = P1;}
public static _opDollar Create(string P1) { return new _opDollar(P1); }

  public IEnumerable<IRunnable> Run() {   
 { 
 #line 28 "Content\Lambda calculus"
var tmp_0 = this; var x = tmp_0.P1; 
 #line 28 "Content\Lambda calculus"
var result = _opDollar.Create(x);
 #line 28 "Content\Lambda calculus"
yield return result; 
 } 

  }

public IEnumerable<IRunnable> Run5_() { foreach(var p in Run()) yield return p; }

public override string ToString() {
 var res = "("; 

 res += " $ "; if (P1 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P1.ToString(); } 

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

public class _Arrow : Dot  {

public _Arrow() {}
public static _Arrow Create() { return new _Arrow(); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run5_() { foreach(var p in Run()) yield return p; }

public override string ToString() {
return "->";
}

public override bool Equals(object other) {
 return other is _Arrow; 
}

public override int GetHashCode() {
 return 0; 
}

}

public class _opSlash : Term  {
public Id P1;
public Dot P2;
public Term P3;

public _opSlash(Id P1, Dot P2, Term P3) {this.P1 = P1; this.P2 = P2; this.P3 = P3;}
public static _opSlash Create(Id P1, Dot P2, Term P3) { return new _opSlash(P1, P2, P3); }

  public IEnumerable<IRunnable> Run() {   
 { 
 #line 33 "Content\Lambda calculus"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opDollar; 
 #line 33 "Content\Lambda calculus"
if (tmp_1 != null) { var x = tmp_1.P1; var tmp_2 = tmp_0.P2 as _Arrow; 
 #line 33 "Content\Lambda calculus"
if (tmp_2 != null) { var t = tmp_0.P3; 
 #line 33 "Content\Lambda calculus"
if(t is Term) { 
 #line 33 "Content\Lambda calculus"
var result = _opSlash.Create(_opDollar.Create(x), _Arrow.Create(), t as Term);
 #line 33 "Content\Lambda calculus"
yield return result;  } } }
 } 

  }

public IEnumerable<IRunnable> Run5_() { foreach(var p in Run()) yield return p; }

public override string ToString() {
 var res = "("; 

 res += " \\ "; res += P1.ToString(); 
res += P2.ToString(); 
res += P3.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _opSlash;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2) && this.P3.Equals(tmp.P3); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class _As : Where  {
public Term P1;
public Term P2;

public _As(Term P1, Term P2) {this.P1 = P1; this.P2 = P2;}
public static _As Create(Term P1, Term P2) { return new _As(P1, P2); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run5_() { foreach(var p in Run()) yield return p; }

public override string ToString() {
 var res = "("; 
res += P1.ToString(); 

 res += " as "; res += P2.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _As;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class run : Term  {

public run() {}
public static run Create() { return new run(); }

  public IEnumerable<IRunnable> Run() {   
 { 
 #line 21 "Content\Lambda calculus"
var tmp_0 = this as run; 
 #line 21 "Content\Lambda calculus"
var tmp_2 = _opBitwiseOr.Create(_opSlash.Create(_opDollar.Create("x"), _Arrow.Create(), _opDollar.Create("x")), _opDollar.Create("y"));
 #line 21 "Content\Lambda calculus"
foreach (var tmp_1 in tmp_2.Run()) { var res = tmp_1; 
 #line 21 "Content\Lambda calculus"
var result = res;
 #line 21 "Content\Lambda calculus"
yield return result;  }
 } 

  }

public IEnumerable<IRunnable> Run5_() { foreach(var p in Run()) yield return p; }

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

public class _With : With  {
public Term P1;
public Where P2;

public _With(Term P1, Where P2) {this.P1 = P1; this.P2 = P2;}
public static _With Create(Term P1, Where P2) { return new _With(P1, P2); }

  public IEnumerable<IRunnable> Run5_() {   
 { 
 #line 59 "Content\Lambda calculus"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opDollar; 
 #line 59 "Content\Lambda calculus"
if (tmp_1 != null) { var y = tmp_1.P1; var tmp_2 = tmp_0.P2 as _As; 
 #line 59 "Content\Lambda calculus"
if (tmp_2 != null) { var tmp_3 = tmp_2.P1 as _opDollar; 
 #line 59 "Content\Lambda calculus"
if (tmp_3 != null) { var x = tmp_3.P1; var u = tmp_2.P2; 
 #line 59 "Content\Lambda calculus"
if(x.Equals(y)) { 
 #line 59 "Content\Lambda calculus"
var result = u;
 #line 59 "Content\Lambda calculus"
yield return result;  } } } }
 } 

  
 { 
 #line 66 "Content\Lambda calculus"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opDollar; 
 #line 66 "Content\Lambda calculus"
if (tmp_1 != null) { var y = tmp_1.P1; var tmp_2 = tmp_0.P2 as _As; 
 #line 66 "Content\Lambda calculus"
if (tmp_2 != null) { var tmp_3 = tmp_2.P1 as _opDollar; 
 #line 66 "Content\Lambda calculus"
if (tmp_3 != null) { var x = tmp_3.P1; var u = tmp_2.P2; 
 #line 66 "Content\Lambda calculus"
if(!x.Equals(y)) { 
 #line 66 "Content\Lambda calculus"
var result = _opDollar.Create(y);
 #line 66 "Content\Lambda calculus"
yield return result;  } } } }
 } 

  
 { 
 #line 73 "Content\Lambda calculus"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opSlash; 
 #line 73 "Content\Lambda calculus"
if (tmp_1 != null) { var tmp_2 = tmp_1.P1 as _opDollar; 
 #line 73 "Content\Lambda calculus"
if (tmp_2 != null) { var x = tmp_2.P1; var tmp_3 = tmp_1.P2 as _Arrow; 
 #line 73 "Content\Lambda calculus"
if (tmp_3 != null) { var t = tmp_1.P3; var tmp_4 = tmp_0.P2 as _As; 
 #line 73 "Content\Lambda calculus"
if (tmp_4 != null) { var tmp_5 = tmp_4.P1 as _opDollar; 
 #line 73 "Content\Lambda calculus"
if (tmp_5 != null) { var y = tmp_5.P1; var u = tmp_4.P2; 
 #line 73 "Content\Lambda calculus"
if(x.Equals(y)) { 
 #line 73 "Content\Lambda calculus"
if(t is Term) { 
 #line 73 "Content\Lambda calculus"
var result = _opSlash.Create(_opDollar.Create(x), _Arrow.Create(), t as Term);
 #line 73 "Content\Lambda calculus"
yield return result;  } } } } } } }
 } 

  
 { 
 #line 80 "Content\Lambda calculus"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opSlash; 
 #line 80 "Content\Lambda calculus"
if (tmp_1 != null) { var tmp_2 = tmp_1.P1 as _opDollar; 
 #line 80 "Content\Lambda calculus"
if (tmp_2 != null) { var x = tmp_2.P1; var tmp_3 = tmp_1.P2 as _Arrow; 
 #line 80 "Content\Lambda calculus"
if (tmp_3 != null) { var t = tmp_1.P3; var tmp_4 = tmp_0.P2 as _As; 
 #line 80 "Content\Lambda calculus"
if (tmp_4 != null) { var tmp_5 = tmp_4.P1 as _opDollar; 
 #line 80 "Content\Lambda calculus"
if (tmp_5 != null) { var y = tmp_5.P1; var u = tmp_4.P2; 
 #line 80 "Content\Lambda calculus"
if(!x.Equals(y)) { 
 #line 80 "Content\Lambda calculus"
if(t is Term && u is Term) { 
 #line 80 "Content\Lambda calculus"
var tmp_7 = _With.Create(t as Term, _As.Create(_opDollar.Create(y), u as Term));
 #line 80 "Content\Lambda calculus"
foreach (var tmp_6 in tmp_7.Run5_()) { var t_Prime = tmp_6; 
 #line 80 "Content\Lambda calculus"
if(t_Prime is Term) { 
 #line 80 "Content\Lambda calculus"
var result = _opSlash.Create(_opDollar.Create(x), _Arrow.Create(), t_Prime as Term);
 #line 80 "Content\Lambda calculus"
yield return result;  } } } } } } } } }
 } 

  
 { 
 #line 89 "Content\Lambda calculus"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opBitwiseOr; 
 #line 89 "Content\Lambda calculus"
if (tmp_1 != null) { var t = tmp_1.P1; var u = tmp_1.P2; var tmp_2 = tmp_0.P2 as _As; 
 #line 89 "Content\Lambda calculus"
if (tmp_2 != null) { var tmp_3 = tmp_2.P1 as _opDollar; 
 #line 89 "Content\Lambda calculus"
if (tmp_3 != null) { var x = tmp_3.P1; var v = tmp_2.P2; 
 #line 89 "Content\Lambda calculus"
if(t is Term && v is Term) { 
 #line 89 "Content\Lambda calculus"
var tmp_5 = _With.Create(t as Term, _As.Create(_opDollar.Create(x), v as Term));
 #line 89 "Content\Lambda calculus"
foreach (var tmp_4 in tmp_5.Run5_()) { var t_Prime = tmp_4; 
 #line 89 "Content\Lambda calculus"
if(u is Term && v is Term) { 
 #line 89 "Content\Lambda calculus"
var tmp_7 = _With.Create(u as Term, _As.Create(_opDollar.Create(x), v as Term));
 #line 89 "Content\Lambda calculus"
foreach (var tmp_6 in tmp_7.Run5_()) { var u_Prime = tmp_6; 
 #line 89 "Content\Lambda calculus"
if(t_Prime is Term && u_Prime is Term) { 
 #line 89 "Content\Lambda calculus"
var result = _opBitwiseOr.Create(t_Prime as Term, u_Prime as Term);
 #line 89 "Content\Lambda calculus"
yield return result;  } } } } } } } }
 } 

 foreach(var p in Run()) yield return p; }

public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }

public override string ToString() {
 var res = "("; 
res += P1.ToString(); 

 res += " with "; res += P2.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _With;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class _opBitwiseOr : Term  {
public Term P1;
public Term P2;

public _opBitwiseOr(Term P1, Term P2) {this.P1 = P1; this.P2 = P2;}
public static _opBitwiseOr Create(Term P1, Term P2) { return new _opBitwiseOr(P1, P2); }

  public IEnumerable<IRunnable> Run() {   
 { 
 #line 38 "Content\Lambda calculus"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opDollar; 
 #line 38 "Content\Lambda calculus"
if (tmp_1 != null) { var x = tmp_1.P1; var u = tmp_0.P2; 
 #line 38 "Content\Lambda calculus"
if(u is Term) { 
 #line 38 "Content\Lambda calculus"
var result = _opBitwiseOr.Create(_opDollar.Create(x), u as Term);
 #line 38 "Content\Lambda calculus"
yield return result;  } }
 } 

  
 { 
 #line 43 "Content\Lambda calculus"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opBitwiseOr; 
 #line 43 "Content\Lambda calculus"
if (tmp_1 != null) { var u = tmp_1.P1; var v = tmp_1.P2; var w = tmp_0.P2; 
 #line 43 "Content\Lambda calculus"
if(u is Term && v is Term) { 
 #line 43 "Content\Lambda calculus"
var tmp_3 = _opBitwiseOr.Create(u as Term, v as Term);
 #line 43 "Content\Lambda calculus"
foreach (var tmp_2 in tmp_3.Run()) { var v_Prime = tmp_2; 
 #line 43 "Content\Lambda calculus"
if(v_Prime is Term && w is Term) { 
 #line 43 "Content\Lambda calculus"
var tmp_5 = _opBitwiseOr.Create(v_Prime as Term, w as Term);
 #line 43 "Content\Lambda calculus"
foreach (var tmp_4 in tmp_5.Run()) { var res = tmp_4; 
 #line 43 "Content\Lambda calculus"
var result = res;
 #line 43 "Content\Lambda calculus"
yield return result;  } } } } }
 } 

  
 { 
 #line 52 "Content\Lambda calculus"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _opSlash; 
 #line 52 "Content\Lambda calculus"
if (tmp_1 != null) { var tmp_2 = tmp_1.P1 as _opDollar; 
 #line 52 "Content\Lambda calculus"
if (tmp_2 != null) { var x = tmp_2.P1; var tmp_3 = tmp_1.P2 as _Arrow; 
 #line 52 "Content\Lambda calculus"
if (tmp_3 != null) { var t = tmp_1.P3; var u = tmp_0.P2; 
 #line 52 "Content\Lambda calculus"
if(t is Term && u is Term) { 
 #line 52 "Content\Lambda calculus"
var tmp_5 = _With.Create(t as Term, _As.Create(_opDollar.Create(x), u as Term));
 #line 52 "Content\Lambda calculus"
foreach (var tmp_4 in tmp_5.Run5_()) { var t_Prime = tmp_4; 
 #line 52 "Content\Lambda calculus"
var result = t_Prime;
 #line 52 "Content\Lambda calculus"
yield return result;  } } } } }
 } 

  }

public IEnumerable<IRunnable> Run5_() { foreach(var p in Run()) yield return p; }

public override string ToString() {
 var res = "("; 
res += P1.ToString(); 

 res += " | "; res += P2.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _opBitwiseOr;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
 else return false; }

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
