using System;
using System.Collections.Generic;
using System.Linq;
namespace Lists {
 public static class Extensions { public static V GetKey<T, V>(this System.Collections.Immutable.ImmutableDictionary<T, V> self, T key) { return self[key]; } }
 public interface IRunnable { IEnumerable<IRunnable> Run();
 }


public interface Bool : IRunnable {}
public interface IntValue : IRunnable {}
public interface ListInt : IRunnable {}
public interface ListIntPair : IRunnable {}



public class _opDollar : IntValue  {
public int P1;

public _opDollar(int P1) {this.P1 = P1;}
public static _opDollar Create(int P1) { return new _opDollar(P1); }


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }

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


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }

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


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }

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

  public IEnumerable<IRunnable> Run() {   
 { 
 #line 125 "Content\Lists"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as nil; 
 #line 125 "Content\Lists"
if (tmp_1 != null) { 
 #line 125 "Content\Lists"
var result = _opDollar.Create(0);
 #line 125 "Content\Lists"
yield return result;  }
 } 

  
 { 
 #line 130 "Content\Lists"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _Semicolon; 
 #line 130 "Content\Lists"
if (tmp_1 != null) { var x = tmp_1.P1; var xs = tmp_1.P2; 
 #line 130 "Content\Lists"
if(xs is ListInt) { 
 #line 130 "Content\Lists"
var tmp_3 = add.Create(xs as ListInt);
 #line 130 "Content\Lists"
foreach (var tmp_2 in tmp_3.Run()) { var tmp_4 = tmp_2 as _opDollar; 
 #line 130 "Content\Lists"
if (tmp_4 != null) { var res = tmp_4.P1; 
 #line 130 "Content\Lists"
var result = _opDollar.Create(x+res);
 #line 130 "Content\Lists"
yield return result;  } } } }
 } 

  }


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

  public IEnumerable<IRunnable> Run() {   
 { 
 #line 153 "Content\Lists"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as nil; 
 #line 153 "Content\Lists"
if (tmp_1 != null) { var k = tmp_0.P2; 
 #line 153 "Content\Lists"
var result = no.Create();
 #line 153 "Content\Lists"
yield return result;  }
 } 

  
 { 
 #line 158 "Content\Lists"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _Semicolon; 
 #line 158 "Content\Lists"
if (tmp_1 != null) { var x = tmp_1.P1; var xs = tmp_1.P2; var k = tmp_0.P2; 
 #line 158 "Content\Lists"
if(x.Equals(k)) { 
 #line 158 "Content\Lists"
var result = yes.Create();
 #line 158 "Content\Lists"
yield return result;  } }
 } 

  
 { 
 #line 165 "Content\Lists"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _Semicolon; 
 #line 165 "Content\Lists"
if (tmp_1 != null) { var x = tmp_1.P1; var xs = tmp_1.P2; var k = tmp_0.P2; 
 #line 165 "Content\Lists"
if(!x.Equals(k)) { 
 #line 165 "Content\Lists"
if(xs is ListInt) { 
 #line 165 "Content\Lists"
var tmp_3 = contains.Create(xs as ListInt, k);
 #line 165 "Content\Lists"
foreach (var tmp_2 in tmp_3.Run()) { var res = tmp_2; 
 #line 165 "Content\Lists"
var result = res;
 #line 165 "Content\Lists"
yield return result;  } } } }
 } 

  }


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

  public IEnumerable<IRunnable> Run() {   
 { 
 #line 112 "Content\Lists"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as nil; 
 #line 112 "Content\Lists"
if (tmp_1 != null) { 
 #line 112 "Content\Lists"
var result = _opDollar.Create(0);
 #line 112 "Content\Lists"
yield return result;  }
 } 

  
 { 
 #line 117 "Content\Lists"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _Semicolon; 
 #line 117 "Content\Lists"
if (tmp_1 != null) { var x = tmp_1.P1; var xs = tmp_1.P2; 
 #line 117 "Content\Lists"
if(xs is ListInt) { 
 #line 117 "Content\Lists"
var tmp_3 = length.Create(xs as ListInt);
 #line 117 "Content\Lists"
foreach (var tmp_2 in tmp_3.Run()) { var tmp_4 = tmp_2 as _opDollar; 
 #line 117 "Content\Lists"
if (tmp_4 != null) { var y = tmp_4.P1; 
 #line 117 "Content\Lists"
var result = _opDollar.Create(1+y);
 #line 117 "Content\Lists"
yield return result;  } } } }
 } 

  }


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

  public IEnumerable<IRunnable> Run() {   
 { 
 #line 55 "Content\Lists"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as nil; 
 #line 55 "Content\Lists"
if (tmp_1 != null) { var tmp_2 = tmp_0.P2 as nil; 
 #line 55 "Content\Lists"
if (tmp_2 != null) { 
 #line 55 "Content\Lists"
var result = nil.Create();
 #line 55 "Content\Lists"
yield return result;  } }
 } 

  
 { 
 #line 60 "Content\Lists"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _Semicolon; 
 #line 60 "Content\Lists"
if (tmp_1 != null) { var x = tmp_1.P1; var xs = tmp_1.P2; var tmp_2 = tmp_0.P2 as nil; 
 #line 60 "Content\Lists"
if (tmp_2 != null) { 
 #line 60 "Content\Lists"
if(xs is ListInt) { 
 #line 60 "Content\Lists"
var result = _Semicolon.Create(x, xs as ListInt);
 #line 60 "Content\Lists"
yield return result;  } } }
 } 

  
 { 
 #line 65 "Content\Lists"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as nil; 
 #line 65 "Content\Lists"
if (tmp_1 != null) { var tmp_2 = tmp_0.P2 as _Semicolon; 
 #line 65 "Content\Lists"
if (tmp_2 != null) { var y = tmp_2.P1; var ys = tmp_2.P2; 
 #line 65 "Content\Lists"
if(ys is ListInt) { 
 #line 65 "Content\Lists"
var result = _Semicolon.Create(y, ys as ListInt);
 #line 65 "Content\Lists"
yield return result;  } } }
 } 

  
 { 
 #line 70 "Content\Lists"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _Semicolon; 
 #line 70 "Content\Lists"
if (tmp_1 != null) { var x = tmp_1.P1; var xs = tmp_1.P2; var tmp_2 = tmp_0.P2 as _Semicolon; 
 #line 70 "Content\Lists"
if (tmp_2 != null) { var y = tmp_2.P1; var ys = tmp_2.P2; 
 #line 70 "Content\Lists"
if(x<=y) { 
 #line 70 "Content\Lists"
if(xs is ListInt && ys is ListInt) { 
 #line 70 "Content\Lists"
var tmp_4 = merge.Create(xs as ListInt, _Semicolon.Create(y, ys as ListInt));
 #line 70 "Content\Lists"
foreach (var tmp_3 in tmp_4.Run()) { var res = tmp_3; 
 #line 70 "Content\Lists"
if(res is ListInt) { 
 #line 70 "Content\Lists"
var result = _Semicolon.Create(x, res as ListInt);
 #line 70 "Content\Lists"
yield return result;  } } } } } }
 } 

  
 { 
 #line 79 "Content\Lists"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _Semicolon; 
 #line 79 "Content\Lists"
if (tmp_1 != null) { var x = tmp_1.P1; var xs = tmp_1.P2; var tmp_2 = tmp_0.P2 as _Semicolon; 
 #line 79 "Content\Lists"
if (tmp_2 != null) { var y = tmp_2.P1; var ys = tmp_2.P2; 
 #line 79 "Content\Lists"
if(x>y) { 
 #line 79 "Content\Lists"
if(xs is ListInt && ys is ListInt) { 
 #line 79 "Content\Lists"
var tmp_4 = merge.Create(_Semicolon.Create(x, xs as ListInt), ys as ListInt);
 #line 79 "Content\Lists"
foreach (var tmp_3 in tmp_4.Run()) { var res = tmp_3; 
 #line 79 "Content\Lists"
if(res is ListInt) { 
 #line 79 "Content\Lists"
var result = _Semicolon.Create(y, res as ListInt);
 #line 79 "Content\Lists"
yield return result;  } } } } } }
 } 

  }


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

  public IEnumerable<IRunnable> Run() {   
 { 
 #line 88 "Content\Lists"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as nil; 
 #line 88 "Content\Lists"
if (tmp_1 != null) { 
 #line 88 "Content\Lists"
var result = nil.Create();
 #line 88 "Content\Lists"
yield return result;  }
 } 

  
 { 
 #line 93 "Content\Lists"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _Semicolon; 
 #line 93 "Content\Lists"
if (tmp_1 != null) { var x = tmp_1.P1; var tmp_2 = tmp_1.P2 as nil; 
 #line 93 "Content\Lists"
if (tmp_2 != null) { 
 #line 93 "Content\Lists"
var result = _Semicolon.Create(x, nil.Create());
 #line 93 "Content\Lists"
yield return result;  } }
 } 

  
 { 
 #line 98 "Content\Lists"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _Semicolon; 
 #line 98 "Content\Lists"
if (tmp_1 != null) { var x = tmp_1.P1; var tmp_2 = tmp_1.P2 as _Semicolon; 
 #line 98 "Content\Lists"
if (tmp_2 != null) { var y = tmp_2.P1; var xs = tmp_2.P2; 
 #line 98 "Content\Lists"
if(xs is ListInt) { 
 #line 98 "Content\Lists"
var tmp_4 = split.Create(_Semicolon.Create(x, _Semicolon.Create(y, xs as ListInt)));
 #line 98 "Content\Lists"
foreach (var tmp_3 in tmp_4.Run()) { var tmp_5 = tmp_3 as _Comma; 
 #line 98 "Content\Lists"
if (tmp_5 != null) { var l = tmp_5.P1; var r = tmp_5.P2; 
 #line 98 "Content\Lists"
if(l is ListInt) { 
 #line 98 "Content\Lists"
var tmp_7 = mergeSort.Create(l as ListInt);
 #line 98 "Content\Lists"
foreach (var tmp_6 in tmp_7.Run()) { var l_Prime = tmp_6; 
 #line 98 "Content\Lists"
if(r is ListInt) { 
 #line 98 "Content\Lists"
var tmp_9 = mergeSort.Create(r as ListInt);
 #line 98 "Content\Lists"
foreach (var tmp_8 in tmp_9.Run()) { var r_Prime = tmp_8; 
 #line 98 "Content\Lists"
if(l_Prime is ListInt && r_Prime is ListInt) { 
 #line 98 "Content\Lists"
var tmp_11 = merge.Create(l_Prime as ListInt, r_Prime as ListInt);
 #line 98 "Content\Lists"
foreach (var tmp_10 in tmp_11.Run()) { var res = tmp_10; 
 #line 98 "Content\Lists"
var result = res;
 #line 98 "Content\Lists"
yield return result;  } } } } } } } } } } }
 } 

  }


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


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }

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


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }

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

  public IEnumerable<IRunnable> Run() {   
 { 
 #line 138 "Content\Lists"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as nil; 
 #line 138 "Content\Lists"
if (tmp_1 != null) { var k = tmp_0.P2; 
 #line 138 "Content\Lists"
var result = nil.Create();
 #line 138 "Content\Lists"
yield return result;  }
 } 

  
 { 
 #line 143 "Content\Lists"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _Semicolon; 
 #line 143 "Content\Lists"
if (tmp_1 != null) { var x = tmp_1.P1; var xs = tmp_1.P2; var k = tmp_0.P2; 
 #line 143 "Content\Lists"
if(xs is ListInt) { 
 #line 143 "Content\Lists"
var tmp_3 = plus.Create(xs as ListInt, k);
 #line 143 "Content\Lists"
foreach (var tmp_2 in tmp_3.Run()) { var xs_Prime = tmp_2; var x_Prime = (x+k); 
 #line 143 "Content\Lists"
if(xs_Prime is ListInt) { 
 #line 143 "Content\Lists"
var result = _Semicolon.Create(x_Prime, xs_Prime as ListInt);
 #line 143 "Content\Lists"
yield return result;  } } } }
 } 

  }


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

  public IEnumerable<IRunnable> Run() {   
 { 
 #line 175 "Content\Lists"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as nil; 
 #line 175 "Content\Lists"
if (tmp_1 != null) { 
 #line 175 "Content\Lists"
var result = nil.Create();
 #line 175 "Content\Lists"
yield return result;  }
 } 

  
 { 
 #line 180 "Content\Lists"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _Semicolon; 
 #line 180 "Content\Lists"
if (tmp_1 != null) { var x = tmp_1.P1; var xs = tmp_1.P2; 
 #line 180 "Content\Lists"
if((x%2).Equals(0)) { 
 #line 180 "Content\Lists"
if(xs is ListInt) { 
 #line 180 "Content\Lists"
var tmp_3 = removeOdd.Create(xs as ListInt);
 #line 180 "Content\Lists"
foreach (var tmp_2 in tmp_3.Run()) { var xs_Prime = tmp_2; 
 #line 180 "Content\Lists"
var result = xs_Prime;
 #line 180 "Content\Lists"
yield return result;  } } } }
 } 

  
 { 
 #line 189 "Content\Lists"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _Semicolon; 
 #line 189 "Content\Lists"
if (tmp_1 != null) { var x = tmp_1.P1; var xs = tmp_1.P2; 
 #line 189 "Content\Lists"
if((x%2).Equals(1)) { 
 #line 189 "Content\Lists"
if(xs is ListInt) { 
 #line 189 "Content\Lists"
var tmp_3 = removeOdd.Create(xs as ListInt);
 #line 189 "Content\Lists"
foreach (var tmp_2 in tmp_3.Run()) { var xs_Prime = tmp_2; 
 #line 189 "Content\Lists"
if(xs_Prime is ListInt) { 
 #line 189 "Content\Lists"
var result = _Semicolon.Create(x, xs_Prime as ListInt);
 #line 189 "Content\Lists"
yield return result;  } } } } }
 } 

  }


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

  public IEnumerable<IRunnable> Run() {   
 { 
 #line 37 "Content\Lists"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as nil; 
 #line 37 "Content\Lists"
if (tmp_1 != null) { 
 #line 37 "Content\Lists"
var result = _Comma.Create(nil.Create(), nil.Create());
 #line 37 "Content\Lists"
yield return result;  }
 } 

  
 { 
 #line 42 "Content\Lists"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _Semicolon; 
 #line 42 "Content\Lists"
if (tmp_1 != null) { var x = tmp_1.P1; var tmp_2 = tmp_1.P2 as nil; 
 #line 42 "Content\Lists"
if (tmp_2 != null) { 
 #line 42 "Content\Lists"
var result = _Comma.Create(_Semicolon.Create(x, nil.Create()), nil.Create());
 #line 42 "Content\Lists"
yield return result;  } }
 } 

  
 { 
 #line 47 "Content\Lists"
var tmp_0 = this; var tmp_1 = tmp_0.P1 as _Semicolon; 
 #line 47 "Content\Lists"
if (tmp_1 != null) { var x = tmp_1.P1; var tmp_2 = tmp_1.P2 as _Semicolon; 
 #line 47 "Content\Lists"
if (tmp_2 != null) { var y = tmp_2.P1; var xs = tmp_2.P2; 
 #line 47 "Content\Lists"
if(xs is ListInt) { 
 #line 47 "Content\Lists"
var tmp_4 = split.Create(xs as ListInt);
 #line 47 "Content\Lists"
foreach (var tmp_3 in tmp_4.Run()) { var tmp_5 = tmp_3 as _Comma; 
 #line 47 "Content\Lists"
if (tmp_5 != null) { var l = tmp_5.P1; var r = tmp_5.P2; 
 #line 47 "Content\Lists"
if(l is ListInt && r is ListInt) { 
 #line 47 "Content\Lists"
var result = _Comma.Create(_Semicolon.Create(x, l as ListInt), _Semicolon.Create(y, r as ListInt));
 #line 47 "Content\Lists"
yield return result;  } } } } } }
 } 

  }


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


public IEnumerable<IRunnable> Run() { foreach (var p in Enumerable.Range(0,0)) yield return null; }

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
 var p = mergeSort.Create(_Semicolon.Create(5, _Semicolon.Create(6, _Semicolon.Create(4, _Semicolon.Create(10, _Semicolon.Create(9, _Semicolon.Create(8, _Semicolon.Create(7, _Semicolon.Create(0, _Semicolon.Create(1, _Semicolon.Create(2, _Semicolon.Create(3, nil.Create()))))))))))));
if(printInput) System.Console.WriteLine(p.ToString());
foreach(var x in p.Run())
yield return x;
}
}

}
