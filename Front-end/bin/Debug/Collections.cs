using System.Collections.Immutable;
using System.Collections.Generic;
using System.Linq;
namespace Collections {
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
res += P1.ToString(); 

 res += " , "; res += P2.ToString(); 

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

public class first<a, b> : TupleOperator  {
public Tuple<a, b> P1;

public first(Tuple<a, b> P1) {this.P1 = P1;}
public static first<a, b> Create(Tuple<a, b> P1) { return new first<a, b>(P1); }

  public static a StaticRun(Tuple<a, b> P1) {    
 { 
 #line 7 "Content\Collections\transform.mc"
var tmp_0 = P1 as _Comma<a, b>; 
 #line 7 "Content\Collections\transform.mc"
if (tmp_0 != null) { 
 var x = tmp_0.P1; var y = tmp_0.P2; 
 #line 7 "Content\Collections\transform.mc"
var result = x;
 #line 7 "Content\Collections\transform.mc"
 return result;  }
 } 

  
throw new System.Exception("Error evaluating: first. No result returned."); }
public a Run() { return StaticRun(P1); }


public override string ToString() {
 var res = "("; 

 res += " first "; if (P1 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P1.ToString(); } 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as first<a, b>;
 if(tmp != null) return this.P1.Equals(tmp.P1); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}




public class EntryPoint {
static public object Run(bool printInput)
{
 #line 1 "input"
 var p = first<int, bool>.Create(_Comma<int, bool>.Create(5, true));
if(printInput) System.Console.WriteLine(p.ToString());
 
 var result = p.Run(); 

return result;
}
}

}
