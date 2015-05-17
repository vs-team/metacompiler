using System;
using System.Collections.Generic;
using System.Linq;
namespace Lists {
 public static class Extensions { public static V GetKey<T, V>(this System.Collections.Immutable.ImmutableDictionary<T, V> self, T key) { return self[key]; } }



public interface ListInt {}
public interface Expr {}
public interface ListIntPair {}



public class _Comma : ListIntPair  {
public ListInt P1;
public ListInt P2;

public _Comma(ListInt P1, ListInt P2) {this.P1 = P1; this.P2 = P2;}
public static _Comma Create(ListInt P1, ListInt P2) { return new _Comma(P1, P2); }

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

public class add : Expr  {
public ListInt P1;

public add(ListInt P1) {this.P1 = P1;}
public static add Create(ListInt P1) { return new add(P1); }

  public static IEnumerable<int> StaticRun(ListInt P1) {   
 { 
 #line 73 "Content\Lists"
var tmp_0 = P1 as nil; 
 #line 73 "Content\Lists"
if (tmp_0 != null) { 
 #line 73 "Content\Lists"
var result = 0;
 #line 73 "Content\Lists"
yield return result;  }
 } 

  
 { 
 #line 76 "Content\Lists"
var tmp_0 = P1 as _Semicolon; 
 #line 76 "Content\Lists"
if (tmp_0 != null) { var x = tmp_0.P1; var xs = tmp_0.P2; 
 #line 76 "Content\Lists"
var tmp_2 = add.Create(xs);
 #line 76 "Content\Lists"
foreach (var tmp_1 in tmp_2.Run()) { var res = tmp_1; 
 #line 76 "Content\Lists"
var result = (x+res);
 #line 76 "Content\Lists"
yield return result;  } }
 } 

  }
public IEnumerable<int> Run() { return StaticRun(P1); }


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

public class contains : Expr  {
public ListInt P1;
public int P2;

public contains(ListInt P1, int P2) {this.P1 = P1; this.P2 = P2;}
public static contains Create(ListInt P1, int P2) { return new contains(P1, P2); }

  public static IEnumerable<bool> StaticRun(ListInt P1, int P2) {   
 { 
 #line 90 "Content\Lists"
var tmp_0 = P1 as nil; 
 #line 90 "Content\Lists"
if (tmp_0 != null) { var k = P2; 
 #line 90 "Content\Lists"
var result = false;
 #line 90 "Content\Lists"
yield return result;  }
 } 

  
 { 
 #line 93 "Content\Lists"
var tmp_0 = P1 as _Semicolon; 
 #line 93 "Content\Lists"
if (tmp_0 != null) { var x = tmp_0.P1; var xs = tmp_0.P2; var k = P2; 
 #line 93 "Content\Lists"
if(x.Equals(k)) { 
 #line 93 "Content\Lists"
var result = true;
 #line 93 "Content\Lists"
yield return result;  } }
 } 

  
 { 
 #line 97 "Content\Lists"
var tmp_0 = P1 as _Semicolon; 
 #line 97 "Content\Lists"
if (tmp_0 != null) { var x = tmp_0.P1; var xs = tmp_0.P2; var k = P2; 
 #line 97 "Content\Lists"
if(!x.Equals(k)) { 
 #line 97 "Content\Lists"
var tmp_2 = contains.Create(xs, k);
 #line 97 "Content\Lists"
foreach (var tmp_1 in tmp_2.Run()) { var res = tmp_1; 
 #line 97 "Content\Lists"
var result = res;
 #line 97 "Content\Lists"
yield return result;  } } }
 } 

  }
public IEnumerable<bool> Run() { return StaticRun(P1, P2); }


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

public class length : Expr  {
public ListInt P1;

public length(ListInt P1) {this.P1 = P1;}
public static length Create(ListInt P1) { return new length(P1); }

  public static IEnumerable<int> StaticRun(ListInt P1) {   
 { 
 #line 65 "Content\Lists"
var tmp_0 = P1 as nil; 
 #line 65 "Content\Lists"
if (tmp_0 != null) { 
 #line 65 "Content\Lists"
var result = 0;
 #line 65 "Content\Lists"
yield return result;  }
 } 

  
 { 
 #line 68 "Content\Lists"
var tmp_0 = P1 as _Semicolon; 
 #line 68 "Content\Lists"
if (tmp_0 != null) { var x = tmp_0.P1; var xs = tmp_0.P2; 
 #line 68 "Content\Lists"
var tmp_2 = length.Create(xs);
 #line 68 "Content\Lists"
foreach (var tmp_1 in tmp_2.Run()) { var y = tmp_1; 
 #line 68 "Content\Lists"
var result = (1+y);
 #line 68 "Content\Lists"
yield return result;  } }
 } 

  }
public IEnumerable<int> Run() { return StaticRun(P1); }


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

public class merge : Expr  {
public ListInt P1;
public ListInt P2;

public merge(ListInt P1, ListInt P2) {this.P1 = P1; this.P2 = P2;}
public static merge Create(ListInt P1, ListInt P2) { return new merge(P1, P2); }

  public static IEnumerable<ListInt> StaticRun(ListInt P1, ListInt P2) {   
 { 
 #line 32 "Content\Lists"
var tmp_0 = P1 as nil; 
 #line 32 "Content\Lists"
if (tmp_0 != null) { var tmp_1 = P2 as nil; 
 #line 32 "Content\Lists"
if (tmp_1 != null) { 
 #line 32 "Content\Lists"
var result = nil.Create();
 #line 32 "Content\Lists"
yield return result;  } }
 } 

  
 { 
 #line 35 "Content\Lists"
var tmp_0 = P1 as _Semicolon; 
 #line 35 "Content\Lists"
if (tmp_0 != null) { var x = tmp_0.P1; var xs = tmp_0.P2; var tmp_1 = P2 as nil; 
 #line 35 "Content\Lists"
if (tmp_1 != null) { 
 #line 35 "Content\Lists"
var result = _Semicolon.Create(x, xs);
 #line 35 "Content\Lists"
yield return result;  } }
 } 

  
 { 
 #line 38 "Content\Lists"
var tmp_0 = P1 as nil; 
 #line 38 "Content\Lists"
if (tmp_0 != null) { var tmp_1 = P2 as _Semicolon; 
 #line 38 "Content\Lists"
if (tmp_1 != null) { var y = tmp_1.P1; var ys = tmp_1.P2; 
 #line 38 "Content\Lists"
var result = _Semicolon.Create(y, ys);
 #line 38 "Content\Lists"
yield return result;  } }
 } 

  
 { 
 #line 41 "Content\Lists"
var tmp_0 = P1 as _Semicolon; 
 #line 41 "Content\Lists"
if (tmp_0 != null) { var x = tmp_0.P1; var xs = tmp_0.P2; var tmp_1 = P2 as _Semicolon; 
 #line 41 "Content\Lists"
if (tmp_1 != null) { var y = tmp_1.P1; var ys = tmp_1.P2; 
 #line 41 "Content\Lists"
if(x<=y) { 
 #line 41 "Content\Lists"
var tmp_3 = merge.Create(xs, _Semicolon.Create(y, ys));
 #line 41 "Content\Lists"
foreach (var tmp_2 in tmp_3.Run()) { var res = tmp_2; 
 #line 41 "Content\Lists"
var result = _Semicolon.Create(x, res);
 #line 41 "Content\Lists"
yield return result;  } } } }
 } 

  
 { 
 #line 46 "Content\Lists"
var tmp_0 = P1 as _Semicolon; 
 #line 46 "Content\Lists"
if (tmp_0 != null) { var x = tmp_0.P1; var xs = tmp_0.P2; var tmp_1 = P2 as _Semicolon; 
 #line 46 "Content\Lists"
if (tmp_1 != null) { var y = tmp_1.P1; var ys = tmp_1.P2; 
 #line 46 "Content\Lists"
if(x>y) { 
 #line 46 "Content\Lists"
var tmp_3 = merge.Create(_Semicolon.Create(x, xs), ys);
 #line 46 "Content\Lists"
foreach (var tmp_2 in tmp_3.Run()) { var res = tmp_2; 
 #line 46 "Content\Lists"
var result = _Semicolon.Create(y, res);
 #line 46 "Content\Lists"
yield return result;  } } } }
 } 

  }
public IEnumerable<ListInt> Run() { return StaticRun(P1, P2); }


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

public class mergeSort : Expr  {
public ListInt P1;

public mergeSort(ListInt P1) {this.P1 = P1;}
public static mergeSort Create(ListInt P1) { return new mergeSort(P1); }

  public static IEnumerable<ListInt> StaticRun(ListInt P1) {   
 { 
 #line 51 "Content\Lists"
var tmp_0 = P1 as nil; 
 #line 51 "Content\Lists"
if (tmp_0 != null) { 
 #line 51 "Content\Lists"
var result = nil.Create();
 #line 51 "Content\Lists"
yield return result;  }
 } 

  
 { 
 #line 54 "Content\Lists"
var tmp_0 = P1 as _Semicolon; 
 #line 54 "Content\Lists"
if (tmp_0 != null) { var x = tmp_0.P1; var tmp_1 = tmp_0.P2 as nil; 
 #line 54 "Content\Lists"
if (tmp_1 != null) { 
 #line 54 "Content\Lists"
var result = _Semicolon.Create(x, nil.Create());
 #line 54 "Content\Lists"
yield return result;  } }
 } 

  
 { 
 #line 57 "Content\Lists"
var tmp_0 = P1 as _Semicolon; 
 #line 57 "Content\Lists"
if (tmp_0 != null) { var x = tmp_0.P1; var tmp_1 = tmp_0.P2 as _Semicolon; 
 #line 57 "Content\Lists"
if (tmp_1 != null) { var y = tmp_1.P1; var xs = tmp_1.P2; 
 #line 57 "Content\Lists"
var tmp_3 = split.Create(_Semicolon.Create(x, _Semicolon.Create(y, xs)));
 #line 57 "Content\Lists"
foreach (var tmp_2 in tmp_3.Run()) { var tmp_4 = tmp_2 as _Comma; 
 #line 57 "Content\Lists"
if (tmp_4 != null) { var l = tmp_4.P1; var r = tmp_4.P2; 
 #line 57 "Content\Lists"
var tmp_6 = mergeSort.Create(l);
 #line 57 "Content\Lists"
foreach (var tmp_5 in tmp_6.Run()) { var l_Prime = tmp_5; 
 #line 57 "Content\Lists"
var tmp_8 = mergeSort.Create(r);
 #line 57 "Content\Lists"
foreach (var tmp_7 in tmp_8.Run()) { var r_Prime = tmp_7; 
 #line 57 "Content\Lists"
var tmp_10 = merge.Create(l_Prime, r_Prime);
 #line 57 "Content\Lists"
foreach (var tmp_9 in tmp_10.Run()) { var res = tmp_9; 
 #line 57 "Content\Lists"
var result = res;
 #line 57 "Content\Lists"
yield return result;  } } } } } } }
 } 

  }
public IEnumerable<ListInt> Run() { return StaticRun(P1); }


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

public class plus : Expr  {
public ListInt P1;
public int P2;

public plus(ListInt P1, int P2) {this.P1 = P1; this.P2 = P2;}
public static plus Create(ListInt P1, int P2) { return new plus(P1, P2); }

  public static IEnumerable<ListInt> StaticRun(ListInt P1, int P2) {   
 { 
 #line 81 "Content\Lists"
var tmp_0 = P1 as nil; 
 #line 81 "Content\Lists"
if (tmp_0 != null) { var k = P2; 
 #line 81 "Content\Lists"
var result = nil.Create();
 #line 81 "Content\Lists"
yield return result;  }
 } 

  
 { 
 #line 84 "Content\Lists"
var tmp_0 = P1 as _Semicolon; 
 #line 84 "Content\Lists"
if (tmp_0 != null) { var x = tmp_0.P1; var xs = tmp_0.P2; var k = P2; 
 #line 84 "Content\Lists"
var tmp_2 = plus.Create(xs, k);
 #line 84 "Content\Lists"
foreach (var tmp_1 in tmp_2.Run()) { var xs_Prime = tmp_1; var x_Prime = (x+k); 
 #line 84 "Content\Lists"
var result = _Semicolon.Create(x_Prime, xs_Prime);
 #line 84 "Content\Lists"
yield return result;  } }
 } 

  }
public IEnumerable<ListInt> Run() { return StaticRun(P1, P2); }


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

public class removeOdd : Expr  {
public ListInt P1;

public removeOdd(ListInt P1) {this.P1 = P1;}
public static removeOdd Create(ListInt P1) { return new removeOdd(P1); }

  public static IEnumerable<ListInt> StaticRun(ListInt P1) {   
 { 
 #line 103 "Content\Lists"
var tmp_0 = P1 as nil; 
 #line 103 "Content\Lists"
if (tmp_0 != null) { 
 #line 103 "Content\Lists"
var result = nil.Create();
 #line 103 "Content\Lists"
yield return result;  }
 } 

  
 { 
 #line 106 "Content\Lists"
var tmp_0 = P1 as _Semicolon; 
 #line 106 "Content\Lists"
if (tmp_0 != null) { var x = tmp_0.P1; var xs = tmp_0.P2; 
 #line 106 "Content\Lists"
if((x%2).Equals(0)) { 
 #line 106 "Content\Lists"
var tmp_2 = removeOdd.Create(xs);
 #line 106 "Content\Lists"
foreach (var tmp_1 in tmp_2.Run()) { var xs_Prime = tmp_1; 
 #line 106 "Content\Lists"
var result = xs_Prime;
 #line 106 "Content\Lists"
yield return result;  } } }
 } 

  
 { 
 #line 111 "Content\Lists"
var tmp_0 = P1 as _Semicolon; 
 #line 111 "Content\Lists"
if (tmp_0 != null) { var x = tmp_0.P1; var xs = tmp_0.P2; 
 #line 111 "Content\Lists"
if((x%2).Equals(1)) { 
 #line 111 "Content\Lists"
var tmp_2 = removeOdd.Create(xs);
 #line 111 "Content\Lists"
foreach (var tmp_1 in tmp_2.Run()) { var xs_Prime = tmp_1; 
 #line 111 "Content\Lists"
var result = _Semicolon.Create(x, xs_Prime);
 #line 111 "Content\Lists"
yield return result;  } } }
 } 

  }
public IEnumerable<ListInt> Run() { return StaticRun(P1); }


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

public class split : Expr  {
public ListInt P1;

public split(ListInt P1) {this.P1 = P1;}
public static split Create(ListInt P1) { return new split(P1); }

  public static IEnumerable<ListIntPair> StaticRun(ListInt P1) {   
 { 
 #line 21 "Content\Lists"
var tmp_0 = P1 as nil; 
 #line 21 "Content\Lists"
if (tmp_0 != null) { 
 #line 21 "Content\Lists"
var result = _Comma.Create(nil.Create(), nil.Create());
 #line 21 "Content\Lists"
yield return result;  }
 } 

  
 { 
 #line 24 "Content\Lists"
var tmp_0 = P1 as _Semicolon; 
 #line 24 "Content\Lists"
if (tmp_0 != null) { var x = tmp_0.P1; var tmp_1 = tmp_0.P2 as nil; 
 #line 24 "Content\Lists"
if (tmp_1 != null) { 
 #line 24 "Content\Lists"
var result = _Comma.Create(_Semicolon.Create(x, nil.Create()), nil.Create());
 #line 24 "Content\Lists"
yield return result;  } }
 } 

  
 { 
 #line 27 "Content\Lists"
var tmp_0 = P1 as _Semicolon; 
 #line 27 "Content\Lists"
if (tmp_0 != null) { var x = tmp_0.P1; var tmp_1 = tmp_0.P2 as _Semicolon; 
 #line 27 "Content\Lists"
if (tmp_1 != null) { var y = tmp_1.P1; var xs = tmp_1.P2; 
 #line 27 "Content\Lists"
var tmp_3 = split.Create(xs);
 #line 27 "Content\Lists"
foreach (var tmp_2 in tmp_3.Run()) { var tmp_4 = tmp_2 as _Comma; 
 #line 27 "Content\Lists"
if (tmp_4 != null) { var l = tmp_4.P1; var r = tmp_4.P2; 
 #line 27 "Content\Lists"
var result = _Comma.Create(_Semicolon.Create(x, l), _Semicolon.Create(y, r));
 #line 27 "Content\Lists"
yield return result;  } } } }
 } 

  }
public IEnumerable<ListIntPair> Run() { return StaticRun(P1); }


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




public class EntryPoint {
 static public int Sleep(float s) { int t = (int)(s * 1000.0f); ; return 0; } 
static public IEnumerable<object> Run(bool printInput)
{
 #line 1 "input"
 var p = add.Create(_Semicolon.Create(0, _Semicolon.Create(1, _Semicolon.Create(2, _Semicolon.Create(3, nil.Create())))));
if(printInput) System.Console.WriteLine(p.ToString());
foreach(var x in p.Run())
yield return x;
}
}

}
