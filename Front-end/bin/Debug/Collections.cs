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
 #line 13 "Content\Collections\transform.mc"
var tmp_0 = P1 as _Comma<a, b>; 
 #line 13 "Content\Collections\transform.mc"
if (tmp_0 != null) { 
 var x = tmp_0.P1; var y = tmp_0.P2; 
 #line 13 "Content\Collections\transform.mc"
var result = x;
 #line 13 "Content\Collections\transform.mc"
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

public class run : TupleOperator  {
public Tuple<ImmutableDictionary<string, int>, ImmutableDictionary<string, int>> P1;

public run(Tuple<ImmutableDictionary<string, int>, ImmutableDictionary<string, int>> P1) {this.P1 = P1;}
public static run Create(Tuple<ImmutableDictionary<string, int>, ImmutableDictionary<string, int>> P1) { return new run(P1); }

  public static ImmutableDictionary<string, int> StaticRun(Tuple<ImmutableDictionary<string, int>, ImmutableDictionary<string, int>> P1) {    
 { 
 #line 19 "Content\Collections\transform.mc"
var t = P1; 
 #line 19 "Content\Collections\transform.mc"
var tmp_1 = fst<ImmutableDictionary<string,int>, ImmutableDictionary<string,int>>.StaticRun(t);
 #line 19 "Content\Collections\transform.mc"

var tmp_0 = tmp_1;

var x = tmp_0; 
 #line 19 "Content\Collections\transform.mc"
var tmp_3 = snd<ImmutableDictionary<string,int>, ImmutableDictionary<string,int>>.StaticRun(t);
 #line 19 "Content\Collections\transform.mc"

var tmp_2 = tmp_3;

var y = tmp_2; 
 #line 19 "Content\Collections\transform.mc"
var result = y;
 #line 19 "Content\Collections\transform.mc"
 return result; 
 } 

  
throw new System.Exception("Error evaluating: run. No result returned."); }
public ImmutableDictionary<string, int> Run() { return StaticRun(P1); }


public override string ToString() {
 var res = "("; 

 res += " run "; if (P1 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P1.ToString(); } 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as run;
 if(tmp != null) return this.P1.Equals(tmp.P1); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class run1 : TupleOperator  {
public Tuple<int, int> P1;

public run1(Tuple<int, int> P1) {this.P1 = P1;}
public static run1 Create(Tuple<int, int> P1) { return new run1(P1); }

  public static int StaticRun(Tuple<int, int> P1) {    
 { 
 #line 24 "Content\Collections\transform.mc"
var t = P1; 
 #line 24 "Content\Collections\transform.mc"
var tmp_1 = fst<int, int>.StaticRun(t);
 #line 24 "Content\Collections\transform.mc"

var tmp_0 = tmp_1;

var x = tmp_0; 
 #line 24 "Content\Collections\transform.mc"
var tmp_3 = snd<int, int>.StaticRun(t);
 #line 24 "Content\Collections\transform.mc"

var tmp_2 = tmp_3;

var y = tmp_2; 
 #line 24 "Content\Collections\transform.mc"
var result = (x+y);
 #line 24 "Content\Collections\transform.mc"
 return result; 
 } 

  
throw new System.Exception("Error evaluating: run1. No result returned."); }
public int Run() { return StaticRun(P1); }


public override string ToString() {
 var res = "("; 

 res += " run1 "; if (P1 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P1.ToString(); } 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as run1;
 if(tmp != null) return this.P1.Equals(tmp.P1); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class run2 : TupleOperator  {
public Tuple<int, ImmutableDictionary<string, int>> P1;

public run2(Tuple<int, ImmutableDictionary<string, int>> P1) {this.P1 = P1;}
public static run2 Create(Tuple<int, ImmutableDictionary<string, int>> P1) { return new run2(P1); }

  public static Tuple<int, ImmutableDictionary<string, int>> StaticRun(Tuple<int, ImmutableDictionary<string, int>> P1) {    
 { 
 #line 29 "Content\Collections\transform.mc"
var t = P1; 
 #line 29 "Content\Collections\transform.mc"
var tmp_1 = fst<int, ImmutableDictionary<string,int>>.StaticRun(t);
 #line 29 "Content\Collections\transform.mc"

var tmp_0 = tmp_1;

var x = tmp_0; 
 #line 29 "Content\Collections\transform.mc"
var tmp_3 = snd<int, ImmutableDictionary<string,int>>.StaticRun(t);
 #line 29 "Content\Collections\transform.mc"

var tmp_2 = tmp_3;

var y = tmp_2; var x1 = (x+1); var y1 = (y.Add("k",x1)); 
 #line 29 "Content\Collections\transform.mc"
var result = _Comma<int, ImmutableDictionary<string,int>>.Create(x1, y1);
 #line 29 "Content\Collections\transform.mc"
 return result; 
 } 

  
throw new System.Exception("Error evaluating: run2. No result returned."); }
public Tuple<int, ImmutableDictionary<string, int>> Run() { return StaticRun(P1); }


public override string ToString() {
 var res = "("; 

 res += " run2 "; if (P1 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P1.ToString(); } 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as run2;
 if(tmp != null) return this.P1.Equals(tmp.P1); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class run3 : TupleOperator  {
public Tuple<int, Tuple<ImmutableDictionary<string, int>, ImmutableDictionary<string, int>>> P1;

public run3(Tuple<int, Tuple<ImmutableDictionary<string, int>, ImmutableDictionary<string, int>>> P1) {this.P1 = P1;}
public static run3 Create(Tuple<int, Tuple<ImmutableDictionary<string, int>, ImmutableDictionary<string, int>>> P1) { return new run3(P1); }

  public static Tuple<int, ImmutableDictionary<string, int>> StaticRun(Tuple<int, Tuple<ImmutableDictionary<string, int>, ImmutableDictionary<string, int>>> P1) {    
 { 
 #line 36 "Content\Collections\transform.mc"
var t = P1; 
 #line 36 "Content\Collections\transform.mc"
var tmp_1 = fst<int, Tuple<ImmutableDictionary<string,int>,ImmutableDictionary<string,int>>>.StaticRun(t);
 #line 36 "Content\Collections\transform.mc"

var tmp_0 = tmp_1;

var x = tmp_0; 
 #line 36 "Content\Collections\transform.mc"
var tmp_3 = snd<int, Tuple<ImmutableDictionary<string,int>,ImmutableDictionary<string,int>>>.StaticRun(t);
 #line 36 "Content\Collections\transform.mc"

var tmp_2 = tmp_3;

var t1 = tmp_2; 
 #line 36 "Content\Collections\transform.mc"
var tmp_5 = snd<ImmutableDictionary<string,int>, ImmutableDictionary<string,int>>.StaticRun(t1);
 #line 36 "Content\Collections\transform.mc"

var tmp_4 = tmp_5;

var y = tmp_4; var x1 = (x+1); var y1 = (y.Add("k",x1)); 
 #line 36 "Content\Collections\transform.mc"
var result = _Comma<int, ImmutableDictionary<string,int>>.Create(x1, y1);
 #line 36 "Content\Collections\transform.mc"
 return result; 
 } 

  
throw new System.Exception("Error evaluating: run3. No result returned."); }
public Tuple<int, ImmutableDictionary<string, int>> Run() { return StaticRun(P1); }


public override string ToString() {
 var res = "("; 

 res += " run3 "; if (P1 is System.Collections.IEnumerable) { res += "{"; foreach(var x in P1 as System.Collections.IEnumerable) res += x.ToString(); res += "}";  } else { res += P1.ToString(); } 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as run3;
 if(tmp != null) return this.P1.Equals(tmp.P1); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class run4 : TupleOperator  {

public run4() {}
public static run4 Create() { return new run4(); }

  public static Tuple<float, float> StaticRun() {    
 { 
 #line 44 "Content\Collections\transform.mc"

 #line 44 "Content\Collections\transform.mc"
var result = _Comma<float, float>.Create(1.000000f, 2.000000f);
 #line 44 "Content\Collections\transform.mc"
 return result; 
 } 

  
throw new System.Exception("Error evaluating: run4. No result returned."); }
public Tuple<float, float> Run() { return StaticRun(); }


public override string ToString() {
return "run4";
}

public override bool Equals(object other) {
 return other is run4; 
}

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
 #line 16 "Content\Collections\transform.mc"
var tmp_0 = P1 as _Comma<a, b>; 
 #line 16 "Content\Collections\transform.mc"
if (tmp_0 != null) { 
 var x = tmp_0.P1; var y = tmp_0.P2; 
 #line 16 "Content\Collections\transform.mc"
var result = y;
 #line 16 "Content\Collections\transform.mc"
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
static public object Run(bool printInput)
{
 #line 1 "input"
 var p = run4.Create();
if(printInput) System.Console.WriteLine(p.ToString());
 
 var result = p.Run(); 

return result;
}
}

}
