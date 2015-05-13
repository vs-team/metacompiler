using System;
using System.Collections.Generic;
using System.Linq;
namespace Lists {
 public static class Extensions { public static V GetKey<T, V>(this System.Collections.Immutable.ImmutableDictionary<T, V> self, T key) { return self[key]; } }
 public interface IRunnable { IEnumerable<IRunnable> Run();
 }


public interface ListInt : IRunnable {}
public interface IntValue : IRunnable {}
public interface Bool : IRunnable {}
public interface ListIntPair : IRunnable {}



public class _opDollar : IntValue  {
public int P1;

public _opDollar(int P1) {this.P1 = P1;}
public static _opDollar Create(int P1) { return new _opDollar(P1); }


public static IEnumerable<IRunnable> StaticRun(int P1) { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run(){ return StaticRun(P1); }

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

public class _Comma : ListIntPair  {
public ListInt P1;
public ListInt P2;

public _Comma(ListInt P1, ListInt P2) {this.P1 = P1; this.P2 = P2;}
public static _Comma Create(ListInt P1, ListInt P2) { return new _Comma(P1, P2); }


public static IEnumerable<IRunnable> StaticRun(ListInt P1, ListInt P2) { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run(){ return StaticRun(P1, P2); }

public override string ToString() {
 var res = "("; 
res += P1.ToString(); 

 res += " , "; res += P2.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as _Comma;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
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


public static IEnumerable<IRunnable> StaticRun(int P1, ListInt P2) { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run(){ return StaticRun(P1, P2); }

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

public class add : IntValue  {
public ListInt P1;

public add(ListInt P1) {this.P1 = P1;}
public static add Create(ListInt P1) { return new add(P1); }

  public static IEnumerable<IRunnable> StaticRun(ListInt P1) {   
 { 
 var tmp_0 = P1 as nil; 
if (tmp_0 != null) { 
var result = _opDollar.Create(0);
yield return result;  }
 } 

  
 { 
 var tmp_0 = P1 as _Semicolon; 
if (tmp_0 != null) { var x = tmp_0.P1; var xs = tmp_0.P2; 
if(xs is ListInt) { 
var tmp_2 = add.Create(xs as ListInt);
foreach (var tmp_1 in tmp_2.Run()) { var tmp_3 = tmp_1 as _opDollar; 
if (tmp_3 != null) { var res = tmp_3.P1; 
var result = _opDollar.Create(x+res);
yield return result;  } } } }
 } 

  }
public IEnumerable<IRunnable> Run() { return StaticRun(P1); }


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

public class contains : Bool  {
public ListInt P1;
public int P2;

public contains(ListInt P1, int P2) {this.P1 = P1; this.P2 = P2;}
public static contains Create(ListInt P1, int P2) { return new contains(P1, P2); }

  public static IEnumerable<IRunnable> StaticRun(ListInt P1, int P2) {   
 { 
 var tmp_0 = P1 as nil; 
if (tmp_0 != null) { var k = P2; 
var result = no.Create();
yield return result;  }
 } 

  
 { 
 var tmp_0 = P1 as _Semicolon; 
if (tmp_0 != null) { var x = tmp_0.P1; var xs = tmp_0.P2; var k = P2; 
if(x.Equals(k)) { 
var result = yes.Create();
yield return result;  } }
 } 

  
 { 
 var tmp_0 = P1 as _Semicolon; 
if (tmp_0 != null) { var x = tmp_0.P1; var xs = tmp_0.P2; var k = P2; 
if(!x.Equals(k)) { 
if(xs is ListInt) { 
var tmp_2 = contains.Create(xs as ListInt, k);
foreach (var tmp_1 in tmp_2.Run()) { var res = tmp_1; 
var result = res;
yield return result;  } } } }
 } 

  }
public IEnumerable<IRunnable> Run() { return StaticRun(P1, P2); }


public override string ToString() {
 var res = "("; 

 res += " contains "; res += P1.ToString(); 
res += P2.ToString(); 

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

public class length : IntValue  {
public ListInt P1;

public length(ListInt P1) {this.P1 = P1;}
public static length Create(ListInt P1) { return new length(P1); }

  public static IEnumerable<IRunnable> StaticRun(ListInt P1) {   
 { 
 var tmp_0 = P1 as nil; 
if (tmp_0 != null) { 
var result = _opDollar.Create(0);
yield return result;  }
 } 

  
 { 
 var tmp_0 = P1 as _Semicolon; 
if (tmp_0 != null) { var x = tmp_0.P1; var xs = tmp_0.P2; 
if(xs is ListInt) { 
var tmp_2 = length.Create(xs as ListInt);
foreach (var tmp_1 in tmp_2.Run()) { var tmp_3 = tmp_1 as _opDollar; 
if (tmp_3 != null) { var y = tmp_3.P1; 
var result = _opDollar.Create(1+y);
yield return result;  } } } }
 } 

  }
public IEnumerable<IRunnable> Run() { return StaticRun(P1); }


public override string ToString() {
 var res = "("; 

 res += " length "; res += P1.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as length;
 if(tmp != null) return this.P1.Equals(tmp.P1); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class merge : ListInt  {
public ListInt P1;
public ListInt P2;

public merge(ListInt P1, ListInt P2) {this.P1 = P1; this.P2 = P2;}
public static merge Create(ListInt P1, ListInt P2) { return new merge(P1, P2); }

  public static IEnumerable<IRunnable> StaticRun(ListInt P1, ListInt P2) {   
 { 
 var tmp_0 = P1 as nil; 
if (tmp_0 != null) { var tmp_1 = P2 as nil; 
if (tmp_1 != null) { 
var result = nil.Create();
yield return result;  } }
 } 

  
 { 
 var tmp_0 = P1 as _Semicolon; 
if (tmp_0 != null) { var x = tmp_0.P1; var xs = tmp_0.P2; var tmp_1 = P2 as nil; 
if (tmp_1 != null) { 
if(xs is ListInt) { 
var result = _Semicolon.Create(x, xs as ListInt);
yield return result;  } } }
 } 

  
 { 
 var tmp_0 = P1 as nil; 
if (tmp_0 != null) { var tmp_1 = P2 as _Semicolon; 
if (tmp_1 != null) { var y = tmp_1.P1; var ys = tmp_1.P2; 
if(ys is ListInt) { 
var result = _Semicolon.Create(y, ys as ListInt);
yield return result;  } } }
 } 

  
 { 
 var tmp_0 = P1 as _Semicolon; 
if (tmp_0 != null) { var x = tmp_0.P1; var xs = tmp_0.P2; var tmp_1 = P2 as _Semicolon; 
if (tmp_1 != null) { var y = tmp_1.P1; var ys = tmp_1.P2; 
if(x<=y) { 
if(xs is ListInt && ys is ListInt) { 
var tmp_3 = merge.Create(xs as ListInt, _Semicolon.Create(y, ys as ListInt));
foreach (var tmp_2 in tmp_3.Run()) { var res = tmp_2; 
if(res is ListInt) { 
var result = _Semicolon.Create(x, res as ListInt);
yield return result;  } } } } } }
 } 

  
 { 
 var tmp_0 = P1 as _Semicolon; 
if (tmp_0 != null) { var x = tmp_0.P1; var xs = tmp_0.P2; var tmp_1 = P2 as _Semicolon; 
if (tmp_1 != null) { var y = tmp_1.P1; var ys = tmp_1.P2; 
if(x>y) { 
if(xs is ListInt && ys is ListInt) { 
var tmp_3 = merge.Create(_Semicolon.Create(x, xs as ListInt), ys as ListInt);
foreach (var tmp_2 in tmp_3.Run()) { var res = tmp_2; 
if(res is ListInt) { 
var result = _Semicolon.Create(y, res as ListInt);
yield return result;  } } } } } }
 } 

  }
public IEnumerable<IRunnable> Run() { return StaticRun(P1, P2); }


public override string ToString() {
 var res = "("; 

 res += " merge "; res += P1.ToString(); 
res += P2.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as merge;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class mergeSort : ListInt  {
public ListInt P1;

public mergeSort(ListInt P1) {this.P1 = P1;}
public static mergeSort Create(ListInt P1) { return new mergeSort(P1); }

  public static IEnumerable<IRunnable> StaticRun(ListInt P1) {   
 { 
 var tmp_0 = P1 as nil; 
if (tmp_0 != null) { 
var result = nil.Create();
yield return result;  }
 } 

  
 { 
 var tmp_0 = P1 as _Semicolon; 
if (tmp_0 != null) { var x = tmp_0.P1; var tmp_1 = tmp_0.P2 as nil; 
if (tmp_1 != null) { 
var result = _Semicolon.Create(x, nil.Create());
yield return result;  } }
 } 

  
 { 
 var tmp_0 = P1 as _Semicolon; 
if (tmp_0 != null) { var x = tmp_0.P1; var tmp_1 = tmp_0.P2 as _Semicolon; 
if (tmp_1 != null) { var y = tmp_1.P1; var xs = tmp_1.P2; 
if(xs is ListInt) { 
var tmp_3 = split.Create(_Semicolon.Create(x, _Semicolon.Create(y, xs as ListInt)));
foreach (var tmp_2 in tmp_3.Run()) { var tmp_4 = tmp_2 as _Comma; 
if (tmp_4 != null) { var l = tmp_4.P1; var r = tmp_4.P2; 
if(l is ListInt) { 
var tmp_6 = mergeSort.Create(l as ListInt);
foreach (var tmp_5 in tmp_6.Run()) { var l_Prime = tmp_5; 
if(r is ListInt) { 
var tmp_8 = mergeSort.Create(r as ListInt);
foreach (var tmp_7 in tmp_8.Run()) { var r_Prime = tmp_7; 
if(l_Prime is ListInt && r_Prime is ListInt) { 
var tmp_10 = merge.Create(l_Prime as ListInt, r_Prime as ListInt);
foreach (var tmp_9 in tmp_10.Run()) { var res = tmp_9; 
var result = res;
yield return result;  } } } } } } } } } } }
 } 

  }
public IEnumerable<IRunnable> Run() { return StaticRun(P1); }


public override string ToString() {
 var res = "("; 

 res += " mergeSort "; res += P1.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as mergeSort;
 if(tmp != null) return this.P1.Equals(tmp.P1); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class nil : ListInt  {

public nil() {}
public static nil Create() { return new nil(); }


public static IEnumerable<IRunnable> StaticRun() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run(){ return StaticRun(); }

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


public static IEnumerable<IRunnable> StaticRun() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run(){ return StaticRun(); }

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

public class plus : ListInt  {
public ListInt P1;
public int P2;

public plus(ListInt P1, int P2) {this.P1 = P1; this.P2 = P2;}
public static plus Create(ListInt P1, int P2) { return new plus(P1, P2); }

  public static IEnumerable<IRunnable> StaticRun(ListInt P1, int P2) {   
 { 
 var tmp_0 = P1 as nil; 
if (tmp_0 != null) { var k = P2; 
var result = nil.Create();
yield return result;  }
 } 

  
 { 
 var tmp_0 = P1 as _Semicolon; 
if (tmp_0 != null) { var x = tmp_0.P1; var xs = tmp_0.P2; var k = P2; 
if(xs is ListInt) { 
var tmp_2 = plus.Create(xs as ListInt, k);
foreach (var tmp_1 in tmp_2.Run()) { var xs_Prime = tmp_1; var x_Prime = (x+k); 
if(xs_Prime is ListInt) { 
var result = _Semicolon.Create(x_Prime, xs_Prime as ListInt);
yield return result;  } } } }
 } 

  }
public IEnumerable<IRunnable> Run() { return StaticRun(P1, P2); }


public override string ToString() {
 var res = "("; 

 res += " plus "; res += P1.ToString(); 
res += P2.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as plus;
 if(tmp != null) return this.P1.Equals(tmp.P1) && this.P2.Equals(tmp.P2); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class removeOdd : ListInt  {
public ListInt P1;

public removeOdd(ListInt P1) {this.P1 = P1;}
public static removeOdd Create(ListInt P1) { return new removeOdd(P1); }

  public static IEnumerable<IRunnable> StaticRun(ListInt P1) {   
 { 
 var tmp_0 = P1 as nil; 
if (tmp_0 != null) { 
var result = nil.Create();
yield return result;  }
 } 

  
 { 
 var tmp_0 = P1 as _Semicolon; 
if (tmp_0 != null) { var x = tmp_0.P1; var xs = tmp_0.P2; 
if((x%2).Equals(0)) { 
if(xs is ListInt) { 
var tmp_2 = removeOdd.Create(xs as ListInt);
foreach (var tmp_1 in tmp_2.Run()) { var xs_Prime = tmp_1; 
var result = xs_Prime;
yield return result;  } } } }
 } 

  
 { 
 var tmp_0 = P1 as _Semicolon; 
if (tmp_0 != null) { var x = tmp_0.P1; var xs = tmp_0.P2; 
if((x%2).Equals(1)) { 
if(xs is ListInt) { 
var tmp_2 = removeOdd.Create(xs as ListInt);
foreach (var tmp_1 in tmp_2.Run()) { var xs_Prime = tmp_1; 
if(xs_Prime is ListInt) { 
var result = _Semicolon.Create(x, xs_Prime as ListInt);
yield return result;  } } } } }
 } 

  }
public IEnumerable<IRunnable> Run() { return StaticRun(P1); }


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

public class split : ListIntPair  {
public ListInt P1;

public split(ListInt P1) {this.P1 = P1;}
public static split Create(ListInt P1) { return new split(P1); }

  public static IEnumerable<IRunnable> StaticRun(ListInt P1) {   
 { 
 var tmp_0 = P1 as nil; 
if (tmp_0 != null) { 
var result = _Comma.Create(nil.Create(), nil.Create());
yield return result;  }
 } 

  
 { 
 var tmp_0 = P1 as _Semicolon; 
if (tmp_0 != null) { var x = tmp_0.P1; var tmp_1 = tmp_0.P2 as nil; 
if (tmp_1 != null) { 
var result = _Comma.Create(_Semicolon.Create(x, nil.Create()), nil.Create());
yield return result;  } }
 } 

  
 { 
 var tmp_0 = P1 as _Semicolon; 
if (tmp_0 != null) { var x = tmp_0.P1; var tmp_1 = tmp_0.P2 as _Semicolon; 
if (tmp_1 != null) { var y = tmp_1.P1; var xs = tmp_1.P2; 
if(xs is ListInt) { 
var tmp_3 = split.Create(xs as ListInt);
foreach (var tmp_2 in tmp_3.Run()) { var tmp_4 = tmp_2 as _Comma; 
if (tmp_4 != null) { var l = tmp_4.P1; var r = tmp_4.P2; 
if(l is ListInt && r is ListInt) { 
var result = _Comma.Create(_Semicolon.Create(x, l as ListInt), _Semicolon.Create(y, r as ListInt));
yield return result;  } } } } } }
 } 

  }
public IEnumerable<IRunnable> Run() { return StaticRun(P1); }


public override string ToString() {
 var res = "("; 

 res += " split "; res += P1.ToString(); 

 res += ")";
 return res;
}

public override bool Equals(object other) {
 var tmp = other as split;
 if(tmp != null) return this.P1.Equals(tmp.P1); 
 else return false; }

public override int GetHashCode() {
 return 0; 
}

}

public class yes : Bool  {

public yes() {}
public static yes Create() { return new yes(); }


public static IEnumerable<IRunnable> StaticRun() { foreach (var p in Enumerable.Range(0,0)) yield return null; }
public IEnumerable<IRunnable> Run(){ return StaticRun(); }

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
 var p = length.Create(_Semicolon.Create(0, _Semicolon.Create(1, _Semicolon.Create(2, _Semicolon.Create(3, nil.Create())))));
if(printInput) System.Console.WriteLine(p.ToString());
foreach(var x in p.Run())
yield return x;
}
}

}
