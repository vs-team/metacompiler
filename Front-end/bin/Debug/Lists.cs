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

  public static int StaticRun(ListInt P1) {   
 { 
 var tmp_0 = P1 as nil; 
if (tmp_0 != null) { 
var result = 0;
 return result;  }
 } 

  
 { 
 var tmp_0 = P1 as _Semicolon; 
if (tmp_0 != null) { var x = tmp_0.P1; var xs = tmp_0.P2; 
var tmp_2 = add.Create(xs);

var tmp_1 = tmp_2.Run();
var res = tmp_1; 
var result = (x+res);
 return result;  }
 } 


 throw new System.Exception("Error evaluating: " + new add(P1).ToString() + " no result returned.");  }
public int Run() { return StaticRun(P1); }


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

  public static bool StaticRun(ListInt P1, int P2) {   
 { 
 var tmp_0 = P1 as nil; 
if (tmp_0 != null) { var k = P2; 
var result = false;
 return result;  }
 } 

  
 { 
 var tmp_0 = P1 as _Semicolon; 
if (tmp_0 != null) { var x = tmp_0.P1; var xs = tmp_0.P2; var k = P2; 
if(x.Equals(k)) { 
var result = true;
 return result;  } }
 } 

  
 { 
 var tmp_0 = P1 as _Semicolon; 
if (tmp_0 != null) { var x = tmp_0.P1; var xs = tmp_0.P2; var k = P2; 
if(!x.Equals(k)) { 
var tmp_2 = contains.Create(xs, k);

var tmp_1 = tmp_2.Run();
var res = tmp_1; 
var result = res;
 return result;  } }
 } 


 throw new System.Exception("Error evaluating: " + new contains(P1, P2).ToString() + " no result returned.");  }
public bool Run() { return StaticRun(P1, P2); }


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

  public static int StaticRun(ListInt P1) {   
 { 
 var tmp_0 = P1 as nil; 
if (tmp_0 != null) { 
var result = 0;
 return result;  }
 } 

  
 { 
 var tmp_0 = P1 as _Semicolon; 
if (tmp_0 != null) { var x = tmp_0.P1; var xs = tmp_0.P2; 
var tmp_2 = length.Create(xs);

var tmp_1 = tmp_2.Run();
var y = tmp_1; 
var result = (1+y);
 return result;  }
 } 


 throw new System.Exception("Error evaluating: " + new length(P1).ToString() + " no result returned.");  }
public int Run() { return StaticRun(P1); }


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

  public static ListInt StaticRun(ListInt P1, ListInt P2) {   
 { 
 var tmp_0 = P1 as nil; 
if (tmp_0 != null) { var tmp_1 = P2 as nil; 
if (tmp_1 != null) { 
var result = nil.Create();
 return result;  } }
 } 

  
 { 
 var tmp_0 = P1 as _Semicolon; 
if (tmp_0 != null) { var x = tmp_0.P1; var xs = tmp_0.P2; var tmp_1 = P2 as nil; 
if (tmp_1 != null) { 
var result = _Semicolon.Create(x, xs);
 return result;  } }
 } 

  
 { 
 var tmp_0 = P1 as nil; 
if (tmp_0 != null) { var tmp_1 = P2 as _Semicolon; 
if (tmp_1 != null) { var y = tmp_1.P1; var ys = tmp_1.P2; 
var result = _Semicolon.Create(y, ys);
 return result;  } }
 } 

  
 { 
 var tmp_0 = P1 as _Semicolon; 
if (tmp_0 != null) { var x = tmp_0.P1; var xs = tmp_0.P2; var tmp_1 = P2 as _Semicolon; 
if (tmp_1 != null) { var y = tmp_1.P1; var ys = tmp_1.P2; 
if(x<=y) { 
var tmp_3 = merge.Create(xs, _Semicolon.Create(y, ys));

var tmp_2 = tmp_3.Run();
var res = tmp_2; 
var result = _Semicolon.Create(x, res);
 return result;  } } }
 } 

  
 { 
 var tmp_0 = P1 as _Semicolon; 
if (tmp_0 != null) { var x = tmp_0.P1; var xs = tmp_0.P2; var tmp_1 = P2 as _Semicolon; 
if (tmp_1 != null) { var y = tmp_1.P1; var ys = tmp_1.P2; 
if(x>y) { 
var tmp_3 = merge.Create(_Semicolon.Create(x, xs), ys);

var tmp_2 = tmp_3.Run();
var res = tmp_2; 
var result = _Semicolon.Create(y, res);
 return result;  } } }
 } 


 throw new System.Exception("Error evaluating: " + new merge(P1, P2).ToString() + " no result returned.");  }
public ListInt Run() { return StaticRun(P1, P2); }


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

  public static ListInt StaticRun(ListInt P1) {   
 { 
 var tmp_0 = P1 as nil; 
if (tmp_0 != null) { 
var result = nil.Create();
 return result;  }
 } 

  
 { 
 var tmp_0 = P1 as _Semicolon; 
if (tmp_0 != null) { var x = tmp_0.P1; var tmp_1 = tmp_0.P2 as nil; 
if (tmp_1 != null) { 
var result = _Semicolon.Create(x, nil.Create());
 return result;  } }
 } 

  
 { 
 var tmp_0 = P1 as _Semicolon; 
if (tmp_0 != null) { var x = tmp_0.P1; var tmp_1 = tmp_0.P2 as _Semicolon; 
if (tmp_1 != null) { var y = tmp_1.P1; var xs = tmp_1.P2; 
var tmp_3 = split.Create(_Semicolon.Create(x, _Semicolon.Create(y, xs)));

var tmp_2 = tmp_3.Run();
var tmp_4 = tmp_2 as _Comma; 
if (tmp_4 != null) { var l = tmp_4.P1; var r = tmp_4.P2; 
var tmp_6 = mergeSort.Create(l);

var tmp_5 = tmp_6.Run();
var l_Prime = tmp_5; 
var tmp_8 = mergeSort.Create(r);

var tmp_7 = tmp_8.Run();
var r_Prime = tmp_7; 
var tmp_10 = merge.Create(l_Prime, r_Prime);

var tmp_9 = tmp_10.Run();
var res = tmp_9; 
var result = res;
 return result;  } } }
 } 


 throw new System.Exception("Error evaluating: " + new mergeSort(P1).ToString() + " no result returned.");  }
public ListInt Run() { return StaticRun(P1); }


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

  public static ListInt StaticRun(ListInt P1, int P2) {   
 { 
 var tmp_0 = P1 as nil; 
if (tmp_0 != null) { var k = P2; 
var result = nil.Create();
 return result;  }
 } 

  
 { 
 var tmp_0 = P1 as _Semicolon; 
if (tmp_0 != null) { var x = tmp_0.P1; var xs = tmp_0.P2; var k = P2; 
var tmp_2 = plus.Create(xs, k);

var tmp_1 = tmp_2.Run();
var xs_Prime = tmp_1; var x_Prime = (x+k); 
var result = _Semicolon.Create(x_Prime, xs_Prime);
 return result;  }
 } 


 throw new System.Exception("Error evaluating: " + new plus(P1, P2).ToString() + " no result returned.");  }
public ListInt Run() { return StaticRun(P1, P2); }


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

  public static ListInt StaticRun(ListInt P1) {   
 { 
 var tmp_0 = P1 as nil; 
if (tmp_0 != null) { 
var result = nil.Create();
 return result;  }
 } 

  
 { 
 var tmp_0 = P1 as _Semicolon; 
if (tmp_0 != null) { var x = tmp_0.P1; var xs = tmp_0.P2; 
if((x%2).Equals(0)) { 
var tmp_2 = removeOdd.Create(xs);

var tmp_1 = tmp_2.Run();
var xs_Prime = tmp_1; 
var result = xs_Prime;
 return result;  } }
 } 

  
 { 
 var tmp_0 = P1 as _Semicolon; 
if (tmp_0 != null) { var x = tmp_0.P1; var xs = tmp_0.P2; 
if((x%2).Equals(1)) { 
var tmp_2 = removeOdd.Create(xs);

var tmp_1 = tmp_2.Run();
var xs_Prime = tmp_1; 
var result = _Semicolon.Create(x, xs_Prime);
 return result;  } }
 } 


 throw new System.Exception("Error evaluating: " + new removeOdd(P1).ToString() + " no result returned.");  }
public ListInt Run() { return StaticRun(P1); }


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

  public static ListIntPair StaticRun(ListInt P1) {   
 { 
 var tmp_0 = P1 as nil; 
if (tmp_0 != null) { 
var result = _Comma.Create(nil.Create(), nil.Create());
 return result;  }
 } 

  
 { 
 var tmp_0 = P1 as _Semicolon; 
if (tmp_0 != null) { var x = tmp_0.P1; var tmp_1 = tmp_0.P2 as nil; 
if (tmp_1 != null) { 
var result = _Comma.Create(_Semicolon.Create(x, nil.Create()), nil.Create());
 return result;  } }
 } 

  
 { 
 var tmp_0 = P1 as _Semicolon; 
if (tmp_0 != null) { var x = tmp_0.P1; var tmp_1 = tmp_0.P2 as _Semicolon; 
if (tmp_1 != null) { var y = tmp_1.P1; var xs = tmp_1.P2; 
var tmp_3 = split.Create(xs);

var tmp_2 = tmp_3.Run();
var tmp_4 = tmp_2 as _Comma; 
if (tmp_4 != null) { var l = tmp_4.P1; var r = tmp_4.P2; 
var result = _Comma.Create(_Semicolon.Create(x, l), _Semicolon.Create(y, r));
 return result;  } } }
 } 


 throw new System.Exception("Error evaluating: " + new split(P1).ToString() + " no result returned.");  }
public ListIntPair Run() { return StaticRun(P1); }


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
static public object Run(bool printInput)
{
 #line 1 "input"
 var p = add.Create(_Semicolon.Create(0, _Semicolon.Create(1, _Semicolon.Create(2, _Semicolon.Create(3, nil.Create())))));
if(printInput) System.Console.WriteLine(p.ToString());
var result = p.Run();
return result;
}
}

}
