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

  public static IEnumerable<IRunnable> StaticRun(string P1) {   
 { 
 var x = P1; 
var result = _opDollar.Create(x);
yield return result; 
 } 

  }
public IEnumerable<IRunnable> Run() { return StaticRun(P1); }

public static IEnumerable<IRunnable> StaticRun5_(string P1) { return StaticRun(P1); }
public IEnumerable<IRunnable> Run5_(){ return StaticRun5_(P1); }

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


public static IEnumerable<IRunnable> StaticRun() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run(){ return StaticRun(); }
public static IEnumerable<IRunnable> StaticRun5_() { return StaticRun(); }
public IEnumerable<IRunnable> Run5_(){ return StaticRun5_(); }

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

  public static IEnumerable<IRunnable> StaticRun(Id P1, Dot P2, Term P3) {   
 { 
 var tmp_0 = P1 as _opDollar; 
if (tmp_0 != null) { var x = tmp_0.P1; var tmp_1 = P2 as _Arrow; 
if (tmp_1 != null) { var t = P3; 
if(t is Term) { 
var result = _opSlash.Create(_opDollar.Create(x), _Arrow.Create(), t as Term);
yield return result;  } } }
 } 

  }
public IEnumerable<IRunnable> Run() { return StaticRun(P1, P2, P3); }

public static IEnumerable<IRunnable> StaticRun5_(Id P1, Dot P2, Term P3) { return StaticRun(P1, P2, P3); }
public IEnumerable<IRunnable> Run5_(){ return StaticRun5_(P1, P2, P3); }

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


public static IEnumerable<IRunnable> StaticRun(Term P1, Term P2) { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run(){ return StaticRun(P1, P2); }
public static IEnumerable<IRunnable> StaticRun5_(Term P1, Term P2) { return StaticRun(P1, P2); }
public IEnumerable<IRunnable> Run5_(){ return StaticRun5_(P1, P2); }

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

  public static IEnumerable<IRunnable> StaticRun() {   
 { 
 
var tmp_1 = _opBitwiseOr.Create(_opSlash.Create(_opDollar.Create("x"), _Arrow.Create(), _opDollar.Create("x")), _opDollar.Create("y"));
foreach (var tmp_0 in tmp_1.Run()) { var res = tmp_0; 
var result = res;
yield return result;  }
 } 

  }
public IEnumerable<IRunnable> Run() { return StaticRun(); }

public static IEnumerable<IRunnable> StaticRun5_() { return StaticRun(); }
public IEnumerable<IRunnable> Run5_(){ return StaticRun5_(); }

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

  public static IEnumerable<IRunnable> StaticRun5_(Term P1, Where P2) {   
 { 
 var tmp_0 = P1 as _opDollar; 
if (tmp_0 != null) { var y = tmp_0.P1; var tmp_1 = P2 as _As; 
if (tmp_1 != null) { var tmp_2 = tmp_1.P1 as _opDollar; 
if (tmp_2 != null) { var x = tmp_2.P1; var u = tmp_1.P2; 
if(x.Equals(y)) { 
var result = u;
yield return result;  } } } }
 } 

  
 { 
 var tmp_0 = P1 as _opDollar; 
if (tmp_0 != null) { var y = tmp_0.P1; var tmp_1 = P2 as _As; 
if (tmp_1 != null) { var tmp_2 = tmp_1.P1 as _opDollar; 
if (tmp_2 != null) { var x = tmp_2.P1; var u = tmp_1.P2; 
if(!x.Equals(y)) { 
var result = _opDollar.Create(y);
yield return result;  } } } }
 } 

  
 { 
 var tmp_0 = P1 as _opSlash; 
if (tmp_0 != null) { var tmp_1 = tmp_0.P1 as _opDollar; 
if (tmp_1 != null) { var x = tmp_1.P1; var tmp_2 = tmp_0.P2 as _Arrow; 
if (tmp_2 != null) { var t = tmp_0.P3; var tmp_3 = P2 as _As; 
if (tmp_3 != null) { var tmp_4 = tmp_3.P1 as _opDollar; 
if (tmp_4 != null) { var y = tmp_4.P1; var u = tmp_3.P2; 
if(x.Equals(y)) { 
if(t is Term) { 
var result = _opSlash.Create(_opDollar.Create(x), _Arrow.Create(), t as Term);
yield return result;  } } } } } } }
 } 

  
 { 
 var tmp_0 = P1 as _opSlash; 
if (tmp_0 != null) { var tmp_1 = tmp_0.P1 as _opDollar; 
if (tmp_1 != null) { var x = tmp_1.P1; var tmp_2 = tmp_0.P2 as _Arrow; 
if (tmp_2 != null) { var t = tmp_0.P3; var tmp_3 = P2 as _As; 
if (tmp_3 != null) { var tmp_4 = tmp_3.P1 as _opDollar; 
if (tmp_4 != null) { var y = tmp_4.P1; var u = tmp_3.P2; 
if(!x.Equals(y)) { 
if(t is Term && u is Term) { 
var tmp_6 = _With.Create(t as Term, _As.Create(_opDollar.Create(y), u as Term));
foreach (var tmp_5 in tmp_6.Run5_()) { var t_Prime = tmp_5; 
if(t_Prime is Term) { 
var result = _opSlash.Create(_opDollar.Create(x), _Arrow.Create(), t_Prime as Term);
yield return result;  } } } } } } } } }
 } 

  
 { 
 var tmp_0 = P1 as _opBitwiseOr; 
if (tmp_0 != null) { var t = tmp_0.P1; var u = tmp_0.P2; var tmp_1 = P2 as _As; 
if (tmp_1 != null) { var tmp_2 = tmp_1.P1 as _opDollar; 
if (tmp_2 != null) { var x = tmp_2.P1; var v = tmp_1.P2; 
if(t is Term && v is Term) { 
var tmp_4 = _With.Create(t as Term, _As.Create(_opDollar.Create(x), v as Term));
foreach (var tmp_3 in tmp_4.Run5_()) { var t_Prime = tmp_3; 
if(u is Term && v is Term) { 
var tmp_6 = _With.Create(u as Term, _As.Create(_opDollar.Create(x), v as Term));
foreach (var tmp_5 in tmp_6.Run5_()) { var u_Prime = tmp_5; 
if(t_Prime is Term && u_Prime is Term) { 
var result = _opBitwiseOr.Create(t_Prime as Term, u_Prime as Term);
yield return result;  } } } } } } } }
 } 

 foreach(var p in StaticRun(P1, P2)) yield return p; }
public IEnumerable<IRunnable> Run5_() { return StaticRun5_(P1, P2); }

public static IEnumerable<IRunnable> StaticRun(Term P1, Where P2) { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run(){ return StaticRun(P1, P2); }

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

  public static IEnumerable<IRunnable> StaticRun(Term P1, Term P2) {   
 { 
 var tmp_0 = P1 as _opDollar; 
if (tmp_0 != null) { var x = tmp_0.P1; var u = P2; 
if(u is Term) { 
var result = _opBitwiseOr.Create(_opDollar.Create(x), u as Term);
yield return result;  } }
 } 

  
 { 
 var tmp_0 = P1 as _opBitwiseOr; 
if (tmp_0 != null) { var u = tmp_0.P1; var v = tmp_0.P2; var w = P2; 
if(u is Term && v is Term) { 
var tmp_2 = _opBitwiseOr.Create(u as Term, v as Term);
foreach (var tmp_1 in tmp_2.Run()) { var v_Prime = tmp_1; 
if(v_Prime is Term && w is Term) { 
var tmp_4 = _opBitwiseOr.Create(v_Prime as Term, w as Term);
foreach (var tmp_3 in tmp_4.Run()) { var res = tmp_3; 
var result = res;
yield return result;  } } } } }
 } 

  
 { 
 var tmp_0 = P1 as _opSlash; 
if (tmp_0 != null) { var tmp_1 = tmp_0.P1 as _opDollar; 
if (tmp_1 != null) { var x = tmp_1.P1; var tmp_2 = tmp_0.P2 as _Arrow; 
if (tmp_2 != null) { var t = tmp_0.P3; var u = P2; 
if(t is Term && u is Term) { 
var tmp_4 = _With.Create(t as Term, _As.Create(_opDollar.Create(x), u as Term));
foreach (var tmp_3 in tmp_4.Run5_()) { var t_Prime = tmp_3; 
var result = t_Prime;
yield return result;  } } } } }
 } 

  }
public IEnumerable<IRunnable> Run() { return StaticRun(P1, P2); }

public static IEnumerable<IRunnable> StaticRun5_(Term P1, Term P2) { return StaticRun(P1, P2); }
public IEnumerable<IRunnable> Run5_(){ return StaticRun5_(P1, P2); }

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
