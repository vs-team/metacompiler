using System.Collections.Immutable;
using System.Collections.Generic;
using System.Linq;
namespace Tuples.mc {
 public static class Extensions { public static V GetKey<T, V>(this System.Collections.Immutable.ImmutableDictionary<T, V> self, T key) { return self[key]; } }



public interface Tuple<a, b> {}
public interface TupleOperator {}



public class _Comma<a, b> : Tuple<a, b>  {
public a P1;
public b P2;

public _Comma(a P1, b P2) {this.P1 = P1; this.P2 = P2;}
public static _Comma<a, b> Create(a P1, b P2) { return new _Comma<a, b>(P1, P2); }

public override string ToString() {
 var res = "("; 
if (P1 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P1.ToString(); } 

 res += " , "; if (P2 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P2 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P2.ToString(); } 

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

public class fst<a, b> : TupleOperator  {
public Tuple<a, b> P1;

public fst(Tuple<a, b> P1) {this.P1 = P1;}
public static fst<a, b> Create(Tuple<a, b> P1) { return new fst<a, b>(P1); }

  public static a StaticRun(Tuple<a, b> P1) {    
 { 
 var tmp_0 = P1 as _Comma<a, b>; 
if (tmp_0 != null) { 
 var x = tmp_0.P1; var y = tmp_0.P2; 
var result = x;
 return result;  }
 } 

  
throw new System.Exception("Error evaluating: fst. No result returned."); }
public a Run() { return StaticRun(P1); }


public override string ToString() {
 var res = "("; 

 res += " fst "; if (P1 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P1.ToString(); } 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as fst<a, b>;
 if(tmp != null) return this.P1.Equals(tmp.P1); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class snd<a, b> : TupleOperator  {
public Tuple<a, b> P1;

public snd(Tuple<a, b> P1) {this.P1 = P1;}
public static snd<a, b> Create(Tuple<a, b> P1) { return new snd<a, b>(P1); }

  public static b StaticRun(Tuple<a, b> P1) {    
 { 
 var tmp_0 = P1 as _Comma<a, b>; 
if (tmp_0 != null) { 
 var x = tmp_0.P1; var y = tmp_0.P2; 
var result = y;
 return result;  }
 } 

  
throw new System.Exception("Error evaluating: snd. No result returned."); }
public b Run() { return StaticRun(P1); }


public override string ToString() {
 var res = "("; 

 res += " snd "; if (P1 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P1.ToString(); } 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as snd<a, b>;
 if(tmp != null) return this.P1.Equals(tmp.P1); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}




public class EntryPoint {
 public static bool Print(string s) {System.Console.WriteLine(s); return true;}
   
static public object Run(bool printInput)
{
 #line 1 "input"
 var p = fst<float, float>.Create(_Comma<float, float>.Create(1.000000f, 2.000000f));
if(printInput) System.Console.WriteLine(p.ToString());
 
 var result = p.Run(); 

return result;
}
}

}
