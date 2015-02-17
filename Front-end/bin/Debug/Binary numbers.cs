using System.Collections.Generic;
using System.Linq;
namespace Binary_numbers {
 public static class Extensions { public static V GetKey<T, V>(this System.Collections.Immutable.ImmutableDictionary<T, V> self, T key) { return self[key]; } }
 public interface IRunnable { IEnumerable<IRunnable> Run();
IEnumerable<IRunnable> Run0_();
IEnumerable<IRunnable> Run0_2_();
 }


public interface Digit : IRunnable {}
public interface Expr : IRunnable {}
public interface Num : Expr {}



public class _opAddition : Expr {
public Num P1;
public Num P2;

public _opAddition(Num P1, Num P2) {this.P1 = P1; this.P2 = P2;}
public static _opAddition Create(Num P1, Num P2) { return new _opAddition(P1, P2); }

  public IEnumerable<IRunnable> Run() {   
 { 
 var tmp_0 = this; var a = tmp_0.P1; var b = tmp_0.P2; 
if(a is Num && b is Num) { 
var tmp_2 = addCarry.Create(a as Num, b as Num, d0.Create());
foreach (var tmp_1 in tmp_2.Run0_()) { var c = tmp_1; 
var result = c;
yield return result;  } }
 } 

  }

public IEnumerable<IRunnable> Run0_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run0_2_() { foreach(var p in Run0_()) yield return p; }

public override string ToString() {
 var res = "("; 
res += P1.ToString(); 

 res += "+"; res += P2.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _opAddition;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
 else return false; }
}

public class _Comma : Num {
public Num P1;
public Digit P2;

public _Comma(Num P1, Digit P2) {this.P1 = P1; this.P2 = P2;}
public static _Comma Create(Num P1, Digit P2) { return new _Comma(P1, P2); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run0_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run0_2_() { foreach(var p in Run0_()) yield return p; }

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
}

public class addCarry : Expr {
public Num P1;
public Num P2;
public Digit P3;

public addCarry(Num P1, Num P2, Digit P3) {this.P1 = P1; this.P2 = P2; this.P3 = P3;}
public static addCarry Create(Num P1, Num P2, Digit P3) { return new addCarry(P1, P2, P3); }

  public IEnumerable<IRunnable> Run0_() {   
 { 
 var tmp_0 = this; var tmp_1 = tmp_0.P1 as nil; 
if (tmp_1 != null) { var tmp_2 = tmp_0.P2 as nil; 
if (tmp_2 != null) { var tmp_3 = tmp_0.P3 as d1; 
if (tmp_3 != null) { 
var result = overflow.Create();
yield return result;  } } }
 } 

  
 { 
 var tmp_0 = this; var tmp_1 = tmp_0.P1 as nil; 
if (tmp_1 != null) { var tmp_2 = tmp_0.P2 as nil; 
if (tmp_2 != null) { var tmp_3 = tmp_0.P3 as d0; 
if (tmp_3 != null) { 
var result = nil.Create();
yield return result;  } } }
 } 

  
 { 
 var tmp_0 = this; var tmp_1 = tmp_0.P1 as _Comma; 
if (tmp_1 != null) { var a = tmp_1.P1; var da = tmp_1.P2; var tmp_2 = tmp_0.P2 as _Comma; 
if (tmp_2 != null) { var b = tmp_2.P1; var db = tmp_2.P2; var dr = tmp_0.P3; 
if(da is Digit && db is Digit && dr is Digit) { 
var tmp_4 = addDigits.Create(da as Digit, db as Digit, dr as Digit);
foreach (var tmp_3 in tmp_4.Run0_2_()) { var tmp_5 = tmp_3 as _Comma; 
if (tmp_5 != null) { var tmp_6 = tmp_5.P1 as _Comma; 
if (tmp_6 != null) { var tmp_7 = tmp_6.P1 as nil; 
if (tmp_7 != null) { var dr_Prime = tmp_6.P2; var d = tmp_5.P2; 
if(a is Num && b is Num && dr_Prime is Digit) { 
var tmp_9 = addCarry.Create(a as Num, b as Num, dr_Prime as Digit);
foreach (var tmp_8 in tmp_9.Run0_2_()) { var res = tmp_8; 
if(res is Num && d is Digit) { 
var result = _Comma.Create(res as Num, d as Digit);
yield return result;  } } } } } } } } } }
 } 

 foreach(var p in Run()) yield return p; }

public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run0_2_() { foreach(var p in Run0_()) yield return p; }

public override string ToString() {
 var res = "("; 

 res += "addCarry"; res += P1.ToString(); 
res += P2.ToString(); 
res += P3.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as addCarry;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2) && this.P3.Equals(tmp.P3); 
 else return false; }
}

public class addDigits : Expr {
public Digit P1;
public Digit P2;
public Digit P3;

public addDigits(Digit P1, Digit P2, Digit P3) {this.P1 = P1; this.P2 = P2; this.P3 = P3;}
public static addDigits Create(Digit P1, Digit P2, Digit P3) { return new addDigits(P1, P2, P3); }

  public IEnumerable<IRunnable> Run0_2_() {   
 { 
 var tmp_0 = this; var tmp_1 = tmp_0.P1 as d0; 
if (tmp_1 != null) { var tmp_2 = tmp_0.P2 as d0; 
if (tmp_2 != null) { var tmp_3 = tmp_0.P3 as d0; 
if (tmp_3 != null) { 
var result = _Comma.Create(_Comma.Create(nil.Create(), d0.Create()), d0.Create());
yield return result;  } } }
 } 

  
 { 
 var tmp_0 = this; var tmp_1 = tmp_0.P1 as d0; 
if (tmp_1 != null) { var tmp_2 = tmp_0.P2 as d0; 
if (tmp_2 != null) { var tmp_3 = tmp_0.P3 as d1; 
if (tmp_3 != null) { 
var result = _Comma.Create(_Comma.Create(nil.Create(), d0.Create()), d1.Create());
yield return result;  } } }
 } 

  
 { 
 var tmp_0 = this; var tmp_1 = tmp_0.P1 as d0; 
if (tmp_1 != null) { var tmp_2 = tmp_0.P2 as d1; 
if (tmp_2 != null) { var tmp_3 = tmp_0.P3 as d0; 
if (tmp_3 != null) { 
var result = _Comma.Create(_Comma.Create(nil.Create(), d0.Create()), d1.Create());
yield return result;  } } }
 } 

  
 { 
 var tmp_0 = this; var tmp_1 = tmp_0.P1 as d0; 
if (tmp_1 != null) { var tmp_2 = tmp_0.P2 as d1; 
if (tmp_2 != null) { var tmp_3 = tmp_0.P3 as d1; 
if (tmp_3 != null) { 
var result = _Comma.Create(_Comma.Create(nil.Create(), d1.Create()), d0.Create());
yield return result;  } } }
 } 

  
 { 
 var tmp_0 = this; var tmp_1 = tmp_0.P1 as d1; 
if (tmp_1 != null) { var tmp_2 = tmp_0.P2 as d0; 
if (tmp_2 != null) { var tmp_3 = tmp_0.P3 as d0; 
if (tmp_3 != null) { 
var result = _Comma.Create(_Comma.Create(nil.Create(), d0.Create()), d1.Create());
yield return result;  } } }
 } 

  
 { 
 var tmp_0 = this; var tmp_1 = tmp_0.P1 as d1; 
if (tmp_1 != null) { var tmp_2 = tmp_0.P2 as d0; 
if (tmp_2 != null) { var tmp_3 = tmp_0.P3 as d1; 
if (tmp_3 != null) { 
var result = _Comma.Create(_Comma.Create(nil.Create(), d1.Create()), d0.Create());
yield return result;  } } }
 } 

  
 { 
 var tmp_0 = this; var tmp_1 = tmp_0.P1 as d1; 
if (tmp_1 != null) { var tmp_2 = tmp_0.P2 as d1; 
if (tmp_2 != null) { var tmp_3 = tmp_0.P3 as d0; 
if (tmp_3 != null) { 
var result = _Comma.Create(_Comma.Create(nil.Create(), d1.Create()), d0.Create());
yield return result;  } } }
 } 

  
 { 
 var tmp_0 = this; var tmp_1 = tmp_0.P1 as d1; 
if (tmp_1 != null) { var tmp_2 = tmp_0.P2 as d1; 
if (tmp_2 != null) { var tmp_3 = tmp_0.P3 as d1; 
if (tmp_3 != null) { 
var result = _Comma.Create(_Comma.Create(nil.Create(), d1.Create()), d1.Create());
yield return result;  } } }
 } 

 foreach(var p in Run0_()) yield return p; }

public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run0_() { foreach(var p in Run()) yield return p; }

public override string ToString() {
 var res = "("; 

 res += "addDigits"; res += P1.ToString(); 
res += P2.ToString(); 
res += P3.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as addDigits;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2) && this.P3.Equals(tmp.P3); 
 else return false; }
}

public class d0 : Digit {

public d0() {}
public static d0 Create() { return new d0(); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run0_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run0_2_() { foreach(var p in Run0_()) yield return p; }

public override string ToString() {
return "d0";
}

public override bool Equals(object other) {
 return other is d0; 
}
}

public class d1 : Digit {

public d1() {}
public static d1 Create() { return new d1(); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run0_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run0_2_() { foreach(var p in Run0_()) yield return p; }

public override string ToString() {
return "d1";
}

public override bool Equals(object other) {
 return other is d1; 
}
}

public class nil : Num {

public nil() {}
public static nil Create() { return new nil(); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run0_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run0_2_() { foreach(var p in Run0_()) yield return p; }

public override string ToString() {
return "nil";
}

public override bool Equals(object other) {
 return other is nil; 
}
}

public class overflow : Num {

public overflow() {}
public static overflow Create() { return new overflow(); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run0_() { foreach(var p in Run()) yield return p; }
public IEnumerable<IRunnable> Run0_2_() { foreach(var p in Run0_()) yield return p; }

public override string ToString() {
return "overflow";
}

public override bool Equals(object other) {
 return other is overflow; 
}
}




public class EntryPoint {
 static public IEnumerable<IRunnable> Run(bool printInput)
{
 #line 1 "input"
 var p = _opAddition.Create(_Comma.Create(_Comma.Create(_Comma.Create(_Comma.Create(nil.Create(), d0.Create()), d1.Create()), d1.Create()), d1.Create()), _Comma.Create(_Comma.Create(_Comma.Create(_Comma.Create(nil.Create(), d0.Create()), d0.Create()), d0.Create()), d1.Create()));
if(printInput) System.Console.WriteLine(p.ToString());
foreach(var x in p.Run())
yield return x;
}
}

}
